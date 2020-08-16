using Console_App_RudyVip.ObjectClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Console_App_RudyVip
{
    public interface ICustomerRepository
    {
        void AddCustomer(Customer customer);
        void RemoveCustomerByID(int ID);
        Customer FindCustomer(int ID);
        List<Customer> FindAllCustomers();
    }
}
