using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using FileDetailsViewTable.Models;
using X.PagedList;

namespace FileDetailsViewTable.Controllers
{
    public class FileController : Controller
    {
        private readonly IConfiguration _configuration;

        public FileController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string GetFolderPath()
        {
            return _configuration["AppSettings:FolderPath"];
        }

        private const int PageSize = 5;

        public IActionResult Index(int? page = 1)
        {
            string folderPath = GetFolderPath();
            List<FileTable> allFiles = GetZipFiles(folderPath);
            allFiles = allFiles.OrderBy(f => f.DateModified).ToList();
            var pagedList = allFiles.ToPagedList(page ?? 1, PageSize);

            return View(pagedList);
        }

        private List<FileTable> GetZipFiles(string folderPath)
        {
            List<FileTable> files = new List<FileTable>();

            if (Directory.Exists(folderPath))
            {
                var zipFiles = Directory.GetFiles(folderPath, "*.zip");

                foreach (var zipFile in zipFiles)
                {
                    using (ZipArchive archive = ZipFile.OpenRead(zipFile))
                    {
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            if (Path.GetExtension(entry.FullName).Equals(".zip", StringComparison.OrdinalIgnoreCase))
                            {
                                files.Add(new FileTable
                                {
                                    Name = entry.Name,
                                    DateModified = entry.LastWriteTime.DateTime,
                                    Type = Path.GetExtension(entry.FullName),
                                    Size = entry.Length,
                                    FilePath = zipFile
                                });
                            }
                        }
                    }
                }
            }

            return files;
        }

        public IActionResult Download(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, "application/zip", Path.GetFileName(filePath));
            }
            else
            {
                return NotFound();
            }
        }
    }
}