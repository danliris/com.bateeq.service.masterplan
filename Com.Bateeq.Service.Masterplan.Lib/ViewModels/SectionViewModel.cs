using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Bateeq.Service.Masterplan.Lib.ViewModels
{
    public class SectionViewModel : BasicViewModel, IValidatableObject
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(this.Code))
            {
                yield return new ValidationResult("Section Code must be not empty", new List<string> { "Code" });
            }

            if (string.IsNullOrWhiteSpace(this.Name))
            {
                yield return new ValidationResult("Section Name must be not empty", new List<string> { "Name" });
            }

            if (string.IsNullOrWhiteSpace(this.Remark))
            {
                yield return new ValidationResult("Section Information must be not empty", new List<string> { "Remark" });
            }
        }
    }
}
