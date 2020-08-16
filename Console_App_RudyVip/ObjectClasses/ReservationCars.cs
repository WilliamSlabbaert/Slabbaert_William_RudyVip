using Console_App_RudyVip.ObjectClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Console_App_RudyVip
{
    public class ReservationCars
    {
        [ForeignKey("reservation")]
        public int reservationID { get; set; }
        public Reservation reservation { get; set; }

        [ForeignKey("car")]
        public int carID { get; set; }
        public Car car { get; set; }

        [ForeignKey("customer")]
        public int customerID { get; set; }
        public Customer customer { get; set; }

        public ReservationCars(Reservation reservation, Car car,Customer customer)
        {
            this.reservationID = reservation.ID;
            this.reservation = reservation;
            this.carID = car.ID;
            this.car = car;
            this.customerID = customer.ID;
            this.customer = customer;
        }
        public ReservationCars() { }
    }
}
