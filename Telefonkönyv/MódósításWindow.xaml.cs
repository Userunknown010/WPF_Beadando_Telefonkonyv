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

namespace Telefonkönyv
{
    /// <summary>
    /// Interaction logic for MódósításWindow.xaml
    /// </summary>
    public partial class MódósításWindow : Window
    {
        public MódósításWindow(PhoneBookEntry obj)
        {
            InitializeComponent();
            nevbe.Text = obj.Name;
            becenevbe.Text = obj.Nickname;
            telefonszambe.Text = obj.Phone;
            varosbe.Text = obj.City;
            emailbe.Text = obj.Email;
            megjegyzesbe.Text = obj.Note;
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
                MessageBox.Show("Sikeres mentés!");
                this.Close();
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
