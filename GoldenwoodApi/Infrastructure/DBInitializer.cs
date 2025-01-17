﻿using Goldenwood;
using Goldenwood.Model;
using Goldenwood.Model.Building;
using Goldenwood.Model.Units;
using Goldenwood.Utilities;

namespace GoldenwoodApi.Infrastructure
{
    /// <summary>
    /// This class seeds data into database. It provides some starting data when the application is launched.
    /// </summary>
    public static class DBInitializer
    {
        /// <summary>
        /// This method defines and inserts data into DB when the application is launched.
        /// </summary>
        /// <param name="applicationBuilder">Application builder instance</param>
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            var unitTypes = new List<Unit>
            {
                new Unit { Id = 1, Name = Constants.FirstUnitName, Power = 5, GoldCost = 30, WoodCost = 20 },
                new Unit { Id = 2, Name = Constants.SecondUnitName, Power = 10, GoldCost = 40, WoodCost = 30 },
                new Unit { Id = 3, Name = Constants.ThirdUnitName, Power = 35, GoldCost = 100, WoodCost = 75 },
                new Unit { Id = 4, Name = Constants.FourthUnitName, Power = 50, GoldCost = 120, WoodCost = 85 },
                new Unit { Id = 5, Name = Constants.FifthUnitName, Power = 100, GoldCost = 200, WoodCost = 100 },
                new Unit { Id = 6, Name = Constants.SixthUnitName, Power = 150, GoldCost = 250, WoodCost = 125 },
            };

            var unitGroups = new List<UnitGroup>
            {
                new UnitGroup { Id = 1, Unit = unitTypes[0], UnitCount = 20},
                new UnitGroup { Id = 2, Unit = unitTypes[0], UnitCount = 40},
                new UnitGroup { Id = 3, Unit = unitTypes[2], UnitCount = 10},
                new UnitGroup { Id = 4, Unit = unitTypes[0], UnitCount = 50},
                new UnitGroup { Id = 5, Unit = unitTypes[2], UnitCount = 20},
                new UnitGroup { Id = 6, Unit = unitTypes[4], UnitCount = 10},
                new UnitGroup { Id = 7, Unit = unitTypes[0], UnitCount = 100},
                new UnitGroup { Id = 8, Unit = unitTypes[2], UnitCount = 50},
                new UnitGroup { Id = 9, Unit = unitTypes[4], UnitCount = 20},
                new UnitGroup { Id = 10, Unit = unitTypes[0], UnitCount = 200},
                new UnitGroup { Id = 11, Unit = unitTypes[2], UnitCount = 100},
                new UnitGroup { Id = 12, Unit = unitTypes[4], UnitCount = 100},
                new UnitGroup { Id = 13, Unit = unitTypes[5], UnitCount = 20},
                new UnitGroup { Id = 14, Unit = unitTypes[0], UnitCount = 0},
                new UnitGroup { Id = 15, Unit = unitTypes[1], UnitCount = 0},
                new UnitGroup { Id = 16, Unit = unitTypes[2], UnitCount = 0},
                new UnitGroup { Id = 17, Unit = unitTypes[3], UnitCount = 0},
                new UnitGroup { Id = 18, Unit = unitTypes[4], UnitCount = 0},
                new UnitGroup { Id = 19, Unit = unitTypes[5], UnitCount = 0},
            };

            var armies = new List<Army>
            {
                new Army { Id = 1, UnitGroups = unitGroups.Where(x => x.Id == 14 || x.Id == 15 || x.Id == 16 || x.Id == 17 || x.Id == 18 || x.Id == 19).ToList()},
                new Army { Id = 2, UnitGroups = unitGroups.Where(x => x.Id == 1).ToList()},
                new Army { Id = 3, UnitGroups = unitGroups.Where(x => x.Id == 2 || x.Id == 3).ToList()},
                new Army { Id = 4, UnitGroups = unitGroups.Where(x => x.Id == 4 || x.Id == 5 || x.Id == 6).ToList()},
                new Army { Id = 5, UnitGroups = unitGroups.Where(x => x.Id == 7 || x.Id == 8 || x.Id == 9).ToList()},
                new Army { Id = 6, UnitGroups = unitGroups.Where(x => x.Id == 10 || x.Id == 11 || x.Id == 12 || x.Id == 13).ToList()},

            };

