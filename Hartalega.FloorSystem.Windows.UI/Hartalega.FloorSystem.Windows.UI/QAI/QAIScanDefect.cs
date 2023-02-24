#region using

using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#endregion

namespace Hartalega.FloorSystem.Windows.UI.QAI
{
    public partial class QAIScanDefect : FormBase
    {
        #region Variables

        private static string _screenName = Constants.QAI_DEFECT_SCREENS;
        private static string _className = "QAIScanDefect";
        private TextBox[] _text = new TextBox[Constants.QAI_DEFECT_KEYSTORKE_COUNT];
        private Label[] _lab = new Label[Constants.QAI_DEFECT_KEYSTORKE_COUNT];
        private QAIDefectType _quiefectType;
        private List<QAIDefectType> _defectsCategory = null;
        private List<Control> _controlList;
        private bool _isCleared = false;
        private int _nextSeq = 0;
        private int _prevSeq = 0;
        private TextBox _textBox1;
        private Label _label1;
        private int _defectRowCount;
        private QAIDTO _qaidto { get; set; }
        private bool _hasChild = false;
        #endregion

        #region Public Variables
        public Constants.QAIPageTransition _DefectTranistion { get; set; }
        #endregion

        #region Load Form

        public QAIScanDefect(QAIDTO qaidto)
        {
            _qaidto = qaidto;
            _defectRowCount = (Constants.QAI_DEFECT_KEYSTORKE_COUNT % 2) == 0 ? (Constants.QAI_DEFECT_KEYSTORKE_COUNT / 2) : (Constants.QAI_DEFECT_KEYSTORKE_COUNT / 2) + 1;
            InitializeComponent();
            try
            {
                if (_qaidto.ScreenName == Constants.QAIScreens.EditDefects)
                {
                    _defectsCategory = QAIBLL.GetQAIDefectTypeList(_qaidto.SerialNo);
                }
                else
                {
                    _defectsCategory = QAIBLL.GetQAIDefectTypeList();
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "QAIScanDefect_Load", null);
                return;
            }
            BindBasicInfo();
            LoadForm();
            _textBox1.Enabled = false;
            _label1.Anchor = (AnchorStyles.Right | AnchorStyles.Top);
            _textBox1.Anchor = (AnchorStyles.Left | AnchorStyles.Top);
            _textBox1.BackColor = Color.White;
        }

        private void QAIScanDefect_Load(object sender, EventArgs e)
        {


        }

        private void BindBasicInfo()
        {
            txtBatchNo.Text = _qaidto.BatchNo;
            txtSerialNo.Text = _qaidto.SerialNo;
            txtQaiInspectorId.Text = _qaidto.InspectorId;
            txtQaiInspectorName.Text = _qaidto.QaiInspectorName;
        }

        # endregion

        #region Event Handlers

        private void text_Click(object sender, KeyEventArgs e)
        {
            TextBox l = (sender) as TextBox;
            string ch = l.Name;
            string keystroke = string.Empty;
            if ((e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9) || (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9))
            {
                if ((e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9))
                {
                    keystroke = Convert.ToString((int)e.KeyValue - (int)Keys.D0);
                }
                if ((e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9))
                {
                    keystroke = Convert.ToString((int)e.KeyValue - (int)Keys.NumPad0);
                }
            }
            else
            {
                keystroke = Convert.ToString(e.KeyCode).ToLower();
                // handle special character /\[]
                keystroke = QAIBLL.KeyStokeReturnChar(keystroke);
            }

            if (keystroke == ch.ToLower())
            {
                if (string.IsNullOrEmpty(l.Text))
                {
                    e.SuppressKeyPress = true;
                    l.Text = ModifierKeys == Keys.Control ? string.Empty : "1";
                }
                else
                {
                    e.SuppressKeyPress = true;
                    if (ModifierKeys == Keys.Control)
                    {
                        int count = Convert.ToInt32(l.Text) - 1;
                        l.Text = count < 1 ? string.Empty : Convert.ToString(count);
                    }
                    else
                    {
                        l.Text = (Convert.ToInt32(l.Text) + 1).ToString();
                    }

                }

                foreach (QAIDefectDTO defct in _quiefectType.DefectList)
                {
                    if (defct.KeyStroke == Convert.ToChar(ch))
                    {
                        defct.Count = string.IsNullOrEmpty(l.Text) ? 0 : Convert.ToInt32(l.Text);
                    }
                }
                int total = (from c in _quiefectType.DefectList
                             select c.Count).Sum();
                _textBox1.Text = total < 1 ? string.Empty : Convert.ToString(total);
            }
            else
            {
                e.SuppressKeyPress = true;
            }
        }

