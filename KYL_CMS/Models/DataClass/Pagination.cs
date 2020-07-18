using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KYL_CMS.Models.DataClass
{
    public class Pagination
    {
        public int PageNumber;
        public int PageCount;
        public int RowCount;
        public int MinNumber;
        public int MaxNumber;
        //public object Data;
        public DateTime? StartTime;
        public DateTime? EndTime;
    }
}