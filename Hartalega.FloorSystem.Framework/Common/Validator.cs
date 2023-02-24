// -----------------------------------------------------------------------
// <copyright file="Validator.cs" company="Avanade">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hartalega.FloorSystem.Framework.Common
{
    using System.Text.RegularExpressions;

    /// <summary>
    /// Cannot have instance and cannot be inherited.
    /// This class provides the feature to validate a input string against specific data type.
    /// </summary>
    public static class Validator
    {
        /// <summary>
        /// Validate input value against specific data type.
        /// </summary>
        /// <param name="type">Validation Type</param>
        /// <param name="value">Input Value</param>
        /// <returns>Valid or Invalid</returns>
        #region
        public static bool IsValidInput(Constants.ValidationType type, string value)
        {
            bool isValid = false;
            Regex regEx = null;

            if (type == Constants.ValidationType.Letter)
            {
                regEx = new Regex(RegExpPattern.Letter);
            }
            else if (type == Constants.ValidationType.Integer)
            {
                regEx = new Regex(RegExpPattern.Number);
            }
            else if (type == Constants.ValidationType.Email)
            {
                regEx = new Regex(RegExpPattern.Email);
            }
            else if (type == Constants.ValidationType.Decimal)
            {
                regEx = new Regex(RegExpPattern.Decimal);
            }
            else if (type == Constants.ValidationType.Percentage)
            {
                regEx = new Regex(RegExpPattern.Percentage);
            }
            else if (type == Constants.ValidationType.PositiveInteger)
            {
                regEx = new Regex(RegExpPattern.PositiveInteger);
            }
            else if (type == Constants.ValidationType.CreditCard)
            {
                regEx = new Regex(RegExpPattern.CreditCard);
            }
            else if (type == Constants.ValidationType.Date)
            {
                regEx = new Regex(RegExpPattern.OnlyDate);
            }
            else if (type == Constants.ValidationType.DateAndTime)
            {
                regEx = new Regex(RegExpPattern.DateAndTime);
            }
            else if (type == Constants.ValidationType.Url)
            {
                regEx = new Regex(RegExpPattern.Url, RegexOptions.IgnoreCase);
            }
            else if (type == Constants.ValidationType.Name)
            {
                regEx = new Regex(RegExpPattern.Name, RegexOptions.IgnoreCase);
            }
            else if (type == Constants.ValidationType.LettersAndNumbers)
            {
                regEx = new Regex(RegExpPattern.LettersAndNumbers);
            }

            if (null != regEx)
            {
                if (regEx.IsMatch(value))
                {
                    isValid = true;
                }
            }

            return isValid;
        }
        #endregion
    }
}
