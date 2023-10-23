using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldenwood.Utilities
{
    public record ResourcesRecord(int GoldAmount, int WoodAmount)
    {
        public static ResourcesRecord operator +(ResourcesRecord lhs, ResourcesRecord rhs)
        {
            return new ResourcesRecord(lhs.GoldAmount + rhs.GoldAmount, lhs.WoodAmount + rhs.WoodAmount);
        }
    }

}
