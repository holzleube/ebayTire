using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EbaySeller.Common.DataInterface;

namespace EbaySeller.ViewModel.Source.Filter
{
    public class CrossSectionFilter: IFilterCriteria<IWheel>
    {
        public bool IsInFilter(IWheel article)
        {
            if (article.CrossSection == null)
            {
                return false;
            }
            var cross = article.CrossSection.Replace("R", "");
            try
            {
                int crossSection = Int32.Parse(cross);
                return crossSection < 19;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
    }
}
