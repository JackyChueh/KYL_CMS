using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KYL_CMS.Models.EntityDefinition;

namespace KYL_CMS.Models.DataClass.Function
{
    public class FunctionRetrieveRes
    {
        public List<FUNCTIONS> FUNCTIONS { get; set; }
        public Pagination Pagination { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
    }
}