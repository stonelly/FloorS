using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Hartalega.BarcodeScannerIntegrator;
using Hartalega.BarcodeScannerIntegrator.D365Integration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hartalega.BarcodeScannerIntegratorUnitTests
{
    [TestClass]
    public class BarcodeScannerIntegratorTests
    {
        public string GetUserSessionOperationPath = D365ClientConfiguration.Default.UriString + "api/services/UserSessionService/AifUserSessionService/GetUserSessionInfo";

        [TestMethod]
        public void JsonAuthTest()
        {
            var request = HttpWebRequest.Create(GetUserSessionOperationPath);
            request.Headers[OAuthHelper.OAuthHeader] = OAuthHelper.GetAuthenticationHeader();
            request.Method = "POST";
            request.ContentLength = 0;
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader streamReader = new StreamReader(responseStream))
                    {
                        string responseString = streamReader.ReadToEnd();

                        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                        Assert.IsFalse(string.IsNullOrEmpty(responseString));
                        Console.WriteLine(responseString);
                        Console.WriteLine($"OAuthHeader:{request.Headers[OAuthHelper.OAuthHeader]}");
                    }
                }
            }
        }

        [TestMethod]
        public void TestScanTransferOrderResult()
        {
            var service = new BarcodeIntegrationService(D365ClientConfiguration.Default.UriString);
            service.GenerateDto(new List<string> { "abc" });
            service.PostToD365();
        }

        [TestMethod]
        public void TestScanCountingJourResult()
        {
            var service = new BarcodeIntegrationService(D365ClientConfiguration.Default.UriString);
            service.GenerateDto(new List<string> { "abc" });
            service.PostToD365();
        }
    }
}
