namespace GoldenwoodClient
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(VillagePage), typeof(VillagePage));
            Routing.RegisterRoute(nameof(MapPage), typeof(MapPage));
        }
    }
}
