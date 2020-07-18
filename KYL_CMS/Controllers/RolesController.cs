using System;
using System.Web.Mvc;
using KYL_CMS.Models.BusinessLogic;
using KYL_CMS.Models.DataClass;
using KYL_CMS.Models.DataClass.Roles;
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
    public class RolesController : BaseController
    {
        [Authorize]
        public ActionResult RolesIndex()
        {
            return View();
        }

        [Authorize]
        [AcceptVerbs("POST")]
        public string RolesRetrieve(RolesRetrieveReq req)
        {
            RolesRetrieveRes res = new RolesRetrieveRes();
            try
            {
                Log("Req=" + JsonConvert.SerializeObject(req));
                res = new Roles("SCC").PaginationRetrieve(req);
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

        [Authorize]
        [AcceptVerbs("POST")]
        public string RolesQuery(RolesModifyReq req)
        {
            RolesModifyRes res = new RolesModifyRes();
            try
            {
                Log("Req=" + JsonConvert.SerializeObject(req));
                res = new RolesModifyRes
                {
                    ROLES = new Roles("SCC").ModificationQuery(req.ROLES.SN),
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
        public string RolesUpdate(RolesModifyReq req)
        {
            RolesModifyRes res = new RolesModifyRes();
            try
            {
                Log("Req=" + JsonConvert.SerializeObject(req));
                req.ROLES.MUSER = User.Identity.Name;
                int i = new Roles("SCC").DataUpdate(req);

                res = new RolesModifyRes
                {
                    ROLES = new Roles("SCC").ModificationQuery(req.ROLES.SN),
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

        [Authorize]
        [AcceptVerbs("POST")]
        public string RolesDelete(RolesModifyReq req)
        {
            RolesModifyRes res = new RolesModifyRes();
            try
            {
                Log("Req=" + JsonConvert.SerializeObject(req));
                req.ROLES.MUSER = User.Identity.Name;
                int i = new Roles("SCC").DataDelete(req);

                res = new RolesModifyRes
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

        [Authorize]
        [AcceptVerbs("POST")]
        public string RolesCreate()
        {
            Stream stream = Request.InputStream;
            stream.Seek(0, SeekOrigin.Begin);
            string input = new StreamReader(stream).ReadToEnd();
            Log(input);
            RolesModifyReq req = new RolesModifyReq();
            JsonConvert.PopulateObject(input, req);

            RolesModifyRes res = new RolesModifyRes();
            try
            {
                Log("Req=" + JsonConvert.SerializeObject(req));
                req.ROLES.CUSER = User.Identity.Name;
                req.ROLES.MUSER = User.Identity.Name;

                int i = new Roles("SCC").DataCreate(req);
                res = new RolesModifyRes
                {
                    ROLES = new Roles("SCC").ModificationQuery(req.ROLES.SN),
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

        [Authorize]
        [AcceptVerbs("POST", "GET")]
        public ActionResult RolesExcel()
        {
            RolesRetrieveReq req = null;
            RequestParameter para = new RequestParameter();
            para.Load(Request);
            req = new RolesRetrieveReq();
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
                        CellValue = new CellValue("序號")
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("群組名稱")
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("狀態")
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
                RolesRetrieveRes res = new Roles("KYL").ReportData(req);
                foreach (ROLES data in res.ROLES) //data
                {
                    row = new Row();
                    row.Append(
                        new Cell()
                        {
                            CellValue = new CellValue(data.SN.ToString())
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(data.NAME.ToString())
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(data.MODE.ToString())
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

    }
}
