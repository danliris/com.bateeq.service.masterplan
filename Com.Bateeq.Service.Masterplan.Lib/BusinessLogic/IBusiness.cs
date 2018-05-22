using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Bateeq.Service.Masterplan.Lib.BusinessLogic
{
    public interface IBusiness<TModel, TViewModel>
        where TModel : class
        where TViewModel : class
    {
        Tuple<List<TModel>, int, Dictionary<string, string>, List<string>> ReadModel(int Page, int Size, string Order, List<string> Select, string Keyword, string Filter);
        IQueryable<TModel> ConfigureSearch(IQueryable<TModel> Query, List<string> SearchAttributes, string Keyword);
        IQueryable<TModel> ConfigureFilter(IQueryable<TModel> Query, Dictionary<string, object> FilterDictionary);
        IQueryable<TModel> ConfigureOrder(IQueryable<TModel> Query, Dictionary<string, string> OrderDictionary);
        TViewModel MapToViewModel(TModel model);
        TModel MapToModel(TViewModel viewModel);
        void Validate(TModel model);
        void Validate(TViewModel viewModel);
        Task<TModel> GetAsync(int id);
        Task<int> UpdateAsync(int id, TModel model);
        bool IsExists(int id);
        Task<int> CreateAsync(TModel model);
        Task<int> DeleteAsync(int id);
    }
}
