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
        public Goldenwood.Utilities.ResourcesRecord MauiResourcesRecordToCoreResourcesRecord(ResourcesRecord resourcesRecord)
        {
            var result = new Goldenwood.Utilities.ResourcesRecord(resourcesRecord.GoldAmount, resourcesRecord.WoodAmount);
            return result;
        }
        
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
