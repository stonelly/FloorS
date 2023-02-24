using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using Hartalega.FloorSystem.Windows.UI.Properties;
using Hartalega.FloorSystem.Windows.UI.Tumbling;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.ProductionLogging
{
    public partial class SpeedControl : FormBase
    {

        #region Class Members
        private const string _screenName = "ProductionLineSpeedControl";
        private const string _className = "ProductionLineSpeedControl";
        private const string _screenNameLineControlMaintainence = "Line Speed Control";
        private string _loggedInUser;
        private string _lineId = string.Empty;
        private int _former = Constants.ZERO;
        private int _speed = Constants.ZERO;
        private decimal _cycle = Constants.ZERO;
        //private string _moduleName;
        #endregion

        #region Constructors
        public SpeedControl(string loggedInUser)
        {
            InitializeComponent();
            try
            {
                LoadProductionLineItems();
                LoadGloveCode();
                cmbLine.SelectedIndex = Constants.ZERO;
                cmbGlove.SelectedIndex = Constants.ZERO;
                dtpDate.MinDate = ServerCurrentDateTime;
                dtpDate.MaxDate = ServerCurrentDateTime.AddDays(Constants.THIRTY);
                //_moduleName = moduleName;
                _loggedInUser = Convert.ToString(loggedInUser);
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, this.Name, this.Text);
                return;
            }
        }
        #endregion

        #region Method
        private void productionActivitiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
            new ProductionLogging.ProductionActivity().ShowDialog();
        }

        private void productionLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
            new ProductionLogging.ProductionLineStartStop(string.Empty).ShowDialog();
        }

        private void lineSpeedControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            new ProductionLogging.SpeedControl(string.Empty).ShowDialog();
        }

        private void SpeedControl_Load(object sender, EventArgs e)
        {

        }

        private void SpeedControl_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void LoadProductionLineItems()
        {
            try
            {
                List<LineDTO> lineList = null;
                lineList = ProductionLoggingBLL.GetLineList(Constants.ZERO);
                if (lineList != null)
                {
                    foreach (LineDTO line in lineList)
                    {
                        //ddProductionLine.Items.Add(line.LineNumber);
                        cmbLine.Items.Add(line.LineNumber);
                        cmbLineAdd.Items.Add(line.LineNumber);
                    }
                }
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, this.Name, this.Text);
                return;
            }
        }

        private void LoadGloveCode()
        {
            try
            {
                List<GloveCodeDTO> lineList = null;
                lineList = ProductionLoggingBLL.GetGloveCodeList();
                if (lineList != null)
                {
                    foreach (GloveCodeDTO line in lineList)
                    {
                        cmbGloveAdd.Items.Add(line.GloveCode);
                    }
                }
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, this.Name, this.Text);
                return;
            }
        }

        private void LoadGloveCodeSelectedLine(string LineId)
        {
            try
            {
                cmbGlove.Items.Clear();
                List<GloveCodeDTO> lineList = null;
                lineList = ProductionLoggingBLL.GetGloveCodeListByLine(LineId);
                if (lineList != null)
                {
                    cmbGlove.Items.Add("");
                    foreach (GloveCodeDTO line in lineList)
                    {
                        cmbGlove.Items.Add(line.GloveCode);
                    }
                    cmbGlove.SelectedIndex = Constants.ZERO;
                }
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, this.Name, this.Text);
                return;
            }
        }

        private void LoadGloveDetails(string GloveCode, string LineId)
        {
            try
            {
                dgvGloveData.Rows.Clear();
                dgvGloveData.Refresh();
                DataTable DTable = ProductionLoggingBLL.GetGloveData(LineId, GloveCode);
                
                if (DTable.Rows.Count > Constants.ZERO)
                {
                    foreach (DataRow row in DTable.Rows)
                    {
                        dgvGloveData.Rows.Add(row["LineSpeedId"].ToString().Trim(), row["ProductionLineId"].ToString().Trim(),
                            row["GloveCode"].ToString().Trim(), row["Speed"].ToString().Trim(),
                            row["EffectiveDateTime"].ToString().Trim());
                    }
                }

            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, this.Name, this.Text);
                return;
            }
        }

        private bool ValidateData()
        {
            bool ret = false;
            try
            {
                if (cmbLineAdd.SelectedItem != null)
                    if (cmbGloveAdd.SelectedItem != null)
                    {
                        int parsedValue;
                        if (int.TryParse(tbSpeed.Text, out parsedValue))
                        {
                            string theDateTime = dtpDate.Value.ToString("yyyy-MM-dd") + " " + dtpTime.Value.ToString("HH:mm:ss");
                            string line = cmbLineAdd.SelectedItem.ToString().Trim();
                            string glove = cmbGloveAdd.SelectedItem.ToString().Trim();
                            DateTime dtx = ProductionLoggingBLL.GetLastSpeedDateTime(line, glove);
                            DateTime dt = DateTime.MinValue;
                            if (DateTime.TryParse(theDateTime, out dt))
                            {
                                if (dt > dtx)
                                    ret = true;
                            }
                        }
                    }
            }
            catch (Exception ex)
            {
                GlobalMessageBox.Show(ex.ToString());
            }
            return ret;
        }

        private void ClearForm()
        {

        }

        private bool SaveForm()
        {
            return ProductionLoggingBLL.SaveLineSpeedData(cmbLineAdd.SelectedItem.ToString().Trim(), cmbGloveAdd.SelectedItem.ToString().Trim(),
                        Convert.ToInt32(tbSpeed.Text), Convert.ToDateTime(dtpDate.Value.ToString("yyyy-MM-dd") + " " + dtpTime.Value.ToString("HH:mm:ss")),
                        Convert.ToInt32(WorkStationDTO.GetInstance().WorkStationId));
        }

        private void UpdateMinDate()
        {
            string line = cmbLineAdd.SelectedItem.ToString().Trim();
            string glove = cmbGloveAdd.SelectedItem.ToString().Trim();
            DateTime dtx = ProductionLoggingBLL.GetLastSpeedDateTime(line, glove);
            dtpDate.MinDate = dtx;
            dtpDate.MaxDate = ServerCurrentDateTime.AddDays(Constants.THIRTY);
        }
        #endregion

        #region Event

        private void cmbGloveAdd_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateMinDate();
        }

        private void cmbLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbLine.SelectedItem.ToString() != "")
                {
                    LoadGloveCodeSelectedLine(cmbLine.SelectedItem.ToString().Trim());
                }
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, this.Name, this.Text);
                return;
            }
        }

        private void cmbGlove_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadGloveDetails(cmbGlove.SelectedItem.ToString().Trim(), cmbLine.SelectedItem.ToString().Trim());
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, this.Name, this.Text);
                return;
            }
        }

        private void btnSpeedAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateData())
                {
                    if (SaveForm())
                    {
                        GlobalMessageBox.Show("Data saved successfully");
                        ClearForm();
                        LoadGloveDetails(cmbGlove.SelectedItem.ToString().Trim(), cmbLine.SelectedItem.ToString().Trim());
                    }
                    else
                    {
                        GlobalMessageBox.Show("Unable to save data. Please contact MIS");
                        ClearForm();
                    }
                }
                else
                    GlobalMessageBox.Show("Please check your values again");
            }
            catch (FloorSystemException fsEX)
            {
                ExceptionLogging(fsEX, _screenName, _className, this.Name, this.Text);
                return;
            }
        }

        #endregion Event

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
        }

    }
}
