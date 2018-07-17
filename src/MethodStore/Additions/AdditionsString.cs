﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodStore
{
    public static class AdditionsString
    {
        public static string ReplaseStartText(this string text, KeyValuePair<string, string> keyValue)
        {
            string find = keyValue.Key;

            if (text.StartsWith(find))
                return keyValue.Value + text.Substring(find.Length);
            else
                return text;
        }

        public static string RemoveStartText(this string text, string find, char prefix = char.MinValue)
        {
            string textFind = find;
            if (prefix != char.MinValue)
                textFind += prefix;

            if (text.StartsWith(textFind))
                return text.Substring(textFind.Length);
            else
                return text;
        }

        public static string RemoveEndText(this string text, string find)
        {
            if (text.EndsWith(find))
                return text.Remove(text.Length - find.Length);
            else
                return text;
        }

        public static string RemoveSpace(this string text)
        {
            return text.Replace(" ", "");
        }

        public static string TrimNotUsedChar(this string text)
        {
            string tempString = text.TrimStart().TrimStart('=').Trim();

            if (tempString != "\"\"")
                tempString = tempString.TrimStart('"').TrimEnd('"');

            return tempString;
        }

        public static string GetTextBefore(this string text, char charSeparator = '.')
        {
            int positionSeparator = text.IndexOf(charSeparator);
            if (positionSeparator < 0)
                return string.Empty;
            else
                return text.Substring(0, positionSeparator);
        }

        public static string GetTextAfter(this string text, char charSeparator = '.', bool trim = true)
        {
            int positionSeparator = text.IndexOf(charSeparator);
            if (positionSeparator < 0)
                return text;
            else
            {
                string textAfter = text.Substring(++positionSeparator);
                if (trim)
                    return textAfter.Trim();
                else
                    return textAfter;
            }
        }

    }
}
