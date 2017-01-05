using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Windows.Controls;

namespace AceGrading
{
    public class NameValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value as string == "")
                return ValidationResult.ValidResult;

            double tempVal;
            if (!double.TryParse(value as string, out tempVal))
                return new ValidationResult(false, "Invalid Input");

            return ValidationResult.ValidResult;
        }
    }

    public class PointValueValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            double outVal = 0;
            if (double.TryParse(value as string, out outVal))
                return ValidationResult.ValidResult;
            else
                return new ValidationResult(false, "Combined Question Points exceed Test Point Worth");
        }
    }

}
