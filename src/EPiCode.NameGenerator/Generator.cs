using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace EPiCode.NameGenerator
{
    public class Generator
    {
        public List<Address> GenerateAddresses(int count, string basePath)
        {

            var firstNames = GetAllFirstNames(basePath).ToArray();
            var lastNames = GetAllLastNames(basePath).ToArray();
            var randomFn = new Random(DateTime.Now.Second);
            List<Address> addresses = new List<Address>();

            for (int i = 0; i < count; i++)
            {
                var address = GetRandomAddress(randomFn.Next(), basePath);
                KeyValuePair<string, bool> name = firstNames[randomFn.Next(firstNames.Count())];
                address.FirstName = name.Key;
                address.Gender = name.Value ? "male" : "female";
                address.LastName = lastNames[randomFn.Next(lastNames.Count())];
                address.BirthDate = new DateTime(randomFn.Next(1955, 2000), randomFn.Next(1, 12), randomFn.Next(1, 28));
                address.Email = string.Format("{0}.{1}@{2}.test", address.FirstName, address.BirthDate.Year, address.LastName);
                addresses.Add(address);
            }
            return addresses;
        }


        public static Dictionary<string, bool> GetAllFirstNames(string basePath)
        {
            Dictionary<string, bool> names = new Dictionary<string, bool>();

            using (var reader = new StreamReader(basePath + @"\FirstNames.txt", Encoding.UTF8))
            {
                string line;
                var ci = new CultureInfo("nb-NO");
                int i = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    if (names.ContainsKey(line) == false)
                    {
                        names.Add(line, true);
                    }
                }
            }

            using (var reader = new StreamReader(basePath + @"\FirstNamesFemale.txt", Encoding.UTF8))
            {
                string line;
                var ci = new CultureInfo("nb-NO");
                int i = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    if (names.ContainsKey(line) == false)
                    {
                        names.Add(line, false);
                    }
                }
            }
            return names;
        }

        public static IEnumerable<string> GetAllLastNames(string basePath)
        {
            using (var reader = new StreamReader(basePath + @"\LastNames.txt", Encoding.UTF8))
            {
                string line;
                var ci = new CultureInfo("nb-NO");
                int i = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            };

        }


        public static string[] GetAllStreetAddresses()
        {
            return
                "Admiral, aften, akt, al, altan, ammalgam, anorakk, aroma, arr, asjett, asparges, aspik, asyl, atlas, atom, avl, avløp, åk, åker, ånd, ånde, åpning, åre, balje, balkong, ball, ballett, balsam, bamse, banan, banjo, bark, barm, barsel, bart, basseng, bast, bataljon, batteri, baug, bavian, båt, bær, beger, bekken, belg, betennelse, bibel, bidé, bihule, bind, binne, bissel, bjelke, bjelle, blære, blei, bleie, blekk, blemme, blest, blink, blod, blødme, bluse, blyant, boble, bog, boms, bonus, bor, borg, bøddel, bølge, bøtte, bøye, bram, brask, bråte, brem, bresj, brett, brille, brissel, brodd, brokk, brunst, brus, brusk, bryl, bryter, buk, bukk, bukt, bul, bunn, burka, buse, byll, bysse, daddel, dam, dåre, dåse, degge, deig, dekken, diadem, dill, dilla, diplom, diplomat, disippel, divan, dobb, dolp, dont, dor, dorg, dørk, drage, drakt, drenasje, dreng, dresin, dressing, drops, drøvel, drue, drunse, dryss, due, duft, dugg, dunst, dur, dus, dusj, dyne, dynge, dyse, egg, eir, eksem, emalje, emblem, entré, eske, etappe, etui, farse, farsott, faster, fatle, fe, feber, fedme, fele, felg, ferge, fett, fiken, filipens, fille, filt, finger, finne, fjes, fjon, fjøl, flak, flanell, flapp, flåte, flekk, flenge, flens, flesk, flod, flor, flørt, fløte, flue, flukt, flygel, fold, fonn, fontene, forelegg, forheng, foster, fot, fødsel, frityr, fromasj, frø, frøken, frue, furu, futteral, galle, gamasje, gamp, gane, garasje, garnityr, gasje, gebiss, gelender, gelé, gemytt, genser, gesims, geskjeft, gikt, gips, gissel, gjedde, gjørme, glans, glasur, glis, glitter, glor, glorie, gluten, grabb, granat, graps, grav, grevling, grind, gro, grop, grotte, grøft, grønske, grøss, grøt, grums, grut, gryn, gryte, gubbe, gulasj, gulp, gunst, gusj, gylf, gymnastikk, hake, hale, ham, hanske, harpe, harpiks, hassel, havn, hår, hæl, hekk, hems, herpes, hette, hinne, hjerne, hjul, hode, hoff, holk, host, hud, humle, hump, hvelv, hvete, hylle, hylse, hyse, igle, iglo, innside, isjas, jakke, jakt, jolle, jord, jorde, jøde, jur, jus, juv, kabal, kabel, kahytt, kajakk, kakkel, kakkerlakk, kalas, kalkyle, kalosj, kalott, kalv, kammer, kanal, kanne, kanon, kant, kaos, kapers, kapsel, kar, karaffel, karamell, karm, kart, kartong, kasserolle, katamaran, kateter, katt, kaviar, kål, kefir, kitt, kjegle, kjekkas, kjelke, kjepp, kjerne, kjertel, kjetting, kjole, kjortel, kjos, kjøkken, kjøl, kjønn, kladd, klang, klase, klatt, kli, klimaks, klink, kliss, klogg, klor, klov, klovn, kløe, kløne, klump, klut, klype, klyse, knagg, knaus, kne, knekt, knep, knokkel, knopp, knute, knyst, kobbel, koffert, komma, kompott, kongle, konstabel, kopp, kork, kors, korsett, kos, kost, krage, krås, kreft, krem, krill, krim, kringle, krom, krone, krukke, krutong, krydder, kube, kull, kultur, kum, kumle, kumpan, kupp, kuppel, kurtasje, kurv, kurve, kusine, kusk, kusma, kuvøse, kvalme, kveg, kveil, kvige, kvise, kyse, kyss, lake, lakei, laken, lakk, lam, lampe, lampett, lanse, lanterne, lapp, lapskaus, lår, ledning, lefse, legeme, legg, lekkasje, lekter, lektor, lem, leppe, lerke, lerret, lesk, lest, liane, liga, linjal, lisse, list, lo, lomme, los, løve, lugar, lugg, lukt, lunge, lunte, lupe, lur, lykt, lymfe, magi, magma, mais, majones, manesj, mappe, marg, marinade, marmelade, maske, massasje, masse, mast, matiné, matros, mæle, megge, melk, membran, midd, milt, molo, moms, monokkel, monter, morfin, mormon, mort, mos, moster, møne, mørtel, mudder, mule, munk, muskedunder, muslim, mynt, navle, nebb, negl, nek, nektar, nepe, nerve, nese, nestle, neve, nips, nisje, nisse, niste, nomade, nøkkel, nøtt, nupp, nylon, nymf, nyp, nype, nyre, nys, nytelse, oberst, omhu, omkrets, onkel, opium, organ, orgel, orm, ovn, ødem, økt, øre, østrogen, pai, palass, palme, panne, pant, paprika, paralyse, pasta, paté, patt, påle, pedal, pelikan, pels, pendel, penn, pennal, pensel, perle, perm, perrong, pest, pike, piknik, pil, pilk, pilot, pinne, pir, pirat, plagg, plakk, planet, planke, plaster, plate, pledd, plett, plog, plomme, pløs, plugg, plysj, pokal, pomade, pomp, pore, port, portemoné, pose, post, postei, potet, potte, pøbel, pølse, prakt, prest, prim, prolaps, propp, pryd, pudder, pudding, pukkel, pult, pulver, puma, pumpe, purke, puss, pute, pynt, rabarbra, radar, ramme, rand, ransel, ras, ratt, ravn, råne, regatta, reir, reke, rektor, rem, remulade, revy, ridder, rim, ring, rive, rogn, rombe, rosin, rot, rømme, rønne, rør, røre, røver, rug, ruke, rulett, rus, sabel, saft, sal, salme, salt, salve, same, sarkofag, sauna, saus, savanne, såpe, seddel, sekk, seminar, sennep, sepe, serum, serviett, sete, seter, sevje, sigd, sikkel, sil, silo, siv, sjakt, sjark, sjarm, sjy, skabb, skaft, skalk, skalle, skammel, skandale, skål, skinn, skipper, skje, skjerm, sklerose, sklie, skodde, skonnert, skorpe, skorstein, skøy, skralle, skrål, skrin, skritt, skrog, skrott, skum, skurk, skute, skvett, skvulp, slam, slange, slarv, slegge, slim, slintre, slott, slør, sløyd, slurk, sluse, smak, smekk, sminke, smitte, smør, smøring, smørje, smug, smule, smult, smuss, snabb, snabel, snerk, snert, snile, snor, snorkel, snøre, snørr, snue, snus, sokk, sokkel, sorbet, sødme, søle, sørpe, søyle, spade, spake, spann, sparkel, speil, spekk, spelt, spenne, spiker, spinat, spindel, spion, spiral, spire, sport, sprekk, sprøyt, spytt, stabbur, stake, stamme, stamp, stang, stappe, stativ, staude, staur, stav, stift, stim, sting, stjert, stokk, stol, stolpe, støpsel, støvel, stråle, streptokokk, strikk, strømpe, strøssel, struma, struts, stuing, sukett, sule, suppe, sus, svie, svinn, sviske, svor, svulst, sydvest, sykdom, syklus, syl, sylte, taffel, talg, tallerken, talong, tang, tann, tante, tapet, tapir, tapp, tarm, taske, tast, tater, tavle, tå, tår, te, teater, teft, tentakkel, teppe, terreng, terskel, tine, tinning, tivoli, toddy, tomat, tommel, tøddel, tønne, tøs, tøyte, trakt, traktor, trall, tram, trampett, trampoline, tran, trasé, trau, traume, trav, trål, træl, trekk, trell, tresk, trommel, trompet, trøffel, trubadur, trumf, truse, trut, tryne, tube, tunnel, tupp, turn, tusj, tut, tvilling, tyv, urne, utslett, vagle, valke, vals, vandel, vanilje, vask, væske, velling, ventrikkel, veranda, verk, veske, vev, vigør, vimpel, vindu, vippe, vogn, voks, voll, vom, vorte, vræl, vulkan, ymse, yngel, ynk, og yoghurt."
                    .Split(',');

        }

        public static string GenerateRandomStreetAddress(int i)
        {
            var randoms = GetAllStreetAddresses();

            var random = new Random(i);

            int x = random.Next(10);
            int streetNumber;
            if (x > 3)
            {
                streetNumber = random.Next(50);
            }
            else
            {
                streetNumber = random.Next(200);
            }

            string streetPostfix = "s vei";
            switch (x)
            {
                case 1:
                    streetPostfix = "veien";
                    break;
                case 2:
                    streetPostfix = "gate";
                    break;
                case 3:
                    streetPostfix = "gata";
                    break;
                case 4:
                    streetPostfix = "vei";
                    break;
                case 5:
                    streetPostfix = "svingen";
                    break;
                case 6:
                    streetPostfix = "kroken";
                    break;
                case 7:
                    streetPostfix = "s gate";
                    break;
                case 8:
                    streetPostfix = "sletta";
                    break;
                case 9:
                    streetPostfix = "veien";
                    break;
                case 10:
                    streetPostfix = "vei";
                    break;


            }

            string streetNamePrefix = UppercaseFirst(randoms[random.Next(randoms.Count())].Trim());

            if (streetNamePrefix.EndsWith("s") && streetPostfix.StartsWith("s"))
                streetPostfix = streetPostfix.Remove(0, 1);

            return streetNamePrefix + streetPostfix + " " + streetNumber;



        }

        static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        public static List<Fylke> GetAllFylks(string basePath) // ;-)
        {
            using (var reader = new StreamReader(basePath + "\\Fylke.txt", Encoding.UTF8))
            {
                string line;
                var ci = new CultureInfo("nb-NO");
                int i = 0;
                List<Fylke> fylks = new List<Fylke>();
                while ((line = reader.ReadLine()) != null)
                {
                    try
                    {
                        string[] data = line.Split(';');

                        var fylke = new Fylke()
                         {
                             Id = Convert.ToInt32(data[0]),
                             Lat = data[8],
                             Long = data[9],
                             Name = data[1],
                             Population = Convert.ToInt32(data[7])
                         };
                        fylks.Add(fylke);
                    }
                    catch (Exception ex)
                    {
                        // This will _never_ happen. But if it does:
                        throw ex;
                    }
                }
                return fylks;
            };


        }

        public static List<Kommune> GetAllKommuns(string basePath) // ;-)
        {
            using (var reader = new StreamReader(basePath + "\\Kommune.txt", Encoding.UTF8))
            {
                string line;
                var ci = new CultureInfo("nb-NO");
                int i = 0;
                var kommuns = new List<Kommune>();
                while ((line = reader.ReadLine()) != null)
                {
                    try
                    {
                        string[] data = line.Split(';');

                        var kommune = new Kommune()
                        {
                            Id = Convert.ToInt32(data[0]),
                            Name = data[2],
                            FylkeId = Convert.ToInt32(data[3]),
                            FylkeName = data[4]

                        };
                        kommuns.Add(kommune);
                    }
                    catch (Exception ex)
                    {
                        // This will _never_ happen. But if it does:
                        throw ex;
                    }
                }
                return kommuns;
            };


        }

        public static List<Post> GetAllPost(string basePath) // ;-)
        {
            using (var reader = new StreamReader(basePath + "\\Post.txt", Encoding.UTF8))
            {
                string line;
                var ci = new CultureInfo("nb-NO");
                int i = 0;
                var posts = new List<Post>();
                while ((line = reader.ReadLine()) != null)
                {
                    try
                    {
                        string[] data = line.Split(';');

                        var post = new Post()
                        {
                            PostalNumber = data[0],
                            City = data[1],
                            KommuneId = Convert.ToInt32(data[5]),
                            KommuneName = data[6],
                            Lat = data[3],
                            Long = data[4]
                        };
                        posts.Add(post);
                    }
                    catch (Exception ex)
                    {
                        // This will _never_ happen. But if it does:
                        throw ex;
                    }
                }
                return posts;
            };


        }

        public static Address GetRandomAddress(int r, string basePath)
        {

            var random = new Random(r);

            var posts = GetAllPost(basePath);

            var post = posts[random.Next(posts.Count)];

            var kommuns = GetAllKommuns(basePath).Where(x => x.Id.Equals(post.KommuneId)).ToArray();

            Kommune kommune = kommuns[random.Next(kommuns.Length)];

            var fylke = GetAllFylks(basePath).FirstOrDefault(x => x.Id.Equals(kommune.FylkeId));

            return new Address()
            {
                City = post.City,
                County = fylke.Name,
                Municipality = kommune.Name,
                PostalCode = post.PostalNumber,
                StreetAddress = GenerateRandomStreetAddress(random.Next(r))

            };
        }
    }
}


