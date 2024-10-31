using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Common
{
    public class FullNameValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var upperWord = "qwertyuiopasdfghjklzxcvbnm".ToUpper();
            string validationChar = @"qwertyuiopasdfghjklzxcvbnm@#1234567890 ".ToUpper();

            if (value is string fullName)
            {
                var words = fullName.Split(' ');
                foreach (var word in words)
                {
                    char[] letters = word.Trim().ToCharArray();
                    if (!upperWord.Contains(letters[0]))
                    {
                        return new ValidationResult(@"Each word of the full name must begin with the capital letter.");
                    }
                    foreach (var letter in letters)
                    {
                        if (!validationChar.Contains(letter.ToString().ToUpper()))
                        {
                            return new ValidationResult(@"Value for Fullname includes a-z, A-Z, space, @, # and digit 0-9.");
                        }
                    }
                }
                return ValidationResult.Success;
            }
            return new ValidationResult("Invalid full name value.");
        }
    }
}
