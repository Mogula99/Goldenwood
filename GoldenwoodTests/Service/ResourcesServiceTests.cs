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
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace GoldenwoodTests.Service
{
    public class ResourcesServiceTests
    {
        [Fact]
        public void GetCurrentResourcesAmountTest()
        {
            //Asking for current resources amount
            var dbContextMock = new Mock<ApplicationDbContext>();
            var playerMockSet = GetPlayerMock();
            dbContextMock.Setup(m => m.Player).Returns(playerMockSet.Object);

            var resourcesService = new ResourcesService(dbContextMock.Object);
            var expectedResult = new ResourcesRecord(123, 45);
            var result = resourcesService.GetCurrentResourcesAmount();

            Assert.Equal(expectedResult.GoldAmount, result.GoldAmount);
            Assert.Equal(expectedResult.WoodAmount, result.WoodAmount);
        }

        [Fact]
        public void GetTotalResourcesIncomeTest()
        {
            //Asking for current resources income
            var dbContextMock = new Mock<ApplicationDbContext>();
            dbContextMock.Setup(m => m.EconomicBuilding).Returns(GetEconomicBuildingsMock().Object);
            dbContextMock.Setup(m => m.Enemy).Returns(GetEnemiesMock().Object);

            var resourcesService = new ResourcesService(dbContextMock.Object);
            var expectedResult = new ResourcesRecord(3 + Constants.DefeatedEnemyGoldIncome * 2, 48 + Constants.DefeatedEnemyWoodIncome * 2);
            var result = resourcesService.GetTotalResourcesIncome();

            Assert.Equal(expectedResult.GoldAmount, result.GoldAmount);
            Assert.Equal(expectedResult.WoodAmount, result.WoodAmount);
        }

        [Fact]
        public void AddResourcesAfterTickTest()
        {
            //Checking resources addition after a tick
            var dbContextMock = new Mock<ApplicationDbContext>();
            var playerMock = GetPlayerMock();
            dbContextMock.Setup(m => m.Player).Returns(playerMock.Object);
            dbContextMock.Setup(m => m.EconomicBuilding).Returns(GetEconomicBuildingsMock().Object);
            dbContextMock.Setup(m => m.Enemy).Returns(GetEnemiesMock().Object);

            var startingResources = new ResourcesRecord(playerMock.Object.FirstOrDefault().GoldAmount, playerMock.Object.FirstOrDefault().WoodAmount);
            var addedResources = new ResourcesRecord(3 + Constants.DefeatedEnemyGoldIncome * 2, 48 + Constants.DefeatedEnemyWoodIncome * 2);
            var expectedResources = startingResources + addedResources;

            var resourcesService = new ResourcesService(dbContextMock.Object);
            resourcesService.AddResourcesAfterTick();

            dbContextMock.Verify(m => m.SaveChanges(), Times.Once());
            Assert.Equal(expectedResources.GoldAmount, playerMock.Object.FirstOrDefault().GoldAmount);
            Assert.Equal(expectedResources.WoodAmount, playerMock.Object.FirstOrDefault().WoodAmount);
        }

        [Fact]
        public void AddResourcesTest()
        {
            //Asking for resources addition
            var dbContextMock = new Mock<ApplicationDbContext>();
            var playerMock = GetPlayerMock();
            dbContextMock.Setup(m => m.Player).Returns(playerMock.Object);

            var resourcesToAdd = new ResourcesRecord(15000, 9999);
            var expectedResources = new ResourcesRecord(123 + resourcesToAdd.GoldAmount, 45 + resourcesToAdd.WoodAmount);

            var resourcesService = new ResourcesService(dbContextMock.Object);
            resourcesService.AddResources(resourcesToAdd);

            dbContextMock.Verify(m => m.SaveChanges(), Times.Once());
            Assert.Equal(expectedResources.GoldAmount, playerMock.Object.FirstOrDefault().GoldAmount);
            Assert.Equal(expectedResources.WoodAmount, playerMock.Object.FirstOrDefault().WoodAmount);
        }

        [Fact]
        public void AddResourcesTest2()
        {
            //Asking for 0 resources addition
            var dbContextMock = new Mock<ApplicationDbContext>();
            var playerMock = GetPlayerMock();
            dbContextMock.Setup(m => m.Player).Returns(playerMock.Object);

            var resourcesToAdd = new ResourcesRecord(0, 0);
            var expectedResources = new ResourcesRecord(123 + resourcesToAdd.GoldAmount, 45 + resourcesToAdd.WoodAmount);

            var resourcesService = new ResourcesService(dbContextMock.Object);
            resourcesService.AddResources(resourcesToAdd);

            dbContextMock.Verify(m => m.SaveChanges(), Times.Once());
            Assert.Equal(expectedResources.GoldAmount, playerMock.Object.FirstOrDefault().GoldAmount);
            Assert.Equal(expectedResources.WoodAmount, playerMock.Object.FirstOrDefault().WoodAmount);
        }

        [Fact]
        public void AddResourcesTest3()
        {
            //Asking to add a negative amount of resources
            var dbContextMock = new Mock<ApplicationDbContext>();
            var playerMock = GetPlayerMock();
            dbContextMock.Setup(m => m.Player).Returns(playerMock.Object);

            var resourcesToAdd = new ResourcesRecord(-151515, -1);
            var expectedResources = new ResourcesRecord(123 + resourcesToAdd.GoldAmount, 45 + resourcesToAdd.WoodAmount);

            var resourcesService = new ResourcesService(dbContextMock.Object);
            resourcesService.AddResources(resourcesToAdd);

            dbContextMock.Verify(m => m.SaveChanges(), Times.Once());
            Assert.Equal(expectedResources.GoldAmount, playerMock.Object.FirstOrDefault().GoldAmount);
            Assert.Equal(expectedResources.WoodAmount, playerMock.Object.FirstOrDefault().WoodAmount);
        }

        //Helper methods

        private Mock<DbSet<Player>> GetPlayerMock()
        {
            var data = new List<Player>
            {
                new Player { Id = Constants.PlayerId, Army = null, GoldAmount = 123, WoodAmount = 45},
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Player>>();
            mockSet.As<IQueryable<Player>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Player>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Player>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Player>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            return mockSet;
        }

        private Mock<DbSet<EconomicBuilding>> GetEconomicBuildingsMock()
        {
            var data = new List<EconomicBuilding>
            {
                new EconomicBuilding { Id = 1, GoldIncome = 1, WoodIncome = 16, GoldCost = 12,  IsBuilt = true, Level = 547, Name = "Gold Mine", TickReduction = 0, WoodCost = 10000000},
                new EconomicBuilding { Id = 3, GoldIncome = 4, WoodIncome = 64, GoldCost = 3124, IsBuilt = false, Level = 0, Name = "A", TickReduction = 0, WoodCost = 737337},
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

        private Mock<DbSet<Enemy>> GetEnemiesMock()
        {
            var data = new List<Enemy>
            {
                new Enemy { Id = 1, Name = "Prague", Alive = true, Army = null },
                new Enemy { Id = 2, Name = "Brno", Alive = false, Army = null },
                new Enemy { Id = 4, Name = "Strahov", Alive = true, Army = null },
                new Enemy { Id = 3, Name = "Dejvice", Alive = false, Army = null },
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Enemy>>();
            mockSet.As<IQueryable<Enemy>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Enemy>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Enemy>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Enemy>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            return mockSet;
        }
    }

}
