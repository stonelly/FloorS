using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hartalega.FloorSystem.Framework;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class GloveSizeMasterDTO
    {
        public string CommonSize { get; set; }

        public string Description { get; set; }

        //public int RecIndex { get; set; }

        public int CommonSizeID { get; set; }
        public Constants.ActionLog ActionType { get; set; }

        public int Stopped { get; set; }

        public string OldSize { get; set; }
    }
}
