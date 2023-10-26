using Goldenwood.Service;
using Goldenwood.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoldenwoodApi.Controllers
{
    /// <summary>
    /// This controller is responsible for building oriented business logic. It can return specific building or build/upgrade buildings.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class BuildingController : ControllerBase
    {
        private BuildingService buildingService;

        /// <summary>
        /// This is a constructor for the Building controller
        /// </summary>
        /// <param name="buildingService">Building service instance</param>
        public BuildingController(BuildingService buildingService)
        {
            this.buildingService = buildingService;
        }

        /// <summary>
        /// Gets the current level of a building.
        /// </summary>
        /// <param name="buildingName">Name of the building</param>
        /// <response code="200">Returns level of a building specified by its <paramref name="buildingName"/></response>
        [HttpGet("Level")]
        public int GetBuildingLevel(string buildingName)
        {
            return buildingService.GetMaxBuiltBuildingLevel(buildingName);
        }

        /// <summary>
        /// Gets the information about whether a building can be built or not
        /// </summary>
        /// <param name="buildingName">Name of the building</param>
        /// <response code="200">Returns true(1) if building specified by its <paramref name="buildingName"/> can be built. False(0) otherwise.</response>
        [HttpGet("Buildable")]
        public bool GetIsBuildable(string buildingName)
        {
            return buildingService.IsBuildable(buildingName);
        }

        /// <summary>
        /// Gets the amount of resources needed to build a building (specified by <paramref name="buildingName"/>) with a specified <paramref name="buildingLevel"/>
        /// </summary>
        /// <param name="buildingName">Name of the building</param>
        /// <param name="buildingLevel">Level of the building</param>
        /// <response code="200">Returns the record with resources needed to build a building with specified level.</response>
        [HttpGet("Resources")]
        public ResourcesRecord GetNeededResources(string buildingName, int buildingLevel)
        {
            return buildingService.GetNeededBuildingResources(buildingName, buildingLevel);
        }

        /// <summary>
        /// Builds or upgrades the specified building.
        /// </summary>
        /// <param name="buildingName">Name of the building</param>
        /// <response code="200">Returns true if the building has been successfully built/upgraded. False otherwise.</response>
        [HttpPost("Build")]
        public bool BuildOrUpgrade(string buildingName)
        {
            return buildingService.BuildOrUpgrade(buildingName);
        }
    }
}
