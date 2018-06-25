using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MethodStore
{
    internal static class Navigating
    {
        internal static void Navigate(Type typePage, params object[] param)
        {
            ParametersNavigating parameters = new ParametersNavigating()
            {
                Parameters = param
            };

            Frame frame = Window.Current.Content as Frame;
            frame.Navigate(typePage, parameters);
        }
    }
}
