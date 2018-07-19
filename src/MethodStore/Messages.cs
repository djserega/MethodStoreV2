using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace MethodStore
{
    internal static class Messages
    {
        internal static async void Show(string content, string title = "")
        {
            if (string.IsNullOrWhiteSpace(title))
                title = "Хранилище методов";

            ContentDialog noWifiDialog = new ContentDialog
            {
                Content = content,
                Title = title,
                CloseButtonText = "ОК"
            };

            try
            {
                ContentDialogResult result = await noWifiDialog.ShowAsync();
            }
            catch (Exception)
            {
            }
        }

        internal static async Task<ContentDialogResult> Question(string content, string title = "")
        {
            if (string.IsNullOrWhiteSpace(title))
                title = "Хранилище методов";

            ContentDialog noWifiDialog = new ContentDialog
            {
                Content = content,
                Title = title,
                CloseButtonText = "Отмена",
                PrimaryButtonText = "OK"
            };

            return await noWifiDialog.ShowAsync();
        }
    }
}
