using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EbaySeller.Model.Source.Data.Interfaces;

namespace EbaySeller.ViewModel.Source.Comperator
{
    public class AvailabilityCriteria:ICompareCriteria
    {
        public bool IsCriteriaSatisfied(IArticle originalArticle, IArticle newArticle)
        {
            return originalArticle.Availability.Equals(newArticle.Availability);
        }
    }
}
