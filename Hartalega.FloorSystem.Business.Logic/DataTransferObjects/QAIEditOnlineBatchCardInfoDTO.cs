using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class QAIEditOnlineBatchCardInfoDTO
    {
        public int Batch_TotalPCs { get; set; }
        public decimal Batch_BatchWeight { get; set; } 
        public int QAI_InnerBox { get; set; }
        public int QAI_PackingSize { get; set; }
    }
}
