using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Com.Bateeq.Service.Masterplan.Lib.ViewModels
{
    public class SectionViewModel : StandardEntity, IValidatableObject
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            MasterplanDbContext dbContext = (MasterplanDbContext)validationContext.GetService(typeof(MasterplanDbContext));

            if (dbContext.Sections.Count(p => p.IsDeleted.Equals(false) && p.Id != this.Id && p.Code.Equals(this.Code)) > 0)
            {
                yield return new ValidationResult($"Section Code with {this.Code} is already exists", new List<string> { "Code" });
            }

            if (string.IsNullOrWhiteSpace(this.Code))
            {
                yield return new ValidationResult("Section Code must be not empty", new List<string> { "Code" });
            }

            if (string.IsNullOrWhiteSpace(this.Name))
            {
                yield return new ValidationResult("Section Name must be not empty", new List<string> { "Name" });
            }
        }
    }
}
