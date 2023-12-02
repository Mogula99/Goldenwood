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
            timer.Tick += (s, e) => LoadDataAsync();
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

        [RelayCommand]
        async void BuildOrUpgrade(string buildingName)
        {
            await buildingManager.BuildOrUpgrade(buildingName);
            LoadDataAsync();
        }
    }
}
