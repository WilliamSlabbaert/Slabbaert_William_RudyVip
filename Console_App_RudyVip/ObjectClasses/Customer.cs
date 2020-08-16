using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Console_App_RudyVip.ObjectClasses
{
    public class Customer
    {
        public int ID { get; set; }
        public String Name { get; set; }
        public String Street { get; set; }
        public String City { get; set; }
        public String Categorie { get; set; }
        public String PhoneNummer { get; set; }
        public String BtwNummer { get; set; }
       

        public Customer(string name, string street, string city, string categorie, string phoneNummer, string btwNummer)
        {
            Name = name;
            Street = street;
            City = city;
            Categorie = categorie;
            PhoneNummer = phoneNummer;
            BtwNummer = btwNummer;
        }

        public Customer(string name, string street, string city, string categorie, string phoneNummer)
        {
            Name = name;
            Street = street;
            City = city;
            Categorie = categorie;
            PhoneNummer = phoneNummer;
        }

        public override string ToString()
        {
            return ID.ToString();
        }
    }
}
