using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SCSS.IdentityServer4.Enities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.IdentityServer4.Data.Enities
{
    public class IdentityDBContext : IdentityDbContext
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityDBContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public IdentityDBContext(DbContextOptions<IdentityDBContext> options)
            : base(options)
        {

        }
        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>(
                users =>
                {
                    users.HasMany(x => x.Claims)
                        .WithOne()
                        .HasForeignKey(x => x.UserId)
                        .IsRequired()
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
