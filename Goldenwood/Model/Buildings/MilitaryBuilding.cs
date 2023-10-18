using Goldenwood.Model.Units;

namespace Goldenwood.Model.Building
{
    public class MilitaryBuilding: Building
    {
        public ICollection<Unit> CreatableUnits { get; set; }
    }
}
