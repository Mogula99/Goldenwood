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

        public async Task RecruitUnit(int unitId)
        {
            ICollection<UnitGroup> playerUnitGroups = await militaryApi.PlayerUnits();
            string unitName = Constants.UnitNames[unitId-1];
            string recruitNumberString = await alertManager.SendRecruitAlert(unitName);

            int count = -1;
            bool parseResult = int.TryParse(recruitNumberString, out count);
            if (parseResult == false || count < 0)
            {
                alertManager.SendInvalidNumberAlert();
                return;
            }

            ICollection<UnitGroup> newPlayerUnitGroups = await militaryApi.Recruit(unitId, count);
            CheckNewUnits(unitId, count, playerUnitGroups, newPlayerUnitGroups);
        }

        public async Task FightEnemy(int enemyId)
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
                }
                else
                {
                    alertManager.SendFightFailureAlert();
                }
            }
        }

        private void CheckNewUnits(int unitId, int recruitedUnits, ICollection<UnitGroup> playerUnitGroups, ICollection<UnitGroup> newPlayerUnitGroups)
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

            if(oldUnitsCount + recruitedUnits != newUnitsCount)
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
