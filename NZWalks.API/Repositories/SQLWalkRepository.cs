using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext dbContext;
        public SQLWalkRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var exsistingWalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (exsistingWalk == null)
            {
                return null;
            }

            dbContext.Walks.Remove(exsistingWalk);
            await dbContext.SaveChangesAsync();
            return exsistingWalk;
        }

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAcending = true, int pageNumber = 1, int pageSize = 1000)
        {
            //return await dbContext.Walks.Include("Region").Include("Difficulity").ToListAsync();

            var walks= dbContext.Walks.Include("Region").Include("Difficulity").AsQueryable();
            //Filtering
            if(string.IsNullOrWhiteSpace(filterOn)==false && string.IsNullOrWhiteSpace(filterQuery)==false)
            {
                if(filterOn.Equals("Name",StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x=>x.Name.Contains(filterQuery));
                }
            }
            //Sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false )
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAcending?walks.OrderBy(x=>x.Name):walks.OrderByDescending(x=>x.Name);
                }
                else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAcending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                }
            }
            //Pagination
            var skipResults = (pageNumber - 1) * pageNumber;
            return await walks.Skip(skipResults).Take(pageSize).ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
           return await dbContext.Walks.Include("Region").Include("Difficulity").FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var exsistingWalk = await dbContext.Walks.FirstOrDefaultAsync(x=>x.Id==id);
            if (exsistingWalk == null)
            {
                return null;
            }
            exsistingWalk.WalkImageUrl = walk.WalkImageUrl;
            exsistingWalk.Name = walk.Name;
            exsistingWalk.DifficulityId = walk.DifficulityId;
            exsistingWalk.LengthInKm = walk.LengthInKm;
            exsistingWalk.RegionId = walk.RegionId;
            exsistingWalk.Description = walk.Description;
            await dbContext.SaveChangesAsync();
            return exsistingWalk;
        }
    }
}
