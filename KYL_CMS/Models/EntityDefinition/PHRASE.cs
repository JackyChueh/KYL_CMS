using System;

namespace KYL_CMS.Models.EntityDefinition
{
    /// <summary>
    /// 主機資料
    /// </summary>
    public class PHRASE
    {
        public int? SN { get; set; }
        public string PHRASE_GROUP { get; set; }
        public string PHRASE_KEY { get; set; }
        public string PHRASE_VALUE { get; set; }
        public string PHRASE_DESC { get; set; }
        public int? PARENT_SN { get; set; }
        public int? SORT { get; set; }
        public string MODE { get; set; }
        public DateTime? CDATE { get; set; }
        public string CUSER { get; set; }
        public DateTime? MDATE { get; set; }
        public string MUSER { get; set; }
    }
}
