using GoldenwoodClient.ExternalApis;
using GoldenwoodClient.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace GoldenwoodClient.Utilities
{
    public class BuildingManager
    {
        private readonly IResourcesApi resourcesApi;
        private readonly IBuildingApi buildingApi;

        private readonly ResourcesRecordConverter resourcesRecordConverter;

        private readonly AlertManager alertManager;

        public BuildingManager(IResourcesApi resourcesApi, IBuildingApi buildingApi, ResourcesRecordConverter resourcesRecordConverter, AlertManager alertManager)
        {
            this.resourcesApi = resourcesApi;
            this.buildingApi = buildingApi;
            this.resourcesRecordConverter = resourcesRecordConverter;
            this.alertManager = alertManager;
        }

        /// <summary>
        /// This method handles building/upgrading of buildings. It checks if it can be done and then calls appropriate API methods. 
        /// </summary>
        /// <param name="buildingName">Name of the building to be build/upgraded</param>
        /// <returns>Nothing</returns>
        public async Task BuildOrUpgrade(string buildingName)
        {
            bool canBeUpgraded = await buildingApi.GetIsBuildable(buildingName);

            if (canBeUpgraded)
            {
                int currentBuildingLevel = await buildingApi.GetBuildingLevel(buildingName);
                var neededResources = await buildingApi.GetNeededResources(buildingName, currentBuildingLevel + 1);

                bool buildingAnswer = await alertManager.SendBuildingQuestionAlert(buildingName, currentBuildingLevel + 1, neededResources);
                if (buildingAnswer == true)
                {
                    //Check that the player does have enough resources
                    var playerResourcesFromApi = await resourcesApi.GetAmount();
                    var playerResources = resourcesRecordConverter.CoreResourcesRecordToMauiResourcesRecord(playerResourcesFromApi);
                    if (playerResources.GoldAmount < neededResources.GoldAmount || playerResources.WoodAmount < neededResources.WoodAmount)
                    {
                        alertManager.SendNotEnoughResourcesAlert();
                        return;
                    }
                    else
                    {
                        bool result = await buildingApi.BuildOrUpgrade(buildingName);
                        if (result == true)
                        {
                            alertManager.SendBuildingSuccessfulAlert();
                        }
                        else
                        {
                            alertManager.SendBuildingFailureAlert();
                        }
                    }
                }
            }
        }
    }
}
