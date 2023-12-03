using Goldenwood.Utilities;
using GoldenwoodClient.ExternalApis;
using GoldenwoodClient.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenwoodClient.Utilities
{
    public class AlertManager
    {
        public async Task<bool> SendBuildingQuestionAlert(string buildingName, int nextBuildingLevel, ResourcesRecord neededResources)
        {
            string question = CreateUpgradeQuestion(buildingName, nextBuildingLevel, neededResources);
            return await Application.Current.MainPage.DisplayAlert("Build or upgrade", question, "Yes", "No");
        }

        public async Task<string> SendRecruitAlert(string unitName, ResourcesRecord neededResources)
        {
            string question = "How many " + unitName + " do you want to recruit?\n";
            question += "1 " + unitName + " costs " + neededResources.GoldAmount + " gold and " + neededResources.WoodAmount + " wood.\n";
            return await Application.Current.MainPage.DisplayPromptAsync("Recruitment", question);
        }

        public async Task<bool> SendFightQuestionAlert(string enemyName, ICollection<Goldenwood.Model.Units.UnitGroup> enemyUnitGroups)
        {
            string question = CreateFightQuestion(enemyName, enemyUnitGroups);
            return await Application.Current.MainPage.DisplayAlert("Fight an enemy", question, "Yes", "No");
        }

        private string CreateUpgradeQuestion(string buildingName, int nextBuildingLevel, ResourcesRecord neededResources)
        {
            string questionString = "Do you really want to ";
            if (nextBuildingLevel == 1)
            {
                questionString += "build a " + buildingName + " ";
            }
            else
            {
                questionString += "upgrade building " + buildingName + " to level " + nextBuildingLevel + " ";
            }
            questionString += "for " + neededResources.GoldAmount + " gold and " + neededResources.WoodAmount + " wood?";

            return questionString;
        }

        private string CreateFightQuestion(string enemyName, ICollection<Goldenwood.Model.Units.UnitGroup> enemyUnitGroups)
        {
            string question = "Are you sure you want to fight " + enemyName + "?\n";
            question += "Their army looks like this:\n";
            foreach (var unitGroup in enemyUnitGroups)
            {
                question += unitGroup.Unit.Name + ": ";
                question += unitGroup.UnitCount;
                question += "\n";
            }
            return question;
        }

        public void SendNotEnoughResourcesAlert()
        {
            Application.Current.MainPage.DisplayAlert("Not enough resources", "You don't have enough resources.", "OK");
        }

        public void SendBuildingSuccessfulAlert()
        {
            Application.Current.MainPage.DisplayAlert("Success", "Building built successfully.", "OK");
        }

        public void SendBuildingFailureAlert()
        {
            Application.Current.MainPage.DisplayAlert("Failure", "The building couldn't be built or upgraded.", "OK");
        }

        public void SendInvalidNumberAlert()
        {
            Application.Current.MainPage.DisplayAlert("Failure", "Invalid number entered.", "OK");
        }

        public void SendRecruitmentFailureAlert()
        {
            Application.Current.MainPage.DisplayAlert("Failure", "Failed to recruit units (possibly due to the lack of resources).", "OK");
        }

        public void SendRecruitmentSuccessAlert()
        {
            Application.Current.MainPage.DisplayAlert("Success", "Units recruited successfully.", "OK");
        }

        public void SendFightSuccessAlert()
        {
            Application.Current.MainPage.DisplayAlert("Success", "Congratulations! Enemy defeated!", "OK");
        }

        public void SendFightFailureAlert()
        {
            Application.Current.MainPage.DisplayAlert("Failure", "The enemy destroy your whole army!", "OK");
        }
    }
}
