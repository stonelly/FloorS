using Hartalega.FloorSystem.Business.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;

namespace Hartalega.FloorSystem.Windows.UI.SetUpConfiguration
{
    /// <summary>
    ///
    ///</summary>
    public partial class WorkStationDetails : FormBase
    {
        /// <summary>
        ///
        ///</summary>
        internal string _key;
        
        /// <summary>
        ///
        ///</summary>
        private ConfMgr _wsBLL;

        /// <summary>
        ///
        ///</summary>
        private bool _isSystemConf;



        /// <summary>
        ///
        ///</summary>

        public WorkStationDetails()
        {
            InitializeComponent();
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param> </param>
        /// <returns></returns>
        public WorkStationDetails(ConfMgr wsBll,bool isSystemConfiguration = false):this()
        {
            _wsBLL = wsBll;
            _isSystemConf = isSystemConfiguration;
            IEnumerable<string> cmbData = null;

            if (isSystemConfiguration == true)
            {
                cmbData = _wsBLL.GetSystemConfigurableItems();                
            }
            else
            {
                cmbData = _wsBLL.GetConfigurableItems();                
            }

            if (cmbData.Count() == 0)
            {               
                
                btnAdd.Enabled = false;

               
                cmbPropWSDTO.Items.Add("Nothing to configure");
                cmbPropWSDTO.SelectedIndex = 0;
                cmbPropWSDTO.Enabled = false;


            }
            else
            {
                cmbPropWSDTO.DataSource = new BindingSource(cmbData, null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <returns></returns>
        private void btnAdd_Click(object sender, EventArgs e)
        {           
                _key = cmbPropWSDTO.SelectedValue as string;
                _wsBLL.SetSelectedKey(_key,_isSystemConf);
                DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <returns></returns>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
