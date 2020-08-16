using System;
using System.Collections.Generic;
using System.Text;

namespace Console_App_RudyVip
{
    public interface ICarsRepository
    {
        void AddCar(Car car);
        void RemoveCarByID(int ID);
        Car FindCar(int ID);
        List<Car> FindAllCars();
        List<Car> FindCarDetails(String brand, String model, string color);
        List<Car> FindAllCarsByBrands(String brand);

    }
}
