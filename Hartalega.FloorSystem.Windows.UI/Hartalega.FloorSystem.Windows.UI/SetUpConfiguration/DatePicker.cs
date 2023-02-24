using System;
using System.Windows.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;

namespace Hartalega.FloorSystem.Windows.UI.SetUpConfiguration
{
    public partial class DatePicker : UserControl
    {
        public DatePicker()
        {
            InitializeComponent();            
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            txtDate.Text = dateTimePicker1.Text.Trim();
        }
        

        private void DatePicker_Load(object sender, EventArgs e)
        {
            txtDate.DatePickerText(); 
        }
    }
}
