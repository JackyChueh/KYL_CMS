using System;
using System.Collections.Generic;
using System.Web.Mvc;
using KYL_CMS.Models.BusinessLogic;
using KYL_CMS.Models.DataClass;
using KYL_CMS.Models.DataClass.Help;
using KYL_CMS.Models.DataClass.InterviewReport;
using Newtonsoft.Json;
using System.Linq;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using X14 = DocumentFormat.OpenXml.Office2010.Excel;
using KYL_CMS.Models.EntityDefinition;
using KYL_CMS.Models.HelpLibrary;

namespace KYL_CMS.Controllers
{
    public class InterviewReportController : BaseController
    {
        // GET: InterviewReport
        ////[Authorize]
        public ActionResult InterviewReportIndex()
        {
            return View();
        }

        ////[Authorize]
        [AcceptVerbs("POST", "GET")]
        public ActionResult InterviewReportExcel()
        {
            var memoryStream = new MemoryStream();

            try
            {
                InterviewReportRetrieveReq req = null;
                RequestParameter para = new RequestParameter();
                para.Load(Request);
                req = new InterviewReportRetrieveReq();
                JsonConvert.PopulateObject(para.Item("json"), req);

                using (var document = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
                {
                    WorkbookPart workbookpart = document.AddWorkbookPart();
                    workbookpart.Workbook = new Workbook();
                    WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
                    WorkbookStylesPart wbsp = workbookpart.AddNewPart<WorkbookStylesPart>();



                    #region Sheet1
                    SheetData sheetData = new SheetData();
                    MergeCells mergeCells = new MergeCells();
                    Row row;
                    int columnIndex = 0;
                    int endIndex = 0;
                    Cell dataCell;
                    CellValue cellValue;
                    #region header

                    string json = @"
{
    ""PhraseGroup"": [""SPECIAL_IDENTITY"", ""INTERVIEW_CLASSIFY"", ""FEELING"", ""INTERVENTION""]
}
";
                    ItemListRetrieveReq itemReq = JsonConvert.DeserializeObject<ItemListRetrieveReq>(json);
                    ItemListRetrieveRes itemRes = new QueryItems("SCC").ItemListQuery(itemReq);

                    Dictionary<string, object> pharse = new Dictionary<string, object>();
                    pharse = (Dictionary<string, object>)itemRes.ItemList;

                    row = new Row();    //merge header line
                    columnIndex = 13;   //跳過不需合併的欄位

                    columnIndex++;
                    dataCell = new Cell();
                    dataCell.CellReference = GetExcelColumnName(columnIndex) + "1";
                    cellValue = new CellValue();
                    cellValue.Text = "特殊身份別";
                    dataCell.DataType = CellValues.String;
                    dataCell.StyleIndex = 1;
                    dataCell.Append(cellValue);
                    row.Append(dataCell);
                    List<ItemList> special_identity = (List<ItemList>)pharse["SPECIAL_IDENTITY"];
                    endIndex = columnIndex + special_identity.Count-1;
                    mergeCells.Append(new MergeCell() { Reference = new StringValue(dataCell.CellReference  + ":" + GetExcelColumnName(endIndex) + "1") });
                    columnIndex = endIndex;

                    columnIndex = columnIndex + 4; //跳過不需合併的欄位

                    columnIndex++;
                    dataCell = new Cell();
                    dataCell.CellReference = GetExcelColumnName(columnIndex) + "1";
                    cellValue = new CellValue();
                    cellValue.Text = "來電者主述問題分類";
                    dataCell.DataType = CellValues.String;
                    dataCell.StyleIndex = 2;
                    dataCell.Append(cellValue);
                    row.Append(dataCell);
                    List<ItemList> interview_classify = (List<ItemList>)pharse["INTERVIEW_CLASSIFY"];
                    endIndex = columnIndex + interview_classify.Count - 1;
                    mergeCells.Append(new MergeCell() { Reference = new StringValue(dataCell.CellReference + ":" + GetExcelColumnName(endIndex) + "1") });
                    columnIndex = endIndex ;

                    columnIndex = columnIndex + 1; //跳過不需合併的欄位

                    columnIndex++;
                    dataCell = new Cell();
                    dataCell.CellReference = GetExcelColumnName(columnIndex) + "1";
                    cellValue = new CellValue();
                    cellValue.Text = "案主在此困擾的情緒";
                    dataCell.DataType = CellValues.String;
                    dataCell.StyleIndex = 3;
                    dataCell.Append(cellValue);
                    row.Append(dataCell);
                    List<ItemList> feeling = (List<ItemList>)pharse["FEELING"];
                    endIndex = columnIndex + feeling.Count - 1;
                    mergeCells.Append(new MergeCell() { Reference = new StringValue(dataCell.CellReference + ":" + GetExcelColumnName(endIndex) + "1") });
                    columnIndex = endIndex;

                    columnIndex++;
                    dataCell = new Cell();
                    dataCell.CellReference = GetExcelColumnName(columnIndex) + "1";
                    cellValue = new CellValue();
                    cellValue.Text = "接案過程介入方式";
                    dataCell.DataType = CellValues.String;
                    dataCell.StyleIndex = 4;
                    dataCell.Append(cellValue);
                    row.Append(dataCell);
                    List<ItemList> intervention = (List<ItemList>)pharse["INTERVENTION"];
                    endIndex = columnIndex + intervention.Count - 1;
                    mergeCells.Append(new MergeCell() { Reference = new StringValue(dataCell.CellReference + ":" + GetExcelColumnName(endIndex) + "1") });
                    columnIndex = endIndex;

                    columnIndex++;
                    dataCell = new Cell();
                    dataCell.CellReference = GetExcelColumnName(columnIndex) + "1";
                    cellValue = new CellValue();
                    cellValue.Text = "輔導員自我評量";
                    dataCell.DataType = CellValues.String;
                    dataCell.StyleIndex = 5;
                    dataCell.Append(cellValue);
                    row.Append(dataCell);
                    endIndex = columnIndex + 9 -1;
                    mergeCells.Append(new MergeCell() { Reference = new StringValue(dataCell.CellReference + ":" + GetExcelColumnName(endIndex) + "1") });
                    columnIndex = endIndex;

                    sheetData.AppendChild(row);

                    row = new Row();    //header line
                    row.Append(
                        new Cell()
                        {
                            CellValue = new CellValue("日期"),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue("星期"),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue("完成進度"),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue("接案者代號"),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue("談話時間"),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue("聯絡時間"),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue("處遇方式"),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue("個案來源"),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue("主述性別"),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue("年齡層"),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue("學歷"),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue("職業"),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue("居住地"),
                            DataType = CellValues.String
                        }
                    );
                    //List<ItemList> special_identity = (List<ItemList>)pharse["SPECIAL_IDENTITY"];
                    foreach (ItemList item in special_identity)
                    {
                        row.Append(new Cell() {
                            CellValue = new CellValue(item.Value),
                            DataType = CellValues.String
                        });
                    }
                    row.Append(
                        new Cell()
                        {
                            CellValue = new CellValue("婚姻情感狀態"),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue("生理障礙"),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue("心理障礙"),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue("曾經來電"),
                            DataType = CellValues.String
                        }
                    );
                    //List<ItemList> interview_classify = (List<ItemList>)pharse["INTERVIEW_CLASSIFY"];
                    foreach (ItemList item in interview_classify)
                    {
                        row.Append(new Cell() {
                            CellValue = new CellValue(item.Value),
                            DataType = CellValues.String
                        });
                    }
                    row.Append(
                        new Cell()
                        {
                            CellValue = new CellValue("解決問題的程度"),
                            DataType = CellValues.String
                        }
                    );
                    //List<ItemList> feeling = (List<ItemList>)pharse["FEELING"];
                    foreach (ItemList item in feeling)
                    {
                        row.Append(new Cell() {
                            CellValue = new CellValue(item.Value),
                            DataType = CellValues.String
                        });
                    }
                    //List<ItemList> intervention = (List<ItemList>)pharse["INTERVENTION"];
                    foreach (ItemList item in intervention)
                    {
                        row.Append(new Cell() {
                            CellValue = new CellValue(item.Value),
                            DataType = CellValues.String
                        });
                    }
                    row.Append(
                        new Cell()
                        {
                            CellValue = new CellValue("一、真心關心當事人"),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue("二、能接納當事人"),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue("三、能引導當事人充分的敘說"),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue("四、與當事人建立有助益的關係"),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue("五、建立明確的協談目標"),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue("六、充分了解當事人的問題"),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue("七、純熟的協談技巧"),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue("八、清晰的協談意圖"),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue("九、能訂定適當的輔導方向"),
                            DataType = CellValues.String
                        }
                    );
                    sheetData.AppendChild(row);

                    #endregion

                    #region data

                    InterviewReportRetrieveRes res = new InterviewReport("KYL").ReportData(req);
                    if(true){ 
                    foreach (CASE_DETAIL interview in res.CASE_DETAIL)
                    {
                        row = new Row();
                        row.Append(
                            new Cell()
                            {
                                CellValue = new CellValue(interview.INCOMING_DATE == null ? "" : interview.INCOMING_DATE.Value.ToString("yyyy-MM-dd")),
                                DataType = CellValues.String
                            },
                            new Cell()
                            {
                                CellValue = new CellValue(interview.INCOMING_DATE == null ? "" : (interview.INCOMING_DATE.Value.DayOfWeek == 0 ? 7 : (int)interview.INCOMING_DATE.Value.DayOfWeek).ToString()),
                                DataType = CellValues.String
                            },
                            new Cell()
                            {
                                CellValue = new CellValue(interview.CASE_STATUS),
                                DataType = CellValues.String
                            },
                            new Cell()
                            {
                                CellValue = new CellValue(interview.VOLUNTEER_SN),
                                DataType = CellValues.String
                            },
                            new Cell()
                            {
                                CellValue = new CellValue(interview.DURING == null ? "" : interview.DURING.ToString()),
                                DataType = CellValues.String
                            },
                            new Cell()
                            {
                                CellValue = new CellValue(interview.CONTACT_TIME),
                                DataType = CellValues.String
                            },
                            new Cell()
                            {
                                CellValue = new CellValue(interview.TREATMENT),
                                DataType = CellValues.String
                            },
                            new Cell()
                            {
                                CellValue = new CellValue(interview.CASE_SOURCE),
                                DataType = CellValues.String
                            },
                            new Cell()
                            {
                                CellValue = new CellValue(interview.GENDER),
                                DataType = CellValues.String
                            },
                            new Cell()
                            {
                                CellValue = new CellValue(interview.AGE),
                                DataType = CellValues.String
                            },
                           new Cell()
                           {
                               CellValue = new CellValue(interview.EDUCATION),
                               DataType = CellValues.String
                           },
                            new Cell()
                            {
                                CellValue = new CellValue(interview.CAREER),
                                DataType = CellValues.String
                            },
                            new Cell()
                            {
                                CellValue = new CellValue(interview.CITY),
                                DataType = CellValues.String
                            }
                        );
                        foreach (ItemList item in special_identity)
                        {
                            bool has = false;
                            foreach (SPECIAL_IDENTITY si in interview.SPECIAL_IDENTITY) {
                                if (item.Key == si.PHRASE_KEY) {
                                    has = true;
                                    break;
                                }
                            }
                            if (has)
                            {
                                row.Append(new Cell() { CellValue = new CellValue("1"),
                                    DataType = CellValues.String
                                });
                            }
                            else
                            {
                                row.Append(new Cell() { CellValue = new CellValue("0"),
                                    DataType = CellValues.String
                                });
                            }
                        }
                        row.Append(
                            new Cell()
                            {
                                CellValue = new CellValue(interview.MARRIAGE),
                                DataType = CellValues.String
                            },
                            new Cell()
                            {
                                CellValue = new CellValue(interview.PHYSIOLOGY),
                                DataType = CellValues.String
                            },
                            new Cell()
                            {
                                CellValue = new CellValue(interview.PSYCHOLOGY),
                                DataType = CellValues.String
                            },
                            new Cell()
                            {
                                CellValue = new CellValue(interview.VISITED),
                                DataType = CellValues.String
                            }
                        );
                        foreach (ItemList item in interview_classify)
                        {
                            bool has = false;
                            foreach (INTERVIEW_CLASSIFY ic in interview.INTERVIEW_CLASSIFY)
                            {
                                if (item.Key == ic.PHRASE_KEY)
                                {
                                    has = true;
                                    break;
                                }
                            }
                            if (has)
                            {
                                row.Append(new Cell() { CellValue = new CellValue("1"),
                                    DataType = CellValues.String
                                });
                            }
                            else
                            {
                                row.Append(new Cell() { CellValue = new CellValue("0"),
                                    DataType = CellValues.String
                                });
                            }
                        }
                        row.Append(
                            new Cell()
                            {
                                CellValue = new CellValue(interview.SOLVE_DEGREE),
                                DataType = CellValues.String
                            }
                        );
                        foreach (ItemList item in feeling)
                        {
                            bool has = false;
                            foreach (FEELING fg in interview.FEELING)
                            {
                                if (item.Key == fg.PHRASE_KEY)
                                {
                                    has = true;
                                    break;
                                }
                            }
                            if (has)
                            {
                                row.Append(new Cell() { CellValue = new CellValue("1"),
                                    DataType = CellValues.String
                                });
                            }
                            else
                            {
                                row.Append(new Cell() { CellValue = new CellValue("0"),
                                    DataType = CellValues.String
                                });
                            }
                        }
                        foreach (ItemList item in intervention)
                        {
                            bool has = false;
                            foreach (INTERVENTION itn in interview.INTERVENTION)
                            {
                                if (item.Key == itn.PHRASE_KEY)
                                {
                                    has = true;
                                    break;
                                }
                            }
                            if (has)
                            {
                                row.Append(new Cell() { CellValue = new CellValue("1"),
                                    DataType = CellValues.String
                                });
                            }
                            else
                            {
                                row.Append(new Cell() { CellValue = new CellValue("0"),
                                    DataType = CellValues.String
                                });
                            }
                        }
                        row.Append(
                            new Cell()
                            {
                                CellValue = new CellValue(interview.OPINION01),
                                DataType = CellValues.String
                            },
                            new Cell()
                            {
                                CellValue = new CellValue(interview.OPINION02),
                                DataType = CellValues.String
                            },
                            new Cell()
                            {
                                CellValue = new CellValue(interview.OPINION03),
                                DataType = CellValues.String
                            },
                            new Cell()
                            {
                                CellValue = new CellValue(interview.OPINION04),
                                DataType = CellValues.String
                            },
                            new Cell()
                            {
                                CellValue = new CellValue(interview.OPINION05),
                                DataType = CellValues.String
                            },
                            new Cell()
                            {
                                CellValue = new CellValue(interview.OPINION06),
                                DataType = CellValues.String
                            },
                            new Cell()
                            {
                                CellValue = new CellValue(interview.OPINION07),
                                DataType = CellValues.String
                            },
                            new Cell()
                            {
                                CellValue = new CellValue(interview.OPINION08),
                                DataType = CellValues.String
                            },
                            new Cell()
                            {
                                CellValue = new CellValue(interview.OPINION09),
                                DataType = CellValues.String
                            }
                        );

                        sheetData.AppendChild(row);
                    }
                }
                    #endregion

                    //Worksheet worksheet = new Worksheet();
                    //worksheet.Append(sheetData);
                    //worksheetPart.Worksheet = worksheet;    //add a Worksheet to the WorksheetPart
                    worksheetPart.Worksheet = new Worksheet(sheetData);
                    wbsp.Stylesheet = GenerateStyleSheet();

                    //////create a MergeCells class to hold each MergeCell
                    //MergeCells mergeCells = new MergeCells();

                    //////append a MergeCell to the mergeCells for each set of merged cells
                    //mergeCells.Append(new MergeCell() { Reference = new StringValue("C1:F1") });

                    worksheetPart.Worksheet.InsertAfter(mergeCells, worksheetPart.Worksheet.Elements<SheetData>().First());

                    Sheets sheets = document.WorkbookPart.Workbook.AppendChild(new Sheets());
                    sheets.AppendChild(new Sheet()
                    {
                        Id = document.WorkbookPart.GetIdOfPart(document.WorkbookPart.WorksheetParts.First()),
                        SheetId = 1,
                        Name = "工作表1"
                    });
                    #endregion

                    workbookpart.Workbook.Save();
                    document.Close();
                }


                memoryStream.Seek(0, SeekOrigin.Begin);
            }
            catch(Exception ex)
            {
                Log("Err=" + ex.Message);
                Log(ex.StackTrace);
            }
            return File(memoryStream.ToArray(), "application/vnd.ms-excel", "輔導記錄明細表.xlsx");
        }


        private string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }

        private Stylesheet GenerateStyleSheet()
        {
            return new Stylesheet(
                new Fonts(
                    new Font(                                                               // Index 0 - The default font.
                        new FontSize() { Val = 12 },
                        new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                        new FontName() { Val = "新細明體" })
                    //new Font(                                                               // Index 1 - The bold font.
                    //    new Bold(),
                    //    new FontSize() { Val = 11 },
                    //    new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                    //    new FontName() { Val = "Calibri" }),
                    //new Font(                                                               // Index 2 - The Italic font.
                    //    new Italic(),
                    //    new FontSize() { Val = 11 },
                    //    new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                    //    new FontName() { Val = "Calibri" }),
                    //new Font(                                                               // Index 2 - The Times Roman font. with 16 size
                    //    new FontSize() { Val = 16 },
                    //    new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                    //    new FontName() { Val = "Times New Roman" })
                ),
                new Fills(
                    new Fill(                                                           // Index 0 - The default fill.
                        new PatternFill() { PatternType = PatternValues.None }),
                    new Fill(                                                           // Index 1 - The default fill of gray 125 (required)
                        new PatternFill() { PatternType = PatternValues.Gray125 }),
                    new Fill(                                                           // Index 2 - The green fill.
                        new PatternFill(
                            new ForegroundColor() { Rgb = new HexBinaryValue() { Value = "FF8AFF7F" } }
                        )
                        { PatternType = PatternValues.Solid }),
                    new Fill(                                                           // Index 3 - The orgine fill.
                        new PatternFill(
                            new ForegroundColor() { Rgb = new HexBinaryValue() { Value = "FFFCD05F" } }
                        )
                        { PatternType = PatternValues.Solid }),
                    new Fill(                                                           // Index 4 - The blue fill.
                        new PatternFill(
                            new ForegroundColor() { Rgb = new HexBinaryValue() { Value = "FF70B0FF" } }
                        )
                        { PatternType = PatternValues.Solid }),
                    new Fill(                                                           // Index 5 - The teal fill.
                        new PatternFill(
                            new ForegroundColor() { Rgb = new HexBinaryValue() { Value = "FF5EFFF1" } }
                        )
                        { PatternType = PatternValues.Solid }),
                    new Fill(                                                           // Index 6 - The red fill.
                        new PatternFill(
                            new ForegroundColor() { Rgb = new HexBinaryValue() { Value = "FFFFA8A8" } }
                        )
                        { PatternType = PatternValues.Solid })
                ),
                new Borders(
                    new Border(                                                         // Index 0 - The default border.
                        new LeftBorder(),
                        new RightBorder(),
                        new TopBorder(),
                        new BottomBorder(),
                        new DiagonalBorder()),
                    new Border(                                                         // Index 1 - Applies a Left, Right, Top, Bottom border to a cell
                        new LeftBorder(
                            new Color() { Auto = true }
                        )
                        { Style = BorderStyleValues.Thin },
                        new RightBorder(
                            new Color() { Auto = true }
                        )
                        { Style = BorderStyleValues.Thin },
                        new TopBorder(
                            new Color() { Auto = true }
                        )
                        { Style = BorderStyleValues.Thin },
                        new BottomBorder(
                            new Color() { Auto = true }
                        )
                        { Style = BorderStyleValues.Thin },
                        new DiagonalBorder())
                ),
                new CellFormats(
                    new CellFormat() { FontId = 0, FillId = 0, BorderId = 0 },                          // Index 0 - The default cell style.  If a cell does not have a style index applied it will use this style combination instead
                    new CellFormat() { FontId = 0, FillId = 2, BorderId = 0, ApplyFont = true },       // Index 1 -  
                    new CellFormat() { FontId = 0, FillId = 3, BorderId = 0, ApplyFont = true },       // Index 2 - 
                    new CellFormat() { FontId = 0, FillId = 4, BorderId = 0, ApplyFont = true },       // Index 3 - 
                    new CellFormat() { FontId = 0, FillId = 5, BorderId = 0, ApplyFill = true },       // Index 4 - 
                    new CellFormat() { FontId = 0, FillId = 6, BorderId = 0, ApplyFill = true },       // Index 5 - 
                    //new CellFormat(                                                                   // Index X - 
                    //    new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center }
                    //)
                    //{ FontId = 0, FillId = 0, BorderId = 0, ApplyAlignment = true },
                    new CellFormat() { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true }      // Index 6 - Border
                )
            ); // return
        }

