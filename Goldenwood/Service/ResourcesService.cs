using Goldenwood.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldenwood.Service
{
    public class ResourcesService
    {
        private readonly ApplicationDbContext dbContext;
        private int tickInterval = 10;

        public ResourcesService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public ResourcesRecord GetCurrentResourcesAmount()
        {
            var goldAmount = GetCurrentGoldAmount();
            var woodAmount = GetCurrentWoodAmount();
            return new ResourcesRecord(goldAmount, woodAmount);
        }

        public ResourcesRecord GetTotalResourcesIncome()
        {
            var goldIncome = GetTotalGoldIncome();
            var woodIncome = GetTotalWoodIncome();
            return new ResourcesRecord(goldIncome, woodIncome);
        }

        public void AddResourcesAfterTick()
        {
            AddResources(GetTotalResourcesIncome());
        }

        public void AddResources(ResourcesRecord resourcesToAdd)
        {
            var player = dbContext.Player.Where(x => x.Id == Constants.PlayerId).FirstOrDefault();
            if (player != null)
            {
                player.GoldAmount += resourcesToAdd.GoldAmount;
                player.WoodAmount += resourcesToAdd.WoodAmount;
                dbContext.SaveChanges();
            }
        }

        //TODO: Add tests for this method
        public int GetTickInterval()
        {
            int tickReduction = dbContext.EconomicBuilding.Where(x => x.IsBuilt == true).Select(x => x.TickReduction).ToList().Sum();
            return (tickInterval - tickReduction);
        }

        //These values could be cached later
        private int GetCurrentGoldAmount()
        {
            var player = dbContext.Player.Where(x => x.Id == Constants.PlayerId).FirstOrDefault();
            return player == null ? 0 : player.GoldAmount;
        }

        //These values could be cached later
        private int GetCurrentWoodAmount()
        {
            var player = dbContext.Player.Where(x => x.Id == Constants.PlayerId).FirstOrDefault();
            return player == null ? 0 : player.WoodAmount;
        }

        //These values could be cached later
        private int GetTotalGoldIncome()
        {
            var buildingIncome = dbContext.EconomicBuilding.Where(x => x.IsBuilt == true).Select(x => x.GoldIncome).ToList().Sum();
            //For every defeated enemy, player will gain a bit of wood income
            var enemyIncome = dbContext.Enemy.Where(x => x.Alive == false).ToList().Count * Constants.DefeatedEnemyGoldIncome;
            return buildingIncome + enemyIncome;
        }

        //These values could be cached later
        private int GetTotalWoodIncome()
        {
            var buildingIncome = dbContext.EconomicBuilding.Where(x => x.IsBuilt == true).Select(x => x.WoodIncome).ToList().Sum();
            //For every defeated enemy, player will gain a bit of wood income
            var enemyIncome = dbContext.Enemy.Where(x => x.Alive == false).ToList().Count * Constants.DefeatedEnemyWoodIncome;
            return buildingIncome + enemyIncome;
        }
    }
}
