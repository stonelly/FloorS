using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework.Common;

namespace Hartalega.FloorSystem.Windows.UI.SetUpConfiguration
{
    /// <summary>
    /// Module: Set Up Configuration
    /// Screen Name: QAI Defect Master
    /// File Type: Code file
    /// </summary>
    public partial class QAIDefectMaster : FormBase
    {
        #region Member Variables
        private string _screenName = "Configuration SetUp - QAIDefectMaster";
        private string _className = "QAIDefectMaster";
        private string _defectCategory;
        private string _defectCategorySel;
        private int _defectCategoryId;
        private int _sequence;
        private int _defectId;
        private string _keyStroke;
        private string _defectName;
        private string _defectCode;
        private bool _isAnd;
        List<QAIDefectCategory> _defectCategoryDTO = null;
        private string _loggedInUser;
        private int _defectCategoryTypeId;
        private string _defectCategoryType;
        private string _qcType;
        private int _defectPositionId;
        private string _keyStroke_P;
        private string _defectName_P;
        private string _defectCategoryGroup;
        private int _defectCategoryGroupId;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor initialising the components
        /// </summary>       
        /// <returns></returns>
        public QAIDefectMaster(string loggedInUser)
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
            dgvDefectCategory.Rows.Clear();
            dgvDefectDetails.Rows.Clear();
        }

