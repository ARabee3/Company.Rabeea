namespace Company.Rabeea.PL.Helpers;

public static class AttachmentSettings
{
    // upload,delete
    public static string UploadFile(IFormFile file,string folderName)
    {
        // File path :
        // -Folder Location
        //var folderPath = Directory.GetCurrentDirectory() + "\\\\wwwroot\\\\files\\" + folderName;
        var folderPath = Path.Combine(Directory.GetCurrentDirectory() , @"wwwroot\files" , folderName);
        // 2. File Name
        var fileName = $"{Guid.NewGuid()}{file.FileName}";
        var filePath = Path.Combine(folderPath, fileName);
        using var fileStream = new FileStream(filePath, FileMode.Create);
        file.CopyTo(fileStream);
        return fileName;
    }

    public static void DeleteFile(string fileName, string folderName)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", folderName, fileName);
        if (File.Exists(filePath)) File.Delete(filePath);
    }
}
