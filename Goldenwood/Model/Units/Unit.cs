using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldenwood.Model.Units
{
    public class Unit
    {
        public string Name { get; set; }
        public int Power { get; set; }
        public int GoldCost { get; set; }
        public int WoodCost { get; set; }
    }
}
