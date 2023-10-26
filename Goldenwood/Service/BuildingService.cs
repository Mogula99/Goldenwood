using Goldenwood.Model.Building;
using Goldenwood.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldenwood.Service
{
    public class BuildingService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ResourcesService resourcesService;

        public BuildingService(ApplicationDbContext dbContext, ResourcesService resourcesService)
        {
            this.dbContext = dbContext;
            this.resourcesService = resourcesService;
        }

        public bool BuildOrUpgrade(string buildingName)
        {
            var result = false;
            if (IsBuildable(buildingName))
            {
                var nextLevel = GetMaxBuiltBuildingLevel(buildingName) + 1;
                var building = GetBuilding(buildingName, nextLevel);
                //Building with specified level was found
                if(building != null)
                {
                    var currentResources = resourcesService.GetCurrentResourcesAmount();
                    //Check if the player has enough resources to build the building
                    if(currentResources.GoldAmount >= building.GoldCost && currentResources.WoodAmount >= building.WoodCost)
                    {
                        building.IsBuilt = true;
                        result = true;
                        resourcesService.AddResources(new ResourcesRecord(-building.GoldCost, -building.WoodCost));
                        dbContext.SaveChanges();
                    }
                }
            }
            return result;
        }

        public bool IsBuildable(string buildingName)
        {
            var buildings = GetBuildings(buildingName);

            //Building with specified name was not found
            //This check could be later replaced with try catch exception in controller
            if(!buildings.Any())
            {
                return false;
            }
            //Check whether there are any more levels that have not been built yet
            return (buildings.Where(x => !x.IsBuilt).ToList().Count > 0);
        }

        public ResourcesRecord GetNeededBuildingResources(string buildingName, int buildingLevel)
        {
            var building = GetBuilding(buildingName, buildingLevel);
            if(building != null)
            {
                return new ResourcesRecord(building.GoldCost, building.WoodCost);
            }
            return new ResourcesRecord(0, 0);
        }

        public Building? GetBuilding(string buildingName, int buildingLevel)
        {
            var buildings = GetBuildings(buildingName);

            //Building with specified name was not found
            //This check could be later replaced with try catch exception in controller
            if (!buildings.Any())
            {
                return null;
            }
            return buildings.Where(x => x.Level == buildingLevel).FirstOrDefault();
        }

        public int GetMaxBuiltBuildingLevel(string buildingName)
        {
            var buildings = GetBuildings(buildingName).Where(x => x.IsBuilt == true);

            //Building with specified name was not found or is not built
            //This check could be later replaced with try catch exception in controller
            if (!buildings.Any())
            {
                return 0;
            }
            return buildings.Max(x => x.Level);
        }

        public IEnumerable<MilitaryBuilding> GetBuiltMilitaryBuildings()
        {
            return dbContext.MilitaryBuilding.Where(x => x.IsBuilt == true).ToList() ?? Enumerable.Empty<MilitaryBuilding>();
        }

        private IEnumerable<Building> GetBuildings(string buildingName)
        {
            var economicBuildings = dbContext.EconomicBuilding.Where(x => x.Name == buildingName).ToList() ?? Enumerable.Empty<EconomicBuilding>();
            if (!economicBuildings.Any())
            {
                var militaryBuildings = dbContext.MilitaryBuilding.Where(x => x.Name == buildingName).ToList() ?? Enumerable.Empty<MilitaryBuilding>();
                return militaryBuildings;
            }
            return economicBuildings;
        }
    }
}
