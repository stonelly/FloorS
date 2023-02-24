using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// DTO class for ProductionLoggingActivities
    /// </summary>
    public class ProductionLoggingActivitiesDTO
    {
        /// <summary>
        /// To Instantiate the class
        /// </summary>
        static ProductionLoggingActivitiesDTO()
        {
        }
        /// <summary>
        /// Id
        /// </summary>
        public int Id;
        /// <summary>
        /// ProductionLineId
        /// </summary>
        public string ProductionLineId { get; set; }
        /// <summary>
        /// LineDate
        /// </summary>
        public DateTime LineDate;
        /// <summary>
        /// LineTime
        /// </summary>
        public TimeSpan LineTime { get; set; }
        /// <summary>
        /// Glove
        /// </summary>
        public string Glove;
        /// <summary>
        /// ActivityType
        /// </summary>
        public string ActivityType;
        /// <summary>
        /// ReasonTypeId
        /// </summary>
        public int ReasonTypeId { get; set; }
        /// <summary>
        /// ReasonText
        /// </summary>
        public string ReasonText;
        /// <summary>
        /// Duration
        /// </summary>
        public string Duration;
        /// <summary>
        /// LastModifiedOn
        /// </summary>
        public DateTime LastModifiedOn;
        /// <summary>
        /// Remarks
        /// </summary>
        public string Remarks;
        /// <summary>
        /// IsBatchInsert
        /// </summary>
        public bool IsBatchInsert;

        /// <summary>
        /// ReasonStartStopId
        /// </summary>
        public int ReasonStartStopId { get; set; }

        /// <summary>
        /// Department
        /// </summary>
        public string Department { get; set; }

    }

    public class ProductionLoggingTierDTO
    {
        /// <summary>
        /// Production Line
        /// </summary>
        public string Line;
        /// <summary>
        /// Line Date
        /// </summary>
        public DateTime LineDate;
        /// <summary>
        /// Left Top Glove
        /// </summary>
        public string LTGlove;
        /// <summary>
        /// Left Bottom Glove;
        /// </summary>
        public string LBGlove;
        /// <summary>
        /// Right Top Glove
        /// </summary>
        public string RTGlove;
        /// <summary>
        /// Right Botom Glove
        /// </summary>
        public string RBGlove;
        /// <summary>
        /// Left Top Speed
        /// </summary>
        public int LTSpeed;
        /// <summary>
        /// Left Bottom Speed
        /// </summary>
        public int LBSpeed;
        /// <summary>
        /// Right Top Speed;
        /// </summary>
        public int RTSpeed;
        /// <summary>
        /// Right Bottom Speed;
        /// </summary>
        public int RBSpeed;

        /// <summary>
        /// Lowest speed from all four tier
        /// </summary>
        public int LowestSpeed;

        /// <summary>
        /// If configured tier have conflict on speed.
        /// </summary>
        public bool ConflictSpeed;
    }
}
