using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldenwood.Model.Building
{
    public class EconomicBuilding : Building
    {
        public int GoldIncome { get; set; }
        public int WoodIncome { get; set; }
        public int TickRedution { get; set; }
    }
}
