using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EbaySeller.Common.DataInterface;

namespace EbaySeller.ViewModel.Source.Comperator
{
    public class ArticleCrudComperator
    {
        private List<ICompareCriteria> criterias;

        public ArticleCrudComperator()
        {
            criterias = new List<ICompareCriteria> {new IdCriteria(), new AvailabilityCriteria(), new PriceCriteria()};
        }
        public bool AreBothArticleEqual(IArticle originalArticle, IArticle newArticle)
        {
            foreach (var compareCriteria in criterias)
            {
                if (!compareCriteria.IsCriteriaSatisfied(originalArticle, newArticle))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
