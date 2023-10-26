using Goldenwood.Model.Units;
using Goldenwood.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoldenwoodApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MilitaryController : ControllerBase
    {
        private MilitaryService militaryService;

        public MilitaryController(MilitaryService militaryService)
        {
            this.militaryService = militaryService;
        }

        /// <summary>
        /// Gets a list of the player's current units
        /// </summary>
        /// <response code="200">Returns a list of the units that the player currently has.</response>
        [HttpGet("Units/Player")]
        public ICollection<UnitGroup> PlayerUnits()
        {
            return militaryService.GetPlayerUnitGroups();
        }

        /// <summary>
        /// Gets a list of enemy's units.
        /// </summary>
        /// <param name="enemyId">Id of the enemy</param>
        /// <response code="200">Returns a list of the units that the specified enemy currently has.</response>
        [HttpGet("Units/Enemy")]
        public ICollection<UnitGroup> EnemyUnits(int enemyId)
        {
            return militaryService.GetEnemyUnitGroups(enemyId);
        }

        /// <summary>
        /// Gets the information about whether a specified unit can be recruited (required buildings are already built) at this time or not.
        /// </summary>
        /// <param name="unitId">Id of the unit</param>
        /// <response code="200">Returns true if the specified unit can be recruited. False otherwise.</response>
        [HttpGet("Recruit")]
        public bool CanBeRecruited(int unitId)
        {
            return militaryService.CanBeRecruited(unitId);
        }

        /// <summary>
        /// Attempts to recruit a <paramref name="recruitCount"/> number of units specified by <paramref name="unitId"/>.
        /// </summary>
        /// <param name="unitId">Id of the unit type we want to recruit.</param>
        /// <param name="recruitCount">Number of units that we want to recruit.</param>
        /// <response code="200">Returns a list of the units that the player currently has.</response>
        [HttpPost("Recruit")]
        public ICollection<UnitGroup> Recruit(int unitId, int recruitCount) {
            militaryService.RecruitUnits(unitId, recruitCount);
            return militaryService.GetPlayerUnitGroups();
        }

        /// <summary>
        /// Sends player's units to fight the specified enemy.
        /// </summary>
        /// <param name="enemyId">Id of the enemy we want to fight.</param>
        /// <response code="200">Returns true if the player defeated the enemy. False otherwise.</response>
        [HttpPost("Fight")]
        public bool Fight(int enemyId)
        {
            return militaryService.Fight(enemyId);
        }
    }
}
