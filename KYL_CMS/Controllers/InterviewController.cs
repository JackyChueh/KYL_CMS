using System;
using System.Web.Mvc;
using KYL_CMS.Models.BusinessLogic;
using KYL_CMS.Models.DataClass;
using KYL_CMS.Models.DataClass.Case;
//using KYL_CMS.Models.DataClass.Users;
using Newtonsoft.Json;
//using System.Linq;
//using System.IO;
//using DocumentFormat.OpenXml;
//using DocumentFormat.OpenXml.Packaging;
//using DocumentFormat.OpenXml.Spreadsheet;
//using KYL_CMS.Models.EntityDefinition;
//using KYL_CMS.Models.HelpLibrary;

namespace KYL_CMS.Controllers
{
    public class InterviewController : BaseController
    {
        // GET: Interview
        ////[Authorize]
        public ActionResult InterviewIndex()
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Login", "Main");
            }
            else
            {
                return View();
            }
        }

        ////[Authorize]
        [AcceptVerbs("POST")]
        public string InterviewRetrieve(CaseRetrieveReq req)
        {
            CaseRetrieveRes res = new CaseRetrieveRes();
            if (Session["ID"] == null)
            {
                res.ReturnStatus = new ReturnStatus(ReturnCode.SESSION_TIMEOUT);
            }
            else
            {
                try
                {
                    Log("Req=" + JsonConvert.SerializeObject(req));

                    res = new Interview("KYL").PaginationRetrieve(req, Session["ID"].ToString());
                    res.ReturnStatus = new ReturnStatus(ReturnCode.SUCCESS);
                }
                catch (Exception ex)
                {
                    Log("Err=" + ex.Message);
                    Log(ex.StackTrace);
                    res.ReturnStatus = new ReturnStatus(ReturnCode.SERIOUS_ERROR);
                }
            }
            var json = JsonConvert.SerializeObject(res);
            Log("Res=" + json);
            return json;
        }

        ////[Authorize]
        [AcceptVerbs("POST")]
        public string InterviewQuery(CaseModifyReq req)
        {
            CaseModifyRes res = new CaseModifyRes();
            if (Session["ID"] == null)
            {
                res.ReturnStatus = new ReturnStatus(ReturnCode.SESSION_TIMEOUT);
            }
            else
            {
                try
                {
                    Log("Req=" + JsonConvert.SerializeObject(req));
                    res = new CaseModifyRes
                    {
                        CASE = new Interview("KYL").ModificationQuery(req.CASE.SN),
                        ReturnStatus = new ReturnStatus(ReturnCode.SUCCESS)
                    };
                }
                catch (Exception ex)
                {
                    Log("Err=" + ex.Message);
                    Log(ex.StackTrace);
                    res.ReturnStatus = new ReturnStatus(ReturnCode.SERIOUS_ERROR);
                }
            }
            var json = JsonConvert.SerializeObject(res);
            Log("Res=" + json);
            return json;
        }

        ////[Authorize]
        [AcceptVerbs("POST")]
        public string InterviewUpdate()
        {
            CaseModifyRes res = new CaseModifyRes();
            if (Session["ID"] == null)
            {
                res.ReturnStatus = new ReturnStatus(ReturnCode.SESSION_TIMEOUT);
            }
            else
            {
                try
                {
                    string data = Request["data"];
                    Log("Res=" + data);
                    CaseModifyReq req = new CaseModifyReq();
                    JsonConvert.PopulateObject(data, req);
                    req.CASE.MUSER = Session["ID"].ToString();
                    int i = new Interview("KYL").DataUpdate(req);

                    //res.CASE = new Case("KYL").ModificationQuery(req.CASE.SN);
                    res.CASE = req.CASE;
                    res.ReturnStatus = new ReturnStatus(ReturnCode.EDIT_SUCCESS);
                }
                catch (Exception ex)
                {
                    Log("Err=" + ex.Message);
                    Log(ex.StackTrace);
                    res.ReturnStatus = new ReturnStatus(ReturnCode.SERIOUS_ERROR);
                }
            }
            var json = JsonConvert.SerializeObject(res);
            Log("Res=" + json);
            return json;
        }

        //[AcceptVerbs("POST")]
        //public string GetUserId()
        //{
        //    UsersModifyRes res = new UsersModifyRes();
        //    try
        //    {
        //        if (User.Identity.IsAuthenticated)
        //        {
        //            USERS users = new USERS();
        //            users.ID = Session["ID"].ToString();
        //            res.USERS = users;
        //            res.ReturnStatus = new ReturnStatus(ReturnCode.SUCCESS);
        //        }
        //        else
        //        {
        //            res.ReturnStatus = new ReturnStatus(ReturnCode.SESSION_TIMEOUT);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log("Err=" + ex.Message);
        //        Log(ex.StackTrace);
        //        res.ReturnStatus = new ReturnStatus(ReturnCode.SERIOUS_ERROR);
        //    }
        //    var json = JsonConvert.SerializeObject(res);
        //    Log("Res=" + json);
        //    return json;
        //}


    }
}