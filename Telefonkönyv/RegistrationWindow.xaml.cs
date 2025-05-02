using System.Text;
using System.Windows;
using Telefonkönyv.Models;

namespace Telefonkönyv
{
    public partial class RegistrationWindow : Window
    {
        private readonly MyDbContext _context;
        string nev = (string)Application.Current.Properties["FelhasznaloNev"];
        public RegistrationWindow(MyDbContext context)
        {
            InitializeComponent();

            _context = context;
            Dropdown();
            
        }

        private void Dropdown() {
            var perm = _context.Permissions.Where(x => x.PermissionName != "admin").Select(x => x.PermissionName).ToList();
            if (perm.Count == 0)
            {
                permissionbox.Text = "Nem elérhető";
                permissionbox.IsEnabled = false;
            }
            else
            {
                permissionbox.ItemsSource = perm;
                permissionbox.SelectedIndex = 1;
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;
            string confirmPassword = PasswordBoxConfirm.Password;
            var users = _context.Users.Select(x=>x.Username).ToList();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword)){
                MessageBox.Show("Kérjük, töltse ki az összes mezőt!");
            }
            else if (users.Contains(username)){
                MessageBox.Show("Ez a felhasználónév már létezik!");
            }
            else if (password != confirmPassword){
                MessageBox.Show("A két jelszó nem egyezik!");
            }
            else {
                var permissionID = _context.Permissions.FirstOrDefault(x => x.PermissionName == permissionbox.Text).PermissionId;
                var newUser = new User
                {
                    Password = Encoding.Unicode.GetBytes(password),
                    Username = username,
                    PermissionId = permissionID
                };
                _context.Users.Add(newUser);
                _context.SaveChanges();
                MessageBox.Show(permissionID.ToString());
                MessageBox.Show($"Sikeres regisztráció: {username}");
                Application.Current.Properties["FelhasznaloNev"] = username;
            }



            this.Close();
        }
    }
}
