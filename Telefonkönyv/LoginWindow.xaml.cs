using System.Windows;
using Telefonk�nyv.Models;

namespace Telefonk�nyv
{
    public partial class LoginWindow : Window
    {

        private readonly MyDbContext _context;
        public LoginWindow(MyDbContext context)
        {
            InitializeComponent();
            _context = context;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = pwdPasswordBox.Password;

            var users = _context.Users.Select(x=>x.Username).ToList();

            if (users.Contains(username))
            {
                Application.Current.Properties["FelhasznaloNev"] = username;
                MessageBox.Show($"Bejelentkez�s sikeres: {username}");
            }
            else MessageBox.Show("Ez a felhaszn�l�n�v nem l�tezik!");
            
            this.Close();
        }
    }
}