        private static Stylesheet CreateStylesheet()
        {
            Stylesheet stylesheet1 = new Stylesheet() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "x14ac" } };
            stylesheet1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            stylesheet1.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");

            Fonts fonts1 = new Fonts() { Count = (UInt32Value)1U, KnownFonts = true };

            Font font1 = new Font();
            FontSize fontSize1 = new FontSize() { Val = 11D };
            Color color1 = new Color() { Theme = (UInt32Value)1U };
            FontName fontName1 = new FontName() { Val = "Calibri" };
            FontFamilyNumbering fontFamilyNumbering1 = new FontFamilyNumbering() { Val = 2 };
            FontScheme fontScheme1 = new FontScheme() { Val = FontSchemeValues.Minor };

            font1.Append(fontSize1);
            font1.Append(color1);
            font1.Append(fontName1);
            font1.Append(fontFamilyNumbering1);
            font1.Append(fontScheme1);

            fonts1.Append(font1);

            Fills fills1 = new Fills() { Count = (UInt32Value)5U };

            // FillId = 0
            Fill fill1 = new Fill();
            PatternFill patternFill1 = new PatternFill() { PatternType = PatternValues.None };
            fill1.Append(patternFill1);

            // FillId = 1
            Fill fill2 = new Fill();
            PatternFill patternFill2 = new PatternFill() { PatternType = PatternValues.Gray125 };
            fill2.Append(patternFill2);

