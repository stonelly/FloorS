

namespace Hartalega.FloorSystem.Windows.UI.Tumbling
{
    using FloorSystem.Business.Logic;
    using FloorSystem.Framework;
    using Hartalega.FloorSystem.Framework.DbExceptionLog;
    using System;
    using System.Windows.Forms;
    using Hartalega.FloorSystem.Windows.UI.Common;
    using System.Data;
    using System.Collections.Generic;
    using System.Drawing;
    using Hartalega.FloorSystem.Framework.Common;

    public partial class Login : Framework.Windows.UI.Forms.FormBase
    {
        #region Public Variables
        public string Authentication { get; set; }
        public bool IsCancel { get; set; }
        public static int Count = 0;
        public string TenPcsWeright { get; set; }

        #endregion

        #region Private Variables
        private Constants.Modules _moduleName { get; set; }
        private string _screenName = string.Empty;
        #endregion


        #region Public Methods
        /// <summary>
        /// Initialize component
        /// </summary>
        public Login(Constants.Modules module)
        {
            _moduleName = module;
            InitializeComponent();
            SetModuleSpecific();
            SetFormSize();
        }

        public Login(Constants.Modules module, string screenName, string tenPCweight = "", bool isPT = false, string message = "", bool isBatch = false, bool isTenPcs = false)
        {
            _moduleName = module;
            _screenName = screenName;
            InitializeComponent();
            TenPcsWeright = tenPCweight;
            SetModuleSpecific();
            Count = Constants.ZERO;
            //SetFormSize();
            if (isPT)
            {
                label1.Text = message;
            }
            if (isBatch)
                this.Text = Constants.BATCHWEIGHT_RANGE;
            if (isTenPcs & isBatch)
                this.Text = Constants.TENPCS_BATCH_TITLE;
            SetFormSize();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Set the form size
        /// </summary>
        private void SetFormSize()
        {
            if (label1.Visible == false && label1.Text == string.Empty)
            {
                flowLayoutPanel1.Height = 150;
                Height = 200;
            }
            else 
            {
                flowLayoutPanel1.Height = 210;
                Height = 255;
            }
        }

        /// <summary>
        /// Validate Password for 3 attempts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text == String.Empty)
            {
                GlobalMessageBox.Show(Messages.PASSWORD_EMPTY, Constants.AlertType.Warning, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);
                this.txtPassword.Focus();
            }
            else
            {
                string authorizedBy = string.Empty;
                bool isAuthorized = false;
                string password = string.Empty;
                DataTable dtEmployeeDetails = new DataTable();

                Count++;
                try
                {
                    password = txtPassword.Text;
                    isAuthorized = SecurityModuleBLL.ValidateEmployeeCredential(password, _screenName);
                    if (isAuthorized)
                    {
                        dtEmployeeDetails = SecurityModuleBLL.GetEmployeeDetailsByPassword(password);
                        authorizedBy = dtEmployeeDetails.Rows[0]["EmployeeId"].ToString();
                        this.Authentication = authorizedBy;
                        Count = Constants.ZERO;
                        this.Close();
                    }
                    else
                    {
                        if (Count <= Constants.TWO)
                        {
                            Authentication = string.Empty;
                            txtErrorMsg.Visible = true;
                            txtPassword.Text = String.Empty;
                            this.txtPassword.Focus();
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.EXCEEDED_YOUR_TRIAL, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK, SystemIcons.Error);
                            Count = Constants.ZERO;
                            this.Close();
                        }
                    }
                }
                catch (FloorSystemException ex)
                {
                    int result = CommonBLL.LogExceptionToDB(ex, _moduleName + " - OutOfrange", "OutOfrange", "btnOK_Click", txtPassword.Text);
                    if (ex.subSystem == Constants.BUSINESSLOGIC)
                        GlobalMessageBox.Show("(" + result + ") - " + Messages.APPLICATIONERROR, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK, SystemIcons.Error);
                    else
                        GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK, SystemIcons.Error);
                    return;
                }
            }
        }

        /// <summary>
        /// Password cannot be Empty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if (txtPassword.Text == String.Empty)
            {
                if (!btnCancel.Focused)
                    GlobalMessageBox.Show(Messages.PASSWORD_EMPTY, Constants.AlertType.Warning, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);
                //this.txtPassword.Focus(); Defect # 14622
            }
        }

        /// <summary>
        /// Load Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OutOfrange_Load(object sender, EventArgs e)
        {
            txtPassword.TextBoxLength(6);
            txtErrorMsg.Visible = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsCancel = true;
            this.Close();
        }

        /// <summary>
        /// Set Module to open password screen
        /// </summary>
        private void SetModuleSpecific()
        {

            switch (_moduleName)
            {
                case Constants.Modules.CONFIGURATIONSETUP:
                case Constants.Modules.PRODUCTIONSYSTEMREPORTS:
                case Constants.Modules.QCYIELDANDPACKING:
                    label1.Text = "Enter Password";
                    label1.Visible = true;
                    this.Text = "Enter Password";
                    break;
                case Constants.Modules.FINALPACKING:
                    if (_screenName == "Re-print Inner Box" || _screenName == "Re-print Outer Case Menu" || _screenName == "Print 2ndGrade Inner Outer" || _screenName == "BarcodeValidation")
                    {
                        label1.Text = "Password";
                        label1.Visible = false;
                        this.Text = "Password";
                    }
                    else
                    {
                        label1.Text = "Authorized Split Batch";
                        label1.Visible = true;
                        this.Text = "Authorized Split Batch";
                    }
                    break;
                case Constants.Modules.TUMBLING:
                case Constants.Modules.HOURLYBATCHCARD:
                case Constants.Modules.SURGICALGLOVESYSTEM:
                    label1.Text = string.Empty;
                    label1.Visible = false;
                    this.Text = "Log In";
                    break;
                case Constants.Modules.QAISYSTEM:

                    if (_screenName == Constants.EDIT_ONLINE_BATCH_CARD_INFO)
                    {
                        label1.Text = "Enter Password";
                        label1.Visible = true;
                        this.Text = "Enter Password";
                    }
                    else
                    {
                        this.Text = "Change Of QC Type";
                        label1.Text = string.Empty;
                        label1.Visible = false;
                    }
                        break;
                case Constants.Modules.QAISYSTEMINNERTENPCS:
                    label1.Text = Messages.ten_Pcs_Weight_outof_range + " : " + TenPcsWeright + "(g)";
                    label1.Visible = true;
                    this.Text = Constants.TENPCSWEIGHT_RANGE;
                    break;
                case Constants.Modules.PRODUCTIONLOGGING:
                    label1.Visible = false;
                    this.Text = "Log in";
                    break;
                //case Constants.Modules.BRANDMASTER:
                //    label1.Text = "Enter Password";
                //    label1.Visible = true;
                //    this.Text = "Enter Password";
                //    break;
                case Constants.Modules.WORKORDER:
                    label1.Text = "Enter Password";
                    label1.Visible = true;
                    this.Text = "Enter Password";
                    break;
                case Constants.Modules.REWORKORDER:
                    label1.Visible = false;
                    this.Text = "Create Rework Order";
                    break;
                default:

                    break;
            }
        #endregion
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                btnOK_Click(sender, e);
            }
        }
    }
}
