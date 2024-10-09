﻿using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext dbContext;
        public SQLRegionRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var exsistingRegion = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (exsistingRegion == null)
            {
                return null;
            }

            dbContext.Regions.Remove(exsistingRegion);
            await dbContext.SaveChangesAsync();
            return exsistingRegion;
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await dbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
           return await dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var exsistingRegion = await dbContext.Regions.FirstOrDefaultAsync(x=>x.Id==id);
            if (exsistingRegion == null)
            {
                return null;
            }
            exsistingRegion.Code = region.Code;
            exsistingRegion.Name = region.Name;
            exsistingRegion.RegionImageUrl = region.RegionImageUrl;
            await dbContext.SaveChangesAsync();
            return exsistingRegion;
        }
    }
}
