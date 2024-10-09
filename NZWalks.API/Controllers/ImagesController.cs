using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto request)
        {
            ValidateFileUpload(request);
            if (ModelState.IsValid) {
                //Convert dto to domain model
                var imageDomainModel = new Image
                {
                    File=request.File,
                    FileExtension=Path.GetExtension(request.File.FileName),
                    FileSizeInBytes=request.File.Length,
                    FileName=request.FileName,
                    FileDescription=request.FileDescription
                };
                //Repository
                await imageRepository.Upload(imageDomainModel);
                return Ok(imageDomainModel);

            }
            return BadRequest(ModelState);
        }
        private void ValidateFileUpload(ImageUploadRequestDto request)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };
            if (!allowedExtensions.Contains(Path.GetExtension(request.File.FileName)))
            {
                ModelState.AddModelError("file","Unsupported media type");
            }
            if (request.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "File Size more than 10 Mb, Please upload small size");
            }
        }
    }
}
