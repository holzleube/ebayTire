using EbaySeller.Model.Source.Data.Interfaces;

namespace EbaySeller.ViewModel.Source.Comperator
{
    public interface ICompareCriteria
    {
        bool IsCriteriaSatisfied(IArticle originalArticle, IArticle newArticle);
    }
}
