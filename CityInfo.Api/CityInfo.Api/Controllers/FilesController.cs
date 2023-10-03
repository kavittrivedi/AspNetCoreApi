﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.Api.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

        public FilesController(
            FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider
                ?? throw new System.ArgumentNullException(
                    nameof(fileExtensionContentTypeProvider)
                    );
        }

        [HttpGet("{fileId}")]
        public ActionResult GetFile(string fileId)
        {
            // look up the actula file, depending on the fileId...
            // demo code
            var pathToFile = "Invoice OD222399819111678000.pdf";

            // Check whether the file exists.
            if (!System.IO.File.Exists(pathToFile))
            { 
                return NotFound();
            }

            if (!_fileExtensionContentTypeProvider.TryGetContentType(
                pathToFile, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var bytes = System.IO.File.ReadAllBytes(pathToFile);

            //return File(bytes,"text/plain",Path.GetFileName(pathToFile));
            return File(bytes, contentType, Path.GetFileName(pathToFile));
        }
    }
}
