using Console_App_RudyVip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer_RudyVip
{
    public class ReservationCarsRepository : IReservationCarsRepository
    {
        private CarContext context;

        public ReservationCarsRepository(CarContext context)
        {
            this.context = context;
        }

        public void AddCustomerCarRepository(ReservationCars CustomerCar)
        {
            context.ReservationCarsData.Add(CustomerCar);
        }

        public List<ReservationCars> FindAllCustomerCars()
        {
            return context.ReservationCarsData.OrderBy(s => s.reservationID).ToList();
        }

        public List<ReservationCars> FindCustomerCar(int ID)
        {
            List<ReservationCars> temp = new List<ReservationCars>();
            foreach (var item in context.ReservationCarsData.OrderBy(s => s.reservationID).ToList())
                if (item.reservationID.Equals(ID)) { temp.Add(item); }

            return temp;

        }

        public void RemoveCustomerCarByID(int ID)
        {
            foreach (var item in context.ReservationCarsData.OrderBy(s => s.reservationID).ToList())
                if (item.reservationID.Equals(ID)) { context.ReservationCarsData.Remove(item); }
        }
    }
}
