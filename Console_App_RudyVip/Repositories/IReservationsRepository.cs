using System;
using System.Collections.Generic;
using System.Text;

namespace Console_App_RudyVip
{
    public interface IReservationsRepository
    {
        void AddReservation(Reservation reservation);
        void RemoveReservationByID(int ID);
        Reservation FindReservation(int ID);
        List<Reservation> FindAllReservation();
    }
}
