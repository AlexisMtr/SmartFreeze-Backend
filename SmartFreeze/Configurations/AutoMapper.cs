﻿using AutoMapper;
using AutoMapper.Configuration;
using SmartFreeze.Profiles;

namespace SmartFreeze
{
    public partial class Startup
    {
        public void ConfigureMapper()
        {
            var configuration = new MapperConfigurationExpression();

            configuration.AddProfile<SiteOverviewProfile>();

            Mapper.Initialize(configuration);
        }
    }
}
