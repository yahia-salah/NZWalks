using NZWalks.API.Data;
using NZWalks.API.Models.Domains;

namespace NZWalks.API.Repositories
{
	public class ImageRepository : IImageRepository
	{
		private readonly IWebHostEnvironment webHostEnvironment;
		private readonly IHttpContextAccessor httpContextAccessor;
		private readonly NZWalksDbContext nZWalksDbContext;

		public ImageRepository(IWebHostEnvironment webHostEnvironment,IHttpContextAccessor httpContextAccessor,NZWalksDbContext nZWalksDbContext)
        {
			this.webHostEnvironment = webHostEnvironment;
			this.httpContextAccessor = httpContextAccessor;
			this.nZWalksDbContext = nZWalksDbContext;
		}

        public async Task<Image> Upload(Image image)
		{
			var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", image.FileName+image.FileExtension);

			// Save the file to the local file system
			using var fileStream = new FileStream(localFilePath, FileMode.Create);
			await image.File.CopyToAsync(fileStream);

			// Save the file information to the database
			var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/images/{image.FileName}{image.FileExtension}";
			image.FilePath = urlFilePath;
			await nZWalksDbContext.Images.AddAsync(image);
			await nZWalksDbContext.SaveChangesAsync();

			return image;
		}
	}
}
