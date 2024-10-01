using Microsoft.AspNetCore.Hosting;

namespace Library.Services
{
    public class PathHelper
    {
        private IWebHostEnvironment _webHostEnviroment;

        public PathHelper(IWebHostEnvironment webHostEnviroment)
        {
            _webHostEnviroment = webHostEnviroment;
        }

        public string GetPathToBookCover(int bookId)
        {
            var fileName = $"cover-{bookId}.jpg";
            return GetPathByFolder("images\\book covers", fileName);
        }

        public bool IsBookCoverExist(int bookId)
        {
            var path = GetPathToBookCover(bookId);
            return File.Exists(path);
        }

        private string GetPathByFolder(string pathToFolder, string fileName)
        {
            var path = Path.Combine(_webHostEnviroment.WebRootPath, pathToFolder, fileName);
            return path;
        }
    }
}
