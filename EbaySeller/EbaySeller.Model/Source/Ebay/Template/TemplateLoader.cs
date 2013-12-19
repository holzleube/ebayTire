using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EbaySeller.Model.Source.Ebay.Template
{
    public class TemplateLoader
    {
        public static string LoadTemplateFromUri(string uri)
        {
            return File.ReadAllText(@uri);
        }
    }
}