            var enemies = new List<Enemy>
            {
                new Enemy { Id = 1, Name = Constants.FirstEnemyName, Alive = true, Army = armies[1]},
                new Enemy { Id = 2, Name = Constants.SecondEnemyName, Alive = true, Army = armies[2]},
                new Enemy { Id = 3, Name = Constants.ThirdEnemyName, Alive = true, Army = armies[3]},
                new Enemy { Id = 4, Name = Constants.FourthEnemyName, Alive = true, Army = armies[4]},
                new Enemy { Id = 5, Name = Constants.FifthEnemyName, Alive = true, Army = armies[5]},
            };

            var economicBuildings = new List<EconomicBuilding>
            {
                new EconomicBuilding { Id = 1, Name = Constants.GoldMineBuildingName, Level = 1, IsBuilt = true, GoldIncome = 20, WoodIncome = 0, TickReduction = 0, GoldCost = 0, WoodCost = 0 },
                new EconomicBuilding { Id = 2, Name = Constants.GoldMineBuildingName, Level = 2, IsBuilt = false, GoldIncome = 40, WoodIncome = 0, TickReduction = 0, GoldCost = 100, WoodCost = 50 },
                new EconomicBuilding { Id = 3, Name = Constants.GoldMineBuildingName, Level = 3, IsBuilt = false, GoldIncome = 75, WoodIncome = 0, TickReduction = 0, GoldCost = 350, WoodCost = 200 },
                new EconomicBuilding { Id = 4, Name = Constants.GoldMineBuildingName, Level = 4, IsBuilt = false, GoldIncome = 120, WoodIncome = 0, TickReduction = 0, GoldCost = 600, WoodCost = 400 },
                new EconomicBuilding { Id = 5, Name = Constants.GoldMineBuildingName, Level = 5, IsBuilt = false, GoldIncome = 200, WoodIncome = 0, TickReduction = 0, GoldCost = 1500, WoodCost = 750 },
                new EconomicBuilding { Id = 6, Name = Constants.LoggingCampBuildingName, Level = 1, IsBuilt = true, GoldIncome = 0, WoodIncome = 10, TickReduction = 0, GoldCost = 0, WoodCost = 0 },
                new EconomicBuilding { Id = 7, Name = Constants.LoggingCampBuildingName, Level = 2, IsBuilt = false, GoldIncome = 0, WoodIncome = 20, TickReduction = 0, GoldCost = 100, WoodCost = 50 },
                new EconomicBuilding { Id = 8, Name = Constants.LoggingCampBuildingName, Level = 3, IsBuilt = false, GoldIncome = 0, WoodIncome = 40, TickReduction = 0, GoldCost = 350, WoodCost = 200 },
                new EconomicBuilding { Id = 9, Name = Constants.LoggingCampBuildingName, Level = 4, IsBuilt = false, GoldIncome = 0, WoodIncome = 75, TickReduction = 0, GoldCost = 600, WoodCost = 400 },
                new EconomicBuilding { Id = 10, Name = Constants.LoggingCampBuildingName, Level = 5, IsBuilt = false, GoldIncome = 0, WoodIncome = 100, TickReduction = 0, GoldCost = 1500, WoodCost = 750 },
                new EconomicBuilding { Id = 11, Name = Constants.MayorBuildingName, Level = 1, IsBuilt = false, GoldIncome = 10, WoodIncome = 10, TickReduction = 0, GoldCost = 80, WoodCost = 30 },
                new EconomicBuilding { Id = 12, Name = Constants.MayorBuildingName, Level = 2, IsBuilt = false, GoldIncome = 15, WoodIncome = 15, TickReduction = 1, GoldCost = 300, WoodCost = 200 },
                new EconomicBuilding { Id = 13, Name = Constants.ChurchBuildingName, Level = 1, IsBuilt = false, GoldIncome = 15, WoodIncome = 5, TickReduction = 0, GoldCost = 120, WoodCost = 50 },
                new EconomicBuilding { Id = 14, Name = Constants.ChurchBuildingName, Level = 2, IsBuilt = false, GoldIncome = 30, WoodIncome = 10, TickReduction = 1, GoldCost = 350, WoodCost = 250 },
                new EconomicBuilding { Id = 15, Name = Constants.WellBuildingName, Level = 1, IsBuilt = false, GoldIncome = 5, WoodIncome = 15, TickReduction = 0, GoldCost = 150, WoodCost = 65 },
                new EconomicBuilding { Id = 16, Name = Constants.WellBuildingName, Level = 2, IsBuilt = false, GoldIncome = 10, WoodIncome = 25, TickReduction = 1, GoldCost = 400, WoodCost = 180 },
                new EconomicBuilding { Id = 17, Name = Constants.BakeryBuildingName, Level = 1, IsBuilt = false, GoldIncome = 15, WoodIncome = 15, TickReduction = 0, GoldCost = 250, WoodCost = 150 },
                new EconomicBuilding { Id = 18, Name = Constants.BakeryBuildingName, Level = 2, IsBuilt = false, GoldIncome = 25, WoodIncome = 25, TickReduction = 1, GoldCost = 400, WoodCost = 200 },
                new EconomicBuilding { Id = 19, Name = Constants.FarmBuildingName, Level = 1, IsBuilt = false, GoldIncome = 25, WoodIncome = 25, TickReduction = 0, GoldCost = 350, WoodCost = 200 },
                new EconomicBuilding { Id = 20, Name = Constants.FarmBuildingName, Level = 2, IsBuilt = false, GoldIncome = 35, WoodIncome = 35, TickReduction = 1, GoldCost = 500, WoodCost = 250 },
            };

