using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using KYL_CMS.Models.DataAccess;
using KYL_CMS.Models.EntityDefinition;
using KYL_CMS.Models.DataClass;
using KYL_CMS.Models.DataClass.Phrase;
using KYL_CMS.Models.HelpLibrary;

namespace KYL_CMS.Models.BusinessLogic
{
    public class Phrase : DatabaseAccess
    {
        public Phrase(string connectionStringName) : base(connectionStringName)
        { }
        public PHRASE ModificationQuery(int? SN)
        {
            PHRASE row = null;

            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
SELECT SN,PHRASE_GROUP,PHRASE_KEY,PHRASE_VALUE,PHRASE_DESC,SORT,MODE,CDATE,CUSER,MDATE,MUSER
    FROM PHRASE
    WHERE SN=@SN
";
                Db.AddInParameter(cmd, "SN", DbType.Int32, SN);

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                using (IDataReader reader = Db.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        row = new PHRASE
                        {
                            SN = reader["SN"] as Int32? ?? null,
                            PHRASE_GROUP = reader["PHRASE_GROUP"] as string,
                            PHRASE_KEY = reader["PHRASE_KEY"] as string,
                            PHRASE_VALUE = reader["PHRASE_VALUE"] as string,
                            PHRASE_DESC = reader["PHRASE_DESC"] as string,
                            SORT = reader["SORT"] as Int32? ?? null,
                            MODE = reader["MODE"] as string,
                            CDATE = reader["CDATE"] as DateTime?,
                            CUSER = reader["CUSER"] as string,
                            MDATE = reader["MDATE"] as DateTime?,
                            MUSER = reader["MUSER"] as string
                        };
                    }
                }
            }

