using EbaySeller.Common.DataInterface;
using EbaySeller.Model.Source.Ebay.Constants;
using eBay.Service.Core.Soap;

namespace EbaySeller.Model.Source.Ebay.ArticleBuilders
{
    public class EbayItemSpecificsBuilder
    {
        public NameValueListTypeCollection GetItemSpecifics(IWheel wheel)
        {
            var result = new NameValueListTypeCollection();
            result.Add(GetSingleItemSpecific("Reifenhersteller", wheel.Manufactorer));
            result.Add(GetSingleItemSpecific("Hersteller-Artikelnummer", wheel.ArticleId));
            result.Add(GetSingleItemSpecific("Reifenspezifikation", GetWheelSpecification(wheel)));
            result.Add(GetSingleItemSpecific("Reifenbreite", wheel.WheelWidth.ToString()));
            result.Add(GetSingleItemSpecific("Reifenquerschnitt", wheel.WheelHeight.ToString()));
            result.Add(GetSingleItemSpecific("Zollgröße", wheel.CrossSection.Replace('R', ' ')));
            result.Add(GetSingleItemSpecific("Tragfähigkeitsindex", wheel.WeightIndex.ToString()));
            result.Add(GetSingleItemSpecific("Geschwindigkeitsindex", wheel.SpeedIndex.ToString()));
            result.Add(GetSingleItemSpecific("Winterreifen (Ja/Nein)", wheel.IsWinter ? "Ja" : "Nein"));
            result.Add(GetSingleItemSpecific("Reifengröße", GetWheelSize(wheel)));
            return result;
        }

        private string GetWheelSpecification(IWheel wheel)
        {
            if (wheel.IsWinter)
            {
                return "Winterreifen";
            }
            if (wheel.IsMudSnow)
            {
                return "Ganzjahresreifen";
            }
            return "Sommerreifen";
        }

        private NameValueListType GetSingleItemSpecific(string key, string value)
        {
            NameValueListType nv1 = new NameValueListType();
            nv1.Name = key;
            StringCollection nv1Col = new StringCollection();
            nv1Col.Add(value);
            nv1.Value = nv1Col;
            return nv1;
        }

        private string GetWheelSize(IWheel wheel)
        {
            return string.Format(EbayArticleConstants.EbaySpecificationTemplate, wheel.WheelWidth, wheel.WheelHeight,
                                 wheel.CrossSection, wheel.WeightIndex, wheel.SpeedIndex);
        }
    }
}
