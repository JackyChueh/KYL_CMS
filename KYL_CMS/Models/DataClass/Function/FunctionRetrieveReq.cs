using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using KYL_CMS.Models.EntityDefinition;

namespace KYL_CMS.Models.DataClass.Function
{
    public class FunctionRetrieveReq
    {
        public FUNCTIONS FUNCTIONS { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }   //資安:必須檢查值域
    }
}