using Com.Bateeq.Service.Masterplan.Lib.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.Bateeq.Service.Masterplan.Lib.ViewModels
{
    public class WeeklyPlanViewModel : BaseViewModel, IValidatableObject
    {
        public int Year { get; set; }
        public string UnitId { get; set; }
        public string UnitCode { get; set; }
        public string UnitName { get; set; }
        public virtual ICollection<WeeklyPlanItemViewModel> Items { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.Year != 0)
            {
                yield return new ValidationResult("Weekly-plan of year must be not equal to 0", new List<string> { "Year" });
            }

            if (string.IsNullOrWhiteSpace(this.UnitId))
            {
                yield return new ValidationResult("Unit Id must not empty", new List<string> { "UnitId" });
            }

            if (string.IsNullOrWhiteSpace(this.UnitCode))
            {
                yield return new ValidationResult("Unit Code must not empty", new List<string> { "UnitCode" });
            }

            if (string.IsNullOrWhiteSpace(this.UnitName))
            {
                yield return new ValidationResult("Unit Name must not empty", new List<string> { "UnitName" });
            }
        }
    }
}
