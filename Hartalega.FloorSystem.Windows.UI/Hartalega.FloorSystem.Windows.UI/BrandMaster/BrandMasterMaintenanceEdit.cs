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
    public partial class BrandMasterMaintenanceEdit : FormBase
    {
        #region Member Variables
        private string _screenName = "Brand Master - Maintenance";
        private string _className = "Brand Master Maintenance Edit";
        private string _itemID = string.Empty;
        private string _loggedInUser;
        private DataGridViewRow _cell = null;
        private List<DropdownDTO> _hartalegasize;
        BrandMasterDTO _brandMaster = new BrandMasterDTO();

        #endregion

        #region Constructor

        public BrandMasterMaintenanceEdit(BrandMasterDTO brandMasterDTO, string loggedInUser)
        {
            InitializeComponent();

            List<DropdownDTO> gloveTypes = CommonBLL.GetGloveType();
            gloveTypes.Insert(0, new DropdownDTO());
            BindingSource gloveType1 = new BindingSource();
            gloveType1.DataSource = gloveTypes;
            BindingSource gloveType2 = new BindingSource();
            gloveType2.DataSource = gloveTypes;
            BindingSource gloveType3 = new BindingSource();
            gloveType3.DataSource = gloveTypes;

            cmbAltCode1.BindMultipleComboBox(gloveType1, false);
            cmbAltCode2.BindMultipleComboBox(gloveType2, false);
            cmbAltCode3.BindMultipleComboBox(gloveType3, false);

            _loggedInUser = loggedInUser;
            _itemID = brandMasterDTO.ITEMID;
            txtFGCode.Text = brandMasterDTO.ITEMID;
            txtBrandName.Text = brandMasterDTO.BRANDNAME;
            txtGloveType.Text = brandMasterDTO.GLOVECODE;
            txtStatus.Text = Constants.ACTIVE;
            txtExpiry.SerialNo();
            txtPalletQuantity.SerialNo();
            txtPreshipment.SerialNo();
            txtNoofLabelToPrint.SerialNo();

            if (brandMasterDTO.NOOFLABELPRINT != null)
                txtNoofLabelToPrint.Text = Convert.ToString(brandMasterDTO.NOOFLABELPRINT);

            if (brandMasterDTO.ACTIVE == 0)
            {
                txtStatus.Text = Constants.INACTIVE;
                cmbInner.Enabled = false;
                cmbInnerDateFormat.Enabled = false;
                txtExpiry.Enabled = false;
                cbSpecialInnerCode.Enabled = false;
                txtSpecialInnerCode.Enabled = false;
                cmbLotVeri.Enabled = false;
                cmbPrinter.Enabled = false;
                cmbOuter.Enabled = false;
                cmbOuterDateFormat.Enabled = false;
                rb1.Enabled = false;
                rb2.Enabled = false;
                rb3.Enabled = false;
                rb4.Enabled = false;
                //KahHeng 28Jan2019 added rb5 and rb6 for PODate and POReceivedDate
                rb5.Enabled = false;
                //KahHeng End
                cbGCLabel.Enabled = false;
                cmbAltCode1.Enabled = false;
                cmbAltCode2.Enabled = false;
                cmbAltCode3.Enabled = false;
                btnSave.Visible = false;
            }
        }

        #endregion

        #region Event Handler

        private void dgvLineSelection_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvLineSelection.Columns[e.ColumnIndex].Name == "Update")
                {
                    BrandLineDTO newitem = new BrandLineDTO();

                    double doub;
                    int integer;
                    string vldmsg = string.Empty;
                    string invalidmsg = string.Empty;

                    if (string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["CustomerSize"].Value)))
                        vldmsg += "\nCustomer Size";
                    if (string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["cmbHartalegaSize"].Value)))
                        vldmsg += "\nHartalega Size";
                    if (string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["PrintingSize"].Value)))
                        vldmsg += "\nPrinting Size";
                    if (string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["BaseUnit"].Value)))
                        vldmsg += "\nBase Unit";

                    if (string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["NoOfPiecesInBaseUnit"].Value)))
                    {
                        vldmsg += "\nNo of Pieces in Base Unit Price";
                    }
                    else
                    {
                        if (!int.TryParse(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["NoOfPiecesInBaseUnit"].Value), out integer))
                            invalidmsg += "\nNo of Pieces in Base Unit Price";
                    }

                    if (!string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["NetWeight"].Value)))
                    {
                        if (!Double.TryParse(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["NetWeight"].Value), out doub))
                            invalidmsg += "\nNet Weight";
                    }

                    if (!string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["PackagingWeight"].Value)))
                    {
                        if (!Double.TryParse(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["PackagingWeight"].Value), out doub))
                            invalidmsg += "\nPackaging Weight";
                    }

                    if (!string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["GrossWeight"].Value)))
                    {
                        if (!Double.TryParse(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["GrossWeight"].Value), out doub))
                            invalidmsg += "\nGross Weight";
                    }

                    if (string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["NoOfGlovesInInnerBox"].Value)))
                    {
                        vldmsg += "\nNo of Gloves in Inner Box";
                    }
                    else
                    {
                        if (!int.TryParse(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["NoOfGlovesInInnerBox"].Value), out integer))
                            invalidmsg += "\nNo of Gloves in Inner Box";
                    }
                    if (string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["NoOfInnerBoxInCase"].Value)))
                    {
                        vldmsg += "\nNo of Inner Box in Case";
                    }
                    else
                    {
                        if (!int.TryParse(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["NoOfInnerBoxInCase"].Value), out integer))
                            invalidmsg += "\nNo of Inner Box in Case";
                    }

                    if (!string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["NoOfPackers"].Value)))
                    {
                        if (!int.TryParse(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["NoOfPackers"].Value), out integer))
                            invalidmsg += "\nNo of Packers";
                    }

                    if (!string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["PackPiecesPerHr"].Value)))
                    {
                        if (!int.TryParse(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["PackPiecesPerHr"].Value), out integer))
                            invalidmsg += "\nPack Piece per Hour";
                    }


                    if (string.IsNullOrEmpty(vldmsg) && string.IsNullOrEmpty(invalidmsg))
                    {
                        newitem.HARTALEGACOMMONSIZE = Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["cmbHartalegaSize"].Value);
                        newitem.BASEUNIT = Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["BaseUnit"].Value);
                        newitem.NUMOFBASEUNITPIECE = Convert.ToInt32(dgvLineSelection.Rows[e.RowIndex].Cells["NoOfPiecesInBaseUnit"].Value);
                        newitem.CUSTOMERSIZE = Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["CustomerSize"].Value);
                        newitem.ITEMID = _itemID;
                        newitem.STOPPED = 0;
                        newitem.ActionType = Constants.ActionLog.Update;
                        newitem.PRINTINGSIZE = Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["PrintingSize"].Value);
                        newitem.GLOVESINNERBOXNO = Convert.ToInt32(dgvLineSelection.Rows[e.RowIndex].Cells["NoOfGlovesInInnerBox"].Value);
                        newitem.INNERBOXINCASENO = Convert.ToInt32(dgvLineSelection.Rows[e.RowIndex].Cells["NoOfInnerBoxInCase"].Value);
                        newitem.AVABRANDLINE_ID = Convert.ToInt32(dgvLineSelection.Rows[e.RowIndex].Cells["BrandLineID"].Value);

                        if (!string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["NetWeight"].Value)))
                        {
                            newitem.NETWEIGHT = Convert.ToDouble(dgvLineSelection.Rows[e.RowIndex].Cells["NetWeight"].Value);
                        }
                        else
                        {
                            newitem.NETWEIGHT = 0.00;
                        }

                        if (!string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["PackagingWeight"].Value)))
                        {
                            newitem.PACKAGINGWEIGHT = Convert.ToDouble(dgvLineSelection.Rows[e.RowIndex].Cells["PackagingWeight"].Value);
                        }
                        else
                        {
                            newitem.PACKAGINGWEIGHT = 0.00;
                        }

                        if (!string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["GrossWeight"].Value)))
                        {
                            newitem.GROSSWEIGHT = Convert.ToDouble(dgvLineSelection.Rows[e.RowIndex].Cells["GrossWeight"].Value);
                        }
                        else
                        {
                            newitem.GROSSWEIGHT = 0.00;
                        }

                        if (!string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["Reference1"].Value)))
                        {
                            newitem.REFERENCE1 = Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["Reference1"].Value);
                        }

                        if (!string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["Reference2"].Value)))
                        {
                            newitem.REFERENCE2 = Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["Reference2"].Value);
                        }

                        if (!string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["InnerProductCode"].Value)))
                        {
                            newitem.INNERPRODUCTCODE = Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["InnerProductCode"].Value);
                        }

                        if (!string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["OuterProductCode"].Value)))
                        {
                            newitem.OUTERPRODUCTCODE = Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["OuterProductCode"].Value);
                        }

                        if (!string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["CompanyCategory"].Value)))
                        {
                            newitem.COMPANYCATEGORYCODE = Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["CompanyCategory"].Value);
                        }

                        if (!string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["NoOfPackers"].Value)))
                        {
                            newitem.NUMOFPACKERS = Convert.ToInt32(dgvLineSelection.Rows[e.RowIndex].Cells["NoOfPackers"].Value);
                        }
                        else
                        {
                            newitem.NUMOFPACKERS = 0;
                        }

                        if (!string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["PackPiecesPerHr"].Value)))
                        {
                            newitem.PACKPCSPERHR = Convert.ToInt32(dgvLineSelection.Rows[e.RowIndex].Cells["PackPiecesPerHr"].Value);

                        }
                        else
                        {
                            newitem.PACKPCSPERHR = 0;
                        }

                        if (BrandMasterBLL.isDuplicateHartalegaCommonSizeUpdate(newitem.ITEMID, newitem.HARTALEGACOMMONSIZE, newitem.AVABRANDLINE_ID) == 0)
                        {
                            BrandLineDTO olditem = BrandMasterBLL.GetBrandLineList(_itemID).Where(p => p.AVABRANDLINE_ID == newitem.AVABRANDLINE_ID).FirstOrDefault();
                            BrandMasterBLL.SaveBrandLineListChanges(olditem, newitem, _loggedInUser, _itemID, _screenName);
                            GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            getBrandLineDetails();
                            btnAdd.Enabled = true;
                            return;
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.DUPLICATE_HARTALEGACOMMONSIZE, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        }
                    }
                    else if (!string.IsNullOrEmpty(vldmsg))
                    {
                        GlobalMessageBox.Show(Messages.REQUIREDFIELDMESSAGE + Environment.NewLine + vldmsg, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                        return;
                    }
                    else if (!string.IsNullOrEmpty(invalidmsg))
                    {
                        GlobalMessageBox.Show(Messages.INVALID_DATA + Environment.NewLine + invalidmsg, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                        return;
                    }
                    btnAdd.Enabled = true;
                    return;
                }
                else if (dgvLineSelection.Columns[e.ColumnIndex].Name == "Edit")
                {
                    btnAdd.Enabled = false;
                    dgvLineSelection.Rows[e.RowIndex].ReadOnly = false;
                    dgvLineSelection.Columns[e.ColumnIndex].Name = "Update";
                    dgvLineSelection[e.ColumnIndex, e.RowIndex].Style.NullValue = "Update";
                    dgvLineSelection.Columns[e.ColumnIndex + 1].Name = "Cancel";
                    dgvLineSelection[e.ColumnIndex + 1, e.RowIndex].Style.NullValue = "Cancel";
                    _cell = dgvLineSelection.CurrentRow;
                }
                else if (dgvLineSelection.Columns[e.ColumnIndex].Name == "Cancel" && dgvLineSelection.CurrentRow == _cell)
                {
                    getBrandLineDetails();
                    btnAdd.Enabled = true;
                }
                else if (dgvLineSelection.Columns[e.ColumnIndex].Name == "Delete")
                {
                    btnAdd.Enabled = false;
                    if (GlobalMessageBox.Show(Messages.DELETE_MESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                    {
                        BrandLineDTO newItem = new BrandLineDTO();
                        newItem = BrandMasterBLL.GetBrandLineList(_itemID).Where(p => p.AVABRANDLINE_ID == Convert.ToInt32(dgvLineSelection.Rows[e.RowIndex].Cells["BrandLineID"].Value)).FirstOrDefault();

                        newItem.ActionType = Constants.ActionLog.Delete;
                        newItem.STOPPED = 1;
                        BrandLineDTO oldItem = BrandMasterBLL.GetBrandLineList(_itemID).Where(p => p.AVABRANDLINE_ID == Convert.ToInt32(newItem.AVABRANDLINE_ID)).FirstOrDefault();
                        BrandMasterBLL.SaveBrandLineListChanges(oldItem, newItem, _loggedInUser, _itemID, _screenName);
                        getBrandLineDetails();
                        btnAdd.Enabled = true;
                    }
                    else
                    {
                        btnAdd.Enabled = true;
                    }
                }
                else if (dgvLineSelection.Columns[e.ColumnIndex].Name == "Insert")
                {
                    BrandLineDTO newitem = new BrandLineDTO();
                    BrandLineDTO olditem = new BrandLineDTO();
                    double doub;
                    int integer;
                    string vldmsg = string.Empty;
                    string invalidmsg = string.Empty;

                    if (string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["CustomerSize"].Value)))
                        vldmsg += "\nCustomer Size";
                    if (string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["cmbHartalegaSize"].Value)))
                        vldmsg += "\nHartalega Size";
                    if (string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["PrintingSize"].Value)))
                        vldmsg += "\nPrinting Size";
                    if (string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["BaseUnit"].Value)))
                        vldmsg += "\nBase Unit";

                    if (string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["NoOfPiecesInBaseUnit"].Value)))
                    {
                        vldmsg += "\nNo of Pieces in Base Unit Price";
                    }
                    else
                    {
                        if (!int.TryParse(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["NoOfPiecesInBaseUnit"].Value), out integer))
                            invalidmsg += "\nNo of Pieces in Base Unit Price";
                    }

                    if (!string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["NetWeight"].Value)))
                    {
                        if (!Double.TryParse(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["NetWeight"].Value), out doub))
                            invalidmsg += "\nNet Weight";
                    }

                    if (!string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["PackagingWeight"].Value)))
                    {
                        if (!Double.TryParse(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["PackagingWeight"].Value), out doub))
                            invalidmsg += "\nPackaging Weight";
                    }

                    if (!string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["GrossWeight"].Value)))
                    {
                        if (!Double.TryParse(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["GrossWeight"].Value), out doub))
                            invalidmsg += "\nGross Weight";
                    }

                    if (string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["NoOfGlovesInInnerBox"].Value)))
                    {
                        vldmsg += "\nNo of Gloves in Inner Box";
                    }
                    else
                    {
                        if (!int.TryParse(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["NoOfGlovesInInnerBox"].Value), out integer))
                            invalidmsg += "\nNo of Gloves in Inner Box";
                    }
                    if (string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["NoOfInnerBoxInCase"].Value)))
                    {
                        vldmsg += "\nNo of Inner Box in Case";
                    }
                    else
                    {
                        if (!int.TryParse(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["NoOfInnerBoxInCase"].Value), out integer))
                            invalidmsg += "\nNo of Inner Box in Case";
                    }

                    if (!string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["NoOfPackers"].Value)))
                    {
                        if (!int.TryParse(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["NoOfPackers"].Value), out integer))
                            invalidmsg += "\nNo of Packers";
                    }

                    if (!string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["PackPiecesPerHr"].Value)))
                    {
                        if (!int.TryParse(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["PackPiecesPerHr"].Value), out integer))
                            invalidmsg += "\nPack Piece per Hour";
                    }

                    if (string.IsNullOrEmpty(vldmsg) && string.IsNullOrEmpty(invalidmsg))
                    {
                        newitem.HARTALEGACOMMONSIZE = Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["cmbHartalegaSize"].Value);
                        newitem.BASEUNIT = Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["BaseUnit"].Value);
                        newitem.NUMOFBASEUNITPIECE = Convert.ToInt32(dgvLineSelection.Rows[e.RowIndex].Cells["NoOfPiecesInBaseUnit"].Value);
                        newitem.CUSTOMERSIZE = Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["CustomerSize"].Value);
                        newitem.ITEMID = _itemID;
                        newitem.STOPPED = 0;
                        newitem.ActionType = Constants.ActionLog.Add;
                        newitem.PRINTINGSIZE = Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["PrintingSize"].Value);
                        newitem.REFERENCE1 = Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["Reference1"].Value);
                        newitem.GLOVESINNERBOXNO = Convert.ToInt32(dgvLineSelection.Rows[e.RowIndex].Cells["NoOfGlovesInInnerBox"].Value);
                        newitem.INNERBOXINCASENO = Convert.ToInt32(dgvLineSelection.Rows[e.RowIndex].Cells["NoOfInnerBoxInCase"].Value);

                        if (!string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["NetWeight"].Value)))
                        {
                            newitem.NETWEIGHT = Convert.ToDouble(dgvLineSelection.Rows[e.RowIndex].Cells["NetWeight"].Value);
                        }
                        else
                        {
                            newitem.NETWEIGHT = 0.00;
                        }

                        if (!string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["PackagingWeight"].Value)))
                        {
                            newitem.PACKAGINGWEIGHT = Convert.ToDouble(dgvLineSelection.Rows[e.RowIndex].Cells["PackagingWeight"].Value);
                        }
                        else
                        {
                            newitem.PACKAGINGWEIGHT = 0.00;
                        }

                        if (!string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["GrossWeight"].Value)))
                        {
                            newitem.GROSSWEIGHT = Convert.ToDouble(dgvLineSelection.Rows[e.RowIndex].Cells["GrossWeight"].Value);
                        }
                        else
                        {
                            newitem.GROSSWEIGHT = 0.00;
                        }

                        if (!string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["Reference1"].Value)))
                        {
                            newitem.REFERENCE1 = Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["Reference1"].Value);
                        }

                        if (!string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["Reference2"].Value)))
                        {
                            newitem.REFERENCE2 = Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["Reference2"].Value);
                        }

                        if (!string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["InnerProductCode"].Value)))
                        {
                            newitem.INNERPRODUCTCODE = Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["InnerProductCode"].Value);
                        }

                        if (!string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["OuterProductCode"].Value)))
                        {
                            newitem.OUTERPRODUCTCODE = Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["OuterProductCode"].Value);
                        }

                        if (!string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["CompanyCategory"].Value)))
                        {
                            newitem.COMPANYCATEGORYCODE = Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["CompanyCategory"].Value);
                        }

                        if (!string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["NoOfPackers"].Value)))
                        {
                            newitem.NUMOFPACKERS = Convert.ToInt32(dgvLineSelection.Rows[e.RowIndex].Cells["NoOfPackers"].Value);
                        }
                        else
                        {
                            newitem.NUMOFPACKERS = 0;
                        }

                        if (!string.IsNullOrEmpty(Convert.ToString(dgvLineSelection.Rows[e.RowIndex].Cells["PackPiecesPerHr"].Value)))
                        {
                            newitem.PACKPCSPERHR = Convert.ToInt32(dgvLineSelection.Rows[e.RowIndex].Cells["PackPiecesPerHr"].Value);

                        }
                        else
                        {
                            newitem.PACKPCSPERHR = 0;
                        }
                        //dgvLineSelection["NoOfPiecesInOuterCase", i].Value = brandLineDTO[i].GlovesInnerBoxNo * brandLineDTO[i].InnerBoxInCaseNo;

                        if (BrandMasterBLL.isDuplicateHartalegaCommonSizeInsert(newitem.ITEMID, newitem.HARTALEGACOMMONSIZE) == 0)
                        {
                            BrandMasterBLL.SaveBrandLineListChanges(olditem, newitem, _loggedInUser, _itemID, _screenName);
                            GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            getBrandLineDetails();
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.DUPLICATE_HARTALEGACOMMONSIZE, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        }

                    }
                    else if (!string.IsNullOrEmpty(vldmsg))
                    {
                        GlobalMessageBox.Show(Messages.REQUIREDFIELDMESSAGE + Environment.NewLine + vldmsg, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                        return;
                    }
                    else if (!string.IsNullOrEmpty(invalidmsg))
                    {
                        GlobalMessageBox.Show(Messages.INVALID_DATA + Environment.NewLine + invalidmsg, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                        return;
                    }
                    btnAdd.Enabled = true;
                    return;
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "dgvLineSelection_CellContentClick", null);
                return;
            }
        }

        private void BrandMaintainanceEdit_Load(object sender, EventArgs e)
        {
            try
            {
                dgvLineSelection.CellContentClick -= new System.Windows.Forms.DataGridViewCellEventHandler(dgvLineSelection_CellContentClick);
                cmbInner.SelectedIndexChanged -= new System.EventHandler(this.cmbInner_SelectedIndexChanged);
                cmbOuter.SelectedIndexChanged -= new System.EventHandler(this.cmbOuter_SelectedIndexChanged);
                BindItem();
                getBrandLineDetails();
                dgvLineSelection.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvLineSelection_CellContentClick);
                cmbInner.SelectedIndexChanged += new System.EventHandler(this.cmbInner_SelectedIndexChanged);
                cmbOuter.SelectedIndexChanged += new System.EventHandler(this.cmbOuter_SelectedIndexChanged);
            }
            catch (Exception ex)
            {

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
                //added 17/11/2017 by MYAdamas on validation for label set date if selected label set is custom date
                if (cmbInnerDateFormat.Enabled)
                {
                    if (cmbInnerDateFormat.SelectedIndex < 0)
                    {
                        vldmsg += "Inner Label Set Date Format";
                    }
                }

                if (cmbOuterDateFormat.Enabled)
                {
                    if (cmbOuterDateFormat.SelectedIndex < 0)
                    {
                        vldmsg += "Outer Label Set Date Format";
                    }
                }


                //if (rb1.Checked)
                //    newitem.MANUFACTURINGDATEON = 0;
                //else if (rb2.Checked)
                //    newitem.MANUFACTURINGDATEON = 1;
                //else if (rb3.Checked)
                //    newitem.MANUFACTURINGDATEON = 2;
                //else if (rb4.Checked)
                //    newitem.MANUFACTURINGDATEON = 3;

                //Azman 26/07/2018 Properly assign text based on EnumValue 
                if (rb1.Checked)
                    newitem.MANUFACTURINGDATEON = BrandMasterBLL.GetManufacturingDateValue(rb1.Text);
                else if (rb2.Checked)
                    newitem.MANUFACTURINGDATEON = BrandMasterBLL.GetManufacturingDateValue(rb2.Text);
                else if (rb3.Checked)
                    newitem.MANUFACTURINGDATEON = BrandMasterBLL.GetManufacturingDateValue(rb3.Text);
                else if (rb4.Checked)
                    newitem.MANUFACTURINGDATEON = BrandMasterBLL.GetManufacturingDateValue(rb4.Text);
                //Azman End

                //KahHeng 28Jan2019, added elseif statement for PODate and POReceivedDate
                else if (rb5.Checked)
                    newitem.MANUFACTURINGDATEON = BrandMasterBLL.GetManufacturingDateValue(rb5.Text);
                //KahHeng End

                //Pang 09/02/2021 First Carton Packing Date
                else if (rb6.Checked)
                    newitem.MANUFACTURINGDATEON = BrandMasterBLL.GetManufacturingDateValue(rb6.Text);
                //Pang End

                BrandMasterDTO olditem = BrandMasterBLL.GetBrandMasterDetails(_itemID);
                newitem.ITEMID = olditem.ITEMID;
                newitem.BRANDNAME = olditem.BRANDNAME;
                newitem.GLOVECODE = olditem.GLOVECODE;
                newitem.ACTIVE = olditem.ACTIVE;

                if (string.IsNullOrEmpty(vldmsg))
                {
                    newitem.GCLABEL = cbGCLabel.Checked ? 1 : 0;
                    newitem.LOTVERIFICATION = Convert.ToInt32(cmbLotVeri.SelectedValue);
                    newitem.SPECIALINNERCODE = cbSpecialInnerCode.Checked ? 1 : 0;

                    if (!string.IsNullOrEmpty((string)cmbAltCode1.SelectedValue))
                    {
                        newitem.ALTERNATEGLOVECODE1 = (string)cmbAltCode1.SelectedValue;
                    }

                    if (!string.IsNullOrEmpty((string)cmbAltCode2.SelectedValue))
                    {
                        newitem.ALTERNATEGLOVECODE2 = (string)cmbAltCode2.SelectedValue;
                    }

                    if (!string.IsNullOrEmpty((string)cmbAltCode3.SelectedValue))
                    {
                        newitem.ALTERNATEGLOVECODE3 = (string)cmbAltCode3.SelectedValue;
                    }

                    if (!string.IsNullOrEmpty(txtExpiry.Text))
                    {
                        newitem.EXPIRY = int.Parse(txtExpiry.Text);
                    }

                    if (cmbInner.SelectedIndex >= 0)
                    {
                        //changed MYAdamas 20171123 due to save innerlabelsetno in dropdown not id
                        // newitem.INNERLABELSET = Convert.ToString(cmbInner.SelectedValue);
                        newitem.INNERLABELSET = Convert.ToString(cmbInner.Text);
                    }

                    if (cmbPrinter.SelectedIndex >= 0)
                    {
                        newitem.INNERPRINTER = Convert.ToInt32(cmbPrinter.SelectedValue);
                    }

                    if (cmbInnerDateFormat.SelectedIndex >= 0)
                    {
                        newitem.INNERLABELSETDATEFORMAT = Convert.ToString(cmbInnerDateFormat.SelectedValue);
                    }

                    if (!string.IsNullOrEmpty(txtSpecialInnerCode.Text))
                    {
                        newitem.SPECIALINNERCHARACTER = txtSpecialInnerCode.Text;
                    }

                    if (cmbOuter.SelectedIndex >= 0)
                    {
                        //changed MYAdamas 20171123 due to save outerlabelsetno in dropdown not id
                        newitem.OUTERLABELSETNO = Convert.ToString(cmbOuter.Text);
                    }

                    if (cmbOuterDateFormat.SelectedIndex >= 0)
                    {
                        newitem.OUTERLABELSETDATEFORMAT = Convert.ToString(cmbOuterDateFormat.SelectedValue);
                    }

                    if (!string.IsNullOrEmpty(txtPreshipment.Text))
                    {
                        newitem.PRESHIPMENTPLAN = Convert.ToInt32(txtPreshipment.Text);
                    }

                    if (!string.IsNullOrEmpty(txtPalletQuantity.Text))
                    {
                        newitem.PALLETCAPACITY = Convert.ToInt32(txtPalletQuantity.Text);
                    }

                    if (!string.IsNullOrEmpty(txtNoofLabelToPrint.Text))
                    {
                        newitem.NOOFLABELPRINT = int.Parse(txtNoofLabelToPrint.Text);
                    }

                    newitem.ActionType = Constants.ActionLog.Update;

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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                dgvLineSelection.Rows.Insert(0, 1);
                dgvLineSelection.Rows[0].ReadOnly = false;

                dgvLineSelection["CustomerSize", 0].Value = string.Empty;

                DataGridViewComboBoxCell cmb = dgvLineSelection["cmbHartalegaSize", 0] as DataGridViewComboBoxCell;
                cmb.DataSource = _hartalegasize;
                cmb.ValueMember = "IDField";
                cmb.DisplayMember = "DisplayField";
                cmb.Value = Convert.ToString(_hartalegasize.FirstOrDefault().IDField);

                // set default value
                dgvLineSelection["BaseUnit", 0].Value = "pcs";
                dgvLineSelection["NoOfPiecesInBaseUnit", 0].Value = 1;

                _cell = dgvLineSelection.Rows[0];

                dgvLineSelection.Columns[19].Name = "Insert";
                dgvLineSelection[19, 0].Style.NullValue = "Insert";
                dgvLineSelection.Columns[20].Name = "Cancel";
                dgvLineSelection[20, 0].Style.NullValue = "Cancel";
                btnAdd.Enabled = false;
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "btnAdd_Click", null);
                return;
            }
        }

        private void cmbInner_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbInner.SelectedValue != null)
            {
                if (SetUpConfigurationBLL.GetInnerLabelCustomDate(Convert.ToString(cmbInner.SelectedValue)))
                {
                    cmbInnerDateFormat.Enabled = true;
                    cmbInnerDateFormat.SelectedValue = _brandMaster.INNERLABELSETDATEFORMAT;
                }
                else
                {
                    cmbInnerDateFormat.SelectedIndex = -1;
                    cmbInnerDateFormat.Enabled = false;
                }
            }
        }

        private void cmbOuter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbOuter.SelectedValue != null)
            {
                if (SetUpConfigurationBLL.GetOuterLabelCustomDate(Convert.ToString(cmbOuter.SelectedValue)))
                {
                    cmbOuterDateFormat.Enabled = true;
                    cmbOuterDateFormat.SelectedValue = _brandMaster.OUTERLABELSETDATEFORMAT;
                }
                else
                {
                    cmbOuterDateFormat.SelectedIndex = -1;
                    cmbOuterDateFormat.Enabled = false;
                }
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
            if (cmbInner.SelectedValue != null)
            {
                if (SetUpConfigurationBLL.GetInnerLabelCustomDate(Convert.ToString(cmbInner.SelectedValue)))
                {
                    cmbInnerDateFormat.Enabled = true;
                    cmbInnerDateFormat.SelectedValue = _brandMaster.INNERLABELSETDATEFORMAT;
                }
                else
                {
                    cmbInnerDateFormat.Enabled = false;
                }
            }
            else
            {
                cmbInnerDateFormat.Enabled = false;
            }

            txtExpiry.Text = Convert.ToString(_brandMaster.EXPIRY);
            if (_brandMaster.SPECIALINNERCODE == 1)
                cbSpecialInnerCode.Checked = true;
            else
                cbSpecialInnerCode.Checked = false;
            txtSpecialInnerCode.Text = _brandMaster.SPECIALINNERCHARACTER;
            cmbLotVeri.SelectedValue = Convert.ToString(_brandMaster.LOTVERIFICATION);
            cmbPrinter.SelectedValue = Convert.ToString(_brandMaster.INNERPRINTER);
            cmbOuter.Text = _brandMaster.OUTERLABELSETNO;
            if (cmbOuter.SelectedValue != null)
            {
                if (SetUpConfigurationBLL.GetOuterLabelCustomDate(Convert.ToString(cmbOuter.SelectedValue)))
                {
                    cmbOuterDateFormat.Enabled = true;
                    cmbOuterDateFormat.SelectedValue = _brandMaster.OUTERLABELSETDATEFORMAT;
                }
                else
                {
                    cmbOuterDateFormat.Enabled = false;
                }
            }
            else
            {
                cmbOuterDateFormat.Enabled = false;
            }

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

                //KahHeng 28Jan2019 added case for PODate (rb5) and POReceivedDate (rb6)
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
            cmbAltCode1.SelectedValue = _brandMaster.ALTERNATEGLOVECODE1;
            cmbAltCode2.SelectedValue = _brandMaster.ALTERNATEGLOVECODE2;
            cmbAltCode3.SelectedValue = _brandMaster.ALTERNATEGLOVECODE3;


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

            if (_brandMaster.NOOFLABELPRINT != null)
                txtNoofLabelToPrint.Text = Convert.ToString(_brandMaster.NOOFLABELPRINT);
        }

        private void getBrandLineDetails()
        {
            dgvLineSelection.Rows.Clear();
            List<BrandLineDTO> brandLineDTO = null;
            try
            {
                brandLineDTO = BrandMasterBLL.GetBrandLineList(_itemID);
                if (_brandMaster.ACTIVE == 0)
                {
                    btnAdd.Enabled = false;
                }
                else if (_brandMaster.ACTIVE == 1)
                {
                    btnAdd.Enabled = true;
                }
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

                    if (_brandMaster.ACTIVE == 0)
                    {
                        dgvLineSelection.Columns[19].Visible = false;
                        dgvLineSelection.Columns[20].Visible = false;
                    }
                    else if (_brandMaster.ACTIVE == 1)
                    {
                        dgvLineSelection.Columns[19].Name = "Edit";
                        dgvLineSelection.Columns[20].Name = "Delete";
                    }
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
