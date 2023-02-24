using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Hartalega.FloorSystem.Framework;
namespace Hartalega.FloorSystem.Business.Logic
{
    public  class GloveCategoryMasterDTO
    {
        public string Description { get; set; }
        public string GloveCategory { get; set; }
        public int GloveCategoryId { get; set; }
        public Constants.ActionLog ActionType { get; set; }

        public int Stopped { get; set; }
         

    }
}
