using System.Collections.Generic;
using KYL_CMS.Models.EntityDefinition;

namespace KYL_CMS.Models.DataClass.Intervention
{
     public class InterventionModifyReq
    {
        public INTERVENTION INTERVENTION { get; set; }
    }

    public class InterventionModifyRes
    {
        public INTERVENTION INTERVENTION { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
    }

    public class InterventionRetrieveReq
    {
        public INTERVENTION INTERVENTION { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }   //資安:必須檢查值域
    }

    public class InterventionRetrieveRes
    {
        public List<INTERVENTION> INTERVENTION { get; set; }
        public Pagination Pagination { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
    }
}
