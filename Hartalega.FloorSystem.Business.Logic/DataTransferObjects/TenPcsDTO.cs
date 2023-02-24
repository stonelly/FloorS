using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class TenPcsDTO
    {
        public TenPcsDTO()
        {

        }

        public TenPcsDTO(string minWeight,string maxWeight )
        {
            MinWeight = minWeight;
            MaxWeight = maxWeight;
           
        }
        public string MinWeight { get; set; }
        public string MaxWeight { get; set; }
    }
}
