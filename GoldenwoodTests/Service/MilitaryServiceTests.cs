using Goldenwood.Model.Building;
using Goldenwood.Model;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Goldenwood.Model.Units;
using Goldenwood.Utilities;
using Goldenwood.Service;
using Goldenwood;

namespace GoldenwoodTests.Service
{
    public class MilitaryServiceTests
    {
        [Fact]
        public void CanBeRecruitedTest()
        {
            //Asking for recruitment of unit that does not exist
            var dbContextMock = new Mock<ApplicationDbContext>();
            var unitsMock = GetUnitsMock();
            dbContextMock.Setup(m => m.Unit).Returns(unitsMock.Object);
            var militaryBuildingsMock = GetMilitaryBuildingsMock(unitsMock.Object.ToArray()[0], unitsMock.Object.ToArray()[1]);
            dbContextMock.Setup(m => m.MilitaryBuilding).Returns(militaryBuildingsMock.Object);

            var resourcesService = new ResourcesService(dbContextMock.Object);
            var buildingService = new BuildingService(dbContextMock.Object, resourcesService);
            var militaryService = new MilitaryService(dbContextMock.Object, resourcesService, buildingService);

            Assert.False(militaryService.CanBeRecruited(20));
        }

        [Fact]
        public void CanBeRecruitedTest2()
        {
            //Asking for recruitment of unit that can't be recruited
            var dbContextMock = new Mock<ApplicationDbContext>();
            var unitsMock = GetUnitsMock();
            dbContextMock.Setup(m => m.Unit).Returns(unitsMock.Object);
            var militaryBuildingsMock = GetMilitaryBuildingsMock(unitsMock.Object.ToArray()[0], unitsMock.Object.ToArray()[1]);
            dbContextMock.Setup(m => m.MilitaryBuilding).Returns(militaryBuildingsMock.Object);

            var resourcesService = new ResourcesService(dbContextMock.Object);
            var buildingService = new BuildingService(dbContextMock.Object, resourcesService);
            var militaryService = new MilitaryService(dbContextMock.Object, resourcesService, buildingService);

            Assert.False(militaryService.CanBeRecruited(3));
        }

        [Fact]
        public void CanBeRecruitedTest3()
        {
            //Asking for recruitment of unit that can be recruited
            var dbContextMock = new Mock<ApplicationDbContext>();
            var unitsMock = GetUnitsMock();
            dbContextMock.Setup(m => m.Unit).Returns(unitsMock.Object);
            var militaryBuildingsMock = GetMilitaryBuildingsMock(unitsMock.Object.ToArray()[0], unitsMock.Object.ToArray()[1]);
            dbContextMock.Setup(m => m.MilitaryBuilding).Returns(militaryBuildingsMock.Object);

            var resourcesService = new ResourcesService(dbContextMock.Object);
            var buildingService = new BuildingService(dbContextMock.Object, resourcesService);
            var militaryService = new MilitaryService(dbContextMock.Object, resourcesService, buildingService);

            Assert.True(militaryService.CanBeRecruited(1));
        }

        [Fact]
        public void RecruitUnitsTest()
        {
            //Recruit non-existing unit
            var dbContextMock = new Mock<ApplicationDbContext>();
            dbContextMock.Setup(m => m.Unit).Returns(GetUnitsMock().Object);
            var weakPlayerMock = GetWeakPlayerMock();
            dbContextMock.Setup(m => m.Player).Returns(weakPlayerMock.Object);

            var resourcesService = new ResourcesService(dbContextMock.Object);
            var buildingService = new BuildingService(dbContextMock.Object, resourcesService);
            var militaryService = new MilitaryService(dbContextMock.Object, resourcesService, buildingService);

            militaryService.RecruitUnits(666, 150);
            Assert.Equal(2, weakPlayerMock.Object.FirstOrDefault().Army.UnitGroups.Count);
            Assert.Equal(0, weakPlayerMock.Object.FirstOrDefault().Army.UnitGroups.Sum(x => x.UnitCount));
            Assert.Equal(1000, weakPlayerMock.Object.FirstOrDefault().GoldAmount);
            Assert.Equal(1000, weakPlayerMock.Object.FirstOrDefault().WoodAmount);
            dbContextMock.Verify(m => m.SaveChanges(), Times.Never());
        }

