using System;
using System.Web.Mvc;
using KYL_CMS.Models.BusinessLogic;
using KYL_CMS.Models.DataClass;
using KYL_CMS.Models.DataClass.Case;
using KYL_CMS.Models.DataClass.Merge;
using Newtonsoft.Json;
using System.Linq;
using System.IO;
using KYL_CMS.Models.EntityDefinition;
using KYL_CMS.Models.HelpLibrary;

namespace KYL_CMS.Controllers
{
    public class MergeController : BaseController
    {
        // GET: Merge
        ////[Authorize]
        public ActionResult MergeIndex()
        {
            return View();
        }

        ////[Authorize]
        [AcceptVerbs("POST")]
        public string CaseRetrieve(CaseRetrieveReq req)
        {
            CaseRetrieveRes res = new CaseRetrieveRes();
            try
            {
                Log("Req=" + JsonConvert.SerializeObject(req));

                res = new Case("KYL").PaginationRetrieve(req);
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
        public string CaseMerge()
        {
            MergeModifyRes res = new MergeModifyRes();

            try
            {
                string data = Request["data"];
                Log("Res=" + data);
                MergeModifyReq req = new MergeModifyReq();
                JsonConvert.PopulateObject(data, req);
                req.ADD.MUSER = Session["ID"].ToString();
                int i = new Case("KYL").DataMerge(req);

                res.CASE = req.ADD;
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

    }
}