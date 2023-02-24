using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class PalletInfoDTO
    {

        public int locationId { set; get; }
        public string OperatorId { set; get; }
        public string Workstationnumber { set; get; }
        public string Packdate { set; get; }
        public string Palletid { set; get; }
        public string orderNumber { get; set; }
        public string Ponumber { set; get; }
        public string ItemName { set; get; }
        public string Size { set; get; }
        public int Casespacked { set; get; }
        public string Name { set; get; }
        public string CaseList { set; get; }
        public Boolean Isavailable { set; get;}
        public Boolean ispreshipmentCase { set; get;}
        public string ItemNumber { set; get; }
    }
}
