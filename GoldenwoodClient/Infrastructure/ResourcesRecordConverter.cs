using GoldenwoodClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenwoodClient.Infrastructure
{
    public class ResourcesRecordConverter
    {
        /// <summary>
        /// This method is used for converting ResourcesRecord objects from GoldenwoodClient project to Goldenwood project
        /// </summary>
        /// <param name="resourcesRecord">GoldenwoodClient's ResourcesRecord</param>
        /// <returns>An equivalent instance of the Goldenwood's ResourcesRecord</returns>
        public Goldenwood.Utilities.ResourcesRecord MauiResourcesRecordToCoreResourcesRecord(ResourcesRecord resourcesRecord)
        {
            var result = new Goldenwood.Utilities.ResourcesRecord(resourcesRecord.GoldAmount, resourcesRecord.WoodAmount);
            return result;
        }

        /// <summary>
        /// This method is used for converting ResourcesRecord objects from Goldenwood project to GoldenwoodClient project
        /// </summary>
        /// <param name="resourcesRecord">Goldenwood's ResourcesRecord</param>
        /// <returns>An equivalent instance of the GoldenwoodClient's ResourcesRecord</returns>
        public ResourcesRecord CoreResourcesRecordToMauiResourcesRecord(Goldenwood.Utilities.ResourcesRecord resourcesRecord)
        {
            var result = new ResourcesRecord
            {
                GoldAmount = resourcesRecord.GoldAmount,
                WoodAmount = resourcesRecord.WoodAmount
            };
            return result;
        }
    }
}
