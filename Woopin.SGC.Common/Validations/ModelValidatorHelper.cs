using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopin.SGC.Common.Validations
{
    public class ModelValidatorHelper<T>
    {
        protected T model { get; set; }
        public ModelValidatorHelper(T model)
        {
            this.model = model;
        }

        public bool Validate(out List<ValidationResult> results)
        {
            var context = new ValidationContext(model, serviceProvider: null, items: null);
            results = new List<ValidationResult>();

            return Validator.TryValidateObject(model, context, results, true);
        }
    }
}
