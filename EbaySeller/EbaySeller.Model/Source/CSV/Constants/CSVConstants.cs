using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EbaySeller.Model.Source.CSV.Constants
{
    public class CSVConstants
    {
        public const string FirstLineOfCsvFile =
            "id|article_id|description|description_2|price|price_4|avg_price|anonym_price|rvo_price|availability|manufacturer_number|image|image_tn|info|manufacturer|directlink|tyrelabel_link|ebayId|LastSync";

        public const string DataFormatLine =
            "{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}|{16}|{17}|{18}";
    }
}
