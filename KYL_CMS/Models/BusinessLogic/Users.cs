using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using KYL_CMS.Models.DataAccess;
using KYL_CMS.Models.EntityDefinition;
using KYL_CMS.Models.DataClass;
using KYL_CMS.Models.DataClass.Users;
using KYL_CMS.Models.HelpLibrary;

namespace KYL_CMS.Models.BusinessLogic
{
    public class Users : DatabaseAccess
    {
        public Users(string connectionStringName) : base(connectionStringName)
        { }
        public USERS ModificationQuery(int? SN)
        {
            USERS row = null;

            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
SELECT SN,ID,NAME,PASSWORD,EMAIL,MODE,MEMO,CDATE,CUSER,MDATE,MUSER
    FROM USERS
    WHERE SN=@SN
";
                Db.AddInParameter(cmd, "SN", DbType.Int32, SN);

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                using (IDataReader reader = Db.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        row = new USERS
                        {
                            SN = reader["SN"] as Int16? ?? null,
                            ID = reader["ID"] as string,
                            NAME = reader["NAME"] as string,
                            PASSWORD = reader["PASSWORD"] as string,
                            EMAIL = reader["EMAIL"] as string,
                            MODE = reader["MODE"] as string,
                            MEMO = reader["MEMO"] as string,
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

        public int DataCreate(UsersModifyReq req)
        {
            int count = 0;
            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                Int64? USERS_SN = new Sequence("SCC").GetSeqBigInt("USERS");
                string sql = @"
INSERT USERS (SN,ID,NAME,PASSWORD,EMAIL,MODE,MEMO,CDATE,CUSER,MDATE,MUSER)
    VALUES (@SN,@ID,@NAME,@PASSWORD,@EMAIL,@MODE,@MEMO,GETDATE(),@CUSER,GETDATE(),@MUSER);
";
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;

                Db.AddInParameter(cmd, "SN", DbType.Int32, USERS_SN);
                Db.AddInParameter(cmd, "ID", DbType.String, req.USERS.ID);
                Db.AddInParameter(cmd, "NAME", DbType.String, req.USERS.NAME);
                Db.AddInParameter(cmd, "PASSWORD", DbType.String, req.USERS.PASSWORD);
                Db.AddInParameter(cmd, "EMAIL", DbType.String, req.USERS.EMAIL);
                Db.AddInParameter(cmd, "MODE", DbType.String, req.USERS.MODE);
                Db.AddInParameter(cmd, "MEMO", DbType.String, req.USERS.MEMO);
                Db.AddInParameter(cmd, "CUSER", DbType.String, req.USERS.CUSER);
                Db.AddInParameter(cmd, "MUSER", DbType.String, req.USERS.MUSER);

                count = Db.ExecuteNonQuery(cmd);
                req.USERS.SN = (short?)USERS_SN;
            }
            return count;
        }

        public int DataUpdate(UsersModifyReq req)
        {
            int count = 0;
            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
UPDATE USERS
	SET ID=@ID,NAME=@NAME,EMAIL=@EMAIL,MODE=@MODE,MEMO=@MEMO,MDATE=GETDATE(),MUSER=@MUSER{0}
    WHERE SN=@SN;
";
                string password = "";
                if (!string.IsNullOrEmpty(req.USERS.PASSWORD))
                { 
                    password = ",PASSWORD=@PASSWORD";
                    
                    Db.AddInParameter(cmd, "PASSWORD", DbType.String, req.USERS.PASSWORD);
                }
                sql = string.Format(sql, password);

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;

                Db.AddInParameter(cmd, "SN", DbType.String, req.USERS.SN);
                Db.AddInParameter(cmd, "ID", DbType.String, req.USERS.ID);
                Db.AddInParameter(cmd, "NAME", DbType.String, req.USERS.NAME);
                
                Db.AddInParameter(cmd, "EMAIL", DbType.String, req.USERS.EMAIL);
                Db.AddInParameter(cmd, "MODE", DbType.String, req.USERS.MODE);
                Db.AddInParameter(cmd, "MEMO", DbType.String, req.USERS.MEMO);
                Db.AddInParameter(cmd, "MUSER", DbType.String, req.USERS.MUSER);
                count = Db.ExecuteNonQuery(cmd);
            }
            return count;
        }

        public int DataReset(UsersModifyReq req, Int16 force)
        {
            int count = 0;
            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
UPDATE USERS
	SET PASSWORD=@PASSWORD, FORCE_PWD=@FORCE_PWD, MDATE=GETDATE(), MUSER=@MUSER
    WHERE SN=@SN;
";
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;

                Db.AddInParameter(cmd, "SN", DbType.String, req.USERS.SN);
                Db.AddInParameter(cmd, "PASSWORD", DbType.String, req.USERS.PASSWORD);
                Db.AddInParameter(cmd, "FORCE_PWD", DbType.Int16, force);
                Db.AddInParameter(cmd, "MUSER", DbType.String, req.USERS.MUSER);
                count = Db.ExecuteNonQuery(cmd);
            }
            return count;
        }

        public int DataDelete(UsersModifyReq req)
        {
            int count = 0;
            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
DELETE FROM USERS
    WHERE SN=@SN;
";
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                Db.AddInParameter(cmd, "SN", DbType.String, req.USERS.SN);
                count = Db.ExecuteNonQuery(cmd);
            }
            return count;
        }

