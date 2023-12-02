using GoldenwoodClient.ExternalApis;
using GoldenwoodClient.ViewModels;

namespace GoldenwoodClient
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
