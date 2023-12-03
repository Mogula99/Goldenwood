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
        /// <summary>
        /// This method is used for converting UnitGroup objects from GoldenwoodClient project to Goldenwood project
        /// </summary>
        /// <param name="unitGroup">GoldenwoodClient's UnitGroup</param>
        /// <returns>An equivalent instance of the Goldenwood's UnitGroup</returns>
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

        /// <summary>
        /// This method is used for converting UnitGroup objects from Goldenwood project to GoldenwoodClient project
        /// </summary>
        /// <param name="unitGroup">Goldenwood's UnitGroup</param>
        /// <returns>An equivalent instance of the GoldenwoodClient's UnitGroup</returns>
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
