using Com.Moonlay.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Com.Bateeq.Service.Masterplan.Lib.ViewModels
{
    public class CommodityViewModel : StandardEntity, IValidatableObject
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            MasterplanDbContext dbContext = (MasterplanDbContext)validationContext.GetService(typeof(MasterplanDbContext));

            if (dbContext.Commodities.Count(p => p.IsDeleted.Equals(false) && p.Id != this.Id && p.Code.Equals(this.Code)) > 0)
            {
                yield return new ValidationResult($"Commodity Code with {this.Code} is already exists", new List<string> { "Code" });
            }

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
