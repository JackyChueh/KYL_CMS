using System.Collections.Generic;
using KYL_CMS.Models.EntityDefinition;

namespace KYL_CMS.Models.DataClass.Case
{
     public class CaseModifyReq
    {
        public string UserId { get; set; }
        public CASE CASE { get; set; }
    }

    public class CaseModifyRes
    {
        public CASE CASE { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
    }

    public class CaseRetrieveReq
    {
        public string UserId { get; set; }
        public CASE CASE { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class CaseRetrieveRes
    {
        public List<CASE> CASE { get; set; }
        public Pagination Pagination { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
    }
}
