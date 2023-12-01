using Goldenwood.Utilities;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenwoodClient.ExternalApis
{
    public interface IResourcesApi
    {
        [Get("/resources/amount")]
        Task<ResourcesRecord> GetAmount();
        [Post("/resources/amount")]
        Task<ResourcesRecord> ChangeAmount(ResourcesRecord resourcesRecord);
        [Get("/resources/income")]
        Task<ResourcesRecord> GetIncome();
        [Get("/resources/tick")]
        Task<int> GetTickInterval();
        [Post("/resources/tick")]
        Task<ResourcesRecord> UpdateResourcesAfterTick();
    }
}
