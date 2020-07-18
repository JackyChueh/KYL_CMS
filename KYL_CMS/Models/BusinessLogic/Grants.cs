using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using KYL_CMS.Models.DataAccess;
using KYL_CMS.Models.EntityDefinition;
using KYL_CMS.Models.DataClass;
using KYL_CMS.Models.DataClass.Grants;
using KYL_CMS.Models.HelpLibrary;

namespace KYL_CMS.Models.BusinessLogic
{
    public class Grants : DatabaseAccess
    {
        public Grants(string connectionStringName) : base(connectionStringName)
        { }

        public List<GRANTS> ByUsersQuery(int? USERS_SN)
        {
            List<GRANTS> GRANTS = new List<GRANTS>();

            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
  SELECT R.SN,R.NAME,G.USERS_SN
	FROM ROLES R LEFT JOIN GRANTS G
		ON R.SN=G.ROLES_SN AND G.USERS_SN=@USERS_SN
";
                Db.AddInParameter(cmd, "USERS_SN", DbType.Int32, USERS_SN);

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                using (IDataReader reader = Db.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        GRANTS row = new GRANTS
                        {
                            ROLES_SN = reader["SN"] as Int32? ?? null,
                            ROLES_NAME = reader["NAME"] as string,
                            USERS_SN = reader["USERS_SN"] as Int32? ?? null
                        };
                        GRANTS.Add(row);
                    }
                }
            }

