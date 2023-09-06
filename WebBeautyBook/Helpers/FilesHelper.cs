using WebBeautyBook.Models;

namespace WebBeautyBook.Helpers
{
    /// <summary>
    /// Helper class for working with files in the context of a C# ASP.NET Core API.
    /// </summary>
    public static class FilesHelper
    {
        /// <summary>
        /// Saves an <see cref="IFormFile"/> to the specified path.
        /// </summary>
        /// <param name="file">The file to save.</param>
        /// <param name="path">The path where the file will be saved.</param>
        /// <returns>A <see cref="HelperResponse"/> indicating the result of the save operation.</returns>
        public static async Task<HelperResponse> Save(this IFormFile file, string path)
        {
            try
            {
                if (file == null || path == null) return new HelperResponse(false, "file is not null");

                using (var fs = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fs);
                }
                return new HelperResponse(true, "Ok");
            }
            catch (Exception ex)
            {
                return new HelperResponse(false, ex.Message);
            }
        }

        /// <summary>
        /// Deletes a file at the specified path.
        /// </summary>
        /// <param name="path">The path of the file to delete.</param>
        /// <returns>A <see cref="HelperResponse"/> indicating the result of the delete operation.</returns>
        public static HelperResponse Delete(string path)
        {
            try
            {
                File.Delete(path);
                return new HelperResponse(true, "Ok");
            }
            catch (Exception ex)
            {
                return new HelperResponse(false, ex.Message);
            }
        }
    }
}
