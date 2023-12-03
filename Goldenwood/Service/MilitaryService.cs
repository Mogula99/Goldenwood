using Goldenwood.Model;
using Goldenwood.Model.Units;
using Goldenwood.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldenwood.Service
{
    public class MilitaryService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ResourcesService resourcesService;
        private readonly BuildingService buildingService;

        public MilitaryService(ApplicationDbContext dbContext, ResourcesService resourcesService, BuildingService buildingService)
        {
            this.dbContext = dbContext;
            this.buildingService = buildingService;
            this.resourcesService = resourcesService;
        }

        public bool CanBeRecruited(int unitId)
        {
            var found = false;
            var militaryBuildings = buildingService.GetBuiltMilitaryBuildings();
            foreach (var militaryBuilding in militaryBuildings)
            {
                if (militaryBuilding.CreatableUnit.Id == unitId)
                {
                    found = true;
                    break;
                }
            }
            return found;
        }

        public Unit GetUnit(int unitId)
        {
            var foundUnit = dbContext.Unit.Where(x => x.Id == unitId).FirstOrDefault();
            return foundUnit;
        }

        public void RecruitUnits(int wantedUnitId, int wantedUnitCount)
        {
            var foundUnit = dbContext.Unit.Where(x => x.Id == wantedUnitId).FirstOrDefault();
            //Wanted unit was found
            if (foundUnit != null)
            {
                //Player has enough resources to build units
                if (PlayerHasEnoughResources(wantedUnitId, wantedUnitCount))
                {
                    var playersArmyId = Constants.PlayerArmyId;
                    var playersArmy = dbContext.Army.Where(x => x.Id == playersArmyId).FirstOrDefault();
                    //Player's army was found
                    if (playersArmy != null)
                    {
                        var playersUnitGroups = playersArmy.UnitGroups;
                        UnitGroup? appropriateUnitGroup = null;
                        //We are trying to find out if the player already has a group consisting of the wanted units
                        var found = UnitGroupExists(foundUnit.Id, playersUnitGroups, out appropriateUnitGroup);

                        //We only need to increase the count of units in the unit group
                        if (found && appropriateUnitGroup != null)
                        {
                            appropriateUnitGroup.UnitCount += wantedUnitCount;
                        }
                        //We need to create need unit group for this type of unit
                        else
                        {
                            var newGroup = new UnitGroup { Unit = foundUnit, UnitCount = wantedUnitCount };
                            dbContext.Add<UnitGroup>(newGroup);
                            playersArmy.UnitGroups.Add(newGroup);
                        }

                        //Now we have tu subtract the used resources
                        var neededResources = GetNeededResources(foundUnit, wantedUnitCount);
                        resourcesService.AddResources(new ResourcesRecord(-neededResources.GoldAmount, -neededResources.WoodAmount));
                        dbContext.SaveChanges();
                    }
                }
            }
        }

        //Method returns true if the player won the fight. False otherwise.
        public bool Fight(int enemyId)
        {
            var foundEnemy = dbContext.Enemy.Where(x => x.Id == enemyId).FirstOrDefault();
            if(foundEnemy != null && foundEnemy.Alive == true)
            {
                var playersArmy = dbContext.Army.Where(x => x.Id == Constants.PlayerArmyId).FirstOrDefault();
                if(playersArmy != null)
                {
                    var fightResult = SimulateFight(playersArmy.UnitGroups, foundEnemy.Army.UnitGroups);
                    if(fightResult == true)
                    {
                        foundEnemy.Alive = false;
                    }
                    dbContext.SaveChanges();
                    return fightResult;
                }
            }
            return false;
        }

        public ICollection<UnitGroup> GetEnemyUnitGroups(int enemyId)
        {
            var enemy = dbContext.Enemy.Where(x => x.Id == enemyId).FirstOrDefault();
            if (enemy != null)
            {
                return enemy.Army == null ? new List<UnitGroup>() : enemy.Army.UnitGroups;
            }
            return new List<UnitGroup>();
        }

        public ICollection<UnitGroup> GetPlayerUnitGroups()
        {
            var playersArmy = dbContext.Army.Where(x => x.Id == Constants.PlayerArmyId).FirstOrDefault();
            return playersArmy == null ? new List<UnitGroup>() : playersArmy.UnitGroups;
        }

        public bool PlayerHasEnoughResources(int unitId, int wantedCount)
        {
            var wantedUnit = dbContext.Unit.Where(x => x.Id == unitId).FirstOrDefault();
            if (wantedUnit == null)
            {
                return false;
            }

            var neededResources = GetNeededResources(wantedUnit, wantedCount);
            var currentResources = resourcesService.GetCurrentResourcesAmount();
            return currentResources.GoldAmount >= neededResources.GoldAmount && currentResources.WoodAmount >= neededResources.WoodAmount;
        }

        private ResourcesRecord GetNeededResources(Unit wantedUnit, int wantedCount)
        {
            return new ResourcesRecord(wantedUnit.GoldCost * wantedCount, wantedUnit.WoodCost * wantedCount);
        }

        //This method checks if player's army already has an unit group for a specific unit
        private bool UnitGroupExists(int unitId, ICollection<UnitGroup> playerUnitGroups, out UnitGroup? appropriateUnitGroup)
        {
            bool found = false;
            appropriateUnitGroup = null;
            foreach (var unitGroup in playerUnitGroups)
            {
                //Player already has unit group of this type
                if (unitGroup.Unit.Id == unitId)
                {
                    found = true;
                    appropriateUnitGroup = unitGroup;
                    break;
                }
            }
            return found;
        }

        //Method returns true if the player won the fight. False otherwise.
        private bool SimulateFight(ICollection<UnitGroup> playerUnitGroups, ICollection<UnitGroup> enemyUnitGroups)
        {
            int playerPower = ComputePower(playerUnitGroups);
            int enemyPower = ComputePower(enemyUnitGroups);

            if(playerPower <= enemyPower)
            {
                foreach (var unitGroup in playerUnitGroups)
                {
                    unitGroup.UnitCount = 0;
                }
                return false;
            }
            else
            {
                enemyUnitGroups.Clear();
                double survivedUnitsPercentage = (double) playerPower / (5 * enemyPower);
                if (survivedUnitsPercentage > 1)
                {
                    survivedUnitsPercentage = 1;
                }

                foreach (var unitGroup in playerUnitGroups)
                {
                    var newUnitCount = unitGroup.UnitCount * survivedUnitsPercentage;
                    unitGroup.UnitCount = (int) newUnitCount;
                }

                return true;
            }
        }

        private int ComputePower(ICollection<UnitGroup> unitGroups)
        {
            int totalPower = 0;
            foreach(var unitGroup in unitGroups)
            {
                totalPower += unitGroup.Unit.Power * unitGroup.UnitCount;
            }
            return totalPower;
        }
    }
}
