using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using KYL_CMS.Models.DataAccess;
using KYL_CMS.Models.EntityDefinition;
using KYL_CMS.Models.DataClass;
using KYL_CMS.Models.DataClass.Feeling;
using KYL_CMS.Models.HelpLibrary;

namespace KYL_CMS.Models.BusinessLogic
{
    public class Feeling : DatabaseAccess
    {
        public Feeling(string connectionStringName) : base(connectionStringName) 
        {}
        public FEELING ModificationQuery(int? SN)
        {
            FEELING row = null;

            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
SELECT 
    FROM FEELING
    WHERE SN=@SN
";
                Db.AddInParameter(cmd, "SN", DbType.Int32, SN);

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                using (IDataReader reader = Db.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        row = new FEELING
                        {
                        };
                    }
                }
            }

            return row;
        }

        public int DataCreate(FeelingModifyReq req)
        {
            int count = 0;
            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
INSERT FEELING ()
    VALUES () SET @SN = SCOPE_IDENTITY();
";
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;


                Db.AddOutParameter(cmd, "SN", DbType.Int32, 1);
                count = Db.ExecuteNonQuery(cmd);
                //req.FEELING.SN = Db.GetParameterValue(cmd, "SN") as Int32? ?? null;
            }
            return count;
        }

public int DataUpdate(FeelingModifyReq req)
        {
            int count = 0;
            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
UPDATE FEELING
	SET 
    WHERE SN=@SN;
";
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;

                count = Db.ExecuteNonQuery(cmd);
            }
            return count;
        }

public int DataDelete(FeelingModifyReq req)
        {
            int count = 0;
            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
DELETE FROM FEELING
    WHERE SN=@SN;
";
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                //Db.AddInParameter(cmd, "SN", DbType.String, req.FEELING.SN);
                count = Db.ExecuteNonQuery(cmd);
            }
            return count;
        }

        public FeelingRetrieveRes PaginationRetrieve(FeelingRetrieveReq req)
        {
            FeelingRetrieveRes res = new FeelingRetrieveRes()
            {
                FEELING = new List<FEELING>(),
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
                string sql = @"SELECT COUNT(1) FROM (SELECT TOP(@TOP) NULL AS N FROM FEELING{0}) A;
SELECT TOP(@TOP) 
    FROM FEELING{0}
    ORDER BY SN DESC;";
                string where = "";
                Db.AddInParameter(cmd, "TOP", DbType.Int32, 1000);


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
                                var row = new FEELING
                                {

                                };
                                res.FEELING.Add(row);
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

        public FeelingRetrieveRes ReportData(FeelingRetrieveReq req)
        {
            FeelingRetrieveRes res = new FeelingRetrieveRes()
            {
                FEELING = new List<FEELING>(),
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
                string sql = @"SELECT TOP(@TOP)

    FROM FEELING{0}
    ORDER BY SN DESC;";
                string where = "";
                Db.AddInParameter(cmd, "TOP", DbType.Int32, 1000000);

                if (where.Length > 0)
                {
                    where = " WHERE" + where.Substring(4);
                }

                sql = String.Format(sql, where);
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                using (IDataReader reader = Db.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        var row = new FEELING
                        {

                        };
                        res.FEELING.Add(row);
                    }
                }
            }
            res.Pagination.EndTime = DateTime.Now;

            return res;
        }

    }
}
