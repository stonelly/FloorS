using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.ServiceModel;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.DbExceptionLog;

namespace Hartalega.FloorSystem.IntegrationServices
{
    public class AXAgent_Old : IDisposable
    {
        //Testing//
        private NetTcpBinding _binding;
        private ChannelFactory<AXService.ImportParentService> _factory;
        private AXService.ImportParentService _channel;

        private static string _connectionString;
        private static string _userName;
        private static string _domain;
        private static string _password;
        private static string _fullDomain;


        #region OLD Code

        static AXAgent_Old()
        {
            _connectionString = ConfigurationManager.AppSettings["AXConnectionString"];
            _userName = ConfigurationManager.AppSettings["AXUserName"];
            _password = ConfigurationManager.AppSettings["AXPassword"];
            _domain = ConfigurationManager.AppSettings["AXDomain"];
            _fullDomain = ConfigurationManager.AppSettings["AXDomainFullName"];
        }

        public AXAgent_Old()
        {
            OpenChannel();
        }

        private void OpenChannel()
        {
            _binding = new NetTcpBinding();


            Uri serviceUri = new Uri(_connectionString);
            EndpointIdentity epIdentity = EndpointIdentity.CreateUpnIdentity(string.Format("{0}@{1}", _userName, _fullDomain));
            EndpointAddress _endpint = new EndpointAddress(serviceUri, epIdentity);
            _factory = new ChannelFactory<AXService.ImportParentService>(_binding, _endpint);


            _factory.Endpoint.Behaviors.Remove(_factory.Endpoint.Behaviors.Find<System.ServiceModel.Description.ClientCredentials>());

            System.ServiceModel.Description.ClientCredentials cdrls = new System.ServiceModel.Description.ClientCredentials();
            cdrls.Windows.ClientCredential.UserName = _userName;
            cdrls.Windows.ClientCredential.Password = _password;
            cdrls.Windows.ClientCredential.Domain = _domain;
            _factory.Endpoint.Behaviors.Add(cdrls);

            _factory.Open();


            _channel = _factory.CreateChannel();
            ((IClientChannel)_channel).Open();
        }

        private void CloseChannel()
        {
            ((IClientChannel)_channel).Close();
            _factory.Close();
        }