            return row;
        }

        public int DataCreate(PhraseModifyReq req)
        {
            int count = 0;
            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                Int64? PHRASE_SN = new Sequence("SCC").GetSeqBigInt("PHRASE");

                string sql = @"
INSERT PHRASE (SN,PHRASE_GROUP,PHRASE_KEY,PHRASE_VALUE,PHRASE_DESC,SORT,MODE,CDATE,CUSER,MDATE,MUSER)
    VALUES (@SN,@PHRASE_GROUP,@PHRASE_KEY,@PHRASE_VALUE,@PHRASE_DESC,@SORT,@MODE,GETDATE(),@CUSER,GETDATE(),@MUSER);
";
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;

                Db.AddInParameter(cmd, "SN", DbType.Int32, PHRASE_SN);
                Db.AddInParameter(cmd, "PHRASE_GROUP", DbType.String, req.PHRASE.PHRASE_GROUP);
                Db.AddInParameter(cmd, "PHRASE_KEY", DbType.String, req.PHRASE.PHRASE_KEY);
                Db.AddInParameter(cmd, "PHRASE_VALUE", DbType.String, req.PHRASE.PHRASE_VALUE);
                Db.AddInParameter(cmd, "PHRASE_DESC", DbType.String, req.PHRASE.PHRASE_DESC);
                Db.AddInParameter(cmd, "SORT", DbType.String, req.PHRASE.SORT);
                Db.AddInParameter(cmd, "MODE", DbType.String, req.PHRASE.MODE);
                Db.AddInParameter(cmd, "CUSER", DbType.String, req.PHRASE.CUSER);
                Db.AddInParameter(cmd, "MUSER", DbType.String, req.PHRASE.MUSER);

                count = Db.ExecuteNonQuery(cmd);
                req.PHRASE.SN = (int?)PHRASE_SN;
            }
            return count;
        }

        public int DataUpdate(PhraseModifyReq req)
        {
            int count = 0;
            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
UPDATE PHRASE
	SET PHRASE_GROUP=@PHRASE_GROUP,PHRASE_KEY=@PHRASE_KEY,PHRASE_VALUE=@PHRASE_VALUE,PHRASE_DESC=@PHRASE_DESC,SORT=@SORT,MODE=@MODE,MDATE=GETDATE(),MUSER=@MUSER
    WHERE SN=@SN;
";
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;

                Db.AddInParameter(cmd, "SN", DbType.String, req.PHRASE.SN);
                Db.AddInParameter(cmd, "PHRASE_GROUP", DbType.String, req.PHRASE.PHRASE_GROUP);
                Db.AddInParameter(cmd, "PHRASE_KEY", DbType.String, req.PHRASE.PHRASE_KEY);
                Db.AddInParameter(cmd, "PHRASE_VALUE", DbType.String, req.PHRASE.PHRASE_VALUE);
                Db.AddInParameter(cmd, "PHRASE_DESC", DbType.String, req.PHRASE.PHRASE_DESC);
                Db.AddInParameter(cmd, "SORT", DbType.String, req.PHRASE.SORT);
                Db.AddInParameter(cmd, "MODE", DbType.String, req.PHRASE.MODE);
                Db.AddInParameter(cmd, "MUSER", DbType.String, req.PHRASE.MUSER);
                count = Db.ExecuteNonQuery(cmd);
            }
            return count;
        }

        public int DataDelete(PhraseModifyReq req)
        {
            int count = 0;
            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
DELETE FROM PHRASE
    WHERE SN=@SN;
";
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                Db.AddInParameter(cmd, "SN", DbType.String, req.PHRASE.SN);
                count = Db.ExecuteNonQuery(cmd);
            }
            return count;
        }

        public PhraseRetrieveRes PaginationRetrieve(PhraseRetrieveReq req)
        {
            PhraseRetrieveRes res = new PhraseRetrieveRes()
            {
                PHRASE = new List<PHRASE>(),
                Pagination = new Pagination
                {
                    PageCount = 0,
                    RowCount = 0,
                    PageNumber = 0,
                    MinNumber = 0,
                    MaxNumber = 0,
                    StartTime = DateTime.Now
                }
            };

            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"SELECT COUNT(1) FROM (SELECT TOP(@TOP) NULL AS N FROM PHRASE{0}) A;
SELECT TOP(@TOP) SN,PHRASE_GROUP,PHRASE_KEY,PHRASE_VALUE,PHRASE_DESC,SORT,dbo.PHRASE_NAME('mode',MODE) AS MODE,CDATE,CUSER,MDATE,MUSER
    FROM PHRASE{0}
    ORDER BY SN DESC;";
                string where = "";
                Db.AddInParameter(cmd, "TOP", DbType.Int32, 1000);

                if (!string.IsNullOrEmpty(req.PHRASE.PHRASE_GROUP))
                {
                    where += " AND PHRASE_GROUP=@PHRASE_GROUP";
                    Db.AddInParameter(cmd, "PHRASE_GROUP", DbType.String, req.PHRASE.PHRASE_GROUP);
                }
                if (!string.IsNullOrEmpty(req.PHRASE.PHRASE_KEY))
                {
                    where += " AND PHRASE_KEY LIKE @PHRASE_KEY";
                    Db.AddInParameter(cmd, "PHRASE_KEY", DbType.String, "%" + req.PHRASE.PHRASE_KEY + "%");
                }
                if (!string.IsNullOrEmpty(req.PHRASE.PHRASE_VALUE))
                {
                    where += " AND PHRASE_VALUE LIKE @PHRASE_VALUE";
                    Db.AddInParameter(cmd, "PHRASE_VALUE", DbType.String, "%" + req.PHRASE.PHRASE_VALUE + "%");
                }
                if (!string.IsNullOrEmpty(req.PHRASE.MODE))
                {
                    where += " AND MODE=@MODE";
                    Db.AddInParameter(cmd, "MODE", DbType.String, req.PHRASE.MODE);
                }
                if (where.Length > 0)
                {
                    where = " WHERE" + where.Substring(4);
                }

                sql = String.Format(sql, where);
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                using (IDataReader reader = Db.ExecuteReader(cmd))
                {
                    reader.Read();
                    int.TryParse(reader[0].ToString(), out res.Pagination.RowCount);
                    if (res.Pagination.RowCount > 0)
                    {
                        reader.NextResult();

                        res.Pagination.PageCount = Convert.ToInt32(Math.Ceiling(1.0 * res.Pagination.RowCount / req.PageSize));
                        res.Pagination.PageNumber = req.PageNumber < 1 ? 1 : req.PageNumber;
                        res.Pagination.PageNumber = req.PageNumber > res.Pagination.PageCount ? res.Pagination.PageCount : res.Pagination.PageNumber;
                        res.Pagination.MinNumber = (res.Pagination.PageNumber - 1) * req.PageSize + 1;
                        res.Pagination.MaxNumber = res.Pagination.PageNumber * req.PageSize;
                        res.Pagination.MaxNumber = res.Pagination.MaxNumber > res.Pagination.RowCount ? res.Pagination.RowCount : res.Pagination.MaxNumber;

                        int i = 0;
                        while (reader.Read())
                        {
                            i++;
                            if (i >= res.Pagination.MinNumber && i <= res.Pagination.MaxNumber)
                            {
                                var row = new PHRASE
                                {
                                    SN = reader["SN"] as Int32? ?? null,
                                    PHRASE_GROUP = reader["PHRASE_GROUP"] as string,
                                    PHRASE_KEY = reader["PHRASE_KEY"] as string,
                                    PHRASE_VALUE = reader["PHRASE_VALUE"] as string,
                                    PHRASE_DESC = reader["PHRASE_DESC"] as string,
                                    SORT = reader["SORT"] as Int32? ?? null,
                                    MODE = reader["MODE"] as string,
                                    CDATE = reader["CDATE"] as DateTime?,
                                    CUSER = reader["CUSER"] as string,
                                    MDATE = reader["MDATE"] as DateTime?,
                                    MUSER = reader["MUSER"] as string
                                };
                                res.PHRASE.Add(row);
                            }
                            else if (i > res.Pagination.MaxNumber)
                            {
                                reader.Close();
                                break;
                            }
                        }
                    }
                }
            }
            res.Pagination.EndTime = DateTime.Now;

            return res;
        }

        
