using System.Collections.Generic;
using KYL_CMS.Models.EntityDefinition;

namespace KYL_CMS.Models.DataClass.Roles
{
     public class RolesModifyReq
    {
        public ROLES ROLES { get; set; }
    }

    public class RolesModifyRes
    {
        public ROLES ROLES { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
    }

    public class RolesRetrieveReq
    {
        public ROLES ROLES { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }   //資安:必須檢查值域
    }

    public class RolesRetrieveRes
    {
        public List<ROLES> ROLES { get; set; }
        public Pagination Pagination { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
    }
}