        private void QAIScanDefect_KeyDown(object sender, KeyEventArgs e)
        {
            QAIDefectDTO keyPressEvent = new QAIDefectDTO();

            string keystroke = string.Empty;

            if ((e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9) || (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9))
            {
                if ((e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9))
                {
                    keystroke = Convert.ToString((int)e.KeyValue - (int)Keys.D0);
                }
                if ((e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9))
                {
                    keystroke = Convert.ToString((int)e.KeyValue - (int)Keys.NumPad0);
                }

                keyPressEvent = (from c in _quiefectType.DefectList
                                 where Convert.ToString(c.KeyStroke).ToLower() == keystroke.ToLower()
                                 select c).FirstOrDefault();

                _hasChild = (from c in _quiefectType.DefectList
                             where Convert.ToString(c.KeyStroke).ToLower() == keystroke.ToLower() && c.HasChild
                             select c).Count() > 0;
            }
            // A-Z
            else if (e.KeyCode >= Keys.A && e.KeyCode <= Keys.Z)
            {
                keyPressEvent = (from c in _quiefectType.DefectList
                                 where Convert.ToString(c.KeyStroke).ToLower() == Convert.ToString(e.KeyCode).ToLower()
                                 select c).FirstOrDefault();

                _hasChild = (from c in _quiefectType.DefectList
                             where Convert.ToString(c.KeyStroke).ToLower() == Convert.ToString(e.KeyCode).ToLower() && c.HasChild
                             select c).Count() > 0;
            }
            else// Special Charactor : /\[];',.
            {
                keyPressEvent = (from c in _quiefectType.DefectList
                                 where Convert.ToString(c.KeyStrokeAltName).ToLower() == (Convert.ToString(e.KeyCode).ToLower())
                                 select c).FirstOrDefault();

                _hasChild = (from c in _quiefectType.DefectList
                             where Convert.ToString(c.KeyStrokeAltName).ToLower() == (Convert.ToString(e.KeyCode).ToLower()) && c.HasChild
                             select c).Count() > 0;
            }

            if (_hasChild)
            {
                if (_qaidto.Defects == null)
                    _qaidto.Defects = _defectsCategory.Where(p => p.DefectCategory == _quiefectType.DefectCategory && p.Sequence == _quiefectType.Sequence).ToList();

                string strKeyCode = Convert.ToString(e.KeyCode);
                if (!string.IsNullOrEmpty(keystroke))
                    strKeyCode = keystroke;

                QAIScanDefectPosition defectPosition = new QAIScanDefectPosition(_qaidto, strKeyCode, _quiefectType.Sequence);
                defectPosition.ShowDialog();
                int total = defectPosition.GetTotal();
                UpdateDefectPostion(total, strKeyCode);

                return;
            }

            if (keyPressEvent != null)
            {
                TextBox txtkeystroke = (TextBox)this.Controls.Find(Convert.ToString(keyPressEvent.KeyStroke), true).FirstOrDefault();
                if (txtkeystroke != null)
                {
                    text_Click(txtkeystroke, e);
                }
            }
            if (keyPressEvent == null)
            {

                if (e.KeyCode == Keys.Subtract || e.KeyCode == Keys.OemMinus)
                {
                    tableLayoutPanel1.Visible = false;
                    if (_prevSeq > 0)
                    {
                        GoToNextDefectCategory();
                        LoadForm(_prevSeq);
                    }
                    else
                    {
                        this.Close();
                    }
                    tableLayoutPanel1.Visible = true;
                }

                if (e.KeyCode == Keys.Add || e.KeyCode == Keys.Oemplus)
                {
                    tableLayoutPanel1.Visible = false;
                    if (_nextSeq <= _defectsCategory.Count)
                    {

                        GoToNextDefectCategory();
                        LoadForm(_nextSeq);
                    }
                    else
                    {
                        GoToNextDefectCategory();
                        DefectSummary defectsummary = new DefectSummary(_qaidto);
                        _DefectTranistion = defectsummary._SummaryTranistion;
                        defectsummary.ShowDialog();
                        QAINavigation(defectsummary._SummaryTranistion);
                    }
                    tableLayoutPanel1.Visible = true;
                }
                if (e.KeyCode == Keys.Delete)
                {
                    if (!_isCleared)
                    {
                        ClearAllTextBoxes();

                    }
                }
                if (e.KeyCode == Keys.End)
                {
                    GoToNextDefectCategory();
                    if (!QAIBLL.HasDefects(_qaidto.Defects))
                    {
                        _qaidto.IsStraingPack = true;
                        //Below commented code will be removed after CFD is updated.
                        /*Defect Number 11188, CFD has to be changed to mainitain selected Qc type when END key is pressed.
                         List<DropdownDTO> qctypelst = CommonBLL.GetQCType();
                        _qaidto.QCType = (from qc in qctypelst
                                          where qc.IDField.Trim().ToLower() == Convert.ToString(Hartalega.FloorSystem.Framework.Constants.STRAIGHT_PACK).ToLower()
                                          select qc.DisplayField).FirstOrDefault();
                        _qaidto.Defects = new List<QAIDefectType>();*/

                        DefectSummary defectsummary = new DefectSummary(_qaidto);
                        _DefectTranistion = defectsummary._SummaryTranistion;
                        defectsummary.ShowDialog();
                        QAINavigation(defectsummary._SummaryTranistion);
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.NON_STRAIGHT_PACK_QAI, Messages.INFORMATION, GlobalMessageBoxButtons.OK, SystemIcons.Information);
                        return;
                    }
                }
            }
        }

