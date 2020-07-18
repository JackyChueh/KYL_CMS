using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using KYL_CMS.Models.DataAccess;
using KYL_CMS.Models.EntityDefinition;
using KYL_CMS.Models.DataClass;
using KYL_CMS.Models.DataClass.CaseOwner;
using KYL_CMS.Models.HelpLibrary;

namespace KYL_CMS.Models.BusinessLogic
{
    public class CaseOwner : DatabaseAccess
    {
        public CaseOwner(string connectionStringName) : base(connectionStringName)
        { }
        public CASE_OWNER ModificationQuery(int? SN)
        {
            CASE_OWNER row = null;

            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
SELECT SN,NAME1,NAME2,NAME3,NAME4,NAME5,NAME6,TEL1,TEL2,TEL3,TEL4,TEL5,TEL6,YEARS_OLD,ADDRESS,TEACHER1,TEACHER2,TEACHER3,FAMILY,RESIDENCE,NOTE,PROBLEM,EXPERIENCE,SUGGEST,CDATE,CUSER,MDATE,MUSER
    FROM CASE_OWNER
    WHERE SN=@SN
";
                Db.AddInParameter(cmd, "SN", DbType.Int32, SN);

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                using (IDataReader reader = Db.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        row = new CASE_OWNER
                        {
                            SN = reader["SN"] as int? ?? null,
                            NAME1 = reader["NAME1"] as string,
                            NAME2 = reader["NAME2"] as string,
                            NAME3 = reader["NAME3"] as string,
                            NAME4 = reader["NAME4"] as string,
                            NAME5 = reader["NAME5"] as string,
                            NAME6 = reader["NAME6"] as string,
                            TEL1 = reader["TEL1"] as string,
                            TEL2 = reader["TEL2"] as string,
                            TEL3 = reader["TEL3"] as string,
                            TEL4 = reader["TEL4"] as string,
                            TEL5 = reader["TEL5"] as string,
                            TEL6 = reader["TEL6"] as string,
                            YEARS_OLD = reader["YEARS_OLD"] as string,
                            ADDRESS = reader["ADDRESS"] as string,
                            FAMILY = reader["FAMILY"] as string,
                            EXPERIENCE = reader["EXPERIENCE"] as string,
                            SUGGEST = reader["SUGGEST"] as string,
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

        public int DataCreate(CaseOwnerModifyReq req)
        {
            int count = 0;
            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
INSERT CASE_OWNER (NAME1,NAME2,NAME3,NAME4,NAME5,NAME6,TEL1,TEL2,TEL3,TEL4,TEL5,TEL6,YEARS_OLD,ADDRESS,TEACHER1,TEACHER2,TEACHER3,FAMILY,RESIDENCE,NOTE,PROBLEM,EXPERIENCE,SUGGEST,CDATE,CUSER,MDATE,MUSER)
    VALUES (@NAME1,@NAME2,@NAME3,@NAME4,@NAME5,@NAME6,@TEL1,@TEL2,@TEL3,@TEL4,@TEL5,@TEL6,@YEARS_OLD,@ADDRESS,@TEACHER1,@TEACHER2,@TEACHER3,@FAMILY,@RESIDENCE,@NOTE,@PROBLEM,@EXPERIENCE,@SUGGEST,GETDATE(),@CUSER,GETDATE(),@MUSER) SET @SN = SCOPE_IDENTITY();
";
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;

                Db.AddInParameter(cmd, "NAME1", DbType.String, req.CASE_OWNER.NAME1);
                Db.AddInParameter(cmd, "NAME2", DbType.String, req.CASE_OWNER.NAME2);
                Db.AddInParameter(cmd, "NAME3", DbType.String, req.CASE_OWNER.NAME3);
                Db.AddInParameter(cmd, "NAME4", DbType.String, req.CASE_OWNER.NAME4);
                Db.AddInParameter(cmd, "NAME5", DbType.String, req.CASE_OWNER.NAME5);
                Db.AddInParameter(cmd, "NAME6", DbType.String, req.CASE_OWNER.NAME6);
                Db.AddInParameter(cmd, "TEL1", DbType.String, req.CASE_OWNER.TEL1);
                Db.AddInParameter(cmd, "TEL2", DbType.String, req.CASE_OWNER.TEL2);
                Db.AddInParameter(cmd, "TEL3", DbType.String, req.CASE_OWNER.TEL3);
                Db.AddInParameter(cmd, "TEL4", DbType.String, req.CASE_OWNER.TEL4);
                Db.AddInParameter(cmd, "TEL5", DbType.String, req.CASE_OWNER.TEL5);
                Db.AddInParameter(cmd, "TEL6", DbType.String, req.CASE_OWNER.TEL6);
                Db.AddInParameter(cmd, "YEARS_OLD", DbType.String, req.CASE_OWNER.YEARS_OLD);
                Db.AddInParameter(cmd, "ADDRESS", DbType.String, req.CASE_OWNER.ADDRESS);
                Db.AddInParameter(cmd, "TEACHER1", DbType.String, req.CASE_OWNER.TEACHER1);
                Db.AddInParameter(cmd, "TEACHER2", DbType.String, req.CASE_OWNER.TEACHER2);
                Db.AddInParameter(cmd, "TEACHER3", DbType.String, req.CASE_OWNER.TEACHER3);
                Db.AddInParameter(cmd, "FAMILY", DbType.String, req.CASE_OWNER.FAMILY);
                Db.AddInParameter(cmd, "RESIDENCE", DbType.String, req.CASE_OWNER.RESIDENCE);
                Db.AddInParameter(cmd, "NOTE", DbType.String, req.CASE_OWNER.NOTE);
                Db.AddInParameter(cmd, "PROBLEM", DbType.String, req.CASE_OWNER.PROBLEM);
                Db.AddInParameter(cmd, "EXPERIENCE", DbType.String, req.CASE_OWNER.EXPERIENCE);
                Db.AddInParameter(cmd, "SUGGEST", DbType.String, req.CASE_OWNER.SUGGEST);
                Db.AddInParameter(cmd, "CUSER", DbType.String, req.CASE_OWNER.CUSER);
                Db.AddInParameter(cmd, "MUSER", DbType.String, req.CASE_OWNER.MUSER);

                Db.AddOutParameter(cmd, "SN", DbType.Int32, 1);
                count = Db.ExecuteNonQuery(cmd);
                req.CASE_OWNER.SN = Db.GetParameterValue(cmd, "SN") as Int32? ?? null;
            }
            return count;
        }

        public int DataUpdate(CaseOwnerModifyReq req)
        {
            int count = 0;
            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
UPDATE CASE_OWNER
	SET SN=@SN,NAME1=@NAME1,NAME2=@NAME2,NAME3=@NAME3,NAME4=@NAME4,NAME5=@NAME5,NAME6=@NAME6,TEL1=@TEL1,TEL2=@TEL2,TEL3=@TEL3,TEL4=@TEL4,TEL5=@TEL5,TEL6=@TEL6,YEARS_OLD=@YEARS_OLD,ADDRESS=@ADDRESS,FAMILY=@FAMILY,EXPERIENCE=@EXPERIENCE,SUGGEST=@SUGGEST
    WHERE SN=@SN;
";
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;

                Db.AddInParameter(cmd, "SN", DbType.String, req.CASE_OWNER.SN);
                Db.AddInParameter(cmd, "NAME1", DbType.String, req.CASE_OWNER.NAME1);
                Db.AddInParameter(cmd, "NAME2", DbType.String, req.CASE_OWNER.NAME2);
                Db.AddInParameter(cmd, "NAME3", DbType.String, req.CASE_OWNER.NAME3);
                Db.AddInParameter(cmd, "NAME4", DbType.String, req.CASE_OWNER.NAME4);
                Db.AddInParameter(cmd, "NAME5", DbType.String, req.CASE_OWNER.NAME5);
                Db.AddInParameter(cmd, "NAME6", DbType.String, req.CASE_OWNER.NAME6);
                Db.AddInParameter(cmd, "TEL1", DbType.String, req.CASE_OWNER.TEL1);
                Db.AddInParameter(cmd, "TEL2", DbType.String, req.CASE_OWNER.TEL2);
                Db.AddInParameter(cmd, "TEL3", DbType.String, req.CASE_OWNER.TEL3);
                Db.AddInParameter(cmd, "TEL4", DbType.String, req.CASE_OWNER.TEL4);
                Db.AddInParameter(cmd, "TEL5", DbType.String, req.CASE_OWNER.TEL5);
                Db.AddInParameter(cmd, "TEL6", DbType.String, req.CASE_OWNER.TEL6);
                Db.AddInParameter(cmd, "YEARS_OLD", DbType.String, req.CASE_OWNER.YEARS_OLD);
                Db.AddInParameter(cmd, "ADDRESS", DbType.String, req.CASE_OWNER.ADDRESS);
                Db.AddInParameter(cmd, "FAMILY", DbType.String, req.CASE_OWNER.FAMILY);
                Db.AddInParameter(cmd, "EXPERIENCE", DbType.String, req.CASE_OWNER.EXPERIENCE);
                Db.AddInParameter(cmd, "SUGGEST", DbType.String, req.CASE_OWNER.SUGGEST);
                count = Db.ExecuteNonQuery(cmd);
            }
            return count;
        }

        public int DataDelete(CaseOwnerModifyReq req)
        {
            int count = 0;
            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
DELETE FROM CASE_OWNER
    WHERE SN=@SN;
";
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                Db.AddInParameter(cmd, "SN", DbType.String, req.CASE_OWNER.SN);
                count = Db.ExecuteNonQuery(cmd);
            }
            return count;
        }

