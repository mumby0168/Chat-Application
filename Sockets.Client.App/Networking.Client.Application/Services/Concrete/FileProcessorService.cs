using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Networking.Client.Application.Services.Concrete
{
    public class FileProcessorService : IFileProcessorService
    {
        public async Task<byte[]> GetBytesFromImage(string imagePath)
        {
            return await Task.Run((() =>
            {
                var image = Image.FromFile(imagePath);

                using (var mem = new MemoryStream())
                {
                    image.Save(mem, image.RawFormat);
                    return mem.ToArray();
                }

            }));
        }

        public string SelectFile(string fileTypes = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png")
        {
            OpenFileDialog dialog = new OpenFileDialog {Filter = fileTypes};
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return dialog.FileName;
            }
            else
            {
                return null;
            }
        }
    }
}
