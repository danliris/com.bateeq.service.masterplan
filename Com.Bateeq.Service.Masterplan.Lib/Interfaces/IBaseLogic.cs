using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Bateeq.Service.Masterplan.Lib.Interfaces
{
    public interface IBaseLogic<TModel>
    {
        Tuple<List<TModel>, int, Dictionary<string, string>, List<string>> ReadModel(int page, int size, string order, List<string> select, string keyword, string filter);
        void CreateModel(TModel model);
        Task<TModel> ReadModelById(int id);
        void UpdateModel(int id, TModel model);
        Task DeleteModel(int id);
    }
}
