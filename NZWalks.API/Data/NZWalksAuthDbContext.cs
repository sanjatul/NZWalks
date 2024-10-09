using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data
{
    public class NZWalksAuthDbContext:IdentityDbContext
    {
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var readerRoleId = "e2b6b074-4f84-4150-8575-05f66c655202";
            var writerRoleId = "d33b10a1-58bd-487c-a53d-75af0224292c";
            var roles = new List<IdentityRole> {
                    new IdentityRole
                    {
                        Id= readerRoleId,
                        ConcurrencyStamp=readerRoleId,
                        Name="Reader",
                        NormalizedName="Reader".ToUpper()
                    },
                     new IdentityRole
                    {
                        Id= writerRoleId,
                        ConcurrencyStamp=writerRoleId,
                        Name="Writer",
                        NormalizedName="Writer".ToUpper()
                    }
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
