using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hartalega.FloorSystem.Framework.XSDDatabase;
using System.Data;
using Hartalega.FloorSystem.Framework.XSDDatabase.SecurityModuleDataXSDTableAdapters;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Database;

namespace Hartalega.FloorSystem.Business.Logic
{
    public class SecurityModuleBLL
    {
        /// <summary>
        /// GetEmployeeDetailsByPassword
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static DataTable GetEmployeeDetailsByPassword(string password)
        {
            usp_GetEmployeeDataByPasswordTableAdapter objEmp = new usp_GetEmployeeDataByPasswordTableAdapter();
            return objEmp.GetEmployeeDataByPassword(password);
        }

        /// <summary>
        /// ValidateEmployeeCredential
        /// </summary>
        /// <param name="password"></param>
        /// <param name="screenName"></param>
        /// <returns></returns>
        public static bool ValidateEmployeeCredential(string password, string screenName)
        {
            //Step-1: get permission details for screen name
            usp_GetScreenPermissionDetailsByScreenNameTableAdapter objScreenPermission = new usp_GetScreenPermissionDetailsByScreenNameTableAdapter();
            DataTable dtScreenPermission = objScreenPermission.GetScreenPermissionDetailByScreenName(screenName);

            //Step-2: get user module-permission access for selected password
            usp_GetUserPermissionDetailsByPasswordTableAdapter objUserPermission = new usp_GetUserPermissionDetailsByPasswordTableAdapter();
            DataTable dtUserPermission = objUserPermission.GetUserPermissionDetailByPassword(password);

            //Step-3: validate module access and permission 
            if ((dtScreenPermission.Rows.Count > 0) & (dtUserPermission.Rows.Count > 0))
            {
                string _screenId = dtScreenPermission.Rows[0]["ScreenId"].ToString();
                string _moduleId = dtScreenPermission.Rows[0]["ModuleId"].ToString();
                string _permissionOperator = dtScreenPermission.Rows[0]["PermissionOperator"].ToString();
                int _permissionSeq = Convert.ToInt32(dtScreenPermission.Rows[0]["PermissionSeq"].ToString());

                int _userPermissionSeq = 0;
                DataRow[] result = dtUserPermission.Select("ModuleId =" + _moduleId + " AND ScreenId = " + _screenId);

                foreach (DataRow row in result)
                {
                    _userPermissionSeq = Convert.ToInt32(row["PermissionSeq"].ToString());
                    return CompareUserandScreenPermission(_userPermissionSeq, _permissionSeq, _permissionOperator);
                }
                return false;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// ValidateOperatorAccess
        /// </summary>
        /// <param name="operatorId"></param>
        /// <param name="screenName"></param>
        /// <returns></returns>
        public static bool ValidateOperatorAccess(string operatorId, string screenName)
        {
            //Step-1: get permission details for screen name
            usp_GetOperatorPermissionDetailsByScreenNameTableAdapter objScreenPermission = new usp_GetOperatorPermissionDetailsByScreenNameTableAdapter();
            DataTable dtScreenPermission = objScreenPermission.GetOperatorPermissionDetailsByScreenName(screenName);

            //Step-2: get operator module-permission access for operator id
            usp_GetUserPermissionDetailsByOperatorIdTableAdapter objUserPermission = new usp_GetUserPermissionDetailsByOperatorIdTableAdapter();
            DataTable dtUserPermission = objUserPermission.GetUserPermissionDetailsByOperatorId(operatorId);

            //Step-3: validate module access and permission 
            if ((dtScreenPermission.Rows.Count > 0) & (dtUserPermission.Rows.Count > 0))
            {
                string _screenId = dtScreenPermission.Rows[0]["ScreenId"].ToString();
                string _moduleId = dtScreenPermission.Rows[0]["ModuleId"].ToString();
                string _permissionOperator = dtScreenPermission.Rows[0]["PermissionOperator"].ToString();
                int _permissionSeq = Convert.ToInt32(dtScreenPermission.Rows[0]["PermissionSeq"].ToString());

                int _userPermissionSeq = 0;
                DataRow[] result = dtUserPermission.Select("ModuleId =" + _moduleId + " AND ScreenId = " + _screenId);

                foreach (DataRow row in result)
                {
                    _userPermissionSeq = Convert.ToInt32(row["PermissionSeq"].ToString());
                    return CompareUserandScreenPermission(_userPermissionSeq, _permissionSeq, _permissionOperator);
                }
                return false;
            }
            else
            {
                return false;
            }

            //return true;
        }

        /// <summary>
        /// CompareUserandScreenPermission
        /// </summary>
        /// <param name="userPermission"></param>
        /// <param name="screenPermission"></param>
        /// <param name="compareOperator"></param>
        /// <returns></returns>
        public static Boolean CompareUserandScreenPermission(int userPermission, int screenPermission, string compareOperator)
        {
            switch (compareOperator)
            {
                case ">":
                    return (userPermission > screenPermission) ? true : false;
                case "<":
                    return (userPermission < screenPermission) ? true : false;
                case "=":
                    return (userPermission == screenPermission) ? true : false;
                case ">=":
                    return (userPermission >= screenPermission) ? true : false;
                case "<=":
                    return (userPermission <= screenPermission) ? true : false;
                default:
                    return false;
            }
        }

        /// <summary>
        /// GetRoleMasterDetails
        /// </summary>
        /// <returns></returns>
        public static DataTable GetRoleMasterDetails()
        {
            usp_GetRoleMasterDataTableAdapter objModuleMaster = new usp_GetRoleMasterDataTableAdapter();
            return objModuleMaster.GetRoleModuleMasterData();
        }

        /// <summary>
        /// GetRoleMasterData
        /// </summary>
        /// <returns></returns>
        public static List<DropdownDTO> GetRoleMasterData()
        {
            List<DropdownDTO> roleList = new List<DropdownDTO>();
            DataTable dt = GetRoleMasterDetails();

            if (dt != null && dt.Rows.Count != 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    roleList.Add(new DropdownDTO() { IDField = Convert.ToString(dr["RoleId"]), DisplayField = Convert.ToString(dr["Role"]) });
                }
            }

            return roleList;
        }

        /// <summary>
        /// GetModuleMasterDetails
        /// </summary>
        /// <returns></returns>
        public static DataTable GetModuleMasterDetails()
        {
            usp_getModuleNamesAndIdTableAdapter objModuleMaster = new usp_getModuleNamesAndIdTableAdapter();
            return objModuleMaster.GetModuleMasterData();
        }

        /// <summary>
        /// GetModuleMasterData
        /// </summary>
        /// <returns></returns>
        public static List<DropdownDTO> GetModuleMasterData()
        {
            List<DropdownDTO> moduleList = new List<DropdownDTO>();
            DataTable dt = GetModuleMasterDetails();


            if (dt != null && dt.Rows.Count != 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    moduleList.Add(new DropdownDTO() { IDField = Convert.ToString(dr["ModuleId"]), DisplayField = Convert.ToString(dr["ModuleName"]) });
                }
            }

            return moduleList;
        }

