using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KYL_CMS.Models.DataClass.Help
{
    public class ItemListRetrieveReq
    {
        public string[] TableItem { get; set; }
        public string[] PhraseGroup { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
    }

    public class ItemListRetrieveRes
    {
        public object ItemList;
        public ReturnStatus ReturnStatus { get; set; }
    }
}