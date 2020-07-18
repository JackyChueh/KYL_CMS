using System;

namespace KYL_CMS.Models.EntityDefinition
{
    /// <summary>
    /// 主機資料
    /// </summary>
    public class GRANTS
    {
        public int? ROLES_SN { get; set; }
        public string ROLES_NAME { get; set; }
        public int? USERS_SN { get; set; }
        public string USERS_NAME { get; set; }
        public DateTime? CDATE { get; set; }
        public string CUSER { get; set; }
        public DateTime? MDATE { get; set; }
        public string MUSER { get; set; }
    }
}
