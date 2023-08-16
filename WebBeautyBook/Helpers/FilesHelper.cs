using WebBeautyBook.Models;

namespace WebBeautyBook.Helpers
{
    public static class FilesHelper
    {

        public static async Task<HelperResponse> Save(this IFormFile file, string path)
        {
            try
            {
                if (file == null || path == null)
                    return new HelperResponse(false, "file is not null");

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
