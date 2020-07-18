using System;

namespace KYL_CMS.Models.EntityDefinition
{
    /// <summary>
    /// 主機資料
    /// </summary>
    public class INTERVIEW
    {
        public int? SN { get; set; }
        public int? CASE_SN { get; set; }
        public string VOLUNTEER_SN { get; set; }
        public DateTime? INCOMING_DATE { get; set; }
        public Int16? DURING { get; set; }
        public string CASE_NO { get; set; }
        public string NAME { get; set; }
        public string TEL { get; set; }
        public string CONTACT_TIME { get; set; }
        public string TREATMENT { get; set; }
        public string TREATMENT_MEMO { get; set; }
        public string CASE_SOURCE { get; set; }
        public string CASE_SOURCE_MEMO { get; set; }
        public string GENDER { get; set; }
        public string AGE { get; set; }
        public string EDUCATION { get; set; }
        public string CAREER { get; set; }
        public string CAREER_MEMO { get; set; }
        public string CITY { get; set; }
        public string MARRIAGE { get; set; }
        public string MARRIAGE_MEMO { get; set; }
        public string PHYSIOLOGY { get; set; }
        public string PHYSIOLOGY_MEMO { get; set; }
        public string PSYCHOLOGY { get; set; }
        public string PSYCHOLOGY_MEMO { get; set; }
        public string VISITED { get; set; }
        public string FAMILY { get; set; }
        public string EXPERIENCE { get; set; }
        public string HARASS { get; set; }
        public string SOLVE_DEGREE { get; set; }
        public string FEELING_MEMO { get; set; }
        public string ABOUT_SELF { get; set; }
        public string ABOUT_OTHERS { get; set; }
        public string BEHAVIOR { get; set; }
        public string ADDITION { get; set; }
        public string INNER_DEMAND { get; set; }
        public string INTERVENTION_MEMO { get; set; }
        public string OPINION01 { get; set; }
        public string OPINION02 { get; set; }
        public string OPINION03 { get; set; }
        public string OPINION04 { get; set; }
        public string OPINION05 { get; set; }
        public string OPINION06 { get; set; }
        public string OPINION07 { get; set; }
        public string OPINION08 { get; set; }
        public string OPINION09 { get; set; }
        public string OPINION { get; set; }
        public string CASE_STATUS { get; set; }
        public DateTime? CDATE { get; set; }
        public string CUSER { get; set; }
        public DateTime? MDATE { get; set; }
        public string MUSER { get; set; }
    }
}
