// -----------------------------------------------------------------------
// <copyright file="FormBase.cs" company="Avanade">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.Database;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;

using System.Windows.Forms;

namespace Hartalega.FloorSystem.Framework.Windows.UI.Forms
{
    /// <summary>
    /// This class provides common functionality that required for all forms.
    /// </summary>
    public class FormBase : System.Windows.Forms.Form
    {
        static DateTime _ServerDateTime;
        static bool _datetimeStarted;
        protected static bool BatchPrint;


        /// <summary>
        /// Gets Server date time
        /// </summary>
        public DateTime ServerCurrentDateTime
        {
            get
            {
                return FormBase.GetServerDate();
            }
        }





        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LayoutSetting();
        }

          


        private static DateTime GetServerDate()
        {
            if (_datetimeStarted)
            {
                return _ServerDateTime;
            }
            else
            {
                InitializeCurrentDateAndTime();
                return _ServerDateTime;
            }
        }


        private static void InitializeCurrentDateAndTime()
        {
            if (!_datetimeStarted)
            {
                _ServerDateTime = Convert.ToDateTime(FloorDBAccess.ExecuteScalar("USP_GET_CurrentDateAndTime"));
                _datetimeStarted = true;
            }
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = Constants.THOUSAND;
            timer.Enabled = true;
            timer.Elapsed += timer_Elapsed;
        }

        private static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _ServerDateTime = _ServerDateTime.AddSeconds(1);
        }


        /// <summary>
        /// Validation Message List
        /// </summary>
        protected List<ValidationMessage> validationMesssageLst = null;

        /// <summary>
        /// StringBuilder to show Message
        /// </summary>
        protected StringBuilder sbvalidation = null;

        /// <summary>
        /// To close form when Esc key is pressed
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape) this.Close();
            bool res = base.ProcessCmdKey(ref msg, keyData);
            FloorDBAccess.TearDown();
            return res;
        }

        public void LayoutSetting()
        {
           
            if (this.Height >= 730 && this.Width >= 1024 && this.FormBorderStyle!=FormBorderStyle.None)
            {
                this.AutoScrollMinSize = new Size(1005, 690);
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.MaximizeBox = true;
                this.MinimizeBox = true;
                this.MaximumSize = new Size(0, 0);
                this.MinimumSize = new Size(1024, 730);
               

                var GenericControl = (from p in this.Controls.OfType<GroupBox>() select p).ToList();
                if (GenericControl.Count() > 0)
                {
                    foreach (Control cnt in GenericControl)
                    {
                        cnt.Anchor = AnchorStyles.None;
                        cnt.Anchor = (AnchorStyles.Top | AnchorStyles.Left);
                    }
                }
                this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            }
        }



        /// <summary>
        /// To Validate the form after adding validation messages and controls to set focus
        /// </summary>
        /// <returns>bool</returns>
        protected bool ValidateForm()
        {
            bool isvalid = true;

            if (validationMesssageLst.Count > 0)
            {
                sbvalidation = new StringBuilder();
                sbvalidation.AppendLine(Messages.REQUIREDFIELDMESSAGE);
                foreach (ValidationMessage vldmess in validationMesssageLst)
                {
                    if (vldmess.TypeOfValidation == ValidationType.Required)
                    {
                        vldmess.IsNotValid = false;
                        Type controlType = vldmess.ControlName.GetType();

                        if (controlType == typeof(TextBox) || controlType == typeof(ComboBox))
                        {
                            if (string.IsNullOrEmpty(vldmess.ControlName.Text))
                            {
                                vldmess.IsNotValid = true;
                                isvalid = false;
                                sbvalidation.AppendLine(vldmess.Message);
                            }
                        }
                    }

                    if (vldmess.TypeOfValidation == ValidationType.Custom)
                    {
                        Type controlType = vldmess.ControlName.GetType();
                        if (vldmess.ControlName.GetType() == typeof(TextBox))
                        {
                            TextBox txtbox = (TextBox)vldmess.ControlName;
                            txtbox.Text = string.Empty;
                        }
                        vldmess.IsNotValid = true;
                        isvalid = false;
                        sbvalidation.AppendLine(vldmess.Message);
                    }
                }
                ValidationMessage ctrl = (from c in validationMesssageLst
                                          where c.IsNotValid == true
                                          select c).FirstOrDefault();
                if (ctrl != null)
                {
                    ctrl.ControlName.Focus();
                    if (sbvalidation != null)
                        GlobalMessageBox.Show(sbvalidation.ToString(), Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                }
                validationMesssageLst = null;
                sbvalidation = null;
                return isvalid;
            }
            return isvalid;
        }

        protected string GetValidateFormMessage()
        {

            sbvalidation = null;

            if (validationMesssageLst.Count > 0)
            {
                sbvalidation = new StringBuilder();
                sbvalidation.AppendLine(Messages.REQUIREDFIELDMESSAGE);
                foreach (ValidationMessage vldmess in validationMesssageLst)
                {
                    if (vldmess.TypeOfValidation == ValidationType.Required)
                    {
                        vldmess.IsNotValid = false;
                        Type controlType = vldmess.ControlName.GetType();

                        if (controlType == typeof(TextBox) || controlType == typeof(ComboBox))
                        {
                            if (string.IsNullOrEmpty(vldmess.ControlName.Text))
                            {
                                vldmess.IsNotValid = true;

                                sbvalidation.AppendLine(vldmess.Message);
                            }
                        }
                    }

                    if (vldmess.TypeOfValidation == ValidationType.Custom)
                    {
                        Type controlType = vldmess.ControlName.GetType();
                        if (vldmess.ControlName.GetType() == typeof(TextBox))
                        {
                            TextBox txtbox = (TextBox)vldmess.ControlName;
                            txtbox.Text = string.Empty;
                        }
                        vldmess.IsNotValid = true;

                        sbvalidation.AppendLine(vldmess.Message);
                    }
                }
                ValidationMessage ctrl = (from c in validationMesssageLst
                                          where c.IsNotValid == true
                                          select c).FirstOrDefault();
                if (ctrl != null)
                {
                    ctrl.ControlName.Focus();
                }
                validationMesssageLst = null;
                if (sbvalidation != null)
                    return sbvalidation.ToString();
                else
                    return String.Empty;
            }
            else
            {
                return String.Empty;
            }

        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FormBase
            // 
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Name = "FormBase";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

       

    }
}
