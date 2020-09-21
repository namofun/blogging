using Blogging.Entities;
using Blogging.Services;
using Markdig;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SatelliteSite.BloggingModule
{
    public class BloggingModule<TContext> : AbstractModule where TContext : DbContext
    {
        public override string Area => "Blog";

        public override void Initialize()
        {
        }

        public override void RegisterEndpoints(IEndpointBuilder endpoints)
        {
            endpoints.MapControllers();
        }

        public override void RegisterServices(IServiceCollection services)
        {
            services.AddMarkdown();
            services.AddTransient<IMarkdownResolver, DefaultMarkdownResolver>();

            services.AddScoped<IBlogStore, BlogStore<TContext>>();
            services.AddScoped<ICommentStore, CommentStore<TContext>>();
            services.AddDbModelSupplier<TContext, BlogEntityConfiguration<TContext>>();
        }
    }
}
