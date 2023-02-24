using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.OEE
{
    public partial class Menu : FormBase
    {
        #region private Variables

        private static string _screenName = Constants.OEE_MENU;
        private static string _className = "Menu";
        #endregion
        #region Load Form
        public Menu()
        {
            InitializeComponent();
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            BindButtons();
        }

        #endregion

        #region Event Handlers


        void btn_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            Process.Start("IExplore.exe", bt.Name);
        }

        #endregion

        #region User Methods
        private void BindButtons()
        {
            try
            {
                List<DropdownDTO> rptlist = TVReportsBLL.GetOEEReportDetails();
                this.tlpMenu.RowCount = rptlist.Count + 1;
                for (int k = 0; k <= rptlist.Count + 1; k++)
                {
                    this.tlpMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50));
                }
                tlpMenu.Height = (rptlist.Count + 1) * 45;

                int i = 0;
                foreach (DropdownDTO dt in rptlist)
                {
                    if (!string.IsNullOrEmpty(dt.DisplayField))
                    {
                        Button btn = new Button();
                        btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        btn.Location = new System.Drawing.Point(3, 3);
                        btn.Name = dt.IDField;
                        btn.Size = new System.Drawing.Size(534, 50);
                        btn.TabIndex = 0;
                        btn.Text = dt.DisplayField;
                        btn.UseVisualStyleBackColor = true;
                        btn.Click += btn_Click;
                        this.tlpMenu.Controls.Add(btn, 0, i);
                        i++;
                    }
                }
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "BindVisaulTesttSamlSize", null);
                return;
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
        #endregion
    }
}