        [Fact]
        public void RecruitUnitsTest2()
        {
            //Not enough resources to recruit
            var dbContextMock = new Mock<ApplicationDbContext>();
            dbContextMock.Setup(m => m.Unit).Returns(GetUnitsMock().Object);
            var weakPlayerMock = GetWeakPlayerMock();
            dbContextMock.Setup(m => m.Player).Returns(weakPlayerMock.Object);

            var resourcesService = new ResourcesService(dbContextMock.Object);
            var buildingService = new BuildingService(dbContextMock.Object, resourcesService);
            var militaryService = new MilitaryService(dbContextMock.Object, resourcesService, buildingService);

            militaryService.RecruitUnits(1, 1000);
            Assert.Equal(2, weakPlayerMock.Object.FirstOrDefault().Army.UnitGroups.Count);
            Assert.Equal(0, weakPlayerMock.Object.FirstOrDefault().Army.UnitGroups.Sum(x => x.UnitCount));
            Assert.Equal(1000, weakPlayerMock.Object.FirstOrDefault().GoldAmount);
            Assert.Equal(1000, weakPlayerMock.Object.FirstOrDefault().WoodAmount);
            dbContextMock.Verify(m => m.SaveChanges(), Times.Never());
        }

        [Fact]
        public void RecruitUnitsTest3()
        {
            //Success when recruiting unit from existing unit group
            var dbContextMock = new Mock<ApplicationDbContext>();
            dbContextMock.Setup(m => m.Unit).Returns(GetUnitsMock().Object);
            var weakPlayerMock = GetWeakPlayerMock();
            dbContextMock.Setup(m => m.Player).Returns(weakPlayerMock.Object);
            dbContextMock.Setup(m => m.Army).Returns(GetArmiesMock(weakPlayerMock, GetEnemiesMock()).Object);

            var resourcesService = new ResourcesService(dbContextMock.Object);
            var buildingService = new BuildingService(dbContextMock.Object, resourcesService);
            var militaryService = new MilitaryService(dbContextMock.Object, resourcesService, buildingService);

            militaryService.RecruitUnits(1, 10);
            Assert.Equal(2, weakPlayerMock.Object.FirstOrDefault().Army.UnitGroups.Count);
            Assert.Equal(10, weakPlayerMock.Object.FirstOrDefault().Army.UnitGroups.Sum(x => x.UnitCount));
            Assert.Equal(10, weakPlayerMock.Object.FirstOrDefault().Army.UnitGroups.ToArray()[0].UnitCount);
            Assert.Equal("Spearman", weakPlayerMock.Object.FirstOrDefault().Army.UnitGroups.ToArray()[0].Unit.Name);
            Assert.Equal(900, weakPlayerMock.Object.FirstOrDefault().GoldAmount);
            Assert.Equal(900, weakPlayerMock.Object.FirstOrDefault().WoodAmount);
            dbContextMock.Verify(m => m.SaveChanges(), Times.Exactly(2));
        }

        [Fact]
        public void RecruitUnitsTest4()
        {
            //Success when recruiting unit and creating new unit group
            var dbContextMock = new Mock<ApplicationDbContext>();
            dbContextMock.Setup(m => m.Unit).Returns(GetUnitsMock().Object);
            var weakPlayerMock = GetWeakPlayerMock();
            dbContextMock.Setup(m => m.Player).Returns(weakPlayerMock.Object);
            dbContextMock.Setup(m => m.Army).Returns(GetArmiesMock(weakPlayerMock, GetEnemiesMock()).Object);

            var resourcesService = new ResourcesService(dbContextMock.Object);
            var buildingService = new BuildingService(dbContextMock.Object, resourcesService);
            var militaryService = new MilitaryService(dbContextMock.Object, resourcesService, buildingService);

            Assert.Equal(2, weakPlayerMock.Object.FirstOrDefault().Army.UnitGroups.Count);
            militaryService.RecruitUnits(5, 1);
            Assert.Equal(3, weakPlayerMock.Object.FirstOrDefault().Army.UnitGroups.Count);
            Assert.Equal(1, weakPlayerMock.Object.FirstOrDefault().Army.UnitGroups.Sum(x => x.UnitCount));
            Assert.Equal(1, weakPlayerMock.Object.FirstOrDefault().Army.UnitGroups.ToArray()[2].UnitCount);
            Assert.Equal("Horseman", weakPlayerMock.Object.FirstOrDefault().Army.UnitGroups.ToArray()[2].Unit.Name);
            Assert.Equal(0, weakPlayerMock.Object.FirstOrDefault().GoldAmount);
            Assert.Equal(0, weakPlayerMock.Object.FirstOrDefault().WoodAmount);
            dbContextMock.Verify(m => m.SaveChanges(), Times.Exactly(2));
        }


