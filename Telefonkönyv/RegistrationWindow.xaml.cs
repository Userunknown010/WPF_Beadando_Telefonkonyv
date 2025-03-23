using System.Windows;

namespace Telefonkönyv
{
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;
            string confirmPassword = PasswordBoxConfirm.Password;

            if (password != confirmPassword)
            {
                MessageBox.Show("A két jelszó nem egyezik!");
                return;
            }

            MessageBox.Show($"Sikeres regisztráció: {username}");

            this.Close();
        }
    }
}
