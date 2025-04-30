using Azure;
using System.Windows;
using System.Windows.Controls;
using Telefonkönyv.Models;
using static MaterialDesignThemes.Wpf.Theme;

namespace Telefonkönyv
{
    /*public class PhoneBookEntry
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
        public string Nickname { get; set; }

        public string Irsz { get; set; }
    }*/

    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            TestDatabaseConnection();
            LoadPhoneBookEntries();
        }

        private void TestDatabaseConnection()
        {
            try
            {
                var testQuery = context.Contacts.FirstOrDefault();
                MessageBox.Show(testQuery != null ? "Adatbázis kapcsolat sikeres!" : "Nincs adat az adatbázisban.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba az adatbázis kapcsolat során: {ex.Message}");
            }
        }


        private void LoadPhoneBookEntries()
        {
            /*var phoneBookEntries = new List<PhoneBookEntry>
            {
                new PhoneBookEntry { Name = "Kiss János", City = "Budapest", Phone = "123456789", Irsz ="5000" ,Nickname = "Jani", Email = "", Note = "asad as da sd as da sd a sd a sd a sd as da  da sd a dsa sd" },
                new PhoneBookEntry { Name = "Nagy Anna", City = "Debrecen", Phone = "987654321", Irsz ="5000" ,Nickname = "", Email = "anna@gmail.com", Note = "" },
                new PhoneBookEntry { Name = "Tóth Péter", City = "Szeged", Phone = "456123789", Irsz ="5000" ,Nickname = "", Email = "asdasda", Note = "" }
            };

            PhoneBookList.ItemsSource = phoneBookEntries;*/

            

        var contactsWithDetails = context.Contacts
       .Where(c => c.IsActive) // Csak az aktív kapcsolatok
       .Select(c => new
       {
           ContactId = c.Id,
           Name = c.Name,
           PhoneNumber = c.PhoneNumber,
           Email = c.Email,
           Nickname = c.Nickname,
           Note = c.Note,
           CityName = c.City != null ? c.City.CityName : "Nincs megadva", // Város neve
           PictureData = c.Pictures.FirstOrDefault().Picture1 // Az első kép bináris adatai
       })
       .ToList();


            PhoneBookList.ItemsSource = contactsWithDetails;
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
                sideIrsz.Text = selected.Irsz == "" ? "nincs megadva" : selected.Irsz;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            teljesFelvétel teljesFelvételWindow = new teljesFelvétel();
            teljesFelvételWindow.ShowDialog();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var selected = PhoneBookList.SelectedItem as PhoneBookEntry;
            if (selected != null)
            {
                MódósításWindow módósításWindow = new MódósításWindow(selected);
                módósításWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Kérlek válassz ki egy bejegyzést a módosításhoz.");
            }
        }
    }
}