        [Fact]
        public void FightTest()
        {
            //Player has too weak army
            var dbContextMock = new Mock<ApplicationDbContext>();
            var enemyMock = GetEnemiesMock();
            dbContextMock.Setup(m => m.Enemy).Returns(enemyMock.Object);
            var weakPlayerMock = GetWeakPlayerMock();
            dbContextMock.Setup(m => m.Player).Returns(weakPlayerMock.Object);
            dbContextMock.Setup(m => m.Army).Returns(GetArmiesMock(weakPlayerMock, enemyMock).Object);


            var resourcesService = new ResourcesService(dbContextMock.Object);
            var buildingService = new BuildingService(dbContextMock.Object, resourcesService);
            var militaryService = new MilitaryService(dbContextMock.Object, resourcesService, buildingService);

            var result = militaryService.Fight(1);
            Assert.False(result);
            Assert.True(enemyMock.Object.FirstOrDefault().Alive);
            Assert.Equal(0, weakPlayerMock.Object.FirstOrDefault().Army.UnitGroups.Count);
            Assert.Equal(2, enemyMock.Object.FirstOrDefault().Army.UnitGroups.Count);
            Assert.Equal(50, enemyMock.Object.FirstOrDefault().Army.UnitGroups.ToArray()[0].UnitCount);
            Assert.True(enemyMock.Object.FirstOrDefault().Army.UnitGroups.ToArray()[0].Unit.Name == "Spearman");
            Assert.Equal(5, enemyMock.Object.FirstOrDefault().Army.UnitGroups.ToArray()[1].UnitCount);
            Assert.True(enemyMock.Object.FirstOrDefault().Army.UnitGroups.ToArray()[1].Unit.Name == "Archer");
        }

        [Fact]
        public void FightTest2()
        {
            //Player's army is as powerful as enemy's
            var dbContextMock = new Mock<ApplicationDbContext>();
            var enemyMock = GetEnemiesMock();
            dbContextMock.Setup(m => m.Enemy).Returns(enemyMock.Object);
            var mediocrePlayerMock = GetMediocrePlayerMock();
            dbContextMock.Setup(m => m.Player).Returns(mediocrePlayerMock.Object);
            dbContextMock.Setup(m => m.Army).Returns(GetArmiesMock(mediocrePlayerMock, enemyMock).Object);


            var resourcesService = new ResourcesService(dbContextMock.Object);
            var buildingService = new BuildingService(dbContextMock.Object, resourcesService);
            var militaryService = new MilitaryService(dbContextMock.Object, resourcesService, buildingService);

            var result = militaryService.Fight(1);
            Assert.False(result);
            Assert.True(enemyMock.Object.FirstOrDefault().Alive);
            Assert.Equal(0, mediocrePlayerMock.Object.FirstOrDefault().Army.UnitGroups.Count);
            Assert.Equal(2, enemyMock.Object.FirstOrDefault().Army.UnitGroups.Count);
            Assert.Equal(50, enemyMock.Object.FirstOrDefault().Army.UnitGroups.ToArray()[0].UnitCount);
            Assert.True(enemyMock.Object.FirstOrDefault().Army.UnitGroups.ToArray()[0].Unit.Name == "Spearman");
            Assert.Equal(5, enemyMock.Object.FirstOrDefault().Army.UnitGroups.ToArray()[1].UnitCount);
            Assert.True(enemyMock.Object.FirstOrDefault().Army.UnitGroups.ToArray()[1].Unit.Name == "Archer");
        }

