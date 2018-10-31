using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.Bateeq.Service.Masterplan.Lib.Models
{
    public class BookingOrderDetail : StandardEntity, IValidatableObject
    {
        public int BookingOrderId { get; set; }
        public virtual BookingOrder BookingOrder { get; set; }
        public string RO { get; set; }
        public string Article { get; set; }
        public string Style { get; set; }
        public string Counter { get; set; }
        public int Total { get; set; }
        public DateTimeOffset DeliveryDate { get; set; }
        public DateTimeOffset ConfirmDate { get; set; }
        public string Remark { get; set; }
        public bool isAddNew { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}
