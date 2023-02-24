using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Hartalega.FloorSystem.Windows.UI.Common;

namespace Hartalega.FloorSystem.Windows.UI.SetUpConfiguration
{
    public partial class EditGloveType : FormBase
    {
        #region Member Variables
        DataGridViewComboBoxCell _comboCellCommonSize;
        private string _screenName = "Configuration SetUp - GloveType";
        private string _className = "GloveTypeMaster";
        private List<DropdownDTO> _sizemasterlist;
        private DataGridViewRow _cell = null;
        private DataTable _dtTableDetails;
        private bool _refresh = false;
        private string _loggedInUser;
        private int _glovetypeid;
        private PageOffsetList _pg;
        #endregion

        #region Constructor

        public EditGloveType(string loggedInUser, int GloveTypeID)
        {
            InitializeComponent();
            _loggedInUser = loggedInUser;
            _glovetypeid = GloveTypeID;

        }
        #endregion


        #region User Methods

        private void PopulateComboBoxes(int rowIndex)
        {
            try
            {
                _comboCellCommonSize = gvSize["CommonSize", rowIndex] as DataGridViewComboBoxCell;
                _comboCellCommonSize.DataSource = new BindingSource(_sizemasterlist, null);
                _comboCellCommonSize.ValueMember = "IDField";
                _comboCellCommonSize.DisplayMember = "DisplayField";

            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "PopulateComboBoxes", null);
                return;
            }
        }


        private void BindItem()
        {
            GloveTypeMasterDTO gloveMaster = new GloveTypeMasterDTO();

            try
            {
                gloveMaster = SetUpConfigurationBLL.GetGloveTypeDetails(_glovetypeid);
                cmbGloveCategory.BindComboBox(SetUpConfigurationBLL.GetGloveCategory(), true);
                cmbPowderFormula.BindComboBox(SetUpConfigurationBLL.GetEnumMaster("PowderFormula"), true);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindItem", null);
            }
            if (gloveMaster != null)
            {
                txtBarcode.Text = gloveMaster.BARCODE;
                txtGloveCode.Text = gloveMaster.GLOVECODE;
                txtDescription.Text = gloveMaster.DESCRIPTION;
                cmbGloveCategory.SelectedValue = gloveMaster.GLOVECATEGORY;
                cbProtein.Checked = gloveMaster.PROTEIN == 1 ? true : false;
                // if protein checked only bind protein spec
                if (gloveMaster.PROTEIN == 1)
                {
                    txtProteinSpec.Text = Convert.ToString(gloveMaster.PROTEINSPEC);
                    txtProteinSpec.Enabled = true;
                }

                cbPowder.Checked = gloveMaster.POWDER == 1 ? true : false;
                //if powder checked only bind powder formula
                if (gloveMaster.POWDER == 1)
                {
                    cmbPowderFormula.SelectedValue = Convert.ToString(gloveMaster.POWDERFORMULA);
                    cmbPowderFormula.Enabled = true;
                }
              
                cbHotbox.Checked = gloveMaster.HOTBOX == 1 ? true : false;
                cbPolymer.Checked = gloveMaster.POLYMER == 1 ? true : false;

            }
        }

