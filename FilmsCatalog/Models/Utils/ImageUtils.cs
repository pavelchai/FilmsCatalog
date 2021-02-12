using Microsoft.AspNetCore.Http;
using SkiaSharp;
using System;

namespace FilmsCatalog.Models
{
    public static class ImageUtils
    {
        private const string pngBase64MimeType = "data:image/png;base64,";

        public static bool TryConvertFileToPNG(this IFormFile file, out byte[] data)
        {
            using (var stream = file.OpenReadStream())
            {
                var bitmap = SKBitmap.Decode(stream);
                
                if (bitmap == null)
                {
                    data = null;
                    return false;
                }

                data = bitmap.Encode(SKEncodedImageFormat.Png, 100).ToArray();
                bitmap.Dispose();

                return true;
            }
        }

        public static string ConvertToBase64WithMimeType(byte[] pngImageData)
        {
            return $"{pngBase64MimeType}{Convert.ToBase64String(pngImageData)}";
        }

        public static byte[] ConvertFromBase64WithMimeType(string pngBase64WithMimeType)
        {
            return Convert.FromBase64String(pngBase64WithMimeType.Substring(pngBase64MimeType.Length));
        }
    }
}