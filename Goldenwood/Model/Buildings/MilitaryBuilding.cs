using Goldenwood.Model.Units;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Goldenwood.Model.Building
{
    public class MilitaryBuilding: Building
    {
        public ICollection<Unit> CreatableUnits { get; set; }
    }
}
