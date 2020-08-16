using Console_App_RudyVip;
using Console_App_RudyVip.ObjectClasses;
using DataLayer_RudyVip;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RudyTest_UnitTest
{
    [TestClass]
    public class WPFCustomerTesting
    {
        [TestMethod]
        public void TestCustomerManagerGetCutomer()
        {
            var categorie = "VIP";

            var Customer = new CustomerManager(new UnitOfWork(new CarContext())).GetCustomer(23);

            Assert.AreEqual(Customer.Categorie, categorie);
        }
        [TestMethod]
        public void TestCustomerManagerDeleteCustomer()
        {

            var Count = 23;
            new CustomerManager(new UnitOfWork(new CarContext())).RemoveCustomer(27);
            new CustomerManager(new UnitOfWork(new CarContext())).Save();
            var Customers = new CustomerManager(new UnitOfWork(new CarContext())).GetAllCustomers();

            Assert.AreEqual(Customers.Count, Count);
        }

        [TestMethod]
        public void TestCustomerManagerGetAll()
        {
            var Count = 23;

            var Customers = new CustomerManager(new UnitOfWork(new CarContext())).GetAllCustomers();

            Assert.AreEqual(Customers.Count, Count);
        }
        [TestMethod]
        public void TestCustomerManagerAddCustomer()
        {
            
            var Count = 24;

            new CustomerManager(new UnitOfWork(new CarContext())).AddCustomer("William", "Poststraat 59", "VIP", "04654165", "qeqweqw");
            var Customers = new CustomerManager(new UnitOfWork(new CarContext())).GetAllCustomers();

            Assert.AreEqual(Customers.Count, Count);
        }
       
    }
}
