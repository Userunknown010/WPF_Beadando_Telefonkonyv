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
    /// Interaction logic for Felvetel.xaml
    /// </summary>
    public partial class Felvetel : Window
    {
        public Felvetel()
        {
            InitializeComponent();
        }

        private void Hozzaadgomb_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Elvetgomb_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
