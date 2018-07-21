using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DT = Windows.ApplicationModel.DataTransfer;

namespace MethodStore
{
    internal class Clipboard
    {
        internal void SetText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            DT.DataPackage dataPackage = new DT.DataPackage();
            dataPackage.SetText(text);

            DT.Clipboard.SetContent(dataPackage);
        }

        internal string GetText()
        {
            return GetTextAsync().Result;
        }

        private static async Task<string> GetTextAsync()
        {
            DT.DataPackageView dataContent = DT.Clipboard.GetContent();

            if (dataContent.AvailableFormats.Contains("Text"))
                return await dataContent.GetTextAsync("Text");

            return string.Empty;
        }
    }
}