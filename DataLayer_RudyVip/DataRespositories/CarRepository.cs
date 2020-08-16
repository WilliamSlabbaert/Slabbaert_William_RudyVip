using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Console_App_RudyVip;

namespace DataLayer_RudyVip
{
    class CarRepository : ICarsRepository
    {
        private CarContext context;

        public CarRepository(CarContext context)
        {
            this.context = context;
        }

        public void AddCar(Car car)
        {
            context.CarData.Add(car);
        }

        public List<Car> FindAllCars()
        {
            return context.CarData.OrderBy(s => s.Brand).ToList();
        }

        public List<Car> FindAllCarsByBrands(string brand)
        {
            List<Car> tempCar = new List<Car> { };
            foreach (var item in context.CarData.OrderBy(s => s.Brand).ToList())
            {
                if(item.Brand.ToUpper().Trim().Replace(" ","").Equals(brand.ToUpper().Replace(" ", "")) && item.Available == true)
                    tempCar.Add(item);
            }
            return tempCar;
        }

        public Car FindCar(int ID)
        {
            return context.CarData.Find(ID);
        }
        public List<Car> FindCarDetails(String brand,String model,string color)
        {
            List<Car> carlist = new List<Car> { };
            foreach (var item in context.CarData.OrderBy(s => s.Brand).ToList())
            {
                if (item.Brand == brand && item.Model == model && item.Color == color)
                    carlist.Add(item);
            }
            return carlist;
        }

        public void RemoveCarByID(int ID)
        {
            context.CarData.Remove(context.CarData.Find(ID));
        }
    }
}
