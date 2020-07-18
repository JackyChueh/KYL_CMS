using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using KYL_CMS.Models.DataAccess;
using KYL_CMS.Models.EntityDefinition;
using KYL_CMS.Models.DataClass;
using KYL_CMS.Models.Interface;
using KYL_CMS.Models.DataClass.Function;

namespace KYL_CMS.Models.BusinessLogic
{
    public class Function : DatabaseAccess, IPaginationg<FunctionRetrieveReq, FunctionRetrieveRes>
    {
        public Function(string connectionStringName) : base(connectionStringName) { }

        public FUNCTIONS ModificationQuery(Int16? SN)
        {
            FUNCTIONS row = null;

            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
SELECT SN, NAME, MODE, CATEGORY, URL, PARENT_SN, SORT, CDATE, CUSER, MDATE, MUSER
    FROM FUNCTIONS WHERE SN=@SN;
";
                Db.AddInParameter(cmd, "SN", DbType.Int32, SN);
                
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                using (IDataReader reader = Db.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                            row = new FUNCTIONS
                            {
                                SN = reader["SN"] as Int16? ?? null,
                                NAME = reader["NAME"] as string,
                                MODE = reader["MODE"] as string,
                                CATEGORY = reader["CATEGORY"] as string,
                                URL = reader["URL"] as string,
                                PARENT_SN = reader["PARENT_SN"] as Int16? ?? null,
                                SORT = reader["SORT"] as Int16? ?? null,
                                CDATE = reader["CDATE"] as DateTime? ?? null,
                                CUSER = reader["CUSER"] as Int16? ?? null,
                                MDATE = reader["MDATE"] as DateTime? ?? null,
                                MUSER = reader["MUSER"] as Int16? ?? null
                            };
                    }
                }
            }

