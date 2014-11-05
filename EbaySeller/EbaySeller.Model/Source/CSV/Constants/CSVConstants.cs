using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EbaySeller.Model.Source.CSV.Constants
{
    public class CSVConstants
    {
        public const string FirstLineOfCsvFile =
            "id|article_id|description|description_2|price|price_4|avg_price|anonym_price|rvo_price|availability|manufacturer_number|image|image_tn|info|manufacturer|directlink|tyrelabel_link|ebayId|ebayId2|ebayId4|LastSync";

        public const string FirstLineOfGambioCSV =
            "XTSOL|p_id|p_model|p_stock|p_sorting|p_startpage|p_startpage_sort|p_shipping|p_tpl|p_opttpl" +
            "|p_manufacturer|p_fsk18|p_priceNoTax|p_priceNoTax.1|p_priceNoTax.2|p_priceNoTax.3|p_tax|p_status|p_weight|p_ean" +
            "|code_isbn|code_upc|code_mpn|code_jan|brand_name|p_disc|p_date_added|p_last_modified|p_date_available|p_ordered" +
            "|nc_ultra_shipping_costs|gm_show_date_added|gm_show_price_offer|gm_show_weight|gm_show_qty_info|gm_price_status|gm_min_order|gm_graduated_qty|gm_options_template|p_vpe" +
            "|p_vpe_status|p_vpe_value|p_image.1|p_image.2|p_image.3|p_image|p_name.en|p_desc.en|p_shortdesc.en|p_checkout_information.en" +
            "|p_meta_title.en|p_meta_desc.en|p_meta_key.en|p_keywords.en|p_url.en|gm_url_keywords.en|p_name.de|p_desc.de|p_shortdesc.de" +
            "|p_checkout_information.de|p_meta_title.de|p_meta_desc.de|p_meta_key.de|p_keywords.de|p_url.de|gm_url_keywords.de|p_cat.0|p_cat.1|p_cat.2" +
            "|p_cat.3|p_cat.4|p_cat.5";

        public const string DataFormatLine =
            "{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}|{16}|{17}|{18}|{19}|{20}"; 
        
        public const string GambioFormatLine =
            "XTSOL|{0}|{1}|{2}|0|0|0|1|default|default" +
            "||0|{3}||||1|1|0|" +
            "|||||{4}|0|{5}|||0|0" +
            "|0|0|0|0|0|1|1|default|0|0" +
            "|0|{6}|{7}||{8}||||||" +
            "|{9}|{10}||{11}|{12}|{13}|{14}|||" +
            "|{15}|{16}||{17}|{18}||";

        public const string PrestoFirstLine =
            "ID;Active (0/1);Name *;Categories (x,y,z...);Price tax excluded or Price tax included;" +
            "Tax rules ID;Wholesale price;On sale (0/1);Discount amount;Discount percent;" +
            "Discount from (yyyy-mm-dd);Discount to (yyyy-mm-dd);Reference #;Supplier reference #;Supplier;" +
            "Manufacturer;EAN13;UPC;Ecotax;Width;" +
            "Height;Depth;Weight;Quantity;Minimal quantity;" +
            "Visibility;Additional shipping cost;Unity;Unit price;Short description;" +
            "Description;Tags (x,y,z...);Meta title;Meta keywords;Meta description;" +
            "URL rewritten;Text when in stock;Text when backorder allowed;Available for order (0 = No, 1 = Yes);Product available date;" +
            "Product creation date;Show price (0 = No, 1 = Yes);Image URLs (x,y,z...);Delete existing images (0 = No, 1 = Yes);Feature(Name:Value:Position);" +
            "Available online only (0 = No, 1 = Yes);Condition;Customizable (0 = No, 1 = Yes);Uploadable files (0 = No, 1 = Yes);Text fields (0 = No, 1 = Yes);" +
            "Out of stock;ID / Name of shop;Advanced stock management;Depends On Stock;Warehouse";
        
        public const string PrestoFormatLine = "{0};1;{1};{2};{3};" +
                                               "53;{4};0;;;" +
                                               ";;;;;" +
                                               "{5};;;;" +
                                               ";;;;{6};" +
                                               "1;;;;;" +
                                               "{7};{8};{9};;;;" +
                                               ";;;1;" +
                                               ";;1;{10};0;" +
                                               ";0;new;0;0;" +
                                               "0;0;0;0;0;0";
    }
}
