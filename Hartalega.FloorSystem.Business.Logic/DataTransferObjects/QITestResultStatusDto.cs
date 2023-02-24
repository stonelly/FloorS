using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class QITestResultStatusDto
    {
        public string Stage { get; set; }
        public string QIResult { get; set; }
        public int WasherScanInCount { get; set; }
        public int DryerScanInCount { get; set; }
        public int PTScanInCount { get; set; }
        public int QCScanInCount { get; set; }
        public int QITestResultCount { get; set; }
        public string QCType { get; set; }
    }
}
