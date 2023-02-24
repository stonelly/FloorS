// -----------------------------------------------------------------------
// <copyright file="RegExpPattern.cs" company="Avanade">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hartalega.FloorSystem.Framework.Common
{

    /// <summary>
    /// Use this class to get regular expressions for individual data type.
    /// </summary>
    public static class RegExpPattern
    {
        /// <summary>
        /// Number validation expression
        /// Valid: 234 or 2 or 2343
        /// </summary>
        public const string Number = "^[0-9]*$";

        /// <summary>
        /// United State of Americas ZIP code validation expression
        /// Valid: 33445 or 34544-4322
        /// </summary>
        public const string UsZipCode = @"^(\d{5})|(\d{5}-\d{4}$";

        /// <summary>
        /// Email validation expression
        /// Valid: india@Avanade.com or india@Avanade.com.au
        /// </summary>
        public const string Email = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        /// <summary>
        /// Percentage validation expression
        /// Valid: 100% or 100 or 34.3%
        /// Invalid: -1 or -2% or 100.1%
        /// </summary>
        public const string Percentage = @"^(0*100{1,1}\.?((?<=\.)0*)?%?$)|(^0*\d{0,2}\.?((?<=\.)\d*)?%?)$";

        /// <summary>
        /// Decimal validation expression
        /// Valid: 10.1 or 134 or -22.33
        /// Invalid: - or 33.2343 or -1234
        /// </summary>
        public const string Decimal = @"^[-+]?[0-9]\d{0,2}(\.\d{1,2})?%?$";

        /// <summary>
        /// Positive Integer validation expression
        /// Valid: 234 or 5 or 20
        /// Invalid: -54 or 32.345 or india
        /// </summary>
        public const string PositiveInteger = @"^\d+$";

        /// <summary>
        /// Credit card validation expression
        /// Valid: 1234-1234-1234-1234 0r 1234 1234 1234 1234 or 1234123412341234
        /// Invalid: Visa or 1234 or 123-1234-12345
        /// </summary>
        public const string CreditCard = @"^(\d{4}[- ]){3}\d{4}|\d{16}$";

        /// <summary>
        /// Letter validation expression
        /// Valid: Smith Ed or Ed Smith or IndiA
        /// </summary>
        public const string Letter = @"^[a-zA-Z]+$";

        /// <summary>
        /// Combination of Letters and Numbers
        /// Valid: Avanade122
        /// </summary>
        public const string LettersAndNumbers = @"^[a-zA-Z0-9]+$";

        /// <summary>
        /// Only date validation expression
        /// Valid: 10/03/1979 or 1-1-02 or 01.1.2003
        /// Invalid: 10/03/197 or 09--02--2004 or 01 02 03
        /// </summary>
        public const string OnlyDate = @"^([0]?[1-9]|[1|2][0-9]|[3][0|1])[./-]([0]?[1-9]|[1][0-2])[./-]([0-9]{4}|[0-9]{2})$";

        /// <summary>
        /// Date and Time validation expression
        /// Valid: 12/31/2002 or 12/31/2002 08:00 or 12/31/2002 08:00 AM
        /// Invalid: 12/31/02 or 12/31/2002 14:00
        /// </summary>
        public const string DateAndTime = @"^([0]\d|[1][0-2])\/([0-2]\d|[3][0-1])\/([2][01]|[1][6-9])\d{2}(\s([0]\d|[1][0-2])(\:[0-5]\d){1,2})*\s*([aApP][mM]{0,2})?$";

        /// <summary>
        /// Site URL validation expression
        /// Valid: https://portal.Avanade.com 0r http://portal.Avanade.com/Page/default.html
        /// Invalid: www.axis.Avanade.com
        /// </summary>
        public const string Url = @"";
        //public const string Url = @"(http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?";

        /// <summary>
        /// Name validation expression
        /// </summary>
        public const string Name = @"^\s*[a-zA-Z,\s]+\s*$";

        /// <summary>
        /// OperatorId Expression
        /// </summary>
        public const string OperatorId = @"[^0-9^+^\-^\/^\*^\(^\)]";

        /// <summary>
        /// Blank Expression
        /// </summary>
        public const string Blank = "\\S";

        /// <summary>
        /// Initializes a new instance of the <see cref="RegExpPattern" /> class.
        /// </summary>
        #region
        static RegExpPattern()
        {
            // No implementation required
        }
        #endregion
    }
}