        public CaseOwnerRetrieveRes PaginationRetrieve(CaseOwnerRetrieveReq req)
        {
            CaseOwnerRetrieveRes res = new CaseOwnerRetrieveRes()
            {
                CASE_OWNER = new List<CASE_OWNER>(),
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
                string sql = @"SELECT COUNT(1) FROM (SELECT TOP(@TOP) NULL AS N FROM CASE_OWNER{0}) A;
SELECT TOP(@TOP) SN,NAME1,NAME2,NAME3,NAME4,NAME5,NAME6,TEL1,TEL2,TEL3,TEL4,TEL5,TEL6,YEARS_OLD,ADDRESS,TEACHER1,TEACHER2,TEACHER3,FAMILY,RESIDENCE,NOTE,PROBLEM,EXPERIENCE,SUGGEST
    FROM CASE_OWNER{0}
    ORDER BY SN DESC;";
                string where = "";
                Db.AddInParameter(cmd, "TOP", DbType.Int32, 1000);


                if (req.CASE_OWNER.SN!=null)
                {
                    where += " AND SN=@SN";
                    Db.AddInParameter(cmd, "SN", DbType.String, req.CASE_OWNER.SN);
                }
                if (!string.IsNullOrEmpty(req.CASE_OWNER.NAME1))
                {
                    where += " AND (NAME1 LIKE @NAME OR NAME2 LIKE @NAME OR NAME3 LIKE @NAME OR NAME4 LIKE @NAME OR NAME5 LIKE @NAME OR NAME6 LIKE @NAME)";
                    Db.AddInParameter(cmd, "NAME", DbType.String, "%" + req.CASE_OWNER.NAME1 + "%");
                }
                if (!string.IsNullOrEmpty(req.CASE_OWNER.TEL1))
                {
                    where += " AND (TEL1 LIKE @TEL OR TEL2 LIKE @TEL OR TEL3 LIKE @TEL OR TEL4 LIKE @TEL OR TEL5 LIKE @TEL OR TEL6 LIKE @TEL)";
                    Db.AddInParameter(cmd, "TEL", DbType.String, "%" + req.CASE_OWNER.TEL1 + "%");
                }
                if (!string.IsNullOrEmpty(req.CASE_OWNER.TEACHER1))
                {
                    where += " AND (TEACHER1 LIKE @TEACHER OR TEACHER2 LIKE @TEACHER OR TEACHER3 LIKE @TEACHER)";
                    Db.AddInParameter(cmd, "TEACHER", DbType.String, "%" + req.CASE_OWNER.TEL1 + "%");
                }
                if (!string.IsNullOrEmpty(req.CASE_OWNER.YEARS_OLD))
                {
                    where += " AND YEARS_OLD LIKE @YEARS_OLD";
                    Db.AddInParameter(cmd, "YEARS_OLD", DbType.String, "%" + req.CASE_OWNER.YEARS_OLD + "%");
                }
                if (!string.IsNullOrEmpty(req.CASE_OWNER.ADDRESS))
                {
                    where += " AND ADDRESS LIKE @ADDRESS";
                    Db.AddInParameter(cmd, "ADDRESS", DbType.String, "%" + req.CASE_OWNER.ADDRESS + "%");
                }
                if (!string.IsNullOrEmpty(req.CASE_OWNER.FAMILY))
                {
                    where += " AND FAMILY LIKE @FAMILY";
                    Db.AddInParameter(cmd, "FAMILY", DbType.String, "%" + req.CASE_OWNER.FAMILY + "%");
                }
                if (!string.IsNullOrEmpty(req.CASE_OWNER.RESIDENCE))
                {
                    where += " AND RESIDENCE LIKE @RESIDENCE";
                    Db.AddInParameter(cmd, "RESIDENCE", DbType.String, "%" + req.CASE_OWNER.RESIDENCE + "%");
                }
                if (!string.IsNullOrEmpty(req.CASE_OWNER.NOTE))
                {
                    where += " AND NOTE LIKE @NOTE";
                    Db.AddInParameter(cmd, "NOTE", DbType.String, "%" + req.CASE_OWNER.NOTE + "%");
                }
                if (!string.IsNullOrEmpty(req.CASE_OWNER.PROBLEM))
                {
                    where += " AND PROBLEM LIKE @PROBLEM";
                    Db.AddInParameter(cmd, "PROBLEM", DbType.String, "%" + req.CASE_OWNER.PROBLEM + "%");
                }
                if (!string.IsNullOrEmpty(req.CASE_OWNER.EXPERIENCE))
                {
                    where += " AND EXPERIENCE LIKE @EXPERIENCE";
                    Db.AddInParameter(cmd, "EXPERIENCE", DbType.String, "%" + req.CASE_OWNER.EXPERIENCE + "%");
                }
                if (!string.IsNullOrEmpty(req.CASE_OWNER.SUGGEST))
                {
                    where += " AND SUGGEST LIKE @SUGGEST";
                    Db.AddInParameter(cmd, "SUGGEST", DbType.String, "%" + req.CASE_OWNER.SUGGEST + "%");
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
                                var row = new CASE_OWNER
                                {
                                    SN = reader["SN"] as int? ?? null,
                                    NAME1 = reader["NAME1"] as string,
                                    NAME2 = reader["NAME2"] as string,
                                    NAME3 = reader["NAME3"] as string,
                                    NAME4 = reader["NAME4"] as string,
                                    NAME5 = reader["NAME5"] as string,
                                    NAME6 = reader["NAME6"] as string,
                                    TEL1 = reader["TEL1"] as string,
                                    TEL2 = reader["TEL2"] as string,
                                    TEL3 = reader["TEL3"] as string,
                                    TEL4 = reader["TEL4"] as string,
                                    TEL5 = reader["TEL5"] as string,
                                    TEL6 = reader["TEL6"] as string,
                                    YEARS_OLD = reader["YEARS_OLD"] as string,
                                    ADDRESS = reader["ADDRESS"] as string,
                                    TEACHER1 = reader["TEACHER1"] as string,
                                    TEACHER2 = reader["TEACHER2"] as string,
                                    TEACHER3 = reader["TEACHER3"] as string,
                                    FAMILY = reader["FAMILY"] as string,
                                    RESIDENCE = reader["RESIDENCE"] as string,
                                    NOTE = reader["NOTE"] as string,
                                    PROBLEM = reader["PROBLEM"] as string,
                                    EXPERIENCE = reader["EXPERIENCE"] as string,
                                    SUGGEST = reader["SUGGEST"] as string,
                                };
                                res.CASE_OWNER.Add(row);
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

        public CaseOwnerRetrieveRes ReportData(CaseOwnerRetrieveReq req)
        {
            CaseOwnerRetrieveRes res = new CaseOwnerRetrieveRes()
            {
                CASE_OWNER = new List<CASE_OWNER>(),
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
SN,NAME1,NAME2,NAME3,NAME4,NAME5,NAME6,TEL1,TEL2,TEL3,TEL4,TEL5,TEL6,YEARS_OLD,ADDRESS,FAMILY,EXPERIENCE,SUGGEST,
    FROM CASE_OWNER{0}
    ORDER BY SN DESC;";
                string where = "";
                Db.AddInParameter(cmd, "TOP", DbType.Int32, 1000000);

                //if (!string.IsNullOrEmpty(req.CASE_OWNER.SN))
                {
                    where += " AND SN=@SN";
                    Db.AddInParameter(cmd, "SN", DbType.String, req.CASE_OWNER.SN);
                }
                if (!string.IsNullOrEmpty(req.CASE_OWNER.NAME1))
                {
                    where += " AND NAME1=@NAME1";
                    Db.AddInParameter(cmd, "NAME1", DbType.String, req.CASE_OWNER.NAME1);
                }
                if (!string.IsNullOrEmpty(req.CASE_OWNER.NAME2))
                {
                    where += " AND NAME2=@NAME2";
                    Db.AddInParameter(cmd, "NAME2", DbType.String, req.CASE_OWNER.NAME2);
                }
                if (!string.IsNullOrEmpty(req.CASE_OWNER.NAME3))
                {
                    where += " AND NAME3=@NAME3";
                    Db.AddInParameter(cmd, "NAME3", DbType.String, req.CASE_OWNER.NAME3);
                }
                if (!string.IsNullOrEmpty(req.CASE_OWNER.NAME4))
                {
                    where += " AND NAME4=@NAME4";
                    Db.AddInParameter(cmd, "NAME4", DbType.String, req.CASE_OWNER.NAME4);
                }
                if (!string.IsNullOrEmpty(req.CASE_OWNER.NAME5))
                {
                    where += " AND NAME5=@NAME5";
                    Db.AddInParameter(cmd, "NAME5", DbType.String, req.CASE_OWNER.NAME5);
                }
                if (!string.IsNullOrEmpty(req.CASE_OWNER.NAME6))
                {
                    where += " AND NAME6=@NAME6";
                    Db.AddInParameter(cmd, "NAME6", DbType.String, req.CASE_OWNER.NAME6);
                }
                if (!string.IsNullOrEmpty(req.CASE_OWNER.TEL1))
                {
                    where += " AND TEL1=@TEL1";
                    Db.AddInParameter(cmd, "TEL1", DbType.String, req.CASE_OWNER.TEL1);
                }
                if (!string.IsNullOrEmpty(req.CASE_OWNER.TEL2))
                {
                    where += " AND TEL2=@TEL2";
                    Db.AddInParameter(cmd, "TEL2", DbType.String, req.CASE_OWNER.TEL2);
                }
                if (!string.IsNullOrEmpty(req.CASE_OWNER.TEL3))
                {
                    where += " AND TEL3=@TEL3";
                    Db.AddInParameter(cmd, "TEL3", DbType.String, req.CASE_OWNER.TEL3);
                }
                if (!string.IsNullOrEmpty(req.CASE_OWNER.TEL4))
                {
                    where += " AND TEL4=@TEL4";
                    Db.AddInParameter(cmd, "TEL4", DbType.String, req.CASE_OWNER.TEL4);
                }
                if (!string.IsNullOrEmpty(req.CASE_OWNER.TEL5))
                {
                    where += " AND TEL5=@TEL5";
                    Db.AddInParameter(cmd, "TEL5", DbType.String, req.CASE_OWNER.TEL5);
                }
                if (!string.IsNullOrEmpty(req.CASE_OWNER.TEL6))
                {
                    where += " AND TEL6=@TEL6";
                    Db.AddInParameter(cmd, "TEL6", DbType.String, req.CASE_OWNER.TEL6);
                }
                if (!string.IsNullOrEmpty(req.CASE_OWNER.YEARS_OLD))
                {
                    where += " AND YEARS_OLD=@YEARS_OLD";
                    Db.AddInParameter(cmd, "YEARS_OLD", DbType.String, req.CASE_OWNER.YEARS_OLD);
                }
                if (!string.IsNullOrEmpty(req.CASE_OWNER.ADDRESS))
                {
                    where += " AND ADDRESS=@ADDRESS";
                    Db.AddInParameter(cmd, "ADDRESS", DbType.String, req.CASE_OWNER.ADDRESS);
                }
                if (!string.IsNullOrEmpty(req.CASE_OWNER.FAMILY))
                {
                    where += " AND FAMILY=@FAMILY";
                    Db.AddInParameter(cmd, "FAMILY", DbType.String, req.CASE_OWNER.FAMILY);
                }
                if (!string.IsNullOrEmpty(req.CASE_OWNER.EXPERIENCE))
                {
                    where += " AND EXPERIENCE=@EXPERIENCE";
                    Db.AddInParameter(cmd, "EXPERIENCE", DbType.String, req.CASE_OWNER.EXPERIENCE);
                }
                if (!string.IsNullOrEmpty(req.CASE_OWNER.SUGGEST))
                {
                    where += " AND SUGGEST=@SUGGEST";
                    Db.AddInParameter(cmd, "SUGGEST", DbType.String, req.CASE_OWNER.SUGGEST);
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
                        var row = new CASE_OWNER
                        {

                            //SN = reader["SN"] as string,
                            NAME1 = reader["NAME1"] as string,
                            NAME2 = reader["NAME2"] as string,
                            NAME3 = reader["NAME3"] as string,
                            NAME4 = reader["NAME4"] as string,
                            NAME5 = reader["NAME5"] as string,
                            NAME6 = reader["NAME6"] as string,
                            TEL1 = reader["TEL1"] as string,
                            TEL2 = reader["TEL2"] as string,
                            TEL3 = reader["TEL3"] as string,
                            TEL4 = reader["TEL4"] as string,
                            TEL5 = reader["TEL5"] as string,
                            TEL6 = reader["TEL6"] as string,
                            YEARS_OLD = reader["YEARS_OLD"] as string,
                            ADDRESS = reader["ADDRESS"] as string,
                            FAMILY = reader["FAMILY"] as string,
                            EXPERIENCE = reader["EXPERIENCE"] as string,
                            SUGGEST = reader["SUGGEST"] as string,
                        };
                        res.CASE_OWNER.Add(row);
                    }
                }
            }
            res.Pagination.EndTime = DateTime.Now;

            return res;
        }

    }
}
