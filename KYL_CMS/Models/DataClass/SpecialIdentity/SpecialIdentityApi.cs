using System.Collections.Generic;
using KYL_CMS.Models.EntityDefinition;

namespace KYL_CMS.Models.DataClass.SpecialIdentity
{
     public class SpecialIdentityModifyReq
    {
        public SPECIAL_IDENTITY SPECIAL_IDENTITY { get; set; }
    }

    public class SpecialIdentityModifyRes
    {
        public SPECIAL_IDENTITY SPECIAL_IDENTITY { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
    }

    public class SpecialIdentityRetrieveReq
    {
        public SPECIAL_IDENTITY SPECIAL_IDENTITY { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }   //資安:必須檢查值域
    }

    public class SpecialIdentityRetrieveRes
    {
        public List<SPECIAL_IDENTITY> SPECIAL_IDENTITY { get; set; }
        public Pagination Pagination { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
    }
}
