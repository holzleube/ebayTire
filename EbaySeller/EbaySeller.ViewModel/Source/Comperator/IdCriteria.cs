using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EbaySeller.Common.DataInterface;

namespace EbaySeller.ViewModel.Source.Comperator
{
    public class IdCriteria:ICompareCriteria
    {
        public bool IsCriteriaSatisfied(IArticle originalArticle, IArticle newArticle)
        {
            return originalArticle.ArticleId.Equals(newArticle.ArticleId);
        }
    }
}