            return GRANTS;
        }

        public List<GRANTS> ByRolesQuery(int? ROLES_SN)
        {
            List<GRANTS> GRANTS = new List<GRANTS>();

            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
  SELECT U.SN,U.NAME,G.ROLES_SN
	FROM USERS U LEFT JOIN GRANTS G
		ON U.SN=G.USERS_SN AND G.ROLES_SN=@ROLES_SN
";
                Db.AddInParameter(cmd, "ROLES_SN", DbType.Int32, ROLES_SN);

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                using (IDataReader reader = Db.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        GRANTS row = new GRANTS
                        {
                            ROLES_SN = reader["ROLES_SN"] as Int32? ?? null,
                            USERS_SN = reader["SN"] as Int16? ?? null,
                            USERS_NAME = reader["NAME"] as string
                        };
                        GRANTS.Add(row);
                    }
                }
            }

            return GRANTS;
        }

        public int ByUsersUpdate(GrantsModifyReq req)
        {
            int count = 0;
            DbConnection conn = null;
            DbTransaction trans = null;
            DbCommand cmd = null;
            string sql = null;
            try
            {
                conn = Db.CreateConnection();
                conn.Open();
                trans = conn.BeginTransaction();

                #region 刪除              
                sql = @"
DELETE GRANTS
    WHERE USERS_SN=@USERS_SN
";
                cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                Db.AddInParameter(cmd, "USERS_SN", DbType.Int32, req.USERS_SN[0]);
                count = Db.ExecuteNonQuery(cmd);
                #endregion

                #region 新增               
                if (req.ROLES_SN != null)
                {
                    foreach (int sn in req.ROLES_SN)
                    {
                        sql = @"
INSERT GRANTS (ROLES_SN,USERS_SN,CDATE,CUSER,MDATE,MUSER)
    VALUES (@ROLES_SN,@USERS_SN,GETDATE(),@CUSER,GETDATE(),@MUSER);
";
                        cmd = conn.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;
                        Db.AddInParameter(cmd, "ROLES_SN", DbType.Int32, sn);
                        Db.AddInParameter(cmd, "USERS_SN", DbType.Int32, req.USERS_SN[0]);
                        Db.AddInParameter(cmd, "CUSER", DbType.String, req.CUSER);
                        Db.AddInParameter(cmd, "MUSER", DbType.String, req.MUSER);
                        count = Db.ExecuteNonQuery(cmd);
                    }
                }
                #endregion
                trans.Commit();
                return count;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        public int ByRolesUpdate(GrantsModifyReq req)
        {
            int count = 0;
            DbConnection conn = null;
            DbTransaction trans = null;
            DbCommand cmd = null;
            string sql = null;
            try
            {
                conn = Db.CreateConnection();
                conn.Open();
                trans = conn.BeginTransaction();

                #region 刪除              
                sql = @"
DELETE GRANTS
    WHERE ROLES_SN=@ROLES_SN
";
                cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                Db.AddInParameter(cmd, "ROLES_SN", DbType.Int32, req.ROLES_SN[0]);
                count = Db.ExecuteNonQuery(cmd);
                #endregion

                #region 新增               
                if (req.USERS_SN != null)
                {
                    foreach (int sn in req.USERS_SN)
                    {
                        sql = @"
INSERT GRANTS (ROLES_SN,USERS_SN,CDATE,CUSER,MDATE,MUSER)
    VALUES (@ROLES_SN,@USERS_SN,GETDATE(),@CUSER,GETDATE(),@MUSER);
";
                        cmd = conn.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;
                        Db.AddInParameter(cmd, "ROLES_SN", DbType.Int32, req.ROLES_SN[0]);
                        Db.AddInParameter(cmd, "USERS_SN", DbType.Int32, sn);
                        Db.AddInParameter(cmd, "CUSER", DbType.String, req.CUSER);
                        Db.AddInParameter(cmd, "MUSER", DbType.String, req.MUSER);
                        count = Db.ExecuteNonQuery(cmd);
                    }
                }
                #endregion
                trans.Commit();
                return count;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
        /*
        public int DataCreate(GrantsModifyReq req)
        {
            int count = 0;
            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
INSERT GRANTS (ROLES_SN,USERS_SN,CDATE,CUSER,MDATE,MUSER)
    VALUES (@ROLES_SN,@USERS_SN,@CDATE,@CUSER,@MDATE,@MUSER) SET @SN = SCOPE_IDENTITY();
";
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;

                Db.AddInParameter(cmd, "ROLES_SN", DbType.String, req.GRANTS.ROLES_SN);
                Db.AddInParameter(cmd, "USERS_SN", DbType.String, req.GRANTS.USERS_SN);
                Db.AddInParameter(cmd, "CDATE", DbType.String, req.GRANTS.CDATE);
                Db.AddInParameter(cmd, "CUSER", DbType.String, req.GRANTS.CUSER);
                Db.AddInParameter(cmd, "MDATE", DbType.String, req.GRANTS.MDATE);
                Db.AddInParameter(cmd, "MUSER", DbType.String, req.GRANTS.MUSER);

                Db.AddOutParameter(cmd, "SN", DbType.Int32, 1);
                count = Db.ExecuteNonQuery(cmd);
                //req.GRANTS.SN = Db.GetParameterValue(cmd, "SN") as Int32? ?? null;
            }
            return count;
        }

        public int DataUpdate(GrantsModifyReq req)
        {
            int count = 0;
            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
UPDATE GRANTS
	SET ROLES_SN=@ROLES_SN,USERS_SN=@USERS_SN,CDATE=@CDATE,CUSER=@CUSER,MDATE=@MDATE,MUSER=@MUSER
    WHERE SN=@SN;
";
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;

                Db.AddInParameter(cmd, "ROLES_SN", DbType.String, req.GRANTS.ROLES_SN);
                Db.AddInParameter(cmd, "USERS_SN", DbType.String, req.GRANTS.USERS_SN);
                Db.AddInParameter(cmd, "CDATE", DbType.String, req.GRANTS.CDATE);
                Db.AddInParameter(cmd, "CUSER", DbType.String, req.GRANTS.CUSER);
                Db.AddInParameter(cmd, "MDATE", DbType.String, req.GRANTS.MDATE);
                Db.AddInParameter(cmd, "MUSER", DbType.String, req.GRANTS.MUSER);
                count = Db.ExecuteNonQuery(cmd);
            }
            return count;
        }

        public int DataDelete(GrantsModifyReq req)
        {
            int count = 0;
            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
DELETE FROM GRANTS
    WHERE SN=@SN;
";
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                //Db.AddInParameter(cmd, "SN", DbType.String, req.GRANTS.SN);
                count = Db.ExecuteNonQuery(cmd);
            }
            return count;
        }

        public GrantsRetrieveRes PaginationRetrieve(GrantsRetrieveReq req)
        {
            GrantsRetrieveRes res = new GrantsRetrieveRes()
            {
                GRANTS = new List<GRANTS>(),
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
                string sql = @"SELECT COUNT(1) FROM (SELECT TOP(@TOP) NULL AS N FROM GRANTS{0}) A;
SELECT TOP(@TOP) ROLES_SN,USERS_SN,CDATE,CUSER,MDATE,MUSER,
    FROM GRANTS{0}
    ORDER BY SN DESC;";
                string where = "";
                Db.AddInParameter(cmd, "TOP", DbType.Int32, 1000);


                if (!string.IsNullOrEmpty(req.GRANTS.ROLES_SN))
                {
                    where += " AND ROLES_SN=@ROLES_SN";
                    Db.AddInParameter(cmd, "ROLES_SN", DbType.String, req.GRANTS.ROLES_SN);
                }
                if (!string.IsNullOrEmpty(req.GRANTS.USERS_SN))
                {
                    where += " AND USERS_SN=@USERS_SN";
                    Db.AddInParameter(cmd, "USERS_SN", DbType.String, req.GRANTS.USERS_SN);
                }
                if (!string.IsNullOrEmpty(req.GRANTS.CDATE))
                {
                    where += " AND CDATE=@CDATE";
                    Db.AddInParameter(cmd, "CDATE", DbType.String, req.GRANTS.CDATE);
                }
                if (!string.IsNullOrEmpty(req.GRANTS.CUSER))
                {
                    where += " AND CUSER=@CUSER";
                    Db.AddInParameter(cmd, "CUSER", DbType.String, req.GRANTS.CUSER);
                }
                if (!string.IsNullOrEmpty(req.GRANTS.MDATE))
                {
                    where += " AND MDATE=@MDATE";
                    Db.AddInParameter(cmd, "MDATE", DbType.String, req.GRANTS.MDATE);
                }
                if (!string.IsNullOrEmpty(req.GRANTS.MUSER))
                {
                    where += " AND MUSER=@MUSER";
                    Db.AddInParameter(cmd, "MUSER", DbType.String, req.GRANTS.MUSER);
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
                                var row = new GRANTS
                                {

                                    ROLES_SN = reader["ROLES_SN"] as string,
                                    USERS_SN = reader["USERS_SN"] as string,
                                    CDATE = reader["CDATE"] as string,
                                    CUSER = reader["CUSER"] as string,
                                    MDATE = reader["MDATE"] as string,
                                    MUSER = reader["MUSER"] as string,
                                };
                                res.GRANTS.Add(row);
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

        public GrantsRetrieveRes ReportData(GrantsRetrieveReq req)
        {
            GrantsRetrieveRes res = new GrantsRetrieveRes()
            {
                GRANTS = new List<GRANTS>(),
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
ROLES_SN,USERS_SN,CDATE,CUSER,MDATE,MUSER,
    FROM GRANTS{0}
    ORDER BY SN DESC;";
                string where = "";
                Db.AddInParameter(cmd, "TOP", DbType.Int32, 1000000);

                if (!string.IsNullOrEmpty(req.GRANTS.ROLES_SN))
                {
                    where += " AND ROLES_SN=@ROLES_SN";
                    Db.AddInParameter(cmd, "ROLES_SN", DbType.String, req.GRANTS.ROLES_SN);
                }
                if (!string.IsNullOrEmpty(req.GRANTS.USERS_SN))
                {
                    where += " AND USERS_SN=@USERS_SN";
                    Db.AddInParameter(cmd, "USERS_SN", DbType.String, req.GRANTS.USERS_SN);
                }
                if (!string.IsNullOrEmpty(req.GRANTS.CDATE))
                {
                    where += " AND CDATE=@CDATE";
                    Db.AddInParameter(cmd, "CDATE", DbType.String, req.GRANTS.CDATE);
                }
                if (!string.IsNullOrEmpty(req.GRANTS.CUSER))
                {
                    where += " AND CUSER=@CUSER";
                    Db.AddInParameter(cmd, "CUSER", DbType.String, req.GRANTS.CUSER);
                }
                if (!string.IsNullOrEmpty(req.GRANTS.MDATE))
                {
                    where += " AND MDATE=@MDATE";
                    Db.AddInParameter(cmd, "MDATE", DbType.String, req.GRANTS.MDATE);
                }
                if (!string.IsNullOrEmpty(req.GRANTS.MUSER))
                {
                    where += " AND MUSER=@MUSER";
                    Db.AddInParameter(cmd, "MUSER", DbType.String, req.GRANTS.MUSER);
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
                    while (reader.Read())
                    {
                        var row = new GRANTS
                        {

                            ROLES_SN = reader["ROLES_SN"] as string,
                            USERS_SN = reader["USERS_SN"] as string,
                            CDATE = reader["CDATE"] as string,
                            CUSER = reader["CUSER"] as string,
                            MDATE = reader["MDATE"] as string,
                            MUSER = reader["MUSER"] as string,
                        };
                        res.GRANTS.Add(row);
                    }
                }
            }
            res.Pagination.EndTime = DateTime.Now;

            return res;
        }
        */
    }
}
