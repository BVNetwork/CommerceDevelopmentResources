﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;
using EPiCode.Datagenerator.Api.DataObjects;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Catalog.Managers;
using Mediachase.Commerce.Catalog.Objects;
using Mediachase.Commerce.Customers;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Orders.Managers;
using Mediachase.Commerce.Website.Helpers;

namespace EPiCode.Datagenerator.Api.Controllers
{
    public class OrderController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<int> Get()
        {
            PurchaseOrder[] purchaseOrders =
                Mediachase.Commerce.Orders.OrderContext.Current.FindPurchaseOrdersByStatus(OrderStatus.InProgress,
                                                                                           OrderStatus.Completed,
                                                                                           OrderStatus.Cancelled,
                                                                                           OrderStatus.AwaitingExchange,
                                                                                           OrderStatus.OnHold,
                                                                                           OrderStatus.PartiallyShipped);
            foreach (PurchaseOrder order in purchaseOrders)
            {
                yield return order.Id;
            }
        }

        public IEnumerable<Order> GetOrdersForCustomer(Guid id)
        {
            // List<string> lineItems = new

            PurchaseOrder[] orders = Mediachase.Commerce.Orders.OrderContext.Current.GetPurchaseOrders(id);
            foreach (PurchaseOrder purchaseOrder in orders)
            {
                foreach (OrderForm orderForm in purchaseOrder.OrderForms)
                {
                    var order = new Order();
                    order.PopulateFrom(orderForm);
                    yield return order;
                }
            }
        }

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }


        [HttpPost]
        public int CreateOrder(PlaceOrderInfo orderInfo)
        {
            return DoOneClickBuy(orderInfo);
        }


        protected int DoOneClickBuy(PlaceOrderInfo orderInfo)
        {
            if (orderInfo == null) throw new ArgumentNullException("orderInfo");
            if (orderInfo.Products.Count == 0 ) throw new ArgumentNullException("orderInfo.Products");
            
            CartHelper ch = new CartHelper("OneClickBuy", orderInfo.CustomerId);

            foreach (PlaceOrderLineItemInfo lineItemInfo in orderInfo.Products)
            {

                Entry ownerProduct = CatalogContext.Current.GetCatalogEntry(lineItemInfo.ProductEntryCode,
                                                                            new CatalogEntryResponseGroup(
                                                                                CatalogEntryResponseGroup.ResponseGroup
                                                                                                         .CatalogEntryFull));
                if (ownerProduct == null)
                    throw new ArgumentNullException("Cannot load Product with code: " +
                                                    lineItemInfo.ProductEntryCode); 

                Entry sku = CatalogContext.Current.GetCatalogEntry(lineItemInfo.VariationEntryCode,
                                                                   new CatalogEntryResponseGroup(
                                                                       CatalogEntryResponseGroup.ResponseGroup
                                                                                                .CatalogEntryFull));
                if (sku == null)
                    throw new ArgumentNullException("Cannot load Variation with code: " +
                                                    lineItemInfo.VariationEntryCode);

                sku.ParentEntry = ownerProduct;

                ch.AddEntry(sku, lineItemInfo.Count, fixedQuantity: true);
            }

            int orderId = PlaceOneClickOrder(ch, orderInfo);
            return orderId;
        }

        private int PlaceOneClickOrder(CartHelper cartHelper, PlaceOrderInfo orderInfo)
        {
            Cart cart = cartHelper.Cart;

            CustomerContact currentContact = CustomerContext.Current.GetContactById(orderInfo.CustomerId);
            if (currentContact == null)
            {
                throw new NullReferenceException("Cannot load customer with id: " + orderInfo.CustomerId.ToString());
            }

            OrderGroupWorkflowManager.RunWorkflow(cart, OrderGroupWorkflowManager.CartCheckOutWorkflowName);
            cart.CustomerId = orderInfo.CustomerId;
            cart.CustomerName = currentContact.FullName;

            if (currentContact.PreferredBillingAddress != null)
            {
                OrderAddress orderBillingAddress = StoreHelper.ConvertToOrderAddress(currentContact.PreferredBillingAddress);
                int billingAddressId = cart.OrderAddresses.Add(orderBillingAddress);
                cart.OrderForms[0].BillingAddressId = orderBillingAddress.Name;
            }
            if(currentContact.PreferredShippingAddress != null)
            {
                int shippingAddressId = cart.OrderAddresses.Add(StoreHelper.ConvertToOrderAddress(currentContact.PreferredShippingAddress));
            }
            
            if(cart.OrderAddresses.Count == 0)
            {
                CustomerAddress address = currentContact.ContactAddresses.FirstOrDefault();
                if (address != null)
                {
                    int defaultAddressId = cart.OrderAddresses.Add(StoreHelper.ConvertToOrderAddress(address));
                    cart.OrderForms[0].BillingAddressId = address.Name;
                }
            }

            var po = cart.SaveAsPurchaseOrder();

            // These does not persist
            po.OrderForms[0].Created = orderInfo.OrderDate;
            po.OrderForms[0].Modified = orderInfo.OrderDate;
            po.Created = orderInfo.OrderDate;
            po.Modified = orderInfo.OrderDate;

            currentContact.LastOrder = po.Created;
            currentContact.SaveChanges();

            // Add note to purchaseOrder
            OrderNotesManager.AddNoteToPurchaseOrder(po, "", OrderNoteTypes.System, orderInfo.CustomerId);
            // OrderNotesManager.AddNoteToPurchaseOrder(po, "Created: " + po.Created, OrderNoteTypes.System, orderInfo.CustomerId);

            po.AcceptChanges();

            SetOrderDates(po.Id, orderInfo.OrderDate);

            //Add do Find index
            // OrderHelper.CreateOrderToFind(po);

            // Remove old cart
            cart.Delete();
            cart.AcceptChanges();
            return po.Id;
        }

        public void SetOrderDates(int id, DateTime orderDate)
        {
            // 
            string connectionString = OrderContext.MetaDataContext.ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string commandText =
                    "UPDATE OrderGroup_PurchaseOrder SET [Created] = @created, [Modified] = @modified WHERE ObjectId = @id";
                using (SqlCommand command = new SqlCommand(commandText, conn))
                {
                    command.Parameters.Add("id", SqlDbType.Int).Value = id;
                    command.Parameters.Add("created", SqlDbType.DateTime).Value = orderDate;
                    command.Parameters.Add("modified", SqlDbType.DateTime).Value = orderDate;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}