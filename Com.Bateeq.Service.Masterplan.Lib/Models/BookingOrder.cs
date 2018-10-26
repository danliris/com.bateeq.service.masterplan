using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.Bateeq.Service.Masterplan.Lib.Models
{
    public class BookingOrder : StandardEntity, IValidatableObject
    {
        public string Code { get; set; }
        public int SerialNumber { get; set; }
        public int SectionId { get; set; }
        public string SectionCode { get; set; }
        public string SectionName { get; set; }
        public DateTimeOffset BookingDate { get; set; }
        public int BuyerId { get; set; }
        public string BuyerCode { get; set; }
        public string BuyerName { get; set; }
        public int OrderQuantity { get; set; }
        public int? InitialOrderQuantity { get; set; }
        public DateTimeOffset DeliveryDate { get; set; }
        public string Remark { get; set; }
        public virtual ICollection<BookingOrderDetail> DetailConfirms { get; set; }
        public int? BlockingPlanId { get; set; }
        public Boolean? IsModified { get; set; }
        public virtual BlockingPlan BlockingPlan { get; set; }
        public int CanceledBookingOrder { get; set; }
        public DateTimeOffset CanceledDate { get; set; }
        public int ExpiredBookingOrder { get; set; }
        public DateTimeOffset ExpiredDeletedDate { get; set; }



        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}