        /// <summary>
        /// GetScreenMasterByModuleId
        /// </summary>
        /// <param name="ModuleId"></param>
        /// <returns></returns>
        public static List<DropdownDTO> GetScreenMasterByModuleId(int ModuleId)
        {
            List<DropdownDTO> ScreenList = new List<DropdownDTO>();
            usp_GetScreenMasterByModuleIdTableAdapter objScreenMaster = new usp_GetScreenMasterByModuleIdTableAdapter();
            DataTable dt = objScreenMaster.GetScreenMasterData(ModuleId);

            if (dt != null && dt.Rows.Count != 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ScreenList.Add(new DropdownDTO() { IDField = Convert.ToString(dr["ScreenId"]), DisplayField = Convert.ToString(dr["ScreenName"]) });
                }
            }

            return ScreenList;
        }

        /// <summary>
        /// GetPermissionMasterDetails
        /// </summary>
        /// <returns></returns>
        public static DataTable GetPermissionMasterDetails()
        {
            usp_GetPermissionMasterDetailsTableAdapter objPermission = new usp_GetPermissionMasterDetailsTableAdapter();
            DataTable dt = objPermission.GetPermissionMasterData();
            return dt;
        }

        /// <summary>
        /// GetPermissionMaster
        /// </summary>
        /// <returns></returns>
        public static List<DropdownDTO> GetPermissionMaster()
        {
            List<DropdownDTO> PermissionList = new List<DropdownDTO>();
            DataTable dt = GetPermissionMasterDetails();

            if (dt != null && dt.Rows.Count != 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    PermissionList.Add(new DropdownDTO() { IDField = Convert.ToString(dr["PermissionId"]), DisplayField = Convert.ToString(dr["PermissionSeq"]) });
                }
            }

