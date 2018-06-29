using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Bateeq.Service.Masterplan.Lib.Models
{
    public class BlockingPlanWorkSchedule : StandardEntity, IValidatableObject
    {
        public int BlockingPlanId { get; set; }
        public virtual BlockingPlan BlockingPlan { get; set; }
        public bool isConfirmed { get; set; }
        public string RO { get; set; }
        public string Article { get; set; }
        public string Style { get; set; }
        public string Counter { get; set; }
        public int SMV_Sewing { get; set; }
        public string UnitId { get; set; }
        public string UnitText { get; set; }
        public int YearId { get; set; }
        public string YearText { get; set; }
        public int WeekId { get; set; }
        public string WeekText { get; set; }
        public int RemainingEh { get; set; }
        public int TotalOrder { get; set; }
        public string Remark { get; set; }
        public DateTimeOffset DeliveryDate { get; set; }
        public int Efficiency { get; set; }
        public double EH_Booking { get; set; }
        public double EH_Remaining { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}
