using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SatelliteSite.BloggingModule
{
    public class BloggingModule<TContext> : AbstractModule where TContext : DbContext
    {
        public override string Area => "Blog";

        public override void Initialize()
        {
        }
    }
}
