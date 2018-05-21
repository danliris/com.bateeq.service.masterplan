using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Bateeq.Service.Masterplan.Lib.ViewModels
{
    public class CommodityViewModel : BasicViewModel, IValidatableObject
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(this.Code))
            {
                yield return new ValidationResult("Commodity Code must be not empty", new List<string> { "Code" });
            }

            if (string.IsNullOrWhiteSpace(this.Name))
            {
                yield return new ValidationResult("Commodity Name must be not empty", new List<string> { "Name" });
            }
        }
    }
}
