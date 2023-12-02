using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GoldenwoodClient.ExternalApis;
using GoldenwoodClient.Infrastructure;
using GoldenwoodClient.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenwoodClient.ViewModels
{
    public partial class MainVm : ObservableObject, IQueryAttributable
    {
        private readonly IMilitaryApi militaryApi;
        private readonly IResourcesApi resourcesApi;

        private readonly UnitGroupConverter unitGroupConverter;
        private readonly ResourcesRecordConverter resourcesRecordConverter;

        [ObservableProperty] private int secondsToTick = 10;

        [ObservableProperty] private ObservableCollection<UnitGroup> playerUnitGroups;
        [ObservableProperty] private ResourcesRecord playerResources = new ResourcesRecord();
        [ObservableProperty] private ResourcesRecord resourcesIncome = new ResourcesRecord();

        public MainVm(IMilitaryApi militaryApi, UnitGroupConverter unitGroupConverter, IResourcesApi resourcesApi, ResourcesRecordConverter resourcesRecordConverter)
        {
            this.militaryApi = militaryApi;
            this.resourcesApi = resourcesApi;
            this.resourcesRecordConverter = resourcesRecordConverter;
            this.unitGroupConverter = unitGroupConverter;

            var timer = Application.Current.Dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) => DoSomething(timer);
            timer.Start();
            // Run in Fire & Forget manner
            LoadDataAsync();
        }

        public async Task LoadDataAsync()
        {
            var unitGroupsFromApi = await militaryApi.PlayerUnits();
            var unitGroups = unitGroupsFromApi.Select(unitGroupConverter.CoreUnitGroupToMauiUnitGroup);
            PlayerUnitGroups = new ObservableCollection<UnitGroup>(unitGroups);


            var playerResourcesFromApi = await resourcesApi.GetAmount();
            PlayerResources = resourcesRecordConverter.CoreResourcesRecordToMauiResourcesRecord(playerResourcesFromApi);

            var resourcesIncomeFromApi = await resourcesApi.GetIncome();
            ResourcesIncome = resourcesRecordConverter.CoreResourcesRecordToMauiResourcesRecord(resourcesIncomeFromApi);
        }

        [RelayCommand]
        async Task GoToVillage()
        {
            await Shell.Current.GoToAsync(nameof(VillagePage), new Dictionary<string, object> { { "Reload", true } });
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

        private async void DoSomething(IDispatcherTimer timer)
        {
            SecondsToTick -= 1;
            if(SecondsToTick <= 0)
            {
                await resourcesApi.UpdateResourcesAfterTick();
                SecondsToTick = 10;
                LoadDataAsync();
            }
        }
    }
}
