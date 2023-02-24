using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Hartalega.FloorSystem.Framework;
namespace Hartalega.FloorSystem.Business.Logic
{
    public  class GloveTypeSizeRelationMasterDTO
    {
        public int AVAGLOVERELCOMSIZE_ID { get; set; }
        public string COMMONSIZE { get; set; }
        public decimal GLOVEWEIGHT { get; set; }
        public decimal MAX10PCSWT { get; set; }
        public decimal MIN10PCSWT { get; set; }
        public int Stopped { get; set; }
        public int GLOVETYPEID { get; set; }

        public Constants.ActionLog ActionType { get; set; }
    }
}
