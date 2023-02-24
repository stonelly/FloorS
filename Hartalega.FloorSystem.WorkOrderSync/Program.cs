using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using System.Transactions;
using System.Reflection;
using Hartalega.FloorSystem.Framework.Database;
using Hartalega.FloorSystem.Framework.Cache;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;

namespace Hartalega.FloorSystem.WorkOrderSync
{
    public class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            log.Info("############## START: Work Order Sync ##############");

            try
            {
                StartSync();
            }
            catch (FloorSystemException ex)
            {
                Exception baseException = ex.GetBaseException() ?? ex;
                log.Error(baseException.Message);
                log.Error(baseException.StackTrace);
                // AX failed log
                WorkOrderSyncBLL.InsertBatchJobSyncLog(1, ex.Message);
            }
            catch (Exception ex)
            {
                var baseException = GetInnerException(ex);
                log.Error(baseException.Message);
                log.Error(baseException.StackTrace);
                // AX failed log
                WorkOrderSyncBLL.InsertBatchJobSyncLog(1, ex.Message);
            }
            finally
            {
                // log success log
                log.InfoFormat("############## END: Work Order Sync. Time elapsed: {0} ##############", stopwatch.Elapsed);
            }
        }

        static void StartSync()
        {
            log.Info("Get Sales Data from AX4");
            var ds = WorkOrderSyncBLL.GetSalesDataFromAX4();

            var salesData = ds.Tables[0];
            var salesLineData = ds.Tables[1];
            var confirmJourData = ds.Tables[2];

            if (salesData.Rows.Count == 0)
            {
                log.Info("No available sales to synchonize");
                return;
            }

            var salesDTOs = DataTableToDTO<SyncSalesTableDTO>(salesData);
            var salesLineDTOs = DataTableToDTO<SyncSalesLineDTO>(salesLineData);
            var confirmJourDTOs = DataTableToDTO<SyncSalesConfirmJourDTO>(confirmJourData);
            log.InfoFormat("Sales Data:{0}; Sales Line Data: {1}; Confirm Journal Data: {2}", salesData.Rows.Count, salesLineData.Rows.Count, confirmJourData.Rows.Count);

            List<string> blockedSalesIds = null;

            using (var scope = new TransactionScope())
            {
                log.Info("Synchronizing data to FS");
                blockedSalesIds = WorkOrderSyncBLL.SyncWorkOrder(salesDTOs, salesLineDTOs);

                if (blockedSalesIds.Count > 0)
                {
                    log.Info("Queue email for blocked sales");
                    // Queue email if have blocked sales from update
                    CommonBLL.GetFloorSystemConfiguration(); //fill configuration
                    CacheManager.FillCache(); //fill message
                    foreach (var salesId in blockedSalesIds)
                    {
                        var salesDTO = salesDTOs.Where(p => p.SalesId == salesId).First();

                        string emailrefid = salesDTO.SalesId + " / " + salesDTO.CustomerRef; //set referenceid format in email
                        string recipients = FloorSystemConfiguration.GetInstance().strWorkOrderNotifyEmail; //recipients
                        string emailSubject = String.Format(Messages.WORKORDERSUBJECT, emailrefid);  //subject
                        string emailBody = String.Format(Messages.WORKORDERCONTENT, emailrefid);  //content
                        SendEmailBLL.QueueEmail(recipients, emailSubject, emailBody);
                    }
                }

                scope.Complete();
            }

            // get list of sales id to update ax4
            var confirmIdsToUpdateAX4 = confirmJourDTOs.Where(p => !blockedSalesIds.Contains(p.SalesId)).Select(p => p.ConfirmId).ToList();
            // update to ax4
            if (confirmIdsToUpdateAX4.Count > 0)
            {
                log.Info("Update AX4 CustConfirmJour->ExtractedToPS");
                WorkOrderSyncBLL.UpdateAXCustConfirmJourExtractedToPS(confirmIdsToUpdateAX4);
            }
        }

        static List<string> SyncWorkOrder(List<SyncSalesTableDTO> salesDTO,
                                          List<SyncSalesLineDTO> salesLineDTO)
        {
            List<FloorSqlParameter> PrmList = new List<FloorSqlParameter>
            {
                new FloorSqlParameter("@SalesDetails", CommonBLL.SerializeTOXML(salesDTO)),
                new FloorSqlParameter("@SalesLineDetails", CommonBLL.SerializeTOXML(salesLineDTO))
            };

            return new List<string>();
        }

        static Exception GetInnerException(Exception ex)
        {
            if (ex.InnerException == null) return ex;
            else return GetInnerException(ex.InnerException);
        }

        static List<T> DataTableToDTO<T>(DataTable data)
        {

            var result = new List<T>();
            if (data != null && data.Rows.Count > 0)
            {
                // get properties name from class
                var properties = typeof(T).GetProperties();
                var propertyNameList = new List<string>();
                foreach (var property in properties)
                    propertyNameList.Add(property.Name);

                var reader = data.CreateDataReader();

                // get columns name from schema table
                var schemaColumNameList = new List<string>();
                foreach (DataRow row in reader.GetSchemaTable().Rows)
                    schemaColumNameList.Add(row["ColumnName"].ToString().ToLower());

                // get common name in both schema and class
                var columnNameList = new List<KeyValuePair<int, string>>();
                foreach (var name in propertyNameList)
                    if (schemaColumNameList.Contains(name.ToLower())) columnNameList.Add(new KeyValuePair<int, string>(schemaColumNameList.IndexOf(name.ToLower()), name));

                // read data into object list
                T item;
                while (reader.Read())
                {
                    // init new object
                    item = Activator.CreateInstance<T>();

                    // read data
                    foreach (var column in columnNameList)
                    {
                        var val = reader.GetValue(column.Key);
                        if (val != DBNull.Value) SetPropertyValue(item, column.Value, reader.GetValue(column.Key));
                    }

                    // add to result list
                    result.Add(item);
                }
            }

            return result;
        }

        static void SetPropertyValue(object o, string name, object value)
        {
            PropertyInfo propertyInfo = o.GetType().GetProperty(name);
            if (propertyInfo == null)
            {
                // check if this model using hastable
                propertyInfo = o.GetType().GetProperty("Hashtable");
                if (propertyInfo != null)
                {
                    System.Collections.Hashtable ht = (System.Collections.Hashtable)propertyInfo.GetValue(o, null);
                    if (ht.ContainsKey(name))
                        ht[name] = value;
                    else
                        ht[name] = value;
                }
            }
            else
            {
                propertyInfo.SetValue(o, value, null);
            }
        }

    }
}
