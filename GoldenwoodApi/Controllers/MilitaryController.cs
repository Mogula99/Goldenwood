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

        [HttpGet("Units/Player")]
        public ICollection<UnitGroup> PlayerUnits()
        {
            return militaryService.GetPlayerUnitGroups();
        }

        [HttpGet("Units/Enemy")]
        public ICollection<UnitGroup> PlayerUnits(int enemyId)
        {
            return militaryService.GetEnemyUnitGroups(enemyId);
        }

        [HttpGet("Recruit")]
        public bool CanBeRecruited(string unitName)
        {
            return militaryService.CanBeRecruited(unitName);
        }

        [HttpPost("Recruit")]
        public ICollection<UnitGroup> Recruit(string unitName, int recruitCount) {
            militaryService.RecruitUnits(unitName, recruitCount);
            return militaryService.GetPlayerUnitGroups();
        }

        [HttpPost("Fight")]
        public bool Fight(int enemyId)
        {
            return militaryService.Fight(enemyId);
        }
    }
}
