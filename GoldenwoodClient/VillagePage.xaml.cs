using GoldenwoodClient.ViewModels;

namespace GoldenwoodClient;

public partial class VillagePage : ContentPage
{
	public VillagePage(VillageVm villageVm)
	{
		InitializeComponent();
		BindingContext = villageVm;
	}
}