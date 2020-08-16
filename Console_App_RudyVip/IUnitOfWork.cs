using System;
using System.Collections.Generic;
using System.Text;

namespace Console_App_RudyVip
{
    public interface IUnitOfWork : IDisposable
    {
        ICarsRepository carsRepository { get; }
        ICustomerRepository customerRepository { get; }
        IReservationsRepository reservationsRepository { get; }
        IReservationCarsRepository reservationCarsRepository { get; }
        void Complete();
    }
}
