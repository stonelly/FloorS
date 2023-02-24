using System;
using System.Collections.Specialized;
using System.Windows.Forms;
using System.Web;
using System.Deployment.Application;
using Hartalega.BarcodeScannerIntegrator.IR_Reader;
using System.Threading.Tasks;
using Hartalega.BarcodeScannerIntegrator.D365Integration;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Hartalega.BarcodeScannerIntegrator
{
	/// <summary>
	/// 
	/// </summary>
	static class Program
	{
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(String lpClassName, String lpWindowName);

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        static int SW_MAXIMIZE = 3;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            BarcodeScanner scanner = null;
            try
            {
                scanner = new BarcodeScanner();
                // Show the system tray icon.	
                using (ProcessIcon pi = new ProcessIcon())
                {
                    pi.Display();
                    // Make sure the application runs!
                    //Application.Run();
                }
                var querystr = GetQueryStringParameters();
                //MessageBox.Show(querystr[0]);
                
                var result = Task.Run<BarcodeDeviceContract>(()=>scanner.DownloadData(querystr)).Result;
                MessageBox.Show(result.ErrorMessage, "Download Complete,Please refresh pallet scan screen!");
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                errorMessage = ex.InnerException?.Message;
                MessageBox.Show(errorMessage, "Download Error:Please contact MIS and upload file manually");
            }
            finally
            {
                scanner.Dispose();
                setCurrentProcessInFocus();
                Application.Exit();
            }
        }

        private static void setCurrentProcessInFocus()
        {
            try
            {
                Process currentProcess = Process.GetCurrentProcess();
                if (currentProcess != null)
                {
                    IntPtr hWnd = currentProcess.MainWindowHandle;
                    if (hWnd != IntPtr.Zero)
                    {
                        SetForegroundWindow(hWnd);
                        ShowWindow(hWnd, SW_MAXIMIZE);
                    }
                }
                //listProcess();
            }
            catch (Exception exc)
            {

            }
        }

        private static NameValueCollection GetQueryStringParameters()
        {
            NameValueCollection nameValueTable = new NameValueCollection();

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                string queryString = ApplicationDeployment.CurrentDeployment.ActivationUri.Query;
                nameValueTable = HttpUtility.ParseQueryString(queryString);
            }
            else//debug mode
            {
                //var querystr = "HostUrl=https://usnconeboxax1aos.cloud.onebox.dynamics.com/&CompanyKey=hngc&JournalId=HNGC-042641";
                //var querystr = "HostUrl=https://ax.d365uat.hartalega.com.my/namespaces/AXSF&CompanyKey=HNGC&JournalId=HNIBJ0000109"; // UAT 
                //var querystr = "HostUrl=https://usnconeboxax1aos.cloud.onebox.dynamics.com/&CompanyKey=HNGC&PickingRouteID=HNGC-00403"; // dev box transfer order
                var querystr = "HostUrl=https://ax-hsbd365uat.hartalega.com.my/namespaces/AXSF&CompanyKey=HSB&JournalId=HSIBJ00000263&TJ=1"; // dev box transfer order
                nameValueTable = HttpUtility.ParseQueryString(querystr);
            }
            return (nameValueTable);
        }
    }
}