using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using Hartalega.FloorSystem.Framework;
using System.Collections.Generic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.Glove
{
    public partial class QcTypeForm : FormBase
    {
        string _qcType = "", _qcDescription = "", _gloveCode = "";
        int _numTesters,_qcId,_qcDescId;
        bool formEdit = false;
        private string _screenName = "QC Target Assignment";
        private string _className = "QcTypeForm";



        private bool ValidateRequiredFields()
        {
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(cbQCType, "QC Type", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(tbNumTester, "Number of Tester", ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(tbPiecesHR, "Pieces Per Hour", ValidationType.Required));
            return ValidateForm();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (GlobalMessageBox.Show(Messages.LEAVE_PAGE_MESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
            {
                this.Close();
            }

        }

        private void QcTypeForm_Load(object sender, EventArgs e)
        {
            cbQCType.DataSource = GloveCodeBLL.GetQCTypeByGloveCode();
            if(formEdit)
            cbQCType.Text = _qcType;
            //cmbConfData.DisplayMember = "ConfName";
            //cmbConfData.ValueMember = "ConfId";
        }

        private void tbNumTester_TextChanged(object sender, EventArgs e)
        {
            ValidateTextInput(tbNumTester);   }

        public void ValidateTextInput(TextBox txt)
        {
          bool  _isValid = Validator.IsValidInput(Constants.ValidationType.Integer, txt.Text);
            if (!_isValid)
            {
                txt.Clear();
            }
        }

        private void tbPiecesHR_TextChanged(object sender, EventArgs e)
        {
            ValidateTextInput(tbPiecesHR);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int rowsReturned = 0;
            bool isDuplicate = false;
            bool isValid = false;

            bool isNumTest = Validator.IsValidInput(Constants.ValidationType.Integer, tbNumTester.Text);
            bool isPieces = Validator.IsValidInput(Constants.ValidationType.Integer, tbPiecesHR.Text);
            
            if (ValidateRequiredFields())
            {
                QCTypeDTO objQcType = new QCTypeDTO();
                try
                {
                   // cbQCType.Text = _qcType;
                    objQcType.QCType = cbQCType.Text;
                    objQcType.NumOfTester = int.Parse(tbNumTester.Text);
                    objQcType.GloveCode = _gloveCode;
                    objQcType.PiecesHR = int.Parse(tbPiecesHR.Text);
                    
                    isValid  = Convert.ToBoolean(GloveCodeBLL.isQcTypeFormValid(objQcType));
                  
                    if (isValid)
                    {
                        if(!formEdit)
                        isDuplicate = Convert.ToBoolean(GloveCodeBLL.isQcTypeFormDuplicate(objQcType));

                        if (!isDuplicate)
                        {

                            if (formEdit)
                            {
                                objQcType.QCType = cbQCType.Text;
                                //objQcType.Description = tbDescription.Text;
                                objQcType.NumOfTester = int.Parse(tbNumTester.Text);
                                objQcType.GloveCode = _gloveCode;
                                objQcType.QcTypeId = _qcId;
                                objQcType.DescId = _qcDescId;
                                rowsReturned = GloveCodeBLL.EditQCTypeForm(objQcType);
                            }
                            else
                            {
                                rowsReturned = GloveCodeBLL.SaveQCTypeForm(objQcType);
                            }
                            if (rowsReturned > 0)
                            {
                                GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);

                                //event log myadamas 20190227
                                EventLogDTO EventLog = new EventLogDTO();

                                EventLog.CreatedBy = String.Empty;
                                Constants.EventLog audAction = Constants.EventLog.Save;
                                EventLog.EventType = Convert.ToInt32(audAction);
                                EventLog.EventLogType = Constants.eventlogtype;

                                var screenid = CommonBLL.GetScreenIdByScreenName(_screenName);
                                CommonBLL.InsertEventLog(EventLog, _screenName, screenid.ToString());

                                this.Close();
                            }
                            else
                            {
                                GlobalMessageBox.Show(Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                            }
                        }
                        else
                        {
                            //Messages.DUPLICATE_QCTYPE_VALUES
                            GlobalMessageBox.Show("DUPLICATE QCTYPE VALUES", Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            cbQCType.Text = string.Empty;
                            cbQCType.Focus();
                        }
                    }
                    else
                    {
                        GlobalMessageBox.Show("INVALID QCTYPE VALUES", Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        cbQCType.Text = string.Empty;
                        cbQCType.Focus();
                    }
                }
                catch (FloorSystemException ex)
                {
                    ExceptionLogging(ex, _screenName, _className, "btnSave_Click", null);
                    return;
                }
                            
            }
        }

        private void ExceptionLogging(FloorSystemException floorException, string screenName, string UiClassName, string uiControl, params object[] parameters)
        {
            int result = CommonBLL.LogExceptionToDB(floorException, screenName, UiClassName, uiControl, parameters);
            if (floorException.subSystem == Constants.BUSINESSLOGIC)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.APPLICATIONERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
        }

        public QcTypeForm(string gloveCode,string qcType,string qcDescription,int numTesters)
        {
            InitializeComponent();
            _gloveCode = gloveCode;
            _qcType = qcType;
            _qcDescription = qcDescription;
            _numTesters = numTesters;formEdit = false;
           
        }

        public QcTypeForm(string gloveCode, string qcType, string qcDescription, int numTesters,int qcId,int qcDescId, int piecesHR)
        {
            InitializeComponent();
            _gloveCode = gloveCode;
            _qcType = qcType;
            _qcDescription = qcDescription;
            _numTesters = numTesters;
            _qcId = qcId;
            _qcDescId = qcDescId;
            formEdit = true;
            cbQCType.Text = qcType;
            tbDescription.Text = qcDescription;
            tbNumTester.Text = numTesters + "";
            tbPiecesHR.Text = piecesHR + "";
            cbQCType.Enabled = false;
        }
    }
}