            return PermissionList;
        }

        /// <summary>
        /// InsertModuleScreenPermissionMapping
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="screenId"></param>
        /// <param name="permissionOperator"></param>
        /// <param name="permissionId"></param>
        /// <param name="workStationNumber"></param>
        /// <returns></returns>
        public static bool InsertModuleScreenPermissionMapping(int moduleId, int screenId, string permissionOperator, int permissionId, string workStationNumber)
        {
            InsertModuleScreenPermissionMapping objInertPageMaster = new InsertModuleScreenPermissionMapping();
            int returnValue = objInertPageMaster.InsertPageMasterData(moduleId, screenId, permissionOperator, permissionId, workStationNumber);
            return Convert.ToBoolean(returnValue);
        }

        /// <summary>
        /// InsertOperatorModulePermissionMapping
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="screenId"></param>
        /// <param name="permissionOperator"></param>
        /// <param name="permissionId"></param>
        /// <param name="workStationNumber"></param>
        /// <returns></returns>
        public static bool InsertOperatorModulePermissionMapping(int moduleId, int screenId, string permissionOperator, int permissionId, string workStationNumber)
        {
            InsertModuleScreenPermissionMapping objInertPageMaster = new InsertModuleScreenPermissionMapping();
            int returnValue = objInertPageMaster.InsertOperatorModulePermissionMapping(moduleId, screenId, permissionOperator, permissionId, workStationNumber);
            return Convert.ToBoolean(returnValue);
        }

        /// <summary>
        /// UpdateModuleScreenPermissionMapping
        /// </summary>
        /// <param name="moduleScreenMappingId"></param>
        /// <param name="moduleId"></param>
        /// <param name="screenId"></param>
        /// <param name="permissionOperator"></param>
        /// <param name="permissionId"></param>
        /// <param name="workStationNumber"></param>
        /// <returns></returns>
        public static bool UpdateModuleScreenPermissionMapping(int moduleScreenMappingId, int moduleId, int screenId, string permissionOperator, int permissionId, string workStationNumber)
        {
            InsertModuleScreenPermissionMapping objUpdatePageMaster = new InsertModuleScreenPermissionMapping();
            int returnValue = objUpdatePageMaster.UpdateModuleScreenPermissionMapping(moduleId, screenId, permissionOperator, permissionId, workStationNumber, moduleScreenMappingId);
            return Convert.ToBoolean(returnValue);
        }

        /// <summary>
        /// UpdateOperatorModulePermissionMapping
        /// </summary>
        /// <param name="moduleScreenMappingId"></param>
        /// <param name="moduleId"></param>
        /// <param name="screenId"></param>
        /// <param name="permissionOperator"></param>
        /// <param name="permissionId"></param>
        /// <param name="workStationNumber"></param>
        /// <returns></returns>
        public static bool UpdateOperatorModulePermissionMapping(int operatorModulePermissionMappingId, int moduleId, int screenId, string permissionOperator, int permissionId, string workStationNumber)
        {
            InsertModuleScreenPermissionMapping objUpdatePageMaster = new InsertModuleScreenPermissionMapping();
            int returnValue = objUpdatePageMaster.UpdateOperatorModulePermissionMapping(moduleId, screenId, permissionOperator, permissionId, workStationNumber, operatorModulePermissionMappingId);
            return Convert.ToBoolean(returnValue);
        }

        /// <summary>
        /// GetPageMasterDetails
        /// </summary>
        /// <returns></returns>
        public static DataTable GetPageMasterDetails()
        {
            usp_GET_PageMasterDetailsTableAdapter objPageMasterAdapter = new usp_GET_PageMasterDetailsTableAdapter();
            return objPageMasterAdapter.GETPageMasterDetails();
        }

        /// <summary>
        /// to get GetOperatorModulePermissionMappingDetails
        /// </summary>
        /// <returns></returns>
        public static DataTable GetOperatorModulePermissionMappingDetails()
        {
            usp_GET_OperatorModulePermissionMappingTableAdapter objPageMasterAdapter = new usp_GET_OperatorModulePermissionMappingTableAdapter();
            return objPageMasterAdapter.usp_GET_OperatorModulePermissionMapping();
        }

        /// <summary>
        /// InsertPageMasterDetails
        /// </summary>
        /// <param name="permissionSeq"></param>
        /// <param name="permissionDescription"></param>
        /// <param name="workStationNumber"></param>
        /// <returns></returns>
        public static bool InsertPageMasterDetails(int permissionSeq, string permissionDescription, string workStationNumber)
        {

            InsertModuleScreenPermissionMapping objInertPermissionMaster = new InsertModuleScreenPermissionMapping();
            int returnValue = objInertPermissionMaster.InsertPermissionMasterDetails(permissionSeq, permissionDescription, workStationNumber);
            return Convert.ToBoolean(returnValue);
        }

        /// <summary>
        /// UpdatePageMasterDetails
        /// </summary>
        /// <param name="permissionId"></param>
        /// <param name="permissionSeq"></param>
        /// <param name="permissionDescription"></param>
        /// <param name="workStationNumber"></param>
        /// <returns></returns>
        public static bool UpdatePageMasterDetails(int permissionId, int permissionSeq, string permissionDescription, string workStationNumber)
        {
            InsertModuleScreenPermissionMapping objUpdatePermissionMaster = new InsertModuleScreenPermissionMapping();
            int returnValue = objUpdatePermissionMaster.UpdatePermissionMasterDetails(permissionSeq, permissionDescription, workStationNumber, permissionId);
            return Convert.ToBoolean(returnValue);
        }

        /// <summary>
        /// GetRoleMaintenanceDetails
        /// </summary>
        /// <returns></returns>
        public static DataTable GetRoleMaintenanceDetails()
        {
            usp_GetRoleMasterDataTableAdapter objRoleMasterAdap = new usp_GetRoleMasterDataTableAdapter();
            DataTable dt = objRoleMasterAdap.GetRoleModuleMasterData();

            return dt;
        }

        /// <summary>
        /// InsertRoleMaintenanceData
        /// </summary>
        /// <param name="role"></param>
        /// <param name="roleDescription"></param>
        /// <param name="permissionId"></param>
        /// <param name="workStationNumber"></param>
        /// <param name="moduleIds"></param>
        /// <returns></returns>
        public static Boolean InsertRoleMaintenanceData(string role, string roleDescription, int permissionId, string workStationNumber, string moduleIds)
        {
            usp_Insert_RoleMaintenanceDataTableAdapter objInsert = new usp_Insert_RoleMaintenanceDataTableAdapter();
            objInsert.GetRoleMaintenanceData(role, roleDescription, permissionId, workStationNumber, moduleIds);
            return true;
        }

        /// <summary>
        /// UpdateRoleMaintenanceData
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="role"></param>
        /// <param name="roleDescription"></param>
        /// <param name="permissionId"></param>
        /// <param name="workStationNumber"></param>
        /// <param name="moduleIds"></param>
        /// <returns></returns>
        public static Boolean UpdateRoleMaintenanceData(int roleId, string role, string roleDescription, int permissionId, string workStationNumber, string moduleIds)
        {
            usp_Update_RoleMaintenanceDataTableAdapter objUpdateData = new usp_Update_RoleMaintenanceDataTableAdapter();
            objUpdateData.GetUpdateRoleMaintenanceData(roleId, role, roleDescription, permissionId, workStationNumber, moduleIds);
            return true;
        }

        /// <summary>
        /// InsertEmployeeMasterData
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="roleId"></param>
        /// <param name="workStationNumber"></param>
        /// <returns></returns>
        public static Boolean InsertEmployeeMasterData(string employeeId, string name, string password, int roleId, string workStationNumber)
        {
            //usp_Insert_EmployeeMasterDataTableAdapter objInsertEmp = new usp_Insert_EmployeeMasterDataTableAdapter();
            InsertModuleScreenPermissionMapping objInsertEmp = new InsertModuleScreenPermissionMapping();
            objInsertEmp.InsertEmployeeMasterData(employeeId, name, password, roleId, workStationNumber);
            return true;
        }

        /// <summary>
        /// UpdateEmployeeMasterData
        /// </summary>
        /// <param name="eId"></param>
        /// <param name="employeeId"></param>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="roleId"></param>
        /// <param name="workStationNumber"></param>
        /// <returns></returns>
        public static Boolean UpdateEmployeeMasterData(int eId, string employeeId, string name, string password, int roleId, string workStationNumber)
        {
            //usp_Update_EmployeeMasterDataTableAdapter objUpdate = new usp_Update_EmployeeMasterDataTableAdapter();
            InsertModuleScreenPermissionMapping objUpdateEmp = new InsertModuleScreenPermissionMapping();
            objUpdateEmp.UpdateEmployeeMasterData(employeeId, name, password, roleId, workStationNumber, eId);
            return true;
        }

        /// <summary>
        /// GetEmployeeMasterDetails
        /// </summary>
        /// <returns></returns>
        public static DataTable GetEmployeeMasterDetails()
        {
            usp_GET_EmployeeMasterDataTableAdapter objEmp = new usp_GET_EmployeeMasterDataTableAdapter();
            DataTable dt = objEmp.GetEmployeeMasterData();
            return dt;
        }

        /// <summary>
        /// GetEmployeeMasterDataByEmpId
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public static DataTable GetEmployeeMasterDataByEmpId(int employeeId)
        {
            usp_GET_EmployeeMasterDataByEmployeeIdTableAdapter objEmpbyId = new usp_GET_EmployeeMasterDataByEmployeeIdTableAdapter();
            return objEmpbyId.GetEmployeeMasterDataByEmpId(employeeId);
        }

        /// <summary>
        /// GenerateRandomPassword
        /// </summary>
        /// <returns></returns>
        public static string GenerateRandomPassword()
        {
            string pin = string.Empty;
            usp_GenerateRandomPasswordTableAdapter objGetPin = new usp_GenerateRandomPasswordTableAdapter();
            DataTable dt = objGetPin.GetRandomPassword();
            if (dt.Rows.Count > 0)
            {
                pin = dt.Rows[0]["PIN"].ToString();
            }
            return pin;
        }

        /// <summary>
        /// SplitintoLine
        /// </summary>
        /// <param name="lineval"></param>
        /// <returns></returns>
        private static string SplitintoLine(string lineval)
        {
            StringBuilder sb = new StringBuilder("");
            if (lineval.Length > 0)
            {
                //lineval wil have delimiter '~' to separate line even at the end of string has '~'
                string[] rowValues = lineval.Substring(0, lineval.Length - 1).Split('~');
                for (int i = 0; i < rowValues.Length; i++)
                {
                    sb.Append(rowValues[i]);
                    if (i + 1 != rowValues.Length)
                    {
                        sb.Append(Environment.NewLine);
                    }
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// IsEmployeeAlreadyExist
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public static Boolean IsEmployeeAlreadyExist(string employeeId)
        {
            usp_GetEmployeeDetailByEmployeeIdTableAdapter objEmpCount = new usp_GetEmployeeDetailByEmployeeIdTableAdapter();
            DataTable dt = objEmpCount.GetEmployeeDetailByEmployeeId(employeeId);
            return (dt.Rows.Count > 0) ? true : false;
        }

        /// <summary>
        /// IsPermissionSequenceAlreadyExist
        /// </summary>
        /// <param name="permissionSeq"></param>
        /// <returns></returns>
        public static Boolean IsPermissionSequenceAlreadyExist(string permissionSeq)
        {
            usp_GetPermissionDetailByPermissionSeqTableAdapter objPerSeqCount = new usp_GetPermissionDetailByPermissionSeqTableAdapter();
            DataTable dt = objPerSeqCount.GetPermissionDetailsByPermissionSeq(permissionSeq);
            return (dt.Rows.Count > 0) ? true : false;
        }

        /// <summary>
        /// IsModuleScreenMappingExist
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="screenId"></param>
        /// <returns></returns>
        public static Boolean IsModuleScreenMappingExist(int moduleId, int screenId)
        {
            usp_Get_IsModuleScreenMappingExistTableAdapter objModuleMapping = new usp_Get_IsModuleScreenMappingExistTableAdapter();
            DataTable dt = objModuleMapping.GetIsModuleScreenMappingExist(moduleId, screenId);
            return (dt.Rows.Count > 0) ? true : false;
        }

        /// <summary>
        /// To check IsOperatorModulePermissionMappingExist
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="screenId"></param>
        /// <returns></returns>
        public static Boolean IsOperatorModulePermissionMappingExist(int moduleId, int screenId)
        {
            usp_Get_IsOperatorModulePermissionMappingExistTableAdapter objModuleMapping = new usp_Get_IsOperatorModulePermissionMappingExistTableAdapter();
            DataTable dt = objModuleMapping.Get_IsOperatorModulePermissionMappingExist(moduleId, screenId);
            return (dt.Rows.Count > 0) ? true : false;
        }

        /// <summary>
        /// IsRoleAlreadyExist
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public static Boolean IsRoleAlreadyExist(string roleName)
        {
            usp_IsRoleAlreadyExistTableAdapter objRole = new usp_IsRoleAlreadyExistTableAdapter();
            DataTable dt = objRole.GetIsRoleAlreadyExist(roleName);
            return (dt.Rows.Count > 0) ? true : false;
        }

        /// <summary>
        /// IsPasswordAlreadyExist
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static Boolean IsPasswordAlreadyExist(string password)
        {
            DataTable dt = SecurityModuleBLL.GetEmployeeDetailsByPassword(password);
            return (dt.Rows.Count > 0) ? true : false;
        }
    }
}
