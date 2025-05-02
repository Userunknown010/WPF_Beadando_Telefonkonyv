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
        public string Felhasználó;

        public MainWindow(MyDbContext context)
        {
            InitializeComponent();
            _context = context;
            TestDatabaseConnection();
            LoadPhoneBookEntries();
            Felhasználó = null;
        }

        private void TestDatabaseConnection()
        {
            try
            {
                var testQuery = _context.Contacts.FirstOrDefault();
                MessageBox.Show("Adatbázis kapcsolat sikeres!");
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
                    Phone = c.PhoneNumber,
                    Email = c.Email,
                    Nickname = c.Nickname,
                    Note = c.Note,
                    City = c.City.CityName, // Város neve
                    Irsz = c.City.Irsz, // Irányítószám
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
            if (Felhasználó != null)
            {
                Felvetel addwindow = new Felvetel(_context, Felhasználó);
                addwindow.ShowDialog();
                LoadPhoneBookEntries();
            }
            else {
            MessageBox.Show("Csak bejelentkezett felhasználók adhatnak hozzá új bejegyzést.");
            }
            
        }

        private void LoginMenuButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.ShowDialog();
        }
        private void RegistrationMenuButton_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow regWindow = new RegistrationWindow(_context);
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
                sideCity.Text = selected.City;
                sideName.Text = selected.Name;
                sideTel.Text = selected.Phone;
                sideIrsz.Text = string.IsNullOrEmpty(selected.Irsz) ? "nincs megadva" : selected.Irsz;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(Felhasználó != null)
            {
                teljesFelvétel teljesFelvételWindow = new teljesFelvétel(Felhasználó);
                teljesFelvételWindow.ShowDialog();
                LoadPhoneBookEntries();
            }else
            {
                MessageBox.Show("Csak bejelentkezett felhasználók adhatnak hozzá új bejegyzést.");
            }
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            if(Felhasználó != null)
            {
                var perm = _context.Users
                    .Where(x => x.Username == Felhasználó)
                    .Select(x => x.Permission.PermissionName).First();
                if (perm == "editor" || perm == "admin")
                {
                    var selected = PhoneBookList.SelectedItem as dynamic;
                    if (selected != null)
                    {
                        MódósításWindow módósításWindow = new MódósításWindow(selected);
                        módósításWindow.ShowDialog();
                    }
                    else MessageBox.Show("Kérlek válassz ki egy bejegyzést a módosításhoz.");
                }
                else MessageBox.Show("nincs ehhez jogosultságod");
                
            }
            else MessageBox.Show("Csak bejelentkezett felhasználók módosíthatnak bejegyzéseket.");

            }

        private void Törlés_Click(object sender, RoutedEventArgs e)
        {

            if (Felhasználó != null)
            {
                var perm = _context.Users
                    .Where(x => x.Username == Felhasználó)
                    .Select(x => x.Permission.PermissionName).First();
                if (perm == "editor" || perm == "admin")
                {
                    var selected = PhoneBookList.SelectedItem as dynamic;
                    if (selected != null)
                    {
                        selected.IsActive = false;
                        _context.SaveChanges();
                    }
                    else MessageBox.Show("Kérlek válassz ki egy bejegyzést a törléshez.");
                }
                else MessageBox.Show("nincs ehhez jogosultságod");

            }
            else MessageBox.Show("Csak bejelentkezett felhasználók módosíthatnak bejegyzéseket.");

            
        }
    }
}
