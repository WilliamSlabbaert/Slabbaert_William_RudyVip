using Console_App_RudyVip.Domain;
using Console_App_RudyVip.ObjectClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Console_App_RudyVip
{
    public class ReservationManager
    {
        private IUnitOfWork uow;
        public ReservationManager(IUnitOfWork uow)
        {
            this.uow = uow;
        }
        public void AddReservation(Reservation res)
        {
            uow.reservationsRepository.AddReservation(res);
            uow.Complete();
        }
        public List<Reservation> GetAllReservations()
        {
            return uow.reservationsRepository.FindAllReservation();
        }
        public Reservation GetReservation(int ID)
        {
            return uow.reservationsRepository.FindReservation(ID);
        }
        public void RemoveReservation(int ID)
        {
            uow.reservationsRepository.RemoveReservationByID(ID);
            uow.Complete();
        }
        public void RemoveAllReservation(List<int> IDS)
        {
            foreach (var item in IDS)
                uow.reservationsRepository.RemoveReservationByID(item);
            uow.Complete();
        }
        public void Save()
        {
            uow.Complete();
        }
        public int CheckDate(DateTime StartChooseDate, DateTime EndChooseDate, int CarID)
        {
            int True = 0;
            List<int> tempResList = new List<int>();
            foreach (var item in uow.reservationsRepository.FindAllReservation())
            {
                DateTime tempDate = item.StartDate;
                DateTime EndtempDate = item.EndDate;

                if (tempDate > EndChooseDate)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        EndChooseDate = EndChooseDate.AddHours(1);
                        if (EndChooseDate.Equals(tempDate))
                            tempResList.Add(item.ID);
                    }
                }
                else if (EndtempDate < StartChooseDate)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        EndtempDate = EndtempDate.AddHours(1);
                        if (EndtempDate.Equals(StartChooseDate))
                            tempResList.Add(item.ID);
                    }
                }
                else if (tempDate == StartChooseDate)
                    tempResList.Add(item.ID);
            }
            foreach (var item in tempResList)
            {
                var list = uow.reservationCarsRepository.FindAllCustomerCars().FindAll(c => c.carID.Equals(CarID));
                if (list.Count != 0)
                    True = 1;
            }

            return True;
        }
        public Double LoadOverhourPrice(int CarId, int ResID, int hours, DateTime End)
        {
            var tempRes = uow.reservationsRepository.FindReservation(ResID);
            int OverHours = hours;
            TimeSpan tempEnd = TimeSpan.Parse(End.ToString("H:mm"));
            TimeSpan TemEndLimit = tempEnd + TimeSpan.FromHours(hours);

            int nightHours = 0;
            int normalHours = 0;

            while (tempEnd < TemEndLimit)
            {
                tempEnd = tempEnd.Add(new TimeSpan(1, 0, 0));

                if (tempEnd >= new TimeSpan(22, 0, 0) || tempEnd <= new TimeSpan(6, 0, 0))
                    nightHours = nightHours + 1;
                else
                    normalHours = normalHours + 1;
            }
            Double OverHourPrice = 0;
            OverHourPrice = normalHours * (uow.carsRepository.FindCar(CarId).FirstHourPrice * 0.65);
            OverHourPrice = OverHourPrice + (nightHours * (uow.carsRepository.FindCar(CarId).FirstHourPrice * 1.4));

            tempRes.OverHours += OverHours;
            tempRes.OverHoursPriceTotal = OverHourPrice;
            tempRes.ExclusiveBTWPrice += OverHourPrice;
            tempRes.TotalPrice = Math.Round(((OverHourPrice + tempRes.ExclusiveBTWPrice) * 0.06) + (OverHourPrice + tempRes.ExclusiveBTWPrice), 2);

            return OverHourPrice;
        }
        public Double LoadPriceMethod(TimeSpan endTime, TimeSpan startTime, Car carTemp, String Type)
        {
            Double price = 0;
            int nightHours = 0;
            int normalHours = 0;

            if (endTime < startTime)
                endTime = endTime.Add(new TimeSpan(1, 0, 0, 0));

            while (startTime < endTime)
            {
                startTime = startTime.Add(new TimeSpan(1, 0, 0));

                if (startTime >= new TimeSpan(22, 0, 0) || startTime <= new TimeSpan(6, 0, 0))
                    nightHours = nightHours + 1;
                else
                    normalHours = normalHours + 1;
            }

            if (Type == "WEDDING")
                price = carTemp.WeddingPrice + (nightHours * (carTemp.FirstHourPrice * 1.4));
            else if (Type == "NIGHTLIFE")
            {
                if (normalHours != 0)
                    price = carTemp.NightlifePrice + (nightHours * (carTemp.FirstHourPrice * 1.4));
                else
                    price = nightHours * (carTemp.FirstHourPrice * 1.4);
            }
            else if (Type == "WELLNESS")
                price = carTemp.WeddingPrice + (nightHours * (carTemp.FirstHourPrice * 1.4));
            else if (Type == "AIRPORT")
            {
                price = ((normalHours - 1) * (carTemp.FirstHourPrice * 0.65)) + carTemp.FirstHourPrice;
                price = price + (nightHours * (carTemp.FirstHourPrice * 1.4));
            }
            else if (Type == "BUSINESS")
            {
                price = ((normalHours - 1) * (carTemp.FirstHourPrice * 0.65)) + carTemp.FirstHourPrice;
                price = price + (nightHours * (carTemp.FirstHourPrice * 1.4));
            }
            return price;
        }
        public DataTable loadingCars(List<String> tempCarList)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("ID", typeof(int)));
            dt.Columns.Add(new DataColumn("Brand", typeof(String)));
            dt.Columns.Add(new DataColumn("Model", typeof(String)));
            dt.Columns.Add(new DataColumn("Color", typeof(String)));
            dt.Columns.Add(new DataColumn("Price", typeof(String)));

            foreach (var item in tempCarList)
            {
                DataRow nr = dt.NewRow();
                String[] temp = item.Split("|");
                nr[0] = Int32.Parse(temp[0]);
                nr[1] = temp[1];
                nr[2] = temp[2];
                nr[3] = temp[3];
                nr[4] = temp[4];

                dt.Rows.Add(nr);
            }
            return dt;
        }
        public Double GenerateDiscount(Double excl, int cs, int Count, int year)
        {
            Double Discount = 0;
            var tempCS = uow.customerRepository.FindCustomer(cs);

            List<int> resList = uow.reservationsRepository.FindAllReservation().FindAll(c => c.StartDate.Year == year).Select(s => s.ID).ToList();

            HashSet<int> resCars = new HashSet<int>();

            foreach (var item in uow.reservationCarsRepository.FindAllCustomerCars().FindAll(c => c.customerID.Equals(cs)).ToList())
                if (resList.Contains(item.reservationID))
                    resCars.Add(item.reservationID);
            

            if (tempCS.Categorie.ToUpper().Trim().Equals("VIP"))
            {
                if (resCars.Count >= 2)
                    Discount = excl * 0.05;
                else if (resCars.Count >= 7)
                    Discount = excl * 0.075;

                if (Count >= 2)
                    Discount += excl * 0.05;
                else if (Count >= 7)
                    Discount += excl * 0.075;
                else if (Count >= 15)
                    Discount += excl * 0.1;
            }
            else if (tempCS.Categorie.ToUpper().Equals("MARRIAGE PLANNER"))
            {
                if (Count >= 5)
                    Discount += excl * 0.075;
                else if (Count >= 10)
                    Discount += excl * 0.1;
                else if (Count >= 15)
                    Discount += excl * 0.125;
                else if (Count >= 20)
                    Discount += excl * 0.15;
                else if (Count >= 25)
                    Discount += excl * 0.25;
            }
            return Discount;
        }
        public void UpdateReservation(Reservation TempReservation, String StreetInput, String CityInput, DateTime StartChooseDate, DateTime EndChooseDate, Double InclPrice, Double ExclPrice, Double Discount, String DeliverInput, String cmbSelect, Customer cs)
        {
            TempReservation.OrderAdress = StreetInput + " | " + CityInput;
            TempReservation.StartDate = StartChooseDate;
            TempReservation.EndDate = EndChooseDate;
            TempReservation.TotalPrice = InclPrice;
            TempReservation.ExclusiveBTWPrice = ExclPrice;
            TempReservation.Discount = Discount;
            TempReservation.DeliverAdress = DeliverInput;
            TempReservation.Categorie = cmbSelect;
            TempReservation.Discount = 0;
            TempReservation.OverHours = 0;
            TempReservation.OverHoursPriceTotal = 0;
            uow.reservationCarsRepository.RemoveCustomerCarByID(TempReservation.ID);

            uow.Complete();
        }
        public DataTable LoadReservation_Reports(List<Reservation> resList, DataTable data)
        {
            try
            {
                DataTable dt = data;
                foreach (var item in resList.GroupBy(x => x.ID).Select(x => x.First()))
                {
                    DataRow nr = dt.NewRow();
                    nr[0] = item.ID;
                    nr[1] = "ID " + uow.reservationCarsRepository.FindCustomerCar(item.ID)[0].customerID;
                    nr[2] = item.OrderingDate;
                    nr[3] = item.StartDate;
                    nr[4] = item.EndDate;
                    nr[5] = item.OrderAdress;
                    nr[6] = item.OverHours;
                    nr[7] = item.OverHoursPriceTotal;
                    nr[8] = Math.Round(item.Discount, 2);
                    nr[9] = Math.Round(item.ExclusiveBTWPrice, 2);
                    nr[10] = Math.Round(item.TotalPrice, 2);
                    nr[11] = item.DeliverAdress;
                    nr[12] = "ID ";
                    nr[13] = item.Categorie;

                    if (uow.reservationCarsRepository.FindCustomerCar(Int32.Parse(item.ID.ToString().Trim())).Count == 0)
                        nr[12] = "NULL";
                    else
                        foreach (var i in uow.reservationCarsRepository.FindCustomerCar(Int32.Parse(item.ID.ToString().Trim())))
                            nr[12] += i.carID + ", ";


                    dt.Rows.Add(nr);
                }
                return dt;
            }
            catch
            {
                return data;
            }

        }
        public DataTable Load_Columns()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("ID", typeof(int)));
            dt.Columns.Add(new DataColumn("Customer", typeof(String)));
            dt.Columns.Add(new DataColumn("OrderDate", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("Start", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("End", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("Adress", typeof(String)));
            dt.Columns.Add(new DataColumn("OverHours", typeof(int)));
            dt.Columns.Add(new DataColumn("OverHoursPrice", typeof(Double)));
            dt.Columns.Add(new DataColumn("Discount", typeof(Double)));
            dt.Columns.Add(new DataColumn("ExclusiveVAT", typeof(Double)));
            dt.Columns.Add(new DataColumn("TotalPrice", typeof(Double)));
            dt.Columns.Add(new DataColumn("Deliver Adress", typeof(String)));
            dt.Columns.Add(new DataColumn("Car(s)", typeof(String)));
            dt.Columns.Add(new DataColumn("Type", typeof(String)));

            return dt;
        }
        public DataTable LoadCarGrid(List<String> tempCarList)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn("ID", typeof(int)));
            dt.Columns.Add(new DataColumn("Brand", typeof(String)));
            dt.Columns.Add(new DataColumn("Model", typeof(String)));
            dt.Columns.Add(new DataColumn("Color", typeof(String)));
            dt.Columns.Add(new DataColumn("Price", typeof(String)));

            foreach (var item in tempCarList)
            {
                DataRow nr = dt.NewRow();
                String[] temp = item.Split("|");
                nr[0] = Int32.Parse(temp[0]);
                nr[1] = temp[1];
                nr[2] = temp[2];
                nr[3] = temp[3];
                nr[4] = temp[4];

                dt.Rows.Add(nr);
            }
            return dt;
        }
    }
}
