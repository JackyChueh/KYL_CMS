using System.Collections.Generic;
using KYL_CMS.Models.EntityDefinition;

namespace KYL_CMS.Models.DataClass.Users
{
     public class UsersModifyReq
    {
        public USERS USERS { get; set; }
    }

    public class UsersModifyRes
    {
        public USERS USERS { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
    }

    public class UsersRetrieveReq
    {
        public USERS USERS { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }   //資安:必須檢查值域
    }

    public class UsersRetrieveRes
    {
        public List<USERS> USERS { get; set; }
        public Pagination Pagination { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
    }
}
