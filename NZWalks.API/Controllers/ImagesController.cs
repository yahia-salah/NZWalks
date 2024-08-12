using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domains;
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

        // POST /api/images/upload
        [HttpPost("upload")]
		public async Task<IActionResult> Upload([FromForm]ImageUploadRequestDTO request)
		{
			ValidateFileUpload(request);

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var imageDomainModel = new Image
			{
				File = request.File,
				FileName = request.FileName,
				FileSizeInBytes = request.File.Length,
				FileExtension = Path.GetExtension(request.File.FileName),
				FileDescription = request.FileDescription
			};

			await imageRepository.Upload(imageDomainModel);

			return Ok(imageDomainModel);
		}

		private void ValidateFileUpload(ImageUploadRequestDTO request)
		{
			var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };
			if (!allowedExtensions.Contains(Path.GetExtension(request.File.FileName)))
			{
				ModelState.AddModelError("file", "Invalid file extension. Only .jpg, .jpeg, .png are allowed.");
			}

			if (request.File.Length > 5*1024*1024)
			{
				ModelState.AddModelError("file", "File size should not exceed 5MB.");
			}
		}
	}	
}
