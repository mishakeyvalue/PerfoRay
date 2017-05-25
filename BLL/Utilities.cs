using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    class Utilities
    {
        static string[] mimeTypes =
{
                "text/html",
                "application/xhtml+xml"
            };

        public static bool IsHtmlMimeType(string mimeType)
        {
            foreach (var type in mimeTypes)
            {
                if (string.Equals(mimeType, type, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}
