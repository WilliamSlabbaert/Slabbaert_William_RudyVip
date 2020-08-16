using System;
using System.Runtime.ConstrainedExecution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console_App_RudyVip;


namespace Console_App_RudyVip
{
    [TestClass]
    public class WPFTests
    {
        [TestMethod]
        public void GetAllCarsTest()
        {
            var m = new CarManager(new UnitOfWork(new CarContext()));
        }
    }
}
