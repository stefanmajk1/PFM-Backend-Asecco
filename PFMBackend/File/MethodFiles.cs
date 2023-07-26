namespace PFMBackend.File
{
    public static class MethodFiles
    {
        public async static Task<string> GetFilePath(IFormFile file)
        {
            var filePath = Path.GetTempFileName();

            var stream = System.IO.File.Create(filePath);

            await file.CopyToAsync(stream);

            stream.Close();

            return filePath;
        }
    }
}
