using System.Collections.Generic;
using KYL_CMS.Models.EntityDefinition;

namespace KYL_CMS.Models.DataClass.InterviewClassify
{
     public class InterviewClassifyModifyReq
    {
        public INTERVIEW_CLASSIFY INTERVIEW_CLASSIFY { get; set; }
    }

    public class InterviewClassifyModifyRes
    {
        public INTERVIEW_CLASSIFY INTERVIEW_CLASSIFY { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
    }

    public class InterviewClassifyRetrieveReq
    {
        public INTERVIEW_CLASSIFY INTERVIEW_CLASSIFY { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }   //資安:必須檢查值域
    }

    public class InterviewClassifyRetrieveRes
    {
        public List<INTERVIEW_CLASSIFY> INTERVIEW_CLASSIFY { get; set; }
        public Pagination Pagination { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
    }
}
