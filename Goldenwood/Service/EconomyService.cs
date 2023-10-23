using Goldenwood.Model.Building;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldenwood.Service
{
    public class EconomyService
    {
        private ApplicationDbContext dbContext;

        public EconomyService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //These values could be cached later
        public int GetCurrentGoldAmount()
        {
            return dbContext.Player.Where(x => x.Id == Program.playerId).Select(x => x.GoldAmount).FirstOrDefault(0);
        }

        //These values could be cached later
        public int GetCurrentWoodAmount()
        {
            return dbContext.Player.Where(x => x.Id == Program.playerId).Select(x => x.WoodAmount).FirstOrDefault(0);
        }

        //These values could be cached later
        public int GetTotalGoldIncome()
        {
            return dbContext.EconomicBuildings.Where(x => x.IsBuilt == true).Select(x => x.GoldIncome).ToList().Sum();
        }

        //These values could be cached later
        public int GetTotalWoodIncome()
        {
            return dbContext.EconomicBuildings.Where(x => x.IsBuilt == true).Select(x => x.WoodIncome).ToList().Sum();
        }

        public void AddResourcesAfterTick()
        {
            var goldIncome = GetTotalGoldIncome();
            var woodIncome = GetTotalWoodIncome();
            AddResources(goldIncome, woodIncome);
        }

        public void AddResources(int goldToAdd, int woodToAdd)
        {
            var player = dbContext.Player.Where(x => x.Id == Program.playerId).FirstOrDefault();
            if (player != null)
            {
                player.GoldAmount += goldToAdd;
                player.WoodAmount += woodToAdd;
                dbContext.SaveChanges();
            }
        }

        public void ReduceTickInterval(int reductionAmount)
        {
            var player = dbContext.Player.Where(x => x.Id == Program.playerId).FirstOrDefault();
            if (player != null)
            {
                player.TickInterval -= reductionAmount;
                dbContext.SaveChanges();
            }
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
                    //Check if the player has enough resources to build the building
                    if(GetCurrentGoldAmount() >= building.GoldCost && GetCurrentWoodAmount() >= building.WoodCost)
                    {
                        building.IsBuilt = true;
                        AddResources(-building.GoldCost, -building.WoodCost);
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
            if(buildings == null)
            {
                return false;
            }
            //Check whether there are any more levels that have not been built yet
            return (buildings.Where(x => !x.IsBuilt).ToList().Count > 0);
        }

        public Building? GetBuilding(string buildingName, int buildingLevel)
        {
            var buildings = GetBuildings(buildingName);

            //Building with specified name was not found
            //This check could be later replaced with try catch exception in controller
            if (buildings == null)
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
            if (buildings == null)
            {
                return -1;
            }
            return buildings.Max(x => x.Level);
        }

        private IEnumerable<Building>? GetBuildings(string buildingName)
        {
            var economicBuildings = dbContext.EconomicBuildings.Where(x => x.Name == buildingName).ToList();
            if (economicBuildings.Count == 0)
            {
                var militaryBuildings = dbContext.MilitaryBuildings.Where(x => x.Name == buildingName).ToList();
                return militaryBuildings;
            }
            return economicBuildings;
        }
    }
}
