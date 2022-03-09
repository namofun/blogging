#nullable enable
using System;

namespace Xylab.Blogging.Services
{
    /// <summary>
    /// The queryable extensions for blogging relate things.
    /// </summary>
    public static class BloggingQueryableExtensions
    {
        /// <summary>
        /// Gets the blogging queryable for facade.
        /// </summary>
        /// <param name="facade">The facade.</param>
        /// <returns>The queryable store.</returns>
        public static IBloggingQueryableStore AsQueryable(this IBloggingFacade facade)
        {
            return facade as IBloggingQueryableStore
                ?? throw new InvalidOperationException(
                    "IBloggingFacade doesn't implement queryable store.");
        }
    }
}
