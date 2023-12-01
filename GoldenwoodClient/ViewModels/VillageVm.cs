using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GoldenwoodClient.ExternalApis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenwoodClient.ViewModels
{
    public partial class VillageVm : ObservableObject
    {
        private readonly IResourcesApi resourcesApi;

        public VillageVm(IResourcesApi resourcesApi)
        {
            this.resourcesApi = resourcesApi;
        }

        [RelayCommand]
        async Task Tick()
        {
            await resourcesApi.UpdateResourcesAfterTick();
        }

        [RelayCommand]
        async Task GoToSurroundings()
        {
            await Shell.Current.GoToAsync("//" + nameof(MainPage), new Dictionary<string, object> { { "Reload", true } });
        }

        [RelayCommand]
        async Task GoToMap()
        {
            await Shell.Current.GoToAsync(nameof(MapPage), new Dictionary<string, object> { { "Reload", true } });
        }
    }
}
