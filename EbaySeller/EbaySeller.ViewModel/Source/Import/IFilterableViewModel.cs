using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EbaySeller.Common.DataInterface;

namespace EbaySeller.ViewModel.Source.Import
{
    public interface IFilterableViewModel
    {
        Dictionary<string, List<IArticle>> GetListsToFilter();

        void SetFilteredLists(Dictionary<string, List<IArticle>> filteredArticleLists);

        List<IArticle> GetFilteredList();
    }
}
