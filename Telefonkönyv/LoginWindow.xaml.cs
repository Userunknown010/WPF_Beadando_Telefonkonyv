using System.Windows;
using Telefonkönyv.Models;

namespace Telefonkönyv
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
                MessageBox.Show($"Bejelentkezés sikeres: {username}");
            }
            else MessageBox.Show("Ez a felhasználónév nem létezik!");
            
            this.Close();
        }
    }
}
