using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Bateeq.Service.Masterplan.Lib.Interfaces
{
    public interface IMap<TModel, TViewModel>
    {
        TViewModel MapToViewModel(TModel model);
        TModel MapToModel(TViewModel viewModel);
    }
}
