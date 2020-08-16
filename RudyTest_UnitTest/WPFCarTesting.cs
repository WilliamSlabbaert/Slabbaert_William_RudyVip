using Console_App_RudyVip;
using DataLayer_RudyVip;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace RudyTest_UnitTest
{
    [TestClass]
    public class WPFCarTesting
    {
        [TestMethod]
        public void TestCustomerManagerGetCar()
        {
            var Brand = "PORSCHE";

            var Car = new CarManager(new UnitOfWork(new CarContext())).GetCar(6);

            Assert.AreEqual(Car.Brand.ToUpper().Trim().Replace(" ",""), Brand);
        }

        [TestMethod]
        public void TestCustomerManagerGetAll()
        {
            var Count = 20;

            var Car = new CarManager(new UnitOfWork(new CarContext())).GetAllCars();

            Assert.AreEqual(Car.Count, Count);
        }
        [TestMethod]
        public void TestCustomerManagerAddCar()
        {
            var Count = 21;

            new CarManager(new UnitOfWork(new CarContext())).AddCarOne("dwwe","ëdfw","wefwef",3545354,5453,543,3543);
            var Car = new CarManager(new UnitOfWork(new CarContext())).GetAllCars();

            Assert.AreEqual(Car.Count, Count);
        }
    }
}
