namespace Scooters.Services;

public static class ImageService
{
    public static async Task<string> SaveImageAsync(FileResult file)
    {
        var extension = Path.GetExtension(file.FileName);
        var newName = Path.ChangeExtension(Path.GetRandomFileName(), extension);
        
        await using var stream = await file.OpenReadAsync();
        var filePath = Path.Combine(FileSystem.Current.AppDataDirectory, newName);
        var fileInfo = new FileInfo(filePath);
        
        var files = Directory.GetFiles(FileSystem.Current.AppDataDirectory);
        
        if (fileInfo.Exists)
        {
            fileInfo.Delete();
        }
        
        await using var fileStream = File.Create(filePath);
        await stream.CopyToAsync(fileStream);
        
        return filePath;
        
        // var extension = Path.GetExtension(file.FileName);
        // var newName = Path.ChangeExtension(Path.GetRandomFileName(), extension);
        //
        // var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        //
        // var files = Directory.GetFiles(folderPath);
        //
        // var filePath = Path.Combine(folderPath, newName);
        // using (var fileStream = File.Create(filePath))
        // {
        //     await fileStream.CopyToAsync(fileStream);
        // }
        //
        // return filePath;
    }
    
    public static string GetImagePath(string imageName)
    {
        var filePath = Path.Combine(FileSystem.Current.AppDataDirectory, imageName);
        return File.Exists(filePath) ? filePath : string.Empty;
    }
}