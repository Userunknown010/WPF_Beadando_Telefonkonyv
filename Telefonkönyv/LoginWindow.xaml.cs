using System.Windows;

namespace Telefonk�nyv
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = pwdPasswordBox.Password;

            MessageBox.Show($"Bejelentkez�s sikeres: {username}");
            this.Close();
        }
    }
}
