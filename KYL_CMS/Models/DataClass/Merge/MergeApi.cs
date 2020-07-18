using System.Collections.Generic;
using KYL_CMS.Models.EntityDefinition;

namespace KYL_CMS.Models.DataClass.Merge
{
     public class MergeModifyReq
    {
        public CASE ADD { get; set; }
        public CASE REMOVE { get; set; }
    }

    public class MergeModifyRes
    {
        public CASE CASE { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
    }

    public class MergeRetrieveReq
    {
        public CASE CASE { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class MergeRetrieveRes
    {
        public List<CASE> CASE { get; set; }
        public Pagination Pagination { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
    }
}
