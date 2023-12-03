using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GoldenwoodClient.ExternalApis;
using GoldenwoodClient.Infrastructure;
using GoldenwoodClient.Models;
using GoldenwoodClient.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenwoodClient.ViewModels
{
    public partial class MapVm : ObservableObject, IQueryAttributable
    {
        private readonly IResourcesApi resourcesApi;

        private readonly MilitaryManager militaryManager;

        private readonly ResourcesRecordConverter resourcesRecordConverter;

        //These properties are used in MapPage.xaml
        [ObservableProperty] private ObservableCollection<bool> isEnemyAlive = new ObservableCollection<bool> { true, true, true, true, true };

        [ObservableProperty] private ResourcesRecord playerResources = new ResourcesRecord();

        public MapVm(IResourcesApi resourcesApi, ResourcesRecordConverter resourcesRecordConverter, MilitaryManager militaryManager)
        {
            this.resourcesApi = resourcesApi;

            this.militaryManager = militaryManager;

            this.resourcesRecordConverter = resourcesRecordConverter;

            LoadDataAsync();
        }

        /// <summary>
        /// This method refreshes all the data that updates in the MapPage.
        /// </summary>
        /// <returns>Nothing</returns>
        public async Task LoadDataAsync()
        {
            var playerResourcesFromApi = await resourcesApi.GetAmount();
            PlayerResources = resourcesRecordConverter.CoreResourcesRecordToMauiResourcesRecord(playerResourcesFromApi);
        }

        /// <summary>
        /// This method checks if the Reload query has been added when switching to the MapPage. If so, it refreshes all the data in the page.
        /// </summary>
        /// <param name="query">Queries sent when switching to the MapPage</param>
        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query["Reload"] is bool and true)
                await LoadDataAsync();
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
        /// This command changes the current page to the MainPage.
        /// </summary>
        /// <returns>Nothing</returns>
        [RelayCommand]
        async Task GoToSurroundings()
        {
            await Shell.Current.GoToAsync("//" + nameof(MainPage), new Dictionary<string, object> { { "Reload", true } });
        }

        /// <summary>
        /// This command makes the appropriate method calls when the player clicks on an enemy.
        /// </summary>
        /// <param name="enemyIdString">String of the ID of the enemy the player clicked on</param>
        /// <returns>Nothing</returns>
        [RelayCommand]
        async Task FightEnemy(string enemyIdString)
        {
            int enemyId = int.Parse(enemyIdString);
            bool enemyDefeated = await militaryManager.FightEnemy(enemyId);
            if (enemyDefeated)
            {
                List<bool> aliveEnemies = new List<bool>(IsEnemyAlive);
                aliveEnemies[enemyId - 1] = false;
                IsEnemyAlive = new ObservableCollection<bool>(aliveEnemies);
            }
            await LoadDataAsync();
        }
    }
}
