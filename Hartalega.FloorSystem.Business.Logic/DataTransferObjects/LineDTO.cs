using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// DTO class for production Line
    /// </summary>
    public class LineDTO
    {
        static LineDTO()
        {

        }
        public string LineNumber { get; set; }
        public int LocationId { get; set; }
        public bool isDeleted { get; set; }
    }
}
