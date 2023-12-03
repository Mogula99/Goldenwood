using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GoldenwoodClient.ExternalApis;
using GoldenwoodClient.Infrastructure;
using GoldenwoodClient.Models;
using GoldenwoodClient.Utilities;
using Goldenwood.Utilities;
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

        private readonly BuildingManager buildingManager;

        //These properties are used in VillagePage.xaml
        public string MayorHouseName { get; set; } = Constants.MayorBuildingName;
        public string ChurchName { get; set; } = Constants.ChurchBuildingName;
        public string WellName { get; set; } = Constants.WellBuildingName;
        public string BakeryName { get; set; } = Constants.BakeryBuildingName;
        public string FarmName { get; set; } = Constants.FarmBuildingName;
        public string BarracksName { get; set; } = Constants.BarracksBuildingName;
        public string ArcheryRangeName { get; set; } = Constants.ArcheryRangeBuildingName;
        public string StablesName { get; set; } = Constants.StablesBuildingName;


        [ObservableProperty] private GoldenwoodClient.Models.ResourcesRecord playerResources = new GoldenwoodClient.Models.ResourcesRecord();

        public VillageVm(IResourcesApi resourcesApi, ResourcesRecordConverter resourcesRecordConverter, BuildingManager buildingManager)
        {
            this.resourcesApi = resourcesApi;
            
            this.resourcesRecordConverter = resourcesRecordConverter;

            this.buildingManager = buildingManager;

            var timer = Application.Current.Dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += async (s, e) => await LoadDataAsync();
            timer.Start();

            LoadDataAsync();
        }

        /// <summary>
        /// This method refreshes all the data that updates in the VillagePage.
        /// </summary>
        /// <returns>Nothing</returns>
        public async Task LoadDataAsync()
        {
            var playerResourcesFromApi = await resourcesApi.GetAmount();
            PlayerResources = resourcesRecordConverter.CoreResourcesRecordToMauiResourcesRecord(playerResourcesFromApi);
        }

        /// <summary>
        /// This method checks if the Reload query has been added when switching to the VillagePage. If so, it refreshes all the data in the page.
        /// </summary>
        /// <param name="query">Queries sent when switching to the VillagePage</param>
        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query["Reload"] is bool and true)
                await LoadDataAsync();
        }

        //Commands

        /// <summary>
        /// This command handles the situation when the player clicks on the "hidden" Tick button.
        /// </summary>
        /// <returns>Nothing</returns>
        [RelayCommand]
        async Task Tick()
        {
            await resourcesApi.UpdateResourcesAfterTick();
            await LoadDataAsync();
        }

        /// <summary>
        /// This command changes the current page to the MainPage.
        /// </summary>
        /// <returns>Nothing</returns>
        [RelayCommand]
        async Task GoToSurroundings()
        {
            await Shell.Current.GoToAsync("//" + nameof(MainPage), new Dictionary<string, object> { { "Reload", true } });
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
    }
}
