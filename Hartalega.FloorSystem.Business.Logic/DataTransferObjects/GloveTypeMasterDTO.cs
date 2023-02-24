using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Hartalega.FloorSystem.Framework;
using static Hartalega.FloorSystem.Framework.Constants;

namespace Hartalega.FloorSystem.Business.Logic
{
    public  class GloveTypeMasterDTO
    {
        public int AVAGLOVECODETABLE_ID { get; set; }
        public string BARCODE { get; set; }
        public string GLOVECODE { get; set; }
        public string DESCRIPTION { get; set; }
        public string GLOVECATEGORY { get; set; }
        public int PROTEIN { get; set; }
        public decimal PROTEINSPEC { get; set; }
        public int POWDER { get; set; }
        public int POWDERFORMULA { get; set; }
        public int HOTBOX { get; set; }
        public int POLYMER { get; set; }
       
        public int STOPPED { get; set; }
        public ActionLog ActionType { get; set; }
    }
}
