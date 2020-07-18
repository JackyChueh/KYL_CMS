using System.Collections.Generic;
using KYL_CMS.Models.EntityDefinition;

namespace KYL_CMS.Models.DataClass.Grants
{
     public class GrantsModifyReq
    {
        public List<int> USERS_SN { get; set; }
        public List<int> ROLES_SN { get; set; }
        public string CUSER { get; set; }
        public string MUSER { get; set; }
        //public GRANTS GRANTS { get; set; }
    }

    public class GrantsModifyRes
    {
        //public GRANTS GRANTS { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
    }

    public class GrantsRetrieveReq
    {
        public GRANTS GRANTS { get; set; }
    }

    public class GrantsRetrieveRes
    {
        public List<GRANTS> GRANTS { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
    }
}
