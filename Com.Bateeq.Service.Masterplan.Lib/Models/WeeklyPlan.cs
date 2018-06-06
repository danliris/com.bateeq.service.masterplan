using Com.Moonlay.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Com.Bateeq.Service.Masterplan.Lib.Models
{
    public class WeeklyPlan : StandardEntity, IValidatableObject
    {
        public int Year { get; set; }
        public string UnitId { get; set; }
        public string UnitCode { get; set; }
        public string UnitName { get; set; }
        public virtual ICollection<WeeklyPlanItem> Items { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            using (var context = (MasterplanDbContext)validationContext.GetService(typeof(MasterplanDbContext)))
            {
                if (context != null)
                {
                    var index = context.WeeklyPlans.Count(p => p.IsDeleted.Equals(false) && p.Id != this.Id && p.Year.Equals(this.Year) && p.UnitCode.Equals(this.UnitCode));

                    if (index > 0)
                    {
                        yield return new ValidationResult($"Year {this.Year} & Unit Code with {this.UnitCode} is already exists", new List<string> { "Year" });
                    }
                }

            }
        }
    }
}
