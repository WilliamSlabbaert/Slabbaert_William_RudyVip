using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Console_App_RudyVip
{
    public class Car
    {
        public int ID { get; set; }
        public String Brand { get; set; }
        public String Model { get; set; }
        public String Color { get; set; }

        public Double FirstHourPrice { get; set; }
        public Double NightlifePrice { get; set; }
        public Double WeddingPrice { get; set; }
        public Double WellnessPrice { get; set; }
        
        public Boolean Available { get; set; }
        public IList<ReservationCars> ReservationCars { get; set; }

        public Car(string brand, string model, string color, double firstHourPrice, double nightlifePrice, double weddingPrice, double wellness)
        {
            Brand = brand;
            Model = model;
            Color = color;
            FirstHourPrice = firstHourPrice;
            NightlifePrice = nightlifePrice;
            WeddingPrice = weddingPrice;
            WellnessPrice = wellness;
        }
        public Car(int ID ,string brand, string model, string color, double firstHourPrice, double nightlifePrice, double weddingPrice, double wellnessprice)
        {
            this.ID = ID;
            Brand = brand;
            Model = model;
            Color = color;
            FirstHourPrice = firstHourPrice;
            NightlifePrice = nightlifePrice;
            WeddingPrice = weddingPrice;
            WellnessPrice = wellnessprice;
        }
        public Car() { }
        

        public override string ToString()
        {
            return ID.ToString();
        }
    }
}
