using Hartalega.FloorSystem.Business.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;

namespace Hartalega.FloorSystem.Windows.UI.QCScanning
{
    public partial class SecondGradeStockVerification : FormBase
    {
        private bool isDisposal;
        private bool isReject;
        private string _saveMessage;

        public SecondGradeStockVerification(string screenName)
        {
            InitializeComponent();
            if (screenName == Constants.SECOND_GRADE_VERIFICATION_DISPOSAL)
                isDisposal = true;
            if (screenName == Constants.REJECT_VERIFICATION_DISPOSAL)
                isReject = true;
            this.Text = screenName;
            grpBox.Text = screenName.Replace("&", "&&");
        }


        private void SecondGradeStockVerification_Load(object sender, EventArgs e)
        {
            if (isReject)
            {
                lblMessage.Text = string.Format(Constants.TOTAL_DISPOSE, Constants.ZERO, Constants.KG);
                _saveMessage = Messages.SAVE_REJECTGRADE_DISPOSAL;
            }
            else
            {
                if (isDisposal)
                {
                    lblMessage.Text = string.Format(Constants.TOTAL_DISPOSE, Constants.ZERO, Constants.PCS);
                    _saveMessage = Messages.SAVE_SECONDGRADE_DISPOSAL;
                }
                else
                {
                    lblMessage.Text = string.Format(Constants.TOTAL_VALID, Constants.ZERO, Constants.PCS);
                    _saveMessage = Messages.SAVE_SECONDGRADE_VERIFY;
                }
            }
            txtLocation.Text = WorkStationDTO.GetInstance().Location;
        }


        private void btnExtractPortable_Click(object sender, EventArgs e)
        {
            StringBuilder validSerialNo = new StringBuilder();
            StringBuilder inValidSerialNo = new StringBuilder();
            ProcessStartInfo startinfo = new ProcessStartInfo();
            startinfo.FileName = WorkStationDataConfiguration.GetInstance().SecondGradeExecutableFileLocation;
            startinfo.Arguments = WorkStationDataConfiguration.GetInstance().SecondGradeArgumentFileLocation;
            if (WorkStationDataConfiguration.GetInstance().LHardwareIntegration == true)
            {
                Process myprocess = Process.Start(startinfo);
                while (myprocess.HasExited == false)
                {
                }
            }
            string filepath = WorkStationDataConfiguration.GetInstance().SecondGradeTextFileLocation;
            if (File.Exists(filepath))
            {
                string[] lines = System.IO.File.ReadAllLines(filepath);
                foreach (string line in lines)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        if (!isReject)
                        {
                            if (!string.IsNullOrEmpty(QCScanningBLL.ValidateSecondGradeSticker(line)))
                            {
                                if (!validSerialNo.ToString().Contains(line))
                                    validSerialNo.Append(line.ToString() + ", ");
                            }
                            else
                                inValidSerialNo.Append(line.ToString() + ", ");
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(QCScanningBLL.ValidateRejectGradeSticker(line)))
                            {
                                if (!validSerialNo.ToString().Contains(line))
                                    validSerialNo.Append(line.ToString() + ", ");
                            }
                            else
                                inValidSerialNo.Append(line.ToString() + ", ");
                        }
                    }
                }
                //int count = Constants.ZERO;
                int totalPCs = Constants.ZERO;
                decimal rejectWeight = Constants.ZERO;
                if (!string.IsNullOrEmpty(validSerialNo.ToString()))
                {
                    txtListSerialNumber.Text = validSerialNo.ToString().Substring(Constants.ZERO, validSerialNo.Length - Constants.TWO);
                    totalPCs = QCScanningBLL.GetTotalPCs2ndGradeSticker(txtListSerialNumber.Text);
                    //count = txtListSerialNumber.Text.Count(f => f == ',') + Constants.ONE;
                    if (isReject)
                        rejectWeight = QCScanningBLL.GetBatchWeightRejectGradeSticker(txtListSerialNumber.Text);
                }
                else
                    txtListSerialNumber.Text = string.Empty;

                if (isReject)
                    lblMessage.Text = string.Format(Constants.TOTAL_DISPOSE, rejectWeight, Constants.KG);
                else
                {
                    if (isDisposal)
                        lblMessage.Text = string.Format(Constants.TOTAL_DISPOSE, totalPCs, Constants.PCS);
                    else
                        lblMessage.Text = string.Format(Constants.TOTAL_VALID, totalPCs, Constants.PCS);
                }

                if (!string.IsNullOrEmpty(inValidSerialNo.ToString()))
                    txtInvalidBarcode.Text = inValidSerialNo.ToString().Substring(Constants.ZERO, inValidSerialNo.Length - Constants.TWO);
                else
                    txtInvalidBarcode.Text = string.Empty;
            }
            else
            {
                GlobalMessageBox.Show(Messages.SECONDNDGRADEFILEVERFICATION, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                ClearForm();
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(txtListSerialNumber, "List of Serial Number", ValidationType.Required));
            if (ValidateForm())
            {
                string confirm = GlobalMessageBox.Show(_saveMessage, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo);
                if (confirm == Constants.YES)
                {
                    string[] values = txtListSerialNumber.Text.Split(',');
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = values[i].Trim();
                        if (isReject)
                            QCScanningBLL.DisposeRejectGrade(values[i]);
                        else
                            QCScanningBLL.VerifySecondGrade(values[i]);
                        if (isDisposal)
                            QCScanningBLL.DisposeSecondGrade(values[i]);
                    }
                    GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    ClearForm();
                }
            }
        }

        /// <summary>
        ///  Alerts the User for clearing the values of all the controls.If yes, clear all the values.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            string confirm = GlobalMessageBox.Show(Messages.CLEARMESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo);
            if (confirm == Constants.YES)
            {
                ClearForm();
            }
        }

        /// <summary>
        /// Clear the values of all the controls. 
        /// </summary>
        private void ClearForm()
        {
            txtListSerialNumber.Text = String.Empty;
            txtInvalidBarcode.Text = String.Empty;
            btnExtractPortable.Focus();
            if (isReject)
            {
                lblMessage.Text = string.Format(Constants.TOTAL_DISPOSE, Constants.ZERO, Constants.KG);
            }
            else
            {
                if (isDisposal)
                {
                    lblMessage.Text = string.Format(Constants.TOTAL_DISPOSE, Constants.ZERO, Constants.PCS);
                }
                else
                {
                    lblMessage.Text = string.Format(Constants.TOTAL_VALID, Constants.ZERO, Constants.PCS);
                }
            }
        }




    }
}
