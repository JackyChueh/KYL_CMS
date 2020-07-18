using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KYL_CMS.Models.DataClass;

namespace KYL_CMS.Models.Interface
{
    interface IPaginationg<P,R>
    {
        R PaginationRetrieve(P req);
    }
}