            return row;
        }

        public int DataCreate(FunctionModifyReq req)
        {
            int count = 0;
            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
INSERT INTO FUNCTIONS (NAME,CATEGORY,PARENT_SN,URL,SORT,MODE,CDATE,CUSER,MDATE,MUSER) 
    VALUES (@NAME,@CATEGORY,@PARENT_SN,@URL,@SORT,@MODE,GETDATE(),@CUSER,GETDATE(),@MUSER) SET @SN = SCOPE_IDENTITY();";
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;

                Db.AddInParameter(cmd, "NAME", DbType.String, req.FUNCTIONS.NAME);
                Db.AddInParameter(cmd, "CATEGORY", DbType.String, req.FUNCTIONS.CATEGORY);
                Db.AddInParameter(cmd, "PARENT_SN", DbType.Int16, req.FUNCTIONS.PARENT_SN);
                Db.AddInParameter(cmd, "URL", DbType.String, req.FUNCTIONS.URL);
                Db.AddInParameter(cmd, "SORT", DbType.Int16, req.FUNCTIONS.SORT);
                Db.AddInParameter(cmd, "MODE", DbType.String, req.FUNCTIONS.MODE);
                //Db.AddInParameter(cmd, "CDATE", DbType.DateTime, req.FUNCTIONS.CDATE);
                Db.AddInParameter(cmd, "CUSER", DbType.Int16, req.FUNCTIONS.CUSER);
                //Db.AddInParameter(cmd, "MDATE", DbType.DateTime, req.FUNCTIONS.MDATE);
                Db.AddInParameter(cmd, "MUSER", DbType.Int16, req.FUNCTIONS.MUSER);
                Db.AddOutParameter(cmd, "SN", DbType.Int16, 1);
                count = Db.ExecuteNonQuery(cmd);
                req.FUNCTIONS.SN = Db.GetParameterValue(cmd, "SN") as Int16? ?? null;
            }
            return count;
        }

        public int DataUpdate(FunctionModifyReq req)
        {
            int count = 0;
            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
UPDATE FUNCTIONS
	SET NAME=@NAME,CATEGORY=@CATEGORY,PARENT_SN=@PARENT_SN,URL=@URL,SORT=@SORT
        ,MODE=@MODE,MDATE=GETDATE(),MUSER=@MUSER
WHERE SN=@SN;
";
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                Db.AddInParameter(cmd, "SN", DbType.String, req.FUNCTIONS.SN);
                Db.AddInParameter(cmd, "NAME", DbType.String, req.FUNCTIONS.NAME);
                Db.AddInParameter(cmd, "CATEGORY", DbType.String, req.FUNCTIONS.CATEGORY);
                Db.AddInParameter(cmd, "PARENT_SN", DbType.Int16, req.FUNCTIONS.PARENT_SN);
                Db.AddInParameter(cmd, "URL", DbType.String, req.FUNCTIONS.URL);
                Db.AddInParameter(cmd, "SORT", DbType.Int16, req.FUNCTIONS.SORT);
                Db.AddInParameter(cmd, "MODE", DbType.String, req.FUNCTIONS.MODE);
                //Db.AddInParameter(cmd, "MDATE", DbType.DateTime, req.FUNCTIONS.MDATE);
                Db.AddInParameter(cmd, "MUSER", DbType.Int16, req.FUNCTIONS.MUSER);
                count = Db.ExecuteNonQuery(cmd);
            }
            return count;
        }

        public int DataDelete()
        {
            return 0;
        }

        public FunctionRetrieveRes PaginationRetrieve(FunctionRetrieveReq req)
        {
            FunctionRetrieveRes res = new FunctionRetrieveRes()
            {
                FUNCTIONS = new List<FUNCTIONS>(),
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
                string sql = @"SELECT COUNT(1) FROM (SELECT TOP(@TOP) SN FROM [FUNCTIONS]{0}) A;
SELECT TOP(@TOP) SN, NAME, MODE, CATEGORY, URL, PARENT_SN, SORT, CDATE, CUSER, MDATE, MUSER
    FROM [FUNCTIONS]{0};";
                string where = "";
                Db.AddInParameter(cmd, "TOP", DbType.Int32, 1000000);

                if (req.FUNCTIONS.SN != null)
                {
                    where += " AND SN=@SN";
                    Db.AddInParameter(cmd, "SN", DbType.Int32, req.FUNCTIONS.SN);
                }
                if (req.FUNCTIONS.NAME != null)
                {
                    where += " AND NAME LIKE '%' + @NAME + '%'";
                    Db.AddInParameter(cmd, "NAME", DbType.String, req.FUNCTIONS.NAME);
                }
                if (req.FUNCTIONS.MODE != null)
                {
                    where += " AND MODE=@MODE";
                    Db.AddInParameter(cmd, "MODE", DbType.String, req.FUNCTIONS.MODE);
                }
                if (req.FUNCTIONS.CATEGORY != null)
                {
                    where += " AND CATEGORY=@CATEGORY";
                    Db.AddInParameter(cmd, "CATEGORY", DbType.String, req.FUNCTIONS.CATEGORY);
                }
                if (req.FUNCTIONS.PARENT_SN != null)
                {
                    where += " AND PARENT_SN=@PARENT_SN";
                    Db.AddInParameter(cmd, "PARENT_SN", DbType.Int32, req.FUNCTIONS.PARENT_SN);
                }
                if (req.FUNCTIONS.URL != null)
                {
                    where += " AND URL LIKE '%' + @URL + '%'";
                    Db.AddInParameter(cmd, "URL", DbType.String, req.FUNCTIONS.URL);
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
                                var row = new FUNCTIONS
                                {
                                    SN = reader["SN"] as Int16? ?? null,
                                    NAME = reader["NAME"] as string,
                                    MODE = reader["MODE"] as string,
                                    CATEGORY = reader["CATEGORY"] as string,
                                    URL = reader["URL"] as string,
                                    PARENT_SN = reader["PARENT_SN"] as Int16? ?? null,
                                    SORT = reader["SORT"] as Int16? ?? null,
                                    CDATE = reader["CDATE"] as DateTime? ?? null,
                                    CUSER = reader["CUSER"] as Int16? ?? null,
                                    MDATE = reader["MDATE"] as DateTime? ?? null,
                                    MUSER = reader["MUSER"] as Int16? ?? null
                                };
                                res.FUNCTIONS.Add(row);
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

        public FunctionRetrieveRes PaginationQuery2(FunctionRetrieveReq req)
        {
            FunctionRetrieveRes res = new FunctionRetrieveRes()
            {
                FUNCTIONS = new List<FUNCTIONS>(),
                Pagination = new Pagination
                {
                    PageCount = 0,
                    RowCount = 0,
                    PageNumber = 0,
                    MinNumber = 0,
                    MaxNumber = 0,
                    //Data = null,
                    StartTime = DateTime.Now
                }
            };

            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
SELECT TOP(@TOP) SN, NAME, MODE, CATEGORY, URL, PARENT_SN, SORT, CDATE, CUSER, MDATE, MUSER
    FROM [FUNCTION]{0};";
                string where = "";
                Db.AddInParameter(cmd, "TOP", DbType.Int32, 1000000);  //參數化

                if (req.FUNCTIONS.SN != null)
                {
                    where += " AND SN=@SN";
                    Db.AddInParameter(cmd, "SN", DbType.Int32, req.FUNCTIONS.SN);
                }
                if (req.FUNCTIONS.NAME != null)
                {
                    where += " AND NAME LIKE '%' + @NAME + '%'";
                    Db.AddInParameter(cmd, "NAME", DbType.String, req.FUNCTIONS.NAME);
                }
                if (req.FUNCTIONS.MODE != null)
                {
                    where += " AND MODE=@MODE";
                    Db.AddInParameter(cmd, "MODE", DbType.String, req.FUNCTIONS.MODE);
                }
                if (req.FUNCTIONS.CATEGORY != null)
                {
                    where += " AND CATEGORY=@CATEGORY";
                    Db.AddInParameter(cmd, "CATEGORY", DbType.String, req.FUNCTIONS.CATEGORY);
                }
                if (req.FUNCTIONS.PARENT_SN != null)
                {
                    where += " AND PARENT_SN=@PARENT_SN";
                    Db.AddInParameter(cmd, "PARENT_SN", DbType.Int32, req.FUNCTIONS.PARENT_SN);
                }
                if (req.FUNCTIONS.URL != null)
                {
                    where += " AND URL LIKE '%' + @URL + '%'";
                    Db.AddInParameter(cmd, "URL", DbType.String, req.FUNCTIONS.URL);
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
                    List<FUNCTIONS> data = new List<FUNCTIONS>();
                    res.Pagination.RowCount = 0;
                    while (reader.Read())
                    {
                        res.Pagination.RowCount++;
                        var row = new FUNCTIONS
                        {
                            ROWNUM = res.Pagination.RowCount,
                            SN = reader["SN"] as Int16? ?? null,
                            NAME = reader["NAME"] as string,
                            MODE = reader["MODE"] as string,
                            CATEGORY = reader["CATEGORY"] as string,
                            URL = reader["URL"] as string,
                            PARENT_SN = reader["PARENT_SN"] as Int16? ?? null,
                            SORT = reader["SORT"] as Int16? ?? null,
                            CDATE = reader["CDATE"] as DateTime? ?? null,
                            CUSER = reader["CUSER"] as Int16? ?? null,
                            MDATE = reader["MDATE"] as DateTime? ?? null,
                            MUSER = reader["MUSER"] as Int16? ?? null
                        };
                        data.Add(row);
                    }

                    res.Pagination.PageCount = Convert.ToInt32(Math.Ceiling(1.0 * res.Pagination.RowCount / req.PageSize));
                    res.Pagination.PageNumber = req.PageNumber < 1 ? 1 : req.PageNumber;
                    res.Pagination.PageNumber = req.PageNumber > res.Pagination.PageCount ? res.Pagination.PageCount : res.Pagination.PageNumber;
                    res.Pagination.MinNumber = (res.Pagination.PageNumber - 1) * req.PageSize + 1;
                    res.Pagination.MaxNumber = res.Pagination.PageNumber * req.PageSize;
                    res.Pagination.MaxNumber = res.Pagination.MaxNumber > res.Pagination.RowCount ? res.Pagination.RowCount : res.Pagination.MaxNumber;

                    data = (from d in data
                            where d.ROWNUM >= res.Pagination.MinNumber && d.ROWNUM <= res.Pagination.MaxNumber
                            select d).ToList();

                    res.FUNCTIONS = data;
                }
            }
            res.Pagination.EndTime = DateTime.Now;

            return res;
        }
    }
}