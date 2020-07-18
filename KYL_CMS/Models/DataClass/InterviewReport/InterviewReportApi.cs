using System;
using System.Collections.Generic;
using KYL_CMS.Models.EntityDefinition;

namespace KYL_CMS.Models.DataClass.InterviewReport
{
    public class InterviewReportModifyReq
    {
        public CASE CASE { get; set; }
    }

    public class InterviewReportModifyRes
    {
        public CASE CASE { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
    }

    public class InterviewReportRetrieveReq
    {
        public DateTime? Start;
        public DateTime? End;
        public int PageNumber { get; set; }
        public int PageSize { get; set; }   //資安:必須檢查值域
    }

    public class InterviewReportRetrieveRes
    {
        public List<CASE_DETAIL> CASE_DETAIL { get; set; }
        public Pagination Pagination { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
    }
}
