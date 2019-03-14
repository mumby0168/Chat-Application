using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking.Client.Application.Static
{
    public static class MainThread
    {
        public static void Dispatch(Action func)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(func);
        }
    }
}