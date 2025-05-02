using Azure;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;
using Telefonkönyv.Models;
using static MaterialDesignThemes.Wpf.Theme;

namespace Telefonkönyv
{
    
    public partial class MainWindow : Window
    {
        private readonly MyDbContext _context;
        string Felhasználó = (string)Application.Current.Properties["FelhasznaloNev"];

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
            PhoneBookList.ItemsSource = null;
            var contactsWithDetails = _context.Contacts
                .AsNoTracking()
                .Include(c => c.City) // Betöltjük a kapcsolódó várost
                .Include(c => c.Picture) // Betöltjük a kapcsolódó képeket
                .Where(c => c.IsActive) // Csak az aktív kapcsolatok
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
            else
            {
                MessageBox.Show("Csak bejelentkezett felhasználók adhatnak hozzá új bejegyzést.");
            }

        }

        private void LoginMenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (Felhasználó != null)
            {
                MessageBox.Show("Már be vagy jelentkezve.");
                return;
            }
            else
            {
                LoginWindow loginWindow = new LoginWindow(_context);
                loginWindow.ShowDialog();
                Felhasználó = (string)Application.Current.Properties["FelhasznaloNev"];
            }

        }
        private void RegistrationMenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (Felhasználó != null)
            {
                MessageBox.Show("Már be vagy jelentkezve.");
                return;
            }
            else
            {
                RegistrationWindow regWindow = new RegistrationWindow(_context);
                regWindow.ShowDialog();
                Felhasználó = (string)Application.Current.Properties["FelhasznaloNev"];
            }

        }


        public static BitmapImage ByteArrayToImage(byte[] imageData)
        {
            using (var ms = new MemoryStream(imageData))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                image.Freeze(); // Ha háttérszálról akarod elérni
                return image;
            }
        }


        private void PhoneBookList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = PhoneBookList.SelectedItem as Contact; // Az objektum a LINQ lekérdezésből származik
            if (selected != null)
            {
                if (selected.PictureId.HasValue)
                {
                    var picture = _context.Pictures.FirstOrDefault(x => x.Id == selected.PictureId.Value)?.Picture1;
                    sideImage.Source = selected.Picture == null ? null : ByteArrayToImage(picture);
                }
                else
                {
                    sideImage.Source = new BitmapImage(new Uri("pack://application:,,,/images/placeholder.png"));
                }

                sideEmail.Text = string.IsNullOrEmpty(selected.Email) ? "nincs megadva" : selected.Email;
                sidedesc.Text = string.IsNullOrEmpty(selected.Note) ? "nincs megadva" : selected.Note;
                sideNickname.Text = string.IsNullOrEmpty(selected.Nickname) ? "nincs megadva" : selected.Nickname;

                sideCity.Text = selected.City.CityName.ToString();
                sideName.Text = selected.Name;
                sideTel.Text = selected.PhoneNumber;
                sideIrsz.Text = string.IsNullOrEmpty(selected.City.Irsz) ? "nincs megadva" : selected.City.Irsz;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Felhasználó != null)
            {
                teljesFelvétel teljesFelvételWindow = new teljesFelvétel(Felhasználó);
                teljesFelvételWindow.ShowDialog();
                LoadPhoneBookEntries();
            }
            else
            {
                MessageBox.Show("Csak bejelentkezett felhasználók adhatnak hozzá új bejegyzést.");
            }

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            if (Felhasználó != null)
            {
                var perm = _context.Users
                    .Where(x => x.Username == Felhasználó)
                    .Select(x => x.Permission.PermissionName).First();
                if (perm == "editor" || perm == "admin")
                {
                    var selected = PhoneBookList.SelectedItem as Contact;
                    if (selected != null)
                    {
                        MódósításWindow módósításWindow = new MódósításWindow(selected);
                        módósításWindow.ShowDialog();
                        PhoneBookList.SelectedItem = null;
                        LoadPhoneBookEntries();
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
                        LoadPhoneBookEntries();
                    }
                    else MessageBox.Show("Kérlek válassz ki egy bejegyzést a törléshez.");
                }
                else MessageBox.Show("nincs ehhez jogosultságod");

            }
            else MessageBox.Show("Csak bejelentkezett felhasználók módosíthatnak bejegyzéseket.");


        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (Felhasználó != null)
            {
                Application.Current.Properties["FelhasznaloNev"] = null;
                Felhasználó = (string)Application.Current.Properties["FelhasznaloNev"];
                MessageBox.Show("Sikeres kijelentkezés.");
            }
            else
            {
                MessageBox.Show("Nincs bejelentkezve felhasználó.");
            }
        }
    }
}
