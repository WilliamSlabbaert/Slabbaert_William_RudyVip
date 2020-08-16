using Console_App_RudyVip;
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
    /// Interaction logic for Customer.xaml
    /// </summary>
    public partial class Customer : Window
    {
        public Customer()
        {
            InitializeComponent();
        }

        private void CheckBoxChanged(object sender, RoutedEventArgs e)
        {
            if (!Bedrijfcheckbox.IsChecked == true)
                VisibilityON(3);
            else
                VisibilityON(2);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CustomerManager h = new CustomerManager(new UnitOfWork(new CarContext()));
            if (!Bedrijfcheckbox.IsChecked == true)
            {
                if (categorieDropdown != null&& !string.IsNullOrWhiteSpace(Firstinpt.Text) && !string.IsNullOrWhiteSpace(LastInput.Text)&& !string.IsNullOrWhiteSpace(PhoneInpt.Text) && !string.IsNullOrWhiteSpace(StreetInpt.Text) && !string.IsNullOrWhiteSpace(HouseNrInpt.Text) && !string.IsNullOrWhiteSpace(ZipInpt.Text) && !string.IsNullOrWhiteSpace(CityInpt.Text))
                {
                    h.AddCustomerCompany(
                       Firstinpt.Text.ToUpper() + " | " + LastInput.Text.ToUpper(), 
                        StreetInpt.Text.ToUpper() + " | " + HouseNrInpt.Text.ToUpper(), 
                        CityInpt.Text.ToUpper() + " | " + ZipInpt.Text.ToUpper(), 
                        categorieDropdown.Text.ToUpper(), 
                        PhoneInpt.Text.ToUpper(), "NULL");
                    Clear();
                }
            }
            else if(Bedrijfcheckbox.IsChecked == true)
            {
                if (categorieDropdown.SelectedItem != null && !string.IsNullOrWhiteSpace(Firstinpt.Text) && !string.IsNullOrWhiteSpace(BtwInput.Text)&& !string.IsNullOrWhiteSpace(PhoneInpt.Text) && !string.IsNullOrWhiteSpace(StreetInpt.Text) && !string.IsNullOrWhiteSpace(HouseNrInpt.Text)&& !string.IsNullOrWhiteSpace(ZipInpt.Text) && !string.IsNullOrWhiteSpace(CityInpt.Text))
                {
                    h.AddCustomerCompany(
                        Firstinpt.Text.ToUpper(),
                        StreetInpt.Text.ToUpper() + " | " + HouseNrInpt.Text.ToUpper(),
                        CityInpt.Text.ToUpper() + " | " + ZipInpt.Text.ToUpper(),
                        categorieDropdown.Text.ToUpper(),
                        PhoneInpt.Text.ToUpper(), 
                        BtwInput.Text.ToUpper());
                    Clear();
                }
            }
            CustomerDataLoading();
        }

        private void HomeBtn_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }
        private void MinimizeButton(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Minimized;
            else if (this.WindowState == WindowState.Minimized)
                this.WindowState = WindowState.Normal;
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
        private void Load_DataGrid(object sender, RoutedEventArgs e)
        {
            CustomerDataLoading();
        }

        private void CustomerDataLoading()
        {
            CustomerManager h = new CustomerManager(new UnitOfWork(new CarContext()));
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("ID", typeof(int)));
            dt.Columns.Add(new DataColumn("Categorie", typeof(String)));
            dt.Columns.Add(new DataColumn("Name", typeof(String)));
            dt.Columns.Add(new DataColumn("PhoneNummer", typeof(String)));
            dt.Columns.Add(new DataColumn("Street", typeof(String)));
            dt.Columns.Add(new DataColumn("City", typeof(String)));
            dt.Columns.Add(new DataColumn("BTW Nummer", typeof(String)));

            foreach (var item in h.GetAllCustomers().OrderBy(s => s.ID))
            {
                DataRow nr = dt.NewRow();
                nr[0] = item.ID;
                nr[1] = item.Categorie;
                nr[2] = item.Name;
                nr[3] = item.PhoneNummer;
                nr[4] = item.Street;
                nr[5] = item.City;
                nr[6] = item.BtwNummer;
                dt.Rows.Add(nr);
            }
            DataGridItems.ItemsSource = dt.DefaultView;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            CustomerManager h = new CustomerManager(new UnitOfWork(new CarContext()));

            if ((DataRowView)DataGridItems.SelectedItem != null)
            {
                DataRowView dataRowView = (DataRowView)DataGridItems.SelectedItem;
                h.RemoveCustomer(Int32.Parse(dataRowView.Row[0].ToString().Trim()));
                CustomerDataLoading();
            }
            else
                MessageBox.Show("Select Value");
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            CustomerManager h = new CustomerManager(new UnitOfWork(new CarContext()));
            if (DeleteBtn.Visibility == Visibility.Visible)
            {
                if ((DataRowView)DataGridItems.SelectedItem != null)
                {
                    VisibilityON(0);

                    DataRowView dataRowView = (DataRowView)DataGridItems.SelectedItem;
                    
                    var temp = h.GetCustomer(Int32.Parse(dataRowView.Row[0].ToString().Trim()));
                    String categorie = temp.Categorie.ToString().ToLower().Trim();

                    if (categorie == "organisation" || categorie == "concert promoter" || categorie == "marriage planner" || categorie == "event agency")
                    {
                        Bedrijfcheckbox.IsChecked = true;
                        BtwInput.Text = temp.BtwNummer;
                        CityInpt.Text = temp.City.Split("|")[0].Trim();
                        ZipInpt.Text = temp.City.Split("|")[1].Trim();
                        Firstinpt.Text = temp.Name;
                        PhoneInpt.Text = temp.PhoneNummer;
                        StreetInpt.Text = temp.Street.Split("|")[0].Trim();
                        HouseNrInpt.Text = temp.Street.Split("|")[1].Trim();
                        IDLabel.Content = temp.ID.ToString().Trim();
                    }
                    else
                    {
                        Bedrijfcheckbox.IsChecked = false;
                        CityInpt.Text = temp.City.Split("|")[0].Trim();
                        ZipInpt.Text = temp.City.Split("|")[1].Trim();
                        Firstinpt.Text = temp.Name.Split("|")[0].Trim();
                        LastInput.Text = temp.Name.Split("|")[1].Trim();
                        PhoneInpt.Text = temp.PhoneNummer;
                        StreetInpt.Text = temp.Street.Split("|")[0].Trim();
                        HouseNrInpt.Text = temp.Street.Split("|")[1].Trim();
                        IDLabel.Content = temp.ID.ToString().Trim();
                    }
                    CustomerDataLoading();
                }
                else
                    MessageBox.Show("Select Value");
            }
            else if (DeleteBtn.Visibility == Visibility.Hidden)
            {
                VisibilityON(1);
                Clear();
            }
        }

        private void UpdateCustomer_click(object sender, RoutedEventArgs e)
        {
            CustomerManager h = new CustomerManager(new UnitOfWork(new CarContext()));
            var temp = h.GetCustomer(Int32.Parse(IDLabel.Content.ToString().Trim()));
            if (!Bedrijfcheckbox.IsChecked == true)
            {
                if (categorieDropdown != null && !string.IsNullOrWhiteSpace(Firstinpt.Text)&& !string.IsNullOrWhiteSpace(LastInput.Text) && !string.IsNullOrWhiteSpace(PhoneInpt.Text)&& !string.IsNullOrWhiteSpace(StreetInpt.Text)&& !string.IsNullOrWhiteSpace(HouseNrInpt.Text)&& !string.IsNullOrWhiteSpace(ZipInpt.Text)&& !string.IsNullOrWhiteSpace(CityInpt.Text))
                {
                    temp.Categorie = categorieDropdown.Text.ToUpper();
                    temp.BtwNummer = "NULL";
                    temp.City = CityInpt.Text.ToUpper() + " | " + ZipInpt.Text.ToUpper();
                    temp.Name = Firstinpt.Text.ToUpper() + " | " + LastInput.Text.ToUpper();
                    temp.PhoneNummer = PhoneInpt.Text.ToUpper();
                    temp.Street = StreetInpt.Text.ToUpper() + " | " + HouseNrInpt.Text.ToUpper();
                    h.Save();
                    Clear();
                }
            }
            else if (Bedrijfcheckbox.IsChecked == true)
            {
                if (categorieDropdown != null && !string.IsNullOrWhiteSpace(Firstinpt.Text) && !string.IsNullOrWhiteSpace(BtwInput.Text)&& !string.IsNullOrWhiteSpace(PhoneInpt.Text)&& !string.IsNullOrWhiteSpace(StreetInpt.Text) && !string.IsNullOrWhiteSpace(HouseNrInpt.Text) && !string.IsNullOrWhiteSpace(ZipInpt.Text)&& !string.IsNullOrWhiteSpace(CityInpt.Text))
                {
                    temp.Categorie = categorieDropdown.Text.ToUpper();
                    temp.BtwNummer = BtwInput.Text.ToUpper();
                    temp.City = CityInpt.Text.ToUpper() + " | " + ZipInpt.Text.ToUpper();
                    temp.Name = Firstinpt.Text.ToUpper();
                    temp.PhoneNummer = PhoneInpt.Text.ToUpper();
                    temp.Street = StreetInpt.Text.ToUpper() + " | " + HouseNrInpt.Text.ToUpper();
                    h.Save();
                    Clear();
                }
            }
            
            VisibilityON(1);
            CustomerDataLoading();
        }
        private void VisibilityON(int btn)
        {
            if (btn == 1)
            {
                Bedrijfcheckbox.IsChecked = false;
                DeleteBtn.Visibility = Visibility.Visible;
                UpdateCustomerBtn.Visibility = Visibility.Collapsed;
                AddCustomer.Visibility = Visibility.Visible;
            }
            else if(btn == 2)
            {
                BtwInput.Visibility = Visibility.Visible;
                BtwLabel.Visibility = Visibility.Visible;
                LastNameLabel.Visibility = Visibility.Collapsed;
                LastInput.Visibility = Visibility.Collapsed;

                firstNameLabel.Content = "Name";
                categorieDropdown.Items.Clear();


                categorieDropdown.Items.Add("Organisation");
                categorieDropdown.Items.Add("Concert promoter");
                categorieDropdown.Items.Add("Marriage planner");
                categorieDropdown.Items.Add("Event agency");
                categorieDropdown.SelectedIndex = 0;
            }
            else if(btn == 3)
            {
                BtwInput.Visibility = Visibility.Collapsed;
                BtwLabel.Visibility = Visibility.Collapsed;
                LastNameLabel.Visibility = Visibility.Visible;
                LastInput.Visibility = Visibility.Visible;
                firstNameLabel.Content = "FirstName";
                categorieDropdown.Items.Clear();

                categorieDropdown.Items.Add("Standard");
                categorieDropdown.Items.Add("VIP");
                categorieDropdown.SelectedIndex = 0;
            }
            else
            {
                DeleteBtn.Visibility = Visibility.Hidden;
                UpdateCustomerBtn.Visibility = Visibility.Visible;
                AddCustomer.Visibility = Visibility.Collapsed;
            }
        }
        private void Clear()
        {
            LastInput.Text = "";
            BtwInput.Text = "";
            Firstinpt.Text = "";
            PhoneInpt.Text = "";
            StreetInpt.Text = "";

            HouseNrInpt.Text = "";
            BtwInput.Text = "";
            ZipInpt.Text = "";
            CityInpt.Text = "";
        }
    }
    
}
