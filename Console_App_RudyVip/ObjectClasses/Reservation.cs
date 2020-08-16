using Console_App_RudyVip.ObjectClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Text;

namespace Console_App_RudyVip
{
    public class Reservation
    {
        public int ID { get; set; }
        public DateTime OrderingDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
        public int OverHours { get; set; }
        public Double OverHoursPriceTotal { get; set; }

        public Double Discount { get; set; }
        public Double ExclusiveBTWPrice { get; set; }
        public Double TotalPrice { get; set; }

        public string OrderAdress { get; set; }
        public String DeliverAdress { get; set; }
        public String Categorie { get; set; }


        public Reservation(DateTime orderingDate, DateTime startDate, DateTime endDate, int overHours, double overHoursPriceTotal, Double discount,Double ExclBtw, Double totalPrice, string orderAdress,string deliverAdress , string categorie)
        {
            this.OrderingDate = orderingDate;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.OverHours = overHours;
            this.OverHoursPriceTotal = overHoursPriceTotal;
            this.TotalPrice = totalPrice;
            this.OrderAdress = orderAdress;
            this.DeliverAdress = deliverAdress;
            this.Discount = discount;
            this.ExclusiveBTWPrice = ExclBtw;
            this.Categorie = categorie;
        }
        public Reservation() { }

    }
}
