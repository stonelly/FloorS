using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Diagnostics;
using System.Configuration;

namespace Hartalega.FloorSystem.Business.Logic
{
    public class PSIReplacementDLL : Framework.Business.BusinessBase
    {
        public static void DeductStockFromPSIWarehouse(decimal serialNumber, int quantity, Guid warehouseId)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@serialNumber", serialNumber));
            lstParameters.Add(new FloorSqlParameter("@quantity", quantity));
            lstParameters.Add(new FloorSqlParameter("@warehouseId", warehouseId));
            Framework.Database.FloorDBAccess.ExecuteNonQuery("USP_DeductStockFromPSIWarehouse", lstParameters);
        }

        public static Guid? GetDefaultPSIWarehouse(int locationId)
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            lstParameters.Add(new FloorSqlParameter("@locationId", locationId));
            object result = Framework.Database.FloorDBAccess.ExecuteScalar("USP_GET_DefaultPSIWarehouse", lstParameters);
            if (string.IsNullOrEmpty(Convert.ToString(result)))
            {
                return null;
            }
            else
                return new Guid(Convert.ToString(result));
        }
    }
}
