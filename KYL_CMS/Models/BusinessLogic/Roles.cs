using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using KYL_CMS.Models.DataAccess;
using KYL_CMS.Models.EntityDefinition;
using KYL_CMS.Models.DataClass;
using KYL_CMS.Models.DataClass.Roles;
using KYL_CMS.Models.HelpLibrary;

namespace KYL_CMS.Models.BusinessLogic
{
    public class Roles : DatabaseAccess
    {
        public Roles(string connectionStringName) : base(connectionStringName) 
        {}
        public ROLES ModificationQuery(int? SN)
        {
            ROLES row = null;

            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
SELECT SN,NAME,MODE,CDATE,CUSER,MDATE,MUSER
    FROM ROLES
    WHERE SN=@SN
";
                Db.AddInParameter(cmd, "SN", DbType.Int32, SN);

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                using (IDataReader reader = Db.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        row = new ROLES
                        {
                            SN = reader["SN"] as Int32? ?? null,
                            NAME = reader["NAME"] as string,
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

        public int DataCreate(RolesModifyReq req)
        {
            int count = 0;
            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
INSERT ROLES (NAME,MODE,CDATE,CUSER,MDATE,MUSER)
    VALUES (@NAME,@MODE,GETDATE(),@CUSER,GETDATE(),@MUSER) SET @SN = SCOPE_IDENTITY();
";
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;

                Db.AddInParameter(cmd, "NAME", DbType.String, req.ROLES.NAME);
                Db.AddInParameter(cmd, "MODE", DbType.String, req.ROLES.MODE);
                Db.AddInParameter(cmd, "CDATE", DbType.String, req.ROLES.CDATE);
                Db.AddInParameter(cmd, "CUSER", DbType.String, req.ROLES.CUSER);
                Db.AddInParameter(cmd, "MDATE", DbType.String, req.ROLES.MDATE);
                Db.AddInParameter(cmd, "MUSER", DbType.String, req.ROLES.MUSER);

                Db.AddOutParameter(cmd, "SN", DbType.Int32, 1);
                count = Db.ExecuteNonQuery(cmd);
                req.ROLES.SN = Db.GetParameterValue(cmd, "SN") as Int32? ?? null;
            }
            return count;
        }

public int DataUpdate(RolesModifyReq req)
        {
            int count = 0;
            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
UPDATE ROLES
	SET NAME=@NAME,MODE=@MODE,MDATE=GETDATE(),MUSER=@MUSER
    WHERE SN=@SN;
";
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;

                Db.AddInParameter(cmd, "SN", DbType.String, req.ROLES.SN);
                Db.AddInParameter(cmd, "NAME", DbType.String, req.ROLES.NAME);
                Db.AddInParameter(cmd, "MODE", DbType.String, req.ROLES.MODE);
                Db.AddInParameter(cmd, "MUSER", DbType.String, req.ROLES.MUSER);
                count = Db.ExecuteNonQuery(cmd);
            }
            return count;
        }

public int DataDelete(RolesModifyReq req)
        {
            int count = 0;
            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
DELETE FROM ROLES
    WHERE SN=@SN;
";
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                Db.AddInParameter(cmd, "SN", DbType.String, req.ROLES.SN);
                count = Db.ExecuteNonQuery(cmd);
            }
            return count;
        }

        public RolesRetrieveRes PaginationRetrieve(RolesRetrieveReq req)
        {
            RolesRetrieveRes res = new RolesRetrieveRes()
            {
                ROLES = new List<ROLES>(),
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
                string sql = @"SELECT COUNT(1) FROM (SELECT TOP(@TOP) NULL AS N FROM ROLES{0}) A;
SELECT TOP(@TOP) SN,NAME,MODE,CDATE,CUSER,MDATE,MUSER
    FROM ROLES{0}
    ORDER BY SN DESC;";
                string where = "";
                Db.AddInParameter(cmd, "TOP", DbType.Int32, 1000);

                if (req.ROLES.SN != null)
                {
                    where += " AND SN=@SN";
                    Db.AddInParameter(cmd, "SN", DbType.Int16, req.ROLES.SN);
                }
                if (!string.IsNullOrEmpty(req.ROLES.NAME))
                {
                    where += " AND NAME LIKE @NAME";
                    Db.AddInParameter(cmd, "NAME", DbType.String, "%" + req.ROLES.NAME+ "%");
                }
                if (!string.IsNullOrEmpty(req.ROLES.MODE))
                {
                    where += " AND MODE=@MODE";
                    Db.AddInParameter(cmd, "MODE", DbType.String, req.ROLES.MODE);
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
                                var row = new ROLES
                                {

                                    SN = reader["SN"] as Int32? ?? null,
                                    NAME = reader["NAME"] as string,
                                    MODE = reader["MODE"] as string,
                                    CDATE = reader["CDATE"] as DateTime?,
                                    CUSER = reader["CUSER"] as string,
                                    MDATE = reader["MDATE"] as DateTime?,
                                    MUSER = reader["MUSER"] as string
                                };
                                res.ROLES.Add(row);
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

        public RolesRetrieveRes ReportData(RolesRetrieveReq req)
        {
            RolesRetrieveRes res = new RolesRetrieveRes()
            {
                ROLES = new List<ROLES>(),
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
SN,NAME,MODE,CDATE,CUSER,MDATE,MUSER,
    FROM ROLES{0}
    ORDER BY SN DESC;";
                string where = "";
                Db.AddInParameter(cmd, "TOP", DbType.Int32, 1000000);

                //if (!string.IsNullOrEmpty(req.ROLES.SN))
                //{
                //    where += " AND SN=@SN";
                //    Db.AddInParameter(cmd, "SN", DbType.String, req.ROLES.SN);
                //}
                if (!string.IsNullOrEmpty(req.ROLES.NAME))
                {
                    where += " AND NAME=@NAME";
                    Db.AddInParameter(cmd, "NAME", DbType.String, req.ROLES.NAME);
                }
                if (!string.IsNullOrEmpty(req.ROLES.MODE))
                {
                    where += " AND MODE=@MODE";
                    Db.AddInParameter(cmd, "MODE", DbType.String, req.ROLES.MODE);
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
                        var row = new ROLES
                        {

                            SN = reader["SN"] as Int32? ?? null,
                            NAME = reader["NAME"] as string,
                            MODE = reader["MODE"] as string,
                            CDATE = reader["CDATE"] as DateTime?,
                            CUSER = reader["CUSER"] as string,
                            MDATE = reader["MDATE"] as DateTime?,
                            MUSER = reader["MUSER"] as string
                        };
                        res.ROLES.Add(row);
                    }
                }
            }
            res.Pagination.EndTime = DateTime.Now;

            return res;
        }

    }
}
