using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;
using System.Data;
using System.Linq;
using System.Drawing;

namespace Hartalega.FloorSystem.Windows.UI.BrandMaster
{
    public partial class BrandMasterWarehouseEdit : FormBase
    {
        #region Member Variables

        private string _screenName = "Brand Master - Warehouse";
        private string _className = "Brand Master Warehouse";
        private string _itemID = string.Empty;
        private string _loggedInUser;
        private List<DropdownDTO> _hartalegasize;
        BrandMasterDTO _brandMaster = new BrandMasterDTO();

        #endregion

        #region Constructor

        public BrandMasterWarehouseEdit(BrandMasterDTO brandMasterDTO, string loggedInUser)
        {
            try
            {
                InitializeComponent();
                _loggedInUser = loggedInUser;
                _itemID = brandMasterDTO.ITEMID;
                txtFGCode.Text = brandMasterDTO.ITEMID;
                txtBrandName.Text = brandMasterDTO.BRANDNAME;
                txtGloveType.Text = brandMasterDTO.GLOVECODE;
                txtStatus.Text = Constants.ACTIVE;
                txtPreshipment.SerialNo();
                txtPalletQuantity.SerialNo();
                BindItem();

                if (brandMasterDTO.ACTIVE == 0)
                {
                    txtStatus.Text = Constants.INACTIVE;
                    btnSave.Visible = false;
                    txtPreshipment.Enabled = false;
                    txtPreshipment.ReadOnly = true;
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BrandMasterWarehouseEdit", null);
                return;
            }
        }

        #endregion

        #region Event Handler

        private void BrandMasterWarehouseEdit_Load(object sender, EventArgs e)
        {
            try
            {
                getBrandLineDetails();

            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BrandMasterWarehouseEdit_Load", null);
                return;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int rowsReturned = 0;

            int integer;

            BrandMasterDTO newitem = new BrandMasterDTO();

            try
            {
                string vldmsg = string.Empty;

                if (string.IsNullOrEmpty(txtPalletQuantity.Text))
                    if (!int.TryParse(txtPalletQuantity.Text, out integer))
                        vldmsg = "Pallet Quantity";

                if (string.IsNullOrEmpty(vldmsg))
                {
                    newitem.PALLETCAPACITY = Convert.ToInt32(txtPalletQuantity.Text);
                    newitem.ActionType = Constants.ActionLog.Update;

                    BrandMasterDTO olditem = BrandMasterBLL.GetBrandMasterDetails(_itemID);
                    newitem.ITEMID = olditem.ITEMID;
                    newitem.BRANDNAME = olditem.BRANDNAME;
                    newitem.GLOVECODE = olditem.GLOVECODE;
                    newitem.ACTIVE = olditem.ACTIVE;
                    newitem.ALTERNATEGLOVECODE1 = olditem.ALTERNATEGLOVECODE1;
                    newitem.ALTERNATEGLOVECODE2 = olditem.ALTERNATEGLOVECODE2;
                    newitem.ALTERNATEGLOVECODE3 = olditem.ALTERNATEGLOVECODE3;
                    newitem.EXPIRY = olditem.EXPIRY;
                    newitem.GCLABEL = olditem.GCLABEL;
                    newitem.INNERLABELSET = olditem.INNERLABELSET;
                    newitem.INNERLABELSETDATEFORMAT = olditem.INNERLABELSETDATEFORMAT;
                    newitem.INNERPRINTER = olditem.INNERPRINTER;
                    newitem.LOTVERIFICATION = olditem.LOTVERIFICATION;
                    newitem.MANUFACTURINGDATEON = olditem.MANUFACTURINGDATEON;
                    newitem.OUTERLABELSETDATEFORMAT = olditem.OUTERLABELSETDATEFORMAT;
                    newitem.OUTERLABELSETNO = olditem.OUTERLABELSETNO;
                    newitem.PRESHIPMENTPLAN = olditem.PRESHIPMENTPLAN;
                    newitem.SPECIALINNERCHARACTER = olditem.SPECIALINNERCHARACTER;
                    newitem.SPECIALINNERCODE = olditem.SPECIALINNERCODE;
                    newitem.NOOFLABELPRINT = olditem.NOOFLABELPRINT; //Pang 11Nov2019 noOfLabelPrint bug fix

                    rowsReturned = BrandMasterBLL.EditBrandMaster(olditem, newitem, _loggedInUser, _screenName);
                    if (rowsReturned > 0)
                    {
                        //event log myadamas 20190227
                        EventLogDTO EventLog = new EventLogDTO();

                        EventLog.CreatedBy = _loggedInUser;
                        Constants.EventLog audAction = Constants.EventLog.Save;
                        EventLog.EventType = Convert.ToInt32(audAction);
                        EventLog.EventLogType = Constants.eventlogtype;

                        var screenid = CommonBLL.GetScreenIdByScreenName(_screenName);
                        CommonBLL.InsertEventLog(EventLog, _screenName, screenid.ToString());

                        GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        this.Close();
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                    }
                }
                else
                {
                    GlobalMessageBox.Show(Messages.REQUIREDFIELDMESSAGE + Environment.NewLine + vldmsg, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                    return;
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "btnSave_Click", null);
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (GlobalMessageBox.Show(Messages.LEAVE_PAGE_MESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
            {
                this.Close();
            }
        }

        #endregion

        #region User Methods

        private void BindItem()
        {
            List<DropdownDTO> selections = new List<DropdownDTO>();
            try
            {
                if (_itemID != string.Empty)
                    _brandMaster = BrandMasterBLL.GetBrandMasterDetails(_itemID);
                selections = BrandMasterBLL.GetEnumMaster(Constants.MANUFACTURINGDATEON);
                cmbLotVeri.BindComboBox(BrandMasterBLL.GetEnumMaster(Constants.LOTVERIFICATION), true);
                cmbPrinter.BindComboBox(BrandMasterBLL.GetEnumMaster(Constants.PRINTER), true);
                cmbInnerDateFormat.BindComboBox(BrandMasterBLL.GetEnumMaster(Constants.LABELSETDATEFORMAT), true);
                cmbOuterDateFormat.BindComboBox(BrandMasterBLL.GetEnumMaster(Constants.LABELSETDATEFORMAT), true);
                if (!string.IsNullOrEmpty(txtFGCode.Text))
                {
                    cmbInner.BindComboBox(SetUpConfigurationBLL.GetInnerLabelSetNo(txtFGCode.Text), true);
                    cmbOuter.BindComboBox(SetUpConfigurationBLL.GetOuterLabelSetNo(txtFGCode.Text), true);
                }
                _hartalegasize = SetUpConfigurationBLL.GetCommonSize();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindItem", null);
                return;
            }
            cmbInner.Text = _brandMaster.INNERLABELSET;
            cmbInnerDateFormat.SelectedValue = _brandMaster.INNERLABELSETDATEFORMAT;
            txtExpiry.Text = Convert.ToString(_brandMaster.EXPIRY);
            if (_brandMaster.SPECIALINNERCODE == 1)
                cbSpecialInnerCode.Checked = true;
            else
                cbSpecialInnerCode.Checked = false;
            txtSpecialInnerCode.Text = _brandMaster.SPECIALINNERCHARACTER;
            cmbLotVeri.SelectedValue = Convert.ToString(_brandMaster.LOTVERIFICATION);
            cmbPrinter.SelectedValue = Convert.ToString(_brandMaster.INNERPRINTER);
            cmbOuter.Text = _brandMaster.OUTERLABELSETNO;
            cmbOuterDateFormat.SelectedValue = _brandMaster.OUTERLABELSETDATEFORMAT;
            if (_brandMaster.GCLABEL == 1)
                cbGCLabel.Checked = true;
            else
                cbGCLabel.Checked = false;
            switch (_brandMaster.MANUFACTURINGDATEON)
            {
                case 0:
                    rb1.Checked = true;
                    break;
                case 1:
                    rb2.Checked = true;
                    break;
                case 2:
                    rb3.Checked = true;
                    break;
                case 3:
                    rb4.Checked = true;
                    break;
                //KahHeng 29Jan2019 added case for PODate (rb5) and POReceivedDate (rb6)
                case 4:
                    rb5.Checked = true;
                    break;
                //KahHeng End
                //Pang 09/02/2021 First Carton Packing Date
                case 5:
                    rb6.Checked = true;
                    break;
                //Pang End
            }
            txtPreshipment.Text = Convert.ToString(_brandMaster.PRESHIPMENTPLAN);
            txtPalletQuantity.Text = Convert.ToString(_brandMaster.PALLETCAPACITY);
            txtAltCode1.Text = _brandMaster.ALTERNATEGLOVECODE1;
            txtAltCode2.Text = _brandMaster.ALTERNATEGLOVECODE2;
            txtAltCode3.Text = _brandMaster.ALTERNATEGLOVECODE3;
            //rb1.Text = selections[0].DisplayField;
            //rb2.Text = selections[1].DisplayField;
            //rb3.Text = selections[2].DisplayField;
            //rb4.Text = selections[3].DisplayField;

            //Azman 26/07/2018 Properly assign text based on EnumValue 
            foreach (DropdownDTO dr in selections)
            {
                if (dr.IDField == "0")
                    rb1.Text = dr.DisplayField;
                else if (dr.IDField == "1")
                    rb2.Text = dr.DisplayField;
                else if (dr.IDField == "2")
                    rb3.Text = dr.DisplayField;
                else if (dr.IDField == "3")
                    rb4.Text = dr.DisplayField;

                //KahHeng 28Jan2019 added else if statement for PODate (rb5) and POReceivedDate (rb6)
                else if (dr.IDField == "4")
                    rb5.Text = dr.DisplayField;
                //KahHeng End
                //Pang 09/02/2021 First Carton Packing Date
                else if (dr.IDField == "5")
                    rb6.Text = dr.DisplayField;
                //Pang End
            }
            //Azman End

            //KahHeng 28Jan2019 added case for PODate (rb5) and POReceivedDate (rb6)
            rb5.Text = selections[4].DisplayField;
            //KahHeng End

            //Pang Start 11Nov2019 noOfLabelPrint bug fix
            txtNoofLabelToPrint.Text = Convert.ToString(_brandMaster.NOOFLABELPRINT);
            //Pang End
        }

        private void getBrandLineDetails()
        {
            dgvLineSelection.Rows.Clear();
            List<BrandLineDTO> brandLineDTO = null;
            try
            {
                brandLineDTO = BrandMasterBLL.GetBrandLineList(_itemID);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "getBrandLineDetails", null);
                return;
            }
            if (brandLineDTO != null)
            {
                for (int i = 0; i < brandLineDTO.Count; i++)
                {
                    dgvLineSelection.Rows.Add();
                    dgvLineSelection.Rows[i].ReadOnly = true;

                    dgvLineSelection[0, i].Value = dgvLineSelection.Rows.Count;
                    dgvLineSelection["CustomerSize", i].Value = brandLineDTO[i].CUSTOMERSIZE;

                    DataGridViewComboBoxCell cmb = dgvLineSelection["cmbHartalegaSize", i] as DataGridViewComboBoxCell;
                    cmb.DataSource = _hartalegasize;
                    cmb.ValueMember = "IDField";
                    cmb.DisplayMember = "DisplayField";
                    // due to some of the size have been stopped=1 to make sure data exist in drop down only bind
                    List<DropdownDTO> HartalegaSizeList = _hartalegasize;

                    for (int j = 0; j < HartalegaSizeList.Count; j++)
                    {
                        if (HartalegaSizeList[j].IDField.ToString().ToUpper() == brandLineDTO[i].HARTALEGACOMMONSIZE.ToUpper())
                        {
                            cmb.Value = brandLineDTO[i].HARTALEGACOMMONSIZE;
                            break;
                        }
                    }
                    //cmb.Value = brandLineDTO[i].HARTALEGACOMMONSIZE;


                    dgvLineSelection["BaseUnit", i].Value = brandLineDTO[i].BASEUNIT;
                    dgvLineSelection["NoOfPiecesInBaseUnit", i].Value = brandLineDTO[i].NUMOFBASEUNITPIECE;
                    dgvLineSelection["NetWeight", i].Value = brandLineDTO[i].NETWEIGHT;
                    dgvLineSelection["PackagingWeight", i].Value = brandLineDTO[i].PACKAGINGWEIGHT;
                    dgvLineSelection["GrossWeight", i].Value = brandLineDTO[i].GROSSWEIGHT;
                    dgvLineSelection["NoOfPiecesInOuterCase", i].Value = brandLineDTO[i].GLOVESINNERBOXNO * brandLineDTO[i].INNERBOXINCASENO;
                    dgvLineSelection["NoOfGlovesInInnerBox", i].Value = brandLineDTO[i].GLOVESINNERBOXNO;
                    dgvLineSelection["NoOfInnerBoxInCase", i].Value = brandLineDTO[i].INNERBOXINCASENO;
                    dgvLineSelection["InnerProductCode", i].Value = brandLineDTO[i].INNERPRODUCTCODE;
                    dgvLineSelection["OuterProductCode", i].Value = brandLineDTO[i].OUTERPRODUCTCODE;
                    dgvLineSelection["CompanyCategory", i].Value = brandLineDTO[i].COMPANYCATEGORYCODE;
                    dgvLineSelection["Reference1", i].Value = brandLineDTO[i].REFERENCE1;
                    dgvLineSelection["Reference2", i].Value = brandLineDTO[i].REFERENCE2;
                    dgvLineSelection["NoOfPackers", i].Value = brandLineDTO[i].NUMOFPACKERS;
                    dgvLineSelection["PackPiecesPerHr", i].Value = brandLineDTO[i].PACKPCSPERHR;
                    dgvLineSelection["BrandLineID", i].Value = brandLineDTO[i].AVABRANDLINE_ID;
                    dgvLineSelection["PrintingSize", i].Value = brandLineDTO[i].PRINTINGSIZE;
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

        #endregion
    }
}
