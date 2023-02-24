using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;

namespace Hartalega.FloorSystem.Windows.UI.SetUpConfiguration
{
    /// <summary>
    /// Module: Set Up Configuration
    /// Screen Name: Production Line Maintenance
    /// File Type: Code file
    /// </summary> 
    public partial class ProductionLineMaintenance : FormBase
    {
        #region Member Variables

        private List<ProductionLineDTO> _productionLineDetails = null;
        private ProductionLineDTO _productionLineDTO = null;
        private string _screenName = "Configuration SetUp - ProductionLineMaintenance";
        private string _className = "ProductionLineMaintenance";
        private string _loggedInUser;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor initialising the components
        /// </summary>       
        /// <returns></returns>
        public ProductionLineMaintenance(string loggedInUser)
        {
            InitializeComponent();
            _loggedInUser = loggedInUser;
        }
        #endregion

        #region User Methods

        /// <summary>
        /// Log to DB, Show Mesage Box to User and clear the form on exception
        /// </summary>
        /// <param name="result"></param>
        private void ExceptionLogging(FloorSystemException floorException, string screenName, string UiClassName, string uiControl, params object[] parameters)
        {
            int result = CommonBLL.LogExceptionToDB(floorException, screenName, UiClassName, uiControl, parameters);
            if (floorException.subSystem == Constants.BUSINESSLOGIC)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.APPLICATIONERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            dgvProductionLineData.Rows.Clear();
        }

        /// <summary>
        /// Get Production Line Details
        /// </summary>
        private void GetProductionLineDetails()
        {
            _productionLineDetails = new List<ProductionLineDTO>();
            try
            {
                _productionLineDetails = SetUpConfigurationBLL.GetProductionLineDetails();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "GetProductionLineDetails", null);
                return;
            }  
        }

        /// <summary>
        /// Get Current Cell Details
        /// </summary>
        private void CurrentCellDetails()
        {
            _productionLineDTO = new ProductionLineDTO();
            if (dgvProductionLineData.Rows.Count > 0 && dgvProductionLineData.CurrentRow != null)
            {
                btnEdit.Enabled = true;
                int row = dgvProductionLineData.CurrentRow.Index;
                _productionLineDTO.LineNumber = Convert.ToString(dgvProductionLineData[1, row].Value);
                if (!string.IsNullOrEmpty(_productionLineDTO.LineNumber))
                {
                    _productionLineDTO.BatchFrequency = Convert.ToString(dgvProductionLineData[2, row].Value);
                    _productionLineDTO.IsOnline = Convert.ToBoolean(dgvProductionLineData[6, row].Value);
                    _productionLineDTO.IsDoubleFormer = Convert.ToBoolean(dgvProductionLineData[12, row].Value);
                    _productionLineDTO.Plant = Convert.ToString(dgvProductionLineData[8, row].Value);
                    string[] gloveTypes = dgvProductionLineData[4, row].Value.ToString().Split(new char[] { '\n' });
                    string[] gloveSizes = dgvProductionLineData[5, row].Value.ToString().Split(new char[] { '\n' });
                    string[] altGloveTypes = dgvProductionLineData[11, row].Value.ToString().Split(new char[] { '\n' });

                    _productionLineDTO.LTGloveType = gloveTypes[0].Trim('\r');
                    _productionLineDTO.RTGloveType = gloveTypes[1].Trim('\r');
                    _productionLineDTO.LBGloveType = gloveTypes[2].Trim('\r');
                    _productionLineDTO.RBGloveType = gloveTypes[3];
                    _productionLineDTO.LTGloveSize = gloveSizes[0].Trim('\r');
                    _productionLineDTO.RTGloveSize = gloveSizes[1].Trim('\r');
                    _productionLineDTO.LBGloveSize = gloveSizes[2].Trim('\r');
                    _productionLineDTO.RBGloveSize = gloveSizes[3];
                    if (!string.IsNullOrEmpty(Convert.ToString(dgvProductionLineData[9, row].Value)))
                    {
                        _productionLineDTO.ProdLoggingStartDateTime = Convert.ToDateTime(dgvProductionLineData[9, row].Value);
                    }
                    _productionLineDTO.OperatorId = _loggedInUser;
                    if (!string.IsNullOrEmpty(Convert.ToString(dgvProductionLineData[10, row].Value)))
                    {
                        _productionLineDTO.LastModifiedOn = Convert.ToDateTime(dgvProductionLineData[10, row].Value);
                    }
                    _productionLineDTO.IsPrintByFormer = Convert.ToBoolean(dgvProductionLineData[7, row].Value);
                    _productionLineDTO.LTAltGlove = altGloveTypes[0].Trim('\r');
                    _productionLineDTO.RTAltGlove = altGloveTypes[1].Trim('\r');
                    _productionLineDTO.LBAltGlove = altGloveTypes[2].Trim('\r');
                    _productionLineDTO.RBAltGlove = altGloveTypes[3];

                    _productionLineDTO.LTFormerType = Convert.ToString(dgvProductionLineData[13, row].Value);
                    _productionLineDTO.LBFormerType = Convert.ToString(dgvProductionLineData[14, row].Value);
                    _productionLineDTO.RTFormerType = Convert.ToString(dgvProductionLineData[15, row].Value);
                    _productionLineDTO.RBFormerType = Convert.ToString(dgvProductionLineData[16, row].Value);
                    _productionLineDTO.LatexType = Convert.ToString(dgvProductionLineData[17, row].Value);
                }
            }
        }
        #endregion