//        public PhraseRetrieveRes ReportData(PhraseRetrieveReq req)
//        {
//            PhraseRetrieveRes res = new PhraseRetrieveRes()
//            {
//                PHRASE = new List<PHRASE>(),
//                Pagination = new Pagination
//                {
//                    PageCount = 0,
//                    RowCount = 0,
//                    PageNumber = 0,
//                    MinNumber = 0,
//                    MaxNumber = 0,
//                    StartTime = DateTime.Now
//                }
//            };

//            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
//            {
//                string sql = @"SELECT TOP(@TOP)
//SN,PHRASE_GROUP,PHRASE_KEY,PHRASE_VALUE,PHRASE_DESC,SORT,MODE,CDATE,CUSER,MDATE,MUSER,
//    FROM PHRASE{0}
//    ORDER BY SN DESC;";
//                string where = "";
//                Db.AddInParameter(cmd, "TOP", DbType.Int32, 1000000);

//                //if (!string.IsNullOrEmpty(req.PHRASE.SN))
//                //{
//                //    where += " AND SN=@SN";
//                //    Db.AddInParameter(cmd, "SN", DbType.String, req.PHRASE.SN);
//                //}
//                if (!string.IsNullOrEmpty(req.PHRASE.PHRASE_GROUP))
//                {
//                    where += " AND PHRASE_GROUP=@PHRASE_GROUP";
//                    Db.AddInParameter(cmd, "PHRASE_GROUP", DbType.String, req.PHRASE.PHRASE_GROUP);
//                }
//                if (!string.IsNullOrEmpty(req.PHRASE.PHRASE_KEY))
//                {
//                    where += " AND PHRASE_KEY=@PHRASE_KEY";
//                    Db.AddInParameter(cmd, "PHRASE_KEY", DbType.String, req.PHRASE.PHRASE_KEY);
//                }
//                if (!string.IsNullOrEmpty(req.PHRASE.PHRASE_VALUE))
//                {
//                    where += " AND PHRASE_VALUE=@PHRASE_VALUE";
//                    Db.AddInParameter(cmd, "PHRASE_VALUE", DbType.String, req.PHRASE.PHRASE_VALUE);
//                }
//                if (!string.IsNullOrEmpty(req.PHRASE.PHRASE_DESC))
//                {
//                    where += " AND PHRASE_DESC=@PHRASE_DESC";
//                    Db.AddInParameter(cmd, "PHRASE_DESC", DbType.String, req.PHRASE.PHRASE_DESC);
//                }
//                //if (!string.IsNullOrEmpty(req.PHRASE.SORT))
//                //{
//                //    where += " AND SORT=@SORT";
//                //    Db.AddInParameter(cmd, "SORT", DbType.String, req.PHRASE.SORT);
//                //}
//                if (!string.IsNullOrEmpty(req.PHRASE.MODE))
//                {
//                    where += " AND MODE=@MODE";
//                    Db.AddInParameter(cmd, "MODE", DbType.String, req.PHRASE.MODE);
//                }
//                //if (!string.IsNullOrEmpty(req.PHRASE.CDATE))
//                //{
//                //    where += " AND CDATE=@CDATE";
//                //    Db.AddInParameter(cmd, "CDATE", DbType.String, req.PHRASE.CDATE);
//                //}
//                //if (!string.IsNullOrEmpty(req.PHRASE.CUSER))
//                //{
//                //    where += " AND CUSER=@CUSER";
//                //    Db.AddInParameter(cmd, "CUSER", DbType.String, req.PHRASE.CUSER);
//                //}
//                //if (!string.IsNullOrEmpty(req.PHRASE.MDATE))
//                //{
//                //    where += " AND MDATE=@MDATE";
//                //    Db.AddInParameter(cmd, "MDATE", DbType.String, req.PHRASE.MDATE);
//                //}
//                //if (!string.IsNullOrEmpty(req.PHRASE.MUSER))
//                //{
//                //    where += " AND MUSER=@MUSER";
//                //    Db.AddInParameter(cmd, "MUSER", DbType.String, req.PHRASE.MUSER);
//                //}
//                if (where.Length > 0)
//                {
//                    where = " WHERE" + where.Substring(4);
//                }

//                sql = String.Format(sql, where);
//                cmd.CommandType = CommandType.Text;
//                cmd.CommandText = sql;
//                using (IDataReader reader = Db.ExecuteReader(cmd))
//                {
//                    while (reader.Read())
//                    {
//                        var row = new PHRASE
//                        {

//                            SN = reader["SN"] as Int32? ?? null,
//                            PHRASE_GROUP = reader["PHRASE_GROUP"] as string,
//                            PHRASE_KEY = reader["PHRASE_KEY"] as string,
//                            PHRASE_VALUE = reader["PHRASE_VALUE"] as string,
//                            PHRASE_DESC = reader["PHRASE_DESC"] as string,
//                            SORT = reader["SORT"] as Int32? ?? null,
//                            MODE = reader["MODE"] as string,
//                            CDATE = reader["CDATE"] as DateTime?,
//                            CUSER = reader["CUSER"] as string,
//                            MDATE = reader["MDATE"] as DateTime?,
//                            MUSER = reader["MUSER"] as string
//                        };
//                        res.PHRASE.Add(row);
//                    }
//                }
//            }
//            res.Pagination.EndTime = DateTime.Now;

//            return res;
//        }

    }
}
