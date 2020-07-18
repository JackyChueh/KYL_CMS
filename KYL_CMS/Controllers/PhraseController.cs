using System;
using System.Web.Mvc;
using KYL_CMS.Models.BusinessLogic;
using KYL_CMS.Models.DataClass;
using KYL_CMS.Models.DataClass.Phrase;
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
    public class PhraseController : BaseController
    {
        ////[Authorize]
        public ActionResult PhraseIndex()
        {
            return View();
        }

        ////[Authorize]
        [AcceptVerbs("POST")]
        public string PhraseRetrieve(PhraseRetrieveReq req)
        {
            PhraseRetrieveRes res = new PhraseRetrieveRes();
            try
            {
                Log("Req=" + JsonConvert.SerializeObject(req));
                res = new KYL_CMS.Models.BusinessLogic.Phrase("SCC").PaginationRetrieve(req);
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
        public string PhraseQuery(PhraseModifyReq req)
        {
            PhraseModifyRes res = new PhraseModifyRes();
            try
            {
                Log("Req=" + JsonConvert.SerializeObject(req));
                res = new PhraseModifyRes
                {
                    PHRASE = new KYL_CMS.Models.BusinessLogic.Phrase("SCC").ModificationQuery(req.PHRASE.SN),
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
        public string PhraseUpdate(PhraseModifyReq req)
        {
            PhraseModifyRes res = new PhraseModifyRes();
            try
            {
                Log("Req=" + JsonConvert.SerializeObject(req));
                req.PHRASE.MUSER = User.Identity.Name;


                PhraseModifyReq oldData = new PhraseModifyReq();
                oldData.PHRASE = new Phrase("SCC").ModificationQuery(req.PHRASE.SN);
                if (oldData.PHRASE.PHRASE_KEY!=req.PHRASE.PHRASE_KEY &&  new Interview("KYL").CheckPharseUsed(oldData))
                {
                    res = new PhraseModifyRes
                    {
                        ReturnStatus = new ReturnStatus(ReturnCode.ITEM_USED)
                    };
                }
                else
                {
                    int i = new Phrase("SCC").DataUpdate(req);
                    res = new PhraseModifyRes
                    {
                        PHRASE = req.PHRASE,
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
            var json = JsonConvert.SerializeObject(res);
            Log("Res=" + json);
            return json;
        }

        ////[Authorize]
        [AcceptVerbs("POST")]
        public string PhraseDelete(PhraseModifyReq req)
        {
            PhraseModifyRes res = new PhraseModifyRes();
            try
            {
                Log("Req=" + JsonConvert.SerializeObject(req));
                req.PHRASE.MUSER = User.Identity.Name;
                if (new Interview("KYL").CheckPharseUsed(req))
                {
                    res = new PhraseModifyRes
                    {
                        ReturnStatus = new ReturnStatus(ReturnCode.ITEM_USED)
                    };
                }
                else
                {
                    int i = new Phrase("SCC").DataDelete(req);
                    res = new PhraseModifyRes
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
            var json = JsonConvert.SerializeObject(res);
            Log("Res=" + json);
            return json;
        }

        ////[Authorize]
        [AcceptVerbs("POST")]
        public string PhraseCreate()
        {
            Stream stream = Request.InputStream;
            stream.Seek(0, SeekOrigin.Begin);
            string input = new StreamReader(stream).ReadToEnd();
            Log(input);
            PhraseModifyReq req = new PhraseModifyReq();
            JsonConvert.PopulateObject(input, req);

            PhraseModifyRes res = new PhraseModifyRes();
            try
            {
                Log("Req=" + JsonConvert.SerializeObject(req));
                req.PHRASE.CUSER = User.Identity.Name;
                req.PHRASE.MUSER = User.Identity.Name;

                int i = new KYL_CMS.Models.BusinessLogic.Phrase("SCC").DataCreate(req);
                res = new PhraseModifyRes
                {
                    PHRASE = req.PHRASE,
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

        //[AcceptVerbs("POST", "GET")]
        //public ActionResult PhraseExcel()
        //{
        //    PhraseRetrieveReq req = null;
        //    RequestParameter para = new RequestParameter();
        //    para.Load(Request);
        //    req = new PhraseRetrieveReq();
        //    JsonConvert.PopulateObject(para.Item("json"), req);

        //    var memoryStream = new MemoryStream();

        //    using (var document = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
        //    {
        //        WorkbookPart workbookpart = document.AddWorkbookPart();
        //        workbookpart.Workbook = new Workbook();
        //        WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();

        //        #region Sheet1
        //        SheetData sheetData = new SheetData();
        //        Row row;

        //        #region header
        //        row = new Row();    //header
        //        row.Append(
        //            new Cell()
        //            {
        //                CellValue = new CellValue("序號")
        //            },
        //            new Cell()
        //            {
        //                CellValue = new CellValue("項目所屬群組")
        //            },
        //            new Cell()
        //            {
        //                CellValue = new CellValue("項目編號")
        //            },
        //            new Cell()
        //            {
        //                CellValue = new CellValue("項目值")
        //            },
        //            new Cell()
        //            {
        //                CellValue = new CellValue("項目說明")
        //            },
        //            new Cell()
        //            {
        //                CellValue = new CellValue("排序")
        //            },
        //            new Cell()
        //            {
        //                CellValue = new CellValue("狀態")
        //            },
        //            new Cell()
        //            {
        //                CellValue = new CellValue("建檔時間")
        //            },
        //            new Cell()
        //            {
        //                CellValue = new CellValue("建檔人員")
        //            },
        //            new Cell()
        //            {
        //                CellValue = new CellValue("異動時間")
        //            },
        //            new Cell()
        //            {
        //                CellValue = new CellValue("異動人員")
        //            }
        //        );
        //        sheetData.AppendChild(row);
        //        #endregion

        //        #region data
        //        PhraseRetrieveRes res = new Phrase("KYL").ReportData(req);
        //        foreach (PHRASE data in res.PHRASE) //data
        //        {
        //            row = new Row();
        //            row.Append(
        //                new Cell()
        //                {
        //                    CellValue = new CellValue(data.SN.ToString())
        //                },
        //                new Cell()
        //                {
        //                    CellValue = new CellValue(data.PHRASE_GROUP.ToString())
        //                },
        //                new Cell()
        //                {
        //                    CellValue = new CellValue(data.PHRASE_KEY.ToString())
        //                },
        //                new Cell()
        //                {
        //                    CellValue = new CellValue(data.PHRASE_VALUE.ToString())
        //                },
        //                new Cell()
        //                {
        //                    CellValue = new CellValue(data.PHRASE_DESC.ToString())
        //                },
        //                new Cell()
        //                {
        //                    CellValue = new CellValue(data.SORT.ToString())
        //                },
        //                new Cell()
        //                {
        //                    CellValue = new CellValue(data.MODE.ToString())
        //                },
        //                new Cell()
        //                {
        //                    CellValue = new CellValue(data.CDATE.ToString())
        //                },
        //                new Cell()
        //                {
        //                    CellValue = new CellValue(data.CUSER.ToString())
        //                },
        //                new Cell()
        //                {
        //                    CellValue = new CellValue(data.MDATE.ToString())
        //                },
        //                new Cell()
        //                {
        //                    CellValue = new CellValue(data.MUSER.ToString())
        //                }
        //            );
        //            sheetData.AppendChild(row);
        //        }
        //        #endregion


        //        Worksheet worksheet = new Worksheet();
        //        worksheet.Append(sheetData);
        //        worksheetPart.Worksheet = worksheet;    //add a Worksheet to the WorksheetPart

        //        Sheets sheets = document.WorkbookPart.Workbook.AppendChild(new Sheets());
        //        sheets.AppendChild(new Sheet()
        //        {
        //            Id = document.WorkbookPart.GetIdOfPart(document.WorkbookPart.WorksheetParts.First()),
        //            SheetId = 1,
        //            Name = "工作表1"
        //        });
        //        #endregion
        //    }
        //    memoryStream.Seek(0, SeekOrigin.Begin);

        //    return File(memoryStream.ToArray(), "application/vnd.ms-excel", "TEST.xlsx");
        //}

    }
}