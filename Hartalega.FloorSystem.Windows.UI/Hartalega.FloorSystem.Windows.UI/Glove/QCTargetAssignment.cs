using Hartalega.FloorSystem.Framework.DbExceptionLog;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Windows.UI.SetUpConfiguration;

namespace Hartalega.FloorSystem.Windows.UI.Glove
{
    //Author : Shabbeer Hussain Shaik, Date : 13-9-2017

    public partial class QCTargetAssignment : Form
    {
        List<GloveCodeDTO> gloveCodeList = null;
        string _qcType = "", _qcDescription = "", _washerProgram = "", _dryerProcess = "", _gloveCode = "";
        int _qcId, _qcDescId, _washerId, _dryerId,_piecesHR;
        int _numTesters, _gloveCodeId;

        public QCTargetAssignment()
        {
            InitializeComponent();

            try
            {
                GloveCodeDetails();
            }
            catch (FloorSystemException fsEX)
            {
                MessageBox.Show(fsEX.Message);
                //ExceptionLogging(fsEX, _screenName, _className, this.Name, this.Text);
                return;
            }
        }

        public void GloveCodeDetails()
        {
            FillGloveCodeGrid();
        }


        /// <summary>
        /// Populate data in the Glove Details grid
        /// </summary>
        public void FillGloveCodeGrid(string gloveCode = "", string gloveCategory = "", string barcode = "", string Cond1 = "", string Cond2 = "")
        {
            gloveCodeList = GloveCodeBLL.GetGloveCodeDetails(gloveCode, gloveCategory, barcode, Cond1, Cond2);

            grdGloveDetails.ClearSelection();
            grdGloveDetails.Rows.Clear();
            if (gloveCodeList != null)
            {
                //selectionException = !selectionException;
                for (int i = Constants.ZERO; i < gloveCodeList.Count; i++)
                {
                    grdGloveDetails.Rows.Add();
                    grdGloveDetails[Constants.ZERO, i].Value = gloveCodeList[i].Id;
                    grdGloveDetails[Constants.ONE, i].Value = gloveCodeList[i].GloveCode;
                    grdGloveDetails[Constants.TWO, i].Value = gloveCodeList[i].Description;
                    grdGloveDetails[Constants.THREE, i].Value = gloveCodeList[i].Barcode;
                    grdGloveDetails[Constants.FOUR, i].Value = gloveCodeList[i].GloveCategory;

                }
                pbQcTypeAdd.Enabled = false;
                grdGloveDetails.ClearSelection();
            }
            //CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
        }


     


        #region SSH: Placeholder

        private void tbGloveCode_Enter(object sender, EventArgs e)
        {
            if (tbGloveCode.Text == "Glove Code")
            {
                tbGloveCode.Text = "";
            }
        }

        private void tbGloveCode_Leave(object sender, EventArgs e)
        {
            if (tbGloveCode.Text == "")
            {
                tbGloveCode.Text = "Glove Code";
            }
        }

        private void tbBarcode_Enter(object sender, EventArgs e)
        {
            if (tbBarcode.Text == "Barcode")
            {
                tbBarcode.Text = "";
            }
        }

        private void tbBarcode_Leave(object sender, EventArgs e)
        {
            if (tbBarcode.Text == "")
            {
                tbBarcode.Text = "Barcode";
            }
        }

        private void tbGloveCategory_Leave(object sender, EventArgs e)
        {
            if (tbGloveCategory.Text == "")
            {
                tbGloveCategory.Text = "Glove Category";
            }
        }

        private void tbGloveCategory_Enter(object sender, EventArgs e)
        {
            if (tbGloveCategory.Text == "Glove Category")
            {
                tbGloveCategory.Text = "";
            }
        }

        #endregion SSH: Placeholder

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string gloveCode = "", gloveCategory = "", barcode = "";
            gloveCode = (tbGloveCode.Text == "Glove Code") ? "" : tbGloveCode.Text;
            barcode = (tbBarcode.Text == "Barcode") ? "" : tbBarcode.Text;
            gloveCategory = (tbGloveCategory.Text == "Glove Category") ? "" : tbGloveCategory.Text;



            FillGloveCodeGrid(gloveCode, gloveCategory, barcode, cbCond1.Text, cbCond2.Text);
        }

        private void grdGloveDetails_SelectionChanged(object sender, EventArgs e)
        {
            if (grdGloveDetails.Rows.Count > 0 && grdGloveDetails.CurrentRow != null)
            {
                try
                {
                    _gloveCode = Convert.ToString(grdGloveDetails.SelectedRows[Constants.ZERO].Cells[1].Value);
                    _gloveCodeId = int.Parse(Convert.ToString(grdGloveDetails.SelectedRows[Constants.ZERO].Cells[0].Value));

                    int index = int.Parse(Convert.ToString(grdGloveDetails.SelectedRows[Constants.ZERO].Cells[Constants.ZERO].Value)) - 1;

                    FillQCTypeForm();
                    pbQcTypeAdd.Enabled = true;
                }
                catch (Exception ed)
                {
                    // GlobalMessageBox.Show(ed.Message, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    return;
                }
            }

        }
         
