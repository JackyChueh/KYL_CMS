using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KYL_CMS.Models.EntityDefinition
{
    public class CASE : CASE_OWNER
    {
        public CASE_DETAIL[] CASE_DETAIL { get; set; }

        public string CASE_STATUS { get; set; }
        public string CONTACT_TIME { get; set; }
        public DateTime? START_DATE { get; set; }
        public DateTime? END_DATE { get; set; }
        public string NAME { get; set; }
        public string TEL { get; set; }
        public string VOLUNTEER_SN { get; set; }
        public string ORDER_BY { get; set; }
        public string ASC_DESC { get; set; }
    }

    public class CASE_DETAIL : INTERVIEW
    {
        public SPECIAL_IDENTITY[] SPECIAL_IDENTITY { get; set; }
        public INTERVIEW_CLASSIFY[] INTERVIEW_CLASSIFY { get; set; }
        public FEELING[] FEELING { get; set; }
        public INTERVENTION[] INTERVENTION { get; set; }
        public string DataRow { get; set; }
    }

}