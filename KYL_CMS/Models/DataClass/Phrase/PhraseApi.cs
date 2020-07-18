using System.Collections.Generic;
using KYL_CMS.Models.EntityDefinition;

namespace KYL_CMS.Models.DataClass.Phrase
{
     public class PhraseModifyReq
    {
        public PHRASE PHRASE { get; set; }
    }

    public class PhraseModifyRes
    {
        public PHRASE PHRASE { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
    }

    public class PhraseRetrieveReq
    {
        public PHRASE PHRASE { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }   //資安:必須檢查值域
    }

    public class PhraseRetrieveRes
    {
        public List<PHRASE> PHRASE { get; set; }
        public Pagination Pagination { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
    }
}
