using System;
using System.Web.Mvc;
using KYL_CMS.Models.BusinessLogic;
using KYL_CMS.Models.DataClass;
using KYL_CMS.Models.DataClass.Grants;
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
    public class GrantsController : BaseController
    {
        [Authorize]
        public ActionResult GrantsIndex()
        {
            return View();
        }

        [Authorize]
        [AcceptVerbs("POST")]
        public string UsersGrantsQuery(GrantsRetrieveReq req)
        {
            GrantsRetrieveRes res = new GrantsRetrieveRes();
            try
            {
                Log("Req=" + JsonConvert.SerializeObject(req));
                res = new GrantsRetrieveRes
                {
                    GRANTS = new Grants("SCC").ByUsersQuery(req.GRANTS.USERS_SN),
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

        [Authorize]
        [AcceptVerbs("POST")]
        public string RolesGrantsQuery(GrantsRetrieveReq req)
        {
            GrantsRetrieveRes res = new GrantsRetrieveRes();
            try
            {
                Log("Req=" + JsonConvert.SerializeObject(req));
                res = new GrantsRetrieveRes
                {
                    GRANTS = new Grants("SCC").ByRolesQuery(req.GRANTS.ROLES_SN),
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

        [Authorize]
        [AcceptVerbs("POST")]
        public string UsersGrantsUpdate(GrantsModifyReq req)
        {
            GrantsModifyRes res = new GrantsModifyRes();
            try
            {
                Log("Req=" + JsonConvert.SerializeObject(req));
                req.CUSER = User.Identity.Name;
                req.MUSER = User.Identity.Name;
                int i = new Grants("SCC").ByUsersUpdate(req);

                res.ReturnStatus = new ReturnStatus(ReturnCode.SET_SUCCESS);
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

        [Authorize]
        [AcceptVerbs("POST")]
        public string RolersGrantsUpdate(GrantsModifyReq req)
        {
            GrantsModifyRes res = new GrantsModifyRes();
            try
            {
                Log("Req=" + JsonConvert.SerializeObject(req));
                req.CUSER = User.Identity.Name;
                req.MUSER = User.Identity.Name;
                int i = new Grants("SCC").ByRolesUpdate(req);

                res.ReturnStatus = new ReturnStatus(ReturnCode.SET_SUCCESS);
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

        /*
        [AcceptVerbs("POST")]
        public string GrantsRetrieve(GrantsRetrieveReq req)
        {
            GrantsRetrieveRes res = new GrantsRetrieveRes();
            try
            {
                Log("Req=" + JsonConvert.SerializeObject(req));
                res = new Grants("SCC").PaginationRetrieve(req);
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

 

        [AcceptVerbs("POST")]
        public string GrantsUpdate(GrantsModifyReq req)
        {
            GrantsModifyRes res = new GrantsModifyRes();
            try
            {
                Log("Req=" + JsonConvert.SerializeObject(req));
                req.GRANTS.MUSER = User.Identity.Name;
                int i = new Grants("SCC").DataUpdate(req);

                res = new GrantsModifyRes
                {
                    GRANTS = new Grants("SCC").ModificationQuery(req.GRANTS.SN),
                    ReturnStatus = new ReturnStatus(ReturnCode.EDIT_SUCCESS)
                };
            }
            catch (Exception ex)
            {
                Log("Err="+ex.Message);
                Log(ex.StackTrace);
                res.ReturnStatus = new ReturnStatus(ReturnCode.SERIOUS_ERROR);
            }
            var json = JsonConvert.SerializeObject(res);
            Log("Res=" + json);
            return json;
        }

        [AcceptVerbs("POST")]
        public string GrantsDelete(GrantsModifyReq req)
        {
            GrantsModifyRes res = new GrantsModifyRes();
            try
            {
                Log("Req=" + JsonConvert.SerializeObject(req));
                req.GRANTS.MUSER = User.Identity.Name;
                int i = new Grants("SCC").DataDelete(req);

                res = new GrantsModifyRes
                {
                    ReturnStatus = new ReturnStatus(ReturnCode.DEL_SUCCESS)
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

        [AcceptVerbs("POST")]
        public string GrantsCreate()
        {
            Stream stream = Request.InputStream;
            stream.Seek(0, SeekOrigin.Begin);
            string input = new StreamReader(stream).ReadToEnd();
            Log(input);
            GrantsModifyReq req = new GrantsModifyReq();
            JsonConvert.PopulateObject(input, req);

            GrantsModifyRes res = new GrantsModifyRes();
            try
            {
                Log("Req=" + JsonConvert.SerializeObject(req));
                req.GRANTS.CUSER = User.Identity.Name;
                req.GRANTS.MUSER = User.Identity.Name;

                int i = new Grants("SCC").DataCreate(req);
                res = new GrantsModifyRes
                {
                    GRANTS = new Grants("SCC").ModificationQuery(req.GRANTS.SN),
                    ReturnStatus = new ReturnStatus(ReturnCode.ADD_SUCCESS)
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

        [AcceptVerbs("POST", "GET")]
        public ActionResult GrantsExcel()
        {
            GrantsRetrieveReq req = null;
            RequestParameter para = new RequestParameter();
            para.Load(Request);
            req = new GrantsRetrieveReq();
            JsonConvert.PopulateObject(para.Item("json"), req);

            var memoryStream = new MemoryStream();

            using (var document = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookpart = document.AddWorkbookPart();
                workbookpart.Workbook = new Workbook();
                WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();

                #region Sheet1
                SheetData sheetData = new SheetData();
                Row row;

                #region header
                row = new Row();    //header
                row.Append(
                    new Cell()
                    {
                        CellValue = new CellValue("群組序號")
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("使用者序號")
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("建檔時間")
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("建檔人員")
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("異動時間")
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("異動人員")
                    }
                );
                sheetData.AppendChild(row);
                #endregion

                #region data
                GrantsRetrieveRes res = new Grants("KYL").ReportData(req);
                foreach (GRANTS data in res.GRANTS) //data
                {
                    row = new Row();
                    row.Append(
                        new Cell()
                        {
                            CellValue = new CellValue(data.ROLES_SN.ToString())
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(data.USERS_SN.ToString())
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(data.CDATE.ToString())
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(data.CUSER.ToString())
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(data.MDATE.ToString())
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(data.MUSER.ToString())
                        }
                    );
                    sheetData.AppendChild(row);
                }
                #endregion


                Worksheet worksheet = new Worksheet();
                worksheet.Append(sheetData);
                worksheetPart.Worksheet = worksheet;    //add a Worksheet to the WorksheetPart

                Sheets sheets = document.WorkbookPart.Workbook.AppendChild(new Sheets());
                sheets.AppendChild(new Sheet()
                {
                    Id = document.WorkbookPart.GetIdOfPart(document.WorkbookPart.WorksheetParts.First()),
                    SheetId = 1,
                    Name = "工作表1"
                });
                #endregion
            }
            memoryStream.Seek(0, SeekOrigin.Begin);

            return File(memoryStream.ToArray(), "application/vnd.ms-excel", "TEST.xlsx");
        }
        */
    }
}
