using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldenwood.Utilities
{
    public class Constants
    {
        public static readonly int PlayerId = 1;
        public static readonly int PlayerArmyId = 1;
        public static readonly int TickInterval = 10;
        public static readonly int DefeatedEnemyGoldIncome = 100;
        public static readonly int DefeatedEnemyWoodIncome = 50;

        public static readonly string GoldMineBuildingName = "Gold mine";
        public static readonly string LoggingCampBuildingName = "Logging camp";
        public static readonly string MayorBuildingName = "Mayor's house";
        public static readonly string ChurchBuildingName = "Church";
        public static readonly string WellBuildingName = "Well";
        public static readonly string BakeryBuildingName = "Bakery";
        public static readonly string FarmBuildingName = "Farm";


        public static readonly string FirstEnemyName = "Green Enemy";
        public static readonly string SecondEnemyName = "Yellow Enemy";
        public static readonly string ThirdEnemyName = "Orange Enemy";
        public static readonly string FourthEnemyName = "Red Enemy";
        public static readonly string FifthEnemyName = "Black Enemy";


        public static readonly string FirstUnitName = "Spearman";
        public static readonly string SecondUnitName = "Armoured Spearman";
        public static readonly string ThirdUnitName = "Archer";
        public static readonly string FourthUnitName = "Crossbowman";
        public static readonly string FifthUnitName = "Horseman";
        public static readonly string SixthUnitName = "Heavy horseman";


        public static readonly string BarracksBuildingName = "Barracks";
        public static readonly string ArcheryRangeBuildingName = "Archery range";
        public static readonly string StablesBuildingName = "Stables";

        public static readonly List<string> UnitNames = new List<string> { "Spearmen", "Armoured Spearmen", "Archers", "Crossbowmen", "Horsemen", "Heavy Horsemen" };
        public static readonly List<string> EnemyNames = new List<string> { FirstEnemyName, SecondEnemyName, ThirdEnemyName, FourthEnemyName, FifthEnemyName };
    }
}
