using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Console_App_RudyVip;
using Console_App_RudyVip.ObjectClasses;

namespace DataLayer_RudyVip
{
    class CustomerRepository : ICustomerRepository
    {
        private CarContext context;

        public CustomerRepository(CarContext context)
        {
            this.context = context;
        }
        public void AddCustomer(Customer customer)
        {
            context.CustomerData.Add(customer);
        }

        public List<Customer> FindAllCustomers()
        {
            return context.CustomerData.OrderBy(s => s.ID).ToList();
        }

        public Customer FindCustomer(int ID)
        {
            return context.CustomerData.Find(ID);
        }

        public void RemoveCustomerByID(int ID)
        {
            context.CustomerData.Remove(context.CustomerData.Find(ID));
        }
    }
}
