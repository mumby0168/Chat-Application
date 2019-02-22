using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking.Client.Application.Services
{
    public interface IFileProcessorService
    {
        Task<byte[]> GetBytesFromImage(string imagePath);

        string SelectFile(string fileTypes = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png");
    }
}
