using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
using WebBeautyBook.Models;

namespace WebBeautyBook.Helpers
{
    public static class ImageHelper
    {
        /// <summary>
        /// Saves the provided image file to the WebP format with resizing.
        /// </summary>
        /// <param name="file">The input image file.</param>
        /// <param name="path">The destination path for saving the WebP image.</param>
        /// <returns>A HelperResponse indicating the success or failure of the operation.</returns>
        public static async Task<HelperResponse> SaveToWebpAsync(this IFormFile file, string path)
        {
            try
            {
                // Load the image from the provided file stream.
                using (var image = Image.Load(file.OpenReadStream()))
                {
                    // Resize the image to a specified size while maintaining aspect ratio.
                    image.Mutate(x =>
                    {
                        x.Resize(new ResizeOptions()
                        {
                            Size = new Size(720, 480),
                            Mode = ResizeMode.Max,
                        });
                    });
                    // Save the resized image as WebP format with specified quality.
                    await image.SaveAsWebpAsync(path, new WebpEncoder()
                    {
                        Quality = 80,
                    });
                }
                // Return a success response.
                return new HelperResponse(true, "Ok");
            }
            catch (Exception ex)
            {
                // Return an error response with the exception message.
                return new HelperResponse(false, ex.Message);
            }
        }

    }
}
