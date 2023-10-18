using MiniLMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLMS.Domain.CustomeValidation
{
    public class NameValidationAttribute:ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var name = validationContext.ObjectInstance as Student;

            if (!Char.IsUpper(name.FullName[0]))
                return new ValidationResult("Name must start with Upper!");

            return base.IsValid(value, validationContext);
        }
    }
}
