using Goldenwood.Service;
using Goldenwood.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoldenwoodApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BuildingController : ControllerBase
    {
        private BuildingService buildingService;

        public BuildingController(BuildingService buildingService)
        {
            this.buildingService = buildingService;
        }

        [HttpGet("Level")]
        public int GetBuildingLevel(string buildingName)
        {
            return buildingService.GetMaxBuiltBuildingLevel(buildingName);
        }

        [HttpGet("Buildable")]
        public bool GetIsBuildable(string buildingName)
        {
            return buildingService.IsBuildable(buildingName);
        }

        [HttpGet("Resources")]
        public ResourcesRecord GetNeededResources(string buildingName, int buildingLevel)
        {
            return buildingService.GetNeededBuildingResources(buildingName, buildingLevel);
        }

        [HttpPost("Build")]
        public bool BuildOrUpgrade(string buildingName)
        {
            return buildingService.BuildOrUpgrade(buildingName);
        }
    }
}
