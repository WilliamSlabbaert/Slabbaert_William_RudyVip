using Console_App_RudyVip;
using Console_App_RudyVip.Domain;
using DataLayer_RudyVip;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPF_RudyVip
{
    /// <summary>
    /// Interaction logic for Reservations.xaml
    /// </summary>
    public partial class Reservations : Window
    {
        public List<String> tempCarList = new List<String> { };
        public Double ExclPrice = 0;
        public Double InclPrice = 0;
        public Reservations()
        {
            InitializeComponent();
        }
        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            StartTime.Items.Clear();
            Endtime.Items.Clear();
            try
            {
                if (cmbSelect.SelectedValue != null)
                {
                    DatePick.Visibility = Visibility.Visible;
                    if (cmbSelect.SelectedIndex.ToString().Equals("0"))
                        StartHourAdding(20, 24);
                    else if (cmbSelect.SelectedIndex.ToString().Equals("1"))
                        StartHourAdding(7, 12);
                    else if (cmbSelect.SelectedIndex.ToString().Equals("2"))
                        StartHourAdding(7, 15);
                    else if (cmbSelect.SelectedIndex.ToString().Equals("3") || cmbSelect.SelectedIndex.ToString().Equals("4"))
                        StartHourAdding(0, 23);
                }
            }
            catch
            {
                MessageBox.Show("Choose a Type");
            }
        }
        private void UpdateTime_Dropdown(object sender, EventArgs e)
        {
            Endtime.Visibility = Visibility.Visible;
            try
            {
                Endtime.Items.Clear();
                if (StartTime.SelectedValue != null && cmbSelect.SelectedValue != null && StartTime.SelectedValue.ToString() != null && cmbSelect.SelectedValue.ToString() != null)
                {
                    String time = StartTime.SelectedValue.ToString();
                    if (cmbSelect.SelectedIndex.ToString().Equals("0") || cmbSelect.SelectedIndex.ToString().Equals("2"))
                        EndHourAdding(HourCalculate(time.Substring(0, 2)), 7);

                    else if (cmbSelect.SelectedIndex.ToString().Equals("3") || cmbSelect.SelectedIndex.ToString().Equals("4"))
                        EndHourAdding(HourCalculate(time.Substring(0, 2)), 11);

                    else if (cmbSelect.SelectedIndex.ToString().Equals("1"))
                        EndHourAdding(HourCalculate(time.Substring(0, 2)), 10);
                }
            }
            catch
            {
                MessageBox.Show("Give Valid Time");
            }
        }
        private int HourCalculate(String H)
        {
            int hour;
            if (H.Contains(":"))
                hour = Int32.Parse(H.Substring(0, 1));
            else
                hour = Int32.Parse(H.Substring(0, 2));

            return hour;
        }
        private void EndHourAdding(int hour, int extra)
        {
            String temp = "";
            for (int i = hour + 1; i <= hour + extra; i++)
            {
                if (i == 24)
                    Endtime.Items.Add("0:00");
                else
                {
                    if (i >= 24)
                    {
                        temp += i.ToString().Substring(0, 1);
                        Endtime.Items.Add(temp.Length + ":00");
                    }
                    else
                        Endtime.Items.Add(i + ":00");
                }
            }
            Endtime.SelectedIndex = 0;
        }
        private void StartHourAdding(int hour, int extra)
        {
            for (int i = hour; i <= extra; i++)
            {
                if (i == 24)
                    StartTime.Items.Add("0:00");
                else
                    StartTime.Items.Add(i.ToString() + ":00");
            }
            StartTime.SelectedIndex = 0;
        }
        private void dpick_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            StartTime.Visibility = Visibility.Visible;
        }
        private void MinimizeButton(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Minimized;
            }
            else if (this.WindowState == WindowState.Minimized)
            {
                this.WindowState = WindowState.Normal;
            }
        }
        private void ExitButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
       
        private void Add_Reservation(object sender, RoutedEventArgs e)
        {
            ReservationManager h = new ReservationManager(new UnitOfWork(new CarContext()));
            CustomerManager c = new CustomerManager(new UnitOfWork(new CarContext()));
            ReservationCarsManager Rcm = new ReservationCarsManager(new UnitOfWork(new CarContext()));
            if (AddBtn.Content.ToString().ToUpper().Contains("ADD RESERVATION"))
            {
                if (IDInpt.SelectedItem != null &&  cmbSelect.SelectedItem != null &&StartTime.SelectedItem != null && Endtime.SelectedItem != null && BrndInput.SelectedItem != null &&ColorInput.SelectedItem != null &&!string.IsNullOrWhiteSpace(StreetInput.Text) &&!string.IsNullOrWhiteSpace(CityInput.Text) &&DeliverInput.SelectedItem != null && tempCarList.Count > 0)
                {
                    DateTime StartChooseDate = DatePick.SelectedDate.Value.Add(TimeSpan.Parse(StartTime.SelectedItem.ToString()));
                    DateTime EndChooseDate = DatePick.SelectedDate.Value.Add(TimeSpan.Parse(Endtime.SelectedItem.ToString()));

                    if (StartChooseDate < DateTime.Now)
                        MessageBox.Show("Cannot book earlier than current time");

                    else
                    {
                        if (TimeSpan.Parse(EndChooseDate.ToString("H:mm")) < TimeSpan.Parse(StartChooseDate.ToString("H:mm")))
                            EndChooseDate = EndChooseDate.AddDays(1);
                        int True = 0;

                        foreach (var item in tempCarList)
                           True = h.CheckDate(StartChooseDate, EndChooseDate, Int32.Parse(item.Split("|")[0]));

                        if (True.Equals(0))
                        {
                            foreach (var item in CarGrid.Items)
                            {
                                DataRowView Row = (DataRowView)item;
                                ExclPrice += Double.Parse(Row[4].ToString().Trim());
                            }
                            ExclPrice = Math.Round(ExclPrice, 2);
                            var cs = c.GetCustomer(Int32.Parse(IDInpt.SelectedItem.ToString().Trim().Split("_")[0]));
                            cs.ID = Int32.Parse(IDInpt.SelectedItem.ToString().Trim().Split("_")[0]);
                            Double Discount = Math.Round(h.GenerateDiscount(ExclPrice, cs.ID, CarGrid.Items.Count), 2);

                            InclPrice = ExclPrice - Discount;
                            InclPrice = InclPrice + (InclPrice * 0.06);
                            InclPrice = (int)Math.Ceiling(InclPrice / 5) * 5;

                            Reservation res = new Reservation(DateTime.Now, StartChooseDate,EndChooseDate, 0, 0, Discount, ExclPrice, InclPrice, StreetInput.Text.ToUpper() + " | " + CityInput.Text.ToUpper(),DeliverInput.SelectedValue.ToString().ToUpper().Replace("SYSTEM.WINDOWS.CONTROLS.COMBOBOXITEM:", "").Trim(), cmbSelect.SelectedValue.ToString().ToUpper().Replace("SYSTEM.WINDOWS.CONTROLS.COMBOBOXITEM:", "").Trim());
                            h.AddReservation(res);

                            foreach (var item in tempCarList)
                                Rcm.AddReservationCars(res.ID, Int32.Parse(item.Split("|")[0]), cs.ID);

                            LoadReservationData();
                            tempCarList.Clear();
                            clear();
                            
                            MessageBox.Show("Done");
                        }
                        else
                            MessageBox.Show("Unavailable date (6 hours difference with other reservations)");
                    }
                }
                else
                    MessageBox.Show("Give value");
            }
            else if (AddBtn.Content.ToString().ToUpper().Equals("UPDATE RESERVATION"))
            {
                if (IDInpt.SelectedItem != null && cmbSelect.SelectedItem != null && StartTime.SelectedItem != null &&Endtime.SelectedItem != null &&BrndInput.SelectedItem != null &&ColorInput.SelectedItem != null &&!string.IsNullOrWhiteSpace(StreetInput.Text) &&!string.IsNullOrWhiteSpace(CityInput.Text) &&DeliverInput.SelectedItem != null &&tempCarList.Count > 0)
                {
                    DateTime StartChooseDate = DatePick.SelectedDate.Value.Add(TimeSpan.Parse(StartTime.SelectedItem.ToString()));
                    DateTime EndChooseDate = DatePick.SelectedDate.Value.Add(TimeSpan.Parse(Endtime.SelectedItem.ToString()));

                    if (TimeSpan.Parse(EndChooseDate.ToString("H:mm")) < TimeSpan.Parse(StartChooseDate.ToString("H:mm")))
                        EndChooseDate = EndChooseDate.AddDays(1);
                    int True = 0;

                    foreach (var item in tempCarList)
                        True = h.CheckDate(StartChooseDate, EndChooseDate, Int32.Parse(item.Split("|")[0]));

                    if (True.Equals(0))
                    {
                        foreach (var item in CarGrid.Items)
                        {
                            DataRowView Row = (DataRowView)item;
                            ExclPrice += Double.Parse(Row[4].ToString().Trim());
                        }
                        var cs = c.GetCustomer(Int32.Parse(IDInpt.SelectedItem.ToString().Trim().Split("_")[0]));
                        Double Discount = Math.Round(h.GenerateDiscount(ExclPrice, cs.ID, CarGrid.Items.Count), 2);

                        InclPrice = ExclPrice - Discount;
                        InclPrice = InclPrice + (InclPrice * 0.06);
                        InclPrice = (int)Math.Ceiling(InclPrice / 5) * 5;
                        ExclPrice = Math.Round(ExclPrice, 2);

                        var TempReservation = h.GetReservation(Int32.Parse(Reservation_ID_Label.Content.ToString()));
                        h.UpdateReservation(TempReservation, StreetInput.Text.ToUpper(), CityInput.Text.ToUpper(), StartChooseDate, EndChooseDate, InclPrice, ExclPrice, Discount, DeliverInput.SelectedValue.ToString().ToUpper().Replace("SYSTEM.WINDOWS.CONTROLS.COMBOBOXITEM:", "").Trim(), cmbSelect.SelectedValue.ToString().ToUpper().Replace("SYSTEM.WINDOWS.CONTROLS.COMBOBOXITEM:", "").Trim(), c.GetCustomer(Int32.Parse(IDInpt.SelectedItem.ToString().Trim().Split("_")[0])));
                        
                        foreach (var item in tempCarList)
                            Rcm.AddReservationCars(TempReservation.ID, Int32.Parse(item.Split("|")[0]), cs.ID);

                        LoadReservationData();
                        tempCarList.Clear();
                        clear();
                        MessageBox.Show("Done");
                    }
                    else
                        MessageBox.Show("Unavailable date (6 hours difference with other reservations)");
                }
                else
                    MessageBox.Show("Give value");
            }
            else if (AddBtn.Content.ToString().Contains("Overhours"))
            {
                loadoverHour_Price(1);

                h.Save();

                Update_overtimeVisibility(Visibility.Visible);
                addCarBtn.Visibility = Visibility.Visible;
                cmbSelect.Visibility = Visibility.Visible;
                CarIDBox.Visibility = Visibility.Collapsed;

                MessageBox.Show("Done");
                LoadReservationData();
            }
        }
        private void Load_Brands(object sender, RoutedEventArgs e)
        {
            CarManager m = new CarManager(new UnitOfWork(new CarContext()));
            foreach (var item in m.GetBrands())
            {
                BrndInput.Items.Add(item.ToString().ToUpper());
            }
            BrndInput.SelectedIndex = 0;
        }

        private void Select_Models(object sender, EventArgs e)
        {
            CarManager m = new CarManager(new UnitOfWork(new CarContext()));
            ColorInput.Items.Clear();
            foreach (var item in m.GetCarsByBrands(BrndInput.Text.Trim().ToUpper()))
            {
                ColorInput.Items.Add(item.ID + "  ->" + item.Model + " | " + item.Color);
            }
            ColorInput.SelectedIndex = 0;
        }
        private void Load_IDS(object sender, RoutedEventArgs e)
        {
            LoadIdsMethod();
        }
        public void LoadIdsMethod()
        {
            CustomerManager c = new CustomerManager(new UnitOfWork(new CarContext()));
            foreach (var item in c.GetAllCustomers())
            {
                IDInpt.Items.Add(item.ID + " _ " + item.Name + " | " + item.Categorie);
            }
            IDInpt.SelectedIndex = 0;
            LoadBusiness();
        }
        private void LoadBusiness()
        {
            try
            {
                CustomerManager c = new CustomerManager(new UnitOfWork(new CarContext()));
                if (IDInpt.Items.Count != 0)
                {
                    String[] tempSplit = IDInpt.SelectedItem.ToString().Trim().Split("_");
                    var temp = c.GetCustomer(Int32.Parse(tempSplit[0]));
                    if (temp.Categorie.ToString().ToLower().Equals("organisation") ||
                        temp.Categorie.ToString().ToLower().Equals("concert promoter") ||
                        temp.Categorie.ToString().ToLower().Equals("marriage planner") ||
                        temp.Categorie.ToString().ToLower().Equals("event agency"))
                        BusinessCombo.Visibility = Visibility.Visible;

                    if (temp.Categorie.ToString().ToLower().Equals("marriage planner") || temp.Categorie.ToString().ToLower().Equals("vip"))
                        addCarBtn.Visibility = Visibility.Visible;
                    else
                    {
                        BusinessCombo.Visibility = Visibility.Collapsed;
                        addCarBtn.Visibility = Visibility.Visible;
                    }
                }
            }
            catch
            {
                MessageBox.Show("No Customers/No Cars");
            }
        }

        private void Load_Types(object sender, EventArgs e)
        {
            if (CustomerIdLabel.Content.ToString() == "Customer Id")
                LoadBusiness();
        }
        public DataTable dt = new DataTable();
        private void LoadReservationData()
        {
            ReservationManager h = new ReservationManager(new UnitOfWork(new CarContext()));
            dt = h.Load_Columns();
            ReservationGrid.ItemsSource = h.LoadReservation_Reports(h.GetAllReservations(),dt).DefaultView;
            CarGrid.ItemsSource = h.LoadCarGrid(tempCarList).DefaultView;
        }

        private void Load_Reservation(object sender, RoutedEventArgs e)
        {
            LoadReservationData();
        }

        private void Delete_Reservation(object sender, RoutedEventArgs e)
        {
            ReservationManager h = new ReservationManager(new UnitOfWork(new CarContext()));
            if ((DataRowView)ReservationGrid.SelectedItem != null)
            {
                DataRowView dataRowView = (DataRowView)ReservationGrid.SelectedItem;
                ReservationCarsManager Rcm = new ReservationCarsManager(new UnitOfWork(new CarContext()));
                foreach (var item in Rcm.GetReservationCars(Int32.Parse(dataRowView.Row[0].ToString().Trim())))
                    Rcm.RemoveReservationCars(item.reservationID);

                h.RemoveReservation(Int32.Parse(dataRowView.Row[0].ToString().Trim()));
                LoadReservationData();
            }
            else
                MessageBox.Show("Select Value");
        }

        private void Update_Reservation(object sender, RoutedEventArgs e)
        {
            ReservationManager h = new ReservationManager(new UnitOfWork(new CarContext()));
            ReservationCarsManager Rcm = new ReservationCarsManager(new UnitOfWork(new CarContext()));
            if (AddBtn.Content.ToString().StartsWith("A"))
            {
                if ((DataRowView)ReservationGrid.SelectedItem != null)
                {
                    AddBtn.Content = "Update Reservation";
                    DataRowView dataRowView = (DataRowView)ReservationGrid.SelectedItem;
                    for (int i = 0; i < IDInpt.Items.Count; i++)
                    {
                        if (IDInpt.Items[i].ToString().Split("_")[0].Trim().Equals(Rcm.GetReservationCars(h.GetReservation(Int32.Parse(dataRowView.Row[0].ToString().Trim())).ID)[0].customerID.ToString()))
                        {
                            IDInpt.SelectedIndex = i;
                            break;
                        }
                    }
                    Reservation_ID_Label.Content = h.GetReservation(Int32.Parse(dataRowView.Row[0].ToString().Trim())).ID;
                    StreetInput.Text = h.GetReservation(Int32.Parse(dataRowView.Row[0].ToString().Trim())).OrderAdress.Split("|")[0];
                    CityInput.Text = h.GetReservation(Int32.Parse(dataRowView.Row[0].ToString().Trim())).OrderAdress.Split("|")[1].Remove(0, 1);
                    for (int i = 0; i < DeliverInput.Items.Count; i++)
                    {
                        if (DeliverInput.Items[i].ToString().ToUpper().Replace("SYSTEM.WINDOWS.CONTROLS.COMBOBOXITEM:", "").Trim().Equals(h.GetReservation(Int32.Parse(dataRowView.Row[0].ToString().Trim())).DeliverAdress.ToString().ToUpper().Trim()))
                            DeliverInput.SelectedIndex = i;
                    }
                    LoadBusiness();
                }
                else
                    MessageBox.Show("Select Value");
            }
            else
            {
                clear();
                AddBtn.Content = "Adds Reservation";
            }
        }
        public void clear()
        {
            IDInpt.SelectedIndex = 0;
            BrndInput.SelectedIndex = 0;
            StreetInput.Text = "";
            CityInput.Text = "";
            DeliverInput.SelectedIndex = 0;
            ExclPrice = 0;
            tempCarList.Clear();
            loadingCars();
            AddBtn.Content = "Add Reservation";
            LoadBusiness();
            exclTaxPrice.Content = "€ 0.00";
            Btw_Price.Content = "€ 0.00";
            TotalBtw_Price.Content = "€ 0.00";
            Discount_Price.Content = "€ 0.00";
        }
        private void LoadPriceMethod()
        {
            if (IDInpt.SelectedItem != null && cmbSelect.SelectedItem != null &&StartTime.SelectedItem != null &&Endtime.SelectedItem != null && BrndInput.SelectedItem != null &&ColorInput.SelectedItem != null)
            {
                CarManager m = new CarManager(new UnitOfWork(new CarContext()));
                ReservationManager h = new ReservationManager(new UnitOfWork(new CarContext()));

                TimeSpan startTime = TimeSpan.Parse(StartTime.SelectedItem.ToString());
                TimeSpan endTime = TimeSpan.Parse(Endtime.SelectedItem.ToString());

                Car carTemp = m.GetCar(Int32.Parse(ColorInput.SelectedItem.ToString().ToUpper().Split(" ->")[0].Trim()));
                String Type = cmbSelect.SelectedItem.ToString().ToUpper().Replace("SYSTEM.WINDOWS.CONTROLS.COMBOBOXITEM:", "").Trim();

                ExclPrice = h.LoadPriceMethod(endTime, startTime,carTemp,Type);
            }
            else
                exclTaxPrice.Content = "€ 0.00";
        }
        
        private void Add_carToList(object sender, RoutedEventArgs e)
        {
            if (IDInpt.SelectedItem != null && cmbSelect.SelectedItem != null && StartTime.SelectedItem != null && Endtime.SelectedItem != null &&BrndInput.SelectedItem != null && ColorInput.SelectedItem != null)
            {
                CarManager h = new CarManager(new UnitOfWork(new CarContext()));

                Car tempcar = h.GetCar(Int32.Parse(ColorInput.SelectedItem.ToString().ToUpper().Split(" ->")[0].Trim()));
                LoadPriceMethod();


                List<int> IDs = new List<int>();
                if (CarGrid.Items.Count > 0)
                {
                    foreach (var item in CarGrid.Items)
                    {
                        DataRowView Row = (DataRowView)item;
                        IDs.Add(Int32.Parse(Row[0].ToString().Trim()));
                    }
                }
                if (IDs.Contains(tempcar.ID))
                    MessageBox.Show("Car already added");
                else
                    tempCarList.Add(tempcar.ID + "|" + tempcar.Brand + "|" + tempcar.Model + "|" + tempcar.Color + "|" + Math.Round(ExclPrice, 2));

                loadingCars();
                LoadTotalPrices();
                ExclPrice = 0;
            }
            else { MessageBox.Show("Select Values(Date, Car, Time)"); }
        }
        private void loadingCars()
        {
            CarGrid.ItemsSource = new ReservationManager(new UnitOfWork(new CarContext())).loadingCars(tempCarList).DefaultView;
        }
        private void Delete_Car(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((DataRowView)CarGrid.SelectedItem != null)
                {
                    DataRowView dataRowView = (DataRowView)CarGrid.SelectedItem;
                    tempCarList.Remove(tempCarList.Find(s => s.Split("|")[0].Equals(dataRowView.Row[0].ToString().Trim())));
                    loadingCars();
                    LoadTotalPrices();
                }
                else { MessageBox.Show("Select Value"); }
            }
            catch { MessageBox.Show("Select Value"); }
        }
        private void LoadTotalPrices()
        {
            double TotalPrice = 0;
            CustomerManager c = new CustomerManager(new UnitOfWork(new CarContext()));
            ReservationManager h = new ReservationManager(new UnitOfWork(new CarContext()));
            foreach (var item in CarGrid.Items)
            {
                DataRowView dataRowView = (DataRowView)item;
                TotalPrice += Double.Parse(dataRowView.Row[4].ToString());
            }
            var temp = c.GetCustomer(Int32.Parse(IDInpt.SelectedItem.ToString().Trim().Split("_")[0]));
            Double discount = h.GenerateDiscount(TotalPrice, temp.ID, CarGrid.Items.Count);


            TotalPrice = TotalPrice - discount;
            Discount_Price.Content = "€ " + Math.Round(discount, 2).ToString();
            exclTaxPrice.Content = "€ " + Math.Round(TotalPrice, 2).ToString();
            Btw_Price.Content = "€ " + Math.Round((TotalPrice * 0.06), 2).ToString();
            InclPrice = Math.Round((TotalPrice + (TotalPrice * 0.06)), 2);
            InclPrice = (int)Math.Round(InclPrice / 5) * 5;
            TotalBtw_Price.Content = "€ " + InclPrice.ToString();
        }
        public int VisibleInt = 0;
        private void Update_OverTime(object sender, RoutedEventArgs e)
        {
            ReservationManager R = new ReservationManager(new UnitOfWork(new CarContext()));
            ReservationCarsManager Rcm = new ReservationCarsManager(new UnitOfWork(new CarContext()));
            CarManager h = new CarManager(new UnitOfWork(new CarContext()));
            if (VisibleInt == 0)
            {
                if ((DataRowView)ReservationGrid.SelectedItem != null)
                {
                    DataRowView dataRowView = (DataRowView)ReservationGrid.SelectedItem;

                    Update_overtimeVisibility(Visibility.Hidden);
                    addCarBtn.Visibility = Visibility.Collapsed;
                    cmbSelect.Visibility = Visibility.Collapsed;
                    CarIDBox.Visibility = Visibility.Visible;

                    Reservation_ID_Label.Content = dataRowView.Row[0].ToString().Trim();

                    IDInpt.Items.Clear();
                    foreach (var item in Rcm.GetReservationCars(Int32.Parse(dataRowView.Row[0].ToString())))
                        IDInpt.Items.Add(h.GetCar(item.carID).ID.ToString() + " | " + h.GetCar(item.carID).Brand + " | " + h.GetCar(item.carID).Model + " | " + h.GetCar(item.carID).Color);

                    var tempRes = R.GetReservation(Int32.Parse(dataRowView.Row[0].ToString()));

                    TotalBtw_Price.Content = "€ " + tempRes.TotalPrice.ToString();
                    Btw_Price.Content = "€ " + Math.Round((tempRes.TotalPrice - tempRes.ExclusiveBTWPrice), 2).ToString();
                    Discount_Price.Content = "€ " + tempRes.Discount.ToString();
                    exclTaxPrice.Content = "€ " + tempRes.ExclusiveBTWPrice.ToString();

                    IDInpt.SelectedIndex = 0;
                    DiscountLabel.Text = "OVERTIME PRICE";
                    CustomerIdLabel.Content = "Car ID";
                    TypeLabel.Content = "OverHours";
                    AddBtn.Content = "Add Overhours";
                    VisibleInt = 1;
                }
            }
            else if (VisibleInt == 1)
            {
                VisibleInt = 0;
                Update_overtimeVisibility(Visibility.Visible);
                addCarBtn.Visibility = Visibility.Visible;
                cmbSelect.Visibility = Visibility.Visible;
                CarIDBox.Visibility = Visibility.Collapsed;

                TotalBtw_Price.Content = "€ 0.00";
                Btw_Price.Content = "€ 0.00";
                Discount_Price.Content = "€ 0.00";
                exclTaxPrice.Content = "€ 0.00";
                DiscountLabel.Text = "TOTAL DISCOUNT";
                CustomerIdLabel.Content = "Customer Id";
                TypeLabel.Content = "Type";
                AddBtn.Content = "Add Reservation";
                IDInpt.Items.Clear();
                LoadIdsMethod();
            }
        }
        private void LoadOverhourPrice(object sender, EventArgs e)
        {
            loadoverHour_Price(0);
        }
        public void loadoverHour_Price(int i)
        {
            ReservationManager h = new ReservationManager(new UnitOfWork(new CarContext()));
            var tempRes = h.GetReservation(Int32.Parse(Reservation_ID_Label.Content.ToString()));

            DiscountLabel.Text = "OVERTIME PRICE";
            Double OverHourPrice = h.LoadOverhourPrice(Int32.Parse(IDInpt.SelectionBoxItem.ToString().Split("|")[0].Trim()),tempRes.ID, Int32.Parse(CarIDBox.SelectionBoxItem.ToString().Replace(":00", "")), tempRes.EndDate);

            Btw_Price.Content = "€ " + Math.Round((tempRes.TotalPrice - tempRes.ExclusiveBTWPrice), 2).ToString();
            Discount_Price.Content = " € " + OverHourPrice.ToString();
            exclTaxPrice.Content = "€ " + tempRes.ExclusiveBTWPrice.ToString();
            TotalBtw_Price.Content = "€ " + tempRes.TotalPrice.ToString();
            if (i == 1)
                h.Save();
        }
        public void Update_overtimeVisibility(Visibility temp)
        {
            DatePick.Visibility = temp;
            StartTime.Visibility = temp;
            Endtime.Visibility = temp;
            BrndInput.Visibility = temp;
            ColorInput.Visibility = temp;
            StreetInput.Visibility = temp;
            CityInput.Visibility = temp;
            DeliverInput.Visibility = temp;
            StartLabel.Visibility = temp;
            EndLabel.Visibility = temp;
            ModelLabel.Visibility = temp;
            BrandLabel.Visibility = temp;
            StreetLabel.Visibility = temp;
            CityLabel.Visibility = temp;
            DeliverLabel.Visibility = temp;
        }
        private void HomeBtn_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0.1-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void Load_CarGrid(object sender, RoutedEventArgs e)
        {
            loadingCars();
        }
    }
}
