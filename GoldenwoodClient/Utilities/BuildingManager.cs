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

        public async Task BuildOrUpgrade(string buildingName)
        {
            int currentBuildingLevel = await buildingApi.GetBuildingLevel(buildingName);
            var neededResourcesFromApi = await buildingApi.GetNeededResources(buildingName, currentBuildingLevel + 1);
            GoldenwoodClient.Models.ResourcesRecord neededResources = resourcesRecordConverter.CoreResourcesRecordToMauiResourcesRecord(neededResourcesFromApi);

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
