using Goldenwood.Service;
using Goldenwood.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace GoldenwoodApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResourcesController : ControllerBase {
        private ResourcesService resourcesService;

        public ResourcesController(ResourcesService resourcesService)
        {
            this.resourcesService = resourcesService;
        }

        /// <summary>
        /// Gets current amount of the player's resources.
        /// </summary>
        /// <response code="200">Returns a record with the information oabout how much gold and wood the player currently has.</response>
        [HttpGet("Amount")]
        public ResourcesRecord GetAmount()
        {
            return resourcesService.GetCurrentResourcesAmount();
        }

        /// <summary>
        /// Adds a specified amount of resources to the player's current resources.
        /// </summary>
        /// <param name="resourcesRecord">Resources to be added to the player's</param>
        /// <response code="200">Returns a record with the information about how much gold and wood the player currently has.</response>
        [HttpPost("Amount")]
        public ResourcesRecord ChangeAmount(ResourcesRecord resourcesRecord)
        {
            resourcesService.AddResources(resourcesRecord);
            return resourcesService.GetCurrentResourcesAmount();
        }

        /// <summary>
        /// Gets current player's resources income.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Returns a record with the information about how much gold and wood the player gets every tick.</response>
        [HttpGet("Income")]
        public ResourcesRecord GetIncome()
        {
            return resourcesService.GetTotalResourcesIncome();
        }

        /// <summary>
        /// Gets the interval between two ticks.
        /// </summary>
        /// <response code="200">Returns a number of seconds that has to pass between two ticks to happen</response>
        [HttpGet("Tick")]
        public int GetTickInterval()
        {
            return resourcesService.GetTickInterval();
        }

        /// <summary>
        /// Updates the current amount of resources the player has after a tick.
        /// </summary>
        /// <response code="200">Returns a record with the information about how much gold and wood the player currently has.</response>
        [HttpPost("Tick")]
        public ResourcesRecord UpdateResourcesAfterTick()
        {
            resourcesService.AddResourcesAfterTick();
            return resourcesService.GetCurrentResourcesAmount();
        }
    }
}