using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StageAPI.Extensions
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ValidationTemplate : ValidationAttribute { }

    public class Password : ValidationTemplate
    {
        public override bool IsValid(object value)
        {
            var str = value as string;

            return str.Length >= Constants.PASSWORD_MIN_LENGTH && str.All(char.IsLetterOrDigit);
        }
    }
}
