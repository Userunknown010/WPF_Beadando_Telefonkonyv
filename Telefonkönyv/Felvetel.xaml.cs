using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for Felvetel.xaml
    /// </summary>
    public partial class Felvetel : Window
    {
        private readonly MyDbContext _context;
        private string Felhasználó;

        public Felvetel(MyDbContext context, string felh)
        {
            InitializeComponent();
            _context = context;
            Felhasználó = felh;

        }

        private void Hozzaadgomb_Click(object sender, RoutedEventArgs e)
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
                addContact();
            }
        }

        private void addContact()
        {
            int? cityid = cityid = _context.Citys
                   .Where(x => x.CityName == varosbe.Text)
                   .Select(x => x.CityId)
                   .FirstOrDefault();

            if (cityid == 0)
            {
                MessageBox.Show("Ilyen város nem létezik");
            }
            else {
                var uploader = _context.Users.Where(x => x.Username == Felhasználó).Select(x => x.Id).First();

                var contact = new Contact
                {
                    Name = nevbe.Text,
                    PhoneNumber = telefonszambe.Text,
                    Email = null,
                    Nickname = null,
                    Note = null,
                    CityId = cityid,
                    UploaderId = uploader,
                    IsActive = true
                };

                _context.Contacts.Add(contact);
                _context.SaveChanges();

                var userId = _context.Users.Where(x => x.Username == Felhasználó).Select(x => x.Id).First();

                var NewLog = new Log
                {
                    UserId = userId,
                    Operation = "Új kontakt - " + contact.Name,
                    Timestamp = DateTime.Now
                };

                _context.Logs.Add(NewLog);
                _context.SaveChanges();

                MessageBox.Show("Sikeres mentés!");
                this.Close();
            }

        }

        private void Elvetgomb_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
