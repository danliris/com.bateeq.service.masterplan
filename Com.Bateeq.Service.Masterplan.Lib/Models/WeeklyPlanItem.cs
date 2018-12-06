using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Com.Bateeq.Service.Masterplan.Lib.Models
{
    public class WeeklyPlanItem : StandardEntity, IValidatableObject
    {
        public int WeeklyPlanId { get; set; }
        public virtual WeeklyPlan WeeklyPlan { get; set; }
        public string WeekNumber { get; set; }
        public int Month { get; set; }
        public int Efficiency { get; set; }
        public int Operator { get; set; }
        public int WorkingHours { get; set; }
        public double AhTotal { get; set; }
        public double EhTotal { get; set; }
        public double UsedEh { get; set; }
        public double RemainingEh { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            using (var context = (MasterplanDbContext)validationContext.GetService(typeof(MasterplanDbContext)))
            {
                var index = context.WeeklyPlanItems.Count(p => p.IsDeleted.Equals(false) && p.Id != this.Id);

                if (index > 0)
                {
                    yield return new ValidationResult($"Weekly-Plan-Item with this Id = {this.Id} is already exists", new List<string> { "Id" });
                }
            }
        }
    }
}
