using Hartalega.FloorSystem.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class QAIAQReferenceFirstDTO : ICloneable
    {
        //MYAdamas remove recindex due to audit log column compare
        // public int RecIndex { get; set; }
        public int RefID { get; set; }
        public int DefectCategoryTypeId { get; set; }
        public int WTSamplingSize { get; set; }
        public int VTSamplingSize { get; set; }
        public int AQLID { get; set; }
        public string QCTypeId { get; set; }
        public int DefectMinValue { get; set; }
        public int DefectMaxValue { get; set; }
        public string GloveType { get; set; }
        public string GloveCategory { get; set; }
        public bool IsResample { get; set; }
        public int ResampleRound { get; set; }
        public bool IsDeleted { get; set; }
        public Constants.ActionLog ActionType { get; set; }
        public int CustomerTypeId { get; set; }

        public object Clone()
        {
            QAIAQReferenceFirstDTO objqaidto = (QAIAQReferenceFirstDTO)this.MemberwiseClone();
            return objqaidto;
        }
    }
}
