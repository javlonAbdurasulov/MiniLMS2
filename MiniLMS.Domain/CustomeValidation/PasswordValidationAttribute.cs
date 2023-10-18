using MiniLMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MiniLMS.Domain.CustomeValidation
{
    public class PasswordValidationAttribute:ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var password = validationContext.ObjectInstance as Student;

            if (!Passwordvalidation(password.Password)) 
                return new ValidationResult("Password is Incorrect!");

            return base.IsValid(value, validationContext);
        }
        public bool Passwordvalidation(string password)
        {
            Regex regex = new Regex("(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{8,15})$");
            if(regex.IsMatch(password))
            {
                return true;
            }
            return false;
        }
    }
}
