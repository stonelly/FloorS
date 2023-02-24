using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hartalega.FloorSystem.Framework;
namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class BrandLineDTO
    {
        public int AVABRANDLINE_ID { get; set; }
        public string CUSTOMERSIZE { get; set; }
        public int GLOVESINNERBOXNO { get; set; }
        public double? GROSSWEIGHT { get; set; }
        public string HARTALEGACOMMONSIZE { get; set; }
        public int INNERBOXINCASENO { get; set; }
        public string ITEMID { get; set; }
        public double? NETWEIGHT { get; set; }
        public double? PACKAGINGWEIGHT { get; set; }
        public string REFERENCE1 { get; set; }
        public string REFERENCE2 { get; set; }
        public string BASEUNIT { get; set; }
        public string INNERPRODUCTCODE { get; set; }
        public string OUTERPRODUCTCODE { get; set; }
        public string COMPANYCATEGORYCODE { get; set; }
        public int NUMOFBASEUNITPIECE { get; set; }
        public int? PACKPCSPERHR { get; set; }
        public int? NUMOFPACKERS { get; set; }     
        public int STOPPED { get; set; }
        public Constants.ActionLog ActionType { get; set; }
        public string PRINTINGSIZE { get; set; }
    }
}
