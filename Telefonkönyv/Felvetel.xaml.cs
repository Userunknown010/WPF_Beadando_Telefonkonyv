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

        public Felvetel(MyDbContext context)
        {
            InitializeComponent();
            _context = context;

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
                MessageBox.Show("Sikeres mentés!");
                this.Close();
            }
        }

        private void addContact()
        {
            int? cityid = cityid = _context.Citys
                   .Where(x => x.CityName == varosbe.Text)
                   .Select(x => x.CityId)
                   .FirstOrDefault();

            var contact = new Contact
            {
                Name = nevbe.Text,
                PhoneNumber = telefonszambe.Text,
                Email = null,
                Nickname = null,
                Note = null,
                CityId = cityid,
                UploaderId = null,
                IsActive = true
            };

            _context.Contacts.Add(contact);
            _context.SaveChanges();
        
        }

        private void Elvetgomb_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
