using System;

namespace KYL_CMS.Models.EntityDefinition
{
    /// <summary>
    /// 主機資料
    /// </summary>
    public class INTERVENTION
    {
        public int? SN { get; set; }
        public int? INTERVIEW_SN { get; set; }
        public string PHRASE_KEY { get; set; }
        public string MEMO { get; set; }
        public DateTime? CDATE { get; set; }
        public string CUSER { get; set; }
        public DateTime? MDATE { get; set; }
        public string MUSER { get; set; }
    }
}
