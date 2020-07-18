using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KYL_CMS.Models.EntityDefinition;

namespace KYL_CMS.Models.DataClass.Function
{
    public class FunctionModifyRes
    {
        public FUNCTIONS FUNCTIONS { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
    }
}