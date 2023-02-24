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
    public partial class GloveTypeMaster : FormBase
    {
        #region Member Variables
        private string _screenName = "Configuration SetUp - QAIDefectMaster";
        private string _className = "QAIDefectMaster";
      //  private string _defectCategory;
       // private string _defectCategorySel;
        private int _glovetypeId;
       // private int _sequence;
      //  private int _defectId;
       // private string _keyStroke;
       // private string _defectName;
       // private string _defectCode;
      //  private bool _isAnd;
        List<GloveTypeMasterDTO> _defectCategoryDTO = null;
        private string _loggedInUser;
     //   private int _defectCategoryTypeId;
      //  private string _defectCategoryType;
       // private string _qcType;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor initialising the components
        /// </summary>       
        /// <returns></returns>
        public GloveTypeMaster(string loggedInUser)
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
          //  gvSize.Rows.Clear();
        }

        /// <summary>
        /// Get QAI Defect Category
        /// </summary>
        private void GetQAIDefectCategory()
        {
            try
            {
                _defectCategoryDTO = SetUpConfigurationBLL.GetGloveType();
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
            List<GloveTypeMasterDTO> defectDetailsDTO = null;
            List<GloveTypeSizeRelationMasterDTO> CodeSizeRelationDTO = null;
            //gvSize.Rows.Clear();
            //btnDetailEdit.Enabled = false;

            try
            {
                defectDetailsDTO = SetUpConfigurationBLL.GetGloveTypeTest(defectCategoryId);
                CodeSizeRelationDTO = SetUpConfigurationBLL.GetGloveTypeSize(defectCategoryId);
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "GetQAIDefectDetails", null);
                return;
            }

            if (defectDetailsDTO != null)
            {
                //chkProtein.Checked = defectDetailsDTO[0].Protein == 1 ? true : false;
                //txtProtein.Text = defectDetailsDTO[0].ProteinSpec.ToString("n3");
                //chkPowder.Checked = defectDetailsDTO[0].Powder == 1 ? true : false;
                //cbPowder.SelectedValue = defectDetailsDTO[0].PowderFormula.ToString();
                //chkHotBox.Checked = defectDetailsDTO[0].Hotbox == 1 ? true : false;
                //chkPolymer.Checked = defectDetailsDTO[0].Polymer == 1 ? true : false;

                //if (CodeSizeRelationDTO != null)
                //{
                //    for (int i = 0; i < CodeSizeRelationDTO.Count; i++)
                //    {
                //        gvSize.Rows.Add();
                //        gvSize[0, i].Value = CodeSizeRelationDTO[i].COMMONSIZE;
                //        gvSize[1, i].Value = CodeSizeRelationDTO[i].GLOVEWEIGHT.ToString("n5");
                //        gvSize[2, i].Value = CodeSizeRelationDTO[i].MAX10PCSWT.ToString("n5");
                //        gvSize[3, i].Value = CodeSizeRelationDTO[i].MIN10PCSWT.ToString("n5");
                //        gvSize[4, i].Value = CodeSizeRelationDTO[i].Stopped;
                //    }
                //}

                    
                //CurrentQAIDefectDetails();
            }
        }

        /// <summary>
        /// Get Current QAI defect category
        /// </summary>
        private void CurrentQAIDefectCategory()
        {
            if (dgvDefectCategory.Rows.Count > 0 && dgvDefectCategory.CurrentRow != null)
            {
                //btnDetailAdd.Enabled = true;
                //btnDetailEdit.Enabled = false;
                //btnEdit.Enabled = true;
                int row = dgvDefectCategory.CurrentRow.Index;
                _glovetypeId = Convert.ToInt32(dgvDefectCategory[0, row].Value);
               // GetQAIDefectDetails(_glovetypeId);
            }
        }

        /// <summary>
        /// Get Current QAI defect details
        /// </summary>
        private void CurrentQAIDefectDetails()
        {
            //if (dgvDefectDetails.Rows.Count > 0 && dgvDefectDetails.CurrentRow != null)
            //{
            //    btnDetailEdit.Enabled = true;
            //    int row = dgvDefectDetails.CurrentRow.Index;
            //    _defectId = Convert.ToInt32(dgvDefectDetails[0, row].Value);
            //    _keyStroke = Convert.ToString(dgvDefectDetails[1, row].Value);
            //    _defectName = Convert.ToString(dgvDefectDetails[2, row].Value);
            //    _defectCategorySel = Convert.ToString(dgvDefectDetails[4, row].Value);
            //    _isAnd = Convert.ToBoolean(dgvDefectDetails[5, row].Value);
            //    _qcType = Convert.ToString(dgvDefectDetails[6, row].Value);
            //    _defectCode = Convert.ToString(dgvDefectDetails[3, row].Value);

            //}
            //else
            //    btnDetailEdit.Enabled = false;
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Event Handler for Form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QAIDefectMaster_Load(object sender, EventArgs e)
        {
            backgroundWorker.RunWorkerAsync();
            this.dgvDefectCategory.AllowUserToAddRows = false;
            //this.dgvDefectDetails.AllowUserToAddRows = false;           
        }

        /// <summary>
        /// Event Handler for Category Add image button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCategoryAdd_Click(object sender, EventArgs e)
        {
            new AddGloveType(_loggedInUser).ShowDialog();
            backgroundWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Event Handler for Category Edit image button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCategoryEdit_Click(object sender, EventArgs e)
        {
            //  new AddOrEditQAIDefectCategory(Constants.EDIT_CONTROL, _defectCategoryId, _defectCategory, _sequence,
            //  _loggedInUser, _defectCategoryTypeId).ShowDialog();
            //  backgroundWorker.RunWorkerAsync();
            CurrentQAIDefectCategory();
            new EditGloveType(_loggedInUser, _glovetypeId).ShowDialog();
            backgroundWorker.RunWorkerAsync();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            CurrentQAIDefectCategory();

            int rowReturned = 0;

            GloveTypeMasterDTO newitem = new GloveTypeMasterDTO();

            try
            {
                if (GlobalMessageBox.Show(Messages.DELETE_MESSAGE, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                {
                    newitem = SetUpConfigurationBLL.GetGloveTypeDetails(_glovetypeId);

                    newitem.STOPPED = 1;
                    newitem.ActionType = Constants.ActionLog.Delete;

                    GloveTypeMasterDTO olditem = new GloveTypeMasterDTO();

                    olditem = SetUpConfigurationBLL.GetGloveTypeDetails(_glovetypeId);
                    newitem.AVAGLOVECODETABLE_ID = olditem.AVAGLOVECODETABLE_ID;

                    rowReturned = SetUpConfigurationBLL.AddGloveType(olditem, newitem, _loggedInUser);
                    if (rowReturned > 0)
                    {
                        GlobalMessageBox.Show(Messages.DELETE_RECORD, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
                    }
                }
                else
                {
                    return;
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "btnDelete_Click", null);
                return;
            }
            backgroundWorker.RunWorkerAsync();
        }
    
        /// <summary>
        /// Event Handler for Defect Add image button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDetailAdd_Click(object sender, EventArgs e)
        {
         //   new AddOrEditQAIDefect(Constants.ADD_CONTROL, _defectCategoryId, _loggedInUser).ShowDialog();
          //  GetQAIDefectDetails(_defectCategoryId);
         //   backgroundWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Event Handler for Defect Edit image button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDetailEdit_Click(object sender, EventArgs e)
        {
          //  new AddOrEditQAIDefect(Constants.EDIT_CONTROL, _defectId, _defectCategoryId, _keyStroke, _defectName, _defectCode,
          //                          _defectCategorySel, _isAnd, _loggedInUser, _qcType).ShowDialog();

           // GetQAIDefectDetails(_defectCategoryId);
            //backgroundWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Event Handler for selection changed of datagrid view for Defect category
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void dgvDefectCategory_SelectionChanged(object sender, EventArgs e)
        {
            //CurrentQAIDefectCategory();
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
                //CurrentQAIDefectCategory();
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
        /// Event Handler to perform the time consuming tasks asynchronously
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
           // btnEdit.Enabled = false;
            dgvDefectCategory.Rows.Clear();
            if (_defectCategoryDTO != null)
            {
                for (int i = 0; i < _defectCategoryDTO.Count; i++)
                {
                    dgvDefectCategory.Rows.Add();
                    dgvDefectCategory[0, i].Value = _defectCategoryDTO[i].AVAGLOVECODETABLE_ID;
                    dgvDefectCategory[1, i].Value = _defectCategoryDTO[i].BARCODE;
                    dgvDefectCategory[2, i].Value = _defectCategoryDTO[i].GLOVECODE;
                    dgvDefectCategory[3, i].Value = _defectCategoryDTO[i].DESCRIPTION;
                    dgvDefectCategory[4, i].Value = _defectCategoryDTO[i].GLOVECATEGORY;
                    dgvDefectCategory[5, i].Value = _defectCategoryDTO[i].POWDER;
                    dgvDefectCategory[6, i].Value = _defectCategoryDTO[i].PROTEIN;
                    dgvDefectCategory[7, i].Value = _defectCategoryDTO[i].HOTBOX;
                    dgvDefectCategory[8, i].Value = _defectCategoryDTO[i].POLYMER;
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

        #endregion

       
    }
}
