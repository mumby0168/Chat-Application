using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Networking.Client.Application.Properties;

namespace Networking.Client.Application.Converters
{
   /// <summary>
   /// Handles the conversion of a byte array to a bitmap image and the reverse.
   /// </summary>
    public class ByteArrayToBitMapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            byte[] bytes = (byte[]) value;

            if(bytes == null)
            {
                return Resources.BlankProfilePic;
            }

            var bitmap = new BitmapImage { };
            bitmap.BeginInit();
            bitmap.StreamSource = new MemoryStream(bytes);
            bitmap.EndInit();

            return bitmap;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Hanldes the conversion of a byte array to stream source and the reverse.
    /// </summary>
    public class ByteArrayToStreamSource : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            byte[] bytes = (byte[])value;

            if (bytes == null) throw new NullReferenceException();

            var bitmap = new BitmapImage { };
            bitmap.BeginInit();
            bitmap.StreamSource = new MemoryStream(bytes);
            bitmap.EndInit();

            return bitmap;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
