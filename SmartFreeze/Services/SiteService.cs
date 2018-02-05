using SmartFreeze.Models;
using SmartFreeze.Repositories;

namespace SmartFreeze.Services
{
    public class SiteService
    {
        private readonly SiteRepository siteRepository;

        public SiteService(SiteRepository siteRepository)
        {
            this.siteRepository = siteRepository;
        }

        public Site Get(string siteId)
        {
            return siteRepository.Get(siteId);
        }
    }
}
