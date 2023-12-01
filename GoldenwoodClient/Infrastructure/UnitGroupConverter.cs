using GoldenwoodClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenwoodClient.Infrastructure
{
    public class UnitGroupConverter
    {
        public Goldenwood.Model.Units.UnitGroup MauiUnitGroupToCoreUnitGroup(UnitGroup unitGroup)
        {
            var result = new Goldenwood.Model.Units.UnitGroup
            {
                Id = unitGroup.Id,
                Unit = unitGroup.Unit,
                UnitCount = unitGroup.UnitCount
            };
            return result;
        }

        public UnitGroup CoreUnitGroupToMauiUnitGroup(Goldenwood.Model.Units.UnitGroup unitGroup)
        {
            var result = new UnitGroup
            {
                Id = unitGroup.Id,
                Unit = unitGroup.Unit,
                UnitCount = unitGroup.UnitCount
            };
            return result;
        }
    }
}
