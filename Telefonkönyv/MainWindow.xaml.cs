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
        public string Email { get; set; }
        public string Note { get; set; }
        public string Nickname { get; set; }
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
                new PhoneBookEntry { Name = "Kiss János", City = "Budapest", Phone = "123456789", Nickname = "Jani", Email = "", Note = "asad as da sd as da sd a sd a sd a sd as da  da sd a dsa sd" },
                new PhoneBookEntry { Name = "Nagy Anna", City = "Debrecen", Phone = "987654321", Nickname = "", Email = "anna@gmail.com", Note = "" },
                new PhoneBookEntry { Name = "Tóth Péter", City = "Szeged", Phone = "456123789", Nickname = "", Email = "asdasda", Note = "" }
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
                sideEmail.Text = selected.Email == "" ? "nincs megadva" : selected.Email;
                sidedesc.Text = selected.Note == "" ? "nincs megadva" : selected.Note;
                sideNickname.Text = selected.Nickname == "" ? "nincs megadva" : selected.Nickname;
                sideCity.Text = selected.City;
                sideName.Text = selected.Name;
                sideTel.Text = selected.Phone;
                
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            teljesFelvétel teljesFelvételWindow = new teljesFelvétel();
            teljesFelvételWindow.ShowDialog();
        }
    }
}
