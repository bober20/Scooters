namespace Scooters.Services;

public static class ImageService
{
    public static async Task<string> SaveImageAsync(FileResult file)
    {
        var extension = Path.GetExtension(file.FileName);
        var newName = Path.ChangeExtension(Path.GetRandomFileName(), extension);
        
        await using var fileStream = await file.OpenReadAsync();
        var filePath = Path.Combine(FileSystem.Current.CacheDirectory, newName);
        var fileInfo = new FileInfo(filePath);
        
        if (fileInfo.Exists)
        {
            fileInfo.Delete();
        }
        
        await using var storageStream = File.Create(filePath);
        await fileStream.CopyToAsync(storageStream);
        
        return newName;
    }
    
    public static ImageSource GetImage(string imageName)
    {
        if (string.IsNullOrEmpty(imageName))
        {
            return null;
        }
        var filePath = Path.Combine(FileSystem.Current.CacheDirectory, imageName);
        if (File.Exists(filePath))
        {
            var fileStream = File.OpenRead(filePath);
        
            return ImageSource.FromStream(() => fileStream);
        }

        return null;
    }

    public static void RemoveImage(string imageName)
    {
        if (string.IsNullOrEmpty(imageName)) return;
        var filePath = Path.Combine(FileSystem.Current.CacheDirectory, imageName);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}