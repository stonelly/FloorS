using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hartalega.FloorSystem.Business.Logic.DataTransferObjects
{
    public class LineClearanceAuthoriseDTO
    {
        /// <summary>
        /// Intantiate the class
        /// </summary>
        static LineClearanceAuthoriseDTO()
        {

        }

        /// <summary>
        /// Employee ID
        /// </summary>
        private string _EmployeeID;

        public string EmployeeID
        {
            get { return _EmployeeID; }
            set { _EmployeeID = value; }
        }

        /// <summary>
        /// Name
        /// </summary>
        private string _EmployeeName;

        public string EmployeeName
        {
            get { return _EmployeeName; }
            set { _EmployeeName = value; }
        }

        /// <summary>
        /// Password
        /// </summary>
        private string _Password;

        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }

        /// <summary>
        /// Glove Type Size
        /// </summary>
        private int _RoldeID;

        public int RoldeID
        {
            get { return _RoldeID; }
            set { _RoldeID = value; }
        }

        /// <summary>
        /// Role
        /// </summary>
        private string _Role;

        public string Role
        {
            get { return _Role; }
            set { _Role = value; }
        }

        /// <summary>
        /// Is Allow Line Clearance Authorise
        /// </summary>
        private bool _IsAllowAuthoriseLineClearance = false;

        public bool IsAllowAuthoriseLineClearance
        {
            get { return _IsAllowAuthoriseLineClearance; }
            set { _IsAllowAuthoriseLineClearance = value; }
        }
    }
}
