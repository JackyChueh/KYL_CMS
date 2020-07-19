using System;
using System.Web.Mvc;
using KYL_CMS.Models.BusinessLogic;
using KYL_CMS.Models.DataClass;
using KYL_CMS.Models.DataClass.Users;
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
    public class UsersController : BaseController
    {
        ////[Authorize]
        public ActionResult UsersIndex()
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
        public string UsersRetrieve(UsersRetrieveReq req)
        {
            UsersRetrieveRes res = new UsersRetrieveRes();
            if (Session["ID"] == null)
            {
                res.ReturnStatus = new ReturnStatus(ReturnCode.SESSION_TIMEOUT);
            }
            else
            {
                try
                {
                    Log("Req=" + JsonConvert.SerializeObject(req));
                    res = new KYL_CMS.Models.BusinessLogic.Users("SCC").PaginationRetrieve(req);
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
        public string UsersQuery(UsersModifyReq req)
        {
            UsersModifyRes res = new UsersModifyRes();
            if (Session["ID"] == null)
            {
                res.ReturnStatus = new ReturnStatus(ReturnCode.SESSION_TIMEOUT);
            }
            else
            {
                try
                {
                    Log("Req=" + JsonConvert.SerializeObject(req));
                    res = new UsersModifyRes
                    {
                        USERS = new KYL_CMS.Models.BusinessLogic.Users("SCC").ModificationQuery(req.USERS.SN),
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
        public string UsersUpdate(UsersModifyReq req)
        {
            UsersModifyRes res = new UsersModifyRes();
            if (Session["ID"] == null)
            {
                res.ReturnStatus = new ReturnStatus(ReturnCode.SESSION_TIMEOUT);
            }
            else
            {
                try
                {
                    Log("Req=" + JsonConvert.SerializeObject(req));
                    req.USERS.MUSER = Session["ID"].ToString();

                    UsersModifyReq oldData = new UsersModifyReq();
                    oldData.USERS = new KYL_CMS.Models.BusinessLogic.Users("SCC").ModificationQuery(req.USERS.SN);
                    if (oldData.USERS.ID != req.USERS.ID && new Interview("KYL").CheckUserIdUsed(req.USERS.ID))
                    {
                        res = new UsersModifyRes
                        {
                            ReturnStatus = new ReturnStatus(ReturnCode.ITEM_USED)
                        };
                    }
                    else
                    {
                        int i = new KYL_CMS.Models.BusinessLogic.Users("SCC").DataUpdate(req);
                        res = new UsersModifyRes
                        {
                            USERS = req.USERS,
                            ReturnStatus = new ReturnStatus(ReturnCode.EDIT_SUCCESS)
                        };
                    }

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
        public string UsersReset(UsersModifyReq req)
        {
            UsersModifyRes res = new UsersModifyRes();
            if (Session["ID"] == null)
            {
                res.ReturnStatus = new ReturnStatus(ReturnCode.SESSION_TIMEOUT);
            }
            else
            {
                try
                {
                    Log("Req=" + JsonConvert.SerializeObject(req));
                    req.USERS.MUSER = Session["ID"].ToString();

                    UsersModifyReq oldData = new UsersModifyReq();
                    oldData.USERS = new KYL_CMS.Models.BusinessLogic.Users("SCC").ModificationQuery(req.USERS.SN);
                    req.USERS.PASSWORD = oldData.USERS.ID;
                    int i = new KYL_CMS.Models.BusinessLogic.Users("SCC").DataReset(req, 1);

                    res = new UsersModifyRes
                    {
                        USERS = req.USERS,
                        ReturnStatus = new ReturnStatus(ReturnCode.RESET_SUCCESS)
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
        public string UsersDelete(UsersModifyReq req)
        {
            UsersModifyRes res = new UsersModifyRes();
            if (Session["ID"] == null)
            {
                res.ReturnStatus = new ReturnStatus(ReturnCode.SESSION_TIMEOUT);
            }
            else
            {
                try
                {
                    Log("Req=" + JsonConvert.SerializeObject(req));
                    req.USERS.MUSER = Session["ID"].ToString();
                    if (new Interview("KYL").CheckUserIdUsed(req.USERS.ID))
                    {
                        res = new UsersModifyRes
                        {
                            ReturnStatus = new ReturnStatus(ReturnCode.ITEM_USED)
                        };
                    }
                    else
                    {
                        int i = new KYL_CMS.Models.BusinessLogic.Users("SCC").DataDelete(req);
                        res = new UsersModifyRes
                        {
                            ReturnStatus = new ReturnStatus(ReturnCode.DEL_SUCCESS)
                        };
                    }


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
        public string UsersCreate()
        {
            Stream stream = Request.InputStream;
            stream.Seek(0, SeekOrigin.Begin);
            string input = new StreamReader(stream).ReadToEnd();
            Log(input);
            UsersModifyReq req = new UsersModifyReq();
            JsonConvert.PopulateObject(input, req);

            UsersModifyRes res = new UsersModifyRes();
            if (Session["ID"] == null)
            {
                res.ReturnStatus = new ReturnStatus(ReturnCode.SESSION_TIMEOUT);
            }
            else
            {
                try
                {
                    Log("Req=" + JsonConvert.SerializeObject(req));
                    req.USERS.CUSER = Session["ID"].ToString();
                    req.USERS.MUSER = Session["ID"].ToString();
                    if (req.USERS.PASSWORD.Length == 0)
                    {
                        req.USERS.PASSWORD = req.USERS.ID;
                    }
                    int i = new KYL_CMS.Models.BusinessLogic.Users("SCC").DataCreate(req);
                    res = new UsersModifyRes
                    {
                        USERS = req.USERS,
                        ReturnStatus = new ReturnStatus(ReturnCode.ADD_SUCCESS)
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
        [AcceptVerbs("POST", "GET")]
        public ActionResult UsersExcel()
        {
            UsersRetrieveReq req = null;
            RequestParameter para = new RequestParameter();
            para.Load(Request);
            req = new UsersRetrieveReq();
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
                        CellValue = new CellValue("帳號")
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("姓名")
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("密碼")
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("電子郵件")
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("狀態")
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("備註")
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
                UsersRetrieveRes res = new KYL_CMS.Models.BusinessLogic.Users("SCC").ReportData(req);
                foreach (USERS data in res.USERS) //data
                {
                    row = new Row();
                    row.Append(
                        new Cell()
                        {
                            CellValue = new CellValue(data.SN.ToString())
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(data.ID)
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(data.NAME.ToString())
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(data.PASSWORD.ToString())
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(data.EMAIL == null ? "" : data.EMAIL)
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(data.MODE.ToString())
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(data.MEMO == null ? "" : data.MEMO)
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