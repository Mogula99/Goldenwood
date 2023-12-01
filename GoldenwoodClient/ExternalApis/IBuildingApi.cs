using Goldenwood.Utilities;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenwoodClient.ExternalApis
{
    public interface IBuildingApi
    {
        [Get("/building/level")]
        Task<int> GetBuildingLevel(string buildingName);
        [Get("/building/buildable")]
        Task<bool> GetIsBuildable(string buildingName);
        [Get("/building/resources")]
        Task<ResourcesRecord> GetNeededResources(string buildingName, int buildingLevel);
        [Post("/building/build")]
        Task<bool> BuildOrUpgrade(string buildingName);
    }
}
