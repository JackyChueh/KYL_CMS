﻿using System;

namespace KYL_CMS.Models.EntityDefinition
{
    /// <summary>
    /// 主機資料
    /// </summary>
    public class USERS
    {
        public Int16? SN { get; set; }
        public string ID { get; set; }
        public string NAME { get; set; }
        public string PASSWORD { get; set; }
        public string EMAIL { get; set; }
        public string MODE { get; set; }
        public string MEMO { get; set; }
        public DateTime? CDATE { get; set; }
        public string CUSER { get; set; }
        public DateTime? MDATE { get; set; }
        public string MUSER { get; set; }
    }
}
