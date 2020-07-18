using System;

namespace KYL_CMS.Models.EntityDefinition
{
    /// <summary>
    /// 主機資料
    /// </summary>
    public class USER_INFO
    {
        public Int16? SN { get; set; }
        public string ID { get; set; }
        public string NAME { get; set; }
        public Int16? FORCE_PWD { get; set; }
        public int? ROLES_SN { get; set; }
    }
}