        [Fact]
        public void FightTest3()
        {
            //Player is more powerful than enemy
            var dbContextMock = new Mock<ApplicationDbContext>();
            var enemyMock = GetEnemiesMock();
            dbContextMock.Setup(m => m.Enemy).Returns(enemyMock.Object);
            var strongPlayerMock = GetStrongPlayerMock();
            dbContextMock.Setup(m => m.Player).Returns(strongPlayerMock.Object);
            dbContextMock.Setup(m => m.Army).Returns(GetArmiesMock(strongPlayerMock, enemyMock).Object);


            var resourcesService = new ResourcesService(dbContextMock.Object);
            var buildingService = new BuildingService(dbContextMock.Object, resourcesService);
            var militaryService = new MilitaryService(dbContextMock.Object, resourcesService, buildingService);

            var result = militaryService.Fight(1);
            Assert.True(result);
            Assert.False(enemyMock.Object.FirstOrDefault().Alive);
            Assert.Equal(2, strongPlayerMock.Object.FirstOrDefault().Army.UnitGroups.Count);
            Assert.Equal(40, strongPlayerMock.Object.FirstOrDefault().Army.UnitGroups.ToArray()[0].UnitCount);
            Assert.True(strongPlayerMock.Object.FirstOrDefault().Army.UnitGroups.ToArray()[0].Unit.Name == "Spearman");
            Assert.Equal(4, strongPlayerMock.Object.FirstOrDefault().Army.UnitGroups.ToArray()[1].UnitCount);
            Assert.True(strongPlayerMock.Object.FirstOrDefault().Army.UnitGroups.ToArray()[1].Unit.Name == "Archer");
        }



        //Helper methods

        private Mock<DbSet<Unit>> GetUnitsMock()
        {
            var data = new List<Unit>
            {
                new Unit { Id = 1, Name = "Spearman", Power = 10, GoldCost = 10, WoodCost = 10},
                new Unit { Id = 2, Name = "Archer", Power = 100, GoldCost = 100, WoodCost = 100},
                new Unit { Id = 5, Name = "Horseman", Power = 1000, GoldCost = 1000, WoodCost = 1000},

            }.AsQueryable();

            var mockSet = new Mock<DbSet<Unit>>();
            mockSet.As<IQueryable<Unit>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Unit>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Unit>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Unit>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            return mockSet;
        }

