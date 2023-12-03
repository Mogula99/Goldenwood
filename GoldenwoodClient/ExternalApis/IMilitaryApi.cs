using Goldenwood.Model.Units;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenwoodClient.ExternalApis
{
    public interface IMilitaryApi
    {
        [Get("/Military/Units/Player")]
        Task<ICollection<UnitGroup>> PlayerUnits();

        [Get("/Military/Units/Enemy")]
        Task<ICollection<UnitGroup>> EnemyUnits(int enemyId);
        [Get("/Military/Unit")]
        Task<Unit> GetUnit(int unitId);

        [Get("/Military/Recruit")]
        Task<bool> CanBeRecruited(int unitId);

        [Post("/Military/Recruit")]
        Task<ICollection<UnitGroup>> Recruit(int unitId, int recruitCount);
        [Get("/Military/Recruit/Resources")]
        Task<bool> DoesHaveEnoughResources(int unitId, int recruitCount);

        [Post("/Military/Fight")]
        Task<bool> Fight(int enemyId);
    }
}
