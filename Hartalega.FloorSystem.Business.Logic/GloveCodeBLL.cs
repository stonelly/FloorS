using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using System.Data;
using Hartalega.FloorSystem.Framework.Database;
using Hartalega.FloorSystem.Framework;

namespace Hartalega.FloorSystem.Business.Logic
{
    public static class GloveCodeBLL
    {
        private static List<FloorSqlParameter> _params;

        /// <summary>
        /// Fetches Glove Code Details 
        /// </summary>
        /// <returns>GloveCodeDTO List</returns>
        public static List<GloveCodeDTO> GetGloveCodeDetails(string gloveCode = "", string gloveCategory = "", string barcode = "",string Cond1="",string Cond2="")
        {
            DataTable dtGloveCode;
            if (!(string.IsNullOrEmpty(gloveCode) && string.IsNullOrEmpty(gloveCategory) && string.IsNullOrEmpty(barcode) && string.IsNullOrEmpty(Cond1) && string.IsNullOrEmpty(Cond2)))
            {
                List<FloorSqlParameter> fspList = new List<FloorSqlParameter>();
                fspList.Add(new FloorSqlParameter("@gloveCode", gloveCode));
                fspList.Add(new FloorSqlParameter("@barcode", barcode));
                fspList.Add(new FloorSqlParameter("@gloveCategory", gloveCategory));
                fspList.Add(new FloorSqlParameter("@cond1", Cond1));
                fspList.Add(new FloorSqlParameter("@cond2", Cond2));
                dtGloveCode = FloorDBAccess.ExecuteDataTable("USP_SEL_GloveCodeByCondition", fspList);
            }
            else
            {
                dtGloveCode = FloorDBAccess.ExecuteDataTable("USP_SEL_GloveCodeDetails");
            }

            List<GloveCodeDTO> gloveCodeDTOList = null;

                if (dtGloveCode != null && dtGloveCode.Rows.Count > Constants.ZERO)
                {
                    gloveCodeDTOList = (from DataRow dr in dtGloveCode.Rows
                                                 select new GloveCodeDTO
                                                 {
                                                     Id = FloorDBAccess.GetValue<int>(dr, "AVAGLOVECODETABLE_ID"),
                                                     GloveCode = FloorDBAccess.GetString(dr, "GLOVECODE"),
                                                     Description = FloorDBAccess.GetString(dr, "DESCRIPTION"),
                                                     GloveCategory = FloorDBAccess.GetString(dr, "GLOVECATEGORY"),
                                                     Barcode = FloorDBAccess.GetString(dr, "BARCODE"),
                                                     Hotbox = FloorDBAccess.GetValue<int>(dr, "HOTBOX"),
                                                     Polymer = FloorDBAccess.GetValue<int>(dr, "POLYMER"),
                                                     Powder = FloorDBAccess.GetValue<int>(dr, "POWDER"),
                                                     Protein = FloorDBAccess.GetValue<int>(dr, "PROTEIN"),
                                                     ProteinSpecification = FloorDBAccess.GetValue<decimal>(dr, "PROTEINSPEC"),
                                                     PowderFormula = FloorDBAccess.GetValue<int>(dr, "POWDERFORMULA")
                                                 }).ToList();
                }
            
            return gloveCodeDTOList;
        }

        public static List<QCTypeDTO> GetQCTypeDetails(string gloveCode )
        {
            List<QCTypeDTO> qcTypeDTOList = null;
            if (string.IsNullOrEmpty(gloveCode))
                return qcTypeDTOList;

            List<FloorSqlParameter> fspList = new List<FloorSqlParameter>();
            fspList.Add(new FloorSqlParameter("@gloveCode", gloveCode));
            using (DataTable dtGloveCode = FloorDBAccess.ExecuteDataTable("USP_SEL_QCTypeDetails",fspList))
            {
                if (dtGloveCode != null && dtGloveCode.Rows.Count > Constants.ZERO)
                {
                    qcTypeDTOList = (from DataRow dr in dtGloveCode.Rows
                                        select new QCTypeDTO
                                        {
                                            QcTypeId = FloorDBAccess.GetValue<int>(dr, "AVAGLOVERELQCTYPE_ID"),
                                            DescId = FloorDBAccess.GetValue<int>(dr, "AVAQCTYPETABLE_ID"),
                                            QCType = FloorDBAccess.GetString(dr, "QCTYPE"),
                                            Description = FloorDBAccess.GetString(dr, "DESCRIPTION"),
                                            NumOfTester = FloorDBAccess.GetValue<int>(dr, "NUMOFTESTER"),
                                            PiecesHR = FloorDBAccess.GetValue<int>(dr, "PIECESHR"),

                                        }).ToList();
                }
            }
            return qcTypeDTOList;
        }

