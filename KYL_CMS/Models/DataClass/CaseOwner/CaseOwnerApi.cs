using System.Collections.Generic;
using KYL_CMS.Models.EntityDefinition;

namespace KYL_CMS.Models.DataClass.CaseOwner
{
     public class CaseOwnerModifyReq
    {
        public CASE_OWNER CASE_OWNER { get; set; }
    }

    public class CaseOwnerModifyRes
    {
        public CASE_OWNER CASE_OWNER { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
    }

    public class CaseOwnerRetrieveReq
    {
        public CASE_OWNER CASE_OWNER { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }   //資安:必須檢查值域
    }

    public class CaseOwnerRetrieveRes
    {
        public List<CASE_OWNER> CASE_OWNER { get; set; }
        public Pagination Pagination { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
    }
}
