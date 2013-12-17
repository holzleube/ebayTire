using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EbaySeller.Model.Source.Data.Interfaces;

namespace EbaySeller.ViewModel.Source.Filter
{
    public interface IFilterCriteria<T>
    {
        bool IsInFilter(T article);
    }
}