        public static List<WasherGloveDTO> GetWasherProgramDetails(string gloveCode)
        {
            List<WasherGloveDTO> washerDTOList = null;
            if (string.IsNullOrEmpty(gloveCode))
                return washerDTOList;

            List<FloorSqlParameter> fspList = new List<FloorSqlParameter>();
            fspList.Add(new FloorSqlParameter("@gloveCode", gloveCode));
            using (DataTable dtGloveCode = FloorDBAccess.ExecuteDataTable("USP_SEL_WasherDetails", fspList))
            {
                if (dtGloveCode != null && dtGloveCode.Rows.Count > Constants.ZERO)
                {
                    washerDTOList = (from DataRow dr in dtGloveCode.Rows
                                     select new WasherGloveDTO
                                     {
                                         GloveCode = FloorDBAccess.GetString(dr, "GLOVECODE"),
                                         WasherId = FloorDBAccess.GetValue<int>(dr, "AVAGLOVERELWASHER_ID"),
                                         WasherProgram = FloorDBAccess.GetString(dr, "WASHERPROGRAM"),    
                                     }).ToList();
                }
            }
            return washerDTOList;
        }

        public static List<DryerGloveDTO> GetDryerProcessDetails(string gloveCode)
        {
            List<DryerGloveDTO> dryerDTOList = null;
            if (string.IsNullOrEmpty(gloveCode))
                return dryerDTOList;

            List<FloorSqlParameter> fspList = new List<FloorSqlParameter>();
            fspList.Add(new FloorSqlParameter("@gloveCode", gloveCode));
            using (DataTable dtGloveCode = FloorDBAccess.ExecuteDataTable("USP_SEL_DryerDetails", fspList))
            {
                if (dtGloveCode != null && dtGloveCode.Rows.Count > Constants.ZERO)
                {
                    dryerDTOList = (from DataRow dr in dtGloveCode.Rows
                                     select new DryerGloveDTO
                                     {
                                         DryerId = FloorDBAccess.GetValue<int>(dr, "AVAGLOVERELCYCLONE_ID"),
                                         DryerProcess = FloorDBAccess.GetString(dr, "DRYERPROCESS"),

                                     }).ToList();
                }
            }
            return dryerDTOList;
        }

        public static List<string> GetQCTypeByGloveCode()
        {
            List<string> qcTypeList = new List<string>();
          
            using (DataTable dtGloveCode = FloorDBAccess.ExecuteDataTable("USP_SEL_QCTypeByRecid"))
            { 
                if (dtGloveCode != null && dtGloveCode.Rows.Count > Constants.ZERO)
                {

                    foreach (DataRow dr in dtGloveCode.Rows)
                    {
                        qcTypeList.Add(FloorDBAccess.GetString(dr, "QCTYPE"));
                    }
                }
            }
            return qcTypeList;

        }

        public static int isQcTypeFormDuplicate(QCTypeDTO objQcTypeDto)
        {
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@qcType", objQcTypeDto.QCType));
            _params.Add(new FloorSqlParameter("@numTester", objQcTypeDto.NumOfTester));
            _params.Add(new FloorSqlParameter("@desc", objQcTypeDto.Description));
            _params.Add(new FloorSqlParameter("@gloveCode", objQcTypeDto.GloveCode));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsQCTypeGloveDuplicate", _params));
            //return 0;

        }

        public static int isQcTypeFormValid(QCTypeDTO objQcTypeDto)
        {
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@qcType", objQcTypeDto.QCType));
            _params.Add(new FloorSqlParameter("@numTester", objQcTypeDto.NumOfTester));
            _params.Add(new FloorSqlParameter("@desc", objQcTypeDto.Description));
            _params.Add(new FloorSqlParameter("@gloveCode", objQcTypeDto.GloveCode));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsQCTypeGloveValid", _params));
            //return 0;

        }


        public static int EditQCTypeForm(QCTypeDTO qCTypeDTO)
        {
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@qcId", qCTypeDTO.QcTypeId));
            _params.Add(new FloorSqlParameter("@qcDescId", qCTypeDTO.DescId));
            _params.Add(new FloorSqlParameter("@gloveCode", qCTypeDTO.GloveCode));
            _params.Add(new FloorSqlParameter("@qcType", qCTypeDTO.QCType));
            _params.Add(new FloorSqlParameter("@numTester", qCTypeDTO.NumOfTester));
            //_params.Add(new FloorSqlParameter("@desc", qCTypeDTO.Description));
            _params.Add(new FloorSqlParameter("@piecesHR", qCTypeDTO.PiecesHR));

            return FloorDBAccess.ExecuteNonQuery("USP_UPD_QCTypeDetails", _params);
        }

        public static int SaveQCTypeForm(QCTypeDTO qCTypeDTO)
        {
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@gloveCode", qCTypeDTO.GloveCode));
            _params.Add(new FloorSqlParameter("@qcType", qCTypeDTO.QCType));
            _params.Add(new FloorSqlParameter("@numTester", qCTypeDTO.NumOfTester));
            //_params.Add(new FloorSqlParameter("@desc", qCTypeDTO.Description));
            _params.Add(new FloorSqlParameter("@piecesHR", qCTypeDTO.PiecesHR));

            return FloorDBAccess.ExecuteNonQuery("USP_INS_QCTypeDetails", _params);
        }

