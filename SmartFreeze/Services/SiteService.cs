﻿using SmartFreeze.Filters;
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

        public PaginatedItems<Site> Get(IMongoFilter<Site> filter, int rowsPerPage, int pageNumber)
        {
            return siteRepository.GetAllPaginated(filter, rowsPerPage, pageNumber);
        }
    }
}