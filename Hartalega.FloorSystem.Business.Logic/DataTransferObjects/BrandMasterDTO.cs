using Hartalega.FloorSystem.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class BrandMasterDTO
    {
        public string ITEMID { get; set; }
        public string BRANDNAME { get; set; }
        public string GLOVECODE { get; set; }
        public int ACTIVE { get; set; }
        public string INNERLABELSET { get; set; }
        public int? EXPIRY { get; set; }
        public int? SPECIALINNERCODE { get; set; }
        public string SPECIALINNERCHARACTER { get; set; }
        public int LOTVERIFICATION { get; set; }
        public int? INNERPRINTER { get; set; }
        public int? MANUFACTURINGDATEON { get; set; }
        public string OUTERLABELSETNO { get; set; }
        public int? GCLABEL { get; set; }
        public int? PRESHIPMENTPLAN { get; set; }
        public int? PALLETCAPACITY { get; set; }
        public string ALTERNATEGLOVECODE1 { get; set; }
        public string ALTERNATEGLOVECODE2 { get; set; }
        public string ALTERNATEGLOVECODE3 { get; set; }
        public string INNERLABELSETDATEFORMAT { get; set; }
        public string OUTERLABELSETDATEFORMAT { get; set; }
        public Constants.ActionLog ActionType { get; set; }

        public int? NOOFLABELPRINT { get; set; }
    }
}
