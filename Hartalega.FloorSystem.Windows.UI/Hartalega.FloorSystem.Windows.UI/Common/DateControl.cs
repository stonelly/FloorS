using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI
{
    public partial class DateControl : UserControl
    {
        #region Public Variables
        public DateTime DateValue { get; set; }
        #endregion

        #region Private Variables
        private string _dateFormat;
        private static string _screenName = "Common - DateControl";
        private static string _className = "DateControl";
        #endregion

        #region Public Methods
        public DateControl()
        {
            InitializeComponent();
            if (ReadDateFormat())
                timer.Start();
        }
        #endregion

        #region Private Methods
        private bool ReadDateFormat()
        {
            try
            {
                _dateFormat = ConfigurationManager.AppSettings["dateFormat"];
            }
            catch (FloorSystemException ex)
            {
                ExceptionLogging(ex, _screenName, _className, "ReadDateFormat", string.Empty);
                return false;
            }
            return true;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                txtDate.Text = CommonBLL.GetCurrentDateAndTime().ToString(_dateFormat);
                DateValue = DateTime.ParseExact(CommonBLL.GetCurrentDateAndTime().ToString(_dateFormat), _dateFormat, null); //Convert.ToDateTime(CommonBLL.GetCurrentDateAndTime().ToString(_dateFormat));
            }
            catch(Exception ex)
            {

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
                GlobalMessageBox.Show("(" + result + ") - " + Messages.APPLICATIONERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);
            else
                GlobalMessageBox.Show("(" + result + ") - " + Messages.DATABASEERROR, Constants.AlertType.Error, Messages.CONTACT_MIS, GlobalMessageBoxButtons.OK);          
        }

       
        #endregion
    }

}
