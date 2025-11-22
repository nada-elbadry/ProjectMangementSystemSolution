using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementBLL.Services.Interfaces
{
    public interface IAttachment_Service
    {
        string? Upload(string FolderName, IFormFile File);

        bool Delete(string FileName, string FolderName);
    }
}
