using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
        public class PalletMasterDTO
    {
        public Boolean IsAvailable { get; set; }
        public string LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public Boolean IsPreshipment { get; set; }
        public Boolean Isoccupied { get; set; }
        public string PalletId { get; set; }
        public string Zone { get; set; }
    }

}