        public UsersRetrieveRes PaginationRetrieve(UsersRetrieveReq req)
        {
            UsersRetrieveRes res = new UsersRetrieveRes()
            {
                USERS = new List<USERS>(),
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
                string sql = @"SELECT COUNT(1) FROM (SELECT TOP(@TOP) NULL AS N FROM USERS{0}) A;
SELECT TOP(@TOP) SN,ID,NAME,PASSWORD,EMAIL,dbo.PHRASE_NAME('mode',MODE) AS MODE,MEMO,CDATE,CUSER,MDATE,MUSER
    FROM USERS{0}
    ORDER BY SN DESC;";
                string where = "";
                Db.AddInParameter(cmd, "TOP", DbType.Int32, 1000);

                if (req.USERS.SN != null)
                {
                    where += " AND SN=@SN";
                    Db.AddInParameter(cmd, "SN", DbType.Int16, req.USERS.SN);
                }
                if (!string.IsNullOrEmpty(req.USERS.ID))
                {
                    where += " AND ID LIKE @ID";
                    Db.AddInParameter(cmd, "ID", DbType.String, "%"+req.USERS.ID + "%");
                }
                if (!string.IsNullOrEmpty(req.USERS.NAME))
                {
                    where += " AND NAME LIKE @NAME";
                    Db.AddInParameter(cmd, "NAME", DbType.String, "%" + req.USERS.NAME + "%" );
                }
                if (!string.IsNullOrEmpty(req.USERS.MODE))
                {
                    where += " AND MODE=@MODE";
                    Db.AddInParameter(cmd, "MODE", DbType.String, req.USERS.MODE);
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
                                var row = new USERS
                                {

                                    SN = reader["SN"] as Int16? ?? null,
                                    ID = reader["ID"] as string,
                                    NAME = reader["NAME"] as string,
                                    PASSWORD = reader["PASSWORD"] as string,
                                    EMAIL = reader["EMAIL"] as string,
                                    MODE = reader["MODE"] as string,
                                    MEMO = reader["MEMO"] as string,
                                    CDATE = reader["CDATE"] as DateTime?,
                                    CUSER = reader["CUSER"] as string,
                                    MDATE = reader["MDATE"] as DateTime?,
                                    MUSER = reader["MUSER"] as string
                                };
                                res.USERS.Add(row);
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

        public UsersRetrieveRes ReportData(UsersRetrieveReq req)
        {
            UsersRetrieveRes res = new UsersRetrieveRes()
            {
                USERS = new List<USERS>(),
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
SN,ID,NAME,PASSWORD,EMAIL,MODE,MEMO,CDATE,CUSER,MDATE,MUSER
    FROM USERS{0}
    ORDER BY SN DESC;";
                string where = "";
                Db.AddInParameter(cmd, "TOP", DbType.Int32, 1000000);

                //if (!string.IsNullOrEmpty(req.USERS.SN))
                //{
                //    where += " AND SN=@SN";
                //    Db.AddInParameter(cmd, "SN", DbType.String, req.USERS.SN);
                //}
                if (!string.IsNullOrEmpty(req.USERS.ID))
                {
                    where += " AND ID=@ID";
                    Db.AddInParameter(cmd, "ID", DbType.String, req.USERS.ID);
                }
                if (!string.IsNullOrEmpty(req.USERS.NAME))
                {
                    where += " AND NAME=@NAME";
                    Db.AddInParameter(cmd, "NAME", DbType.String, req.USERS.NAME);
                }
                if (!string.IsNullOrEmpty(req.USERS.PASSWORD))
                {
                    where += " AND PASSWORD=@PASSWORD";
                    Db.AddInParameter(cmd, "PASSWORD", DbType.String, req.USERS.PASSWORD);
                }
                if (!string.IsNullOrEmpty(req.USERS.EMAIL))
                {
                    where += " AND EMAIL=@EMAIL";
                    Db.AddInParameter(cmd, "EMAIL", DbType.String, req.USERS.EMAIL);
                }
                if (!string.IsNullOrEmpty(req.USERS.MODE))
                {
                    where += " AND MODE=@MODE";
                    Db.AddInParameter(cmd, "MODE", DbType.String, req.USERS.MODE);
                }
                if (!string.IsNullOrEmpty(req.USERS.MEMO))
                {
                    where += " AND MEMO=@MEMO";
                    Db.AddInParameter(cmd, "MEMO", DbType.String, req.USERS.MEMO);
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
                        var row = new USERS
                        {
                            SN = reader["SN"] as Int16? ?? null,
                            ID = reader["ID"] as string,
                            NAME = reader["NAME"] as string,
                            PASSWORD = reader["PASSWORD"] as string,
                            EMAIL = reader["EMAIL"] as string,
                            MODE = reader["MODE"] as string,
                            MEMO = reader["MEMO"] as string,
                            CDATE = reader["CDATE"] as DateTime?,
                            CUSER = reader["CUSER"] as string,
                            MDATE = reader["MDATE"] as DateTime?,
                            MUSER = reader["MUSER"] as string
                        };
                        res.USERS.Add(row);
                    }
                }
            }
            res.Pagination.EndTime = DateTime.Now;

            return res;
        }

    }
}
