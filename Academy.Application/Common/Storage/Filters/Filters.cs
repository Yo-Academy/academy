using Microsoft.AspNetCore.Http;
using SkiaSharp;


namespace Academy.Application.Common.Storage.Filters
{
    public static class Filter
    {
        public static Stream Resize(this Stream file, int newWidth, int newHeight)
        {
            try
            {
                using (var inputStream = file)
                {
                    using (var original = SKBitmap.Decode(inputStream))
                    {
                        var resizedBitmap = new SKBitmap(newWidth, newHeight);
                        using (var canvas = new SKCanvas(resizedBitmap))
                        {
                            var paint = new SKPaint
                            {
                                FilterQuality = SKFilterQuality.High
                            };
                            canvas.DrawBitmap(original, new SKRect(0, 0, newWidth, newHeight), paint);
                        }
                        var resizedData = resizedBitmap.Encode(SKEncodedImageFormat.Jpeg, 100);
                        return new MemoryStream(resizedData.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public static Stream Resize(this IFormFile formFile, int newWidth, int newHeight)
        {
            Stream file = formFile.OpenReadStream();
            return file.Resize(newWidth, newHeight);
        }

        public static string GetFileExtension(this IFormFile formFile)
        {
            try
            {
                return Path.GetExtension(formFile.FileName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public static string GetFileExtension(this string fileName)
        {
            try
            {
                return fileName.Substring(fileName.LastIndexOf("."));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
