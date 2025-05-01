using Azure;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;
using Telefonkönyv.Models;
using static MaterialDesignThemes.Wpf.Theme;

namespace Telefonkönyv
{

    public partial class MainWindow : Window
    {
        private readonly MyDbContext _context;

        public MainWindow(MyDbContext context)
        {
            InitializeComponent();
            _context = context;
            TestDatabaseConnection();
            LoadPhoneBookEntries();

            var cityList = _context.Citys
                .Select(c => new { c.CityId, c.CityName }).First();
            MessageBox.Show(cityList.CityName);

        }

        private void TestDatabaseConnection()
        {
            try
            {
                var testQuery = _context.Contacts.FirstOrDefault();
                MessageBox.Show(testQuery != null ? "Adatbázis kapcsolat sikeres!" : "Nincs adat az adatbázisban.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba az adatbázis kapcsolat során: {ex.Message}");
            }
        }


        private void LoadPhoneBookEntries()
        {
            var contactsWithDetails = _context.Contacts
                .Include(c => c.City) // Betöltjük a kapcsolódó várost
                .Include(c => c.Pictures) // Betöltjük a kapcsolódó képeket
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
                    Irsz = c.City != null ? c.City.Irsz : "Nincs megadva", // Irányítószám
                    PictureData = c.Pictures.FirstOrDefault() != null ? c.Pictures.FirstOrDefault().Picture1 : null // Az első kép bináris adatai
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
            var selected = PhoneBookList.SelectedItem as dynamic; // Az objektum a LINQ lekérdezésből származik
            if (selected != null)
            {
                sideEmail.Text = string.IsNullOrEmpty(selected.Email) ? "nincs megadva" : selected.Email;
                sidedesc.Text = string.IsNullOrEmpty(selected.Note) ? "nincs megadva" : selected.Note;
                sideNickname.Text = string.IsNullOrEmpty(selected.Nickname) ? "nincs megadva" : selected.Nickname;
                sideCity.Text = selected.CityName;
                sideName.Text = selected.Name;
                sideTel.Text = selected.PhoneNumber;
                sideIrsz.Text = string.IsNullOrEmpty(selected.Irsz) ? "nincs megadva" : selected.Irsz;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            teljesFelvétel teljesFelvételWindow = new teljesFelvétel();
            teljesFelvételWindow.ShowDialog();
            LoadPhoneBookEntries();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var selected = PhoneBookList.SelectedItem as dynamic;
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
