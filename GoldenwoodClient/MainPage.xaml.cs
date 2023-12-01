using GoldenwoodClient.ViewModels;

namespace GoldenwoodClient
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainVm mainVm)
        {
            InitializeComponent();
            BindingContext = mainVm;
        }
    }

}
