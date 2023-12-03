using Goldenwood.Model.Units;
using Goldenwood.Utilities;
using GoldenwoodClient.ExternalApis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenwoodClient.Utilities
{
    public class MilitaryManager
    {
        private readonly IMilitaryApi militaryApi;

        private readonly AlertManager alertManager;

        public MilitaryManager(IMilitaryApi militaryApi, AlertManager alertManager)
        {
            this.militaryApi = militaryApi;
            this.alertManager = alertManager;
        }

        /// <summary>
        /// This method handles the recruitment of units. It checks if the recruitment can be finished and then makes appropriate api calls.
        /// </summary>
        /// <param name="unitId">ID of the unit type that is to be recruited</param>
        /// <returns>Nothing</returns>
        public async Task RecruitUnit(int unitId)
        {
            ICollection<UnitGroup> playerUnitGroups = await militaryApi.PlayerUnits();

            Unit recruitableUnit = await militaryApi.GetUnit(unitId);

            ResourcesRecord neededResources = new ResourcesRecord(recruitableUnit.GoldCost, recruitableUnit.WoodCost);

            string unitName = Constants.UnitNames[unitId - 1];
            string recruitNumberString = await alertManager.SendRecruitAlert(unitName, neededResources);
            if(recruitNumberString == null)
            {
                return;
            }

            int recruitCount = -1;
            bool parseResult = int.TryParse(recruitNumberString, out recruitCount);
            if (parseResult == false || recruitCount < 0)
            {
                alertManager.SendInvalidNumberAlert();
                return;
            }

            if(recruitCount == 0)
            {
                return;
            }

            bool doesHaveEnoughResources = await militaryApi.DoesHaveEnoughResources(unitId, recruitCount);
            if(!doesHaveEnoughResources)
            {
                alertManager.SendNotEnoughResourcesAlert();
                return;
            }

            ICollection<UnitGroup> newPlayerUnitGroups = await militaryApi.Recruit(unitId, recruitCount);
            CheckNewUnits(unitId, recruitCount, playerUnitGroups, newPlayerUnitGroups);
        }

        /// <summary>
        /// This method handles the fight with an enemy. It asks the player about the fight and then makes the appropriate api calls.
        /// </summary>
        /// <param name="enemyId">ID of the enemy selected for a fight</param>
        /// <returns>True if the player succeded in the fight. False otherwise.</returns>
        public async Task<bool> FightEnemy(int enemyId)
        {
            ICollection<UnitGroup> enemyUnitGroups = await militaryApi.EnemyUnits(enemyId);
            string enemyName = Constants.EnemyNames[enemyId-1];
            bool userChoice = await alertManager.SendFightQuestionAlert(enemyName, enemyUnitGroups);
            if(userChoice == true)
            {
                bool result = await militaryApi.Fight(enemyId);
                if(result == true)
                {
                    alertManager.SendFightSuccessAlert();
                    return true;
                }
                else
                {
                    alertManager.SendFightFailureAlert();
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// This method checks if the newly recruited units have been successfully added to the player's army.
        /// </summary>
        /// <param name="unitId">ID of the recruited units</param>
        /// <param name="recruitedUnitsCount">Number of recruited units</param>
        /// <param name="playerUnitGroups">Player's units before the recruitment</param>
        /// <param name="newPlayerUnitGroups">Player's units after the recruitment</param>
        private void CheckNewUnits(int unitId, int recruitedUnitsCount, ICollection<UnitGroup> playerUnitGroups, ICollection<UnitGroup> newPlayerUnitGroups)
        {
            int oldUnitsCount = -1;
            int newUnitsCount = -1;
            foreach (var unitGroup in playerUnitGroups)
            {
                if(unitGroup.Unit.Id == unitId)
                {
                    oldUnitsCount = unitGroup.UnitCount;
                    break;
                }
            }

            foreach (var unitGroup in newPlayerUnitGroups)
            {
                if (unitGroup.Unit.Id == unitId)
                {
                    newUnitsCount = unitGroup.UnitCount;
                    break;
                }
            }

            if(oldUnitsCount + recruitedUnitsCount != newUnitsCount)
            {
                alertManager.SendRecruitmentFailureAlert();
            }
            else
            {
                alertManager.SendRecruitmentSuccessAlert();
            }
        }
    }
}