        /// <summary>
        /// Get QAI Defect Category
        /// </summary>
        private void GetQAIDefectCategory()
        {            
            try
            {
                _defectCategoryDTO = SetUpConfigurationBLL.GetQAIDefectCategory();
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "GetQAIDefectCategory", null);
                return;
            }    
        }

        /// <summary>
        /// Get QAI defect details
        /// </summary>
        /// <param name="defectCategoryId"></param>
        private void GetQAIDefectDetails(int defectCategoryId)
        {
            List<QAIDefectDetails> defectDetailsDTO = null;
            dgvDefectDetails.Rows.Clear();
            btnDetailEdit.Enabled = false;

            try
            {
                defectDetailsDTO = SetUpConfigurationBLL.GetQAIDefectDetails(defectCategoryId);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "GetQAIDefectDetails", null);
                return;
            }

            if (defectDetailsDTO != null)
            {
                for (int i = 0; i < defectDetailsDTO.Count; i++)
                {
                    dgvDefectDetails.Rows.Add();
                    dgvDefectDetails[0, i].Value = defectDetailsDTO[i].DefectID;
                    dgvDefectDetails[1, i].Value = defectDetailsDTO[i].KeyStroke;
                    dgvDefectDetails[2, i].Value = defectDetailsDTO[i].DefectItem;
                    dgvDefectDetails[3, i].Value = defectDetailsDTO[i].DefectCode;
                    dgvDefectDetails[4, i].Value = defectDetailsDTO[i].DefectCategory;
                   dgvDefectDetails[5, i].Value = defectDetailsDTO[i].IsAnd;                  
                    dgvDefectDetails[6, i].Value = defectDetailsDTO[i].QCType;
                    dgvDefectDetails[7, i].Value = defectDetailsDTO[i].DefectCategoryGroup;
                    dgvDefectDetails[8, i].Value = defectDetailsDTO[i].DefectCategoryGroupId;
                }
                CurrentQAIDefectDetails();
            }
        }      
        
        /// <summary>
        /// Get QAI defect Positions
        /// </summary>
        /// <param name="defectId"></param>
        private void GetQAIDefectPositions(int defectId)
        {
            List<QAIDefectPositions> defectPositionsDTO = null;
            dgvDefectDetailPosition.Rows.Clear();
            btnPositionEdit.Enabled = false;

            try
            {
                defectPositionsDTO = SetUpConfigurationBLL.GetQAIDefectPositions(defectId);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "GetQAIDefectPositions", null);
                return;
            }

            if (defectPositionsDTO != null)
            {
                for (int i = 0; i < defectPositionsDTO.Count; i++)
                {
                    dgvDefectDetailPosition.Rows.Add();
                    dgvDefectDetailPosition[0, i].Value = defectPositionsDTO[i].DefectPositionId;
                    dgvDefectDetailPosition[1, i].Value = defectPositionsDTO[i].KeyStroke;
                    dgvDefectDetailPosition[2, i].Value = defectPositionsDTO[i].DefectPositionItem;
                }
                CurrentQAIDefectPositions();
            }
        }

        /// <summary>
        /// Get Current QAI defect category
        /// </summary>
        private void CurrentQAIDefectCategory()
        {
            if (dgvDefectCategory.Rows.Count > 0 && dgvDefectCategory.CurrentRow != null)
            {
                btnDetailAdd.Enabled = true;
                btnDetailEdit.Enabled = false;
                btnCategoryEdit.Enabled = true;
                int row = dgvDefectCategory.CurrentRow.Index;
                _defectCategoryId = Convert.ToInt32(dgvDefectCategory[0, row].Value);
                _defectCategory = Convert.ToString(dgvDefectCategory[1, row].Value);
                _defectCategoryType = Convert.ToString(dgvDefectCategory[2, row].Value);
                _sequence = Convert.ToInt32(dgvDefectCategory[3, row].Value);
                _defectCategoryTypeId = Convert.ToInt32(dgvDefectCategory[4, row].Value);
                GetQAIDefectDetails(_defectCategoryId);
            }   
        }

        /// <summary>
        /// Get Current QAI defect details
        /// </summary>
        private void CurrentQAIDefectDetails()
        {
            if (dgvDefectDetails.Rows.Count > 0 && dgvDefectDetails.CurrentRow != null)
            {
                btnPositionAdd.Enabled = true;
                btnDetailEdit.Enabled = true;
                int row = dgvDefectDetails.CurrentRow.Index;
                _defectId = Convert.ToInt32(dgvDefectDetails[0, row].Value);
                _keyStroke = Convert.ToString(dgvDefectDetails[1, row].Value);
                _defectName = Convert.ToString(dgvDefectDetails[2, row].Value);
                _defectCategorySel = Convert.ToString(dgvDefectDetails[4, row].Value);
                _isAnd = Convert.ToBoolean(dgvDefectDetails[5, row].Value);
                _qcType = Convert.ToString(dgvDefectDetails[6, row].Value);
                _defectCode = Convert.ToString(dgvDefectDetails[3, row].Value);
                _defectCategoryGroup = Convert.ToString(dgvDefectDetails[7, row].Value);
                _defectCategoryGroupId = Convert.ToInt32(dgvDefectDetails[8, row].Value);
                GetQAIDefectPositions(_defectId);
            }
            else
                btnDetailEdit.Enabled = false;
        }



        /// <summary>
        /// Get Current QAI defect positions
        /// </summary>
        private void CurrentQAIDefectPositions()
        {
            if (dgvDefectDetailPosition.Rows.Count > 0 && dgvDefectDetailPosition.CurrentRow != null)
            {
                btnPositionEdit.Enabled = true;
                int row = dgvDefectDetailPosition.CurrentRow.Index;
                _defectPositionId = Convert.ToInt32(dgvDefectDetailPosition[0, row].Value);
                _keyStroke_P = Convert.ToString(dgvDefectDetailPosition[1, row].Value);
                _defectName_P = Convert.ToString(dgvDefectDetailPosition[2, row].Value);
            }
            else
                btnPositionEdit.Enabled = false;
        }

        #endregion

        #region Event Handlers

        private void QAIDefectMaster_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                e.SuppressKeyPress = true;

        }

        /// <summary>
        /// Event Handler for Form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QAIDefectMaster_Load(object sender, EventArgs e)
        {
            backgroundWorker.RunWorkerAsync();
            this.dgvDefectCategory.AllowUserToAddRows = false;
            this.dgvDefectDetails.AllowUserToAddRows = false;           
        }

        /// <summary>
        /// Event Handler for Category Add image button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCategoryAdd_Click(object sender, EventArgs e)
        {
            new AddOrEditQAIDefectCategory(_loggedInUser,Constants.ADD_CONTROL).ShowDialog();
            backgroundWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Event Handler for Category Edit image button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCategoryEdit_Click(object sender, EventArgs e)
        {
            new AddOrEditQAIDefectCategory(Constants.EDIT_CONTROL, _defectCategoryId, _defectCategory, _sequence, 
                                          _loggedInUser,_defectCategoryTypeId).ShowDialog();
            backgroundWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Event Handler for Defect Add image button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDetailAdd_Click(object sender, EventArgs e)
        {
            new AddOrEditQAIDefect(Constants.ADD_CONTROL, _defectCategoryId, _loggedInUser).ShowDialog();
            GetQAIDefectDetails(_defectCategoryId);
            backgroundWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Event Handler for Defect Edit image button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDetailEdit_Click(object sender, EventArgs e)
        {
            new AddOrEditQAIDefect(Constants.EDIT_CONTROL,_defectId,_defectCategoryId,_keyStroke,_defectName,_defectCode,
                                    _defectCategorySel, _isAnd, _loggedInUser, _qcType, _defectCategoryGroupId).ShowDialog();
           
            GetQAIDefectDetails(_defectCategoryId);
            backgroundWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Event Handler for selection changed of datagrid view for Defect category
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void dgvDefectCategory_SelectionChanged(object sender, EventArgs e)
        {
            CurrentQAIDefectCategory();
        }
                
        /// <summary>
        /// Event Handler for datagridview cell click
        /// using keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDefectCategory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                e.Handled = true;
            }

            //disable delete key
            if (e.KeyCode == Keys.Delete)
                e.SuppressKeyPress = true;
        }

        /// <summary>
        /// Event Handler for datagridview cell click
        /// using keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDefectCategory_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                CurrentQAIDefectCategory();
            }
        }

        /// <summary>
        /// Event Handler for datagridview cell click
        /// using keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDefectDetails_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                e.Handled = true;
            }

            if (e.KeyCode == Keys.Delete)
                e.SuppressKeyPress = true;
        }

        /// <summary>
        /// Event Handler for datagridview cell click
        /// using keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDefectDetails_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                CurrentQAIDefectDetails();
            }
        }


        /// <summary>
        /// Event Handler for datagridview cell click
        /// using keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDefectPositions_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                e.Handled = true;
            }

            if (e.KeyCode == Keys.Delete)
                e.SuppressKeyPress = true;
        }

        /// <summary>
        /// Event Handler for datagridview cell click
        /// using keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDefectPositions_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                CurrentQAIDefectPositions();
            }
        }

        /// <summary>
        /// Event Handler to perform the time consuming tasks asynchronously
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            btnCategoryEdit.Enabled = false;
            dgvDefectCategory.Rows.Clear();
            if (_defectCategoryDTO != null)
            {
                for (int i = 0; i < _defectCategoryDTO.Count; i++)
                {
                    dgvDefectCategory.Rows.Add();
                    dgvDefectCategory[0, i].Value = _defectCategoryDTO[i].ID;
                    dgvDefectCategory[1, i].Value = _defectCategoryDTO[i].DefectCategory;
                    dgvDefectCategory[2, i].Value = _defectCategoryDTO[i].DefectCategoryType;
                    dgvDefectCategory[3, i].Value = _defectCategoryDTO[i].Sequence;
                    dgvDefectCategory[4, i].Value = _defectCategoryDTO[i].DefectCategoryTypeId;
                }
                CurrentQAIDefectCategory();
            }            
        }

        /// <summary>
        /// Event Handler to perform the time consuming tasks asynchronously
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            GetQAIDefectCategory();
        }

        /// <summary>
        /// Event Handler for datagrid selection changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDefectDetails_SelectionChanged(object sender, EventArgs e)
        {
            CurrentQAIDefectDetails();
        }


        /// <summary>
        /// Event Handler for datagrid selection changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDefectPositions_SelectionChanged(object sender, EventArgs e)
        {
            CurrentQAIDefectPositions();
        }
        #endregion

        private void btnPositionAdd_Click(object sender, EventArgs e)
        {
            new AddOrEditQAIDefectPosition(Constants.ADD_CONTROL, _defectPositionId, _defectId, _defectCategoryId, _loggedInUser).ShowDialog();
            GetQAIDefectPositions(_defectPositionId);
            backgroundWorker.RunWorkerAsync();

        }

        private void btnPositionEdit_Click(object sender, EventArgs e)
        {
            new AddOrEditQAIDefectPosition(Constants.EDIT_CONTROL, _defectPositionId, _defectId, _defectCategoryId, _keyStroke_P, _defectName_P, _loggedInUser).ShowDialog();

            GetQAIDefectPositions(_defectPositionId);
            backgroundWorker.RunWorkerAsync();

        }
    }
}
