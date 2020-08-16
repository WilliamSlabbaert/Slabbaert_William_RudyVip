using Console_App_RudyVip.ObjectClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Console_App_RudyVip.Domain
{
    public class ReservationCarsManager
    {
        private IUnitOfWork uow;
        public ReservationCarsManager(IUnitOfWork uow)
        {
            this.uow = uow;
        }
        public void AddReservationCars(int res, int car, int cus)
        {
            uow.reservationCarsRepository.AddCustomerCarRepository(new ReservationCars(uow.reservationsRepository.FindReservation(res),uow.carsRepository.FindCar(car),uow.customerRepository.FindCustomer(cus)));
            uow.Complete();
        }
        public List<ReservationCars> GetAllReservationCars()
        {
            return uow.reservationCarsRepository.FindAllCustomerCars();
        }
        public List<ReservationCars> GetReservationCars(int ID)
        {
            return uow.reservationCarsRepository.FindCustomerCar(ID);
        }
        public void RemoveReservationCars(int ID)
        {
            uow.reservationCarsRepository.RemoveCustomerCarByID(ID);
            uow.Complete();
        }
        public void RemoveAllReservationCars(List<int> IDS)
        {
            foreach (var item in IDS)
            {
                uow.reservationCarsRepository.RemoveCustomerCarByID(item);
            }
            uow.Complete();
        }
        public void Save()
        {
            uow.Complete();
        }
    }
}
