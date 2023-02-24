// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Avanade">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hartalega.FloorSystem.Windows.UI
{
    using FloorSystem.Framework.DbExceptionLog;
    using Hartalega.FloorSystem.Business.Logic;
    using System;
    using System.Windows.Forms;
    using Hartalega.FloorSystem.Windows.UI.Common;
    using System.Threading;
    using System.Diagnostics;
    using Hartalega.FloorSystem.Framework;
    
    using Hartalega.FloorSystem.Framework.Cache;
    using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
    using Hartalega.FloorSystem.Framework.Common;
    using System.Drawing;

    /// <summary>
    /// Startup program for Floor System
    /// File Type: Application startup file
    /// </summary>
    public static class Program
    {
        public static bool debugMode;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            //test module
            //new BatchEnquiry().ShowDialog();
            //System.Environment.Exit(0);
            string procName = string.Empty;
            try
            { 
                procName = Process.GetCurrentProcess().ProcessName;
                CacheManager.FillCache();
                // for development no need to check application instance, Max He
#if DEBUG
                debugMode = true;
                MessageBox.Show("Debug on " + System.Net.Dns.GetHostName());
#else
#if UAT
                debugMode = true;
                MessageBox.Show("UAT on " + System.Net.Dns.GetHostName());
#endif
#endif
                if (Process.GetProcessesByName(procName).Length == Constants.ONE || debugMode)
                {
                    string moduleInfo = Messages.MODULE_INFO_NOT_AVAILABLE_IN_DATABASE;
                    if (!string.IsNullOrEmpty(GetexistConfigurationDetails()))
                    {
                        Thread workerThread = new Thread(Getprinters);
                        workerThread.Start();
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        CommonBLL.InitializeCurrentDateAndTime();
                        CommonBLL.GetWorkStationdetails(System.Net.Dns.GetHostName());
                        CommonBLL.GetFloorSystemConfiguration();
                        string checkKeysMessage = CommonBLL.CheckConfigurationKeys();
                        if (!string.IsNullOrEmpty(checkKeysMessage))
                        Application.Run(new Common.MasterMenu(false,checkKeysMessage));
                        else
                        Application.Run(new Common.MasterMenu(true,checkKeysMessage));
                    }
                    else
                    {
                        GlobalMessageBox.Show(Messages.MACHINE_CONFIGURATION_ERROR,Constants.AlertType.Warning,Messages.CONTACT_MIS);
                    }
                }
                else
                {
                    GlobalMessageBox.Show(Messages.APPLICATION_ALREADY_RUNNING, Constants.AlertType.Information, Messages.INFORMATION);
                }
            }
            catch (FloorSystemException ex)
            {
                ex.WorkStationId = Messages.WORKSTATION_NOT_EXIST;
                ex.screenName = Messages.MAIN_PROGRAM;
                ex.uiClassName = Messages.MAIN;
                ex.uiControlName = Messages.MAIN;
                ex.baseexception = ex.GetBaseException().ToString();
                ex.LogExceptionToDB(); 
            }
            catch (System.Data.SqlClient.SqlException ex)
            {                
                GlobalMessageBox.Show("Database Error.\n" + ex.Message, Constants.AlertType.Error,"Contact MIS");
                try
                {
                    EventLog.WriteEntry(procName, ex.ToString(), EventLogEntryType.Error);
                }
                catch(Exception)
                {
                    GlobalMessageBox.Show("Application does not have Admin rights to write into Event log.", Constants.AlertType.Error, "Contact MIS");
                }
            }
            catch (Exception ex)
            {
                if (debugMode)
                {
                    GlobalMessageBox.Show(ex.Message, Constants.AlertType.Error, "Contact MIS");
                    GlobalMessageBox.Show(ex.StackTrace, Constants.AlertType.Error, "Contact MIS");
                }
                else
                    GlobalMessageBox.Show("Unable to access the Application.", Constants.AlertType.Error, "Contact MIS");

                FloorSystemException fsexp = new FloorSystemException(ex.Message, string.Empty, ex);
                ExceptionLogging(fsexp, Messages.MAIN, Constants.PROGRAM, Messages.MAIN, null);
                string.Format(Messages.SYSTEMERROR, ExceptionLogging(fsexp, Messages.MAIN, Constants.PROGRAM, Messages.MAIN, null));
            }
        }
        private static int ExceptionLogging(FloorSystemException floorException, string screenName, string UiClassName, string uiControl, params object[] parameters)
        {
            return CommonBLL.LogExceptionToDB(floorException, screenName, UiClassName, uiControl, parameters);

        }

        private static void Getprinters()
        {
            PrinterClass.DefeaultPrinterConnected();
        }
        private static string GetexistConfigurationDetails()
        {
            ConfMgr _wsBLL = new ConfMgr();
            return _wsBLL.GetSelectWorkStation(System.Net.Dns.GetHostName());
        }
    }
}
