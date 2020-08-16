using Console_App_RudyVip;
using Console_App_RudyVip.Domain;
using DataLayer_RudyVip;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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
    /// Interaction logic for Reports.xaml
    /// </summary>
    public partial class Reports : Window
    {
        public DataTable dt = new DataTable();

        public Reports()
        {
            InitializeComponent();
        }
        private void ExitButton(object sender, RoutedEventArgs e)
        {
            this.Close();
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
        private void HomeBtn_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
        private void Load_Columns(object sender, RoutedEventArgs e)
        {
            dt = new ReservationManager(new UnitOfWork(new CarContext())).Load_Columns();
            ReservationGrid.ItemsSource = dt.DefaultView;
        }
        private void Select_Radio(object sender, RoutedEventArgs e)
        {
            List<String> IDS = new List<String>();
            if (R1.IsChecked == true)
                cmbSelect.Visibility = Visibility.Hidden;
            else if (R2.IsChecked == true)
            {
                cmbSelect.Items.Clear();
                cmbSelect.Visibility = Visibility.Visible;
                LoadingIdsIntoComboBox(new CustomerManager(new UnitOfWork(new CarContext())).GetAllCustomers().Select(c => c.ID.ToString()).ToList());
            }
            else if (R3.IsChecked == true)
            {
                cmbSelect.Items.Clear();
                cmbSelect.Visibility = Visibility.Visible;
                LoadingIdsIntoComboBox(new CarManager(new UnitOfWork(new CarContext())).GetAllCars().Select(c => c.ID.ToString()).ToList());
            }
        }
        private void LoadingIdsIntoComboBox(List<String> IDS)
        {
            foreach (var item in IDS)
                cmbSelect.Items.Add("ID " + item);
            cmbSelect.SelectedIndex = 0;
        }
        private void Click_Date_Visibility(object sender, RoutedEventArgs e)
        {
            if(DateCheckbox.IsChecked == true)
            {
                StartDate_Picker.Visibility = Visibility.Visible;
                EndDate_Picker.Visibility = Visibility.Visible;
                StartDate_Label.Visibility = Visibility.Visible;
                EndDate_Label.Visibility = Visibility.Visible;
            }
            else
            {
                StartDate_Picker.Visibility = Visibility.Hidden;
                EndDate_Picker.Visibility = Visibility.Hidden;
                StartDate_Label.Visibility = Visibility.Hidden;
                EndDate_Label.Visibility = Visibility.Hidden;
            }
        }
        private void Click_LoadReservations(object sender, RoutedEventArgs e)
        {
            ReservationManager h = new ReservationManager(new UnitOfWork(new CarContext()));
            ReservationCarsManager Rcm = new ReservationCarsManager(new UnitOfWork(new CarContext()));
            List<Reservation> temp = new List<Reservation>();
            Load_ReservationsData(temp);
            String SelectItem = cmbSelect.SelectionBoxItem.ToString().Replace("ID ", "");
            
            if (!DateCheckbox.IsChecked == true)
            {
                if (R1.IsChecked == true)
                    Load_ReservationsData(h.GetAllReservations());
                else if (R2.IsChecked == true) {
                    foreach (var item in Rcm.GetAllReservationCars().FindAll(c => c.customerID.ToString().Equals(SelectItem)).ToList())
                        temp.Add(h.GetReservation(item.reservationID));
                    Load_ReservationsData(temp);
                }
                else if (R3.IsChecked == true) {
                    foreach (var item in Rcm.GetAllReservationCars().FindAll(c => c.carID.ToString().Equals(SelectItem)).ToList())
                        temp.Add(h.GetReservation(item.reservationID));
                    Load_ReservationsData(temp);
                }
            }
            else if (DateCheckbox.IsChecked == true)
            {
                DateTime Start;
                DateTime End; 
                try{
                    Start = StartDate_Picker.SelectedDate.Value;
                    End = EndDate_Picker.SelectedDate.Value;
                    if (End < Start)
                        MessageBox.Show("End Date cant be later than Start date");
                    else if (End > Start)
                    {
                        if (R1.IsChecked == true)
                            Load_ReservationsData(h.GetAllReservations().FindAll(c => c.StartDate >= Start && c.EndDate <= End).ToList());
                        else if (R2.IsChecked == true)
                        {
                            foreach (var item in Rcm.GetAllReservationCars().FindAll(c => c.customerID.ToString().Equals(SelectItem)).ToList())
                                temp.Add(h.GetReservation(item.reservationID));
                            Load_ReservationsData(temp.FindAll(c => c.StartDate >= Start && c.EndDate <= End).ToList());
                        }
                        else if (R3.IsChecked == true)
                        {
                            foreach (var item in Rcm.GetAllReservationCars().FindAll(c => c.carID.ToString().Equals(SelectItem)).ToList())
                                temp.Add(h.GetReservation(item.reservationID));
                            Load_ReservationsData(temp.FindAll(c => c.StartDate >= Start && c.EndDate <= End).ToList());
                        }
                    }
                }
                catch { MessageBox.Show("Fill in the Dates"); }
            }
        }
        void Load_ReservationsData(List<Reservation> resList)
        {
            dt.Rows.Clear();
            ReservationManager h = new ReservationManager(new UnitOfWork(new CarContext()));
            ReservationGrid.ItemsSource = h.LoadReservation_Reports(resList,dt).DefaultView;
        }
    }
}
