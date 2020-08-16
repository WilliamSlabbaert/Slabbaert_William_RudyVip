using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using Console_App_RudyVip;
using DataLayer_RudyVip;
using System.Data;
using System.Linq;
using System.Globalization;

namespace WPF_RudyVip
{
    /// <summary>
    /// Interaction logic for Cars.xaml
    /// </summary>
    public partial class Cars : Window
    {

        public Cars()
        {
            InitializeComponent();
        }
        private void HomeBtn_Loaded(object sender, RoutedEventArgs e)
        {
            HomeBtn.Height = HomeBtn.ActualHeight;
            HomeBtn.Width = HomeBtn.ActualWidth;
        }
        private void HomeBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow x = new MainWindow();
            x.Show();
            this.Close();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.-]+");
            e.Handled = regex.IsMatch(e.Text);
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
        private void Add_NewCar(object sender, RoutedEventArgs e)
        {
            CarManager h = new CarManager(new UnitOfWork(new CarContext()));
            if (!string.IsNullOrWhiteSpace(WellnessInpt.Text)
                && !string.IsNullOrWhiteSpace(NightInpt.Text)
                && !string.IsNullOrWhiteSpace(WeddingInpt.Text)
                && !string.IsNullOrWhiteSpace(FirstInpt.Text)
                && !string.IsNullOrWhiteSpace(ColorInpt.Text)
                && !string.IsNullOrWhiteSpace(NightInpt.Text)
                && !string.IsNullOrWhiteSpace(ModelInpt.Text)
                && !string.IsNullOrWhiteSpace(BrandInpt.Text))
            {
                Adding(h);
                MessageBox.Show("Done");
            }
            else { MessageBox.Show("Please give all values"); }
        }

        private void Load_CarData(object sender, RoutedEventArgs e)
        {
            CarDataLoading();
        }
        private void CarDataLoading()
        {
            CarDataGrid.ItemsSource = new CarManager(new UnitOfWork(new CarContext())).LoadGrid().DefaultView;
        }

        private void Delete_Item(object sender, RoutedEventArgs e)
        {
            CarManager h = new CarManager(new UnitOfWork(new CarContext()));
            try
            {
                if ((DataRowView)CarDataGrid.SelectedItem != null)
                {
                    DataRowView dataRowView = (DataRowView)CarDataGrid.SelectedItem;
                    h.RemoveCar(Int32.Parse(dataRowView.Row[0].ToString().Trim()));
                    CarDataLoading();
                }
                else { MessageBox.Show("Select Value"); }
            }
            catch { MessageBox.Show("Select Value"); }
        }

        private void Update_Item(object sender, RoutedEventArgs e)
        {
            CarManager h = new CarManager(new UnitOfWork(new CarContext()));
            if (AddBtn.Visibility != Visibility.Collapsed)
            {
                ClearInput();
                try
                {
                    if ((DataRowView)CarDataGrid.SelectedItem != null)
                    {
                        DataRowView dataRowView = (DataRowView)CarDataGrid.SelectedItem;

                        WellnessInpt.Text = h.GetCar(Int32.Parse(dataRowView.Row[0].ToString().Trim())).WellnessPrice.ToString();
                        NightInpt.Text = h.GetCar(Int32.Parse(dataRowView.Row[0].ToString().Trim())).NightlifePrice.ToString();
                        WeddingInpt.Text = h.GetCar(Int32.Parse(dataRowView.Row[0].ToString().Trim())).WeddingPrice.ToString();
                        FirstInpt.Text = h.GetCar(Int32.Parse(dataRowView.Row[0].ToString().Trim())).FirstHourPrice.ToString();
                        ColorInpt.Text = h.GetCar(Int32.Parse(dataRowView.Row[0].ToString().Trim())).Color.ToString();
                        ModelInpt.Text = h.GetCar(Int32.Parse(dataRowView.Row[0].ToString().Trim())).Model.ToString();
                        BrandInpt.Text = h.GetCar(Int32.Parse(dataRowView.Row[0].ToString().Trim())).Brand.ToString();
                        VisibilityON();
                        IDInput.Content = h.GetCar(Int32.Parse(dataRowView.Row[0].ToString().Trim())).ID.ToString();

                    }
                    else { MessageBox.Show("Select Value"); }
                }
                catch { MessageBox.Show("Select Value"); }
            }
            else
            {
                ClearInput();
                VisibilityOFF();
            }
        }

