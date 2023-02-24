using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Database;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;

namespace Hartalega.FloorSystem.Business.Logic
{
    public class ProductionSystemReportsBLL : Framework.Business.BusinessBase
    {
        List<FloorSqlParameter> _params;

        public List<DropdownDTO> GetDryerNumber()
        {
            DataTable dt = new DataTable();
            List<DropdownDTO> _dryerMaster = new List<DropdownDTO>();

            dt = FloorDBAccess.ExecuteDataTable("usp_DryerNumber_Get", null);
            dt.Rows.InsertAt(dt.NewRow(), 0);
            try
            {
                if (dt != null && dt.Rows.Count != 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        _dryerMaster.Add(new DropdownDTO() { IDField = Convert.ToString(dr["DryerNumber"]), DisplayField = Convert.ToString(dr["DryerNumber"]) });
                    }
                }
            }
            catch (RowNotInTableException rex)
            {
                throw new FloorSystemException(Messages.ROWNOTINTABLEEXCEPTION, Constants.BUSINESSLOGIC, rex);
            }
            catch (ArgumentException argex)
            {
                throw new FloorSystemException(Messages.ARGUMENTEXCEPTION, Constants.BUSINESSLOGIC, argex);
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.GETLINEMASTEREXCEPTION, Constants.BUSINESSLOGIC, ex);
            }
            return _dryerMaster;
        }

        public static List<DropdownDTO> GetReportDetails()
        {
            List<FloorSqlParameter> lstParameters = new List<FloorSqlParameter>();
            List<DropdownDTO> list = new List<DropdownDTO>(); ;
            using (DataTable dt = FloorDBAccess.ExecuteDataTable("usp_ReportName_Get", null))
            {
                try
                {
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            list.Add(new DropdownDTO() { IDField = Convert.ToString(dr["ReportURL"]), DisplayField = Convert.ToString(dr["ReportName"]) });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new FloorSystemException(Messages.REPORTEXCEPTION, Constants.BUSINESSLOGIC, ex);
                }
            }
            return list;
        }
        public List<DropdownDTO> GetWasherNumber()
        {
            DataTable dt = new DataTable();
            List<DropdownDTO> _washerMaster = new List<DropdownDTO>();

            dt = FloorDBAccess.ExecuteDataTable("usp_WasherNumber_Get", null);
            dt.Rows.InsertAt(dt.NewRow(), 0);
            try
            {
                if (dt != null && dt.Rows.Count != 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        _washerMaster.Add(new DropdownDTO() { IDField = Convert.ToString(dr["WasherNumber"]), DisplayField = Convert.ToString(dr["WasherNumber"]) });
                    }
                }
            }
            catch (RowNotInTableException rex)
            {
                throw new FloorSystemException(Messages.ROWNOTINTABLEEXCEPTION, Constants.BUSINESSLOGIC, rex);
            }
            catch (ArgumentException argex)
            {
                throw new FloorSystemException(Messages.ARGUMENTEXCEPTION, Constants.BUSINESSLOGIC, argex);
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.GETLINEMASTEREXCEPTION, Constants.BUSINESSLOGIC, ex);
            }
            return _washerMaster;
        }

        public List<DropdownDTO> GetZone()
        {
            DataTable dt = new DataTable();
            List<DropdownDTO> _locationMaster = new List<DropdownDTO>();

            dt = FloorDBAccess.ExecuteDataTable("usp_Zone_Get", null);
            dt.Rows.InsertAt(dt.NewRow(), 0);
            try
            {
                if (dt != null && dt.Rows.Count != 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        _locationMaster.Add(new DropdownDTO() { IDField = Convert.ToString(dr["ZoneName"]), DisplayField = Convert.ToString(dr["ZoneName"]) });
                    }
                }
            }
            catch (RowNotInTableException rex)
            {
                throw new FloorSystemException(Messages.ROWNOTINTABLEEXCEPTION, Constants.BUSINESSLOGIC, rex);
            }
            catch (ArgumentException argex)
            {
                throw new FloorSystemException(Messages.ARGUMENTEXCEPTION, Constants.BUSINESSLOGIC, argex);
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.GETLINEMASTEREXCEPTION, Constants.BUSINESSLOGIC, ex);
            }
            return _locationMaster;
        }

        public DataSet GetDryerEfficiency(DateTime from, DateTime to, string dryerNumber, string zone)
        {
            DataSet ds = new DataSet();
            List<DropdownDTO> _lineMaster = new List<DropdownDTO>();
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@from", from));
            _params.Add(new FloorSqlParameter("@to", to));
            _params.Add(new FloorSqlParameter("@dryerNumber", dryerNumber));
            _params.Add(new FloorSqlParameter("@zone", zone));
            return FloorDBAccess.ExecuteDataSet("usp_DryerEfficiency_Get", _params);
        }

        public DataSet GetWasherEfficiency(DateTime from, DateTime to, string washerNumber, string zone)
        {
            DataSet ds = new DataSet();
            List<DropdownDTO> _lineMaster = new List<DropdownDTO>();
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@From", from));
            _params.Add(new FloorSqlParameter("@To", to));
            _params.Add(new FloorSqlParameter("@WasherNumber", washerNumber));
            _params.Add(new FloorSqlParameter("@Zone", zone));
            return FloorDBAccess.ExecuteDataSet("usp_WasherEfficiency_Get", _params);
        }
    }
}
