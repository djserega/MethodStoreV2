using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

namespace MethodStore
{
    internal class TextParser
    {
        internal Models.Method MethodInClipboard { get; private set; }

        internal async Task GetTextInClipboard()
        {
            DataPackageView dataContent = Clipboard.GetContent();

            if (dataContent.AvailableFormats.Contains("Text"))
            {
                string originalText = await dataContent.GetTextAsync("Text");

                if (string.IsNullOrWhiteSpace(originalText))
                    return;

                MethodInClipboard = new Models.Method();

                string text = originalText;

                text = RemoveStartText(text);

                int countDot = text.Count(f => f == '.');
                int countOpeningBracket = text.Count(f => f == '(');
                int countClosingBracket = text.Count(f => f == ')');

                if (countDot >= 1 && countOpeningBracket == 1 && countClosingBracket == 1)
                {
                    string textObjectName = text.GetTextBefore();

                    if (textObjectName.Count(f => f == '=') > 0)
                        MethodInClipboard.ObjectName = textObjectName.GetTextAfter('=');
                    else
                        MethodInClipboard.ObjectName = textObjectName;

                    text = text.RemoveStartText(MethodInClipboard.ObjectName, '.');

                    MethodInClipboard.MethodName = text.GetTextBefore('(');
                }

            }
        }

        private string RemoveStartText(string text)
        {
            List<Models.RemovingText> removingTexts = new EF.Context<Models.RemovingText>().GetList();

            List<Models.RemovingText> removingTextsStart = removingTexts.FindAll(f => f.Type == TypesRemovingText.Start);
            foreach (Models.RemovingText item in removingTextsStart)
            {
                text = text.RemoveStartText(item.Text + " ");
            }
            text.TrimStart();
            return text;
        }
    }
}
