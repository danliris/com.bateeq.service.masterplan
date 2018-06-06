using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Bateeq.Service.Masterplan.Lib.Interfaces
{
    public interface IBaseFacade<TModel>
    {
        Tuple<List<TModel>, int, Dictionary<string, string>, List<string>> Read(int Page, int Size, string Order, List<string> Select, string Keyword, string Filter);
        Task<int> Create(TModel model);
        Task<TModel> ReadById(int id);
        Task<int> Update(int id, TModel model);
        Task<int> Delete(int id);
    }
}
