using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Bateeq.Service.Masterplan.Lib.Models
{
    public class BlockingPlan : StandardEntity, IValidatableObject
    {
        public int BookingOrderId { get; set; }
        public virtual BookingOrder BookingOrder { get; set; }
        public virtual ICollection<BlockingPlanWorkSchedule> WorkSchedules { get; set; }
        public string Status { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}