        # endregion

        #region User Methods

        /// <summary>
        /// Load form with defect types
        /// </summary>
        /// <param name="sequence"></param>
        private void LoadForm(int sequence = 0)
        {
            _quiefectType = null;
            if (sequence == 0)
            {
                _quiefectType = (from c in _defectsCategory
                                 where c.IsCaptured == false
                                 orderby c.Sequence
                                 select c).FirstOrDefault();
                _nextSeq = _quiefectType.Sequence + 1;
                _prevSeq = _quiefectType.Sequence - 1;
                BuildTable(_quiefectType.DefectList.Count);
                BindDefectsTypes();
            }
            else if (sequence > 0 && sequence <= _defectsCategory.Count)
            {
                ClearTableLayout();
                _quiefectType = (from c in _defectsCategory
                                 where c.Sequence == sequence
                                 select c).FirstOrDefault();
                _nextSeq = _quiefectType.Sequence + 1;
                _prevSeq = _quiefectType.Sequence - 1;
                BuildTable(_quiefectType.DefectList.Count);
                BindDefectsTypes();
            }
        }

        /// <summary>
        /// Bind Defects Types
        /// </summary>
        private void BindDefectsTypes()
        {
            Font font = new Font("Microsoft Sans Serif", 18.0f,
                       FontStyle.Regular);

            _controlList = new List<Control>();
            _isCleared = false;
            int i = 0;
            try
            {
                QAIDefectGroupBoxSecond.Text = _quiefectType.DefectCategory;
                this.Text = _qaidto.ScreenTitle + "- " + _quiefectType.DefectCategory;
                QAIDefectGroupBoxMain.Text = _qaidto.ScreenTitle + "- " + _quiefectType.DefectCategory;
                bool columnAlternate = true;

                foreach (QAIDefectDTO defct in _quiefectType.DefectList)
                {
                    if (columnAlternate)
                    {
                        _lab[i] = new Label();
                        _lab[i].Name = "lbl" + Convert.ToString(defct.KeyStroke) + i.ToString();
                        _lab[i].Text = defct.KeyStroke + " - " + defct.DefectItem + ":";
                        _lab[i].Anchor = (AnchorStyles.Left | AnchorStyles.Top);
                        _lab[i].AutoSize = true;
                        _lab[i].Width = 370;
                        _lab[i].Height = 35;
                        this.tableLayoutPanel1.Controls.Add(_lab[i], 0, i);
                        _controlList.Add(_lab[i]);


                        _text[i] = new TextBox();
                        _text[i].Name = Convert.ToString(defct.KeyStroke);
                        _text[i].Anchor = (AnchorStyles.Left | AnchorStyles.Top);
                        _text[i].Text = defct.Count == 0 ? string.Empty : Convert.ToString(defct.Count);
                        _text[i].Font = font;
                        _text[i].Width = 50;
                        _text[i].BackColor = Color.White;
                        this.tableLayoutPanel1.Controls.Add(_text[i], 1, i);
                        _text[i].KeyDown += new KeyEventHandler(text_Click);
                        _text[i].Enabled = false;
                        _controlList.Add(_text[i]);
                        columnAlternate = false;
                    }
                    else
                    {
                        _lab[i] = new Label();
                        _lab[i].Name = "lbl" + Convert.ToString(defct.KeyStroke) + i.ToString();
                        _lab[i].Text = defct.KeyStroke + " - " + defct.DefectItem + ":";
                        _lab[i].Anchor = (AnchorStyles.Left | AnchorStyles.Top);
                        _lab[i].AutoSize = true;
                        _lab[i].Width = 370;
                        _lab[i].Height = 35;
                        this.tableLayoutPanel1.Controls.Add(_lab[i], 2, i - 1);
                        _controlList.Add(_lab[i]);


                        _text[i] = new TextBox();
                        _text[i].Name = Convert.ToString(defct.KeyStroke);
                        _text[i].Anchor = (AnchorStyles.Left | AnchorStyles.Top);
                        _text[i].Text = defct.Count == 0 ? string.Empty : Convert.ToString(defct.Count);
                        _text[i].Font = font;
                        _text[i].Width = 50;
                        _text[i].BackColor = Color.White;
                        this.tableLayoutPanel1.Controls.Add(_text[i], 3, i - 1);
                        _text[i].KeyDown += new KeyEventHandler(text_Click);
                        _text[i].Enabled = false;
                        _controlList.Add(_text[i]);
                        columnAlternate = true;
                    }
                    i++;
                }
                this.tableLayoutPanel1.Controls.Add(this._label1, !columnAlternate ? 2 : 0, columnAlternate ? i + 1 : i - 1);
                this.tableLayoutPanel1.Controls.Add(this._textBox1, !columnAlternate ? 3 : 1, columnAlternate ? i + 1 : i - 1);

                int total = (from c in _quiefectType.DefectList
                             select c.Count).Sum();
                _textBox1.Text = total == 0 ? string.Empty : Convert.ToString(total);
                int tblpanelheight = tableLayoutPanel1.Height;
                QAIDefectGroupBoxSecond.Height = tblpanelheight > 481 ? tblpanelheight + 25 : 481;
                int QAIDefectGroupBoxSecondheight = QAIDefectGroupBoxSecond.Height;
                if (QAIDefectGroupBoxSecondheight > 660)
                {
                    QAIDefectGroupBoxMain.Height = QAIDefectGroupBoxSecondheight + 30;
                    tableLayoutPanel1.Height = QAIDefectGroupBoxSecondheight - 50;
                    this.AutoScrollMinSize = new Size(1022, QAIDefectGroupBoxSecond.Height + 50);
                }
                else
                {
                    QAIDefectGroupBoxMain.Height = 660;
                    this.AutoScrollMinSize = new Size(1022, QAIDefectGroupBoxMain.Height);
                }

            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindDefectsTypes", null);
                return;
            }
            catch (Exception ex)
            {
                ExceptionLogging(new FloorSystemException(ex.StackTrace), _screenName, _className, "BindDefectsTypes", null);
                return;
            }
        }

