using Console_App_RudyVip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer_RudyVip
{
    public class ReservationsRepository : IReservationsRepository
    {
        private CarContext context;
        public ReservationsRepository(CarContext context)
        {
            this.context = context;
        }

        public void AddReservation(Reservation reservation)
        {
            context.ReservationData.Add(reservation);
        }

        public List<Reservation> FindAllReservation()
        {
            return context.ReservationData.OrderBy(s => s.ID).ToList();
        }

        public Reservation FindReservation(int ID)
        {
            return context.ReservationData.Find(ID);
        }

        public void RemoveReservationByID(int ID)
        {
            context.ReservationData.Remove(context.ReservationData.Find(ID));
        }
    }
}
