using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// DTO class for DryerScanBatchCard
    /// </summary>
    public class LocationDTO
    {
        public int LocationId
        {
            get;
            internal set;
        }

        public int AreaId
        {
            get;
            internal set;
        }

        public string LocationName
        {
            get;
            internal set;
        }

        public string CompanyName
        {
            get;
            internal set;
        }

        public bool IsDeleted
        {
            get;
            internal set;
        }

        public string Zone
        {
            get;
            internal set;
        }

        public string Area
        {
            get;
            internal set;
        }

        public string LocationAreaCode
        {
            get;
            internal set;
        }
    }
}
