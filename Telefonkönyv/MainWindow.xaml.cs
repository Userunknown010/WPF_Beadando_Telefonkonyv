using System.Windows;
using System.Windows.Controls;
using static MaterialDesignThemes.Wpf.Theme;

namespace Telefonkönyv
{
    public class PhoneBookEntry
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadPhoneBookEntries();
        }

        private void LoadPhoneBookEntries()
        {
            var phoneBookEntries = new List<PhoneBookEntry>
            {
                new PhoneBookEntry { Name = "Kiss János", City = "Budapest", Phone = "123456789" },
                new PhoneBookEntry { Name = "Nagy Anna", City = "Debrecen", Phone = "987654321" },
                new PhoneBookEntry { Name = "Tóth Péter", City = "Szeged", Phone = "456123789" }
            };

            PhoneBookList.ItemsSource = phoneBookEntries;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Felvetel addwindow = new Felvetel();
            addwindow.ShowDialog();
        }

        private void LoginMenuButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.ShowDialog();
        }
        private void RegistrationMenuButton_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow regWindow = new RegistrationWindow();
            regWindow.ShowDialog();
        }

        private void PhoneBookList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
               var selected = PhoneBookList.SelectedItem as PhoneBookEntry;
            if (selected != null)
            {
                sideCity.Text = selected.City;
                sideName.Text = selected.Name;
                sideTel.Text = selected.Phone;
            }
        }
    }
}
