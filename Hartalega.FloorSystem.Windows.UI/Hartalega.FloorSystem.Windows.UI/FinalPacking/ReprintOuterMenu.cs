using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Tumbling;
using System;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.FinalPacking
{
    public partial class ReprintOuterMenu : FormBase
    {
        # region PRIVATE VARIABLES
        private string _screenname = "Re-print Outer Case Menu";
        private string _uiClassName = "ReprintOuterCaseMenu";
        string _operatorName = String.Empty;
        #endregion

        public ReprintOuterMenu()
        {
            InitializeComponent();
        }

        private void ReprintOuterMenu_Load(object sender, EventArgs e)
        {
            this.Hide();
            Login _passwordForm = new Login(Constants.Modules.FINALPACKING, _screenname);
            _passwordForm.ShowDialog();
            try
            {
                if (_passwordForm.Authentication != Convert.ToString(Constants.ZERO) && !string.IsNullOrEmpty(_passwordForm.Authentication) && _passwordForm.Authentication != Messages.EXCEEDED_YOUR_TRIAL)
                {
                    _operatorName = Convert.ToString(_passwordForm.Authentication);
                    this.Show();
                }
                else
                {
                    this.Close();
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "ReprintOuterBox_Load", null);
            }
        }

        private void btnReprintOuterCase_Click(object sender, EventArgs e)
        {
            var frm = new ReprintOuterBox(_operatorName).ShowDialog();
            if(frm == DialogResult.OK)
                this.Close();
        }

        private void btnReprintOuterCaseInternalLotNo_Click(object sender, EventArgs e)
        {
            var frm = new ReprintOuterBoxInternalLotNo(_operatorName).ShowDialog();
            if (frm == DialogResult.OK)
                this.Close();
        }

        /// <summary>
        /// LOG TO DB, SHOW MESAGE BOX TO USER AND CLEAR THE FORM ON EXCEPTION
        /// </summary>
        /// <param name="floorException">APPLICATION EXCEPTION</param>
        /// <param name="screenName">SCREEN NAME TO BE LOGGED</param>
        /// <param name="UiClassName">CLASS NAME TO BE LOGGED</param>
        /// <param name="uiControl">CONTROL FOR WHICH THE EXCEPTION OCCURED</param>
        /// <param name="parameters"></param>
        private void ExceptionLogging(FloorSystemException floorException, string screenName, string UiClassName, string uiControl, params object[] parameters)
        {
            int result = CommonBLL.LogExceptionToDB(floorException, screenName, UiClassName, uiControl, parameters);
            if (floorException.subSystem == Constants.BUSINESSLOGIC)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.APPLICATIONERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
        }
    }
}
