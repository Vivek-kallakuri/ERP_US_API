using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PDFUploader.Models;

namespace PDFUploader.Controllers
{
    public class FileMetadatumsController : Controller
    {
        private readonly MyDbContext _context;

        // Uploaded files will be stored in the file called Uploads.
        private readonly string _uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

        public FileMetadatumsController(MyDbContext context)
        {
            _context = context;
        }


        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file, string uploadedBy)
        {
            if (file == null || file.Length == 0) return BadRequest("File is empty.");

            // Ensure the directory exists or Not, if not, it creates the directory
            if (!Directory.Exists(_uploadFolderPath))
            {
                Directory.CreateDirectory(_uploadFolderPath);
            }

            // Extracts the filename from the full path
            var fileName = Path.GetFileName(file.FileName);

            //Creates the full file path where the file will be saved
            var filePath = Path.Combine(_uploadFolderPath, fileName);

            // 1: Creates a new file at the specified filePath using FileStream.
            // 2: Opens the file in Create mode.
            // 3: Copies the uploaded file's content into the new file asynchronously
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }


            var fileMetadata = new FileMetadatum
            {
                FileName = fileName,
                FilePath = filePath,
                UploadedBy = uploadedBy
            };

            _context.FileMetadata.Add(fileMetadata);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "File uploaded successfully", FileId = fileMetadata.Id });
        }


        [HttpGet("download/all")]
        public async Task<IActionResult> GetAllFiles()
        {
            var files = await _context.FileMetadata.ToListAsync();

            if (files == null || files.Count == 0)
                return NotFound("No files found.");

            return Ok(files);
        }


        [HttpGet("download/{id}")]
        public async Task<IActionResult> DownloadFile(int id)
        {
            var fileMetadata = await _context.FileMetadata.FindAsync(id);
            if (fileMetadata == null)
                return NotFound("File not found in database.");

            var filePath = fileMetadata.FilePath;
            if (!System.IO.File.Exists(filePath))
                return NotFound("File does not exist on the server.");

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

            return File(fileBytes, "application/pdf", fileMetadata.FileName);
        }


    }
}
