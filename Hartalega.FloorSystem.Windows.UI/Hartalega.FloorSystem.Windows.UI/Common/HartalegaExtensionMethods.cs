using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Hartalega.FloorSystem.Framework.Common;

namespace Hartalega.FloorSystem.Windows.UI.Common
{
    public static class HartalegaExtensionMethods
    {
        /// <summary>
        /// Extension Method to bind ComboBox using List DropdownDTO
        /// </summary>
        /// <param name="cmbbox"></param>
        /// <param name="datasource"></param>
        /// <param name="showZeroIndex"></param>
        public static void BindComboBox(this ComboBox cmbbox, List<DropdownDTO> datasource, bool showZeroIndex)
        {
            cmbbox.DataSource = datasource;
            cmbbox.ValueMember = "IDField";
            cmbbox.DisplayMember = "DisplayField";
            if (showZeroIndex)
            {
                cmbbox.SelectedIndex = Constants.MINUSONE;
            }
        }

        /// <summary>
        /// Extension Method to Make Text box to allow only numbers and Max length as 6
        /// </summary>
        /// <param name="txtoperatorId"></param>
        public static void OperatorId(this TextBox txtoperatorId)
        {
            txtoperatorId.KeyPress += textBox1_KeyPress;
            txtoperatorId.MaxLength = 6;
        }

        static void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //define a string containing special characters 
            string arr = "!`~@#$%^&*()+=-_[]\\\';,./{}|\":<>? ";
            for (int k = 0; k < arr.Length; k++)
            {
                if (e.KeyChar == arr[k])
                {
                    e.Handled = true;
                    break;

                }
            }
        }



        /// <summary>
        /// Extension Method to check key changed for operatorid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void txtoperatorId_TextChanged(object sender, System.EventArgs e)
        {
            TextBox txtbx = (TextBox)sender;
            Regex regex = new Regex(RegExpPattern.OperatorId);
            MatchCollection matches = regex.Matches(txtbx.Text);
            if (matches.Count > 0)
            {
                ((TextBox)sender).Text = string.Empty;
            }
        }



        /// <summary>
        /// Extension Method to check length of textbox
        /// </summary>
        /// <param name="txtControl"></param>
        /// <param name="maxLength"></param>
        public static void TextBoxLength(this TextBox txtControl, int maxLength)
        {
            txtControl.KeyPress += txtControl_KeyPress;
            txtControl.MaxLength = maxLength;
        }

        /// <summary>
        /// Extension Method to check key press of textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void txtControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)8 && !char.IsNumber(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Extension Method to Make Text box to allow only numbers and Max length as 10
        /// </summary>
        /// <param name="txtSerialNo"></param>
        public static void SerialNo(this TextBox txtSerialNo)
        {
            txtSerialNo.KeyPress += txtControl_KeyPress;
            txtSerialNo.TextChanged += txtoperatorId_TextChanged;
            txtSerialNo.MaxLength = 10;
        }

        /// <summary>
        /// Extension Method to Make Text box to Disable only numbers and Max length as 10
        /// </summary>
        /// <param name="txtSerialNo"></param>
        public static void DisableSerialNo(this TextBox txtSerialNo)
        {
            txtSerialNo.KeyPress -= txtControl_KeyPress;
            txtSerialNo.TextChanged -= txtoperatorId_TextChanged;
            txtSerialNo.MaxLength -= 10;
        }

        /// <summary>
        /// Extension Method to check key press for SerialNo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void txtSerialNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)8 && !char.IsNumber(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Extension Method to Make Inner Box Count Text box to allow only numbers and Max length as 15
        /// </summary>
        /// <param name="txtSerialNo"></param>
        public static void InnerBoxCount(this TextBox txtInnerBox)
        {
            txtInnerBox.KeyPress += txtControl_KeyPress;
            txtInnerBox.TextChanged += txtoperatorId_TextChanged;
            txtInnerBox.MaxLength = 6;
        }


        /// <summary>
        /// Extension Method to Make Sequnce Text box to allow only numbers and Max length as 4
        /// </summary>
        /// <param name="txtSerialNo"></param>
        public static void Sequence(this TextBox txtSequence)
        {
            txtSequence.KeyPress += txtControl_KeyPress;
            txtSequence.TextChanged += txtoperatorId_TextChanged;
            txtSequence.MaxLength = 4;
        }




        /// <summary>
        /// Extension Method to Make Password Text box to allow only numbers and Max length as 6
        /// </summary>
        /// <param name="txtSerialNo"></param>
        public static void Password(this TextBox txtPassword)
        {
            txtPassword.KeyPress += txtControl_KeyPress;
            txtPassword.MaxLength = 6;
        }



