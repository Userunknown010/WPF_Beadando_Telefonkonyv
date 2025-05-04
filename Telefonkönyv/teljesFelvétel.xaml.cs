using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
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
    /// Interaction logic for teljesFelvétel.xaml
    /// </summary>
    public partial class teljesFelvétel : Window
    {

        private byte[]? UploadedImageData;
        private string Felhasználó;
        public teljesFelvétel(string felh)
        {
            InitializeComponent();
            Felhasználó = felh;
        }

        private void Hozzaadgomb_Click(object sender, RoutedEventArgs e)
        {
            if (nevbe.Text == "") {
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
                AddContact();
                MessageBox.Show("Sikeres mentés!");
                this.Close();
            }
        }

        private void AddContact()
        {
            

            using (var context = new MyDbContext())
            {

                int? pictureId = null;

                if (UploadedImageData != null)
                {
                    var picture = new Picture
                    {
                        Picture1 = UploadedImageData, // A kép bináris adatai
                    };

                    context.Pictures.Add(picture);
                    context.SaveChanges(); // Mentés a pictures táblába

                    pictureId = picture.Id; // A mentett kép ID-jének lekérése
                }

                int? cityid = cityid = context.Citys
                    .Where(x => x.CityName == varosbe.Text)
                    .Select(x => x.CityId)
                    .FirstOrDefault();

                var uploader = context.Users.Where(x=>x.Username == Felhasználó).Select(x => x.Id).First();

                var contact = new Contact
                {
                    Name = nevbe.Text,
                    PhoneNumber = telefonszambe.Text,
                    Email = emailbe.Text,
                    Nickname = becenevbe.Text,
                    Note = megjegyzesbe.Text,
                    CityId = cityid,
                    UploaderId = uploader,
                    PictureId = pictureId,
                    IsActive = true
                };

                context.Contacts.Add(contact);
                context.SaveChanges();

                var user = context.Users
                    .Where(x => x.Username == Felhasználó)
                    .Select(x => x.Id)
                    .FirstOrDefault();
                var NewLog = new Log
                {
                    UserId = user,
                    Operation = "Új kontakt - " + contact.Name,
                    Timestamp = DateTime.Now
                };

                context.Logs.Add(NewLog);
                context.SaveChanges();


            }

            MessageBox.Show("Kapcsolat sikeresen hozzáadva.");
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

                    UploadedImageData = System.IO.File.ReadAllBytes(openFileDialog.FileName);
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
