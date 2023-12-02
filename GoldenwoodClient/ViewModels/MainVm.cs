using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GoldenwoodClient.ExternalApis;
using GoldenwoodClient.Infrastructure;
using GoldenwoodClient.Models;
using GoldenwoodClient.Utilities;
using Goldenwood.Utilities;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Refit;

namespace GoldenwoodClient.ViewModels
{
    public partial class MainVm : ObservableObject, IQueryAttributable
    {
        private readonly IResourcesApi resourcesApi;
        private readonly IBuildingApi buildingApi;
        private readonly IMilitaryApi militaryApi;

        private readonly UnitGroupConverter unitGroupConverter;
        private readonly ResourcesRecordConverter resourcesRecordConverter;

        private readonly BuildingManager buildingManager;
        private readonly MilitaryManager militaryManager;

        private ICollection<int> unitIds = new List<int> {1, 2, 3, 4, 5, 6};

        public string GoldMineName { get; set; } = Constants.GoldMineBuildingName;
        public string LoggingCampName { get; set; } = Constants.LoggingCampBuildingName;
        public List<string> UnitNames { get; set; } = Constants.UnitNames;

        [ObservableProperty] private int secondsToTick = 10;

        [ObservableProperty] private ObservableCollection<UnitGroup> playerUnitGroups;
        [ObservableProperty] private ObservableCollection<bool> isUnitRecruitable;

        [ObservableProperty] private GoldenwoodClient.Models.ResourcesRecord playerResources = new GoldenwoodClient.Models.ResourcesRecord();
        [ObservableProperty] private GoldenwoodClient.Models.ResourcesRecord resourcesIncome = new GoldenwoodClient.Models.ResourcesRecord();

        public MainVm(IMilitaryApi militaryApi, UnitGroupConverter unitGroupConverter, IResourcesApi resourcesApi, ResourcesRecordConverter resourcesRecordConverter, IBuildingApi buildingApi, BuildingManager buildingManager, MilitaryManager militaryManager)
        {
            this.resourcesApi = resourcesApi;
            this.buildingApi = buildingApi;
            this.militaryApi = militaryApi;

            this.resourcesRecordConverter = resourcesRecordConverter;
            this.unitGroupConverter = unitGroupConverter;

            this.buildingManager = buildingManager;

            //Set ticking timer
            var timer = Application.Current.Dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) => ProcessTick();
            timer.Start();

            // Run in Fire & Forget manner
            LoadDataAsync();
            this.militaryManager = militaryManager;
        }

        public async Task LoadDataAsync()
        {
            var unitGroupsFromApi = await militaryApi.PlayerUnits();
            var unitGroups = unitGroupsFromApi.Select(unitGroupConverter.CoreUnitGroupToMauiUnitGroup);
            PlayerUnitGroups = new ObservableCollection<UnitGroup>(unitGroups);

            List<bool> isUnitRecruitable = new List<bool> { false, false, false, false, false, false };
            foreach (int unitId in unitIds)
            {
                isUnitRecruitable[unitId-1] = await militaryApi.CanBeRecruited(unitId);
            }
            IsUnitRecruitable = new ObservableCollection<bool>(isUnitRecruitable);


            var playerResourcesFromApi = await resourcesApi.GetAmount();
            PlayerResources = resourcesRecordConverter.CoreResourcesRecordToMauiResourcesRecord(playerResourcesFromApi);

            var resourcesIncomeFromApi = await resourcesApi.GetIncome();
            ResourcesIncome = resourcesRecordConverter.CoreResourcesRecordToMauiResourcesRecord(resourcesIncomeFromApi);
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query["Reload"] is bool and true)
                await LoadDataAsync();
        }

        private async void ProcessTick()
        {
            SecondsToTick -= 1;
            if(SecondsToTick <= 0)
            {
                await resourcesApi.UpdateResourcesAfterTick();
                SecondsToTick = 10;
                LoadDataAsync();
            }
        }

        //Commands

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

        [RelayCommand]
        async void BuildOrUpgrade(string buildingName)
        {
            await buildingManager.BuildOrUpgrade(buildingName);
            LoadDataAsync();
        }

        [RelayCommand]
        async void RecruitUnits(string unitIdString)
        {
            int unitId = int.Parse(unitIdString);
            await militaryManager.RecruitUnit(unitId);
            LoadDataAsync();
        }
    }
}
