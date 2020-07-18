using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KYL_CMS.Models.DataClass.Case;
using KYL_CMS.Models.DataClass.History;
using Newtonsoft.Json;
using KYL_CMS.Models.BusinessLogic;
using KYL_CMS.Models.DataClass;

namespace KYL_CMS.Controllers
{
    public class HistoryController : BaseController
    {
        // GET: History
        public ActionResult HistoryIndex()
        {
            return View();
        }

        ////[Authorize]
        [AcceptVerbs("POST")]
        public string HistoryRetrieve(HistoryRetrieveReq req)
        {
            HistoryRetrieveRes res = new HistoryRetrieveRes();
            try
            {
                Log("Req=" + JsonConvert.SerializeObject(req));

                res = new History("KYL").PaginationRetrieve(req, Session["ID"].ToString());
                res.ReturnStatus = new ReturnStatus(ReturnCode.SUCCESS);
            }
            catch (Exception ex)
            {
                Log("Err=" + ex.Message);
                Log(ex.StackTrace);
                res.ReturnStatus = new ReturnStatus(ReturnCode.SERIOUS_ERROR);
            }
            var json = JsonConvert.SerializeObject(res);
            Log("Res=" + json);
            return json;
        }

        ////[Authorize]
        [AcceptVerbs("POST")]
        public string HistoryQuery(CaseModifyReq req)
        {
            CaseModifyRes res = new CaseModifyRes();
            try
            {
                Log("Req=" + JsonConvert.SerializeObject(req));
                res = new CaseModifyRes
                {
                    CASE = new History("KYL").ModificationQuery(req.CASE.SN, Session["ID"].ToString()),
                    ReturnStatus = new ReturnStatus(ReturnCode.SUCCESS)
                };
            }
            catch (Exception ex)
            {
                Log("Err=" + ex.Message);
                Log(ex.StackTrace);
                res.ReturnStatus = new ReturnStatus(ReturnCode.SERIOUS_ERROR);
            }
            var json = JsonConvert.SerializeObject(res);
            Log("Res=" + json);
            return json;
        }
    }
}