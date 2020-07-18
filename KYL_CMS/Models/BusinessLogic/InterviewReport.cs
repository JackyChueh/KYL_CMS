using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using KYL_CMS.Models.DataAccess;
using KYL_CMS.Models.EntityDefinition;
using KYL_CMS.Models.DataClass;
using KYL_CMS.Models.DataClass.InterviewReport;
using KYL_CMS.Models.DataClass.Merge;
using KYL_CMS.Models.HelpLibrary;

namespace KYL_CMS.Models.BusinessLogic
{
    public class InterviewReport : DatabaseAccess
    {
        public InterviewReport(string connectionStringName) : base(connectionStringName)
        { }

        public InterviewReportRetrieveRes ReportData(InterviewReportRetrieveReq req)
        {
            InterviewReportRetrieveRes res = new InterviewReportRetrieveRes();

            DbConnection conn = null;
            DbTransaction trans = null;
            DbCommand cmd = null;
            string sql = null;
            try
            {
                conn = Db.CreateConnection();
                conn.Open();

                #region INTERVIEW
                List<CASE_DETAIL> detail = new List<CASE_DETAIL>();
                sql = @"
SELECT SN,CASE_SN,VOLUNTEER_SN,INCOMING_DATE,DURING,CASE_NO,NAME,TEL,CONTACT_TIME,TREATMENT,TREATMENT_MEMO,CASE_SOURCE,CASE_SOURCE_MEMO,GENDER,AGE,EDUCATION,CAREER,CAREER_MEMO,CITY,MARRIAGE,MARRIAGE_MEMO,PHYSIOLOGY,PHYSIOLOGY_MEMO,PSYCHOLOGY,PSYCHOLOGY_MEMO,VISITED,FAMILY,EXPERIENCE,HARASS,SOLVE_DEGREE,ABOUT_SELF,ABOUT_OTHERS,BEHAVIOR,ADDITION,INNER_DEMAND,OPINION01,OPINION02,OPINION03,OPINION04,OPINION05,OPINION06,OPINION07,OPINION08,OPINION09,OPINION,CASE_STATUS,CDATE,CUSER,MDATE,MUSER
    FROM INTERVIEW
    WHERE INCOMING_DATE>= @START AND INCOMING_DATE<@END
";
                cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                Db.AddInParameter(cmd, "START", DbType.DateTime2, req.Start);
                Db.AddInParameter(cmd, "END", DbType.DateTime2, req.End.Value.AddDays(1));

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
                            ABOUT_SELF = reader["ABOUT_SELF"] as string,
                            ABOUT_OTHERS = reader["ABOUT_OTHERS"] as string,
                            BEHAVIOR = reader["BEHAVIOR"] as string,
                            ADDITION = reader["ADDITION"] as string,
                            INNER_DEMAND = reader["INNER_DEMAND"] as string,
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

                res.CASE_DETAIL = detail;
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
            return res;
        }


    }
}