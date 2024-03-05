using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using FileDetailsViewTable.Models;
using X.PagedList;

namespace FileDetailsViewTable.Controllers
{
    public class FileController : Controller
    {
        //public IActionResult Index()
        //{
        //    string folderPath = @"E:\PC's Software\Tools";

        //    List<FileTable> files = GetZipFiles(folderPath);

        //    // Sort the files list in ascending order based on DateModified
        //    files = files.OrderBy(f => f.DateModified).ToList();
        //    //files = files.OrderByDescending(f => f.DateModified).ToList();
        //    return View(files);
        //}

        private const int PageSize = 10; // Set the number of items per page

        public IActionResult Index(int? page)
        {
            string folderPath = @"E:\PC's Software\Tools";

            List<FileTable> allFiles = GetZipFiles(folderPath);

            // Sort the files list in ascending order based on DateModified
            allFiles = allFiles.OrderBy(f => f.DateModified).ToList();

            // Paginate the files
            var pagedList = allFiles.ToPagedList(page ?? 1, PageSize);

            return View(pagedList);
        }

        private List<FileTable> PaginateFiles(List<FileTable> allFiles, int page)
        {
            return allFiles.Skip((page - 1) * PageSize).Take(PageSize).ToList();
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