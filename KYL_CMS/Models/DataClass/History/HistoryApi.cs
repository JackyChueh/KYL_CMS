using System.Collections.Generic;
using KYL_CMS.Models.EntityDefinition;

namespace KYL_CMS.Models.DataClass.History
{
     public class HistoryModifyReq
    {
        public INTERVIEW INTERVIEW { get; set; }
    }

    public class HistoryModifyRes
    {
        public INTERVIEW INTERVIEW { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
    }

    public class HistoryRetrieveReq
    {
        public INTERVIEW INTERVIEW { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }   //資安:必須檢查值域
    }

    public class HistoryRetrieveRes
    {
        public List<INTERVIEW> INTERVIEW { get; set; }
        public Pagination Pagination { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
    }
}
