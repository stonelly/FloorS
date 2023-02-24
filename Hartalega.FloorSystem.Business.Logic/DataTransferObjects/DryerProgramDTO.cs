using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    /// <summary>
    /// DTO for DryerProgram
    /// </summary>
    public class DryerProgramDTO
    {
        /// <summary>
        /// Intantiate the DryerProgramDTO
        /// </summary>
        static DryerProgramDTO()
        {

        }
        /// <summary>
        /// DryerProgramId for DryerProgram
        /// </summary>
        private int dryerProgramId;

        public int DryerProgramId
        {
            get { return dryerProgramId; }
            set { dryerProgramId = value; }
        }

        /// <summary>
        /// DryerProgram for DryerProgram
        /// </summary>
        private string dryerProgram;

        public string DryerProgram
        {
            get { return dryerProgram; }
            set { dryerProgram = value; }
        }
    }
}
