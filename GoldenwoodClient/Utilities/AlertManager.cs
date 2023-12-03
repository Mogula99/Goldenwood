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
        /// <summary>
        /// This method sends a question to the users about whether they want to build/upgrade a building
        /// </summary>
        /// <param name="buildingName">Name of the building to be built/upgraded</param>
        /// <param name="nextBuildingLevel">The next level of the built/upgraded building</param>
        /// <param name="neededResources">Resources needed for the building/upgrade</param>
        /// <returns>True if the player wants to build/upgrade building. False otherwise.</returns>
        public async Task<bool> SendBuildingQuestionAlert(string buildingName, int nextBuildingLevel, ResourcesRecord neededResources)
        {
            string question = CreateUpgradeQuestion(buildingName, nextBuildingLevel, neededResources);
            return await Application.Current.MainPage.DisplayAlert("Build or upgrade", question, "Yes", "No");
        }

        /// <summary>
        /// This method sends an alert to the player about how many of the specified units they want to recruit.
        /// </summary>
        /// <param name="unitName">Name of the unit to be recruited</param>
        /// <param name="neededResources">Resources needed to build 1 unit of the specified unit type.</param>
        /// <returns>String representing the value entered by the player</returns>
        public async Task<string> SendRecruitAlert(string unitName, ResourcesRecord neededResources)
        {
            string question = "How many " + unitName + " do you want to recruit?\n";
            question += "1 " + unitName + " costs " + neededResources.GoldAmount + " gold and " + neededResources.WoodAmount + " wood.\n";
            return await Application.Current.MainPage.DisplayPromptAsync("Recruitment", question);
        }

        /// <summary>
        /// This method sends an alert to the player about whether they really want to fight the selected enemy or not.
        /// </summary>
        /// <param name="enemyName">Name of the selected enemy</param>
        /// <param name="enemyUnitGroups">Unit's in the enemy's army</param>
        /// <returns>True if the player really wants to fight the enemy. False otherwise.</returns>
        public async Task<bool> SendFightQuestionAlert(string enemyName, ICollection<Goldenwood.Model.Units.UnitGroup> enemyUnitGroups)
        {
            string question = CreateFightQuestion(enemyName, enemyUnitGroups);
            return await Application.Current.MainPage.DisplayAlert("Fight an enemy", question, "Yes", "No");
        }

        /// <summary>
        /// This method creates the question string for the alert about whether the player really wants to build/upgrade a building.
        /// </summary>
        /// <param name="buildingName">Name of the building to be built/upgraded</param>
        /// <param name="nextBuildingLevel">Next level of the selected building</param>
        /// <param name="neededResources">Resources needed to build/upgrade the selected building</param>
        /// <returns>String representing the question for the user.</returns>
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

        /// <summary>
        /// This method creates the question that is displayed to the player when they want to fight an enemy
        /// </summary>
        /// <param name="enemyName">Name of the selected enemy</param>
        /// <param name="enemyUnitGroups">Units of the selected enemy</param>
        /// <returns>Question shown to the player before attacking the selected enemy</returns>
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

        /// <summary>
        /// This method sends an alert to the player that says that the player does not have enough resources to perform some action.
        /// </summary>
        public void SendNotEnoughResourcesAlert()
        {
            Application.Current.MainPage.DisplayAlert("Not enough resources", "You don't have enough resources.", "OK");
        }

        /// <summary>
        /// This method sends an alert to the player saying that some building has been sucessfully built.
        /// </summary>
        public void SendBuildingSuccessfulAlert()
        {
            Application.Current.MainPage.DisplayAlert("Success", "Building built successfully.", "OK");
        }

        /// <summary>
        /// This method sends an alert to the player saying that some building couldn't be built/upgraded for some reason.
        /// </summary>
        public void SendBuildingFailureAlert()
        {
            Application.Current.MainPage.DisplayAlert("Failure", "The building couldn't be built or upgraded.", "OK");
        }

        /// <summary>
        /// This methods sends an alert to the player saying that they entered an invalid number.
        /// </summary>
        public void SendInvalidNumberAlert()
        {
            Application.Current.MainPage.DisplayAlert("Failure", "Invalid number entered.", "OK");
        }

        /// <summary>
        /// This method sends an alert to the player saying that the recruitment of units couldn't be finished (for some reason).
        /// </summary>
        public void SendRecruitmentFailureAlert()
        {
            Application.Current.MainPage.DisplayAlert("Failure", "Failed to recruit units.", "OK");
        }

        /// <summary>
        /// This method sends an alert to the player saying that the recruitment of units was successful.
        /// </summary>
        public void SendRecruitmentSuccessAlert()
        {
            Application.Current.MainPage.DisplayAlert("Success", "Units recruited successfully.", "OK");
        }

        /// <summary>
        /// This method sends an alert to the player saying that they successfully won the fight with the enemy.
        /// </summary>
        public void SendFightSuccessAlert()
        {
            Application.Current.MainPage.DisplayAlert("Success", "Congratulations! Enemy defeated!", "OK");
        }

        /// <summary>
        /// This method sends an alert to the player saying that they lost the fight with the enemy.
        /// </summary>
        public void SendFightFailureAlert()
        {
            Application.Current.MainPage.DisplayAlert("Failure", "The enemy destroy your whole army!", "OK");
        }
    }
}
