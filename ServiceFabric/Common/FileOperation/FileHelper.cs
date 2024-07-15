using Common.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.FileOperation
{
    public class FileHelper
    {
        public static UploadDTO UploadFileOverNetwork(IFormFile file)
        {
            UploadDTO fileNetwork;

            using (var stream = file.OpenReadStream())
            {
                byte[] fileContent;
                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    fileContent = memoryStream.ToArray();
                }

                fileNetwork = new UploadDTO(file.FileName, file.ContentType, fileContent);
            }

            return fileNetwork;
        }
    }
}
