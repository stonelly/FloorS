using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
namespace Hartalega.FloorSystem.Windows.UI.ProductionDefect
{
    /// <summary>
    /// Production Defect List Screen
    /// </summary>
    public partial class ProductionDefectList : FormBase  
    {
        #region Private Class Members
        private int _locationId = Constants.ZERO;
        private decimal _serialNumber =Constants.ZERO;
        private string _productionLineNumber = string.Empty;
        private DateTime _productionDate = new DateTime();
        private string _defectTime = string.Empty;
        private string _gloveSize = string.Empty;
        private string _tierSide = string.Empty;
        private int _qaiDefectQty = Constants.ZERO;
        private int _pnDefectQty =Constants.ZERO;
        private int _productionDefectId =Constants.ZERO;
        private string _defectDetail = string.Empty;
        private int _defectDetailQty = Constants.ZERO;
        private string _defectDetailDescription = string.Empty;
        private int _id = Constants.ZERO;
        private int _defectCategoryId = Constants.ZERO;
        private string _workStationNumber = string.Empty;
        public const string _screenName = "ProductionDefectList";
        public const string _className = "ProductionDefectList";
        #endregion

        #region Constructors
        /// <summary>
        /// Intiantite the class
        /// </summary>
        public ProductionDefectList()
        {
            InitializeComponent();
            try
            {
                ClearForm();
                LoadProductionLineItems();
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, Name, Text);
                return;
            }
        }
        #endregion

        #region Event Handlers

