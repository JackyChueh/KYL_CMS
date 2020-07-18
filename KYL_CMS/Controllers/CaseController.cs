using System;
using System.Web.Mvc;
using KYL_CMS.Models.BusinessLogic;
using KYL_CMS.Models.DataClass;
using KYL_CMS.Models.DataClass.Case;
using Newtonsoft.Json;
using System.Linq;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using KYL_CMS.Models.EntityDefinition;
using KYL_CMS.Models.HelpLibrary;

namespace KYL_CMS.Controllers
{
    public class CaseController : BaseController
    {
        // GET: Case
        ////[Authorize]
        public ActionResult CaseIndex()
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
        public string CaseQuery(CaseModifyReq req)
        {
            CaseModifyRes res = new CaseModifyRes();
            try
            {
                Log("Req=" + JsonConvert.SerializeObject(req));
                res = new CaseModifyRes
                {
                    CASE = new Case("KYL").ModificationQuery(req.CASE.SN),
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

        ////[Authorize]
        [AcceptVerbs("POST")]
        public string CaseCreate()
        {
            CaseModifyRes res = new CaseModifyRes();
            try
            {
                //上傳檔案
                string fileName = new UploadFile().FamilyFileUpload(Request);

                try
                {
                    string data = Request["data"];
                    Log("Res=" + data);
                    CaseModifyReq req = new CaseModifyReq();
                    JsonConvert.PopulateObject(data, req);
                    req.CASE.CUSER = User.Identity.Name;
                    req.CASE.MUSER = User.Identity.Name;
                    req.CASE.FAMILY_FILE = fileName;
                    int i = new Case("KYL").DataCreate(req);

                    //res.CASE = new Case("KYL").ModificationQuery(req.CASE.SN);
                    res.CASE = req.CASE;
                    res.ReturnStatus = new ReturnStatus(ReturnCode.ADD_SUCCESS);
                }
                catch (Exception ex)
                {
                    Log("Err=" + ex.Message);
                    Log(ex.StackTrace);
                    res.ReturnStatus = new ReturnStatus(ReturnCode.SERIOUS_ERROR);
                }
            }
            catch (Exception ex)
            {
                Log("Err=" + ex.Message);
                Log(ex.StackTrace);
                res.ReturnStatus = new ReturnStatus(ReturnCode.UPLOAD_FAIL, ex.Message);
            }
            var json = JsonConvert.SerializeObject(res);
            Log("Res=" + json);
            return json;
        }

        ////[Authorize]
        [AcceptVerbs("POST")]
        public string CaseUpdate()
        {
            CaseModifyRes res = new CaseModifyRes();
            try
            {
                //上傳檔案
                string fileName = new UploadFile().FamilyFileUpload(Request);

                try
                {
                    string data = Request["data"];
                    Log("Res=" + data);
                    CaseModifyReq req = new CaseModifyReq();
                    JsonConvert.PopulateObject(data, req);
                    req.CASE.CUSER = User.Identity.Name;
                    req.CASE.MUSER = User.Identity.Name;
                    req.CASE.FAMILY_FILE = fileName;
                    int i = new Case("KYL").DataUpdate(req);

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
            catch (Exception ex)
            {
                Log("Err=" + ex.Message);
                Log(ex.StackTrace);
                res.ReturnStatus = new ReturnStatus(ReturnCode.UPLOAD_FAIL, ex.Message);
            }
            var json = JsonConvert.SerializeObject(res);
            Log("Res=" + json);
            return json;
        }

        ////[Authorize]
        [AcceptVerbs("POST")]
        public string CaseDelete()
        {
            CaseModifyRes res = new CaseModifyRes();
            try
            {
                string data = Request["data"];
                Log("Res=" + data);
                CaseModifyReq req = new CaseModifyReq();
                JsonConvert.PopulateObject(data, req);
                req.CASE.CUSER = User.Identity.Name;
                req.CASE.MUSER = User.Identity.Name;
                int i = new Case("KYL").DataDelete(req);
                res.ReturnStatus = new ReturnStatus(ReturnCode.DEL_SUCCESS);
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