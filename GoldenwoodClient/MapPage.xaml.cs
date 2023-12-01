using GoldenwoodClient.ViewModels;

namespace GoldenwoodClient;

public partial class MapPage : ContentPage
{
	public MapPage(MapVm vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}