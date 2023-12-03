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

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = base.CreateWindow(activationState);

            const int newWidth = 1024;
            const int newHeight = 768;

            window.Width = newWidth;
            window.Height = newHeight;
            window.X = 200;
            window.Y = 50;

            return window;
        }
    }
}