        private void FillQCTypeForm()
        {
            List<QCTypeDTO> qcTypeList = null;

            try
            {
                qcTypeList = GloveCodeBLL.GetQCTypeDetails(_gloveCode);
                grdQcTypeDetails.Rows.Clear();
                if (qcTypeList != null)
                {
                    for (int i = Constants.ZERO; i < qcTypeList.Count; i++)
                    {
                        grdQcTypeDetails.Rows.Add();
                        grdQcTypeDetails[Constants.ZERO, i].Value = qcTypeList[i].QCType;
                        grdQcTypeDetails[Constants.ONE, i].Value = qcTypeList[i].NumOfTester;
                        grdQcTypeDetails[Constants.TWO, i].Value = qcTypeList[i].PiecesHR;
                        grdQcTypeDetails[Constants.THREE, i].Value = qcTypeList[i].Description;
                        grdQcTypeDetails[Constants.FOUR, i].Value = qcTypeList[i].QcTypeId;
                        grdQcTypeDetails[Constants.FIVE, i].Value = qcTypeList[i].DescId;

                    }
                    //grdQcTypeDetails.ClearSelection();
                }
            }
            catch(Exception ex)
            {

            }
        }


        private void pbQcTypeAdd_Click(object sender, EventArgs e)
        {
            new QcTypeForm(_gloveCode, _qcType, _qcDescription, _numTesters).ShowDialog();
            FillQCTypeForm();
        }

        private void pbQcTypeEdit_Click(object sender, EventArgs e)
        {
            if (grdQcTypeDetails.SelectedRows.Count > 0)
            {
                _qcId = int.Parse(grdQcTypeDetails.SelectedRows[0].Cells[4].Value.ToString());
                _qcDescId = int.Parse(grdQcTypeDetails.SelectedRows[0].Cells[5].Value.ToString());
                _qcDescription = grdQcTypeDetails.SelectedRows[0].Cells[3].Value.ToString();
                _piecesHR = int.Parse(grdQcTypeDetails.SelectedRows[0].Cells[2].Value.ToString());
                _qcType = grdQcTypeDetails.SelectedRows[0].Cells[0].Value.ToString();
                _numTesters = int.Parse(grdQcTypeDetails.SelectedRows[0].Cells[1].Value.ToString());
                new QcTypeForm(_gloveCode,_qcType, _qcDescription, _numTesters, _qcId,_qcDescId,_piecesHR).ShowDialog();
                FillQCTypeForm();
            }
            else
                pbQcTypeEdit.Enabled = false;
        }

        private void pbQcTypeDelete_Click(object sender, EventArgs e)
        {
            if (grdQcTypeDetails.SelectedRows.Count > 0)
            {
                QCTypeDTO qcObj = new QCTypeDTO()
                {
                    GloveCode = _gloveCode, Description = grdQcTypeDetails.SelectedRows[0].Cells[2].Value.ToString(),
                    QCType = grdQcTypeDetails.SelectedRows[0].Cells[0].Value.ToString(),
                    NumOfTester = int.Parse(grdQcTypeDetails.SelectedRows[0].Cells[1].Value.ToString())
                };
                if (GlobalMessageBox.Show(Messages.DELETE_DEFECTIVE_GLOVE_REASON, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                {
                    GloveCodeBLL.DeleteQCTypeForm(qcObj);
                    FillQCTypeForm();
                }
            }
            else
                pbQcTypeDelete.Enabled = false;
        }

        
        private void GloveCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
           
                gloveCodeList = GloveCodeBLL.GetGloveCodeDetails("","","","","");

                grdGloveDetails.Rows.Clear();
                if (gloveCodeList != null)
                {
                    for (int i = Constants.ZERO; i < gloveCodeList.Count; i++)
                    {
                        grdGloveDetails.Rows.Add();
                        grdGloveDetails[Constants.ZERO, i].Value = gloveCodeList[i].Id;
                        grdGloveDetails[Constants.ONE, i].Value = gloveCodeList[i].GloveCode;
                        grdGloveDetails[Constants.TWO, i].Value = gloveCodeList[i].Description;
                        grdGloveDetails[Constants.THREE, i].Value = gloveCodeList[i].Barcode;
                        grdGloveDetails[Constants.FOUR, i].Value = gloveCodeList[i].GloveCategory;

                    }

                }
           
        }
        
        private void grdQcTypeDetails_SelectionChanged(object sender, EventArgs e)
        {
            if (grdQcTypeDetails.SelectedRows.Count > 0)
            {
                pbQcTypeEdit.Enabled = true;
                pbQcTypeDelete.Enabled = true;
            }
        }
    }
}