        #region Event Handlers

        /// <summary>
        /// Event Handler for form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProductionLineMaintenance_Load(object sender, EventArgs e)
        {
            backgroundWorker.RunWorkerAsync();
            this.dgvProductionLineData.AllowUserToAddRows = false;
        }

        /// <summary>
        /// Event Handler for datagridview selection changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void dgvProductionLineData_SelectionChanged(object sender, EventArgs e)
        {
            CurrentCellDetails();
        }
       
        /// <summary>
        /// Event Handler for Edit button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            new EditLineRecord(_productionLineDTO).ShowDialog();
            backgroundWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Event Handler for double click on grid row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProductionLineData_DoubleClick(object sender, EventArgs e)
        {
            CurrentCellDetails();
            new EditLineRecord(_productionLineDTO).ShowDialog();
            backgroundWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Event Handler for datagridview cell click
        /// using keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProductionLineData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Event Handler for datagridview cell click
        /// using keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProductionLineData_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                CurrentCellDetails();
            }
        }

        /// <summary>
        /// Event Handler to perform the time consuming tasks asynchronously
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            btnEdit.Enabled = false;
            dgvProductionLineData.Rows.Clear();
            if (_productionLineDetails != null)
            {
                for (int i = 0; i < _productionLineDetails.Count; i++)
                {
                    dgvProductionLineData.Rows.Add();
                    dgvProductionLineData[0, i].Value = dgvProductionLineData.Rows.Count;
                    dgvProductionLineData[1, i].Value = _productionLineDetails[i].LineNumber;
                    dgvProductionLineData[2, i].Value = _productionLineDetails[i].BatchFrequency;
                    if (!string.IsNullOrEmpty(_productionLineDetails[i].BatchFrequency))
                    {
                        if (_productionLineDetails[i].IsDoubleFormer)
                        {
                            dgvProductionLineData[3, i].Value = "LT" + Environment.NewLine + "RT" + Environment.NewLine +
                                                                "LB" + Environment.NewLine + "RB";
                        }
                        else 
                        {
                            dgvProductionLineData[3, i].Value = "LT" + Environment.NewLine + "RT";
                        }
                    }
                    dgvProductionLineData[4, i].Value = _productionLineDetails[i].LTGloveType + Environment.NewLine +
                                                        _productionLineDetails[i].RTGloveType + Environment.NewLine +
                                                        _productionLineDetails[i].LBGloveType + Environment.NewLine +
                                                        _productionLineDetails[i].RBGloveType;
                    dgvProductionLineData[5, i].Value = _productionLineDetails[i].LTGloveSize + Environment.NewLine +
                                                        _productionLineDetails[i].RTGloveSize + Environment.NewLine +
                                                        _productionLineDetails[i].LBGloveSize + Environment.NewLine +
                                                        _productionLineDetails[i].RBGloveSize;
                    dgvProductionLineData[6, i].Value = _productionLineDetails[i].IsOnline;
                    dgvProductionLineData[7, i].Value = _productionLineDetails[i].IsPrintByFormer;
                    dgvProductionLineData[8, i].Value = _productionLineDetails[i].Plant;
                    dgvProductionLineData[9, i].Value = _productionLineDetails[i].ProdLoggingStartDateTime;
                    dgvProductionLineData[10, i].Value = _productionLineDetails[i].LastModifiedOn;
                    dgvProductionLineData[11, i].Value = _productionLineDetails[i].LTAltGlove + Environment.NewLine +
                                                       _productionLineDetails[i].RTAltGlove + Environment.NewLine +
                                                       _productionLineDetails[i].LBAltGlove + Environment.NewLine +
                                                       _productionLineDetails[i].RBAltGlove;
                    dgvProductionLineData[12, i].Value = _productionLineDetails[i].IsDoubleFormer;

                    dgvProductionLineData[13, i].Value = _productionLineDetails[i].LTFormerType;
                    dgvProductionLineData[14, i].Value = _productionLineDetails[i].LBFormerType;
                    dgvProductionLineData[15, i].Value = _productionLineDetails[i].RTFormerType;
                    dgvProductionLineData[16, i].Value = _productionLineDetails[i].RBFormerType;
                    dgvProductionLineData[17, i].Value = _productionLineDetails[i].LatexType;
                }
                CurrentCellDetails();
            }            
        }

        /// <summary>
        /// Event Handler to perform the time consuming tasks asynchronously
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            GetProductionLineDetails();
        }
        #endregion         
    }
}
