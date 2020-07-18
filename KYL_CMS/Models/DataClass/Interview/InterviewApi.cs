using System.Collections.Generic;
using KYL_CMS.Models.EntityDefinition;

namespace KYL_CMS.Models.DataClass.Interview
{
     public class InterviewModifyReq
    {
        public INTERVIEW INTERVIEW { get; set; }
    }

    public class InterviewModifyRes
    {
        public INTERVIEW INTERVIEW { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
    }

    public class InterviewRetrieveReq
    {
        public INTERVIEW INTERVIEW { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }   //資安:必須檢查值域
    }

    public class InterviewRetrieveRes
    {
        public List<INTERVIEW> INTERVIEW { get; set; }
        public Pagination Pagination { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
    }
}
