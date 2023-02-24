using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Serialization;
namespace Hartalega.FloorSystem.Business.Logic.EventLog
{
    public class EventLogDataFieldContainer
    {
        [XmlArray("DataFields")]
        [XmlArrayItem("DataField")]
        public EventLogDataField[] DataFields { get; set; }
    }
}