        /// <summary>
        /// Clear form and load production line items on form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Maintainence_Load(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// Validate inputs in required fields and populate production defect summary grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGo_Click(object sender, EventArgs e)
        {
            string lineNumber = string.Empty;
            int productionDefectId = Constants.ZERO;
            string tierSide = string.Empty;
            decimal serialNumber = Constants.ZERO;
            string gloveSize = string.Empty;
            string defectTime = string.Empty;
            //int defectCategoryId = Constants.ZERO;
            try
            {
                if (ValidateRequiredFields())
                {
                    PopulateDefectSummaryGrid();
                    lineNumber = Convert.ToString(ddProductionLine.SelectedItem);
                    _workStationNumber = WorkStationDTO.GetInstance().WorkStationId;
                    if (grdDefectSummary.RowCount > Constants.ZERO)
                    {
                        for (int i = Constants.ZERO; i < grdDefectSummary.RowCount; i++)
                        {
                            if (Convert.ToInt32(grdDefectSummary[Constants.FOUR, i].Value) == Constants.ZERO && Convert.ToInt32(grdDefectSummary[Constants.SEVEN, i].Value) == Constants.ZERO)
                            {
                                productionDefectId = Convert.ToInt32(grdDefectSummary[Constants.SEVEN, i].Value);
                                tierSide = Convert.ToString(grdDefectSummary[Constants.THREE, i].Value);
                                serialNumber = Convert.ToDecimal(grdDefectSummary[Constants.SIX, i].Value);
                                gloveSize = Convert.ToString(grdDefectSummary[Constants.TWO, i].Value);
                                defectTime = Convert.ToString(grdDefectSummary[Constants.ONE, i].Value);
                                //defectCategoryId = Convert.ToInt32(grdDefectSummary[Constants.EIGHT, i].Value);
                                ProductionDefectBLL.UpdateProductionDefectDetails(productionDefectId, Constants.NODEFECT, Constants.ZERO, tierSide, serialNumber,
                                    lineNumber, gloveSize, Constants.ZERO, dtpProductionDate.Value, defectTime, _workStationNumber
                                    //, defectCategoryId
                                    );
                            }
                        }
                        PopulateDefectSummaryGrid();
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.NORECORDS_AVAILABLE_LEFT_GRID, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                        tsBtnEditDefect.Enabled = false;
                        tsBtnAddDefect.Enabled = false;
                    }
                }
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, btnGo.Name,null);
                return;
            }
        }

        /// <summary>
        /// populate prodeuction defect detail grid based on the defect selected in summary grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdDefectSummary_SelectionChanged(object sender, EventArgs e)
        {
            decimal serialNumber = Constants.ZERO;
            int pnDefectQty = Constants.ZERO;
            int productionDefectId = Constants.ZERO;
            //int defectCategoryId = Constants.ZERO;
            int qaiDefectQty = Constants.ZERO;
            try
            {
                if (grdDefectSummary.SelectedRows.Count > Constants.ZERO)
                {
                    serialNumber=Convert.ToDecimal(grdDefectSummary.SelectedRows[Constants.ZERO].Cells[Constants.SIX].Value);
                    pnDefectQty = Convert.ToInt32(grdDefectSummary.SelectedRows[Constants.ZERO].Cells[Constants.FIVE].Value);
                    productionDefectId = Convert.ToInt32(grdDefectSummary.SelectedRows[Constants.ZERO].Cells[Constants.SEVEN].Value);
                    //defectCategoryId = Convert.ToInt32(grdDefectSummary.SelectedRows[Constants.ZERO].Cells[Constants.EIGHT].Value);
                    qaiDefectQty = Convert.ToInt32(grdDefectSummary.SelectedRows[Constants.ZERO].Cells[Constants.FOUR].Value);
                    PopulateDefectDetailGrid(productionDefectId
                        //,defectCategoryId
                        );
                    if (qaiDefectQty==Constants.ZERO || (pnDefectQty == Constants.ZERO && grdDefectDetail.Rows.Count == Constants.ZERO))
                    {
                        tsBtnEditDefect.Enabled = false;
                    }
                    else
                    {
                        tsBtnEditDefect.Enabled = true;
                    }
                }
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, grdDefectSummary.Name, null);
                return;
            }
        }

        /// <summary>
        /// Open Add/Edit Defect Screen for adding new production defect details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsBtnAddDefect_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (grdDefectSummary.SelectedRows.Count > Constants.ZERO)
                {
                    _productionLineNumber = ddProductionLine.SelectedItem.ToString();
                    _productionDate = dtpProductionDate.Value;
                    _defectTime = grdDefectSummary.SelectedRows[Constants.ZERO].Cells[Constants.ONE].Value.ToString();
                    _gloveSize = grdDefectSummary.SelectedRows[Constants.ZERO].Cells[Constants.TWO].Value.ToString();
                    _tierSide = grdDefectSummary.SelectedRows[Constants.ZERO].Cells[Constants.THREE].Value.ToString();
                    _pnDefectQty = Convert.ToInt32(grdDefectSummary.SelectedRows[Constants.ZERO].Cells[Constants.FIVE].Value);
                    _qaiDefectQty = Convert.ToInt32(grdDefectSummary.SelectedRows[Constants.ZERO].Cells[Constants.FOUR].Value);
                    _serialNumber = Convert.ToDecimal(grdDefectSummary.SelectedRows[Constants.ZERO].Cells[Constants.SIX].Value);
                    _productionDefectId = Convert.ToInt32(grdDefectSummary.SelectedRows[Constants.ZERO].Cells[Constants.SEVEN].Value);
                    //_defectCategoryId = Convert.ToInt32(grdDefectSummary.SelectedRows[Constants.ZERO].Cells[Constants.EIGHT].Value);
                    if (!(_qaiDefectQty == Constants.ZERO && grdDefectDetail.Rows.Count > Constants.ZERO))
                    {
                        new ProductionDefect.AddEditDefect(_serialNumber, _productionLineNumber, _productionDate, _defectTime,
                            _gloveSize, _tierSide, _pnDefectQty, _qaiDefectQty,_productionDefectId
                            //,_defectCategoryId
                            ,this).ShowDialog();
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.NODEFECTDETAIL_ALREADY_EXIST, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    }
                }
                else
                {
                    GlobalMessageBox.Show(Messages.PLEASE_SELECT_A_RECORD_FROM_SUMMARY, Constants.AlertType.Warning, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);
                }
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, tsBtnAddDefect.Name, null);
                return;
            }
        }
        /// <summary>
        /// Open Add/Edit Defect Screen for editing production defect details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsBtnEditDefect_Click(object sender, EventArgs e)
        {
            OpenEditDefectScreen();
        }

        /// <summary>
        /// Close the form , when user presses ESC key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProductionDefectList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }
        /// <summary>
        /// set the date format 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtpProductionDate_ValueChanged(object sender, EventArgs e)
        {
            dtpProductionDate.CustomFormat = "dd - MMM - yyyy";
            ClearResult();
        }
        /// <summary>
        /// Open the edit screen from production Defect detail
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdDefectDetail_DoubleClick(object sender, EventArgs e)
        {
            OpenEditDefectScreen();
        }

        private void ddProductionLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearResult();
        }
        
        #endregion

        #region User Methods
        /// <summary>
        /// Open Edit defect screen
        /// </summary>
        private void OpenEditDefectScreen()
        {
            try
            {
                if (grdDefectDetail.SelectedRows.Count > Constants.ZERO)
                {
                    _serialNumber = Convert.ToDecimal(grdDefectSummary.SelectedRows[Constants.ZERO].Cells[Constants.SIX].Value);
                    _pnDefectQty = Convert.ToInt32(grdDefectSummary.SelectedRows[Constants.ZERO].Cells[Constants.FIVE].Value);
                    _qaiDefectQty = Convert.ToInt32(grdDefectSummary.SelectedRows[Constants.ZERO].Cells[Constants.FOUR].Value);
                    _defectDetail = grdDefectDetail.SelectedRows[Constants.ZERO].Cells[Constants.ZERO].Value.ToString();
                    _defectDetailDescription = grdDefectDetail.SelectedRows[Constants.ZERO].Cells[Constants.ONE].Value.ToString();
                    _defectDetailQty = Convert.ToInt32(grdDefectDetail.SelectedRows[Constants.ZERO].Cells[Constants.TWO].Value);
                    _productionDefectId = Convert.ToInt32(grdDefectDetail.SelectedRows[Constants.ZERO].Cells[Constants.THREE].Value);
                    _id = Convert.ToInt32(grdDefectDetail.SelectedRows[Constants.ZERO].Cells[Constants.FOUR].Value);
                    //_defectCategoryId = Convert.ToInt32(grdDefectSummary.SelectedRows[Constants.ZERO].Cells[Constants.EIGHT].Value);
                    new ProductionDefect.AddEditDefect(_serialNumber, _productionDefectId, _defectDetail, _defectDetailDescription,
                        _defectDetailQty, _pnDefectQty, _qaiDefectQty, _id
                        //,_defectCategoryId
                        , this).ShowDialog();
                }
                else
                {
                    GlobalMessageBox.Show(Messages.PLEASE_SELECT_A_RECORD_FROM_DETAIL, Constants.AlertType.Warning, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                }
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, tsBtnAddDefect.Name, null);
                return;
            }
        }
        /// <summary>
        /// Validate inputs in the required fields for fetching production defect summary records
        /// </summary>
        /// <returns></returns>
        private bool ValidateRequiredFields()
        {
            bool status = false;
            string requiredFieldMessage = Messages.REQUIREDFIELDMESSAGE;
            if (ddProductionLine.SelectedIndex == Constants.MINUSONE)
            {
                requiredFieldMessage = requiredFieldMessage + Constants.LINENUMBER + Environment.NewLine;
            }
            if(dtpProductionDate.Text.Equals(" "))
            {
                requiredFieldMessage = requiredFieldMessage + Constants.PRODUCTIONDATE + Environment.NewLine;
            }
            if (requiredFieldMessage.Equals(Messages.REQUIREDFIELDMESSAGE))
            {
                if (dtpProductionDate.Value.Date > CommonBLL.GetCurrentDateAndTime().Date)
                {
                    GlobalMessageBox.Show(Messages.DATE_GREATERTHAN_TODAY, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                    ddProductionLine.Focus();
                    ClearForm();
                    LoadProductionLineItems();
                    dtpProductionDate.CustomFormat = " ";
                    status = false;
                }
                else
                {
                    status = true;
                }
            }
            else
            {
                if (requiredFieldMessage != Messages.REQUIREDFIELDMESSAGE)
                {
                    GlobalMessageBox.Show(requiredFieldMessage, Constants.AlertType.Information, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);
                    if (ddProductionLine.SelectedIndex == Constants.MINUSONE)
                    {
                        ddProductionLine.Focus();
                    }
                    else
                    {
                        dtpProductionDate.Focus();
                    }
                }
                status = false;
            }
            return status;
        }

        /// <summary>
        /// Clear data from all the controls of form
        /// </summary>
        private void ClearForm()
        {
            ddProductionLine.Items.Clear();
            grdDefectSummary.Rows.Clear();
            grdDefectDetail.Rows.Clear();
            CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
            ddProductionLine.Focus();
            tsBtnAddDefect.Enabled = false;
            tsBtnEditDefect.Enabled = false;
        }

        /// <summary>
        /// Load production line items
        /// </summary>
        private void LoadProductionLineItems()
        {
            List<LineDTO> lineList = null;
            _locationId = WorkStationDTO.GetInstance().LocationId;
            lineList = ProductionDefectBLL.GetLineList(_locationId);
            if (lineList != null)
            {
                foreach (LineDTO line in lineList)
                {
                    ddProductionLine.Items.Add(line.LineNumber);
                }
            }
        }

        /// <summary>
        /// Populate data in the production summary grid
        /// </summary>
        public void PopulateDefectSummaryGrid()
        {
            Constants.ProductionDefectStatus defectStatus;
            List<ProductionDefectSummaryDTO> productionDefectList = null;
            string lineNumber = string.Empty;
            DateTime qaiDefectDate = new DateTime();
            grdDefectSummary.Rows.Clear();
            lineNumber = ddProductionLine.SelectedItem.ToString();
            qaiDefectDate = dtpProductionDate.Value;
            productionDefectList = ProductionDefectBLL.GetDefectSummaryList(lineNumber, qaiDefectDate);
            grdDefectDetail.Rows.Clear();
            if (productionDefectList != null)
            {
                for (int i = Constants.ZERO; i < productionDefectList.Count; i++)
                {
                    defectStatus = (Constants.ProductionDefectStatus)Enum.Parse(typeof(Constants.ProductionDefectStatus), productionDefectList[i].Status);
                    grdDefectSummary.Rows.Add();
                    switch (defectStatus)
                    {
                        case Constants.ProductionDefectStatus.GREEN:
                            grdDefectSummary[Constants.ZERO, i].Value = Properties.Resources.Green_New;
                            break;
                        case Constants.ProductionDefectStatus.YELLOW:
                            grdDefectSummary[Constants.ZERO, i].Value = Properties.Resources.Yellow_New;
                            break;
                        case Constants.ProductionDefectStatus.RED:
                            grdDefectSummary[Constants.ZERO, i].Value = Properties.Resources.Red_New;
                            break;
                    }
                    grdDefectSummary[Constants.ONE, i].Value = productionDefectList[i].DefectTime;
                    grdDefectSummary[Constants.TWO, i].Value = productionDefectList[i].Size;
                    grdDefectSummary[Constants.THREE, i].Value = productionDefectList[i].TierSide;
                    grdDefectSummary[Constants.FIVE, i].Value = String.Format(Constants.NUMBER_FORMAT,productionDefectList[i].PNDefectQuantity);
                    grdDefectSummary[Constants.FOUR, i].Value = String.Format(Constants.NUMBER_FORMAT,productionDefectList[i].QAIDefectQuantity);
                    grdDefectSummary[Constants.SIX, i].Value = productionDefectList[i].SerialNumber;
                    grdDefectSummary[Constants.SEVEN, i].Value = productionDefectList[i].ProductionDefectId;
                    //grdDefectSummary[Constants.EIGHT, i].Value = productionDefectList[i].DefectCategoryId;
                }
                grdDefectSummary.Columns[Constants.SEVEN].Visible = false;
                grdDefectSummary.ClearSelection();
                tsBtnEditDefect.Enabled = true;
                tsBtnAddDefect.Enabled = true;
            }
            CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
        }

        /// <summary>
        /// Select the row in the production defect summary grid
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="defectCategoryId"></param>
        public void SelectRowInDefectSummaryGrid(decimal serialNumber
            //,int defectCategoryId
            )
        {
            for (int i = Constants.ZERO; i < grdDefectSummary.RowCount; i++)
            {
                if (Convert.ToDecimal(grdDefectSummary[Constants.SIX, i].Value) == serialNumber 
                    //&& Convert.ToInt32(grdDefectSummary[Constants.EIGHT, i].Value) == defectCategoryId
                    )
                {
                    grdDefectSummary.Rows[i].Selected = true;
                }
            }
        }

        /// <summary>
        /// Select the row in the production defect summary grid after edit
        /// </summary>
        /// <param name="productionDefectId"></param>
        /// <param name="defectCategoryId"></param>
        public void SelectRowInDefectSummaryGridForEdit(int productionDefectId
            //, int defectCategoryId
         )
        {
            for (int i = Constants.ZERO; i < grdDefectSummary.RowCount; i++)
            {
                if (Convert.ToInt32(grdDefectSummary[Constants.SEVEN, i].Value) == productionDefectId 
                    //&& Convert.ToInt32(grdDefectSummary[Constants.EIGHT, i].Value) == defectCategoryId
                    )
                {
                    grdDefectSummary.Rows[i].Selected = true;
                }
            }
        }

        /// <summary>
        /// populate data in the production defect detail grid
        /// </summary>
        /// <param name="serialNumber"></param>
        public void PopulateDefectDetailGrid(int productionDefectId
            //,int defectCategoryId
            )
        {
            grdDefectDetail.Rows.Clear();
            List<ProductionDefectDetailDTO> productionDefectDetailList = null;
            productionDefectDetailList = ProductionDefectBLL.GetDefectDetailList(productionDefectId
                //,defectCategoryId
                );
            if (productionDefectDetailList != null)
            {
                for (int i = Constants.ZERO; i < productionDefectDetailList.Count; i++)
                {
                    grdDefectDetail.Rows.Add();
                    grdDefectDetail[Constants.ZERO, i].Value = productionDefectDetailList[i].DefectType;
                    grdDefectDetail[Constants.ONE, i].Value = productionDefectDetailList[i].DefectDescription;
                    grdDefectDetail[Constants.TWO, i].Value = String.Format(Constants.NUMBER_FORMAT,productionDefectDetailList[i].DefectQuantity);
                    grdDefectDetail[Constants.THREE, i].Value = productionDefectDetailList[i].ProductionDefectId;
                    grdDefectDetail[Constants.FOUR, i].Value = productionDefectDetailList[i].Id;
                }
            }
            grdDefectDetail.ClearSelection();
            grdDefectDetail.Columns[Constants.THREE].Visible = false;
            grdDefectDetail.Columns[Constants.FOUR].Visible = false;
        }

        /// <summary>
        /// To call exception log method
        /// </summary>
        /// <param name="floorException">FloorException object </param>
        /// <param name="screenName">ScreenName</param>
        /// <param name="uiClassName">UIClassName</param>
        /// <param name="uiControl">UIControl</param>
        /// <param name="parameters">Parameters</param>
        private void ExceptionLogging(FloorSystemException floorException, string screenName, string uiClassName, string uiControl, params object[] parameters)
        {
            int result = CommonBLL.LogExceptionToDB(floorException, screenName, uiClassName, uiControl, parameters);
            if (floorException.subSystem == Constants.BUSINESSLOGIC)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.APPLICATIONERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            ClearForm();
        }

        private void ClearResult()
        {
            grdDefectSummary.Rows.Clear();
            grdDefectDetail.Rows.Clear();
            tsBtnEditDefect.Enabled = false;
            tsBtnAddDefect.Enabled = false;
        }
        #endregion

    }
}
