using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Database;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using System.Data;
using Hartalega.FloorSystem.Framework.Cache;

namespace Hartalega.FloorSystem.Business.Logic
{
    public class WorkOrderSyncBLL : Framework.Business.BusinessBase
    {
        public static DataSet GetSalesDataFromAX4()
        {
            return FloorDBAccess.ExecuteDataSet("USP_FSSync_GetSalesData", null, ConnName.AX);
        }

        //sync work order
        public static List<string> SyncWorkOrder(List<SyncSalesTableDTO> salesDTO, List<SyncSalesLineDTO> salesLineDTO)
        {
            try
            {
                List<FloorSqlParameter> prmList = new List<FloorSqlParameter>
                {
                    new FloorSqlParameter("@SalesDetails", CommonBLL.SerializeTOXML(salesDTO)),
                    new FloorSqlParameter("@SalesLineDetails", CommonBLL.SerializeTOXML(salesLineDTO))
                };

                var result = FloorDBAccess.ExecuteDataTable("USP_FS_AX_WorkOrderSyncBatchJob", prmList);
                var resultList = new List<string>();
                foreach (DataRow row in result.Rows)
                {
                    resultList.Add(row.Field<string>("SalesId"));
                }
                return resultList;

            }
            catch (Exception ex)
            {
                throw new FloorSystemException("Exception occured while sync Work Order.", Constants.BUSINESSLOGIC, ex);
            }
        }

        public static void UpdateAXCustConfirmJourExtractedToPS(List<string> confirmIds)
        {
            try
            {
                List<FloorSqlParameter> prmList = new List<FloorSqlParameter>
                {
                    new FloorSqlParameter("@ArrayOfConfirmIds", CommonBLL.SerializeTOXML(confirmIds)),
                };

                FloorDBAccess.ExecuteNonQuery("USP_FSSync_UpdateCustConfirmJour_ExtractedToPS", prmList, ConnName.AX);

            }
            catch (Exception ex)
            {
                throw new FloorSystemException("Exception occured Update CustConfirmJour->ExtractedToPS.", Constants.BUSINESSLOGIC, ex);
            }
        }

        //insert record into AX_BatchJobSyncLog
        public static void InsertBatchJobSyncLog(byte jobstatus, string validationmessage)
        {
            try
            {
                List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>();
                PrmList.Add(new FloorSqlParameter("@JobStatus", jobstatus));
                PrmList.Add(new FloorSqlParameter("@ValidationMessage", validationmessage));
                FloorDBAccess.ExecuteNonQuery("USP_FS_Insert_AXBatchJobSyncLog", PrmList);
            }
            catch (Exception ex)
            {
                throw new FloorSystemException("Exception occured insert AXBatchJobSyncLog.", Constants.BUSINESSLOGIC, ex);
            }
        }
    }
}
