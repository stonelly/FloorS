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

    public partial class WasherDryerAssignment : Form
    {
        List<GloveCodeDTO> gloveCodeList = null;
        string _washerProgram = "", _dryerProcess = "", _gloveCode = "";
        int _washerId, _dryerId, _gloveCodeId;

        public WasherDryerAssignment()
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
                pbDryerAdd.Enabled = false;
                pbWasherAdd.Enabled = false;
                grdGloveDetails.ClearSelection();
            }
            //CommonBLL.ReloadWorkStationAndFloorSystemConfiguration();
        }




        //private void FillQCTypeForm()
        //{
        //    List<QCTypeDTO> qcTypeList = null;

        //    try
        //    {
        //        qcTypeList = GloveCodeBLL.GetQCTypeDetails(_gloveCode);
        //        grdQcTypeDetails.Rows.Clear();
        //        if (qcTypeList != null)
        //        {
        //            for (int i = Constants.ZERO; i < qcTypeList.Count; i++)
        //            {
        //                grdQcTypeDetails.Rows.Add();
        //                grdQcTypeDetails[Constants.ZERO, i].Value = qcTypeList[i].QCType;
        //                grdQcTypeDetails[Constants.ONE, i].Value = qcTypeList[i].NumOfTester;
        //                grdQcTypeDetails[Constants.TWO, i].Value = qcTypeList[i].PiecesHR;
        //                grdQcTypeDetails[Constants.THREE, i].Value = qcTypeList[i].Description;
        //                grdQcTypeDetails[Constants.FOUR, i].Value = qcTypeList[i].QcTypeId;
        //                grdQcTypeDetails[Constants.FIVE, i].Value = qcTypeList[i].DescId;

        //            }
        //            //grdQcTypeDetails.ClearSelection();
        //        }
        //    }
        //    catch(Exception ex)
        //    {

        //    }
        //}

        private void FillWasherForm()
        {
            List<WasherGloveDTO> washerList = null;

            washerList = GloveCodeBLL.GetWasherProgramDetails(_gloveCode);
            //foreach (var item in washerList)
            //{
            //    if (item.GloveCode == null)
            //        washerList.Remove(item);
            //}
            gridWasher.Rows.Clear();
            if (washerList != null)
            {
                for (int i = Constants.ZERO; i < washerList.Count; i++)
                {
                    gridWasher.Rows.Add();
                    gridWasher[Constants.ONE, i].Value = washerList[i].WasherId;
                    gridWasher[Constants.ZERO, i].Value = washerList[i].WasherProgram;
                }
                gridWasher.ClearSelection();
            }
        }

        private void FillDryerProcess()
        {
            List<DryerGloveDTO> dryerList = null;
            dryerList = GloveCodeBLL.GetDryerProcessDetails(_gloveCode);
            gridDryer.Rows.Clear();
            if (dryerList != null)
            {
                for (int i = Constants.ZERO; i < dryerList.Count; i++)
                {
                    gridDryer.Rows.Add();
                    gridDryer[Constants.ONE, i].Value = dryerList[i].DryerId;
                    gridDryer[Constants.ZERO, i].Value = dryerList[i].DryerProcess;
                }
                gridDryer.ClearSelection();
            }
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

        private void GloveCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
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
                    FillWasherForm();
                    FillDryerProcess();
                    pbWasherAdd.Enabled = true;
                    pbDryerAdd.Enabled = true;

                }
                catch (Exception ed)
                {
                    // GlobalMessageBox.Show(ed.Message, Constants.AlertType.Information, Messages.INFORMATION, GlobalMessageBoxButtons.OK);
                    return;
                }
            }
        }

        private void pbWasherAdd_Click(object sender, EventArgs e)
        {
            new WasherForm(_gloveCode, _washerProgram).ShowDialog();
            FillWasherForm();

        }

        private void pbWasherEdit_Click(object sender, EventArgs e)
        {
            if (gridWasher.SelectedRows.Count > 0)
            {
                _washerId = int.Parse(gridWasher.SelectedRows[0].Cells[1].Value.ToString());
                _washerProgram = gridWasher.SelectedRows[0].Cells[0].Value.ToString();
                new WasherForm(_gloveCode, _washerProgram, _washerId).ShowDialog();
                FillWasherForm();
            }
            else
                pbWasherEdit.Enabled = false;
        }

        private void pbWasherDelete_Click(object sender, EventArgs e)
        {
            if (gridWasher.SelectedRows.Count > 0)
            {
                int washerId = int.Parse(gridWasher.SelectedRows[0].Cells[1].Value.ToString());
                if (GlobalMessageBox.Show(Messages.DELETE_DEFECTIVE_GLOVE_REASON, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                {
                    GloveCodeBLL.DeleteWasherForm(washerId);
                    FillWasherForm();
                }
            }
            else
                pbWasherDelete.Enabled = false;
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            gloveCodeList = GloveCodeBLL.GetGloveCodeDetails("", "", "", "", "");
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

        private void gridWasher_SelectionChanged(object sender, EventArgs e)
        {
            if (gridWasher.SelectedRows.Count > 0)
            {
                pbWasherEdit.Enabled = true;
                pbWasherDelete.Enabled = true;
            }
        }

        private void WasherDryerAssignment_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void gridDryer_SelectionChanged(object sender, EventArgs e)
        {
            if (gridDryer.SelectedRows.Count > 0)
            {
                pbDryerEdit.Enabled = true;
                pbDryerDelete.Enabled = true;
            }
        }

        private void pbDryerDelete_Click(object sender, EventArgs e)
        {
            if (gridDryer.SelectedRows.Count > 0)
            {
                int dryerId = int.Parse(gridDryer.SelectedRows[0].Cells[1].Value.ToString());
                if (GlobalMessageBox.Show(Messages.DELETE_DEFECTIVE_GLOVE_REASON, Constants.AlertType.Question, Messages.CONFIRM, GlobalMessageBoxButtons.YesNo) == Constants.YES)
                {
                    GloveCodeBLL.DeleteDryerForm(dryerId);
                    FillDryerProcess();
                }
            }
            else
                pbDryerDelete.Enabled = false;
        }

        private void pbDryerEdit_Click(object sender, EventArgs e)
        {
            if (gridDryer.SelectedRows.Count > 0)
            {
                _dryerId = int.Parse(gridDryer.SelectedRows[0].Cells[1].Value.ToString());

                _dryerProcess = gridDryer.SelectedRows[0].Cells[0].Value.ToString();
                new DryerForm(_gloveCode, _dryerProcess, _dryerId).ShowDialog();
                FillDryerProcess();
            }
            else
                pbDryerEdit.Enabled = false;
        }

        private void pbDryerAdd_Click(object sender, EventArgs e)
        {
            new DryerForm(_gloveCode, _dryerProcess).ShowDialog();
            FillDryerProcess();
        }

    }
}