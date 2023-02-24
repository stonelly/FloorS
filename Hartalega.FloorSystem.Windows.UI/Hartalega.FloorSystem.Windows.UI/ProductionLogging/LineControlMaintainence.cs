using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.ProductionLogging
{
    public partial class LineControlMaintainence : FormBase
    {
        #region Private Class Members
        private string _lineId;
        private int _former;
        private int _speed;
        private decimal _cycle;
        private ProductionLineStartStop _productionLineStartStop;
        private const string _screenName = "LineControlMaintainence";
        private const string _className = "LineControlMaintainence";
        private bool _isValid;
        #endregion

        #region Constructors
        public LineControlMaintainence()
        {
            InitializeComponent();
        }

        public LineControlMaintainence(string lineId, int former, int speed, decimal cycle)
            : this()
        {
            _lineId = lineId;
            _former = former;
            _speed = speed;
            _cycle = cycle;
        }

        public LineControlMaintainence(string lineId, int former, int speed, decimal cycle, ProductionLineStartStop productionLineStartStop)
            : this()
        {
            _lineId = lineId;
            _former = former;
            _speed = speed;
            _cycle = cycle;
            _productionLineStartStop = productionLineStartStop;
        }
        #endregion

        #region Event Handlers

        /// <summary>
        /// Populate fom fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LineControlMaintainence_Load(object sender, EventArgs e)
        {
            try
            {
                PopulateFormFields();
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, Name, null);
                return;
            }
        }

        /// <summary>
        /// Save data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateUserInput())
            {
                int rowsaffected = Constants.ZERO;
                string lineId = txtLine.Text;
                int former = Convert.ToInt32(txtFormer.Text);
                int speed = Convert.ToInt32(txtPcsPerHour.Text);
                float cycle = float.Parse(txtCyclePerHour.Text);
                rowsaffected = ProductionLoggingBLL.SaveLineControlDetails(lineId, former, speed, cycle);
                if (rowsaffected > Constants.ZERO)
                {
                    GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    Close();
                    try
                    {
                        _productionLineStartStop.PopulateProductionLineGrid();
                    }
                    catch (FloorSystemException fsEX)
                    {
                        ExceptionLogging(fsEX, _screenName, _className, Name, null);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Clear form fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (GlobalMessageBox.Show(Messages.CONFIRM_CANCEL_TRANSACTION, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
            {
                ClearForm();
            }
        }

        /// <summary>
        /// restrict non numeric values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFormer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == '\b') //The  character represents a backspace
            {
                e.Handled = false; //Do not reject the input
            }
            else
            {
                e.Handled = true; //Reject the input
            }
        }

        /// <summary>
        /// restrict non numeric values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPcsPerHour_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == '\b') //The  character represents a backspace
            {
                e.Handled = false; //Do not reject the input
            }
            else
            {
                e.Handled = true; //Reject the input
            }
        }

        /// <summary>
        /// restrict non numeric values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCyclePerHour_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == '\b' || e.KeyChar == '.') //The  character represents a backspace
            {
                e.Handled = false; //Do not reject the input
            }
            else
            {
                e.Handled = true; //Reject the input
            }
        }

        /// <summary>
        ///  restrict non numeric values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFormer_TextChanged(object sender, EventArgs e)
        {
            ValidateTextInput(txtFormer);
            CalculateCyclesPerHour();
        }

        /// <summary>
        ///  restrict non numeric values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPcsPerHour_TextChanged(object sender, EventArgs e)
        {
            ValidateTextInput(txtPcsPerHour);
            CalculateCyclesPerHour();
        }

        /// <summary>
        ///  restrict non numeric values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCyclePerHour_TextChanged(object sender, EventArgs e)
        {
            ValidateDecimalInput(txtCyclePerHour);
        }

        #endregion

        #region User Methods

        /// <summary>
        /// Populate form fields
        /// </summary>
        private void PopulateFormFields()
        {
            txtLine.Text = _lineId;
            txtFormer.Text = Convert.ToString(_former);
            txtPcsPerHour.Text = Convert.ToString(_speed);
            txtCyclePerHour.Text = Convert.ToString(_cycle);
            CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
        }

        /// <summary>
        /// Validate user input
        /// </summary>
        /// <returns></returns>
        private bool ValidateUserInput()
        {
            bool status = false;
            string requiredFieldMessage = Messages.REQUIREDFIELDMESSAGE;
            if (txtFormer.Text.Equals(string.Empty) || txtPcsPerHour.Text.Equals(string.Empty))
            {
                if (txtFormer.Text.Equals(string.Empty))
                {
                    requiredFieldMessage = requiredFieldMessage + Constants.NOFORMERS + Environment.NewLine;
                }
                if (txtPcsPerHour.Text.Equals(string.Empty))
                {
                    requiredFieldMessage = requiredFieldMessage + Constants.PCSPERHR + Environment.NewLine;
                }
                if (requiredFieldMessage == Messages.REQUIREDFIELDMESSAGE)
                {
                    status = true;
                }
                else
                {
                    GlobalMessageBox.Show(requiredFieldMessage, Constants.AlertType.Information, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);
                    status = false;
                }
                return status;
            }
            else
            {
                if (!(Validator.IsValidInput(Constants.ValidationType.Integer, txtFormer.Text) && Convert.ToInt32(txtFormer.Text) != Constants.ZERO))
                {
                    GlobalMessageBox.Show(Messages.INVALID_VALUE_FOR_FORMER, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    txtFormer.Clear();
                    txtFormer.Focus();
                    return false;
                }
                else if (!(Validator.IsValidInput(Constants.ValidationType.Integer, txtPcsPerHour.Text))) // && Convert.ToInt32(txtPcsPerHour.Text) != Constants.ZERO))
                {
                    GlobalMessageBox.Show(Messages.INVALID_VALUE_FOR_SPEED, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    txtPcsPerHour.Clear();
                    txtPcsPerHour.Focus();
                    return false;
                }

                else
                    return true;
            }
        }

        /// <summary>
        /// Clear form
        /// </summary>
        private void ClearForm()
        {
            CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            Close();
        }

        /// <summary>
        /// Log exception
        /// </summary>
        /// <param name="floorException"></param>
        /// <param name="screenName"></param>
        /// <param name="uiClassName"></param>
        /// <param name="uiControl"></param>
        /// <param name="parameters"></param>
        private void ExceptionLogging(FloorSystemException floorException, string screenName, string uiClassName, string uiControl, params object[] parameters)
        {
            int result = CommonBLL.LogExceptionToDB(floorException, screenName, uiClassName, uiControl, parameters);
            if (floorException.subSystem == Constants.BUSINESSLOGIC)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.APPLICATIONERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            ClearForm();
        }

        /// <summary>
        /// Restrict user from pasting non numeric inputs
        /// </summary>
        /// <param name="txt"></param>
        public void ValidateTextInput(TextBox txt)
        {
            _isValid = Validator.IsValidInput(Constants.ValidationType.Integer, txt.Text);
            if (!_isValid)
            {
                txt.Clear();
            }
        }

        /// <summary>
        /// Restrict user from pasting non numeric/decimal inputs
        /// </summary>
        /// <param name="txt"></param>
        public void ValidateDecimalInput(TextBox txt)
        {
            foreach (char c in txt.Text)
            {
                _isValid = false;
                if (c >= '0' && c <= '9' || c == '.')
                {
                    _isValid = true;
                }
            }
            if (!_isValid)
            {
                txt.Clear();
            }
        }

        private void CalculateCyclesPerHour()
        {
            if (!string.IsNullOrEmpty(txtFormer.Text) && !string.IsNullOrEmpty(txtPcsPerHour.Text))
            {
                if (Convert.ToInt64(txtFormer.Text) > 0)
                {
                    txtCyclePerHour.Text = (float.Parse(txtPcsPerHour.Text) / float.Parse(txtFormer.Text)).ToString("0.00");
                }
            }
            else
            {
                txtCyclePerHour.Text = string.Empty;
            }
        }

        #endregion

        private void txtFormer_Leave(object sender, EventArgs e)
        {
            CalculateCyclesPerHour();
        }

    }
}
