using System;
using Humanizer;
using Microsoft.AspNetCore.Mvc;

namespace portfolio_builder_server.Controllers;

public class FileController : BaseApiController
{
    private readonly string _uploadPath;

    public FileController()
    {
        _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "upload");
        if (!Directory.Exists(_uploadPath))
        {
            Directory.CreateDirectory(_uploadPath);
        }
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file selected");
        }

        var filePath = Path.Combine(_uploadPath, file.FileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Ok(new { filePath });
    }

        [HttpDelete("delete")]
        public IActionResult DeleteFile([FromQuery] string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return BadRequest("No file name provided");

            var filePath = Path.Combine(_uploadPath, fileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound("File not found");

            System.IO.File.Delete(filePath);

            return Ok("File deleted");
        }

}
