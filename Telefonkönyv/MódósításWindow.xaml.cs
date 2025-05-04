using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Telefonkönyv.Models;

namespace Telefonkönyv
{
    /// <summary>
    /// Interaction logic for MódósításWindow.xaml
    /// </summary>
    public partial class MódósításWindow : Window
    {
        private Contact _contact;
        private MyDbContext _context;
        private string Felhasználó = (string)Application.Current.Properties["FelhasznaloNev"];
        public MódósításWindow(Contact contact, MyDbContext context)
        {
            InitializeComponent();
            _contact = contact;
            _context = context;
            nevbe.Text = _contact.Name;
            becenevbe.Text = _contact.Nickname;
            telefonszambe.Text = _contact.PhoneNumber;
            varosbe.Text = _contact.City.CityName;
            emailbe.Text = _contact.Email;
            megjegyzesbe.Text = _contact.Note;
        }
        private void Módosításgomb_Click(object sender, RoutedEventArgs e)
        {
            if (nevbe.Text == "")
            {
                MessageBox.Show("Név megadása kötelező!");
            }
            else if (telefonszambe.Text == "")
            {
                MessageBox.Show("Telefonszám megadása kötelező!");
            }
            else if (!telefonszambe.Text.All(char.IsDigit))
            {
                MessageBox.Show("A telefonszám csak számokat tartalmazhat!");
            }
            else if (varosbe.Text == "")
            {
                MessageBox.Show("Város megadása kötelező!");
            }
            else
            {
                Update();
                MessageBox.Show("Sikeres mentés!");
                this.Close();
            }
        }

        private void Update()
        {
            using (var context = new MyDbContext())
            {
                var contact = context.Contacts.Find(_contact.Id);
                if (contact != null)
                {
                    contact.Name = nevbe.Text;
                    contact.Nickname = becenevbe.Text;
                    contact.PhoneNumber = telefonszambe.Text;
                    contact.Email = emailbe.Text;
                    contact.Note = megjegyzesbe.Text;
                    int? cityid = context.Citys
                        .Where(x => x.CityName == varosbe.Text)
                        .Select(x => x.CityId)
                        .FirstOrDefault();
                    contact.CityId = cityid;
                    context.SaveChanges();


                    var user = _context.Users.Where(X => X.Username == Felhasználó).Select(x => x.Id).First();

                    var newLog = new Log
                    {
                        UserId = user,
                        Operation = "Módosítás - " + contact.Name,
                        Timestamp = DateTime.Now
                    };
                    context.Logs.Add(newLog);
                    context.SaveChanges();
                }
            }
        }

        private void UploadPicture_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png, *.bmp)|*.jpg;*.jpeg;*.png;*.bmp"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    BitmapImage bitmap = new BitmapImage(new Uri(openFileDialog.FileName));
                    MessageBox.Show("Kép sikeresen feltöltve!");
                }
                catch (Exception)
                {

                    MessageBox.Show("Nem támogatott formátumú képet töltöttél fel!");
                }

            }
            else
            {
                MessageBox.Show("Kép feltöltése sikertelen.");
            }
        }
    }
}
