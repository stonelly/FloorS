using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Tumbling;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.FinalPacking
{
    public partial class ReprintSurgicalInner : FormBase
    {
        private string _screenname = "Re-print Inner Box";
        private string _uiClassName = "ReprintInnerBox";
       
        string _operatorName = String.Empty;

        public ReprintSurgicalInner()
        {
            InitializeComponent();
        }

        private void txtInnerLotNo_Leave(object sender, EventArgs e)
        {
             try
            {
                if (!string.IsNullOrEmpty(txtInnerLotNo.Text))
                {
                    // Change behaviour to not validate ItemType for this module
                    if(FinalPackingBLL.validateInnerLotNumber(txtInnerLotNo.Text.Trim(),Constants.ZERO))
                    {
                        SurgicalFinalPackingDTO objSurgicalFinalPackingDTO = FinalPackingBLL.GetSurgicalInternalLotNumberDetails(txtInnerLotNo.Text.Trim());
                        txtPoNumber.Text = string.IsNullOrEmpty(objSurgicalFinalPackingDTO.CustomerReferenceNumber) ? objSurgicalFinalPackingDTO.PoNumber : objSurgicalFinalPackingDTO.CustomerReferenceNumber + " | " + objSurgicalFinalPackingDTO.PoNumber;    
                        txtItemNo.Text = objSurgicalFinalPackingDTO.ItemNumber;
                        txtItemName.Text = objSurgicalFinalPackingDTO.ItemName;
                        txtGroupId.Text = Convert.ToString(objSurgicalFinalPackingDTO.GroupId);
                        txtSerialNo.Text = Convert.ToString(objSurgicalFinalPackingDTO.SerialNumberLeft);
                        txtSerialNo2.Text = objSurgicalFinalPackingDTO.SerialNumberRight == 0? string.Empty: Convert.ToString(objSurgicalFinalPackingDTO.SerialNumberRight);
                        txtBatchNo.Text = objSurgicalFinalPackingDTO.BatchNumberLeft;
                        txtBatchNo2.Text = objSurgicalFinalPackingDTO.BatchNumberRight;
                        txtBoxesPacked.Text = Convert.ToString(objSurgicalFinalPackingDTO.BoxesPacked);
                        txtPalletId.Text = Convert.ToString(objSurgicalFinalPackingDTO.palletid);
                        btnPrint.Enabled = true;
                        btnPrint.Focus();
                    }
                    else
                    {                        
                        GlobalMessageBox.Show(Messages.INVALIDINTERNALLOTNUMBER, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);                                                                          
                        ClearControls();
                        txtInnerLotNo.Focus();
                    }
                }
                else
                {
                    ClearControls();
                }
            }
             catch (FloorSystemException ex)
             {
                 ExceptionLogging(ex, _screenname, _uiClassName, "txtInternalLot_Leave", null);
             }
        }

        private Boolean ValidateRequiredFields()
        {
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(txtInnerLotNo, Messages.REQINTERNALLOTNUMBER, Hartalega.FloorSystem.Framework.Common.ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(txtLabelCount, Messages.REQLABELCOUNT, Hartalega.FloorSystem.Framework.Common.ValidationType.Required));
            return ValidateForm();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateRequiredFields())   
                if (ValidateLabelCount())
                {
                    if (!string.IsNullOrEmpty(txtInnerLotNo.Text))
                    {
                        FPReprintInner objFPReprintInner = new FPReprintInner();
                        objFPReprintInner.InternalLotNumber = txtInnerLotNo.Text.Trim();
                        objFPReprintInner.LocationId = WorkStationDTO.GetInstance().LocationId;
                        objFPReprintInner.PrinterName = txtInnerLabelPrinter.Text.Trim();
                        objFPReprintInner.WorkStationNumber = WorkStationDTO.GetInstance().WorkStationId;
                        objFPReprintInner.AuthhorizedBy = _operatorName;
                        int rowsReturned = FinalPackingBLL.InsertFPReprintInner(objFPReprintInner);
                        if (rowsReturned > 0)
                        {
                            GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            FinalPackingPrint.InnerLabelPrint(txtInnerLotNo.Text); //pouch printing
                            FinalPackingPrint.PrintsurgicalInner(txtInnerLotNo.Text, true, labelcount: Convert.ToInt32(txtLabelCount.Text)); // Surgical Inner                                                        
                            ClearControls();
                            this.Close();
                        }
                        else
                        {                            
                            GlobalMessageBox.Show(Messages.FPERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);                                                                          
                            ClearControls();
                        }
                    }
                    else
                    {                        
                        GlobalMessageBox.Show(Messages.REQINTERNALLOTNUMBER, Constants.AlertType.Warning, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);                                                                          
                    }
                }
                else { txtLabelCount.Focus(); }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "btnPrint_Click", null);
            }
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

        private void ReprintSurgicalInner_Load(object sender, EventArgs e)
        {
            this.Hide();
            Login _passwordForm = new Login(Constants.Modules.FINALPACKING, _screenname);
            _passwordForm.ShowDialog();

            if (_passwordForm.Authentication != Convert.ToString(Constants.ZERO) && !string.IsNullOrEmpty(_passwordForm.Authentication) && _passwordForm.Authentication != Messages.EXCEEDED_YOUR_TRIAL)
            {
                _operatorName = _passwordForm.Authentication;
                txtLocation.Text = WorkStationDTO.GetInstance().Location;                
                if (!string.IsNullOrEmpty(WorkStationDataConfiguration.GetInstance().stationNumber))
                {
                    txtStationNo.Text = WorkStationDataConfiguration.GetInstance().stationNumber;
                    
                }
                else
                    GlobalMessageBox.Show(Messages.STATIONNUMBER_NOTCONFIGURED,Constants.AlertType.Information,Messages.INFORMATION,GlobalMessageBoxButtons.OK);

                txtInnerLabelPrinter.Text = WorkStationDataConfiguration.GetInstance().innerPrinter;
                this.WindowState = FormWindowState.Maximized;
                this.Show();
            }
            else
            {                
                this.Close();
            }
          
        }

       

        private void txtLabelCount_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtLabelCount.Text.Trim()))
                if (!ValidateLabelCount())
                    txtLabelCount.Focus();
        }     

        private bool ValidateLabelCount()
        {
            if (int.Parse(txtLabelCount.Text.Trim()) > int.Parse(txtBoxesPacked.Text.Trim()))
            {               
                GlobalMessageBox.Show(Messages.LABELCOUNT_VALIDATION, Constants.AlertType.Warning, Messages.INFORMATION, GlobalMessageBoxButtons.OK);                                                                          
                return false;
            }
            else
            {
                return true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (GlobalMessageBox.Show(Messages.CANCELMESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)                                 
            {
                ClearControls();
                txtInnerLotNo.Focus();
            } 
        }

        private void ClearControls()
        {
            txtInnerLotNo.Text = string.Empty;   
            txtPoNumber.Text = string.Empty;
            txtItemNo.Text = string.Empty;
            txtItemName.Text = string.Empty;
            txtGroupId.Text = string.Empty;
            txtSerialNo.Text = string.Empty;
            txtSerialNo2.Text = string.Empty;
            txtBatchNo.Text = string.Empty;
            txtBatchNo2.Text = string.Empty;
            txtBoxesPacked.Text = string.Empty;
            txtPalletId.Text = string.Empty;
            txtLabelCount.Text = string.Empty;
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "ClearForm", null);
            }
        }

    }
}
