using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Console_App_RudyVip
{
    public class CarManager
    {
        private IUnitOfWork uow;

        public CarManager(IUnitOfWork uow)
        {
            this.uow = uow;
        }
        public void AddCarOne(string brand, string model, string color, double firstHourPrice, double nightlifePrice, double weddingPrice, double welnessPrice)
        {
            Car x = new Car(brand, model, color, firstHourPrice, nightlifePrice, weddingPrice, welnessPrice);
            x.Available = true;
            uow.carsRepository.AddCar(x);
            uow.Complete();
        }
       
        public List<Car> GetAllCars()
        {
            return uow.carsRepository.FindAllCars();
        }
        public Car GetCar(int id)
        {
            return uow.carsRepository.FindCar(id);
        }
        public List<Car> GetCarsByBrands(string brand)
        {
            return uow.carsRepository.FindAllCarsByBrands(brand);
        }
        
        public HashSet<String> GetBrands()
        {
            HashSet<String> brands = new HashSet<String> { };
            foreach (var item in GetAllCars())
            {
                if(item.Available == true)
                    brands.Add(item.Brand.ToUpper().Trim().Replace(" ",""));
            }
            return brands;
        }
        public void RemoveCar(int id)
        {
            uow.carsRepository.RemoveCarByID(id);
            uow.Complete();
        }
        public void RemoveAllCar(List<int> ids)
        {
            foreach (var item in ids)
            {
                uow.carsRepository.RemoveCarByID(item);
            }
            uow.Complete();
        }
        public void Save()
        {
            uow.Complete();
        }
        public DataTable LoadGrid()
        {
            
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("ID", typeof(int)));
            dt.Columns.Add(new DataColumn("Brand", typeof(String)));
            dt.Columns.Add(new DataColumn("Model", typeof(String)));
            dt.Columns.Add(new DataColumn("Color", typeof(String)));
            dt.Columns.Add(new DataColumn("FirstPrice in €", typeof(Double)));
            dt.Columns.Add(new DataColumn("NightLifePrice in €", typeof(Double)));
            dt.Columns.Add(new DataColumn("WeddingPrice in €", typeof(Double)));
            dt.Columns.Add(new DataColumn("WellnessPrice in €", typeof(Double)));
            dt.Columns.Add(new DataColumn("available", typeof(Boolean)));

            foreach (var item in uow.carsRepository.FindAllCars().OrderBy(s => s.ID))
            {
                DataRow nr = dt.NewRow();
                nr[0] = item.ID;
                nr[1] = item.Brand;
                nr[2] = item.Model;
                nr[3] = item.Color;
                nr[4] = item.FirstHourPrice;
                nr[5] = item.NightlifePrice;
                nr[6] = item.WeddingPrice;
                nr[7] = item.WellnessPrice;
                nr[8] = item.Available;
                dt.Rows.Add(nr);
            }
            return dt;
        }
    }
}
