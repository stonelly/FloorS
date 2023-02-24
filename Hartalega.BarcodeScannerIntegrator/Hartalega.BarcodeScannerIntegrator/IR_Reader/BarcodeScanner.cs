using Hartalega.BarcodeScannerIntegrator.D365Integration;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Deployment.Application;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hartalega.BarcodeScannerIntegrator.IR_Reader
{
    public class BarcodeScanner:IDisposable
    {
        private int retryCount = 3;
        private readonly TimeSpan retryDelay = TimeSpan.FromSeconds(5);
        private readonly TimeSpan readFileDelay = TimeSpan.FromSeconds(10);

        string ScanFilePath;//  = "C:\\Barcode\\";
        string BarcodeReaderPath;// = "C:\\Barcode\\Reader\\";
        string irReaderName = "IR_Read";
        string irReaderFullName = "IR_Read.exe";
        

        public async Task<BarcodeDeviceContract> DownloadData(NameValueCollection queryStr)
        {
            BarcodeDeviceContract barcodeDeviceContract;
            int currentRetry = 0;
            var filePath = "";
            for (; ; )
            {
                try
                {
                    string HostUrl = queryStr["HostUrl"];
                    string JournalId = queryStr["JournalId"];
                    string PickingRouteID = queryStr["PickingRouteID"];
                    string CompanyKey = queryStr["CompanyKey"];
                    string strIsTransferJournal = queryStr["TJ"];
                    bool isTransferJournal = false;

                    //MessageBox.Show("TJ is " + strIsTransferJournal);
                    if (!string.IsNullOrEmpty(strIsTransferJournal) && strIsTransferJournal == "1")
                    {
                        isTransferJournal = true;
                    }
                        


                    string DocId = string.IsNullOrEmpty(JournalId) ? PickingRouteID : JournalId;
                    // Call IR_Reader.
                    filePath = await this.ReadExe(DocId);
                    if (!ApplicationDeployment.IsNetworkDeployed)
                    {
                        //debug
                        filePath = ScanFilePath + "HSIBJ00000263_58500308032022.txt";
                    }
                    // 
                    await Task.Delay(readFileDelay);

                    List<string> lines = null;
                    if (!string.IsNullOrEmpty(filePath))
                        lines = AppFileHelper.ReadLines(filePath).Reverse().ToList(); // read file
                                                                                      // call web service send the barcode list
                    var integrationService = new BarcodeIntegrationService(HostUrl);
                    integrationService.GenerateDto(lines, CompanyKey, JournalId, PickingRouteID, isTransferJournal);
                    //MessageBox.Show("Web service url is " + integrationService.getFullWebApiUrl());
                    barcodeDeviceContract = integrationService.PostToD365();

                    // Return or break.
                    break;
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Operation Exception");

                    currentRetry++;

                    // Check if the exception thrown was a transient exception
                    // based on the logic in the error detection strategy.
                    // Determine whether to retry the operation, as well as how
                    // long to wait, based on the retry strategy.
                    if (currentRetry > this.retryCount || !IsTransient(ex))
                    {
                        // If this isn't a transient error or we shouldn't retry, 
                        // rethrow the exception.
                        KillIrReaderProcess();
                        throw ex;
                    }
                }

                // Wait to retry the operation.
                // Consider calculating an exponential delay here and
                // using a strategy best suited for the operation and fault.
                await Task.Delay(retryDelay);
            }
            return barcodeDeviceContract;
        }

        public async Task<string> ReadExe(string filePrefix)
        {
            var fileName = "";

            try
            {
                ScanFilePath = System.Configuration.ConfigurationManager.AppSettings["ScanFilePath"];
                BarcodeReaderPath = System.Configuration.ConfigurationManager.AppSettings["BarcodeReaderPath"];
                var directory = new DirectoryInfo(ScanFilePath);
                if (!directory.Exists)
                {
                    throw new Exception(string.Format("Barcode file output folder {0} not setup in local PC.", ScanFilePath));
                }

                //directory = new DirectoryInfo(BarcodeReaderPath);
                //if (!directory.Exists)
                //{
                //    throw new Exception(string.Format("Barcode reader software {0} not found in local PC.", BarcodeReaderPath));
                //}

                if (!File.Exists(BarcodeReaderPath + irReaderFullName))
                {
                    throw new Exception(string.Format("Barcode reader software {0} not found in local PC.", BarcodeReaderPath + irReaderFullName));
                }

                fileName = string.Format("{0}_{1}.txt", filePrefix, DateTime.Now.ToString("ssmmhhddMMyyyy"));

                KillIrReaderProcess();

                //ScanFilePath = InventParameters::find().AvaBarcodeFile + '\\' + fileName + ",3,1,1,1,1,1,0,0,0,2";
                var comPort = "3";
                var fileFullPath = ScanFilePath + fileName + $",2,{comPort},1,1,1,1,0,1,0,0,2,2,0,0,0,0,0,3,0,9,0,2";

                Process.Start(BarcodeReaderPath + irReaderFullName, fileFullPath);

                return ScanFilePath + fileName;
            }
            catch (Exception ex)
            {
                throw new IRReaderOperationException(ex.Message, ex);
            }

        }

        private void KillIrReaderProcess()
        {
            var readerProcesses = Process.GetProcessesByName(irReaderName);
            foreach (var process in readerProcesses)
            {
                process.Kill();
            }
        }

        private bool IsTransient(Exception ex)
        {
            // Determine if the exception is transient.
            // In some cases this is as simple as checking the exception type, in other
            // cases it might be necessary to inspect other properties of the exception.
            if (ex is IRReaderOperationException)
                return true;

            if (ex is IOException)
                return true;

            var webException = ex as WebException;
            if (webException != null)
            {
                // If the web exception contains one of the following status values
                // it might be transient.
                return new[] {WebExceptionStatus.ConnectionClosed,
                  WebExceptionStatus.Timeout,
                  WebExceptionStatus.RequestCanceled }.
                        Contains(webException.Status);
            }

            // Additional exception checking logic goes here.
            return false;
        }

        public void Dispose()
        {
            KillIrReaderProcess();
        }
    }
}
