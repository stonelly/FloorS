using Hartalega.FloorSystem.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class ProductionDefectMasterDTO : ICloneable
    {
        public int RecIndex { get; set; }
        public int ProdDefectId { get; set; }
        public string ProdDefectName { get; set; }
        public string DefectDescription { get; set; }
        public Boolean IsDeleted { get; set; }
        public Constants.ActionLog ActionType { get; set; }
     

        public object Clone()
        {
            ProductionDefectMasterDTO objqaidto = (ProductionDefectMasterDTO)this.MemberwiseClone();
            return objqaidto;
        }
    }
}
