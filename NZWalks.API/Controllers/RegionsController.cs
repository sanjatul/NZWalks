using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(NZWalksDbContext dbContext,IRegionRepository regionRepository,IMapper mapper,ILogger<RegionsController> logger)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        [HttpGet]
       // [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll()
        {
           
                var regionsDomain = await regionRepository.GetAllAsync();
                //Map domain models to dtos
                var regionsDto = mapper.Map<List<RegionDto>>(regionsDomain);

            //var regionsDto = new List<RegionDto>();
            //foreach(var regionDomain in regionsDomain)
            //{
            //    regionsDto.Add(new RegionDto { 
            //    Id = regionDomain.Id,
            //    Code= regionDomain.Code,
            //    Name= regionDomain.Name,
            //    RegionImageUrl= regionDomain.RegionImageUrl,
            //    });
            //}

            //return Ok(regionsDto);

            //Dummy exception
            throw new Exception("This is a new exception");

                return Ok(mapper.Map<List<RegionDto>>(regionsDomain));
          
            
        }

        [HttpGet]
        //[Authorize(Roles ="Reader")]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            //var region = dbContext.Regions.Find(id);
            var regionDomain = await regionRepository.GetByIdAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<RegionDto>(regionDomain));
        }

        [HttpPost]
        //[Authorize(Roles ="Writer")]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
          
                var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);
                regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);
                var regionDto = mapper.Map<RegionDto>(regionDomainModel);
                return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id }, regionDto);
         
        }

        [HttpPut]
      
        [Route("{id:Guid}")]
       // [Authorize(Roles = "Writer")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody]UpdateRegionRequestDto updateRegionRequestDto)
        {
          
                var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

                regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);
                if (regionDomainModel == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<RegionDto>(regionDomainModel));
        
        }


        [HttpDelete]
       // [Authorize(Roles = "Writer,Reader")]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomain =await regionRepository.DeleteAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<RegionDto>(regionDomain));
        }
    }
}