        private void TestServiceMethod(string batchCardNumber, string batchNumber, string itemNumber, int pcWeight, decimal batchWt)
        {
            OpenChannel();
            AXService.ImportParentServiceCreateParentChildRecordRequest request = new AXService.ImportParentServiceCreateParentChildRecordRequest(new AXService.CallContext(), batchCardNumber, batchNumber, itemNumber, pcWeight, batchWt);
            AXService.ImportParentServiceCreateParentChildRecordResponse response = _channel.createParentChildRecord(request);
            //To do// 
            //This response needs to be parsed and returned.
            CloseChannel();
        }
        /// <summary>
        /// This is the service call for print the normat batch card. Calling this service will push the information to the AX system.
        /// </summary>
        /// <param name="batchCardNumber"></param>
        /// <param name="serialNumber"></param>
        /// <param name="stationGloveCode"></param>
        /// <param name="size"></param>
        /// <param name="piecesWeight"></param>
        /// <param name="batchWeight"></param>
        /// <param name="plantStation"></param>
        /// <param name="plantProductionSize"></param>
        /// <param name="qCType"></param>
        /// <param name="createddateandtime"></param>
        /// <param name="postingdateandtime"></param>
        /// <param name="numberofPieces"></param>
        /// <param name="shift"></param>
        /// <param name="functionIdentifier"></param>
        /// <param name="batchSequence"></param>
        /// <returns>Transaction ID from the AX system.</returns>
        public int PrintNormalBatchCard(string batchCardNumber,
            string serialNumber,
            string stationGloveCode,
            string size,
            decimal piecesWeight,
            decimal batchWeight,
            string plantStation,
            string plantProductionSize,
            string qCType,
            DateTime createddateandtime,
            DateTime postingdateandtime,
            int numberofPieces,
            string shift,
            string functionIdentifier,
            int batchSequence)
        {

            // To do//

            //Once we have the relevent AX service is created the web reference will be updated and the service calls will be updated and can be utilized can called from here.

            // need to read the tracking ID that will be provided by the ax from the response and return the value.

            // To do//
            Dispose();
            return -1;
        }
        /// <summary>
        /// This is the service call for print the normat batch card. Calling this service will push the information to the AX system.
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="referenceItemNumber"></param>
        /// <param name="size"></param>
        /// <param name="plantStation"></param>
        /// <param name="plantPackingstation"></param>
        /// <param name="customerPO"></param>
        /// <param name="customerReference"></param>
        /// <param name="salesorderNumber"></param>
        /// <param name="innerLotnumber"></param>
        /// <param name="outerlotNumber"></param>
        /// <param name="customerlotNumber"></param>
        /// <param name="manufacturingdate"></param>
        /// <param name="expirydate"></param>
        /// <param name="preshipment"></param>
        /// <param name="preshipmentcases"></param>
        /// <param name="createddateandtime"></param>
        /// <param name="postingdateandtime"></param>
        /// <param name="numberofPieces"></param>
        /// <param name="functionIdentifier"></param>
        /// <param name="batchSequence"></param>
        /// <returns></returns>
        public int ScanbatchcardforInnerandOuter(

            string serialNumber,
            string referenceItemNumber,
            string size,
            string plantStation,
            decimal plantPackingstation,
            decimal customerPO,
            string customerReference,
            string salesorderNumber,
            string innerLotnumber,
            string outerlotNumber,
            string customerlotNumber,
            DateTime manufacturingdate,
            DateTime expirydate,
            bool preshipment,
            int preshipmentcases,
            DateTime createddateandtime,
            DateTime postingdateandtime,
            int numberofPieces,
            string functionIdentifier,
            int batchSequence
            )
        {

            // To do//

            //Once we have the relevent AX service is created the web reference will be updated and the service calls will be updated and can be utilized can called from here.

            // need to read the tracking ID that will be provided by the ax from the response and return the value.

            // To do//
            Dispose();
            return -1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="referenceItemNumber"></param>
        /// <param name="size"></param>
        /// <param name="plantStation"></param>
        /// <param name="plantPackingstation"></param>
        /// <param name="customerPO"></param>
        /// <param name="customerReference"></param>
        /// <param name="salesorderNumber"></param>
        /// <param name="innerLotnumber"></param>
        /// <param name="outerlotNumber"></param>
        /// <param name="customerlotNumber"></param>
        /// <param name="manufacturingdate"></param>
        /// <param name="expirydate"></param>
        /// <param name="preshipment"></param>
        /// <param name="preshipmentcases"></param>
        /// <param name="refSerialNumber1"></param>
        /// <param name="refSize1"></param>
        /// <param name="refNumberofPieces1"></param>
        /// <param name="refBatchSequence1"></param>
        /// <param name="createddateandtime"></param>
        /// <param name="postingdateandtime"></param>
        /// <param name="numberofPieces"></param>
        /// <param name="functionIdentifier"></param>
        /// <param name="refSerialNumber2"></param>
        /// <param name="refSize2"></param>
        /// <param name="refNumberofPieces2"></param>
        /// <param name="refBatchSequence2"></param>
        /// <returns></returns>
        public int scanBatchcardtoPrintInner(
            string referenceItemNumber,
            string size,
            string plantStation,
            string plantPackingstation,
            string customerPO,
            string customerReference,
            string salesorderNumber,
            string innerLotnumber,
            string outerlotNumber,
            string customerlotNumber,
            DateTime manufacturingdate,
            DateTime expirydate,
            bool preshipment,
            int preshipmentcases,
            string refSerialNumber1,
            string refSize1,
            string refNumberofPieces1,
            int refBatchSequence1,
            DateTime createddateandtime,
            DateTime postingdateandtime,
            decimal numberofPieces,
            string functionIdentifier,
            string refSerialNumber2 = "",
            string refSize2 = "",
            decimal refNumberofPieces2 = 0,
            string refBatchSequence2 = "")
        {
            // To do//

            //Once we have the relevent AX service is created the web reference will be updated and the service calls will be updated and can be utilized can called from here.

            // need to read the tracking ID that will be provided by the ax from the response and return the value.

            // To do//
            Dispose();
            return -1;

        }
        public int ScanMulitpleBatchPacking(

            string referenceItemNumber,
            string size,
            string plantStation,
            string plantPackingstation,
            string customerPO,
            string customerReference,
            string salesorderNumber,
            string innerLotnumber,
            string outerlotNumber,
            string customerlotNumber,
            DateTime manufacturingdate,
            DateTime expirydate,
            bool preshipment,
            int preshipmentcases,
            string refSerialNumber1,
            string refSize1,
            decimal refNumberofPieces1,
            int refBatchSequence1,
            DateTime createddateandtime,
            DateTime postingdateandtime,
            double numberofPieces,
            string functionIdentifier,
            string refSerialNumber2 = "",
            string refSize2 = "",
            double refNumberofPieces2 = 0.0,
            string refBatchSequence2 = "",
            string refSerialNumber3 = "",
            string refSize3 = "",
            double refNumberofPieces3 = 0.0,
            string refBatchSequence3 = "",
            string refSerialNumber4 = "",
            string refSize4 = "",
            string refNumberofPieces4 = "",
            string refBatchSequence4 = "",
            string refSerialNumber5 = "",
            string refSize5 = "",
            double refNumberofPieces5 = 0.0,
            string refBatchSequence5 = ""


            )
        {
            // To do//

            //Once we have the relevent AX service is created the web reference will be updated and the service calls will be updated and can be utilized can called from here.

            // need to read the tracking ID that will be provided by the ax from the response and return the value.

            // To do//
            Dispose();
            return -1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="batchCardNumber"></param>
        /// <param name="serialNumber"></param>
        /// <param name="stationGlovecode"></param>
        /// <param name="size"></param>
        /// <param name="piecesWeight"></param>
        /// <param name="batchWeight"></param>
        /// <param name="plantStation"></param>
        /// <param name="QCType"></param>
        /// <param name="createddateandtime"></param>
        /// <param name="postingdateandtime"></param>
        /// <param name="numberofPieces"></param>
        /// <param name="shift"></param>
        /// <param name="functionIdentifier"></param>
        /// <param name="batchSequence"></param>
        /// <returns></returns>
        public int ScanPTBatchCardNonSPBatches(


            string batchCardNumber,
            string serialNumber,
            string stationGlovecode,
            string size,
            decimal piecesWeight,
            decimal batchWeight,
            string plantStation,
            string QCType,
            DateTime createddateandtime,
            DateTime postingdateandtime,
            decimal numberofPieces,
            string shift,
            string functionIdentifier,
            int batchSequence)
        {
            // To do//

            //Once we have the relevent AX service is created the web reference will be updated and the service calls will be updated and can be utilized can called from here.

            // need to read the tracking ID that will be provided by the ax from the response and return the value.

            // To do//
            Dispose();
            return -1;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="batchCardNumber"></param>
        /// <param name="serialNumber"></param>
        /// <param name="stationGlove"></param>
        /// <param name="size"></param>
        /// <param name="piecesWeight"></param>
        /// <param name="batchWeight"></param>
        /// <param name="plantStation"></param>
        /// <param name="QCType"></param>
        /// <param name="createddateandtime"></param>
        /// <param name="postingdateandtime"></param>
        /// <param name="numberofPieces"></param>
        /// <param name="shift"></param>
        /// <param name="functionIdentifier"></param>
        /// <param name="batchSequence"></param>
        /// <returns></returns>
        public int ScanOutBatchCard(
            string batchCardNumber,
            string serialNumber,
            string stationGlove,
            string size,
            decimal piecesWeight,
            decimal batchWeight,
            string plantStation,
            string QCType,
            DateTime createddateandtime,
            DateTime postingdateandtime,
            decimal numberofPieces,
            string shift,
            string functionIdentifier,
            int batchSequence)
        {
            // To do//

            //Once we have the relevent AX service is created the web reference will be updated and the service calls will be updated and can be utilized can called from here.

            // need to read the tracking ID that will be provided by the ax from the response and return the value.

            // To do//
            Dispose();
            return -1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="batchCardNumber"></param>
        /// <param name="serialNumber"></param>
        /// <param name="stationGlove"></param>
        /// <param name="size"></param>
        /// <param name="piecesWeight"></param>
        /// <param name="batchWeight"></param>
        /// <param name="plantStation"></param>
        /// <param name="QCType"></param>
        /// <param name="createddateandtime"></param>
        /// <param name="postingdateandtime"></param>
        /// <param name="numberofPieces"></param>
        /// <param name="shift"></param>
        /// <param name="functionIdentifiers"></param>
        /// <param name="batchSequence"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        public int Watertightbatchcard(
            string batchCardNumber,
            string serialNumber,
            string stationGlove,
            string size,
            decimal piecesWeight,
            decimal batchWeight,
            string plantStation,
            string QCType,
            DateTime createddateandtime,
            DateTime postingdateandtime,
            decimal numberofPieces,
            string shift,
            string functionIdentifiers,
            int batchSequence,
            string area)
        {
            // To do//

            //Once we have the relevent AX service is created the web reference will be updated and the service calls will be updated and can be utilized can called from here.

            // need to read the tracking ID that will be provided by the ax from the response and return the value.

            // To do//
            Dispose();
            return -1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="batchCardNumber"></param>
        /// <param name="serialNumber"></param>
        /// <param name="stationGlove"></param>
        /// <param name="size"></param>
        /// <param name="piecesWeight"></param>
        /// <param name="batchWeight"></param>
        /// <param name="plantStation"></param>
        /// <param name="qCType"></param>
        /// <param name="createddateandtime"></param>
        /// <param name="postingdateandtime"></param>
        /// <param name="numberofPieces"></param>
        /// <param name="shift"></param>
        /// <param name="functionIdentifiers"></param>
        /// <param name="batchSequence"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        public int VisualTestbatchcard(
            string batchCardNumber,
            string serialNumber,
            string stationGlove,
            string size,
            decimal piecesWeight,
            decimal batchWeight,
            string plantStation,
            string qCType,
            DateTime createddateandtime,
            DateTime postingdateandtime,
            decimal numberofPieces,
            string shift,
            string functionIdentifiers,
            int batchSequence,
            string area

            )
        {
            // To do//

            //Once we have the relevent AX service is created the web reference will be updated and the service calls will be updated and can be utilized can called from here.

            // need to read the tracking ID that will be provided by the ax from the response and return the value.

            // To do//
            Dispose();
            return -1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemNumber"></param>
        /// <param name="size"></param>
        /// <param name="plantStation"></param>
        /// <param name="postingdateandtime"></param>
        /// <param name="numberofPieces"></param>
        /// <param name="functionIdentifier"></param>
        /// <returns></returns>
        public int ScanBatchcardtoPrintInner(
            string itemNumber,
            string size,
            string plantStation,
            string postingdateandtime,
            decimal numberofPieces,
            decimal functionIdentifier
)
        {
            // To do//

            //Once we have the relevent AX service is created the web reference will be updated and the service calls will be updated and can be utilized can called from here.

            // need to read the tracking ID that will be provided by the ax from the response and return the value.

            // To do//
            Dispose();
            return -1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="batchCardNumber"></param>
        /// <param name="serialNumber"></param>
        /// <param name="stationGlove"></param>
        /// <param name="size"></param>
        /// <param name="piecesWeight"></param>
        /// <param name="batchWeight"></param>
        /// <param name="plantStation"></param>
        /// <param name="qCType"></param>
        /// <param name="createddateandtime"></param>
        /// <param name="postingdateandtime"></param>
        /// <param name="numberofPieces"></param>
        /// <param name="shift"></param>
        /// <param name="functionIdentifiers"></param>
        /// <param name="batchSequence"></param>
        /// <returns></returns>
        public int ChangeGloveType(
            string batchCardNumber,
            string serialNumber,
            string stationGlove,
            string size,
            decimal piecesWeight,
            decimal batchWeight,
            string plantStation,
            string qCType,
            DateTime createddateandtime,
            DateTime postingdateandtime,
            decimal numberofPieces,
            string shift,
            string functionIdentifiers,
            int batchSequence

            )
        {
            // To do//

            //Once we have the relevent AX service is created the web reference will be updated and the service calls will be updated and can be utilized can called from here.

            // need to read the tracking ID that will be provided by the ax from the response and return the value.

            // To do//
            Dispose();
            return -1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="glovecode"></param>
        /// <param name="size"></param>
        /// <param name="plantStation"></param>
        /// <param name="postingdateandtime"></param>
        /// <param name="numberofPieces"></param>
        /// <param name="functionIdentifiers"></param>
        /// <returns></returns>
        public int DowngradeBatchCardto2ndGrade(
            string serialNumber,
            string glovecode,
            string size,
            string plantStation,
            DateTime postingdateandtime,
            string numberofPieces,
            string functionIdentifiers

            )
        {
            // To do//

            //Once we have the relevent AX service is created the web reference will be updated and the service calls will be updated and can be utilized can called from here.

            // need to read the tracking ID that will be provided by the ax from the response and return the value.

            // To do//
            Dispose();
            return -1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="batchCardNumber"></param>
        /// <param name="serialNumber"></param>
        /// <param name="stationGlovecode"></param>
        /// <param name="referenceItemNo"></param>
        /// <param name="size"></param>
        /// <param name="referanceSize"></param>
        /// <param name="piecesWeight"></param>
        /// <param name="batchWeight"></param>
        /// <param name="plantStation"></param>
        /// <param name="qCType"></param>
        /// <param name="createddateandtime"></param>
        /// <param name="postingdateandtime"></param>
        /// <param name="numberofPieces"></param>
        /// <param name="shift"></param>
        /// <param name="functionIdentifier"></param>
        /// <param name="batchSequence"></param>
        /// <returns></returns>
        public int PrintCustomerReject(
            string batchCardNumber,
            string serialNumber,
            string stationGlovecode,
            string referenceItemNo,
            string size,
            string referanceSize,
            decimal piecesWeight,
            decimal batchWeight,
            string plantStation,
            string qCType,
            DateTime createddateandtime,
            DateTime postingdateandtime,
            string numberofPieces,
            string shift,
            string functionIdentifier,
            int batchSequence

            )
        {
            // To do//

            //Once we have the relevent AX service is created the web reference will be updated and the service calls will be updated and can be utilized can called from here.

            // need to read the tracking ID that will be provided by the ax from the response and return the value.

            // To do//
            Dispose();
            return -1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serialNumberOld"></param>
        /// <param name="serialNumberNew"></param>
        /// <param name="size"></param>
        /// <param name="plantStation"></param>
        /// <param name="salesorderOldSerialnumber"></param>
        /// <param name="createddateandtime"></param>
        /// <param name="postingdateandtime"></param>
        /// <param name="numberofPiecesOldbatchnumberquantity"></param>
        /// <param name="refBatchsequence1"></param>
        /// <param name="refBatchSequence2"></param>
        /// <param name="functionIdentifier"></param>
        /// <param name="batchSequence"></param>
        /// <returns></returns>
        public int ChangeBatchCardforInner(
            string serialNumberOld,
            string serialNumberNew,
            string size,
            string plantStation,
            string salesorderOldSerialnumber,
            DateTime createddateandtime,
            DateTime postingdateandtime,
            decimal numberofPiecesOldbatchnumberquantity,
            string refBatchsequence1,
            string refBatchSequence2,
            string functionIdentifier,
            int batchSequence

            )
        {
            // To do//

            //Once we have the relevent AX service is created the web reference will be updated and the service calls will be updated and can be utilized can called from here.

            // need to read the tracking ID that will be provided by the ax from the response and return the value.

            // To do//
            Dispose();
            return -1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="batchCardNumber"></param>
        /// <param name="serialNumber"></param>
        /// <param name="stationGlovecode"></param>
        /// <param name="size"></param>
        /// <param name="piecesWeight"></param>
        /// <param name="batchWeight"></param>
        /// <param name="plantStation"></param>
        /// <param name="qCType"></param>
        /// <param name="createddateandtime"></param>
        /// <param name="postingdateandtime"></param>
        /// <param name="numberofPieces"></param>
        /// <param name="shift"></param>
        /// <param name="functionIdentifier"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        public int PrintLostBatchCard(
            string batchCardNumber,
            string serialNumber,
            string stationGlovecode,
            string size,
            decimal piecesWeight,
            decimal batchWeight,
            string plantStation,
            string qCType,
            DateTime createddateandtime,
            DateTime postingdateandtime,
            int numberofPieces,
            int shift,
            int functionIdentifier,
            string area

            )
        {
            // To do//

            //Once we have the relevent AX service is created the web reference will be updated and the service calls will be updated and can be utilized can called from here.

            // need to read the tracking ID that will be provided by the ax from the response and return the value.

            // To do//
            Dispose();
            return -1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="batchCardNumber"></param>
        /// <param name="serialNumber"></param>
        /// <param name="size"></param>
        /// <param name="plantStation"></param>
        /// <param name="location"></param>
        /// <param name="numberofPieces"></param>
        /// <param name="scanIndateandtime"></param>
        /// <param name="functionIdentifier"></param>
        /// <param name="batchSequence"></param>
        /// <returns></returns>
        public int ScanIn(
            string batchCardNumber,
            string serialNumber,
            string size,
            string plantStation,
            string location,
            int numberofPieces,
            DateTime scanIndateandtime,
            int functionIdentifier,
            int batchSequence

            )
        {
            // To do//

            //Once we have the relevent AX service is created the web reference will be updated and the service calls will be updated and can be utilized can called from here.

            // need to read the tracking ID that will be provided by the ax from the response and return the value.

            // To do//
            Dispose();
            return -1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="batchCardNumber"></param>
        /// <param name="serialNumber"></param>
        /// <param name="stationGlovecode"></param>
        /// <param name="size"></param>
        /// <param name="plantStationFromWarehouse"></param>
        /// <param name="plantStationToWarehouse"></param>
        /// <param name="numberofPieces"></param>
        /// <param name="scanOutdateandtime"></param>
        /// <param name="functionIdentifier"></param>
        /// <param name="batchSequence"></param>
        /// <returns></returns>
        public int Scanout(
            string batchCardNumber,
            string serialNumber,
            string stationGlovecode,
            string size,
            string plantStationFromWarehouse,
            string plantStationToWarehouse,
            int numberofPieces,
            DateTime scanOutdateandtime,
            int functionIdentifier,
            int batchSequence

            )
        {
            // To do//

            //Once we have the relevent AX service is created the web reference will be updated and the service calls will be updated and can be utilized can called from here.

            // need to read the tracking ID that will be provided by the ax from the response and return the value.

            // To do//
            Dispose();
            return -1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="BatchCardNumber"></param>
        /// <param name="SerialNumber"></param>
        /// <param name="Size"></param>
        /// <param name="PlantStation"></param>
        /// <param name="NumberofPieces"></param>
        /// <param name="Location"></param>
        /// <param name="ScanTMPpackdateandtime"></param>
        /// <param name="FunctionIdentifier"></param>
        /// <param name="BatchSequence"></param>
        /// <returns></returns>
        public int ScanTMPpackInventory(
            string batchCardNumber,
            string serialNumber,
            string size,
            string plantStation,
            string numberofPieces,
            string location,
            DateTime scanTMPpackdateandtime,
            int functionIdentifier,
            int batchSequence

            )
        {
            // To do//

            //Once we have the relevent AX service is created the web reference will be updated and the service calls will be updated and can be utilized can called from here.

            // need to read the tracking ID that will be provided by the ax from the response and return the value.

            // To do//
            Dispose();
            return -1;
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose();
            CloseChannel();
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
