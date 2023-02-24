using Hartalega.FloorSystem.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class QITestResultAQLDTO : ICloneable
    {
        public int RecIndex { get; set; }
        public int TestResultID { get; set; }
        public int WTSamplingSize { get; set; }
        public int DefectMinVal { get; set; }
        public int DefectMaxVal { get; set; }
        public int AQLID { get; set; }
        public bool IsDeleted { get; set; }
        public Constants.ActionLog ActionType { get; set; }
        public string QCTypeId { get; set; }
        public int VTSamplingSize { get; set; }
        public int CustomerTypeId { get; set; }
        public object Clone()
        {
            QITestResultAQLDTO objqaidto = (QITestResultAQLDTO)this.MemberwiseClone();
            return objqaidto;
        }
    }
}
