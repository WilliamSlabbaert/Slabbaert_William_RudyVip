using Console_App_RudyVip;
using DataLayer_RudyVip;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace RudyTest_UnitTest
{
    [TestClass]
    public class WPFReservationTest
    {
        [TestMethod]
        public void TestReservationManagerGetAll()
        {
            var Count = 1;

            var Reservations = new ReservationManager(new UnitOfWork(new CarContext())).GetAllReservations();

            Assert.AreEqual(Reservations.Count, Count);
        }
        [TestMethod]
        public void TestReservationManagerGetReservation()
        {
            var Price = 690;

            var Reservation = new ReservationManager(new UnitOfWork(new CarContext())).GetReservation(52);

            Assert.AreEqual(Reservation.TotalPrice, Price);
        }
    }
}