            var militaryBuildings = new List<MilitaryBuilding>
            {
                new MilitaryBuilding { Id = 1, Name = Constants.BarracksBuildingName, Level = 1, CreatableUnit = unitTypes[0], IsBuilt = false, GoldCost = 100, WoodCost = 50 },
                new MilitaryBuilding { Id = 2, Name = Constants.BarracksBuildingName, Level = 2, CreatableUnit = unitTypes[1], IsBuilt = false, GoldCost = 200, WoodCost = 85 },
                new MilitaryBuilding { Id = 3, Name = Constants.ArcheryRangeBuildingName, Level = 1, CreatableUnit = unitTypes[2], IsBuilt = false, GoldCost = 400, WoodCost = 150 },
                new MilitaryBuilding { Id = 4, Name = Constants.ArcheryRangeBuildingName, Level = 2, CreatableUnit = unitTypes[3], IsBuilt = false, GoldCost = 600, WoodCost = 200 },
                new MilitaryBuilding { Id = 5, Name = Constants.StablesBuildingName, Level = 1, CreatableUnit = unitTypes[4], IsBuilt = false, GoldCost = 1000, WoodCost = 300 },
                new MilitaryBuilding { Id = 6, Name = Constants.StablesBuildingName, Level = 2, CreatableUnit = unitTypes[5], IsBuilt = false, GoldCost = 1500, WoodCost = 400 },
            };
            var player = new Player { Id = 1, GoldAmount = 500, WoodAmount = 250, Army = armies[0] };


            var context = applicationBuilder.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Unit.AddRange(unitTypes);
            context.UnitGroup.AddRange(unitGroups);
            context.Army.AddRange(armies);
            context.Enemy.AddRange(enemies);
            context.Player.Add(player);
            context.EconomicBuilding.AddRange(economicBuildings);
            context.MilitaryBuilding.AddRange(militaryBuildings);
            context.SaveChanges();
        }
    }
}
