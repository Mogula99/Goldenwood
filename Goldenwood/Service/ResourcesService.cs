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

        //These values could be cached later
        public int GetCurrentGoldAmount()
        {
            return dbContext.Player.Where(x => x.Id == Program.PlayerId).Select(x => x.GoldAmount).FirstOrDefault(0);
        }

        //These values could be cached later
        public int GetCurrentWoodAmount()
        {
            return dbContext.Player.Where(x => x.Id == Program.PlayerId).Select(x => x.WoodAmount).FirstOrDefault(0);
        }

        //These values could be cached later
        public int GetTotalGoldIncome()
        {
            return dbContext.EconomicBuilding.Where(x => x.IsBuilt == true).Select(x => x.GoldIncome).ToList().Sum();
        }

        //These values could be cached later
        public int GetTotalWoodIncome()
        {
            return dbContext.EconomicBuilding.Where(x => x.IsBuilt == true).Select(x => x.WoodIncome).ToList().Sum();
        }

        public void AddResourcesAfterTick()
        {
            var goldIncome = GetTotalGoldIncome();
            var woodIncome = GetTotalWoodIncome();
            AddResources(goldIncome, woodIncome);
        }

        public void AddResources(int goldToAdd, int woodToAdd)
        {
            var player = dbContext.Player.Where(x => x.Id == Program.PlayerId).FirstOrDefault();
            if (player != null)
            {
                player.GoldAmount += goldToAdd;
                player.WoodAmount += woodToAdd;
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
    }
}