        private void Add_UpdatedCar(object sender, RoutedEventArgs e)
        {
            CarManager h = new CarManager(new UnitOfWork(new CarContext()));
            Car x = h.GetCar(Int32.Parse(IDInput.Content.ToString()));
            x.Brand = BrandInpt.Text.ToString().ToUpper();
            x.Model = ModelInpt.Text.ToString().ToUpper();
            x.Color = ColorInpt.Text.ToString().ToUpper();
            x.FirstHourPrice = Math.Round(Double.Parse(FirstInpt.Text, CultureInfo.InvariantCulture), 2, MidpointRounding.AwayFromZero);
            x.NightlifePrice = Math.Round(Double.Parse(NightInpt.Text, CultureInfo.InvariantCulture), 2, MidpointRounding.AwayFromZero);
            x.WeddingPrice = Math.Round(Double.Parse(WeddingInpt.Text, CultureInfo.InvariantCulture), 2, MidpointRounding.AwayFromZero);
            x.WellnessPrice = Math.Round(Double.Parse(WellnessInpt.Text, CultureInfo.InvariantCulture), 2, MidpointRounding.AwayFromZero);

            h.Save();
            CarDataLoading();
            VisibilityOFF();
            ClearInput();
            
        }
        private void Adding(CarManager h)
        {
            h.AddCarOne(BrandInpt.Text.ToString().ToUpper(),
                    ModelInpt.Text.ToString().ToUpper(),
                    ColorInpt.Text.ToString().ToUpper(),
                    Math.Round(Double.Parse(FirstInpt.Text, CultureInfo.InvariantCulture), 2, MidpointRounding.AwayFromZero),
                    Math.Round(Double.Parse(NightInpt.Text, CultureInfo.InvariantCulture), 2, MidpointRounding.AwayFromZero),
                    Math.Round(Double.Parse(WeddingInpt.Text, CultureInfo.InvariantCulture), 2, MidpointRounding.AwayFromZero),
                    Math.Round(Double.Parse(WellnessInpt.Text, CultureInfo.InvariantCulture), 2, MidpointRounding.AwayFromZero));
            ClearInput();
            CarDataLoading();
        }
        private void VisibilityON()
        {
            AddBtn.Visibility = Visibility.Collapsed;
            AddUpdate.Visibility = Visibility.Visible;
            DeleteBtn.Visibility = Visibility.Hidden;
        }
        private void VisibilityOFF()
        {
            AddBtn.Visibility = Visibility.Visible;
            AddUpdate.Visibility = Visibility.Collapsed;
            DeleteBtn.Visibility = Visibility.Visible;
        }
        private void ClearInput()
        {
            WellnessInpt.Text = "";
            NightInpt.Text = "";
            WeddingInpt.Text = "";
            FirstInpt.Text = "";
            ColorInpt.Text = "";
            ModelInpt.Text = "";
            BrandInpt.Text = "";
        }

        private void Check_Update(object sender, RoutedEventArgs e)
        {
            CarManager h = new CarManager(new UnitOfWork(new CarContext()));
            try
            {
                if ((DataRowView)CarDataGrid.SelectedItem != null)
                {
                    DataRowView dataRowView = (DataRowView)CarDataGrid.SelectedItem;
                    Car temp = h.GetCar(Int32.Parse(dataRowView.Row[0].ToString().Trim()));
                    if (temp.Available == true)
                    {
                        MessageBox.Show(temp.ID.ToString() + " " + temp.Brand + " " + temp.Model + " " + temp.Color + " is unavailable.");
                        temp.Available = false;
                    }
                    else
                    {
                        MessageBox.Show(temp.ID.ToString() + " " + temp.Brand + " " + temp.Model + " " + temp.Color + " is available.");
                        temp.Available = true;
                    }
                    h.Save();
                    CarDataLoading();
                }
                else { MessageBox.Show("Select Value"); }
            }
            catch { MessageBox.Show("Select Value"); }
        }
    }
}
