using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Common
{
    public class BirthdayValidationAttribute : ValidationAttribute
    {
        private readonly DateTime _date;

        public BirthdayValidationAttribute(string date)
        {
            _date = DateTime.Parse(date);
            ErrorMessage = $"Date must be before {_date:dd-MM-yyyy}.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateValue && dateValue >= _date)
            {
                return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }
    }
}