        private Mock<DbSet<Player>> GetWeakPlayerMock()
        {
            var spearmanUnit = new Unit { Id = 1, GoldCost = 0, Name = "Spearman", Power = 10, WoodCost = 0 };
            var archerUnit = new Unit { Id = 2, GoldCost = 0, Name = "Archer", Power = 100, WoodCost = 0 };
            var playerSpearmanUnitGroup = new UnitGroup { Id = 1, Unit = spearmanUnit, UnitCount = 0 };
            var playerArcherUnitGroup = new UnitGroup { Id = 2, Unit = archerUnit, UnitCount = 0 };
            var playerUnitGroups = new List<UnitGroup>() { playerSpearmanUnitGroup, playerArcherUnitGroup };
            var playerArmy = new Army { Id = Constants.PlayerArmyId, UnitGroups = playerUnitGroups };

            var data = new List<Player>
            {
                new Player { Id = Constants.PlayerId, Army = playerArmy, GoldAmount = 1000, WoodAmount = 1000},
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Player>>();
            mockSet.As<IQueryable<Player>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Player>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Player>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Player>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            return mockSet;
        }

        private Mock<DbSet<Player>> GetMediocrePlayerMock()
        {
            var spearmanUnit = new Unit { Id = 1, GoldCost = 0, Name = "Spearman", Power = 10, WoodCost = 0 };
            var archerUnit = new Unit { Id = 2, GoldCost = 0, Name = "Archer", Power = 100, WoodCost = 0 };
            var playerSpearmanUnitGroup = new UnitGroup { Id = 1, Unit = spearmanUnit, UnitCount = 50 };
            var playerArcherUnitGroup = new UnitGroup { Id = 2, Unit = archerUnit, UnitCount = 5 };
            var playerUnitGroups = new List<UnitGroup>() { playerSpearmanUnitGroup, playerArcherUnitGroup };
            var playerArmy = new Army { Id = Constants.PlayerArmyId, UnitGroups = playerUnitGroups };

            var data = new List<Player>
            {
                new Player { Id = Constants.PlayerId, Army = playerArmy, GoldAmount = 123, WoodAmount = 45},
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Player>>();
            mockSet.As<IQueryable<Player>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Player>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Player>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Player>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            return mockSet;
        }

        private Mock<DbSet<Player>> GetStrongPlayerMock()
        {
            var spearmanUnit = new Unit { Id = 1, GoldCost = 0, Name = "Spearman", Power = 10, WoodCost = 0 };
            var archerUnit = new Unit { Id = 2, GoldCost = 0, Name = "Archer", Power = 100, WoodCost = 0 };
            var playerSpearmanUnitGroup = new UnitGroup { Id = 1, Unit = spearmanUnit, UnitCount = 100 };
            var playerArcherUnitGroup = new UnitGroup { Id = 2, Unit = archerUnit, UnitCount = 10 };
            var playerUnitGroups = new List<UnitGroup>() { playerSpearmanUnitGroup, playerArcherUnitGroup };
            var playerArmy = new Army { Id = Constants.PlayerArmyId, UnitGroups = playerUnitGroups };

            var data = new List<Player>
            {
                new Player { Id = Constants.PlayerId, Army = playerArmy, GoldAmount = 123, WoodAmount = 45},
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Player>>();
            mockSet.As<IQueryable<Player>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Player>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Player>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Player>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            return mockSet;
        }

        private Mock<DbSet<MilitaryBuilding>> GetMilitaryBuildingsMock(Unit first, Unit second)
        {
            var data = new List<MilitaryBuilding>
            {
                new MilitaryBuilding { Id = 1, CreatableUnit = first, GoldCost = 150, IsBuilt = true, Level = 500, Name = "Barracks", WoodCost = 0},
                new MilitaryBuilding { Id = 2, CreatableUnit = second, GoldCost = 50, IsBuilt = false, Level = 0, Name = "Shooting range", WoodCost = -520},
            }.AsQueryable();

            var mockSet = new Mock<DbSet<MilitaryBuilding>>();
            mockSet.As<IQueryable<MilitaryBuilding>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<MilitaryBuilding>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<MilitaryBuilding>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<MilitaryBuilding>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            return mockSet;
        }

        private Mock<DbSet<Enemy>> GetEnemiesMock()
        {
            var spearmanUnit = new Unit { Id = 1, GoldCost = 0, Name = "Spearman", Power = 10, WoodCost = 0 };
            var archerUnit = new Unit { Id = 2, GoldCost = 0, Name = "Archer", Power = 100, WoodCost = 0 };
            var enemySpearmanUnitGroup = new UnitGroup { Id = 1, Unit = spearmanUnit, UnitCount = 50 };
            var enemyArcherUnitGroup = new UnitGroup { Id = 1, Unit = archerUnit, UnitCount = 5 };
            var enemyUnitGroups = new List<UnitGroup>() { enemySpearmanUnitGroup, enemyArcherUnitGroup };
            var enemyArmy = new Army { Id = 2, UnitGroups = enemyUnitGroups };

            var data = new List<Enemy>
            {
                new Enemy { Id = 1, Name = "Prague", Alive = true, Army = enemyArmy }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Enemy>>();
            mockSet.As<IQueryable<Enemy>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Enemy>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Enemy>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Enemy>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            return mockSet;
        }

        private Mock<DbSet<Army>> GetArmiesMock(Mock<DbSet<Player>> playerMock, Mock<DbSet<Enemy>> enemyMock)
        {
            var data = new List<Army>
            {
                playerMock.Object.FirstOrDefault().Army,
                enemyMock.Object.FirstOrDefault().Army
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Army>>();
            mockSet.As<IQueryable<Army>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Army>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Army>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Army>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            return mockSet;
        }
    }
}
