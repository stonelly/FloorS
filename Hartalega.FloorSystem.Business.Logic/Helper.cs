using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic
{
    public static class Helper
    {
        public static string FixNewLine(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            var tempNewLine = "[#newline#]";
            return text.Replace("\r\n", tempNewLine).Replace("\n", Environment.NewLine).Replace("\r", Environment.NewLine).Replace(tempNewLine, Environment.NewLine);
        }
    }
}
