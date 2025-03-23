using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;

namespace WebStore.Helpers
{
    public static class ImageUtil
    {
        private static IWebHostEnvironment? _env;

        public static void Initialize(IWebHostEnvironment env)
        {
            _env = env;
        }

        public static List<string> GetImages(int productId)
        {
            if(_env == null)
                throw new InvalidOperationException("ImageUtil not initialized");

            string imagesFolder = Path.Combine(_env.WebRootPath, "images", productId.ToString());

            if (!Directory.Exists(imagesFolder))
                return new List<string> { "/images/no-image.png" };

            var files = Directory.GetFiles(imagesFolder, "*.*")
                .Where(file => file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
                            || file.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase)
                            || file.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                .Select(file => "/images/" + productId + "/" + Path.GetFileName(file))
                .ToList();

            return files.Any() ? files : new List<string> { "/images/no-image.png" };
        }
    }
}
