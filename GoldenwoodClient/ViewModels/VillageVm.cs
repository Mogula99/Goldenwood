using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GoldenwoodClient.ExternalApis;
using GoldenwoodClient.Infrastructure;
using GoldenwoodClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenwoodClient.ViewModels
{
    public partial class VillageVm : ObservableObject, IQueryAttributable
    {
        private readonly IResourcesApi resourcesApi;

        private readonly ResourcesRecordConverter resourcesRecordConverter;

        [ObservableProperty] private ResourcesRecord playerResources = new ResourcesRecord();

        public VillageVm(IResourcesApi resourcesApi, ResourcesRecordConverter resourcesRecordConverter)
        {
            this.resourcesApi = resourcesApi;
            this.resourcesRecordConverter = resourcesRecordConverter;

            var timer = Application.Current.Dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) => DoSomething();
            timer.Start();
            LoadDataAsync();
        }

        public async Task LoadDataAsync()
        {
            var playerResourcesFromApi = await resourcesApi.GetAmount();
            PlayerResources = resourcesRecordConverter.CoreResourcesRecordToMauiResourcesRecord(playerResourcesFromApi);
        }


        [RelayCommand]
        async Task Tick()
        {
            await resourcesApi.UpdateResourcesAfterTick();
            LoadDataAsync();
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

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query["Reload"] is bool and true)
                await LoadDataAsync();
        }

        private void DoSomething()
        {
            LoadDataAsync();
        }
    }
}
