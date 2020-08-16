using System;
using System.Collections.Generic;
using System.Text;

namespace Console_App_RudyVip
{
    public interface IReservationCarsRepository
    {
        void AddCustomerCarRepository(ReservationCars CustomerCar);
        void RemoveCustomerCarByID(int ID);
        List<ReservationCars> FindCustomerCar(int ID);
        List<ReservationCars> FindAllCustomerCars();
    }
}
