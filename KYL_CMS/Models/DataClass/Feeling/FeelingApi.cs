using System.Collections.Generic;
using KYL_CMS.Models.EntityDefinition;

namespace KYL_CMS.Models.DataClass.Feeling
{
     public class FeelingModifyReq
    {
        public FEELING FEELING { get; set; }
    }

    public class FeelingModifyRes
    {
        public FEELING FEELING { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
    }

    public class FeelingRetrieveReq
    {
        public FEELING FEELING { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }   //資安:必須檢查值域
    }

    public class FeelingRetrieveRes
    {
        public List<FEELING> FEELING { get; set; }
        public Pagination Pagination { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
    }
}
