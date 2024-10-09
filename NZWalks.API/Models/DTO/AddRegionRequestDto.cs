using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class AddRegionRequestDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Code must be a maximum 100 characters")]
        public string Name { get; set; }
        [Required]
        [MinLength(3,ErrorMessage ="Code must be a minimum of 3 characters")]
        [MaxLength(3,ErrorMessage ="Code must be a maximum 3 characters")]
        public string Code { get; set; }
        public string? RegionImageUrl { get; set; }
    }
} 
