using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Serialization;
namespace Hartalega.FloorSystem.Business.Logic.EventLog
{

    public class EventLogDataField
    {
        public EventLogDataField() { }

        public EventLogDataField(string resourcesKey, string fieldValue)
        {
            ResourcesKey = resourcesKey;
            FieldValue = fieldValue;
        }

        [XmlElement("ResourcesKey")]
        public string ResourcesKey { get; set; }

        [XmlElement("FieldValue")]
        public string FieldValue { get; set; }

        [XmlElement("Display")]
        public bool Display { get; set; }
    }
}
