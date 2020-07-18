using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KYL_CMS.Models.DataClass.History;
using KYL_CMS.Models.DataAccess;
using System.Data;
using System.Data.Common;
using KYL_CMS.Models.EntityDefinition;
using KYL_CMS.Models.DataClass;

namespace KYL_CMS.Models.BusinessLogic
{
    public class History : DatabaseAccess
    {
        public History(string connectionStringName) : base(connectionStringName)
        { }

        public HistoryRetrieveRes PaginationRetrieve(HistoryRetrieveReq req, string UserId)
        {
            HistoryRetrieveRes res = new HistoryRetrieveRes()
            {
                INTERVIEW = new List<INTERVIEW>(),
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
                string sql = @"SELECT COUNT(1) FROM (SELECT TOP(@TOP) NULL AS N FROM INTERVIEW{0}) A;
SELECT TOP(@TOP) SN,CASE_SN,VOLUNTEER_SN,INCOMING_DATE,DURING,CASE_NO,NAME,TEL,CONTACT_TIME,TREATMENT,TREATMENT_MEMO,CASE_SOURCE,CASE_SOURCE_MEMO,GENDER,AGE,EDUCATION,CAREER,CAREER_MEMO,CITY,MARRIAGE,MARRIAGE_MEMO,PHYSIOLOGY,PHYSIOLOGY_MEMO,PSYCHOLOGY,PSYCHOLOGY_MEMO,VISITED,FAMILY,EXPERIENCE,HARASS,SOLVE_DEGREE,FEELING_MEMO,ABOUT_SELF,ABOUT_OTHERS,BEHAVIOR,ADDITION,INNER_DEMAND,INTERVENTION_MEMO,OPINION01,OPINION02,OPINION03,OPINION04,OPINION05,OPINION06,OPINION07,OPINION08,OPINION09,OPINION,CASE_STATUS,CDATE,CUSER,MDATE,MUSER
    FROM INTERVIEW{0}";
                string where = "";
                Db.AddInParameter(cmd, "TOP", DbType.Int32, 1000);

                if (!string.IsNullOrEmpty(req.INTERVIEW.NAME))
                {
                    where += " AND NAME LIKE @NAME";
                    Db.AddInParameter(cmd, "NAME", DbType.String, "%" + req.INTERVIEW.NAME + "%");
                }
              
                where += " AND VOLUNTEER_SN = @VOLUNTEER_SN";
                Db.AddInParameter(cmd, "VOLUNTEER_SN", DbType.String, UserId);

                if (where.Length > 0)
                {
                    where = " WHERE" + where.Substring(4);
                }

                //string[] orderColumn = { "SN", "TEL1", "TEL2", "TEL3", "TEL4", "TEL5", "TEL6", "NAME1", "NAME2", "NAME3", "NAME4", "NAME5", "NAME6", "TEACHER1", "TEACHER2", "TEACHER3" };
                //string[] ascDesc = { "ASC", "DESC", "" };

                //string order = "";

                //if (!string.IsNullOrEmpty(req.CASE.ORDER_BY))
                //{
                //    if (Array.IndexOf(orderColumn, req.CASE.ORDER_BY) > -1 && Array.IndexOf(ascDesc, req.CASE.ASC_DESC) > -1)
                //    {
                //        order = " ORDER BY " + req.CASE.ORDER_BY + " " + req.CASE.ASC_DESC;
                //    }
                //}

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
                                var row = new INTERVIEW
                                {
                                    SN = reader["SN"] as int? ?? null,
                                    CASE_SN = reader["CASE_SN"] as int? ?? null,
                                    VOLUNTEER_SN = reader["VOLUNTEER_SN"] as string,
                                    INCOMING_DATE = reader["INCOMING_DATE"] as DateTime?,
                                    DURING = reader["DURING"] as Int16? ?? null,
                                    CASE_NO = reader["CASE_NO"] as string,
                                    NAME = reader["NAME"] as string,
                                    TEL = reader["TEL"] as string,
                                    CONTACT_TIME = reader["CONTACT_TIME"] as string,
                                    TREATMENT = reader["TREATMENT"] as string,
                                    TREATMENT_MEMO = reader["TREATMENT_MEMO"] as string,
                                    CASE_SOURCE = reader["CASE_SOURCE"] as string,
                                    CASE_SOURCE_MEMO = reader["CASE_SOURCE_MEMO"] as string,
                                    GENDER = reader["GENDER"] as string,
                                    AGE = reader["AGE"] as string,
                                    EDUCATION = reader["EDUCATION"] as string,
                                    CAREER = reader["CAREER"] as string,
                                    CAREER_MEMO = reader["CAREER_MEMO"] as string,
                                    CITY = reader["CITY"] as string,
                                    MARRIAGE = reader["MARRIAGE"] as string,
                                    MARRIAGE_MEMO = reader["MARRIAGE_MEMO"] as string,
                                    PHYSIOLOGY = reader["PHYSIOLOGY"] as string,
                                    PHYSIOLOGY_MEMO = reader["PHYSIOLOGY_MEMO"] as string,
                                    PSYCHOLOGY = reader["PSYCHOLOGY"] as string,
                                    PSYCHOLOGY_MEMO = reader["PSYCHOLOGY_MEMO"] as string,
                                    VISITED = reader["VISITED"] as string,
                                    FAMILY = reader["FAMILY"] as string,
                                    EXPERIENCE = reader["EXPERIENCE"] as string,
                                    HARASS = reader["HARASS"] as string,
                                    SOLVE_DEGREE = reader["SOLVE_DEGREE"] as string,
                                    FEELING_MEMO = reader["FEELING_MEMO"] as string,
                                    ABOUT_SELF = reader["ABOUT_SELF"] as string,
                                    ABOUT_OTHERS = reader["ABOUT_OTHERS"] as string,
                                    BEHAVIOR = reader["BEHAVIOR"] as string,
                                    ADDITION = reader["ADDITION"] as string,
                                    INNER_DEMAND = reader["INNER_DEMAND"] as string,
                                    INTERVENTION_MEMO = reader["INTERVENTION_MEMO"] as string,
                                    OPINION01 = reader["OPINION01"] as string,
                                    OPINION02 = reader["OPINION02"] as string,
                                    OPINION03 = reader["OPINION03"] as string,
                                    OPINION04 = reader["OPINION04"] as string,
                                    OPINION05 = reader["OPINION05"] as string,
                                    OPINION06 = reader["OPINION06"] as string,
                                    OPINION07 = reader["OPINION07"] as string,
                                    OPINION08 = reader["OPINION08"] as string,
                                    OPINION09 = reader["OPINION09"] as string,
                                    OPINION = reader["OPINION"] as string,
                                    CASE_STATUS = reader["CASE_STATUS"] as string,
                                    CDATE = reader["CDATE"] as DateTime?,
                                    CUSER = reader["CUSER"] as string,
                                    MDATE = reader["MDATE"] as DateTime?,
                                    MUSER = reader["MUSER"] as string,
                                };
                                res.INTERVIEW.Add(row);
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

        public CASE ModificationQuery(int? SN,string UserId)
        {
            DbConnection conn = null;
            DbTransaction trans = null;
            DbCommand cmd = null;
            string sql = null;
            CASE master = null;
            try
            {
                conn = Db.CreateConnection();
                conn.Open();

                #region CASE_OWNER
                sql = @"
SELECT SN,NAME1,NAME2,NAME3,NAME4,NAME5,NAME6,TEL1,TEL2,TEL3,TEL4,TEL5,TEL6,YEARS_OLD,ADDRESS,TEACHER1,TEACHER2,TEACHER3,FAMILY,FAMILY_FILE,RESIDENCE,NOTE,PROBLEM,EXPERIENCE,SUGGEST,MERGE_REASON,CDATE,CUSER,MDATE,MUSER
    FROM CASE_OWNER
    WHERE SN=@SN
";
                cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                Db.AddInParameter(cmd, "SN", DbType.Int32, SN);
                using (IDataReader reader = Db.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        master = new CASE
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
                            FAMILY_FILE = reader["FAMILY_FILE"] as string,
                            RESIDENCE = reader["RESIDENCE"] as string,
                            NOTE = reader["NOTE"] as string,
                            PROBLEM = reader["PROBLEM"] as string,
                            EXPERIENCE = reader["EXPERIENCE"] as string,
                            SUGGEST = reader["SUGGEST"] as string,
                            MERGE_REASON = reader["MERGE_REASON"] as string,
                            CDATE = reader["CDATE"] as DateTime?,
                            CUSER = reader["CUSER"] as string,
                            MDATE = reader["MDATE"] as DateTime?,
                            MUSER = reader["MUSER"] as string
                        };
                    }
                }
                #endregion

                #region INTERVIEW
                List<CASE_DETAIL> detail = new List<CASE_DETAIL>();
                sql = @"
SELECT SN,CASE_SN,VOLUNTEER_SN,INCOMING_DATE,DURING,CASE_NO,NAME,TEL,CONTACT_TIME,TREATMENT,TREATMENT_MEMO,CASE_SOURCE,CASE_SOURCE_MEMO,GENDER,AGE,EDUCATION,CAREER,CAREER_MEMO,CITY,MARRIAGE,MARRIAGE_MEMO,PHYSIOLOGY,PHYSIOLOGY_MEMO,PSYCHOLOGY,PSYCHOLOGY_MEMO,VISITED,FAMILY,EXPERIENCE,HARASS,SOLVE_DEGREE,FEELING_MEMO,ABOUT_SELF,ABOUT_OTHERS,BEHAVIOR,ADDITION,INNER_DEMAND,INTERVENTION_MEMO,OPINION01,OPINION02,OPINION03,OPINION04,OPINION05,OPINION06,OPINION07,OPINION08,OPINION09,OPINION,CASE_STATUS,CDATE,CUSER,MDATE,MUSER
    FROM INTERVIEW
    WHERE CASE_SN=@CASE_SN AND VOLUNTEER_SN=@VOLUNTEER_SN;
";
                cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                Db.AddInParameter(cmd, "CASE_SN", DbType.Int32, SN);
                Db.AddInParameter(cmd, "VOLUNTEER_SN", DbType.String, UserId);

                using (IDataReader reader = Db.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        CASE_DETAIL interview = new CASE_DETAIL
                        {
                            SN = reader["SN"] as int? ?? null,
                            VOLUNTEER_SN = reader["VOLUNTEER_SN"] as string,
                            INCOMING_DATE = reader["INCOMING_DATE"] as DateTime?,
                            DURING = reader["DURING"] as Int16? ?? null,
                            CASE_NO = reader["CASE_NO"] as string,
                            NAME = reader["NAME"] as string,
                            TEL = reader["TEL"] as string,
                            CONTACT_TIME = reader["CONTACT_TIME"] as string,
                            TREATMENT = reader["TREATMENT"] as string,
                            TREATMENT_MEMO = reader["TREATMENT_MEMO"] as string,
                            CASE_SOURCE = reader["CASE_SOURCE"] as string,
                            CASE_SOURCE_MEMO = reader["CASE_SOURCE_MEMO"] as string,
                            GENDER = reader["GENDER"] as string,
                            AGE = reader["AGE"] as string,
                            EDUCATION = reader["EDUCATION"] as string,
                            CAREER = reader["CAREER"] as string,
                            CAREER_MEMO = reader["CAREER_MEMO"] as string,
                            CITY = reader["CITY"] as string,
                            MARRIAGE = reader["MARRIAGE"] as string,
                            MARRIAGE_MEMO = reader["MARRIAGE_MEMO"] as string,
                            PHYSIOLOGY = reader["PHYSIOLOGY"] as string,
                            PHYSIOLOGY_MEMO = reader["PHYSIOLOGY_MEMO"] as string,
                            PSYCHOLOGY = reader["PSYCHOLOGY"] as string,
                            PSYCHOLOGY_MEMO = reader["PSYCHOLOGY_MEMO"] as string,
                            VISITED = reader["VISITED"] as string,
                            FAMILY = reader["FAMILY"] as string,
                            EXPERIENCE = reader["EXPERIENCE"] as string,
                            HARASS = reader["HARASS"] as string,
                            SOLVE_DEGREE = reader["SOLVE_DEGREE"] as string,
                            FEELING_MEMO = reader["FEELING_MEMO"] as string,
                            ABOUT_SELF = reader["ABOUT_SELF"] as string,
                            ABOUT_OTHERS = reader["ABOUT_OTHERS"] as string,
                            BEHAVIOR = reader["BEHAVIOR"] as string,
                            ADDITION = reader["ADDITION"] as string,
                            INNER_DEMAND = reader["INNER_DEMAND"] as string,
                            INTERVENTION_MEMO = reader["INTERVENTION_MEMO"] as string,
                            OPINION01 = reader["OPINION01"] as string,
                            OPINION02 = reader["OPINION02"] as string,
                            OPINION03 = reader["OPINION03"] as string,
                            OPINION04 = reader["OPINION04"] as string,
                            OPINION05 = reader["OPINION05"] as string,
                            OPINION06 = reader["OPINION06"] as string,
                            OPINION07 = reader["OPINION07"] as string,
                            OPINION08 = reader["OPINION08"] as string,
                            OPINION09 = reader["OPINION09"] as string,
                            OPINION = reader["OPINION"] as string,
                            CASE_STATUS = reader["CASE_STATUS"] as string,
                            CDATE = reader["CDATE"] as DateTime?,
                            CUSER = reader["CUSER"] as string,
                            MDATE = reader["MDATE"] as DateTime?,
                            MUSER = reader["MUSER"] as string,
                            DataRow = "query"
                        };

                        #region SPECIAL_IDENTITY
                        List<SPECIAL_IDENTITY> SPECIAL_IDENTITY = new List<SPECIAL_IDENTITY>();
                        sql = @"
SELECT SN,INTERVIEW_SN,PHRASE_KEY,MEMO,CDATE,CUSER,MDATE,MUSER,CDATE,CUSER,MDATE,MUSER
    FROM SPECIAL_IDENTITY
    WHERE INTERVIEW_SN=@INTERVIEW_SN
";
                        cmd = conn.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;
                        Db.AddInParameter(cmd, "INTERVIEW_SN", DbType.Int32, interview.SN);
                        using (IDataReader reader2 = Db.ExecuteReader(cmd))
                        {
                            while (reader2.Read())
                            {
                                SPECIAL_IDENTITY special_identity = new SPECIAL_IDENTITY
                                {
                                    SN = reader["SN"] as int? ?? null,
                                    INTERVIEW_SN = reader2["INTERVIEW_SN"] as int? ?? null,
                                    PHRASE_KEY = reader2["PHRASE_KEY"] as string,
                                    MEMO = reader2["MEMO"] as string,
                                    CDATE = reader2["CDATE"] as DateTime?,
                                    CUSER = reader2["CUSER"] as string,
                                    MDATE = reader2["MDATE"] as DateTime?,
                                    MUSER = reader2["MUSER"] as string
                                };
                                SPECIAL_IDENTITY.Add(special_identity);
                            }
                        }
                        interview.SPECIAL_IDENTITY = SPECIAL_IDENTITY.ToArray();
                        #endregion

                        #region INTERVIEW_CLASSIFY
                        List<INTERVIEW_CLASSIFY> INTERVIEW_CLASSIFY = new List<INTERVIEW_CLASSIFY>();
                        sql = @"
SELECT SN,INTERVIEW_SN,PHRASE_KEY,MEMO,CDATE,CUSER,MDATE,MUSER,CDATE,CUSER,MDATE,MUSER
    FROM INTERVIEW_CLASSIFY
    WHERE INTERVIEW_SN=@INTERVIEW_SN
";
                        cmd = conn.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;
                        Db.AddInParameter(cmd, "INTERVIEW_SN", DbType.Int32, interview.SN);
                        using (IDataReader reader2 = Db.ExecuteReader(cmd))
                        {
                            while (reader2.Read())
                            {
                                INTERVIEW_CLASSIFY interview_classify = new INTERVIEW_CLASSIFY
                                {
                                    SN = reader["SN"] as int? ?? null,
                                    INTERVIEW_SN = reader2["INTERVIEW_SN"] as int? ?? null,
                                    PHRASE_KEY = reader2["PHRASE_KEY"] as string,
                                    MEMO = reader2["MEMO"] as string,
                                    CDATE = reader2["CDATE"] as DateTime?,
                                    CUSER = reader2["CUSER"] as string,
                                    MDATE = reader2["MDATE"] as DateTime?,
                                    MUSER = reader2["MUSER"] as string
                                };
                                INTERVIEW_CLASSIFY.Add(interview_classify);
                            }
                        }
                        interview.INTERVIEW_CLASSIFY = INTERVIEW_CLASSIFY.ToArray();
                        #endregion

                        #region FEELING
                        List<FEELING> FEELING = new List<FEELING>();
                        sql = @"
SELECT SN,INTERVIEW_SN,PHRASE_KEY,MEMO,CDATE,CUSER,MDATE,MUSER,CDATE,CUSER,MDATE,MUSER
    FROM FEELING
    WHERE INTERVIEW_SN=@INTERVIEW_SN
";
                        cmd = conn.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;
                        Db.AddInParameter(cmd, "INTERVIEW_SN", DbType.Int32, interview.SN);
                        using (IDataReader reader2 = Db.ExecuteReader(cmd))
                        {
                            while (reader2.Read())
                            {
                                FEELING feeling = new FEELING
                                {
                                    SN = reader["SN"] as int? ?? null,
                                    INTERVIEW_SN = reader2["INTERVIEW_SN"] as int? ?? null,
                                    PHRASE_KEY = reader2["PHRASE_KEY"] as string,
                                    MEMO = reader2["MEMO"] as string,
                                    CDATE = reader2["CDATE"] as DateTime?,
                                    CUSER = reader2["CUSER"] as string,
                                    MDATE = reader2["MDATE"] as DateTime?,
                                    MUSER = reader2["MUSER"] as string
                                };
                                FEELING.Add(feeling);
                            }
                        }
                        interview.FEELING = FEELING.ToArray();
                        #endregion

                        #region INTERVENTION
                        List<INTERVENTION> INTERVENTION = new List<INTERVENTION>();
                        sql = @"
SELECT SN,INTERVIEW_SN,PHRASE_KEY,MEMO,CDATE,CUSER,MDATE,MUSER,CDATE,CUSER,MDATE,MUSER
    FROM INTERVENTION
    WHERE INTERVIEW_SN=@INTERVIEW_SN
";
                        cmd = conn.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;
                        Db.AddInParameter(cmd, "INTERVIEW_SN", DbType.Int32, interview.SN);
                        using (IDataReader reader2 = Db.ExecuteReader(cmd))
                        {
                            while (reader2.Read())
                            {
                                INTERVENTION intervention = new INTERVENTION
                                {
                                    SN = reader["SN"] as int? ?? null,
                                    INTERVIEW_SN = reader2["INTERVIEW_SN"] as int? ?? null,
                                    PHRASE_KEY = reader2["PHRASE_KEY"] as string,
                                    MEMO = reader2["MEMO"] as string,
                                    CDATE = reader2["CDATE"] as DateTime?,
                                    CUSER = reader2["CUSER"] as string,
                                    MDATE = reader2["MDATE"] as DateTime?,
                                    MUSER = reader2["MUSER"] as string
                                };
                                INTERVENTION.Add(intervention);
                            }
                        }
                        interview.INTERVENTION = INTERVENTION.ToArray();
                        #endregion

                        detail.Add(interview);
                    }
                }
                #endregion

                master.CASE_DETAIL = detail.ToArray();
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
            return master;
        }
    }
}