            // FillId = 2, green
            Fill fill3 = new Fill();
            PatternFill patternFill3 = new PatternFill() { PatternType = PatternValues.Solid };
            ForegroundColor foregroundColor1 = new ForegroundColor() { Rgb = "FFa3ff7c" };
            BackgroundColor backgroundColor1 = new BackgroundColor() { Indexed = (UInt32Value)64U };
            patternFill3.Append(foregroundColor1);
            patternFill3.Append(backgroundColor1);
            fill3.Append(patternFill3);

            // FillId = 3, oringe
            Fill fill4 = new Fill();
            PatternFill patternFill4 = new PatternFill() { PatternType = PatternValues.Solid };
            ForegroundColor foregroundColor2 = new ForegroundColor() { Rgb = "FFffd366" };
            BackgroundColor backgroundColor2 = new BackgroundColor() { Indexed = (UInt32Value)64U };
            patternFill4.Append(foregroundColor2);
            patternFill4.Append(backgroundColor2);
            fill4.Append(patternFill4);

            // FillId = 4, blue
            Fill fill5 = new Fill();
            PatternFill patternFill5 = new PatternFill() { PatternType = PatternValues.Solid };
            ForegroundColor foregroundColor3 = new ForegroundColor() { Rgb = "FF70b5ff" };
            BackgroundColor backgroundColor3 = new BackgroundColor() { Indexed = (UInt32Value)64U };
            patternFill5.Append(foregroundColor3);
            patternFill5.Append(backgroundColor3);
            fill5.Append(patternFill5);

            fills1.Append(fill1);
            fills1.Append(fill2);
            fills1.Append(fill3);
            fills1.Append(fill4);
            fills1.Append(fill5);

            Borders borders1 = new Borders() { Count = (UInt32Value)1U };

            Border border1 = new Border();
            LeftBorder leftBorder1 = new LeftBorder();
            RightBorder rightBorder1 = new RightBorder();
            TopBorder topBorder1 = new TopBorder();
            BottomBorder bottomBorder1 = new BottomBorder();
            DiagonalBorder diagonalBorder1 = new DiagonalBorder();

            border1.Append(leftBorder1);
            border1.Append(rightBorder1);
            border1.Append(topBorder1);
            border1.Append(bottomBorder1);
            border1.Append(diagonalBorder1);

            borders1.Append(border1);

