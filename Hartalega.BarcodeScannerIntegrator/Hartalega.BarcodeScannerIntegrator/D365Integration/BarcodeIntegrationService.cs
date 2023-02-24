using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Hartalega.BarcodeScannerIntegrator.D365Integration
{
    public class BarcodeIntegrationService
    {
        protected D365ClientConfiguration d365ClientConfiguration;
        protected BarcodeDeviceContract deviceContract;
        protected string baseServiceUrl = "api/services/DOT_BarcodeDeviceIntegration/BarcodeScan/";
        protected string fullWebApiUrl = "";
        private string _hostUrl = "";

        public BarcodeIntegrationService(string HostUrl)
        {
            d365ClientConfiguration = GetD365ClientConfig(HostUrl);
            _hostUrl = HostUrl;
        }

        public void GenerateDto(List<string> lines, string dataAreaId = "", string JournalId = "", string PickingRouteID = "", bool isTransferJournal = false)
        {
            lines.Reverse();
            deviceContract = new BarcodeDeviceContract
            {
                ProcessingStatus = ProcessingStatus.Ready,
                ErrorMessage = "",
                DataAreaId = dataAreaId,
                WorkStationName = Environment.MachineName,
                BarcodeList = lines,
                JournalId = JournalId,
                PickingRouteID = PickingRouteID
            };
            if (!_hostUrl.EndsWith("/"))
                _hostUrl = _hostUrl + "/";
            if (!string.IsNullOrEmpty(PickingRouteID))
                fullWebApiUrl = _hostUrl + baseServiceUrl + "sendTransferOrderScanResult";

            if (!string.IsNullOrEmpty(JournalId) && !isTransferJournal)
                fullWebApiUrl = _hostUrl + baseServiceUrl + "sendCountingJourScanResult";

            if (!string.IsNullOrEmpty(JournalId) && isTransferJournal)
                fullWebApiUrl = _hostUrl + baseServiceUrl + "sendTransferJournalScanResult";
        }

        public string getFullWebApiUrl()
        {
            return fullWebApiUrl;
        }

        /// <summary>
        /// based on host URL to get the different config
        /// </summary>
        /// <returns></returns>
        protected D365ClientConfiguration GetD365ClientConfig(string EnvironmentUrl)
        {
            D365ClientConfiguration d365Configuration = null;
            if (EnvironmentUrl.Contains("ax-hsbd365uat.hartalega.com.my"))
                d365Configuration = D365ClientConfiguration.D365UAT_HSB;
            else if (EnvironmentUrl.Contains("ax-hsbd365live.hartalega.com.my"))
                d365Configuration = D365ClientConfiguration.D365_HSB_Live_OnPremise;
            else if (EnvironmentUrl.Contains("ax.d365uat.hartalega.com.my"))
                d365Configuration = D365ClientConfiguration.UATOnPremise;
            else if (EnvironmentUrl.Contains("live.hartalega.com.my"))
                d365Configuration = D365ClientConfiguration.D365LiveOnPremise;
            else if (EnvironmentUrl.Contains("ax-d365.hartalega.com.my"))
                d365Configuration = D365ClientConfiguration.UAT_V10;
            else
                d365Configuration = D365ClientConfiguration.OneBox;
            return d365Configuration;
        }

        public BarcodeDeviceContract PostToD365()
        {
            BarcodeDeviceContract dto = deviceContract;
            //fullWebApiUrl = D365ClientConfiguration.Default.UriString + "api/services/DOT_FSIntegrationService/DOT_FSIntegrationService/processCountJournal";
            var request = HttpWebRequest.Create(fullWebApiUrl);
            request.Headers[OAuthHelper.OAuthHeader] = OAuthHelper.GetAuthenticationHeader(d365ClientConfiguration);
            request.Method = "POST";
            request.ContentLength = 0;

            dynamic contract = new ExpandoObject();
            contract.interfaceContract = dto;

            var requestContractString = JsonConvert.SerializeObject(contract);
            request.ContentType = "application/json";
            request.ContentLength = requestContractString.Length;

            using (var stream = request.GetRequestStream())
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(requestContractString);
                    writer.Flush();
                }
            }

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader streamReader = new StreamReader(responseStream))
                    {
                        string responseString = streamReader.ReadToEnd();
                        var strResult = JsonConvert.DeserializeObject<String>(responseString);
                        dto = JsonConvert.DeserializeObject<BarcodeDeviceContract>(strResult);
                    }
                }
            }

            return dto;
        }
    }
}
