﻿using SmartFreeze.Filters;
using SmartFreeze.Models;
using SmartFreeze.Repositories;
using System;
using System.Collections.Generic;

namespace SmartFreeze.Services
{
    public class SiteService
    {
        private readonly ISiteRepository siteRepository;

        public SiteService(ISiteRepository siteRepository)
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

        public PaginatedItems<Site> GetByIds(IEnumerable<string> ids, int rowsPerPage, int pageNumber)
        {
            return siteRepository.GetPaginatedByIds(ids, rowsPerPage, pageNumber);
        }

        public Site Create(Site site)
        {
            return siteRepository.Create(site);
        }

        public bool Update(string siteId, Site site)
        {
            return siteRepository.Update(siteId, site);
        }

        public bool Delete(string siteId)
        {
            return siteRepository.Delete(siteId);
        }
    }
}
