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
        private readonly IMilitaryApi militaryApi;

        private readonly UnitGroupConverter unitGroupConverter;
        private readonly ResourcesRecordConverter resourcesRecordConverter;

        private readonly BuildingManager buildingManager;
        private readonly MilitaryManager militaryManager;

        //These properties are used for binding in the MainPage.xaml
        public string GoldMineName { get; set; } = Constants.GoldMineBuildingName;
        public string LoggingCampName { get; set; } = Constants.LoggingCampBuildingName;
        public List<string> UnitNames { get; set; } = Constants.UnitNames;

        [ObservableProperty] private int secondsToTick = 10;
        [ObservableProperty] private int tickInterval = 10;

        [ObservableProperty] private ObservableCollection<UnitGroup> playerUnitGroups = new ObservableCollection<UnitGroup>();
        [ObservableProperty] private ObservableCollection<bool> isUnitRecruitable = new ObservableCollection<bool>();

        [ObservableProperty] private GoldenwoodClient.Models.ResourcesRecord playerResources = new GoldenwoodClient.Models.ResourcesRecord();
        [ObservableProperty] private GoldenwoodClient.Models.ResourcesRecord resourcesIncome = new GoldenwoodClient.Models.ResourcesRecord();

        public MainVm(IMilitaryApi militaryApi, UnitGroupConverter unitGroupConverter, IResourcesApi resourcesApi, ResourcesRecordConverter resourcesRecordConverter, BuildingManager buildingManager, MilitaryManager militaryManager)
        {
            this.resourcesApi = resourcesApi;
            this.militaryApi = militaryApi;

            this.resourcesRecordConverter = resourcesRecordConverter;
            this.unitGroupConverter = unitGroupConverter;

            this.buildingManager = buildingManager;
            this.militaryManager = militaryManager;

            //Set ticking timer
            var timer = Application.Current.Dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) => ProcessTick();
            timer.Start();

            // Run in Fire & Forget manner
            LoadDataAsync();
        }

        /// <summary>
        /// This method refreshes all the data that updates in the MainPage.
        /// </summary>
        /// <returns>Nothing</returns>
        public async Task LoadDataAsync()
        {
            var unitGroupsFromApi = await militaryApi.PlayerUnits();
            var unitGroups = unitGroupsFromApi.Select(unitGroupConverter.CoreUnitGroupToMauiUnitGroup);
            PlayerUnitGroups = new ObservableCollection<UnitGroup>(unitGroups);

            List<bool> isUnitRecruitable = new List<bool> { false, false, false, false, false, false };
            foreach (int unitId in Constants.UnitIds)
            {
                isUnitRecruitable[unitId-1] = await militaryApi.CanBeRecruited(unitId);
            }
            IsUnitRecruitable = new ObservableCollection<bool>(isUnitRecruitable);

            TickInterval = await resourcesApi.GetTickInterval();

            var playerResourcesFromApi = await resourcesApi.GetAmount();
            PlayerResources = resourcesRecordConverter.CoreResourcesRecordToMauiResourcesRecord(playerResourcesFromApi);

            var resourcesIncomeFromApi = await resourcesApi.GetIncome();
            ResourcesIncome = resourcesRecordConverter.CoreResourcesRecordToMauiResourcesRecord(resourcesIncomeFromApi);
        }

        /// <summary>
        /// This method checks if the Reload query has been added when switching to the MainPage. If so, it refreshes all the data in the page.
        /// </summary>
        /// <param name="query">Queries sent when switching to the MainPage</param>
        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query["Reload"] is bool and true)
                await LoadDataAsync();
        }

        /// <summary>
        /// This method processes one tick of the timer.
        /// </summary>
        private async void ProcessTick()
        {
            SecondsToTick -= 1;
            if(SecondsToTick <= 0)
            {
                await resourcesApi.UpdateResourcesAfterTick();
                SecondsToTick = TickInterval;
                await LoadDataAsync();
            }
        }

        //Commands

        /// <summary>
        /// This command changes the current page to the VillagePage.
        /// </summary>
        /// <returns>Nothing</returns>
        [RelayCommand]
        async Task GoToVillage()
        {
            await Shell.Current.GoToAsync(nameof(VillagePage), new Dictionary<string, object> { { "Reload", true } });
        }

        /// <summary>
        /// This command changes the current page to the MapPage.
        /// </summary>
        /// <returns>Nothing</returns>
        [RelayCommand]
        async Task GoToMap()
        {
            await Shell.Current.GoToAsync(nameof(MapPage), new Dictionary<string, object> { { "Reload", true } });
        }

        /// <summary>
        /// This command calls appropriate methods when the player clicks a building.
        /// </summary>
        /// <param name="buildingName">Name of the building the player clicked at</param>
        /// <returns>Nothing</returns>
        [RelayCommand]
        async Task BuildOrUpgrade(string buildingName)
        {
            await buildingManager.BuildOrUpgrade(buildingName);
            await LoadDataAsync();
        }

        /// <summary>
        /// This command calls appropriate methods when the player wants to recruit a unit.
        /// </summary>
        /// <param name="unitIdString">String of the unitID of the unit that the player clicked at.</param>
        /// <returns>Nothing</returns>
        [RelayCommand]
        async Task RecruitUnits(string unitIdString)
        {
            int unitId = int.Parse(unitIdString);
            await militaryManager.RecruitUnit(unitId);
            await LoadDataAsync();
        }
    }
}
