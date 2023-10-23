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
        private ApplicationDbContext dbContext;

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
            var player = dbContext.Player.Where(x => x.Id == Program.PlayerId).FirstOrDefault();
            if (player != null)
            {
                player.GoldAmount += resourcesToAdd.goldAmount;
                player.WoodAmount += resourcesToAdd.woodAmount;
                dbContext.SaveChanges();
            }
        }

        public void ReduceTickInterval(int reductionAmount)
        {
            var player = dbContext.Player.Where(x => x.Id == Program.PlayerId).FirstOrDefault();
            if (player != null)
            {
                player.TickInterval -= reductionAmount;
                dbContext.SaveChanges();
            }
        }

        //These values could be cached later
        private int GetCurrentGoldAmount()
        {
            return dbContext.Player.Where(x => x.Id == Program.PlayerId).Select(x => x.GoldAmount).FirstOrDefault(0);
        }

        //These values could be cached later
        private int GetCurrentWoodAmount()
        {
            return dbContext.Player.Where(x => x.Id == Program.PlayerId).Select(x => x.WoodAmount).FirstOrDefault(0);
        }

        //These values could be cached later
        private int GetTotalGoldIncome()
        {
            return dbContext.EconomicBuilding.Where(x => x.IsBuilt == true).Select(x => x.GoldIncome).ToList().Sum();
        }

        //These values could be cached later
        private int GetTotalWoodIncome()
        {
            return dbContext.EconomicBuilding.Where(x => x.IsBuilt == true).Select(x => x.WoodIncome).ToList().Sum();
        }
    }
}