        public static int DeleteQCTypeForm(QCTypeDTO qCTypeDTO)
        {
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@gloveCode", qCTypeDTO.GloveCode));
            _params.Add(new FloorSqlParameter("@qcType", qCTypeDTO.QCType));
            _params.Add(new FloorSqlParameter("@numTester", qCTypeDTO.NumOfTester));
            //_params.Add(new FloorSqlParameter("@desc", qCTypeDTO.Description));
            return FloorDBAccess.ExecuteNonQuery("USP_DEL_QCTypeDetails", _params);
        }

        public static int isWasherFormDuplicate(WasherGloveDTO objQcTypeDto)
        {
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@washerProgram", objQcTypeDto.WasherProgram));
            _params.Add(new FloorSqlParameter("@gloveCode", objQcTypeDto.GloveCode));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsWasherGloveDuplicate", _params));
        }

        public static int isWasherFormValid(WasherGloveDTO objQcTypeDto)
        {
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@washerProgram", objQcTypeDto.WasherProgram));
            _params.Add(new FloorSqlParameter("@gloveCode", objQcTypeDto.GloveCode));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsWasherGloveValid", _params));
        }


        public static int EditWasherForm(WasherGloveDTO washerDTO)
        {
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@washerId", washerDTO.WasherId));
            _params.Add(new FloorSqlParameter("@washerProgram", washerDTO.WasherProgram));
            return FloorDBAccess.ExecuteNonQuery("USP_UPD_WasherDetails", _params);
        }

        public static int SaveWasherForm(WasherGloveDTO washerDTO)
        {
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@gloveCode", washerDTO.GloveCode));
            _params.Add(new FloorSqlParameter("@washerPro", washerDTO.WasherProgram));
            return FloorDBAccess.ExecuteNonQuery("USP_INS_WasherDetails", _params);
        }

        public static int DeleteWasherForm(int washerId)
        {
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@washerId", washerId));
            return FloorDBAccess.ExecuteNonQuery("USP_DEL_WasherDetails", _params);
        }


        public static int isDryerFormDuplicate(DryerGloveDTO objQcTypeDto)
        {
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@dryerProcess", objQcTypeDto.DryerProcess));
            _params.Add(new FloorSqlParameter("@gloveCode", objQcTypeDto.GloveCode));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsDryerGloveDuplicate", _params));
        }

        public static int isDryerFormValid(DryerGloveDTO objQcTypeDto)
        {
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@dryerProcess", objQcTypeDto.DryerProcess));
            _params.Add(new FloorSqlParameter("@gloveCode", objQcTypeDto.GloveCode));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsDryerGloveValid", _params));
        }


        public static int EditDryerForm(DryerGloveDTO dryerDTO)
        {
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@dryerId", dryerDTO.DryerId));
            _params.Add(new FloorSqlParameter("@dryerProcess", dryerDTO.DryerProcess));
            return FloorDBAccess.ExecuteNonQuery("USP_UPD_DryerDetails", _params);
        }

        public static int SaveDryerForm(DryerGloveDTO dryerDTO)
        {
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@gloveCode", dryerDTO.GloveCode));
            _params.Add(new FloorSqlParameter("@dryerProcess", dryerDTO.DryerProcess));
            return FloorDBAccess.ExecuteNonQuery("USP_INS_DryerDetails", _params);
        }

        public static int DeleteDryerForm(int dryerId)
        {
            _params = new List<FloorSqlParameter>();           
            _params.Add(new FloorSqlParameter("@dryerId", dryerId));
            return FloorDBAccess.ExecuteNonQuery("USP_DEL_DryerDetails", _params);
        }
        
        public static int SaveGloveCode(GloveCodeDTO gloveDTO)
        {
            long recId = Convert.ToInt64("70896" + new Random().Next(10000, 99999).ToString());
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@gloveCode", gloveDTO.GloveCode));
            _params.Add(new FloorSqlParameter("@description", gloveDTO.Description));
            _params.Add(new FloorSqlParameter("@gloveCategory", gloveDTO.GloveCategory));
            _params.Add(new FloorSqlParameter("@barcode", gloveDTO.Barcode));
            _params.Add(new FloorSqlParameter("@recId", recId));

            return FloorDBAccess.ExecuteNonQuery("USP_INS_GloveCodeMaster", _params);
        }

        public static int isGloveAddDuplicate(GloveCodeDTO gloveDTO)
        {
            _params = new List<FloorSqlParameter>();
            _params.Add(new FloorSqlParameter("@gloveCode", gloveDTO.GloveCode));
            _params.Add(new FloorSqlParameter("@description", gloveDTO.Description));
            _params.Add(new FloorSqlParameter("@gloveCategory", gloveDTO.GloveCategory));
            _params.Add(new FloorSqlParameter("@barcode", gloveDTO.Barcode));
            return Convert.ToInt16(FloorDBAccess.ExecuteScalar("USP_GET_IsGloveAddDuplicate", _params));
        }

        //public static int DeleteGlove(int GloveId)
        //{
        //    _params = new List<FloorSqlParameter>();
        //    _params.Add(new FloorSqlParameter("@gloveCodeId", GloveId));
        //    return FloorDBAccess.ExecuteNonQuery("USP_DEL_GloveCodeDetails", _params);
        //}

    }
}
