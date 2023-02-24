
namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// Dropdown common DTO
    /// </summary>
    public class DropdownDTO
    {
        #region constructors

        public DropdownDTO()
        {

        }

        public DropdownDTO(string iDField, string displayField)
        {
            IDField = iDField;
            DisplayField = displayField;
        }

        public DropdownDTO(string iDField, string displayField, string selectedValue)
        {
            IDField = iDField;
            DisplayField = displayField;
            SelectedValue = selectedValue;
        }

        #endregion

        public string IDField { get; set; }
        public string DisplayField { get; set; }
        public string SelectedValue { get; set; }
        public string CustomField { get; set; }
    }
}