        /// <summary>
        /// Extension Method to Make Text box to allow only numbers and Max length as 10
        /// </summary>
        /// <param name="txtReference"></param>
        public static void Reference(this TextBox txtReference)
        {
            txtReference.KeyPress += txtReference_KeyPress;
            txtReference.MaxLength = 10;
        }

        /// <summary>
        /// Extension Method to check key press for reference
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void txtReference_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)8 && !char.IsNumber(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Extension Method to Make Text box to allow only numbers and Max length as 10
        /// </summary>
        /// <param name="txtReference"></param>
        public static void BoxesPacked(this TextBox txtBoxesPacked)
        {
            txtBoxesPacked.KeyPress += txtControl_KeyPress;
            txtBoxesPacked.MaxLength = 9;
        }



        /// <summary>
        /// Extension Method to Make Text box to allow only numbers and decimal
        /// </summary>
        /// <param name=""></param>
        public static void Weight(this TextBox txtWeight)
        {
            txtWeight.KeyPress += txtWeight_KeyPress;
            txtWeight.MaxLength = 10;
        }

        /// <summary>
        /// Extension Method to Make Date Picker not to select Future date and Custo Date Format As DD/MM/YYYY
        /// </summary>
        /// <param name="dtp"></param>
        public static void NofutureDateSelection(this DateTimePicker dtp)
        {
            dtp.Format = DateTimePickerFormat.Custom;
            dtp.CustomFormat = Constants.CUSTOM_DATE_FORMAT;
            dtp.MaxDate = CommonBLL.GetCurrentDateAndTime();
        }

        static void txtWeight_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)8 && !char.IsNumber(e.KeyChar) && e.KeyChar != (char)46)
            {
                e.Handled = true;
            }
            else
            {
                string strkey = e.KeyChar.ToString().Substring(e.KeyChar.ToString().Length - 1,
                        e.KeyChar.ToString().Length - (e.KeyChar.ToString().Length - 1));

                if (char.IsNumber(e.KeyChar))
                {
                    TextBox tb = sender as TextBox;
                    int cursorPosLeft = tb.SelectionStart;
                    int cursorPosRight = tb.SelectionStart + tb.SelectionLength;
                    string result1 = tb.Text.Substring(0, cursorPosLeft) +
                          strkey + tb.Text.Substring(cursorPosRight);
                    string[] parts = result1.Split('.');
                    if (parts.Length > 1)
                    {
                        if (parts[1].Length > 4 || parts.Length > 2)
                        {
                            e.Handled = true;
                        }
                    }
                }
                if (((TextBox)sender).Text.Contains(".") && (e.KeyChar == (char)46))
                {
                    e.Handled = true;
                }
            }
        }


        /// <summary>
        /// Extension Method to Make Date picker text box to accept only numbers and '/'
        /// </summary>
        /// <param name="txtoperatorId"></param>
        public static void DatePickerText(this TextBox txtDate)
        {
            txtDate.KeyPress += txtDate_KeyPress;
        }

        static void txtDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)8 && !char.IsNumber(e.KeyChar) && e.KeyChar != (char)47)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Extension Method to Make combo box to allow only numbers and Max length as 10
        /// </summary>
        /// <param name="txtSerialNo"></param>
        public static void QcTypeCombo(this ComboBox cmbQcType)
        {
            cmbQcType.KeyPress += txtControl_KeyPress;
            cmbQcType.MaxLength = 15;
        }

        /// <summary>
        /// Extension Method to bind Multiple ComboBox using same Datasource
        /// </summary>
        /// <param name="cmbbox"></param>
        /// <param name="datasource"></param>
        /// <param name="showZeroIndex"></param>
        public static void BindMultipleComboBox(this ComboBox cmbbox, BindingSource datasource, bool showZeroIndex)
        {
            cmbbox.DataSource = datasource;
            cmbbox.ValueMember = "IDField";
            cmbbox.DisplayMember = "DisplayField";
            if (showZeroIndex)
            {
                cmbbox.SelectedIndex = Constants.MINUSONE;
            }
        }


        public static string FPCustomerRefernceSplit(this string customerRefernce)
        {
            string[] Fpcrn = null;
            if (!string.IsNullOrEmpty(customerRefernce))
            {
                  Fpcrn = customerRefernce.Split('|');
            }
            if(Fpcrn.Length>1)
            {
                return  Fpcrn[1].Trim();
            }
            else if (Fpcrn.Length == 1)
            {
                return Fpcrn[0].Trim();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
