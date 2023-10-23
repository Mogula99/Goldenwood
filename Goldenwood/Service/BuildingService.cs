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
        private ApplicationDbContext dbContext;
        private ResourcesService resourcesService;

        public BuildingService(ApplicationDbContext dbContext, ResourcesService resourcesService)
        {
            this.dbContext = dbContext;
            this.resourcesService = resourcesService;
        }

        public void BuildOrUpgrade(string buildingName)
        {
            if (IsBuildable(buildingName))
            {
                var nextLevel = GetMaxBuiltBuildingLevel(buildingName) + 1;
                var building = GetBuilding(buildingName, nextLevel);
                //Building was found
                if(building != null)
                {
                    var currentResources = resourcesService.GetCurrentResourcesAmount();
                    //Check if the player has enough resources to build the building
                    if(currentResources.goldAmount >= building.GoldCost && currentResources.woodAmount >= building.WoodCost)
                    {
                        building.IsBuilt = true;
                        resourcesService.AddResources(new ResourcesRecord(-building.GoldCost, -building.WoodCost));
                        dbContext.SaveChanges();
                    }
                }
            }
        }

        public bool IsBuildable(string buildingName)
        {
            var buildings = GetBuildings(buildingName);

            //Building with specified name was not found
            //This check could be later replaced with try catch exception in controller
            if(buildings.Count() == 0)
            {
                return false;
            }
            //Check whether there are any more levels that have not been built yet
            return (buildings.Where(x => !x.IsBuilt).ToList().Count > 0);
        }

        public ResourcesRecord? GetNeededBuildingResources(string buildingName, int buildingLevel)
        {
            var building = GetBuilding(buildingName, buildingLevel);
            if(building != null)
            {
                return new ResourcesRecord(building.GoldCost, building.WoodCost);
            }
            return null;
        }

        public Building? GetBuilding(string buildingName, int buildingLevel)
        {
            var buildings = GetBuildings(buildingName);

            //Building with specified name was not found
            //This check could be later replaced with try catch exception in controller
            if (buildings.Count() == 0)
            {
                return null;
            }
            return buildings.Where(x => x.Level == buildingLevel).FirstOrDefault();
        }

        public int GetMaxBuiltBuildingLevel(string buildingName)
        {
            var buildings = GetBuildings(buildingName);

            //Building with specified name was not found
            //This check could be later replaced with try catch exception in controller
            if (buildings.Count() == 0)
            {
                return -1;
            }
            return buildings.Max(x => x.Level);
        }

        private IEnumerable<Building> GetBuildings(string buildingName)
        {
            var economicBuildings = dbContext.EconomicBuilding.Where(x => x.Name == buildingName).ToList() ?? Enumerable.Empty<EconomicBuilding>();
            if (economicBuildings.Count() == 0)
            {
                var militaryBuildings = dbContext.MilitaryBuilding.Where(x => x.Name == buildingName).ToList() ?? Enumerable.Empty<MilitaryBuilding>();
                return militaryBuildings;
            }
            return economicBuildings;
        }

        public IEnumerable<MilitaryBuilding> GetBuiltMilitaryBuildings()
        {
            return dbContext.MilitaryBuilding.Where(x => x.IsBuilt == true).ToList() ?? Enumerable.Empty<MilitaryBuilding>();
        }
    }
}
