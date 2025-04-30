using System.Configuration;
using System.Data;
using System.Windows;

namespace Telefonkönyv
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        services.AddDbContext<MyDbContext>(options =>
            options.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=MyAppDb;Trusted_Connection=True;TrustServerCertificate=True;"));

    }

}
