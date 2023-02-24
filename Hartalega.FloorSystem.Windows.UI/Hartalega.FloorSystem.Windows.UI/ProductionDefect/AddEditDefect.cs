using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.ProductionDefect
{
    public partial class AddEditDefect : FormBase
    {
        #region Private Class Members
        private bool _isCalledForAdd = true;
        private decimal _serialNumber = Constants.ZERO;
        private string _productionLineNumber = string.Empty;
        private DateTime _productionDate = new DateTime();
        private string _defectTime = string.Empty;
        private string _gloveSize = string.Empty;
        private string _tierSide = string.Empty;
        private int _qaiDefectQty = Constants.ZERO;
        private int _pnDefectQty = Constants.ZERO;
        private ProductionDefectList _pdl = new ProductionDefectList();
        private int _productionDefectId = Constants.ZERO;
        private string _defectDetail = string.Empty;
        private string _defectDetailDescription = string.Empty;
        private int _defectDetailQty = Constants.ZERO;
        private int _Id = Constants.ZERO;
        private int _defectCategoryId = Constants.ZERO;
        private string _workStationNumber = string.Empty;
        public const string _screenName = "AddEditDefect";
        public const string _className = "AddEditDefect";
        #endregion

        #region Constructors

        /// <summary>
        /// Initantiate the class
        /// </summary>
        public AddEditDefect()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Instantiate the class and initialize class members for Adding new defect
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="productionLineNumber"></param>
        /// <param name="productionDate"></param>
        /// <param name="defectTime"></param>
        /// <param name="gloveSize"></param>
        /// <param name="tierSide"></param>
        /// <param name="pnDefectQty"></param>
        /// <param name="qaiDefectQty"></param>
        /// <param name="productionDefectId"></param>
        /// <param name="pdl"></param>
        public AddEditDefect(decimal serialNumber, string productionLineNumber, DateTime productionDate, string defectTime,
            string gloveSize, string tierSide, int pnDefectQty, int qaiDefectQty, int productionDefectId
            //,int defectCategoryId
            ,ProductionDefectList pdl)
            : this()
        {
            _isCalledForAdd = true;
            _serialNumber = serialNumber;
            _productionLineNumber = productionLineNumber;
            _productionDate = productionDate;
            _defectTime = defectTime;
            _gloveSize = gloveSize;
            _tierSide = tierSide;
            _qaiDefectQty = qaiDefectQty;
            _pnDefectQty = pnDefectQty;
            _productionDefectId = productionDefectId;
           // _defectCategoryId = defectCategoryId;
            _pdl = pdl;
        }

        /// <summary>
        /// Instantiate the class and initialize Class Members for Editing production defect details
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="productionDefectId"></param>
        /// <param name="defectDetail"></param>
        /// <param name="defectDetailDescription"></param>
        /// <param name="defectDetailQty"></param>
        /// <param name="pnDefectQty"></param>
        /// <param name="qaiDefectQty"></param>
        /// <param name="id"></param>
        /// <param name="pdl"></param>
        public AddEditDefect(decimal serialNumber, int productionDefectId, string defectDetail, string defectDetailDescription,
            int defectDetailQty, int pnDefectQty, int qaiDefectQty, int id
            //,int defectCategoryId
            ,ProductionDefectList pdl)
            : this()
        {
            _serialNumber = serialNumber;
            _isCalledForAdd = false;
            _productionDefectId = productionDefectId;
            _defectDetail = defectDetail;
            _defectDetailDescription = defectDetailDescription;
            _defectDetailQty = defectDetailQty;
            _qaiDefectQty = qaiDefectQty;
            _Id = id;
            _pnDefectQty = pnDefectQty;
            //_defectCategoryId = defectCategoryId;
            _pdl = pdl;
        }

        #endregion

        #region User Methods

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

        /// <summary>
        /// Populate fields for adding new production defect detaisl
        /// </summary>
        private void PopulateFieldsForAdd()
        {
            List<TierSideMasterDTO> tierSideList = null;
            List<DefectTypeDTO> defectList = null;
            Text = Constants.ADD_DEFECT_SCREEN;
            txtLine.Text = _productionLineNumber;
            txtDate.Text = _productionDate.ToString(ConfigurationManager.AppSettings["smallDateFormat"]);
            txtHour.Text = _defectTime;
            txtSize.Text = _gloveSize;
            if (!string.IsNullOrEmpty(_tierSide))
            {
                ddSide.Items.Add(_tierSide);
                ddSide.SelectedIndex = Constants.ZERO;
                ddSide.Enabled = false;
            }
            else
            {
                tierSideList = ProductionDefectBLL.GetTierSide();
                foreach (TierSideMasterDTO tierSide in tierSideList)
                {
                    ddSide.Items.Add(tierSide.TierSide);
                }
            }
            defectList = ProductionDefectBLL.GetDefectTypeList();
            if (_qaiDefectQty > Constants.ZERO)
            {
                if (defectList != null)
                {
                    foreach (DefectTypeDTO defect in defectList)
                    {
                        if (!defect.ProdDefectName.Equals(Constants.NODEFECT))
                            ddDefect.Items.Add(defect.ProdDefectName);
                    }
                }
            }
            else
            {
                var defect = defectList.Where(d => d.ProdDefectName.Equals(Constants.NODEFECT)).Single();
                ddDefect.Items.Add(defect.ProdDefectName);
                ddDefect.SelectedIndex = Constants.ZERO;
                ddDefect.Enabled = false;
                txtDefectDescription.Enabled = false;
                nudQuantity.Enabled = false;
            }
        }

        /// <summary>
        /// Populate form fields for editing production defect detail
        /// </summary>
        private void PopulateFieldsForEdit()
        {
            ProductionDefectDTO pd = ProductionDefectBLL.GetProductionDefect(_productionDefectId);
            Text = Constants.EDIT_DEFECT_SCREEN;
            txtLine.Text = pd.LineId.ToString();
            txtDate.Text = pd.DefectDate.ToString(ConfigurationManager.AppSettings["smallDateFormat"]);
            txtHour.Text = pd.DefectTime;
            ddSide.Items.Add(pd.TierSide);
            ddSide.SelectedIndex = Constants.ZERO;
            ddSide.Enabled = false;
            txtSize.Text = pd.GloveSize;
            ddDefect.Items.Add(_defectDetail);
            ddDefect.SelectedIndex = Constants.ZERO;
            ddDefect.Enabled = false;
            ddDefect.Enabled = false;
            txtDefectDescription.Text = _defectDetailDescription;
            nudQuantity.Value = _defectDetailQty;
        }

        /// <summary>
        /// Validate data in required fields
        /// </summary>
        /// <returns></returns>
        private bool ValidateRequiredFieldsForAdd()
        {
            bool status = false;
            string requiredFieldMessage = Messages.REQUIREDFIELDMESSAGE;
            if (string.IsNullOrEmpty(ddSide.Text) || ddDefect.SelectedIndex < Constants.ZERO )
            {
                if (string.IsNullOrEmpty(ddSide.Text))
                {
                    requiredFieldMessage = requiredFieldMessage + Constants.SIDE + Environment.NewLine;
                }
                if (ddDefect.SelectedIndex < Constants.ZERO)
                {
                    requiredFieldMessage = requiredFieldMessage + Constants.DEFECT + Environment.NewLine;
                }
                if (requiredFieldMessage == Messages.REQUIREDFIELDMESSAGE)
                {
                    status = true;
                }
                else
                {
                    GlobalMessageBox.Show(requiredFieldMessage, Constants.AlertType.Information, Messages.DATAREQUIRED, GlobalMessageBoxButtons.OK);
                    if(ddSide.Enabled && ddSide.SelectedIndex==Constants.MINUSONE)
                    {
                        ddSide.Focus();
                    }
                    else if(ddDefect.SelectedIndex==Constants.MINUSONE)
                    {
                        ddDefect.Focus();
                    }
                    status = false;
                }
                return status;
            }
            else
            {
                if (nudQuantity.Value == Constants.ZERO)
                {
                    if (_qaiDefectQty != Constants.ZERO)
                    {
                        GlobalMessageBox.Show(Messages.QUANTITY_NULL_PRODUCTION_DEFECT, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        nudQuantity.Focus();
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Clear data on form controls
        /// </summary>
        private void ClearForm()
        {
            if (_isCalledForAdd)
            {
                ddDefect.SelectedIndex = Constants.MINUSONE;
                txtDefectDescription.Clear();
                nudQuantity.Value = Constants.ZERO;
                if (ddSide.Enabled)
                {
                    ddSide.SelectedIndex = Constants.MINUSONE;
                    ddSide.Focus();
                }
                else
                {
                    ddDefect.Focus();
                }
            }
            else
            {
                nudQuantity.Value = _defectDetailQty;
                nudQuantity.Focus();
            }
            CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Load defect description based on defect selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ddDefect_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string defectType = string.Empty;
                if (ddDefect.SelectedIndex >= Constants.ZERO)
                {
                    defectType = ddDefect.SelectedItem.ToString();
                    txtDefectDescription.Text = ProductionDefectBLL.GetDefectDescriptionByDefectType(defectType);
                }
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, ddDefect.Name, ddDefect.SelectedValue.ToString());
                return;
            }
        }

        /// <summary>
        /// Save production defect details in database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int noofrows = Constants.ZERO;
                string defectType = string.Empty;
                int defectQuantity = Constants.ZERO;
                int GetPNQuantity = Constants.ZERO;
                int newDefectQuantity = Constants.ZERO;
                string tierSide = string.Empty;
                _workStationNumber = WorkStationDTO.GetInstance().WorkStationId;
                if (_isCalledForAdd)
                {
                    if (ValidateRequiredFieldsForAdd())
                    {
                        defectType = ddDefect.SelectedItem.ToString();
                        defectQuantity = Convert.ToInt32(nudQuantity.Value);
                        tierSide = ddSide.Text;
                        //GetPNQuantity = ProductionDefectBLL.GetPNQuantityDetails(_productionDefectId);
                        GetPNQuantity = ProductionDefectBLL.GetPNQuantityDetailsBySerialNo(_serialNumber); //Azman Kasim 15/05/2020
                        if (GetPNQuantity + defectQuantity <= _qaiDefectQty)
                        {
                            noofrows = ProductionDefectBLL.UpdateProductionDefectDetails(_productionDefectId, defectType, defectQuantity, tierSide, _serialNumber,
                                _productionLineNumber, _gloveSize, _qaiDefectQty, _productionDate, _defectTime, _workStationNumber
                                //,_defectCategoryId
                                );
                            GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                            if (noofrows > Constants.ZERO)
                            {
                                _pdl.PopulateDefectSummaryGrid();
                                _pdl.SelectRowInDefectSummaryGrid(_serialNumber
                                    //, _defectCategoryId
                                    );
                                Close();
                            }
                        }
                        else
                        {
                            GlobalMessageBox.Show(Messages.PNDEFECTCOUNT_GREATERTHAN_QAIDEFECTCOUNT, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                            nudQuantity.Value = _defectDetailQty;
                            nudQuantity.Focus();
                        }
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.PNDEFECTCOUNT_GREATERTHAN_QAIDEFECTCOUNT, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        nudQuantity.Value = Constants.ZERO;
                        nudQuantity.Focus();
                    }                   
                }
                else
                {
                    newDefectQuantity = Convert.ToInt32(nudQuantity.Value);
                    GetPNQuantity = ProductionDefectBLL.GetPNQuantityDetails(_productionDefectId);
                    if (GetPNQuantity - _defectDetailQty + newDefectQuantity <= _qaiDefectQty)
                    {
                        noofrows = ProductionDefectBLL.EditProductionDefectDetails(_Id, _productionDefectId, newDefectQuantity);
                        GlobalMessageBox.Show(Messages.DATA_SAVED_SUCCESSFULLY, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                        if (noofrows > Constants.ZERO)
                        {
                            _pdl.PopulateDefectSummaryGrid();
                            _pdl.SelectRowInDefectSummaryGridForEdit(_productionDefectId
                                //,_defectCategoryId
                                );
                            Close();
                        }
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.PNDEFECTCOUNT_GREATERTHAN_QAIDEFECTCOUNT, Constants.AlertType.Warning, Messages.INVALID_DATA, GlobalMessageBoxButtons.OK);
                        nudQuantity.Value = _defectDetailQty;
                        nudQuantity.Focus();
                    }
                }
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, btnSave.Name, null);
                return;
            }
        }

        /// <summary>
        /// Clear the data in form controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (GlobalMessageBox.Show(Messages.CONFIRM_CANCEL_TRANSACTION, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
            {
                Close();
            }
        }

        /// <summary>
        /// Load data in form controls on form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddEditDefect_Load(object sender, EventArgs e)
        {
            try
            {
                if (_isCalledForAdd)
                    PopulateFieldsForAdd();
                else
                    PopulateFieldsForEdit();
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, Name, null);
                return;
            }
        }
        #endregion

    }
}
