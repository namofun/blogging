﻿using Markdig;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SatelliteSite;
using Xylab.Blogging.Entities;
using Xylab.Blogging.Services;

[assembly: RoleDefinition(25, "BlogPublisher", "blogger", "Blog Publisher")]

namespace SatelliteSite.BloggingModule
{
    public class BloggingModule<TUser, TContext> : AbstractModule
        where TUser : SatelliteSite.IdentityModule.Entities.User
        where TContext : DbContext
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

            services.AddScoped<BloggingFacade<TUser, TContext>>();
            services.AddScopedUpcast<IBloggingFacade, BloggingFacade<TUser, TContext>>();
            services.AddScopedUpcast<IBlogStore, BloggingFacade<TUser, TContext>>();
            services.AddScopedUpcast<ICommentStore, BloggingFacade<TUser, TContext>>();
            services.AddDbModelSupplier<TContext, BlogEntityConfiguration<TUser, TContext>>();
        }
    }
}
