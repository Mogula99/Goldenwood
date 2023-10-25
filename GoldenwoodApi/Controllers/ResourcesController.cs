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

        [HttpGet("Amount")]
        public ResourcesRecord GetAmount()
        {
            return resourcesService.GetCurrentResourcesAmount();
        }

        [HttpPost("Amount")]
        public ResourcesRecord ChangeAmount(ResourcesRecord resourcesRecord)
        {
            resourcesService.AddResources(resourcesRecord);
            return resourcesService.GetCurrentResourcesAmount();
        }

        [HttpGet("Income")]
        public ResourcesRecord GetIncome()
        {
            return resourcesService.GetTotalResourcesIncome();
        }

        [HttpGet("Tick")]
        public int GetTickInterval()
        {
            return resourcesService.GetTickInterval();
        }

        [HttpPost("Tick")]
        public ResourcesRecord UpdateResourcesAfterTick()
        {
            resourcesService.AddResourcesAfterTick();
            return resourcesService.GetCurrentResourcesAmount();
        }
    }
}