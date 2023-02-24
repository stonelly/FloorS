using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hartalega.FloorSystem.Framework;
namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class MessagesDTO
    {
      /// <summary>
      /// Message Key
      /// </summary>
        public string MessageKey { get; set; }
        /// <summary>
        /// Message content.
        /// </summary>
        public string MessageText { get; set; }
        //for audit log purpose
        public int Id { get; set; }

        public Constants.ActionLog ActionType { get; set; }
    }
}
