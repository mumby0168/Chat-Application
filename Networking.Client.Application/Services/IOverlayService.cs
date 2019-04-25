using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking.Client.Application.Services
{
    public interface IOverlayService
    {
        void DisplayError(string title, List<string> messages = null);

        void RemoveOverlay();
    }
}
