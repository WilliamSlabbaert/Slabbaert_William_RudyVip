using Console_App_RudyVip;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer_RudyVip
{
    public class UnitOfWork : IUnitOfWork
    {
        private CarContext context;
        public ICarsRepository carsRepository { get; private set; }
        public ICustomerRepository customerRepository { get; private set; }
        public IReservationsRepository reservationsRepository { get; private set; }
        public IReservationCarsRepository reservationCarsRepository { get; private set; }

        public UnitOfWork(CarContext context)
        {
            this.context = context;
            carsRepository = new CarRepository(context);
            customerRepository = new CustomerRepository(context);
            reservationsRepository = new ReservationsRepository(context);
            reservationCarsRepository = new ReservationCarsRepository(context);
        }


        public void Dispose()
        {
            context.Dispose();
        }
        public void Complete()
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                context.SaveChanges();
                transaction.Commit();
            }
        }
       
    }
}
