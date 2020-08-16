using Console_App_RudyVip.ObjectClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Console_App_RudyVip
{
    public class CustomerManager 
    {
        private IUnitOfWork uow;

        public CustomerManager(IUnitOfWork uow)
        {
            this.uow = uow;
        }
        public void AddCustomerCompany(string name, string street, string city, string categorie, string phoneNummer, string btwNummer)
        {
            uow.customerRepository.AddCustomer(new Customer(name, street, city, categorie, phoneNummer, btwNummer));
            uow.Complete();
        }
        public void AddCustomer(string name, string street, string city, string categorie, string phoneNummer)
        {
            uow.customerRepository.AddCustomer(new Customer(name, street, city, categorie, phoneNummer));
            uow.Complete();
        }
        public List<Customer> GetAllCustomers()
        {
            return uow.customerRepository.FindAllCustomers();
        }
        public Customer GetCustomer(int ID)
        {
            return uow.customerRepository.FindCustomer(ID);
        }
        public void RemoveCustomer(int ID)
        {
            uow.customerRepository.RemoveCustomerByID(ID);
            uow.Complete();
        }
        public void RemoveAllCustomers(List<int> IDS)
        {
            foreach (var ID in IDS)
            {
                uow.customerRepository.RemoveCustomerByID(ID);
            }
            uow.Complete();
        }
        public void Save()
        {
            uow.Complete();
        }
    }
}
