using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Networking.Client.Application.Models;

namespace Networking.Client.Application.Converters
{
    /// <summary>
    /// Hanldes teh selection of UI Template to use dependant on model type.
    /// </summary>
    class ChatTypeTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DependencyObject parent = container;
            bool isUserControl = false;
            do
            {
                parent = VisualTreeHelper.GetParent(parent);

                if (parent == null) throw new InvalidOperationException("No Parent user control was found.");

                if (parent is UserControl)
                    isUserControl = true;

            } while (!isUserControl);

            var usercontrol = parent as UserControl;


            if(item is ChatMessageModel)
                return usercontrol.Resources["TextTemplate"] as DataTemplate;            
            else if(item is ImageMessageModel)
                return usercontrol.Resources["ImageTemplate"] as DataTemplate;

            throw new InvalidOperationException("No template was found to match the type.");
        }
    }
}
