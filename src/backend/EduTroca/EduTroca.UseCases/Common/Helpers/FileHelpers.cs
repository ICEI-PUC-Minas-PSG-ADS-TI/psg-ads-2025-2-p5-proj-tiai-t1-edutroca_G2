namespace EduTroca.UseCases.Common.Helpers;
public static class FileHelpers
{
    public static string GetExtensionFromMimeType(string mimeType)
    {
        return mimeType.ToLower() switch
        {
            "video/mp4" => ".mp4",
            "video/webm" => ".webm",
            "video/ogg" => ".ogv",
            "video/x-msvideo" => ".avi",
            "video/quicktime" => ".mov",

            "image/jpeg" => ".jpg",
            "image/jpg" => ".jpg",
            "image/png" => ".png",
            "image/webp" => ".webp",
            "image/gif" => ".gif",

            _ => mimeType.StartsWith("video/") ? ".mp4" : ".jpg"
        };
    }
}