            CellStyleFormats cellStyleFormats1 = new CellStyleFormats() { Count = (UInt32Value)1U };
            CellFormat cellFormat1 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U };

            cellStyleFormats1.Append(cellFormat1);

            CellFormats cellFormats1 = new CellFormats() { Count = (UInt32Value)4U };
            CellFormat cellFormat2 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U };
            CellFormat cellFormat3 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)2U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U, ApplyFill = true };
            CellFormat cellFormat4 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)3U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U, ApplyFill = true };
            CellFormat cellFormat5 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)4U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U, ApplyFill = true };

            cellFormats1.Append(cellFormat2);
            cellFormats1.Append(cellFormat3);
            cellFormats1.Append(cellFormat4);
            cellFormats1.Append(cellFormat5);

            CellStyles cellStyles1 = new CellStyles() { Count = (UInt32Value)1U };
            CellStyle cellStyle1 = new CellStyle() { Name = "Normal", FormatId = (UInt32Value)0U, BuiltinId = (UInt32Value)0U };

            cellStyles1.Append(cellStyle1);
            DifferentialFormats differentialFormats1 = new DifferentialFormats() { Count = (UInt32Value)0U };
            TableStyles tableStyles1 = new TableStyles() { Count = (UInt32Value)0U, DefaultTableStyle = "TableStyleMedium2", DefaultPivotStyle = "PivotStyleMedium9" };

            StylesheetExtensionList stylesheetExtensionList1 = new StylesheetExtensionList();

            StylesheetExtension stylesheetExtension1 = new StylesheetExtension() { Uri = "{EB79DEF2-80B8-43e5-95BD-54CBDDF9020C}" };
            stylesheetExtension1.AddNamespaceDeclaration("x14", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
            X14.SlicerStyles slicerStyles1 = new X14.SlicerStyles() { DefaultSlicerStyle = "SlicerStyleLight1" };

            stylesheetExtension1.Append(slicerStyles1);

            stylesheetExtensionList1.Append(stylesheetExtension1);

            stylesheet1.Append(fonts1);
            stylesheet1.Append(fills1);
            stylesheet1.Append(borders1);
            stylesheet1.Append(cellStyleFormats1);
            stylesheet1.Append(cellFormats1);
            stylesheet1.Append(cellStyles1);
            stylesheet1.Append(differentialFormats1);
            stylesheet1.Append(tableStyles1);
            stylesheet1.Append(stylesheetExtensionList1);
            return stylesheet1;
        }

        //        [AcceptVerbs("POST", "GET")]
        //        public ActionResult InterviewReportExcel2()
        //        {

        //            var memoryStream = new MemoryStream();

        //            InterviewReportRetrieveReq req = null;
        //            RequestParameter para = new RequestParameter();
        //            para.Load(Request);
        //            req = new InterviewReportRetrieveReq();
        //            JsonConvert.PopulateObject(para.Item("json"), req);


        //            using (var document = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
        //            {
        //                var workbookPart = document.AddWorkbookPart();
        //                workbookPart.Workbook = new Workbook();

        //                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
        //                worksheetPart.Worksheet = new Worksheet(new SheetData());

        //                var sheets = workbookPart.Workbook.AppendChild(new Sheets());

        //                sheets.Append(new Sheet()
        //                {
        //                    Id = workbookPart.GetIdOfPart(worksheetPart),
        //                    SheetId = 1,
        //                    Name = "Sheet 1"
        //                });

        //                var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

        //                var row = new Row();

        //                string json = @"
        //{
        //    ""PhraseGroup"": [""SPECIAL_IDENTITY"", ""INTERVIEW_CLASSIFY"", ""FEELING"", ""INTERVENTION""]
        //}";
        //                ItemListRetrieveReq itemReq = JsonConvert.DeserializeObject<ItemListRetrieveReq>(json);
        //                ItemListRetrieveRes itemRes = new QueryItems("SCC").ItemListQuery(itemReq);

        //                Dictionary<string, object> pharse = new Dictionary<string, object>();
        //                pharse = (Dictionary<string, object>)itemRes.ItemList;

        //                #region header
        //                row = new Row();    //header
        //                row.Append(
        //                    new Cell()
        //                    {
        //                        CellValue = new CellValue("日期")
        //                    },
        //                    new Cell()
        //                    {
        //                        CellValue = new CellValue("星期")
        //                    },
        //                    new Cell()
        //                    {
        //                        CellValue = new CellValue("接案者代號")
        //                    },
        //                    new Cell()
        //                    {
        //                        CellValue = new CellValue("談話時間")
        //                    },
        //                    new Cell()
        //                    {
        //                        CellValue = new CellValue("聯絡時間")
        //                    },
        //                    new Cell()
        //                    {
        //                        CellValue = new CellValue("處遇方式")
        //                    },
        //                    new Cell()
        //                    {
        //                        CellValue = new CellValue("個案來源")
        //                    },
        //                    new Cell()
        //                    {
        //                        CellValue = new CellValue("主述性別")
        //                    },
        //                    new Cell()
        //                    {
        //                        CellValue = new CellValue("年齡層")
        //                    },
        //                    new Cell()
        //                    {
        //                        CellValue = new CellValue("學歷")
        //                    },
        //                    new Cell()
        //                    {
        //                        CellValue = new CellValue("職業")
        //                    },
        //                    new Cell()
        //                    {
        //                        CellValue = new CellValue("居住地")
        //                    }
        //                );
        //                List<ItemList> special_identity = (List<ItemList>)pharse["SPECIAL_IDENTITY"];
        //                foreach (ItemList item in special_identity)
        //                {
        //                    row.Append(new Cell() { CellValue = new CellValue(item.Value) });
        //                }
        //                row.Append(
        //                    new Cell()
        //                    {
        //                        CellValue = new CellValue("婚姻情感狀態")
        //                    },
        //                    new Cell()
        //                    {
        //                        CellValue = new CellValue("生理障礙")
        //                    },
        //                    new Cell()
        //                    {
        //                        CellValue = new CellValue("心理障礙")
        //                    },
        //                    new Cell()
        //                    {
        //                        CellValue = new CellValue("曾經來電")
        //                    }
        //                );
        //                List<ItemList> interview_classify = (List<ItemList>)pharse["INTERVIEW_CLASSIFY"];
        //                foreach (ItemList item in interview_classify)
        //                {
        //                    row.Append(new Cell() { CellValue = new CellValue(item.Value) });
        //                }
        //                List<ItemList> feeling = (List<ItemList>)pharse["FEELING"];
        //                foreach (ItemList item in feeling)
        //                {
        //                    row.Append(new Cell() { CellValue = new CellValue(item.Value) });
        //                }
        //                List<ItemList> intervention = (List<ItemList>)pharse["INTERVENTION"];
        //                foreach (ItemList item in intervention)
        //                {
        //                    row.Append(new Cell() { CellValue = new CellValue(item.Value) });
        //                }
        //                sheetData.AppendChild(row);

        //                #endregion

        //                #region data

        //                InterviewReportRetrieveRes res = new InterviewReport("KYL").ReportData(req);
        //                foreach (CASE_DETAIL interview in res.CASE_DETAIL)
        //                {
        //                    row = new Row();
        //                    row.Append(
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue(interview.INCOMING_DATE == null ? "" : interview.INCOMING_DATE.Value.ToString("yyyy-MM-dd"))
        //                        },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue(interview.INCOMING_DATE == null ? "" : (interview.INCOMING_DATE.Value.DayOfWeek == 0 ? 7 : (int)interview.INCOMING_DATE.Value.DayOfWeek).ToString())
        //                        },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue(interview.VOLUNTEER_SN)
        //                        },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue(interview.DURING == null ? "" : interview.DURING.ToString())
        //                        },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue(interview.CONTACT_TIME)
        //                        },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue(interview.TREATMENT)
        //                        },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue(interview.CASE_SOURCE)
        //                        },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue(interview.GENDER)
        //                        },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue(interview.AGE)
        //                        },
        //                       new Cell()
        //                       {
        //                           CellValue = new CellValue(interview.EDUCATION)
        //                       },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue(interview.CAREER)
        //                        },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue(interview.CITY)
        //                        }
        //                    );
        //                    foreach (ItemList item in special_identity)
        //                    {
        //                        bool has = false;
        //                        foreach (SPECIAL_IDENTITY si in interview.SPECIAL_IDENTITY)
        //                        {
        //                            if (item.Key == si.PHRASE_KEY)
        //                            {
        //                                has = true;
        //                                break;
        //                            }
        //                        }
        //                        if (has)
        //                        {
        //                            row.Append(new Cell() { CellValue = new CellValue("1") });
        //                        }
        //                        else
        //                        {
        //                            row.Append(new Cell() { CellValue = new CellValue("0") });
        //                        }
        //                    }
        //                    row.Append(
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue(interview.MARRIAGE)
        //                        },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue(interview.PHYSIOLOGY)
        //                        },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue(interview.PSYCHOLOGY)
        //                        },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue(interview.VISITED)
        //                        }
        //                    );
        //                    foreach (ItemList item in interview_classify)
        //                    {
        //                        bool has = false;
        //                        foreach (INTERVIEW_CLASSIFY ic in interview.INTERVIEW_CLASSIFY)
        //                        {
        //                            if (item.Key == ic.PHRASE_KEY)
        //                            {
        //                                has = true;
        //                                break;
        //                            }
        //                        }
        //                        if (has)
        //                        {
        //                            row.Append(new Cell() { CellValue = new CellValue("1") });
        //                        }
        //                        else
        //                        {
        //                            row.Append(new Cell() { CellValue = new CellValue("0") });
        //                        }
        //                    }
        //                    foreach (ItemList item in feeling)
        //                    {
        //                        bool has = false;
        //                        foreach (FEELING fg in interview.FEELING)
        //                        {
        //                            if (item.Key == fg.PHRASE_KEY)
        //                            {
        //                                has = true;
        //                                break;
        //                            }
        //                        }
        //                        if (has)
        //                        {
        //                            row.Append(new Cell() { CellValue = new CellValue("1") });
        //                        }
        //                        else
        //                        {
        //                            row.Append(new Cell() { CellValue = new CellValue("0") });
        //                        }
        //                    }
        //                    foreach (ItemList item in intervention)
        //                    {
        //                        bool has = false;
        //                        foreach (INTERVENTION itn in interview.INTERVENTION)
        //                        {
        //                            if (item.Key == itn.PHRASE_KEY)
        //                            {
        //                                has = true;
        //                                break;
        //                            }
        //                        }
        //                        if (has)
        //                        {
        //                            row.Append(new Cell() { CellValue = new CellValue("1") });
        //                        }
        //                        else
        //                        {
        //                            row.Append(new Cell() { CellValue = new CellValue("0") });
        //                        }
        //                    }

        //                    sheetData.AppendChild(row);
        //                }
        //                #endregion

        //            }

        //            memoryStream.Seek(0, SeekOrigin.Begin);
        //            //return new FileStreamResult(memoryStream,
        //            //    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        //            return File(memoryStream.ToArray(), "application/vnd.ms-excel", "輔導記錄明細表.xlsx");
        //        }

        //        [AcceptVerbs("POST", "GET")]
        //        public ActionResult InterviewReportExcel3()
        //        {
        //            var memoryStream = new MemoryStream();

        //            using (var document = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
        //            {
        //                var workbookPart = document.AddWorkbookPart();
        //                workbookPart.Workbook = new Workbook();

        //                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
        //                worksheetPart.Worksheet = new Worksheet(new SheetData());

        //                var sheets = workbookPart.Workbook.AppendChild(new Sheets());

        //                sheets.Append(new Sheet()
        //                {
        //                    Id = workbookPart.GetIdOfPart(worksheetPart),
        //                    SheetId = 1,
        //                    Name = "Sheet 1"
        //                });

        //                var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

        //                var row = new Row();
        //                row.Append(
        //                    new Cell()
        //                    {
        //                        CellValue = new CellValue("日期"),
        //                        DataType= CellValues.String
        //                    },
        //                    new Cell()
        //                    {
        //                        CellValue = new CellValue("0123")
        //                    },
        //                   new Cell()
        //                   {
        //                       CellValue = new CellValue("0123")
        //                   }
        //                );
        //                //row.Append(
        //                //    new Cell()
        //                //    {
        //                //        CellValue = new CellValue("No."),
        //                //        DataType = CellValues.String
        //                //    },
        //                //    new Cell()
        //                //    {
        //                //        CellValue = new CellValue("Name"),
        //                //        DataType = CellValues.String
        //                //    },
        //                //    new Cell()
        //                //    {
        //                //        CellValue = new CellValue("Links"),
        //                //        DataType = CellValues.String
        //                //    }
        //                //);
        //                sheetData.AppendChild(row);
        //                 string[][] _smapleData = new string[][]
        //        {
        //            new string[]{ "John Wu Blog","https://blog.johnwu.cc/" },
        //            new string[]{ "大內攻城粉絲團", "https://www.facebook.com/SoftwareENG.NET" }
        //        };
        //                for (var i = 0; i < _smapleData.Length; i++)
        //                {
        //                    var data = _smapleData[i];
        //                    row = new Row();
        //                    row.Append(
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue((i + 1).ToString()),
        //                            DataType = CellValues.Number
        //                        },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue(data[0]),
        //                            DataType = CellValues.String
        //                        },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue(data[1]),
        //                            DataType = CellValues.String
        //                        }
        //                    );
        //                    sheetData.AppendChild(row);
        //                }
        //            }

        //            memoryStream.Seek(0, SeekOrigin.Begin);
        //            //return new FileStreamResult(memoryStream,
        //            //    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        //            return File(memoryStream.ToArray(), "application/vnd.ms-excel", "輔導記錄明細表.xlsx");
        //        }


        //        [AcceptVerbs("POST", "GET")]
        //        public ActionResult InterviewReportExcel4()
        //        {
        //            InterviewReportRetrieveReq req = null;
        //            RequestParameter para = new RequestParameter();
        //            para.Load(Request);
        //            req = new InterviewReportRetrieveReq();
        //            JsonConvert.PopulateObject(para.Item("json"), req);

        //            var memoryStream = new MemoryStream();

        //            using (var document = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
        //            {
        //                WorkbookPart workbookpart = document.AddWorkbookPart();
        //                workbookpart.Workbook = new Workbook();
        //                WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();

        //                #region Sheet1
        //                SheetData sheetData = new SheetData();
        //                Row row;

        //                #region header
        //                row = new Row();    //header
        //                row.Append(
        //                    new Cell()
        //                    {
        //                        CellValue = new CellValue("日期")
        //                    },
        //                    new Cell()
        //                    {
        //                        CellValue = new CellValue("星期")
        //                    },
        //                    new Cell()
        //                    {
        //                        CellValue = new CellValue("接案者代號")
        //                    },
        //                    new Cell()
        //                    {
        //                        CellValue = new CellValue("談話時間")
        //                    },
        //                    new Cell()
        //                    {
        //                        CellValue = new CellValue("聯絡時間")
        //                    },
        //                    new Cell()
        //                    {
        //                        CellValue = new CellValue("處遇方式")
        //                    },
        //                    new Cell()
        //                    {
        //                        CellValue = new CellValue("個案來源")
        //                    },
        //                    new Cell()
        //                    {
        //                        CellValue = new CellValue("主述性別")
        //                    },
        //                    new Cell()
        //                    {
        //                        CellValue = new CellValue("年齡層")
        //                    },
        //                    new Cell()
        //                    {
        //                        CellValue = new CellValue("學歷")
        //                    },
        //                    new Cell()
        //                    {
        //                        CellValue = new CellValue("職業")
        //                    },
        //                    new Cell()
        //                    {
        //                        CellValue = new CellValue("居住地")
        //                    },
        //                    new Cell()
        //                    {
        //                        CellValue = new CellValue("婚姻情感狀態")
        //                    },
        //                    new Cell()
        //                    {
        //                        CellValue = new CellValue("生理障礙")
        //                    },
        //                    new Cell()
        //                    {
        //                        CellValue = new CellValue("心理障礙")
        //                    },
        //                    new Cell()
        //                    {
        //                        CellValue = new CellValue("曾經來電")
        //                    }
        //                );
        //                sheetData.AppendChild(row);

        //                #endregion

        //                #region data

        //                InterviewReportRetrieveRes res = new InterviewReport("KYL").ReportData(req);
        //                foreach (INTERVIEW data in res.CASE_DETAIL) //data
        //                {
        //                    row = new Row();
        //                    row.Append(
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue(data.INCOMING_DATE==null ? ""  : data.INCOMING_DATE.Value.ToString("yyyy-MM-dd"))
        //                        },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue(data.INCOMING_DATE == null ? "" : (data.INCOMING_DATE.Value.DayOfWeek == 0 ? 7 : (int)data.INCOMING_DATE.Value.DayOfWeek).ToString())
        //                        },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue(data.VOLUNTEER_SN)
        //                        },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue(data.DURING == null ? "" : data.DURING.ToString())
        //                        },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue(data.CONTACT_TIME)
        //                        },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue(data.TREATMENT)
        //                        },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue(data.CASE_SOURCE)
        //                        },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue(data.GENDER)
        //                        },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue(data.AGE)
        //                        },
        //                       new Cell()
        //                       {
        //                           CellValue = new CellValue(data.EDUCATION)
        //                       },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue(data.CAREER)
        //                        },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue(data.CITY)
        //                        },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue(data.MARRIAGE)
        //                        },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue(data.PHYSIOLOGY)
        //                        },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue(data.PSYCHOLOGY)
        //                        },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue(data.VISITED)
        //                        }
        //                    );
        //                    sheetData.AppendChild(row);
        //                }

        //                #endregion


        //                Worksheet worksheet = new Worksheet();
        //                worksheet.Append(sheetData);
        //                worksheetPart.Worksheet = worksheet;    //add a Worksheet to the WorksheetPart

        //                Sheets sheets = document.WorkbookPart.Workbook.AppendChild(new Sheets());
        //                sheets.AppendChild(new Sheet()
        //                {
        //                    Id = document.WorkbookPart.GetIdOfPart(document.WorkbookPart.WorksheetParts.First()),
        //                    SheetId = 1,
        //                    Name = "工作表1"
        //                });
        //                #endregion

        //                workbookpart.Workbook.Save();
        //                document.Close();
        //            }


        //            memoryStream.Seek(0, SeekOrigin.Begin);

        //            return File(memoryStream.ToArray(), "application/vnd.ms-excel", "輔導記錄明細表.xlsx");
        //        }

        //        [AcceptVerbs("POST", "GET")]
        //        public ActionResult InterviewReportExcel5()
        //        {
        //            var memoryStream = new MemoryStream();

        //            try
        //            {
        //                InterviewReportRetrieveReq req = null;
        //                RequestParameter para = new RequestParameter();
        //                para.Load(Request);
        //                req = new InterviewReportRetrieveReq();
        //                JsonConvert.PopulateObject(para.Item("json"), req);



        //                using (var document = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
        //                {
        //                    WorkbookPart workbookpart = document.AddWorkbookPart();
        //                    workbookpart.Workbook = new Workbook();
        //                    WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();

        //                    #region Sheet1
        //                    SheetData sheetData = new SheetData();
        //                    Row row;

        //                    #region header
        //                    row = new Row();    //header

        //                    string json = @"
        //{
        //    ""PhraseGroup"": [""SPECIAL_IDENTITY"", ""INTERVIEW_CLASSIFY"", ""FEELING"", ""INTERVENTION""]
        //}
        //";
        //                    ItemListRetrieveReq itemReq = JsonConvert.DeserializeObject<ItemListRetrieveReq>(json);
        //                    ItemListRetrieveRes itemRes = new QueryItems("SCC").ItemListQuery(itemReq);

        //                    Dictionary<string, object> pharse = new Dictionary<string, object>();
        //                    pharse = (Dictionary<string, object>)itemRes.ItemList;

        //                    row.Append(
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue("日期"),
        //                            DataType = CellValues.String
        //                        },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue("星期"),
        //                            DataType = CellValues.String
        //                        },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue("接案者代號"),
        //                            DataType = CellValues.String
        //                        },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue("談話時間"),
        //                            DataType = CellValues.String
        //                        },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue("聯絡時間"),
        //                            DataType = CellValues.String
        //                        },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue("處遇方式"),
        //                            DataType = CellValues.String
        //                        },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue("個案來源"),
        //                            DataType = CellValues.String
        //                        },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue("主述性別"),
        //                            DataType = CellValues.String
        //                        },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue("年齡層"),
        //                            DataType = CellValues.String
        //                        },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue("學歷"),
        //                            DataType = CellValues.String
        //                        },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue("職業"),
        //                            DataType = CellValues.String
        //                        },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue("居住地"),
        //                            DataType = CellValues.String
        //                        }
        //                    );
        //                    List<ItemList> special_identity = (List<ItemList>)pharse["SPECIAL_IDENTITY"];
        //                    foreach (ItemList item in special_identity)
        //                    {
        //                        row.Append(new Cell()
        //                        {
        //                            CellValue = new CellValue(item.Value),
        //                            DataType = CellValues.String
        //                        });
        //                    }
        //                    row.Append(
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue("婚姻情感狀態"),
        //                            DataType = CellValues.String
        //                        },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue("生理障礙"),
        //                            DataType = CellValues.String
        //                        },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue("心理障礙"),
        //                            DataType = CellValues.String
        //                        },
        //                        new Cell()
        //                        {
        //                            CellValue = new CellValue("曾經來電"),
        //                            DataType = CellValues.String
        //                        }
        //                    );
        //                    List<ItemList> interview_classify = (List<ItemList>)pharse["INTERVIEW_CLASSIFY"];
        //                    foreach (ItemList item in interview_classify)
        //                    {
        //                        row.Append(new Cell()
        //                        {
        //                            CellValue = new CellValue(item.Value),
        //                            DataType = CellValues.String
        //                        });
        //                    }
        //                    List<ItemList> feeling = (List<ItemList>)pharse["FEELING"];
        //                    foreach (ItemList item in feeling)
        //                    {
        //                        row.Append(new Cell()
        //                        {
        //                            CellValue = new CellValue(item.Value),
        //                            DataType = CellValues.String
        //                        });
        //                    }
        //                    List<ItemList> intervention = (List<ItemList>)pharse["INTERVENTION"];
        //                    foreach (ItemList item in intervention)
        //                    {
        //                        row.Append(new Cell()
        //                        {
        //                            CellValue = new CellValue(item.Value),
        //                            DataType = CellValues.String
        //                        });
        //                    }
        //                    sheetData.AppendChild(row);

        //                    #endregion

        //                    #region data

        //                    InterviewReportRetrieveRes res = new InterviewReport("KYL").ReportData(req);
        //                    if (true)
        //                    {
        //                        foreach (CASE_DETAIL interview in res.CASE_DETAIL)
        //                        {
        //                            row = new Row();
        //                            row.Append(
        //                                new Cell()
        //                                {
        //                                    CellValue = new CellValue(interview.INCOMING_DATE == null ? "" : interview.INCOMING_DATE.Value.ToString("yyyy-MM-dd")),
        //                                    DataType = CellValues.String

        //                                },
        //                                new Cell()
        //                                {
        //                                    CellValue = new CellValue(interview.INCOMING_DATE == null ? "" : (interview.INCOMING_DATE.Value.DayOfWeek == 0 ? 7 : (int)interview.INCOMING_DATE.Value.DayOfWeek).ToString()),
        //                                    DataType = CellValues.String
        //                                },
        //                                new Cell()
        //                                {
        //                                    CellValue = new CellValue(interview.VOLUNTEER_SN),
        //                                    DataType = CellValues.String
        //                                },
        //                                new Cell()
        //                                {
        //                                    CellValue = new CellValue(interview.DURING == null ? "" : interview.DURING.ToString()),
        //                                    DataType = CellValues.String
        //                                },
        //                                new Cell()
        //                                {
        //                                    CellValue = new CellValue(interview.CONTACT_TIME),
        //                                    DataType = CellValues.String
        //                                },
        //                                new Cell()
        //                                {
        //                                    CellValue = new CellValue(interview.TREATMENT),
        //                                    DataType = CellValues.String
        //                                },
        //                                new Cell()
        //                                {
        //                                    CellValue = new CellValue(interview.CASE_SOURCE),
        //                                    DataType = CellValues.String
        //                                },
        //                                new Cell()
        //                                {
        //                                    CellValue = new CellValue(interview.GENDER),
        //                                    DataType = CellValues.String
        //                                },
        //                                new Cell()
        //                                {
        //                                    CellValue = new CellValue(interview.AGE),
        //                                    DataType = CellValues.String
        //                                },
        //                               new Cell()
        //                               {
        //                                   CellValue = new CellValue(interview.EDUCATION),
        //                                   DataType = CellValues.String
        //                               },
        //                                new Cell()
        //                                {
        //                                    CellValue = new CellValue(interview.CAREER),
        //                                    DataType = CellValues.String
        //                                },
        //                                new Cell()
        //                                {
        //                                    CellValue = new CellValue(interview.CITY),
        //                                    DataType = CellValues.String
        //                                }
        //                            );
        //                            foreach (ItemList item in special_identity)
        //                            {
        //                                bool has = false;
        //                                foreach (SPECIAL_IDENTITY si in interview.SPECIAL_IDENTITY)
        //                                {
        //                                    if (item.Key == si.PHRASE_KEY)
        //                                    {
        //                                        has = true;
        //                                        break;
        //                                    }
        //                                }
        //                                if (has)
        //                                {
        //                                    row.Append(new Cell()
        //                                    {
        //                                        CellValue = new CellValue("1"),
        //                                        DataType = CellValues.String
        //                                    });
        //                                }
        //                                else
        //                                {
        //                                    row.Append(new Cell()
        //                                    {
        //                                        CellValue = new CellValue("0"),
        //                                        DataType = CellValues.String
        //                                    });
        //                                }
        //                            }
        //                            row.Append(
        //                                new Cell()
        //                                {
        //                                    CellValue = new CellValue(interview.MARRIAGE),
        //                                    DataType = CellValues.String
        //                                },
        //                                new Cell()
        //                                {
        //                                    CellValue = new CellValue(interview.PHYSIOLOGY),
        //                                    DataType = CellValues.String
        //                                },
        //                                new Cell()
        //                                {
        //                                    CellValue = new CellValue(interview.PSYCHOLOGY),
        //                                    DataType = CellValues.String
        //                                },
        //                                new Cell()
        //                                {
        //                                    CellValue = new CellValue(interview.VISITED),
        //                                    DataType = CellValues.String
        //                                }
        //                            );
        //                            foreach (ItemList item in interview_classify)
        //                            {
        //                                bool has = false;
        //                                foreach (INTERVIEW_CLASSIFY ic in interview.INTERVIEW_CLASSIFY)
        //                                {
        //                                    if (item.Key == ic.PHRASE_KEY)
        //                                    {
        //                                        has = true;
        //                                        break;
        //                                    }
        //                                }
        //                                if (has)
        //                                {
        //                                    row.Append(new Cell()
        //                                    {
        //                                        CellValue = new CellValue("1"),
        //                                        DataType = CellValues.String
        //                                    });
        //                                }
        //                                else
        //                                {
        //                                    row.Append(new Cell()
        //                                    {
        //                                        CellValue = new CellValue("0"),
        //                                        DataType = CellValues.String
        //                                    });
        //                                }
        //                            }
        //                            foreach (ItemList item in feeling)
        //                            {
        //                                bool has = false;
        //                                foreach (FEELING fg in interview.FEELING)
        //                                {
        //                                    if (item.Key == fg.PHRASE_KEY)
        //                                    {
        //                                        has = true;
        //                                        break;
        //                                    }
        //                                }
        //                                if (has)
        //                                {
        //                                    row.Append(new Cell()
        //                                    {
        //                                        CellValue = new CellValue("1"),
        //                                        DataType = CellValues.String
        //                                    });
        //                                }
        //                                else
        //                                {
        //                                    row.Append(new Cell()
        //                                    {
        //                                        CellValue = new CellValue("0"),
        //                                        DataType = CellValues.String
        //                                    });
        //                                }
        //                            }
        //                            foreach (ItemList item in intervention)
        //                            {
        //                                bool has = false;
        //                                foreach (INTERVENTION itn in interview.INTERVENTION)
        //                                {
        //                                    if (item.Key == itn.PHRASE_KEY)
        //                                    {
        //                                        has = true;
        //                                        break;
        //                                    }
        //                                }
        //                                if (has)
        //                                {
        //                                    row.Append(new Cell()
        //                                    {
        //                                        CellValue = new CellValue("1"),
        //                                        DataType = CellValues.String
        //                                    });
        //                                }
        //                                else
        //                                {
        //                                    row.Append(new Cell()
        //                                    {
        //                                        CellValue = new CellValue("0"),
        //                                        DataType = CellValues.String
        //                                    });
        //                                }
        //                            }

        //                            sheetData.AppendChild(row);
        //                        }
        //                    }
        //                    #endregion

        //                    Worksheet worksheet = new Worksheet();
        //                    worksheet.Append(sheetData);
        //                    worksheetPart.Worksheet = worksheet;    //add a Worksheet to the WorksheetPart

        //                    Sheets sheets = document.WorkbookPart.Workbook.AppendChild(new Sheets());
        //                    sheets.AppendChild(new Sheet()
        //                    {
        //                        Id = document.WorkbookPart.GetIdOfPart(document.WorkbookPart.WorksheetParts.First()),
        //                        SheetId = 1,
        //                        Name = "工作表1"
        //                    });
        //                    #endregion

        //                    workbookpart.Workbook.Save();
        //                    document.Close();
        //                }


        //                memoryStream.Seek(0, SeekOrigin.Begin);
        //            }
        //            catch (Exception ex)
        //            {
        //                Log("Err=" + ex.Message);
        //                Log(ex.StackTrace);
        //            }
        //            return File(memoryStream.ToArray(), "application/vnd.ms-excel", "輔導記錄明細表.xlsx");
        //        }
    }
}