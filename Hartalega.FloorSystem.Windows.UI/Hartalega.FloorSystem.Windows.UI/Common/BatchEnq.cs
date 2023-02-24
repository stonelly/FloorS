using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.Database;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.Common
{
    public enum ConnName
    {
        FS, AX
    }

    public partial class BatchEnquiry : FormBase
    {
        public BatchEnquiry()
        {
            InitializeComponent();
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                string serial = tbPNSerial.Text;
                clearAll();

                double no;
                bool res = Double.TryParse(serial, out no);
                if ((serial.Length == 10) && (res == true))
                {
                    tbPNSerial.Text = serial;
                    tbPTSerial.Text = serial;
                    tbQCSerial.Text = serial;
                    tbSHPSerial.Text = serial;
                    tbQASerial.Text = serial;
                    tbCHGSerial.Text = serial;

                    tabPN(tbPNSerial.Text);
                    tabPT(tbPTSerial.Text);
                    tabQC(tbQCSerial.Text);
                    tabSHP(tbSHPSerial.Text);
                    tabQA(tbQASerial.Text);
                    tabCHG(tbCHGSerial.Text);
                }
                else
                {
                    MessageBox.Show("Invalid Serial Number. Please enter a valid Serial Number.", "Invalid Serial Number",MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void clearAll()
        {
            tbPnBatch.Text = "";
            tbPnBatchWeightKg.Text = "";
            tbPnBatchWeightPcs.Text = "";
            tbPnDate.Text = "";
            tbPnGlove.Text = "";
            tbPnLine.Text = "";
            tbPnShift.Text = "";
            tbPnSize.Text = "";
            tbPnTenPcsWeight.Text = "";
            tbPnTime.Text = "";
            tbPNSerial.Text = "";
            tbPnQAI.Text = "";

            tbPTBatch.Text = "";
            tbPTBatchWeightKg.Text = "";
            tbPTBatchWeightPcs.Text = "";
            tbPTDate.Text = "";
            tbPTGlove.Text = "";
            tbPTLine.Text = "";
            tbPTSerial.Text = "";
            tbPTShift.Text = "";
            tbPTSize.Text = "";
            tbPTTenPcsWeight.Text = "";
            tbPTTime.Text = "";
            tbPTWeightLossKg.Text = "";
            tbPTWeightLossPcs.Text = "";
            tbPTWeightLossPct.Text = "";

            tbQCBatchWeightKg.Text = "";
            tbQCBatchWeightPcs.Text = "";
            tbQCDate.Text = "";
            tbQCLoc.Text = "";
            tbQCRejectPcsPct.Text = "";
            tbQCRejectWeightKg.Text = "";
            tbQCRejectWeightPcs.Text = "";
            tbQCSerial.Text = "";

            tbSHPSerial.Text = "";

            tbQAGlove.Text = "";
            tbQASerial.Text = "";

            tbCHGGlove.Text = "";
            tbCHGQCType.Text = "";
            tbCHGSerial.Text = "";

            cbPnVerification.Checked = false;
            cbQCRework.Checked = false;

            dgvCHGGlove.Rows.Clear();
            dgvPTDryer.Rows.Clear();
            dgvPTWasher.Rows.Clear();
            dgvQAHotbox.Rows.Clear();
            dgvQAProtein.Rows.Clear();
            dgvQCGroup.Rows.Clear();
            dgvQCGrpName.Rows.Clear();
            dgvSHPInfo.Rows.Clear();
            dgvSHPInner.Rows.Clear();
            dgvSHPOuter.Rows.Clear();
            dgvCHGQC.Rows.Clear();

            tbPnBatch2.Text = "";
            tbPnTime2.Text = "";
            cmbSN.ResetText();
        }

        private void tabPN(string serial)
        {
            try
            {
                DataTable dt = BatchEnqDBHelper.getPNInfo(serial);
                if (dt.Rows.Count > 0)
                {
                    tbPnBatch.Text = dt.Rows[0].Field<string>(1) ?? "";
                    tbPnDate.Text = dt.Rows[0].Field<DateTime>(2).ToString("dd/MM/yyyy") ?? "";
                    tbPnTime.Text = dt.Rows[0].Field<DateTime>(2).ToString("hh:mm:ss tt") ?? "";
                    tbPnGlove.Text = dt.Rows[0].Field<string>(3) ?? "";
                    tbPnShift.Text = dt.Rows[0].Field<string>(4) ?? "";
                    tbPnLine.Text = dt.Rows[0].Field<string>(5) ?? "";
                    tbPnSize.Text = dt.Rows[0].Field<string>(6) ?? "";
                    tbPnBatchWeightKg.Text = dt.Rows[0].Field<decimal?>(7).ToString() ?? "";
                    tbPnTenPcsWeight.Text = dt.Rows[0].Field<decimal?>(8).ToString() ?? "";
                    tbPnQAI.Text = dt.Rows[0].Field<string>(9) ?? "";
                    tbPnBatchWeightPcs.Text = dt.Rows[0].Field<int?>(11).ToString() ?? "";

                    string QAIDate = dt.Rows[0].Field<DateTime?>(10).ToString() ?? "";
                    if (QAIDate != "")
                    {
                        cbPnVerification.Checked = true;
                    }

                    tbPnBatch2.Text = tbPnBatch.Text;
                    tbPnTime2.Text = tbPnTime.Text;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show("Error occured: " + ex.Message, "Please contact MIS");
                //throw new FloorSystemException(String.Format(Messages.FLOOR_DBACCESS_ERROR, "HSB_CUSTOM_IMPLEMENTATION"), Constants.DAL, ex);
            }
        }

        private void tabPT(string serial)
        {
            try
            {
                DataTable dt = BatchEnqDBHelper.getPTInfo(serial);
                DataTable dt2 = BatchEnqDBHelper.getPTDryerInfo(serial);
                DataTable dt3 = BatchEnqDBHelper.getPTWasherInfo(serial);

                if (dt.Rows.Count > 0)
                {
                    DateTime? datetime1 = dt.Rows[0].Field<DateTime?>(7);

                    tbPTBatch.Text = dt.Rows[0].Field<string>(15) ?? "";
                    tbPTBatchWeightKg.Text = dt.Rows[0].Field<decimal>(9).ToString() ?? "";
                    tbPTBatchWeightPcs.Text = Convert.ToInt32(dt.Rows[0].Field<decimal>(10)).ToString() ?? "";

                    if (datetime1.HasValue)
                    {
                        tbPTDate.Text = dt.Rows[0].Field<DateTime>(7).ToString("dd/MM/yyyy") ?? "";
                        tbPTTime.Text = dt.Rows[0].Field<DateTime>(7).ToString("hh:mm:ss tt") ?? "";
                    }

                    tbPTGlove.Text = dt.Rows[0].Field<string>(11) ?? "";
                    tbPTLine.Text = dt.Rows[0].Field<string>(5) ?? "";
                    tbPTShift.Text = dt.Rows[0].Field<string>(4) ?? "";
                    tbPTSize.Text = dt.Rows[0].Field<string>(16) ?? "";
                    tbPTTenPcsWeight.Text = dt.Rows[0].Field<decimal>(8).ToString() ?? "";
                    tbPTWeightLossKg.Text = dt.Rows[0].Field<decimal>(0).ToString() ?? "";
                    tbPTWeightLossPcs.Text = Convert.ToInt32(dt.Rows[0].Field<decimal>(1)).ToString() ?? "";
                    tbPTWeightLossPct.Text = dt.Rows[0].Field<decimal>(2).ToString() ?? "";
                }

                if (dt2.Rows.Count > 0)
                {
                    int c = 1;
                    dgvPTDryer.Rows.Clear();
                    foreach (DataRow row in dt2.Rows)
                    {
                        DateTime? datetime1 = row.Field<DateTime?>(4);
                        DateTime? datetime2 = row.Field<DateTime?>(5);

                        bool rwk = Convert.ToBoolean(row.Field<bool>(1));
                        string dryerNo = row.Field<int>(2).ToString() ?? "";
                        string hotColdCycle = row.Field<string>(7) ?? "";
                        string date = "";
                        string endTime = "";

                        if (datetime1.HasValue)
                        {
                            date = row.Field<DateTime>(4).ToString("dd/MM/yyyy hh:mm:ss tt") ?? "";
                        }
                        if (datetime2.HasValue)
                        {
                            endTime = row.Field<DateTime>(5).ToString("dd/MM/yyyy hh:mm:ss tt") ?? "";
                        }

                        string operatorId = row.Field<string>(6) ?? "";
                        dgvPTDryer.Rows.Add(c.ToString(), rwk, dryerNo, hotColdCycle, date, endTime, operatorId);
                        c++;
                    }
                }

                if (dt3.Rows.Count > 0)
                {
                    int c = 1;
                    dgvPTWasher.Rows.Clear();
                    foreach (DataRow row in dt3.Rows)
                    {
                        DateTime? datetime1 = row.Field<DateTime?>(4);
                        DateTime? datetime2 = row.Field<DateTime?>(6);

                        bool rwk = Convert.ToBoolean(row.Field<bool>(1));
                        string washerNo = row.Field<int>(2).ToString() ?? "";
                        string programNo = row.Field<int>(3).ToString() ?? "";
                        string date = "";
                        string end = "";

                        if (datetime1.HasValue)
                        {
                            date = row.Field<DateTime>(4).ToString("dd/MM/yyyy hh:mm:ss tt") ?? "";
                        }
                        if (datetime2.HasValue)
                        {
                            end = row.Field<DateTime>(6).ToString("dd/MM/yyyy hh:mm:ss tt") ?? "";
                        }

                        string operatorId = row.Field<string>(7) ?? "";
                        dgvPTWasher.Rows.Add(c.ToString(), rwk, washerNo, programNo, date, end, operatorId);
                        c++;
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show("Error occured: " + ex.Message, "Please contact MIS");
                //throw new FloorSystemException(String.Format(Messages.FLOOR_DBACCESS_ERROR, "HSB_CUSTOM_IMPLEMENTATION"), Constants.DAL, ex);
            }
        }

        private void tabSHP(string serial)
        {
            try
            {
                DataTable dt = BatchEnqDBHelper.getSHPInfo(serial);
                DataTable dt2 = BatchEnqDBHelper.getSHPInner(serial);
                DataTable dt3 = BatchEnqDBHelper.getSHPOuter(serial);

                if (dt.Rows.Count > 0)
                {
                    dgvSHPInfo.Rows.Clear();
                    foreach (DataRow row in dt.Rows)
                    {
                        string po = row.Field<string>(2) ?? "";
                        string cust = row.Field<string>(3) ?? "";
                        string brand = row.Field<string>(4) ?? "";
                        string lotNo = row.Field<string>(1) ?? "";
                        dgvSHPInfo.Rows.Add(po, cust, brand, lotNo);
                    }
                }

                if (dt2.Rows.Count > 0)
                {
                    dgvSHPInner.Rows.Clear();
                    int c = 1;
                    foreach (DataRow row in dt2.Rows)
                    {
                        string lotNo = row.Field<string>(1) ?? "";
                        dgvSHPInner.Rows.Add(c.ToString(), lotNo);
                        c++;
                    }
                }
                if (dt3.Rows.Count > 0)
                {
                    int c = 1;
                    dgvSHPOuter.Rows.Clear();
                    foreach (DataRow row in dt3.Rows)
                    {
                        string Outer = row.Field<string>(0) ?? "";
                        string CartonNo = row.Field<string>(1) ?? "";
                        string SO = row.Field<string>(2) ?? "";
                        string Brand = row.Field<string>(3) ?? "";
                        dgvSHPOuter.Rows.Add(c.ToString(), Outer, CartonNo, SO, Brand);
                        c++;
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show("Error occured: " + ex.Message, "Please contact MIS");
                //throw new FloorSystemException(String.Format(Messages.FLOOR_DBACCESS_ERROR, "HSB_CUSTOM_IMPLEMENTATION"), Constants.DAL, ex);
            }
        }

        private void tabQA(string serial)
        {
            try
            {
                DataTable dt = BatchEnqDBHelper.getQAInfo(serial);
                DataTable dt2 = BatchEnqDBHelper.getQAProtein(serial);
                DataTable dt3 = BatchEnqDBHelper.getQAHotbox(serial);
                if (dt.Rows.Count > 0)
                {
                    string glove = dt.Rows[0].Field<string>(0);
                    tbQAGlove.Text = glove;
                }

                if (dt2.Rows.Count > 0)
                {
                    dgvQAProtein.Rows.Clear();
                    int c = 1;
                    foreach (DataRow row in dt2.Rows)
                    {
                        string date = row.Field<DateTime>(3).ToString("dd/MM/yyyy") ?? "";
                        string reference = row.Field<Decimal>(1).ToString() ?? "";
                        string testerID = row.Field<string>(2).ToString() ?? "";
                        string gloveWeight = row.Field<Decimal>(4).ToString() ?? "";
                        string proteinContent = row.Field<Decimal>(5).ToString() ?? "";
                        bool proteinResult = Convert.ToBoolean(row.Field<bool>(6));
                        string res = "";
                        if (proteinResult) { res = "Pass"; } else { res = "Fail"; }
                        dgvQAProtein.Rows.Add(c.ToString(), date, reference, testerID, gloveWeight, proteinContent, res);
                        c++;
                    }
                }

                if (dt3.Rows.Count > 0)
                {
                    int c = 1;
                    dgvQAHotbox.Rows.Clear();
                    foreach (DataRow row in dt3.Rows)
                    {
                        string date = row.Field<DateTime>(2).ToString("dd/MM/yyyy") ?? "";
                        string reference = row.Field<Decimal>(1).ToString() ?? "";
                        string testerID = row.Field<string>(3).ToString() ?? "";
                        bool proteinResult = Convert.ToBoolean(row.Field<bool>(4));
                        string res = "";
                        if (proteinResult) { res = "Pass"; } else { res = "Fail"; }
                        dgvQAHotbox.Rows.Add(c.ToString(), date, reference, testerID, res);
                        c++;
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show("Please contact MIS", "Error occured: " + ex.Message);
                //throw new FloorSystemException(String.Format(Messages.FLOOR_DBACCESS_ERROR, "HSB_CUSTOM_IMPLEMENTATION"), Constants.DAL, ex);
            }
        }

        private void tabCHG(string serial)
        {
            try
            {
                DataTable dt = BatchEnqDBHelper.getCHGData(serial);
                DataTable dt2 = BatchEnqDBHelper.getCHGInfo(serial);
                DataTable dt3 = BatchEnqDBHelper.getCHGQC(serial);
                if (dt.Rows.Count > 0)
                {
                    string glove = dt.Rows[0].Field<string>(0);
                    string qc = dt.Rows[0].Field<string>(1);
                    tbCHGGlove.Text = glove;
                    tbCHGQCType.Text = qc;
                }
                if (dt2.Rows.Count > 0)
                {
                    dgvCHGGlove.Rows.Clear();
                    int c = 1;
                    foreach (DataRow row in dt2.Rows)
                    {
                        string date = row.Field<DateTime>(1).ToString("dd/MM/yyyy") ?? "";
                        string time = row.Field<DateTime>(1).ToString("hh:mm:ss tt") ?? "";
                        string location = row.Field<string>(2).ToString() ?? "";
                        string fromGlove = row.Field<string>(3).ToString() ?? "";
                        string toGlove = row.Field<string>(4).ToString() ?? "";
                        dgvCHGGlove.Rows.Add(c.ToString(), date, time, location, fromGlove, toGlove);
                        c++;
                    }
                }
                if (dt3.Rows.Count > 0)
                {
                    dgvCHGQC.Rows.Clear();
                    int c = 1;
                    foreach (DataRow row in dt3.Rows)
                    {
                        string datetime = row.Field<DateTime>(0).ToString("dd/MM/yyyy hh:mm:ss tt") ?? "";
                        string location = row.Field<string>(4).ToString() ?? "";
                        string module = row.Field<string>(5).ToString() ?? "";
                        string QC = row.Field<string>(1).ToString() ?? "";
                        string desc = row.Field<string>(2).ToString() ?? "";
                        string qiTestResult = row.Field<string>(6).ToString() ?? "";
                        dgvCHGQC.Rows.Add(c.ToString(), datetime, location, module, QC, desc, qiTestResult);
                        c++;
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show("Error occured: " + ex.Message, "Please contact MIS");
                //throw new FloorSystemException(String.Format(Messages.FLOOR_DBACCESS_ERROR, "HSB_CUSTOM_IMPLEMENTATION"), Constants.DAL, ex);
            }
        }

        private void tabQC(string serial)
        {
            try
            {
                DataTable dt = BatchEnqDBHelper.getQCInfo(serial);
                if (dt.Rows.Count > 0)
                {
                    string qcid2 = "";
                    DateTime? datetime = dt.Rows[0].Field<DateTime?>(1);
                    string qcdate = "";

                    if (datetime.HasValue)
                    {
                        qcdate = datetime.ToString() ?? "";
                    }

                    //string location = dt.Rows[0].Field<string>(2).ToString() ?? "";
                    string location = dt.Rows[0]["LocationAreaCode"].ToString();
                    int? rework = dt.Rows[0].Field<int?>(3) ?? 0;
                    decimal? batchWeight = dt.Rows[0].Field<decimal?>(4) ?? 0;
                    int? batchPcs = dt.Rows[0].Field<int?>(5) ?? 0;
                    decimal? rejectWeight = dt.Rows[0].Field<decimal?>(9) ?? 0;
                    decimal? rejectPcs = dt.Rows[0].Field<decimal?>(10) ?? 0;
                    decimal? rejectPct = dt.Rows[0].Field<decimal?>(11) ?? 0;

                    tbQCDate.Text = qcdate;
                    tbQCLoc.Text = location;
                    if (rework > 0) { cbQCRework.Checked = true; } else { cbQCRework.Checked = false; }
                    tbQCBatchWeightKg.Text = batchWeight.ToString() ?? "";
                    tbQCBatchWeightPcs.Text = batchPcs.ToString() ?? "";
                    tbQCRejectWeightKg.Text = rejectWeight.ToString() ?? "";
                    tbQCRejectWeightPcs.Text = rejectPcs.ToString() ?? "";
                    tbQCRejectPcsPct.Text = rejectPct.ToString() ?? "";

                    dgvQCGroup.Rows.Clear();
                    foreach (DataRow row in dt.Rows)
                    {
                        int? qcid = row.Field<int?>(6);
                        string qctype = row.Field<string>(12);
                        int? qcgroup = row.Field<int?>(7);
                        DateTime? dtx = row.Field<DateTime?>(1);
                        string status = row.Field<string>(13);
                        string dtxdate = string.Empty;
                        if (dtx.HasValue)
                        {
                            dtxdate = dtx.ToString() ?? "";
                        }

                        if (qcid.HasValue)
                        {
                            qcid2 = qcid.ToString();
                            dgvQCGroup.Rows.Add(qcid, qctype, qcgroup, status);
                        }
                    }

                    if (qcid2 != "")
                    {
                        DataTable dt2 = BatchEnqDBHelper.getQCMember(qcid2);
                        dgvQCGrpName.Rows.Clear();
                        if (dt2.Rows.Count > 0)
                        {
                            int c = 1;
                            foreach (DataRow row in dt2.Rows)
                            {
                                string id = row.Field<string>(0) ?? "";
                                string name = row.Field<string>(1) ?? "";
                                dgvQCGrpName.Rows.Add(c.ToString(), id, name);
                                c++;
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show("Error occured: " + ex.Message, "Please contact MIS");
            }
        }

        private void dgvQCGroup_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    string serial = tbQCSerial.Text;
                    string qcid = dgvQCGroup.Rows[e.RowIndex].Cells[0].FormattedValue.ToString();
                    DataTable dt = BatchEnqDBHelper.getQCInfo(serial, qcid);
                    DataTable dt2 = BatchEnqDBHelper.getQCMember(qcid);

                    if (dt.Rows.Count > 0)
                    {
                        string qcdate = dt.Rows[0].Field<DateTime>(1).ToString("dd/MM/yyyy hh:mm:ss tt") ?? "";
                        string location = dt.Rows[0].Field<string>(2).ToString() ?? "";
                        int rework = dt.Rows[0].Field<int>(3);
                        string batchWeight = dt.Rows[0].Field<decimal>(4).ToString() ?? "";
                        string batchPcs = dt.Rows[0].Field<int>(5).ToString() ?? "";
                        string rejectWeight = dt.Rows[0].Field<decimal>(9).ToString() ?? "";
                        string rejectPcs = dt.Rows[0].Field<decimal>(10).ToString() ?? "";
                        string rejectPct = dt.Rows[0].Field<decimal>(11).ToString() ?? "";

                        tbQCDate.Text = qcdate;
                        tbQCLoc.Text = location;
                        if (rework > 0) { cbQCRework.Checked = true; } else { cbQCRework.Checked = false; }
                        tbQCBatchWeightKg.Text = batchWeight;
                        tbQCBatchWeightPcs.Text = batchPcs;
                        tbQCRejectWeightKg.Text = rejectWeight;
                        tbQCRejectWeightPcs.Text = rejectPcs;
                        tbQCRejectPcsPct.Text = rejectPct;
                    }

                    if (dt2.Rows.Count > 0)
                    {
                        dgvQCGrpName.Rows.Clear();
                        if (dt2.Rows.Count > 0)
                        {
                            int c = 1;
                            foreach (DataRow row in dt2.Rows)
                            {
                                string id = row.Field<string>(0) ?? "";
                                string name = row.Field<string>(1) ?? "";
                                dgvQCGrpName.Rows.Add(c.ToString(), id, name);
                                c++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show("Error occured: " + ex.Message, "Please contact MIS");
            }
        }

        private void rbIntLotNo_CheckedChanged(object sender, EventArgs e)
        {
            if (rbIntLotNo.Checked == true)
                panel10.Visible = true;
            else
                panel10.Visible = false;
            clearAll();
        }

        private void rbSN_CheckedChanged(object sender, EventArgs e)
        {
            tbIntLotNumber.Text = string.Empty;
            cmbSN.DataSource = null;
            cmbSN.Items.Clear();
            clearAll();
        }

        private void cmbSN_SelectionChangeCommited(object sender, EventArgs e)
        {
            if (this.cmbSN.SelectedValue != null)
            {
                string serial = this.cmbSN.SelectedValue.ToString();
                clearAll();

                double no;
                bool res = Double.TryParse(serial, out no);
                if ((serial.Length == 10) && (res == true))
                {
                    tbPNSerial.Text = serial;
                    tbPTSerial.Text = serial;
                    tbQCSerial.Text = serial;
                    tbSHPSerial.Text = serial;
                    tbQASerial.Text = serial;
                    tbCHGSerial.Text = serial;

                    tabPN(tbPNSerial.Text);
                    tabPT(tbPTSerial.Text);
                    tabQC(tbQCSerial.Text);
                    tabSHP(tbSHPSerial.Text);
                    tabQA(tbQASerial.Text);
                    tabCHG(tbCHGSerial.Text);
                }
            }
        }

        private void tbIntLotNumber_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                clearAll();
                cmbSN.SelectedItem = null;
                string intLotNumber = tbIntLotNumber.Text;
                cmbSN.SelectedIndexChanged -= cmbSN_SelectionChangeCommited;
                List<DropdownDTO> lstSNList = BatchEnqDBHelper.getSerialNumberByInternalLotNumber(intLotNumber);
                cmbSN.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cmbSN.AutoCompleteSource = AutoCompleteSource.ListItems;
                cmbSN.BindComboBox(lstSNList, true);
                cmbSN.SelectedIndexChanged += cmbSN_SelectionChangeCommited;
            }
        }
    }

    public static class BatchEnqDBHelper
    {
        private static SqlConnection _conn;
        private static string _floorDBconnstring;
        private const string _subsystem = Constants.DAL;
        private static string spName = "USP_BatchEnquiry";
        private static string spName2 = "USP_BatchEnqGetQCMember";

        static BatchEnqDBHelper()
        {
            _floorDBconnstring = EncryptDecrypt.GetDecryptedString(ConfigurationManager.ConnectionStrings["FSDB"].ToString(),"hidden");
        }
        private static void Init(ConnName connName)
        {
            try
            {
                if (_conn == null || _conn.State != ConnectionState.Open)
                {
                    switch (connName)
                    {
                        case ConnName.FS:
                            _conn = new SqlConnection(_floorDBconnstring);
                            break;
                    }
                    _conn.Open();
                }
            }

            catch (Exception)
            {
                throw;
            }
        }

        public static void TearDown()
        {
            try
            {
                if (_conn != null)
                {
                    if (_conn.State == ConnectionState.Open)
                    {
                        _conn.Close();
                    }
                    _conn = null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region DataTable
        public static DataTable getPNInfo(string serial, ConnName connectionName = ConnName.FS)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            try
            {
                Init(connectionName);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = _conn;
                cmd.CommandText = spName;
                cmd.Parameters.Add(new SqlParameter("@Section", "PN"));
                cmd.Parameters.Add(new SqlParameter("@Serial", serial));
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show("Error occured: " + ex.Message, "Please contact MIS");
                //throw new FloorSystemException(String.Format(Messages.FLOOR_DBACCESS_ERROR, "HSB_CUSTOM_IMPLEMENTATION"), _subsystem, ex);
            }
            finally
            {
                TearDown();
            }
            return dt;
        }

        public static DataTable getPTInfo(string serial, ConnName connectionName = ConnName.FS)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            try
            {
                Init(connectionName);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = _conn;
                cmd.CommandText = spName;
                cmd.Parameters.Add(new SqlParameter("@Section", "PT"));
                cmd.Parameters.Add(new SqlParameter("@Serial", serial));
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show("Error occured: " + ex.Message, "Please contact MIS");
                //throw new FloorSystemException(String.Format(Messages.FLOOR_DBACCESS_ERROR, "HSB_CUSTOM_IMPLEMENTATION"), _subsystem, ex);
            }
            finally
            {
                TearDown();
            }
            return dt;
        }

        public static DataTable getPTWasherInfo(string serial, ConnName connectionName = ConnName.FS)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            try
            {
                Init(connectionName);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = _conn;
                cmd.CommandText = spName;
                cmd.Parameters.Add(new SqlParameter("@Section", "Wash"));
                cmd.Parameters.Add(new SqlParameter("@Serial", serial));
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show("Error occured: " + ex.Message, "Please contact MIS");
                //throw new FloorSystemException(String.Format(Messages.FLOOR_DBACCESS_ERROR, "HSB_CUSTOM_IMPLEMENTATION"), _subsystem, ex);
            }
            finally
            {
                TearDown();
            }
            return dt;
        }

        public static DataTable getPTDryerInfo(string serial, ConnName connectionName = ConnName.FS)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            try
            {
                Init(connectionName);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = _conn;
                cmd.CommandText = spName;
                cmd.Parameters.Add(new SqlParameter("@Section", "Dryer"));
                cmd.Parameters.Add(new SqlParameter("@Serial", serial));
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show("Error occured: " + ex.Message, "Please contact MIS");
                //throw new FloorSystemException(String.Format(Messages.FLOOR_DBACCESS_ERROR, "HSB_CUSTOM_IMPLEMENTATION"), _subsystem, ex);
            }
            finally
            {
                TearDown();
            }
            return dt;
        }

        public static DataTable getQCInfo(string serial, string group = "", ConnName connectionName = ConnName.FS)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            try
            {
                Init(connectionName);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = _conn;
                cmd.CommandText = spName;
                cmd.Parameters.Add(new SqlParameter("@Section", "QC"));
                cmd.Parameters.Add(new SqlParameter("@Serial", serial));
                if (group != "")
                {
                    cmd.Parameters.Add(new SqlParameter("@var", group));
                }
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show("Error occured: " + ex.Message, "Please contact MIS");
                //throw new FloorSystemException(String.Format(Messages.FLOOR_DBACCESS_ERROR, "HSB_CUSTOM_IMPLEMENTATION"), _subsystem, ex);
            }
            finally
            {
                TearDown();
            }
            return dt;
        }

        public static DataTable getSHPInfo(string serial, ConnName connectionName = ConnName.FS)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            try
            {
                Init(connectionName);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = _conn;
                cmd.CommandText = spName;
                cmd.Parameters.Add(new SqlParameter("@Section", "SHP"));
                cmd.Parameters.Add(new SqlParameter("@Serial", serial));
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show("Error occured: " + ex.Message, "Please contact MIS");
                //throw new FloorSystemException(String.Format(Messages.FLOOR_DBACCESS_ERROR, "HSB_CUSTOM_IMPLEMENTATION"), _subsystem, ex);
            }
            finally
            {
                TearDown();
            }
            return dt;
        }

        public static DataTable getSHPInner(string serial, ConnName connectionName = ConnName.FS)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            try
            {
                Init(connectionName);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = _conn;
                cmd.CommandText = spName;
                cmd.Parameters.Add(new SqlParameter("@Section", "Inner"));
                cmd.Parameters.Add(new SqlParameter("@Serial", serial));
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show("Error occured: " + ex.Message, "Please contact MIS");
                //throw new FloorSystemException(String.Format(Messages.FLOOR_DBACCESS_ERROR, "HSB_CUSTOM_IMPLEMENTATION"), _subsystem, ex);
            }
            finally
            {
                TearDown();
            }
            return dt;
        }

        public static DataTable getSHPOuter(string serial, ConnName connectionName = ConnName.FS)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            try
            {
                Init(connectionName);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = _conn;
                cmd.CommandText = spName;
                cmd.Parameters.Add(new SqlParameter("@Section", "Outer"));
                cmd.Parameters.Add(new SqlParameter("@Serial", serial));
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show("Error occured: " + ex.Message, "Please contact MIS");
                //throw new FloorSystemException(String.Format(Messages.FLOOR_DBACCESS_ERROR, "HSB_CUSTOM_IMPLEMENTATION"), _subsystem, ex);
            }
            finally
            {
                TearDown();
            }
            return dt;
        }

        public static DataTable getQAInfo(string serial, ConnName connectionName = ConnName.FS)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            try
            {
                Init(connectionName);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = _conn;
                cmd.CommandText = spName;
                cmd.Parameters.Add(new SqlParameter("@Section", "QA"));
                cmd.Parameters.Add(new SqlParameter("@Serial", serial));
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured: " + ex.Message, "Please contact MIS");
                //throw new FloorSystemException(String.Format(Messages.FLOOR_DBACCESS_ERROR, "HSB_CUSTOM_IMPLEMENTATION"), _subsystem, ex);
            }
            finally
            {
                TearDown();
            }
            return dt;
        }

        public static DataTable getQAProtein(string serial, ConnName connectionName = ConnName.FS)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            try
            {
                Init(connectionName);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = _conn;
                cmd.CommandText = spName;
                cmd.Parameters.Add(new SqlParameter("@Section", "Prtn"));
                cmd.Parameters.Add(new SqlParameter("@Serial", serial));
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured: " + ex.Message, "Please contact MIS");
                //throw new FloorSystemException(String.Format(Messages.FLOOR_DBACCESS_ERROR, "HSB_CUSTOM_IMPLEMENTATION"), _subsystem, ex);
            }
            finally
            {
                TearDown();
            }
            return dt;
        }

        public static DataTable getQAHotbox(string serial, ConnName connectionName = ConnName.FS)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            try
            {
                Init(connectionName);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = _conn;
                cmd.CommandText = spName;
                cmd.Parameters.Add(new SqlParameter("@Section", "Htbx"));
                cmd.Parameters.Add(new SqlParameter("@Serial", serial));
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured: " + ex.Message, "Please contact MIS");
                //throw new FloorSystemException(String.Format(Messages.FLOOR_DBACCESS_ERROR, "HSB_CUSTOM_IMPLEMENTATION"), _subsystem, ex);
            }
            finally
            {
                TearDown();
            }
            return dt;
        }

        public static DataTable getCHGInfo(string serial, ConnName connectionName = ConnName.FS)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            try
            {
                Init(connectionName);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = _conn;
                cmd.CommandText = spName;
                cmd.Parameters.Add(new SqlParameter("@Section", "CHG"));
                cmd.Parameters.Add(new SqlParameter("@Serial", serial));
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured: " + ex.Message, "Please contact MIS");
                //throw new FloorSystemException(String.Format(Messages.FLOOR_DBACCESS_ERROR, "HSB_CUSTOM_IMPLEMENTATION"), _subsystem, ex);
            }
            finally
            {
                TearDown();
            }
            return dt;
        }

        public static DataTable getCHGData(string serial, ConnName connectionName = ConnName.FS)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            try
            {
                Init(connectionName);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = _conn;
                cmd.CommandText = spName;
                cmd.Parameters.Add(new SqlParameter("@Section", "CHI"));
                cmd.Parameters.Add(new SqlParameter("@Serial", serial));
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured: " + ex.Message, "Please contact MIS");
                //throw new FloorSystemException(String.Format(Messages.FLOOR_DBACCESS_ERROR, "HSB_CUSTOM_IMPLEMENTATION"), _subsystem, ex);
            }
            finally
            {
                TearDown();
            }
            return dt;
        }

        public static DataTable getCHGQC(string serial, ConnName connectionName = ConnName.FS)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            try
            {
                Init(connectionName);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = _conn;
                cmd.CommandText = spName;
                cmd.Parameters.Add(new SqlParameter("@Section", "qch"));
                cmd.Parameters.Add(new SqlParameter("@Serial", serial));
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured: " + ex.Message, "Please contact MIS");
                //throw new FloorSystemException(String.Format(Messages.FLOOR_DBACCESS_ERROR, "HSB_CUSTOM_IMPLEMENTATION"), _subsystem, ex);
            }
            finally
            {
                TearDown();
            }
            return dt;
        }

        public static DataTable getQCMember(string qcid, ConnName connectionName = ConnName.FS)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            try
            {
                Init(connectionName);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = _conn;
                cmd.CommandText = spName2;
                cmd.Parameters.Add(new SqlParameter("@QCId", qcid));
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured: " + ex.Message, "Please contact MIS");
                //throw new FloorSystemException(String.Format(Messages.FLOOR_DBACCESS_ERROR, "HSB_CUSTOM_IMPLEMENTATION"), _subsystem, ex);
            }
            finally
            {
                TearDown();
            }
            return dt;
        }
        #endregion

        #region Get Serial Number List
        public static List<DropdownDTO> getSerialNumberByInternalLotNumber(string intLotNumber)
        {
            List<DropdownDTO> list = new List<DropdownDTO>();
            List<FloorSqlParameter> lstFP = new List<FloorSqlParameter>();
            lstFP.Add(new FloorSqlParameter("@internalLotNumber", intLotNumber));
            using (DataTable dt = FloorDBAccess.ExecuteDataSet("usp_GetSerialNoListByInternalLotNo", lstFP).Tables[0].DefaultView.ToTable(true))
            {
                try
                {
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        list = (from DataRow row in dt.Rows
                                select new DropdownDTO
                                {
                                    IDField = FloorDBAccess.GetString(row, "SerialNumber"),
                                    DisplayField = FloorDBAccess.GetString(row, "SerialNumber")
                                }).ToList();
                    }
                    else
                    {
                        list = new List<DropdownDTO>();
                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.GETSNLIST_EXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return list;
        }
        #endregion
    }
}
