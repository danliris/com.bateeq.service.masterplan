using Com.Bateeq.Service.Masterplan.Lib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Bateeq.Service.Masterplan.Lib.ViewModels.BookingOrder
{
    public class BookingOrderDetailViewModel : BaseViewModel
    {
        public string RO { get; set; }
        public string Article { get; set; }
        public string Style { get; set; }
        public string Counter { get; set; }
        public double? Total { get; set; }
        public DateTimeOffset DeliveryDate { get; set; }
        public DateTimeOffset ConfirmDate { get; set; }
        public string Remark { get; set; }
        public bool isAddNew { get; set; }
        public bool isDeleted { get; set; } 
    }
}