        private void PopulateGrid()
        {
            try
            {
                _sizemasterlist = MasterTableBLL.GetGloveSizeMasterList();
                _dtTableDetails = SetUpConfigurationBLL.GetGloveCommonSizeByGloveType(_glovetypeid);
                FillGrid(_dtTableDetails);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "PopulateGrid", null);
                return;
            }
            //   _pg = new PageOffsetList(Constants.TWENTY, _dtTableDetails.Rows.Count);
            if (_dtTableDetails.Rows.Count > 0)
            {
                // bindingNavigatorTable.BindingSource = bindingSourceTable;
                //   bindingSourceTable.CurrentChanged += new System.EventHandler(bindingSource_CurrentChanged);
                //  bindingSourceTable.DataSource = _pg.GetList();

                if (_refresh)
                {
                    gvSize.Columns[5].Name = "Edit";
                    gvSize.Columns[6].Name = "Delete";
                }
            }
        }

        private void FillGrid(DataTable dtTable)
        {
            btnAddSize.Enabled = true;
            //bindingNavigatorTable.Enabled = true;
            gvSize.Rows.Clear();
            if (dtTable != null)
            {
                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    gvSize.Rows.Add();
                    gvSize.Rows[i].ReadOnly = true;
                    gvSize["AVAGLOVERELCOMSIZE_ID", i].Value = dtTable.Rows[i]["AVAGLOVERELCOMSIZE_ID"];
                    PopulateComboBoxes(i);

                    List<DropdownDTO> SizeList = _sizemasterlist;

                    for (int j = 0; j < SizeList.Count; j++)
                    {
                        if (SizeList[j].IDField.ToString().ToUpper() == dtTable.Rows[i]["CommonSize"].ToString().ToUpper())
                        {
                            gvSize["CommonSize", i].Value = dtTable.Rows[i]["CommonSize"];
                            break;
                        }
                    }
                    
                    gvSize["GloveWeight", i].Value = dtTable.Rows[i]["GloveWeight"];
                    gvSize["MAX10PCSWT", i].Value = dtTable.Rows[i]["MAX10PCSWT"];
                    gvSize["MIN10PCSWT", i].Value = dtTable.Rows[i]["MIN10PCSWT"];
                }
            }
            else
            {
                GlobalMessageBox.Show(Messages.NO_DATA_FOUND, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                gvSize.DataSource = null;
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

        #region EventHandlers
        private void btnAddSize_Click(object sender, EventArgs e)
        {
            gvSize.Rows.Insert(0, 1);
            gvSize.Rows[0].ReadOnly = false;
            PopulateComboBoxes(Constants.ZERO);
            gvSize["GloveWeight", 0].Value = "0.00";

            gvSize["MAX10PCSWT", 0].Value = "0.00";

            gvSize["MIN10PCSWT", 0].Value = "0.00";

            _cell = gvSize.Rows[0];
            gvSize.Columns[5].Name = "Insert";
            gvSize[5, 0].Style.NullValue = "Insert";
            gvSize.Columns[6].Name = "Cancel";
            gvSize[6, 0].Style.NullValue = "Cancel";
            btnAddSize.Enabled = false;
        }

        private void EditGloveType_Load(object sender, EventArgs e)
        {
            try
            {
                bindingSourceTable.CurrentChanged -= new System.EventHandler(bindingSource_CurrentChanged);
                gvSize.CellContentClick -= new System.Windows.Forms.DataGridViewCellEventHandler(gvSize_CellContentClick);
                _refresh = false;
                PopulateGrid();
                bindingSourceTable.CurrentChanged += new System.EventHandler(bindingSource_CurrentChanged);
                gvSize.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(gvSize_CellContentClick);
                BindItem();

            }

            catch (Exception ex)
            {

            }
        }
        private void bindingSource_CurrentChanged(object sender, EventArgs e)
        {
            DataTable dtTableDetails = new DataTable();
            dtTableDetails = _dtTableDetails.Clone();
            int offset = (int)bindingSourceTable.Current;
            for (int i = offset; i < offset + _pg.pageSize && i < _pg.TotalRecords; i++)
            {
                DataRow newRow = dtTableDetails.NewRow();
                dtTableDetails.ImportRow(_dtTableDetails.Rows[i]);
            }
            FillGrid(dtTableDetails);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int rowReturned = 0;

            GloveTypeMasterDTO newitem = new GloveTypeMasterDTO();

            try
            {
                string vldmsg = string.Empty;

                if (string.IsNullOrEmpty(txtBarcode.Text))
                    vldmsg += "\nBarcode";
                if (string.IsNullOrEmpty(txtGloveCode.Text))
                    vldmsg += "\nGlove Code";
                if (string.IsNullOrEmpty(txtDescription.Text))
                    vldmsg += "\nDescription";
                if (string.IsNullOrEmpty(Convert.ToString(cmbGloveCategory.SelectedValue)))
                    vldmsg += "\nGlove Category";
                if (cbProtein.Checked)
                    if (string.IsNullOrEmpty(Convert.ToString(txtProteinSpec.Text)))
                        vldmsg += "\nProtein Specification";
                if (cbPowder.Checked)
                    if (string.IsNullOrEmpty(Convert.ToString(cmbPowderFormula.SelectedValue)))
                        vldmsg += "\nPowder Formula";

                newitem.BARCODE = txtBarcode.Text;
                newitem.DESCRIPTION = txtDescription.Text;
                newitem.GLOVECATEGORY = Convert.ToString(cmbGloveCategory.SelectedValue);
                newitem.GLOVECODE = txtGloveCode.Text;
                newitem.HOTBOX = cbHotbox.Checked ? 1 : 0;
                newitem.POLYMER = cbPolymer.Checked ? 1 : 0;
                newitem.POWDER = cbPowder.Checked ? 1 : 0;
                newitem.POWDERFORMULA = Convert.ToInt32(cmbPowderFormula.SelectedValue);
                newitem.PROTEIN = cbProtein.Checked ? 1 : 0;
                newitem.PROTEINSPEC = string.IsNullOrEmpty(txtProteinSpec.Text) ? decimal.Zero : Convert.ToDecimal(txtProteinSpec.Text);
                newitem.STOPPED = 0;
                newitem.ActionType = Constants.ActionLog.Update;

                GloveTypeMasterDTO olditem = new GloveTypeMasterDTO();
                olditem = SetUpConfigurationBLL.GetGloveTypeDetails(_glovetypeid);
                newitem.AVAGLOVECODETABLE_ID = olditem.AVAGLOVECODETABLE_ID;

                if (string.IsNullOrEmpty(vldmsg))
                {
                    int isDuplicate = 0;
                    isDuplicate = Convert.ToInt16(SetUpConfigurationBLL.IsGloveTypeMasterDuplicate(newitem.AVAGLOVECODETABLE_ID, txtBarcode.Text, txtGloveCode.Text));
                    if (isDuplicate == 0 )
                    {
                        rowReturned = SetUpConfigurationBLL.AddGloveType(olditem, newitem, _loggedInUser);
                        if (rowReturned > 0)
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
                        GlobalMessageBox.Show(Messages.DUPLICATE_GLOVETYPEMASTER_GloveCode, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
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

        private void gvSize_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (gvSize.Columns[e.ColumnIndex].Name == "Update")
                {
                    string invaliddata = string.Empty;
                    
                    string vldmsg = string.Empty;
                    if (gvSize.Rows[e.RowIndex].Cells["GloveWeight"].Value.ToString() == "0.00" || string.IsNullOrEmpty(gvSize.Rows[e.RowIndex].Cells["GloveWeight"].Value.ToString()))
                        vldmsg += "\nGlove Weight";
                    if (gvSize.Rows[e.RowIndex].Cells["MAX10PCSWT"].Value.ToString() == "0.00" || string.IsNullOrEmpty(gvSize.Rows[e.RowIndex].Cells["MAX10PCSWT"].Value.ToString()))
                        vldmsg += "\nMax 10 Pcs Weight";
                    if (gvSize.Rows[e.RowIndex].Cells["MIN10PCSWT"].Value.ToString() == "0.00" || string.IsNullOrEmpty(gvSize.Rows[e.RowIndex].Cells["MIN10PCSWT"].Value.ToString()))
                        vldmsg += "\nMin 10 Pcs Weight";

                    if (string.IsNullOrEmpty(vldmsg))
                    {
                        decimal decima;
                        if (!Decimal.TryParse(Convert.ToString(gvSize.Rows[e.RowIndex].Cells["GloveWeight"].Value.ToString()), out decima))
                        {
                            invaliddata += "\nGlove Weight";
                        }
                        if (!Decimal.TryParse(Convert.ToString(gvSize.Rows[e.RowIndex].Cells["MAX10PCSWT"].Value.ToString()), out decima))
                        {
                            invaliddata += "\nMax 10 Pcs Weight";
                        }

                        if (!Decimal.TryParse(Convert.ToString(gvSize.Rows[e.RowIndex].Cells["MIN10PCSWT"].Value.ToString()), out decima))
                        {
                            invaliddata += "\nMin 10 Pcs Weight";
                        }

                        if (invaliddata != String.Empty)
                        {
                            GlobalMessageBox.Show(Messages.INVALIDDATA + invaliddata, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        }
                        else
                        {
                            GloveTypeSizeRelationMasterDTO pdmNew = new GloveTypeSizeRelationMasterDTO();
                            pdmNew.COMMONSIZE = Convert.ToString(gvSize.Rows[e.RowIndex].Cells["CommonSize"].Value);
                            pdmNew.GLOVEWEIGHT = Convert.ToDecimal(gvSize.Rows[e.RowIndex].Cells["GloveWeight"].Value);
                            pdmNew.MAX10PCSWT = Convert.ToDecimal(gvSize.Rows[e.RowIndex].Cells["MAX10PCSWT"].Value);
                            pdmNew.MIN10PCSWT = Convert.ToDecimal(gvSize.Rows[e.RowIndex].Cells["MIN10PCSWT"].Value);
                            pdmNew.AVAGLOVERELCOMSIZE_ID = Convert.ToInt32(gvSize.Rows[e.RowIndex].Cells["AVAGLOVERELCOMSIZE_ID"].Value);
                            pdmNew.ActionType = Constants.ActionLog.Update;
                            pdmNew.GLOVETYPEID = _glovetypeid;

                            if (pdmNew.MIN10PCSWT > pdmNew.MAX10PCSWT)
                            {
                                GlobalMessageBox.Show(Messages.INVALIDWEIGHT_GLOVETYPEMASTER, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                return;
                            }
                            else
                            {

                                GloveTypeSizeRelationMasterDTO pdmold = SetUpConfigurationBLL.GetGloveCommonSizeGloveTypeMaster(_glovetypeid).Where(p => p.AVAGLOVERELCOMSIZE_ID == pdmNew.AVAGLOVERELCOMSIZE_ID).FirstOrDefault();
                                SetUpConfigurationBLL.SaveGloveCommonSize(pdmold, pdmNew, _loggedInUser);
                                GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                _refresh = true;
                                PopulateGrid();

                                return;
                            }

                        }
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.REQUIREDFIELDMESSAGE + Environment.NewLine + vldmsg, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                        return;
                    }
                }
                else if (gvSize.Columns[e.ColumnIndex].Name == "Edit")
                {
                    btnAddSize.Enabled = false;
                    gvSize.Rows[e.RowIndex].ReadOnly = false;

                    gvSize.Rows[e.RowIndex].Cells["CommonSize"].ReadOnly = true;

                    gvSize.Columns[e.ColumnIndex].Name = "Update";
                    gvSize[e.ColumnIndex, e.RowIndex].Style.NullValue = "Update";
                    gvSize.Columns[e.ColumnIndex + 1].Name = "Cancel";
                    gvSize[e.ColumnIndex + 1, e.RowIndex].Style.NullValue = "Cancel";
                    _cell = gvSize.CurrentRow;
                }
                else if (gvSize.Columns[e.ColumnIndex].Name == "Cancel" && gvSize.CurrentRow == _cell)
                {
                    _refresh = true;
                    PopulateGrid();
                }
                else if (gvSize.Columns[e.ColumnIndex].Name == "Delete")
                {
                    btnAddSize.Enabled = false;
                    if (GlobalMessageBox.Show(Messages.DELETE_MESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                    {
                        GloveTypeSizeRelationMasterDTO pdmNew = new GloveTypeSizeRelationMasterDTO();
                        pdmNew.AVAGLOVERELCOMSIZE_ID = Convert.ToInt32(gvSize.Rows[e.RowIndex].Cells["AVAGLOVERELCOMSIZE_ID"].Value);
                        pdmNew.ActionType = Constants.ActionLog.Delete;
                        pdmNew.Stopped = 1;
                        pdmNew.COMMONSIZE = Convert.ToString(gvSize.Rows[e.RowIndex].Cells["CommonSize"].Value);
                        pdmNew.GLOVEWEIGHT = Convert.ToDecimal(gvSize.Rows[e.RowIndex].Cells["GloveWeight"].Value);
                        pdmNew.MAX10PCSWT = Convert.ToDecimal(gvSize.Rows[e.RowIndex].Cells["MAX10PCSWT"].Value);
                        pdmNew.MIN10PCSWT = Convert.ToDecimal(gvSize.Rows[e.RowIndex].Cells["MIN10PCSWT"].Value);
                        pdmNew.GLOVETYPEID = _glovetypeid;
                        GloveTypeSizeRelationMasterDTO pdmold = SetUpConfigurationBLL.GetGloveCommonSizeGloveTypeMaster(_glovetypeid).Where(p => p.AVAGLOVERELCOMSIZE_ID == pdmNew.AVAGLOVERELCOMSIZE_ID).FirstOrDefault();
                        SetUpConfigurationBLL.SaveGloveCommonSize(pdmold, pdmNew, _loggedInUser);

                        PopulateGrid();
                    }
                    else
                    {
                        btnAddSize.Enabled = true; // if user choose no confirm
                    }
                }
                else if (gvSize.Columns[e.ColumnIndex].Name == "Insert")
                {
                    GloveTypeSizeRelationMasterDTO pdmNew = new GloveTypeSizeRelationMasterDTO();
                    pdmNew.COMMONSIZE = Convert.ToString(gvSize.Rows[e.RowIndex].Cells["CommonSize"].Value);

                    string invaliddata = string.Empty;
                  
                    string vldmsg = string.Empty;
                    if (string.IsNullOrEmpty(pdmNew.COMMONSIZE))
                        vldmsg += "\nHartalega Common Size";
                    if (gvSize.Rows[e.RowIndex].Cells["GloveWeight"].Value.ToString() == "0.00" ||string.IsNullOrEmpty(gvSize.Rows[e.RowIndex].Cells["GloveWeight"].Value.ToString()))
                        vldmsg += "\nGlove Weight";
                    if (gvSize.Rows[e.RowIndex].Cells["MAX10PCSWT"].Value.ToString() == "0.00" || string.IsNullOrEmpty(gvSize.Rows[e.RowIndex].Cells["MAX10PCSWT"].Value.ToString()))
                        vldmsg += "\nMax 10 Pcs Weight";
                    if (gvSize.Rows[e.RowIndex].Cells["MIN10PCSWT"].Value.ToString() == "0.00"|| string.IsNullOrEmpty(gvSize.Rows[e.RowIndex].Cells["MIN10PCSWT"].Value.ToString()))
                        vldmsg += "\nMin 10 Pcs Weight";
                   
                    if (string.IsNullOrEmpty(vldmsg))
                    {

                        decimal decima;
                        if (!Decimal.TryParse(Convert.ToString(gvSize.Rows[e.RowIndex].Cells["GloveWeight"].Value), out decima))
                        {
                            invaliddata += "\nGlove Weight";
                        }
                        if (!Decimal.TryParse(Convert.ToString(gvSize.Rows[e.RowIndex].Cells["MAX10PCSWT"].Value.ToString()), out decima))
                        {
                            invaliddata += "\nMax 10 Pcs Weight";
                        }

                        if (!Decimal.TryParse(Convert.ToString(gvSize.Rows[e.RowIndex].Cells["MIN10PCSWT"].Value.ToString()), out decima))
                        {
                            invaliddata += "\nMin 10 Pcs Weight";
                        }

                        if (invaliddata != String.Empty)
                        {
                            GlobalMessageBox.Show(Messages.INVALIDDATA + invaliddata, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        }
                        else
                        {
                            pdmNew.GLOVEWEIGHT = Convert.ToDecimal(gvSize.Rows[e.RowIndex].Cells["GloveWeight"].Value);
                            pdmNew.MAX10PCSWT = Convert.ToDecimal(gvSize.Rows[e.RowIndex].Cells["MAX10PCSWT"].Value);
                            pdmNew.MIN10PCSWT = Convert.ToDecimal(gvSize.Rows[e.RowIndex].Cells["MIN10PCSWT"].Value);
                            pdmNew.GLOVETYPEID = _glovetypeid;
                            pdmNew.ActionType = Constants.ActionLog.Add;
                            pdmNew.Stopped = 0;

                            GloveTypeSizeRelationMasterDTO pdmold = new GloveTypeSizeRelationMasterDTO();
                            pdmold.Stopped = 0;

                            bool isDuplicate = false;

                            isDuplicate = Convert.ToBoolean(SetUpConfigurationBLL.IsGloveTypeCommonSizeDuplicate(pdmNew.COMMONSIZE, _glovetypeid));
                            if (!isDuplicate)
                            {

                                if (pdmNew.MIN10PCSWT > pdmNew.MAX10PCSWT)
                                {
                                    GlobalMessageBox.Show(Messages.INVALIDWEIGHT_GLOVETYPEMASTER, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                                    return;
                                }
                                else
                                {
                                    SetUpConfigurationBLL.SaveGloveCommonSize(pdmold, pdmNew, _loggedInUser);
                                    GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);

                                    _refresh = true;
                                    PopulateGrid();
                                }
                            }
                            else
                            {
                                GlobalMessageBox.Show(Messages.DUPLICATE_GLOVETYPECOMMONSIZEMASTER, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            }
                        }
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.REQUIREDFIELDMESSAGE + Environment.NewLine + vldmsg, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                        return;
                    }
                    btnAddSize.Enabled = true;


                    return;
                }

            }
            catch (Exception ex)
            {
                FloorSystemException fex = new FloorSystemException(ex.Message, ex.Message, ex, true);
                ExceptionLogging(fex, _screenName, _className, "gvSize_CellContentClick", null);
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

        private void EditGloveType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                e.SuppressKeyPress = true;

        }

        /// <summary>
        /// Event Handler for validating Protein Spec
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtProteinSpec_Leave(object sender, EventArgs e)
        {
            try
            {
                // Validation for Protein Spec
                if (!string.IsNullOrEmpty(txtProteinSpec.Text.Trim()))
                {
                    decimal decima;
                    if (!Decimal.TryParse(Convert.ToString(txtProteinSpec.Text.Trim()), out decima))
                    {
                        GlobalMessageBox.Show(Messages.INVALID_VALUE_FOR_PROTEINSPEC, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        txtProteinSpec.Text = String.Empty;
                        txtProteinSpec.Focus();
                    }
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "txtProteinSpec_Leave", null);
                return;
            }
        }

        private void cbProtein_CheckedChanged(object sender, EventArgs e)
        {
            if (cbProtein.Checked)
                txtProteinSpec.Enabled = true;
            else
            {
                txtProteinSpec.Enabled = false;
                txtProteinSpec.Text = string.Empty;
            }
        }

        private void cbPowder_CheckedChanged(object sender, EventArgs e)
        {
            if (cbPowder.Checked)
                cmbPowderFormula.Enabled = true;
            else
            {
                cmbPowderFormula.Enabled = false;
                cmbPowderFormula.SelectedIndex = -1;
            }
        }


        #endregion

    }
}
