using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EbaySeller.Common.DataInterface;

namespace EbaySeller.ViewModel.Source.Filter
{
    public class SummerFilterCriteria : IFilterCriteria<IWheel>
    {
        public bool IsInFilter(IWheel article)
        {
            return !article.IsMudSnow && !article.IsWinter;
        }
    }
}
