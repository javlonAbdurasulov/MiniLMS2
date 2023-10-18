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
    public class LoginValidationAttribute:ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var Login = validationContext.ObjectInstance as Student;

            if (LoginCheck(Login.Login))
                return new ValidationResult("Login is Incorrect format!");
            
            return base.IsValid(value, validationContext);
        }
        public bool LoginCheck(string login)
        {
            Regex regex = new Regex("[a-zA-Z0-9]");
            if(regex.IsMatch(login)) return true;
            return false;
        }
    }
}
