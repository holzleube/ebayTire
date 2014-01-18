using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EbaySeller.Common.DataInterface;

namespace EbaySeller.ViewModel.Source.Filter
{
    public class WidthHeightFilter: IFilterCriteria<IWheel>
    {
        private List<int> allowedWidths = new List<int>(){205, 195, 175, 185};
        private List<int> allowedHeights = new List<int>(){55, 65, 60};
        
        public bool IsInFilter(IWheel article)
        {
            if (!allowedWidths.Contains(article.WheelWidth))
            {
                return false;
            }
            //if (!allowedHeights.Contains(article.WheelHeight))
            //{
            //    return false;
            //}
            return true;
        }
    }
}
