using EbaySeller.Common.DataInterface;

namespace EbaySeller.ViewModel.Source.Comperator
{
    public interface ICompareCriteria
    {
        bool IsCriteriaSatisfied(IArticle originalArticle, IArticle newArticle);
    }
}
