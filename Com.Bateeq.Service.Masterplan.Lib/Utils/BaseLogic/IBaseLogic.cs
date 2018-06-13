using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.Bateeq.Service.Masterplan.Lib.Utils.BaseLogic
{
    public interface IBaseLogic<TModel>
    {
        ReadResponse<TModel> ReadModel(int page, int size, string order, List<string> select, string keyword, string filter);
        void CreateModel(TModel model);
        Task<TModel> ReadModelById(int id);
        void UpdateModel(int id, TModel model);
        Task DeleteModel(int id);
    }
}
