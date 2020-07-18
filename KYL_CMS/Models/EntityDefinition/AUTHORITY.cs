using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KYL_CMS.Models.EntityDefinition
{
    public partial class AUTHORITY
    {
        public int? SN { get; set; }
        public Int16? USERS_SN { get; set; }
        public int? ROLES_SN { get; set; }
        public Int16? FUNCTIONS_SN { get; set; }
        public DateTime? CDATE { get; set; }
        public Int16? CUSER { get; set; }
        public DateTime? MDATE { get; set; }
        public Int16? MUSER { get; set; }
    }

    public partial class AUTHORITY
    {
        public USERS USERS { get; set; }
        public FUNCTIONS FUNCTIONS { get; set; }
    }

}