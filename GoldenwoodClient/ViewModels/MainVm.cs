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
        [ObservableProperty] private ObservableCollection<UnitGroup> playerUnitGroups;
        [ObservableProperty] private ResourcesRecord playerResources = new ResourcesRecord { GoldAmount = 12, WoodAmount=24 };

        public MainVm(IMilitaryApi militaryApi, UnitGroupConverter unitGroupConverter, IResourcesApi resourcesApi, ResourcesRecordConverter resourcesRecordConverter)
        {
            this.militaryApi = militaryApi;
            this.resourcesApi = resourcesApi;
            this.resourcesRecordConverter = resourcesRecordConverter;
            this.unitGroupConverter = unitGroupConverter;
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
            Console.WriteLine("Přišli mi data z apička, uživatel má počet zlata " + playerResources.GoldAmount);
        }

        [RelayCommand]
        async Task GoToVillage()
        {
            await Shell.Current.GoToAsync(nameof(VillagePage), new Dictionary<string, object> { });
        }

        [RelayCommand]
        async Task GoToMap()
        {
            await Shell.Current.GoToAsync(nameof(MapPage), new Dictionary<string, object> { });
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query["Reload"] is bool and true)
                await LoadDataAsync();
        }
    }
}
