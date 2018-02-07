using MongoDB.Driver.Linq;
using SmartFreeze.Models;
using System.Linq;

namespace SmartFreeze.Filters
{
    public class SiteFilter : IMongoFilter<Site>
    {
        public ApplicationContext Context { get; set; }
        public bool? HasAlarms { get; set; }

        public IMongoQueryable<Site> FilterSource(IMongoQueryable<Site> source)
        {
            source = source.Where(e => e.SiteType == Context);

            if(HasAlarms.HasValue && HasAlarms.Value)
            {
                source = source.Where(e => e.Alarms.Any(a => a.IsActive));
            }
            else if(HasAlarms.HasValue && !HasAlarms.Value)
            {
                source = source.Where(e => !e.Alarms.Any(a => a.IsActive));
            }

            return source;
        }
    }
}