        /// <summary>
        /// Build Table
        /// </summary>
        /// <param name="rows"></param>
        private void BuildTable(int rows)
        {
            rows = (rows % 2) == 0 ? (rows / 2) : (rows / 2) + 1;
            int tblrows = rows <= _defectRowCount ? rows : _defectRowCount;
            this.tableLayoutPanel1.RowCount = tblrows + 2;
            this.tableLayoutPanel1.Height = (tblrows + 2) * 37;
            for (int i = 0; i <= tblrows + 2; i++)
            {
                this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize, 30F));
            }
        }

        /// <summary>
        /// Log to DB, Show Mesage Box to User and clear the form on exception
        /// </summary>
        /// <param name="result"></param>
        private void ExceptionLogging(FloorSystemException floorException, string screenName, string UiClassName, string uiControl, params object[] parameters)
        {
            int result = CommonBLL.LogExceptionToDB(floorException, screenName, UiClassName, uiControl, parameters);
            if (floorException.subSystem == Constants.BUSINESSLOGIC)
                GlobalMessageBox.Show("(" + result + ") - " + Messages.APPLICATIONERROR, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK, SystemIcons.Error);
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK, SystemIcons.Error);
        }

        /// <summary>
        /// Go T oNext Defect Category
        /// </summary>
        private void GoToNextDefectCategory()
        {
            _quiefectType.IsCaptured = true;
            _defectsCategory
               .Where(p => p.DefectCategory == _quiefectType.DefectCategory && p.Sequence == _quiefectType.Sequence)
               .All(p => { p.DefectList = _quiefectType.DefectList; return true; });
            _qaidto.Defects = _defectsCategory;
        }

        /// <summary>
        /// Clear Table Layout
        /// </summary>
        private void ClearTableLayout()
        {
            foreach (Control ctrl in _controlList)
            {
                tableLayoutPanel1.Controls.Remove(ctrl);
            }
            tableLayoutPanel1.RowStyles.Clear();
            _controlList = null;
            _textBox1.Text = string.Empty;
        }

        /// <summary>
        /// Clear All Text Boxes
        /// </summary>
        private void ClearAllTextBoxes()
        {
            foreach (Control ctrl in _controlList)
            {
                if (ctrl.GetType() == typeof(TextBox))
                {
                    ctrl.Text = string.Empty;
                }
            }
            _quiefectType.DefectList.All(p => { p.Count = 0; return true; });
            _textBox1.Text = string.Empty;
            _isCleared = false;
        }

        private void QAINavigation(Constants.QAIPageTransition defectTranistion)
        {
            _DefectTranistion = defectTranistion;
            if (defectTranistion == Constants.QAIPageTransition.FormClose || defectTranistion == Constants.QAIPageTransition.FormEscape)
            {
                this.Close();
            }
        }

        # endregion

        public void UpdateDefectPostion(int total, string keyCode)
        {
            var defectType = (from b in _qaidto.Defects where b.Sequence == _quiefectType.Sequence from a in b.DefectList select a).Where(c => Convert.ToString(c.KeyStroke).ToLower() == keyCode.ToLower()).FirstOrDefault();
            defectType.Count = total;

            TextBox txtkeystroke = (TextBox)this.Controls.Find(Convert.ToString(keyCode), true).FirstOrDefault();
            txtkeystroke.Text = total == 0 ? string.Empty : Convert.ToString(total);

            int newTotal = (from c in _quiefectType.DefectList
                            select c.Count).Sum();
            _textBox1.Text = newTotal == 0 ? string.Empty : Convert.ToString(newTotal);
        }
    }
}
