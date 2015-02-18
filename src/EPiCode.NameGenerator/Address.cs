using System;

namespace EPiCode.NameGenerator
{
    public class Address
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string PostalCode { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string Municipality { get; set; } // A.k.a. "Kommune"
        public string County { get; set; }
    }

    
}
