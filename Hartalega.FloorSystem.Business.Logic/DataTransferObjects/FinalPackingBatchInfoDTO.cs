using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
   public class FinalPackingBatchInfoDTO
    {

        [XmlAttribute]
        public decimal SerialNumber	 {get; set;}
       [XmlAttribute]    
        public string BatchNumber { get; set;}
        [XmlAttribute]
        public int BoxesPacked	 {get; set;}
        [XmlAttribute]
        public int CasesPacked {get; set;}
        [XmlAttribute]
        public int PreshipmentCasesPacked {get; set;}       
        [XmlAttribute]
        public string InternalLotNumber { get; set; }
        [XmlAttribute]
        public int TotalPcs { get; set; }
        [XmlAttribute]
        public Boolean IsTempPack { get; set; }
    }
}
