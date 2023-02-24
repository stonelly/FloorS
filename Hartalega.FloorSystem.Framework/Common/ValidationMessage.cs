using System.Windows.Forms;
using Hartalega.FloorSystem.Framework;

namespace Hartalega.FloorSystem.Framework.Common
{
    /// <summary>
    /// Validation Message and control
    /// </summary>
    public class ValidationMessage
    {

        public ValidationMessage(Control controlName, string message, ValidationType validationType)
        {
            ControlName = controlName;
            Message = message;
            TypeOfValidation = validationType;
        }

        /// <summary>
        /// Control to set focus
        /// </summary>
        public Control ControlName { get; set; }

        /// <summary>
        /// Message to show
        /// </summary>
        public string Message { get; set; }

        public ValidationType TypeOfValidation { get; set; }

        public bool IsNotValid { get; set; }

    }

    /// <summary>
    /// This is enum for client side validations. This is not moved to Constants as it will be complex to change all the refernces.
    /// </summary>
    public enum ValidationType
    {
        /// <summary>
        /// Required field validator
        /// </summary>
        Required,
        /// <summary>
        /// Custom field validator
        /// </summary>
        Custom
    }
}
