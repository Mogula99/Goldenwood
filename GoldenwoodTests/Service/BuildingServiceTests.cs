using Goldenwood;
using Goldenwood.Model;
using Goldenwood.Model.Building;
using Goldenwood.Service;
using Goldenwood.Utilities;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenwoodTests.Service
{
    public class BuildingServiceTests
    {
        [Fact]
        public void GetBuiltMilitaryBuildingsTest()
        {
            //Just test all the built military buildings
            var dbContextMock = new Mock<ApplicationDbContext>();
            dbContextMock.Setup(m => m.MilitaryBuilding).Returns(GetMilitaryBuildingsMock().Object);

            var resourcesService = new ResourcesService(dbContextMock.Object);
            var buildingService = new BuildingService(dbContextMock.Object, resourcesService);

            var result = buildingService.GetBuiltMilitaryBuildings();
            Assert.Equal(3, result.Count());
            Assert.Equal(2, result.ToArray()[0].Id);
            Assert.Equal(1, result.ToArray()[1].Id);
            Assert.Equal(3, result.ToArray()[2].Id);
        }

        [Fact]
        public void GetMaxBuiltBuildingLevelTest()
        {
            //Asking for a level of military building
            var dbContextMock = new Mock<ApplicationDbContext>();
            dbContextMock.Setup(m => m.MilitaryBuilding).Returns(GetMilitaryBuildingsMock().Object);
            dbContextMock.Setup(m => m.EconomicBuilding).Returns(GetEconomicBuildingsMock().Object);

            var resourcesService = new ResourcesService(dbContextMock.Object);
            var buildingService = new BuildingService(dbContextMock.Object, resourcesService);

            var result = buildingService.GetMaxBuiltBuildingLevel("A");
            Assert.Equal(100, result);
        }

        [Fact]
        public void GetMaxBuiltBuildingLevelTest2()
        {
            //Asking for a level of economic building
            var dbContextMock = new Mock<ApplicationDbContext>();
            dbContextMock.Setup(m => m.MilitaryBuilding).Returns(GetMilitaryBuildingsMock().Object);
            dbContextMock.Setup(m => m.EconomicBuilding).Returns(GetEconomicBuildingsMock().Object);

            var resourcesService = new ResourcesService(dbContextMock.Object);
            var buildingService = new BuildingService(dbContextMock.Object, resourcesService);

            var result = buildingService.GetMaxBuiltBuildingLevel("Dlog Enim");
            Assert.Equal(762, result);
        }

        [Fact]
        public void GetMaxBuiltBuildingLevelTest3()
        {
            //Asking for a level of military building that is not built
            var dbContextMock = new Mock<ApplicationDbContext>();
            dbContextMock.Setup(m => m.MilitaryBuilding).Returns(GetMilitaryBuildingsMock().Object);
            dbContextMock.Setup(m => m.EconomicBuilding).Returns(GetEconomicBuildingsMock().Object);

            var resourcesService = new ResourcesService(dbContextMock.Object);
            var buildingService = new BuildingService(dbContextMock.Object, resourcesService);

            var result = buildingService.GetMaxBuiltBuildingLevel("B");
            Assert.Equal(0, result);
        }

        [Fact]
        public void GetMaxBuiltBuildingLevelTest4()
        {
            //Asking for a level of building that does not exist
            var dbContextMock = new Mock<ApplicationDbContext>();
            dbContextMock.Setup(m => m.MilitaryBuilding).Returns(GetMilitaryBuildingsMock().Object);
            dbContextMock.Setup(m => m.EconomicBuilding).Returns(GetEconomicBuildingsMock().Object);

            var resourcesService = new ResourcesService(dbContextMock.Object);
            var buildingService = new BuildingService(dbContextMock.Object, resourcesService);

            var result = buildingService.GetMaxBuiltBuildingLevel("Name of a building that does not exist");
            Assert.Equal(-1, result);
        }

        [Fact]
        public void GetBuildingTest()
        {
            //Asking for a military building
            var dbContextMock = new Mock<ApplicationDbContext>();
            dbContextMock.Setup(m => m.MilitaryBuilding).Returns(GetMilitaryBuildingsMock().Object);
            dbContextMock.Setup(m => m.EconomicBuilding).Returns(GetEconomicBuildingsMock().Object);

            var resourcesService = new ResourcesService(dbContextMock.Object);
            var buildingService = new BuildingService(dbContextMock.Object, resourcesService);

            var result = buildingService.GetBuilding("A", 100);
            Assert.NotNull(result);
            Assert.Equal(3, result.Id);
        }

        [Fact]
        public void GetBuildingTest2()
        {
            //Asking for an economic building
            var dbContextMock = new Mock<ApplicationDbContext>();
            dbContextMock.Setup(m => m.MilitaryBuilding).Returns(GetMilitaryBuildingsMock().Object);
            dbContextMock.Setup(m => m.EconomicBuilding).Returns(GetEconomicBuildingsMock().Object);

            var resourcesService = new ResourcesService(dbContextMock.Object);
            var buildingService = new BuildingService(dbContextMock.Object, resourcesService);

            var result = buildingService.GetBuilding("Gold Mine", 547);
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public void GetBuildingTest3()
        {
            //Asking for a building that does not exist
            var dbContextMock = new Mock<ApplicationDbContext>();
            dbContextMock.Setup(m => m.MilitaryBuilding).Returns(GetMilitaryBuildingsMock().Object);
            dbContextMock.Setup(m => m.EconomicBuilding).Returns(GetEconomicBuildingsMock().Object);

            var resourcesService = new ResourcesService(dbContextMock.Object);
            var buildingService = new BuildingService(dbContextMock.Object, resourcesService);

            var result = buildingService.GetBuilding("Name of a building that does not exist", 0);
            Assert.Null(result);
        }

        [Fact]
        public void GetBuildingTest4()
        {
            //Asking for an economic building that exists, but its level is different
            var dbContextMock = new Mock<ApplicationDbContext>();
            dbContextMock.Setup(m => m.MilitaryBuilding).Returns(GetMilitaryBuildingsMock().Object);
            dbContextMock.Setup(m => m.EconomicBuilding).Returns(GetEconomicBuildingsMock().Object);

            var resourcesService = new ResourcesService(dbContextMock.Object);
            var buildingService = new BuildingService(dbContextMock.Object, resourcesService);

            var result = buildingService.GetBuilding("Gold Mine", -150);
            Assert.Null(result);
        }

        [Fact]
        public void GetNeededBuildingResourcesTest()
        {
            //Asking for needed resources of a military building
            var dbContextMock = new Mock<ApplicationDbContext>();
            dbContextMock.Setup(m => m.MilitaryBuilding).Returns(GetMilitaryBuildingsMock().Object);
            dbContextMock.Setup(m => m.EconomicBuilding).Returns(GetEconomicBuildingsMock().Object);

            var expectedResources = new ResourcesRecord(50000, 1222200);

            var resourcesService = new ResourcesService(dbContextMock.Object);
            var buildingService = new BuildingService(dbContextMock.Object, resourcesService);

            var result = buildingService.GetNeededBuildingResources("A", 100);
            Assert.NotNull(result);
            Assert.Equal(expectedResources.GoldAmount, result.GoldAmount);
            Assert.Equal(expectedResources.WoodAmount, result.WoodAmount);
        }

        [Fact]
        public void GetNeededBuildingResourcesTest2()
        {
            //Asking for needed resources of an economic building
            var dbContextMock = new Mock<ApplicationDbContext>();
            dbContextMock.Setup(m => m.MilitaryBuilding).Returns(GetMilitaryBuildingsMock().Object);
            dbContextMock.Setup(m => m.EconomicBuilding).Returns(GetEconomicBuildingsMock().Object);

            var expectedResources = new ResourcesRecord(7843, 12445);

            var resourcesService = new ResourcesService(dbContextMock.Object);
            var buildingService = new BuildingService(dbContextMock.Object, resourcesService);

            var result = buildingService.GetNeededBuildingResources("Dlog Enim", 762);
            Assert.NotNull(result);
            Assert.Equal(expectedResources.GoldAmount, result.GoldAmount);
            Assert.Equal(expectedResources.WoodAmount, result.WoodAmount);
        }

        [Fact]
        public void GetNeededBuildingResourcesTest3()
        {
            //Asking for needed resources of a building that does not exist
            var dbContextMock = new Mock<ApplicationDbContext>();
            dbContextMock.Setup(m => m.MilitaryBuilding).Returns(GetMilitaryBuildingsMock().Object);
            dbContextMock.Setup(m => m.EconomicBuilding).Returns(GetEconomicBuildingsMock().Object);

            var resourcesService = new ResourcesService(dbContextMock.Object);
            var buildingService = new BuildingService(dbContextMock.Object, resourcesService);

            var result = buildingService.GetNeededBuildingResources("Name of a building that does not exist", 762);
            Assert.Null(result);
        }

        [Fact]
        public void GetNeededBuildingResourcesTest4()
        {
            //Asking for needed resources of an economic building that exists but does not have a version with specified level
            var dbContextMock = new Mock<ApplicationDbContext>();
            dbContextMock.Setup(m => m.MilitaryBuilding).Returns(GetMilitaryBuildingsMock().Object);
            dbContextMock.Setup(m => m.EconomicBuilding).Returns(GetEconomicBuildingsMock().Object);

            var resourcesService = new ResourcesService(dbContextMock.Object);
            var buildingService = new BuildingService(dbContextMock.Object, resourcesService);

            var result = buildingService.GetNeededBuildingResources("Dlog Enim", -250);
            Assert.Null(result);
        }

        [Fact]
        public void IsBuildableTest()
        {
            //Asking if a military building is buildable
            var dbContextMock = new Mock<ApplicationDbContext>();
            dbContextMock.Setup(m => m.MilitaryBuilding).Returns(GetMilitaryBuildingsMock().Object);
            dbContextMock.Setup(m => m.EconomicBuilding).Returns(GetEconomicBuildingsMock().Object);

            var resourcesService = new ResourcesService(dbContextMock.Object);
            var buildingService = new BuildingService(dbContextMock.Object, resourcesService);

            var result = buildingService.IsBuildable("A");
            Assert.True(result);
        }

        [Fact]
        public void IsBuildableTest2()
        {
            //Asking if an economic building is buildable
            var dbContextMock = new Mock<ApplicationDbContext>();
            dbContextMock.Setup(m => m.MilitaryBuilding).Returns(GetMilitaryBuildingsMock().Object);
            dbContextMock.Setup(m => m.EconomicBuilding).Returns(GetEconomicBuildingsMock().Object);

            var resourcesService = new ResourcesService(dbContextMock.Object);
            var buildingService = new BuildingService(dbContextMock.Object, resourcesService);

            var result = buildingService.IsBuildable("Gold Mine");
            Assert.False(result);
        }

        [Fact]
        public void IsBuildableTest3()
        {
            //Asking if a building that does not exist is buildable
            var dbContextMock = new Mock<ApplicationDbContext>();
            dbContextMock.Setup(m => m.MilitaryBuilding).Returns(GetMilitaryBuildingsMock().Object);
            dbContextMock.Setup(m => m.EconomicBuilding).Returns(GetEconomicBuildingsMock().Object);

            var resourcesService = new ResourcesService(dbContextMock.Object);
            var buildingService = new BuildingService(dbContextMock.Object, resourcesService);

            var result = buildingService.IsBuildable("Building that does not exist in the database");
            Assert.False(result);
        }

        [Fact]
        public void BuildOrUpgradeTest()
        {
            //Success path
            var dbContextMock = new Mock<ApplicationDbContext>();
            dbContextMock.Setup(m => m.MilitaryBuilding).Returns(GetMilitaryBuildingsMock().Object);
            var economicMock = GetEconomicBuildingsMock();
            dbContextMock.Setup(m => m.EconomicBuilding).Returns(economicMock.Object);
            var playerMock = GetPlayerMock();
            dbContextMock.Setup(m => m.Player).Returns(playerMock.Object);

            var resourcesService = new ResourcesService(dbContextMock.Object);
            var buildingService = new BuildingService(dbContextMock.Object, resourcesService);

            buildingService.BuildOrUpgrade("B");
            Assert.True(economicMock.Object.Where(x => x.Name == "B").FirstOrDefault().IsBuilt == true);
            Assert.True(economicMock.Object.Where(x => x.Name == "B").Select(x => x.Level).Max() == 1);
            Assert.True(playerMock.Object.FirstOrDefault().GoldAmount == 0);
            Assert.True(playerMock.Object.FirstOrDefault().WoodAmount == 0);
            dbContextMock.Verify(m => m.SaveChanges(), Times.Exactly(2));
        }

        [Fact]
        public void BuildOrUpgradeTest2()
        {
            //Building does not exist
            var dbContextMock = new Mock<ApplicationDbContext>();
            dbContextMock.Setup(m => m.MilitaryBuilding).Returns(GetMilitaryBuildingsMock().Object);
            var economicMock = GetEconomicBuildingsMock();
            dbContextMock.Setup(m => m.EconomicBuilding).Returns(economicMock.Object);
            var playerMock = GetPlayerMock();
            dbContextMock.Setup(m => m.Player).Returns(playerMock.Object);

            var resourcesService = new ResourcesService(dbContextMock.Object);
            var buildingService = new BuildingService(dbContextMock.Object, resourcesService);

            buildingService.BuildOrUpgrade("Non-existing name of a building");
            Assert.True(playerMock.Object.FirstOrDefault().GoldAmount == 1);
            Assert.True(playerMock.Object.FirstOrDefault().WoodAmount == 2);
            dbContextMock.Verify(m => m.SaveChanges(), Times.Never());
        }

        [Fact]
        public void BuildOrUpgradeTest3()
        {
            //Not enough resources to upgrade the building
            var dbContextMock = new Mock<ApplicationDbContext>();
            dbContextMock.Setup(m => m.MilitaryBuilding).Returns(GetMilitaryBuildingsMock().Object);
            var economicMock = GetEconomicBuildingsMock();
            dbContextMock.Setup(m => m.EconomicBuilding).Returns(economicMock.Object);
            var playerMock = GetPlayerMock();
            dbContextMock.Setup(m => m.Player).Returns(playerMock.Object);

            var resourcesService = new ResourcesService(dbContextMock.Object);
            var buildingService = new BuildingService(dbContextMock.Object, resourcesService);

            buildingService.BuildOrUpgrade("A");
            Assert.True(playerMock.Object.FirstOrDefault().GoldAmount == 1);
            Assert.True(playerMock.Object.FirstOrDefault().WoodAmount == 2);
            dbContextMock.Verify(m => m.SaveChanges(), Times.Never());
        }


        //Helper methods
        private Mock<DbSet<EconomicBuilding>> GetEconomicBuildingsMock()
        {
            var data = new List<EconomicBuilding>
            {
                new EconomicBuilding { Id = 1, GoldIncome = 1, WoodIncome = 16, GoldCost = 12,  IsBuilt = true, Level = 547, Name = "Gold Mine", TickReduction = 0, WoodCost = 10000000},
                new EconomicBuilding { Id = 3, GoldIncome = 4, WoodIncome = 64, GoldCost = 12224, IsBuilt = true, Level = 0, Name = "B", TickReduction = 0, WoodCost = 224242},
                new EconomicBuilding { Id = 5, GoldIncome = 4, WoodIncome = 64, GoldCost = 1, IsBuilt = false, Level = 1, Name = "B", TickReduction = 0, WoodCost = 2},
                new EconomicBuilding { Id = 2, GoldIncome = 2, WoodIncome = 32, GoldCost = 7843, IsBuilt = true, Level = 762, Name = "Dlog Enim", TickReduction = 0, WoodCost = 12445},
                new EconomicBuilding { Id = 4, GoldIncome = 8, WoodIncome = 128, GoldCost = 1471, IsBuilt = false, Level = -1, Name = "Really long name for a building", TickReduction = 0, WoodCost = 0},
            }.AsQueryable();

            var mockSet = new Mock<DbSet<EconomicBuilding>>();
            mockSet.As<IQueryable<EconomicBuilding>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<EconomicBuilding>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<EconomicBuilding>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<EconomicBuilding>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            return mockSet;
        }

        private Mock<DbSet<MilitaryBuilding>> GetMilitaryBuildingsMock()
        {
            var data = new List<MilitaryBuilding>
            {
                new MilitaryBuilding { Id = 2, CreatableUnit = null, GoldCost = 150, IsBuilt = true, Level = 500, Name = "A really long name for a military building", WoodCost = 0},
                new MilitaryBuilding { Id = 1, CreatableUnit = null, GoldCost = 50, IsBuilt = true, Level = 0, Name = "A", WoodCost = -520},
                new MilitaryBuilding { Id = 5, CreatableUnit = null, GoldCost = 50, IsBuilt = false, Level = 1, Name = "A", WoodCost = 0},
                new MilitaryBuilding { Id = 3, CreatableUnit = null, GoldCost = 50000, IsBuilt = true, Level = 100, Name = "A", WoodCost = 1222200 },
                new MilitaryBuilding { Id = 4, CreatableUnit = null, GoldCost = 0, IsBuilt = false, Level = 413813, Name = "Something Something", WoodCost = 150},
            }.AsQueryable();

            var mockSet = new Mock<DbSet<MilitaryBuilding>>();
            mockSet.As<IQueryable<MilitaryBuilding>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<MilitaryBuilding>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<MilitaryBuilding>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<MilitaryBuilding>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            return mockSet;
        }

        private Mock<DbSet<Player>> GetPlayerMock()
        {
            var data = new List<Player>
            {
                new Player { Id = Constants.PlayerId, Army = null, GoldAmount = 1, TickInterval = 10, WoodAmount = 2},
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Player>>();
            mockSet.As<IQueryable<Player>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Player>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Player>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Player>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            return mockSet;
        }
    }
}
