using System.Text;
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

            var users = _context.Users.Where(x=>x.Password == Encoding.Unicode.GetBytes(password)).Select(x=>x.Username).ToList();

            if (users.Contains(username))
            {
                Application.Current.Properties["FelhasznaloNev"] = username;
                MessageBox.Show($"Bejelentkez�s sikeres: {username}");
                var newLog = new Log
                {
                    UserId = _context.Users.Where(x => x.Username == username).Select(x => x.Id).First(),
                    Operation = "Bejelentkez�s - " + username,
                    Timestamp = DateTime.Now
                };
                _context.Logs.Add(newLog);
                _context.SaveChanges();
            }
            else MessageBox.Show("Ez a felhaszn�l�n�v �s jelsz� p�ros nem l�tezik!");
            
            this.Close();
        }
    }
}
