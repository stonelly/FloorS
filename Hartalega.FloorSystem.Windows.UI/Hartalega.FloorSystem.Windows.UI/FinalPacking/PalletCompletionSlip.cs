using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Windows.UI.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Common;
namespace Hartalega.FloorSystem.Windows.UI.FinalPacking
{
    public partial class PalletCompletionSlip : FormBase 
    {
        List<SOLineDTO> lstPolist = null;
        List<SOLineDTO> lstItemNumber = null;
        private string _screenname = "Pallet Completion Slip"; //To use for DB logging.
        private string _uiClassName = "PalletCompletionSlip"; //To use for DB logging.
        private string _operatorName = string.Empty;
        private Boolean _isValidUser = false;
        private Boolean _isPreshipment;
        private string _poNumber = string.Empty;
        public PalletCompletionSlip()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PalletCompletionSlip_Load(object sender, EventArgs e)
        {
            try
            {
                txtOperatorId.OperatorId();
                txtLocation.Text = Convert.ToString(WorkStationDTO.GetInstance().LocationId);
                BindDefaults();
                this.WindowState = FormWindowState.Maximized;
                // FX - add pallet combobox event
                cmbPalletId.SelectedIndexChanged += cmbPalletId_SelectionChangeCommitted;
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "PalletCompletionSlip_Load", null);
            }
        }
        /// <summary>
        /// Valisate Operator Id on leave event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtOperatorId_leave(object sender, EventArgs e)
        {
            string operatorName = string.Empty;
            _isValidUser = false;
            if (!string.IsNullOrEmpty(txtOperatorId.Text.Trim()))
            {
                try
                {
                    if (SecurityModuleBLL.ValidateOperatorAccess(txtOperatorId.Text.Trim(), _screenname))
                    {
                        _operatorName = CommonBLL.GetOperatorNameQAI((txtOperatorId.Text).Trim());
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.OPERATOR_SCREEN_ACCESS_MSG, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                        txtOperatorId.Text = String.Empty;
                        txtOperatorId.Focus();
                    }
                }
                catch (FloorSystemException ex)
                {
                    ExceptionLogging(ex, _screenname, _uiClassName, "txtOperatorId_Leave", Convert.ToInt16(txtOperatorId.Text));
                    return;
                }
            }            
        }       
        /// <summary>
        /// Purcahse order selection change event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbPurchaseOrder_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string poNumber = Convert.ToString(cmbPurchaseOrder.SelectedItem);
            lstItemNumber = lstPolist.Where(SOLineDTO => SOLineDTO.PONumber == poNumber).ToList();
            cmbItemNumber.Items.Clear();
            cmbItemNumber.SelectedIndexChanged -= cmbItemNumber_SelectionChangeCommited;
            foreach (SOLineDTO soline in lstItemNumber)
            {
                if (!cmbItemNumber.Items.Contains(soline.ItemNumber))
                {                   
                    cmbItemNumber.Items.Add(soline.ItemNumber);
                }
            }            
            cmbItemNumber.SelectedIndexChanged += cmbItemNumber_SelectionChangeCommited;
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
          //  ClearForm();
        }        
        /// <summary>
        /// Generate the Pallet details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_click(object sender, EventArgs e)
        {
            try
            {
                txtOperatorId_leave(txtOperatorId, e);
                if(!_isValidUser)
                {
                    if (ValidateRequiredFields())
                    {
                        PalletInfoDTO objPalletInfoDTO = FinalPackingBLL.GetPalletInfo(Convert.ToString(cmbPlant.SelectedValue), _poNumber, cmbItemNumber.Text, cmbItemSize.Text, cmbPalletId.Text);
						string customerSize = FinalPackingBLL.GetCustomerSizeByPO(_poNumber, cmbItemNumber.Text, cmbItemSize.Text);
                        
						if (objPalletInfoDTO != null)
                        {
                            if (objPalletInfoDTO.Isavailable == true)
                            {
                                if (GlobalMessageBox.Show(Messages.PALLETCLOSURE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                                {
                                    //Generate the pallet text file in the Ewarenavi file   
                                    if(Convert.ToBoolean(FloorSystemConfiguration.GetInstance().boolEWareNaviFileGenerator))
                                        if (FinalPackingBLL.OBPWMSCheckWorkOrderType(_poNumber) == false)
                                            FinalPackingBLL.GeneratePalletFile(objPalletInfoDTO.Palletid, objPalletInfoDTO.ispreshipmentCase, cmbItemNumber.Text, objPalletInfoDTO.Casespacked, objPalletInfoDTO.Ponumber, objPalletInfoDTO.Size);
                                    //edit by Cheah (17Aug2017)
                                    // GeneratePalletFile to be disabled once ewarenavi upgrade - TBD
                                    if(Convert.ToBoolean(FloorSystemConfiguration.GetInstance().boolEWareNaviSQL))
                                        if (FinalPackingBLL.OBPWMSCheckWorkOrderType(_poNumber) == false)
                                            //FinalPackingBLL.InsertEWareNaviData(objPalletInfoDTO.Palletid, objPalletInfoDTO.ispreshipmentCase, cmbItemNumber.Text, objPalletInfoDTO.Casespacked, objPalletInfoDTO.Ponumber, customerSize);
                                            //New column FGCodeAndSize (HartalegaCommonSize) for IOT scan, original Item (CustomerSize) field for EWN and lifter scan.
                                            FinalPackingBLL.InsertEWareNaviData(objPalletInfoDTO.Palletid, objPalletInfoDTO.ispreshipmentCase, cmbItemNumber.Text, objPalletInfoDTO.Casespacked, objPalletInfoDTO.Ponumber, objPalletInfoDTO.Size, customerSize);
									//end edit (17Aug2017)

                                    // update the pallet closure
                                    FinalPackingBLL.UpdatePalletClosure(objPalletInfoDTO.Palletid);
                                }
                            }
                            if (GlobalMessageBox.Show(Messages.PRINT_CONFIRM, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                            {
                                CompletedPalletSlip objCompletedInfoDTO = new CompletedPalletSlip(objPalletInfoDTO);
                                objCompletedInfoDTO._operatorId = txtOperatorId.Text;
                                objCompletedInfoDTO._operatorName = _operatorName;
                                objCompletedInfoDTO.ShowDialog();
                                ClearControls();
                            }
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.NO_DATA_FOUND, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);                         
                            ClearControls();
                        }
                    }
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "btnOk_click", null);
            }
        }
        /// <summary>
        /// bind default values
        /// </summary>
        private void BindDefaults()
        {
            List<string> lstPalletId = FinalPackingBLL.GetPalletCompletionSlipPalletIdList();
            string[] arraylstPalletId = (lstPalletId.ToArray());
            cmbPalletId.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbPalletId.AutoCompleteSource = AutoCompleteSource.CustomSource;
            cmbPalletId.Items.Clear();
            if (lstPalletId != null)
            {
                cmbPalletId.Items.AddRange(arraylstPalletId);   // yiksiu add items one time to increase performance
                cmbPalletId.AutoCompleteCustomSource.AddRange(arraylstPalletId);
            }
                //foreach (string plt in lstPalletId)
                //{
                //    cmbPalletId.Items.Add(plt);
                //    cmbPalletId.AutoCompleteCustomSource.Add(plt);
                //}
            // FX - Comment Code event and move combobox event trigger to page load to prevent multiple evet trigger  added
            //cmbPalletId.SelectedIndexChanged += cmbPalletId_SelectionChangeCommitted;
        }
        /// <summary>
        /// required field validation
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateRequiredFields()
        {
            validationMesssageLst = new List<ValidationMessage>();
            validationMesssageLst.Add(new ValidationMessage(txtOperatorId, Messages.REQID, ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbPalletId, Messages.REQPALLETREQUIRED, ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbPlant, Messages.REQPLANT, ValidationType.Required));
            validationMesssageLst.Add(new ValidationMessage(cmbPurchaseOrder, Messages.REQPONUMBER, ValidationType.Required));
            if (!_isPreshipment)
            {
                validationMesssageLst.Add(new ValidationMessage(cmbItemNumber, Messages.REQITEM, ValidationType.Required));
                validationMesssageLst.Add(new ValidationMessage(cmbItemSize, Messages.REQITEMSIZE, ValidationType.Required));
            }
            return ValidateForm();
        }
        /// <summary>
        /// item number selection index changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbItemNumber_SelectionChangeCommited(object sender, EventArgs e)
        {
            try
            {
                SOLineDTO objItemNumber = new SOLineDTO();              
                string itemNumber = cmbItemNumber.Text;
                foreach (SOLineDTO objItemNum in lstItemNumber)
                {
                    if (objItemNum.ItemNumber == itemNumber)
                        objItemNumber = objItemNum;
                }
                List<SOLineDTO> listItemSize = lstItemNumber.Where(SOLineDTO => SOLineDTO.ItemNumber == itemNumber).ToList();
                cmbItemSize.DataSource = listItemSize;
                cmbItemSize.ValueMember = "ItemSize";
                cmbItemSize.DisplayMember = "ItemSize";
                cmbItemSize.SelectedIndex = Constants.MINUSONE;
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "cmbItemNumber_SelectionChangeCommit", null);
            }
        }

        /// <summary>
        /// pallet id selection index changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
          private void cmbPalletId_SelectionChangeCommitted(object sender,EventArgs e)
          {
              try
              {
                  PalletInfoDTO objPallet = FinalPackingBLL.GetPalletInfoByPalletId(Convert.ToString(cmbPalletId.SelectedItem));
                  if (objPallet != null)
                  {
                      cmbPurchaseOrder.Items.Add(objPallet.CustomerRefPo);
                      cmbPurchaseOrder.SelectedItem = objPallet.CustomerRefPo;
                      cmbItemNumber.Items.Add(objPallet.ItemNumber);
                      cmbItemNumber.SelectedItem = objPallet.ItemNumber;
                      cmbItemSize.Items.Add(objPallet.Size);
                      cmbItemSize.SelectedItem = objPallet.Size;
                      List<DropdownDTO> lstDdl = new List<DropdownDTO>() { new DropdownDTO() { IDField = objPallet.locationId.ToString(), DisplayField = objPallet.Location } };
                      cmbPlant.BindComboBox(lstDdl, false);
                      cmbPlant.SelectedItem = objPallet.Location;
                      _isPreshipment = objPallet.ispreshipmentCase;
                      _poNumber = objPallet.Ponumber;
                  }
              }
              catch (FloorSystemException ex)
              {
                  ExceptionLogging(ex, _screenname, _uiClassName, "cmbPalletId_SelectionChangeCommitted", null);
              }
          }


        /// <summary>
        /// Cancel button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (GlobalMessageBox.Show(Messages.CANCELMESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)       
            {
                ClearControls();
                txtOperatorId.Focus();
            }
        }     
        /// <summary>
        /// Clear Form
        /// </summary>
        private void ClearControls()
        {
            txtOperatorId.Text = string.Empty;
            cmbPurchaseOrder.SelectedIndex = Constants.MINUSONE;
            cmbPlant.SelectedIndex = Constants.MINUSONE;
            cmbPalletId.SelectedIndex = Constants.MINUSONE;
            cmbItemSize.SelectedIndex = Constants.MINUSONE;
            cmbItemNumber.SelectedIndex = Constants.MINUSONE;
            txtOperatorId.Focus();
            try
            {
                CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
                BindDefaults();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenname, _uiClassName, "ClearForm", null);
            }

        }
        /// <summary>
        /// Pallet selection event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbPalletId_Click(object sender, EventArgs e)
        {
            if (!cmbPalletId.Items.Contains(cmbPalletId.Text.ToUpper()) && cmbPalletId.Text != String.Empty)
            {
                cmbPalletId.Text = String.Empty;
                cmbPalletId.Focus();
            }
        }
    }
}
