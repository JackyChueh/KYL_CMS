using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using KYL_CMS.Models.DataAccess;
using KYL_CMS.Models.EntityDefinition;
using KYL_CMS.Models.DataClass;
using KYL_CMS.Models.DataClass.Case;
using KYL_CMS.Models.DataClass.Merge;
using KYL_CMS.Models.HelpLibrary;


namespace KYL_CMS.Models.BusinessLogic
{
    public class Case : DatabaseAccess
    {
        public Case(string connectionStringName) : base(connectionStringName)
        { }
        public CASE ModificationQuery(int? SN)
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
    WHERE CASE_SN=@CASE_SN
";
                cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                Db.AddInParameter(cmd, "CASE_SN", DbType.Int32, SN);
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

        public int DataCreate(CaseModifyReq req)
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
                cmd = conn.CreateCommand();

                #region CASE_OWNER
                Int64? CASE_SN = new Sequence("SCC").GetSeqBigInt("CASE_OWNER");
                sql = @"
INSERT CASE_OWNER (SN,NAME1,NAME2,NAME3,NAME4,NAME5,NAME6,TEL1,TEL2,TEL3,TEL4,TEL5,TEL6,YEARS_OLD,ADDRESS,TEACHER1,TEACHER2,TEACHER3,FAMILY,FAMILY_FILE,RESIDENCE,NOTE,PROBLEM,EXPERIENCE,SUGGEST,MERGE_REASON,CDATE,CUSER,MDATE,MUSER)
    VALUES (@SN,@NAME1,@NAME2,@NAME3,@NAME4,@NAME5,@NAME6,@TEL1,@TEL2,@TEL3,@TEL4,@TEL5,@TEL6,@YEARS_OLD,@ADDRESS,@TEACHER1,@TEACHER2,@TEACHER3,@FAMILY,@FAMILY_FILE,@RESIDENCE,@NOTE,@PROBLEM,@EXPERIENCE,@SUGGEST,@MERGE_REASON,GETDATE(),@CUSER,GETDATE(),@MUSER);
";
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;

                Db.AddInParameter(cmd, "SN", DbType.Int32, CASE_SN);
                Db.AddInParameter(cmd, "NAME1", DbType.String, req.CASE.NAME1);
                Db.AddInParameter(cmd, "NAME2", DbType.String, req.CASE.NAME2);
                Db.AddInParameter(cmd, "NAME3", DbType.String, req.CASE.NAME3);
                Db.AddInParameter(cmd, "NAME4", DbType.String, req.CASE.NAME4);
                Db.AddInParameter(cmd, "NAME5", DbType.String, req.CASE.NAME5);
                Db.AddInParameter(cmd, "NAME6", DbType.String, req.CASE.NAME6);
                Db.AddInParameter(cmd, "TEL1", DbType.String, req.CASE.TEL1);
                Db.AddInParameter(cmd, "TEL2", DbType.String, req.CASE.TEL2);
                Db.AddInParameter(cmd, "TEL3", DbType.String, req.CASE.TEL3);
                Db.AddInParameter(cmd, "TEL4", DbType.String, req.CASE.TEL4);
                Db.AddInParameter(cmd, "TEL5", DbType.String, req.CASE.TEL5);
                Db.AddInParameter(cmd, "TEL6", DbType.String, req.CASE.TEL6);
                Db.AddInParameter(cmd, "YEARS_OLD", DbType.String, req.CASE.YEARS_OLD);
                Db.AddInParameter(cmd, "ADDRESS", DbType.String, req.CASE.ADDRESS);
                Db.AddInParameter(cmd, "TEACHER1", DbType.String, req.CASE.TEACHER1);
                Db.AddInParameter(cmd, "TEACHER2", DbType.String, req.CASE.TEACHER2);
                Db.AddInParameter(cmd, "TEACHER3", DbType.String, req.CASE.TEACHER3);
                Db.AddInParameter(cmd, "FAMILY", DbType.String, req.CASE.FAMILY);
                Db.AddInParameter(cmd, "FAMILY_FILE", DbType.String, req.CASE.FAMILY_FILE);
                Db.AddInParameter(cmd, "RESIDENCE", DbType.String, req.CASE.RESIDENCE);
                Db.AddInParameter(cmd, "NOTE", DbType.String, req.CASE.NOTE);
                Db.AddInParameter(cmd, "PROBLEM", DbType.String, req.CASE.PROBLEM);
                Db.AddInParameter(cmd, "EXPERIENCE", DbType.String, req.CASE.EXPERIENCE);
                Db.AddInParameter(cmd, "SUGGEST", DbType.String, req.CASE.SUGGEST);
                Db.AddInParameter(cmd, "MERGE_REASON", DbType.String, req.CASE.MERGE_REASON);
                Db.AddInParameter(cmd, "CUSER", DbType.String, req.CASE.CUSER);
                Db.AddInParameter(cmd, "MUSER", DbType.String, req.CASE.MUSER);

                //Db.AddOutParameter(cmd, "SN", DbType.Int32, 1);   //取消使用 SET @SN = SCOPE_IDENTITY(), 因為重啟MSSQL會導致跳號, 改用自訂流水號
                count = Db.ExecuteNonQuery(cmd);
                //req.CASE.SN = Db.GetParameterValue(cmd, "SN") as Int32? ?? null;
                req.CASE.SN = (int?)CASE_SN;
                #endregion

                #region INTERVIEW
                foreach (CASE_DETAIL interview in req.CASE.CASE_DETAIL)
                {
                    Int64? INTERVIEW_SN = new Sequence("SCC").GetSeqBigInt("INTERVIEW");
                    string SeqNo = new Sequence("SCC").GetSeqNo("INTERVIEW", "DATETIME", 3);

                    sql = @"
INSERT INTERVIEW (SN,CASE_SN,VOLUNTEER_SN,INCOMING_DATE,DURING,CASE_NO,NAME,TEL,CONTACT_TIME,TREATMENT,TREATMENT_MEMO,CASE_SOURCE,CASE_SOURCE_MEMO,GENDER,AGE,EDUCATION,CAREER,CAREER_MEMO,CITY,MARRIAGE,MARRIAGE_MEMO,PHYSIOLOGY,PHYSIOLOGY_MEMO,PSYCHOLOGY,PSYCHOLOGY_MEMO,VISITED,FAMILY,EXPERIENCE,HARASS,SOLVE_DEGREE,FEELING_MEMO,ABOUT_SELF,ABOUT_OTHERS,BEHAVIOR,ADDITION,INNER_DEMAND,INTERVENTION_MEMO,OPINION01,OPINION02,OPINION03,OPINION04,OPINION05,OPINION06,OPINION07,OPINION08,OPINION09,OPINION,CASE_STATUS,CDATE,CUSER,MDATE,MUSER)
    VALUES (@SN,@CASE_SN,@VOLUNTEER_SN,@INCOMING_DATE,@DURING,@CASE_NO,@NAME,@TEL,@CONTACT_TIME,@TREATMENT,@TREATMENT_MEMO,@CASE_SOURCE,@CASE_SOURCE_MEMO,@GENDER,@AGE,@EDUCATION,@CAREER,@CAREER_MEMO,@CITY,@MARRIAGE,@MARRIAGE_MEMO,@PHYSIOLOGY,@PHYSIOLOGY_MEMO,@PSYCHOLOGY,@PSYCHOLOGY_MEMO,@VISITED,@FAMILY,@EXPERIENCE,@HARASS,@SOLVE_DEGREE,@FEELING_MEMO,@ABOUT_SELF,@ABOUT_OTHERS,@BEHAVIOR,@ADDITION,@INNER_DEMAND,@INTERVENTION_MEMO,@OPINION01,@OPINION02,@OPINION03,@OPINION04,@OPINION05,@OPINION06,@OPINION07,@OPINION08,@OPINION09,@OPINION,@CASE_STATUS,GETDATE(),@CUSER,GETDATE(),@MUSER);
";
                    cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;

                    Db.AddInParameter(cmd, "SN", DbType.Int32, INTERVIEW_SN);
                    Db.AddInParameter(cmd, "CASE_SN", DbType.Int32, req.CASE.SN);
                    Db.AddInParameter(cmd, "VOLUNTEER_SN", DbType.String, interview.VOLUNTEER_SN);
                    Db.AddInParameter(cmd, "INCOMING_DATE", DbType.DateTime2, interview.INCOMING_DATE);
                    Db.AddInParameter(cmd, "DURING", DbType.Int16, interview.DURING);
                    Db.AddInParameter(cmd, "CASE_NO", DbType.String, SeqNo);
                    Db.AddInParameter(cmd, "NAME", DbType.String, interview.NAME);
                    Db.AddInParameter(cmd, "TEL", DbType.String, interview.TEL);
                    Db.AddInParameter(cmd, "CONTACT_TIME", DbType.String, interview.CONTACT_TIME);
                    Db.AddInParameter(cmd, "TREATMENT", DbType.String, interview.TREATMENT);
                    Db.AddInParameter(cmd, "TREATMENT_MEMO", DbType.String, interview.TREATMENT_MEMO);
                    Db.AddInParameter(cmd, "CASE_SOURCE", DbType.String, interview.CASE_SOURCE);
                    Db.AddInParameter(cmd, "CASE_SOURCE_MEMO", DbType.String, interview.CASE_SOURCE_MEMO);
                    Db.AddInParameter(cmd, "GENDER", DbType.String, interview.GENDER);
                    Db.AddInParameter(cmd, "AGE", DbType.String, interview.AGE);
                    Db.AddInParameter(cmd, "EDUCATION", DbType.String, interview.EDUCATION);
                    Db.AddInParameter(cmd, "CAREER", DbType.String, interview.CAREER);
                    Db.AddInParameter(cmd, "CAREER_MEMO", DbType.String, interview.CAREER_MEMO);
                    Db.AddInParameter(cmd, "CITY", DbType.String, interview.CITY);
                    Db.AddInParameter(cmd, "MARRIAGE", DbType.String, interview.MARRIAGE);
                    Db.AddInParameter(cmd, "MARRIAGE_MEMO", DbType.String, interview.MARRIAGE_MEMO);
                    Db.AddInParameter(cmd, "PHYSIOLOGY", DbType.String, interview.PHYSIOLOGY);
                    Db.AddInParameter(cmd, "PHYSIOLOGY_MEMO", DbType.String, interview.PHYSIOLOGY_MEMO);
                    Db.AddInParameter(cmd, "PSYCHOLOGY", DbType.String, interview.PSYCHOLOGY);
                    Db.AddInParameter(cmd, "PSYCHOLOGY_MEMO", DbType.String, interview.PSYCHOLOGY_MEMO);
                    Db.AddInParameter(cmd, "VISITED", DbType.String, interview.VISITED);
                    Db.AddInParameter(cmd, "FAMILY", DbType.String, interview.FAMILY);
                    Db.AddInParameter(cmd, "EXPERIENCE", DbType.String, interview.EXPERIENCE);
                    Db.AddInParameter(cmd, "HARASS", DbType.String, interview.HARASS);
                    Db.AddInParameter(cmd, "SOLVE_DEGREE", DbType.String, interview.SOLVE_DEGREE);
                    Db.AddInParameter(cmd, "FEELING_MEMO", DbType.String, interview.FEELING_MEMO);
                    Db.AddInParameter(cmd, "ABOUT_SELF", DbType.String, interview.ABOUT_SELF);
                    Db.AddInParameter(cmd, "ABOUT_OTHERS", DbType.String, interview.ABOUT_OTHERS);
                    Db.AddInParameter(cmd, "BEHAVIOR", DbType.String, interview.BEHAVIOR);
                    Db.AddInParameter(cmd, "ADDITION", DbType.String, interview.ADDITION);
                    Db.AddInParameter(cmd, "INNER_DEMAND", DbType.String, interview.INNER_DEMAND);
                    Db.AddInParameter(cmd, "INTERVENTION_MEMO", DbType.String, interview.INTERVENTION_MEMO);
                    Db.AddInParameter(cmd, "OPINION01", DbType.String, interview.OPINION01);
                    Db.AddInParameter(cmd, "OPINION02", DbType.String, interview.OPINION02);
                    Db.AddInParameter(cmd, "OPINION03", DbType.String, interview.OPINION03);
                    Db.AddInParameter(cmd, "OPINION04", DbType.String, interview.OPINION04);
                    Db.AddInParameter(cmd, "OPINION05", DbType.String, interview.OPINION05);
                    Db.AddInParameter(cmd, "OPINION06", DbType.String, interview.OPINION06);
                    Db.AddInParameter(cmd, "OPINION07", DbType.String, interview.OPINION07);
                    Db.AddInParameter(cmd, "OPINION08", DbType.String, interview.OPINION08);
                    Db.AddInParameter(cmd, "OPINION09", DbType.String, interview.OPINION09);
                    Db.AddInParameter(cmd, "OPINION", DbType.String, interview.OPINION);
                    Db.AddInParameter(cmd, "CASE_STATUS", DbType.String, interview.CASE_STATUS);
                    Db.AddInParameter(cmd, "CUSER", DbType.String, req.CASE.CUSER);
                    Db.AddInParameter(cmd, "MUSER", DbType.String, req.CASE.MUSER);

                    count += Db.ExecuteNonQuery(cmd);
                    interview.SN = (int?)INTERVIEW_SN;

                    foreach (SPECIAL_IDENTITY special_identity in interview.SPECIAL_IDENTITY)
                    {
                        Int64? SPECIAL_IDENTITY_SN = new Sequence("SCC").GetSeqBigInt("SPECIAL_IDENTITY");
                        sql = @"
INSERT SPECIAL_IDENTITY (SN,INTERVIEW_SN,PHRASE_KEY,MEMO,CDATE,CUSER,MDATE,MUSER)
    VALUES (@SN,@INTERVIEW_SN,@PHRASE_KEY,@MEMO,GETDATE(),@CUSER,GETDATE(),@MUSER);
";
                        cmd = conn.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;

                        Db.AddInParameter(cmd, "SN", DbType.Int32, SPECIAL_IDENTITY_SN);
                        Db.AddInParameter(cmd, "INTERVIEW_SN", DbType.String, interview.SN);
                        Db.AddInParameter(cmd, "PHRASE_KEY", DbType.String, special_identity.PHRASE_KEY);
                        Db.AddInParameter(cmd, "MEMO", DbType.String, special_identity.MEMO);
                        Db.AddInParameter(cmd, "CUSER", DbType.String, req.CASE.CUSER);
                        Db.AddInParameter(cmd, "MUSER", DbType.String, req.CASE.MUSER);

                        count = Db.ExecuteNonQuery(cmd);
                        special_identity.SN = (int?)SPECIAL_IDENTITY_SN;
                    }

                    foreach (INTERVIEW_CLASSIFY interview_classify in interview.INTERVIEW_CLASSIFY)
                    {
                        Int64? INTERVIEW_CLASSIFY_SN = new Sequence("SCC").GetSeqBigInt("INTERVIEW_CLASSIFY");
                        sql = @"
INSERT INTERVIEW_CLASSIFY (SN,INTERVIEW_SN,PHRASE_KEY,MEMO,CDATE,CUSER,MDATE,MUSER)
    VALUES (@SN,@INTERVIEW_SN,@PHRASE_KEY,@MEMO,GETDATE(),@CUSER,GETDATE(),@MUSER);
";
                        cmd = conn.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;

                        Db.AddInParameter(cmd, "SN", DbType.Int32, INTERVIEW_CLASSIFY_SN);
                        Db.AddInParameter(cmd, "INTERVIEW_SN", DbType.String, interview.SN);
                        Db.AddInParameter(cmd, "PHRASE_KEY", DbType.String, interview_classify.PHRASE_KEY);
                        Db.AddInParameter(cmd, "MEMO", DbType.String, interview_classify.MEMO);
                        Db.AddInParameter(cmd, "CUSER", DbType.String, req.CASE.CUSER);
                        Db.AddInParameter(cmd, "MUSER", DbType.String, req.CASE.MUSER);

                        count = Db.ExecuteNonQuery(cmd);
                        interview_classify.SN = (int?)INTERVIEW_CLASSIFY_SN;
                    }

                    foreach (FEELING feeling in interview.FEELING)
                    {
                        Int64? FEELING_SN = new Sequence("SCC").GetSeqBigInt("FEELING");
                        sql = @"
INSERT FEELING (SN,INTERVIEW_SN,PHRASE_KEY,MEMO,CDATE,CUSER,MDATE,MUSER)
    VALUES (@SN,@INTERVIEW_SN,@PHRASE_KEY,@MEMO,GETDATE(),@CUSER,GETDATE(),@MUSER);
";
                        cmd = conn.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;

                        Db.AddInParameter(cmd, "SN", DbType.Int32, FEELING_SN);
                        Db.AddInParameter(cmd, "INTERVIEW_SN", DbType.String, interview.SN);
                        Db.AddInParameter(cmd, "PHRASE_KEY", DbType.String, feeling.PHRASE_KEY);
                        Db.AddInParameter(cmd, "MEMO", DbType.String, feeling.MEMO);
                        Db.AddInParameter(cmd, "CUSER", DbType.String, req.CASE.CUSER);
                        Db.AddInParameter(cmd, "MUSER", DbType.String, req.CASE.MUSER);

                        count = Db.ExecuteNonQuery(cmd);
                        feeling.SN = (int?)FEELING_SN;
                    }

                    foreach (INTERVENTION intervention in interview.INTERVENTION)
                    {
                        Int64? INTERVENTION_SN = new Sequence("SCC").GetSeqBigInt("INTERVENTION");
                        sql = @"
INSERT INTERVENTION (SN,INTERVIEW_SN,PHRASE_KEY,MEMO,CDATE,CUSER,MDATE,MUSER)
    VALUES (@SN,@INTERVIEW_SN,@PHRASE_KEY,@MEMO,GETDATE(),@CUSER,GETDATE(),@MUSER);
";
                        cmd = conn.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;

                        Db.AddInParameter(cmd, "SN", DbType.Int32, INTERVENTION_SN);
                        Db.AddInParameter(cmd, "INTERVIEW_SN", DbType.String, interview.SN);
                        Db.AddInParameter(cmd, "PHRASE_KEY", DbType.String, intervention.PHRASE_KEY);
                        Db.AddInParameter(cmd, "MEMO", DbType.String, intervention.MEMO);
                        Db.AddInParameter(cmd, "CUSER", DbType.String, req.CASE.CUSER);
                        Db.AddInParameter(cmd, "MUSER", DbType.String, req.CASE.MUSER);

                        count = Db.ExecuteNonQuery(cmd);
                        intervention.SN = (int?)INTERVENTION_SN;
                    }

                }
                #endregion

                trans.Commit();

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
            return count;
        }

        public int DataUpdate(CaseModifyReq req)
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

                #region CASE_OWNER
                if (!string.IsNullOrEmpty(req.CASE.FAMILY_FILE))
                {
                    sql = @"
UPDATE CASE_OWNER
	SET FAMILY_FILE=@FAMILY_FILE
    WHERE SN=@SN;
";
                    cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;
                    Db.AddInParameter(cmd, "SN", DbType.String, req.CASE.SN);
                    Db.AddInParameter(cmd, "FAMILY_FILE", DbType.String, req.CASE.FAMILY_FILE);
                    count = Db.ExecuteNonQuery(cmd);
                }
                sql = @"
UPDATE CASE_OWNER
	SET YEARS_OLD=@YEARS_OLD,ADDRESS=@ADDRESS,TEACHER1=@TEACHER1,TEACHER2=@TEACHER2,TEACHER3=@TEACHER3,NAME1=@NAME1,NAME2=@NAME2,NAME3=@NAME3,NAME4=@NAME4,NAME5=@NAME5,NAME6=@NAME6,TEL1=@TEL1,TEL2=@TEL2,TEL3=@TEL3,TEL4=@TEL4,TEL5=@TEL5,TEL6=@TEL6
        ,FAMILY=@FAMILY,RESIDENCE=@RESIDENCE,NOTE=@NOTE,PROBLEM=@PROBLEM,EXPERIENCE=@EXPERIENCE,SUGGEST=@SUGGEST,MERGE_REASON=@MERGE_REASON,MDATE=GETDATE(), MUSER=@MUSER
    WHERE SN=@SN;
";
                cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;

                Db.AddInParameter(cmd, "SN", DbType.String, req.CASE.SN);
                Db.AddInParameter(cmd, "NAME1", DbType.String, req.CASE.NAME1);
                Db.AddInParameter(cmd, "NAME2", DbType.String, req.CASE.NAME2);
                Db.AddInParameter(cmd, "NAME3", DbType.String, req.CASE.NAME3);
                Db.AddInParameter(cmd, "NAME4", DbType.String, req.CASE.NAME4);
                Db.AddInParameter(cmd, "NAME5", DbType.String, req.CASE.NAME5);
                Db.AddInParameter(cmd, "NAME6", DbType.String, req.CASE.NAME6);
                Db.AddInParameter(cmd, "TEL1", DbType.String, req.CASE.TEL1);
                Db.AddInParameter(cmd, "TEL2", DbType.String, req.CASE.TEL2);
                Db.AddInParameter(cmd, "TEL3", DbType.String, req.CASE.TEL3);
                Db.AddInParameter(cmd, "TEL4", DbType.String, req.CASE.TEL4);
                Db.AddInParameter(cmd, "TEL5", DbType.String, req.CASE.TEL5);
                Db.AddInParameter(cmd, "TEL6", DbType.String, req.CASE.TEL6);
                Db.AddInParameter(cmd, "YEARS_OLD", DbType.String, req.CASE.YEARS_OLD);
                Db.AddInParameter(cmd, "ADDRESS", DbType.String, req.CASE.ADDRESS);
                Db.AddInParameter(cmd, "TEACHER1", DbType.String, req.CASE.TEACHER1);
                Db.AddInParameter(cmd, "TEACHER2", DbType.String, req.CASE.TEACHER2);
                Db.AddInParameter(cmd, "TEACHER3", DbType.String, req.CASE.TEACHER3);
                Db.AddInParameter(cmd, "FAMILY", DbType.String, req.CASE.FAMILY);
                Db.AddInParameter(cmd, "RESIDENCE", DbType.String, req.CASE.RESIDENCE);
                Db.AddInParameter(cmd, "NOTE", DbType.String, req.CASE.NOTE);
                Db.AddInParameter(cmd, "PROBLEM", DbType.String, req.CASE.PROBLEM);
                Db.AddInParameter(cmd, "EXPERIENCE", DbType.String, req.CASE.EXPERIENCE);
                Db.AddInParameter(cmd, "SUGGEST", DbType.String, req.CASE.SUGGEST);
                Db.AddInParameter(cmd, "MERGE_REASON", DbType.String, req.CASE.MERGE_REASON);
                Db.AddInParameter(cmd, "CUSER", DbType.String, req.CASE.CUSER);
                Db.AddInParameter(cmd, "MUSER", DbType.String, req.CASE.MUSER);

                //Db.AddOutParameter(cmd, "SN", DbType.Int32, 1);
                count = Db.ExecuteNonQuery(cmd);
                //req.CASE.SN = Db.GetParameterValue(cmd, "SN") as Int32? ?? null;
                #endregion

                #region INTERVIEW

                foreach (CASE_DETAIL interview in req.CASE.CASE_DETAIL)
                {
                    if (interview.DataRow == "add")
                    {
                        Int64? INTERVIEW_SN = new Sequence("SCC").GetSeqBigInt("INTERVIEW");
                        string SeqNo = new Sequence("SCC").GetSeqNo("INTERVIEW", "DATETIME", 3);

                        sql = @"
INSERT INTERVIEW (SN,CASE_SN,VOLUNTEER_SN,INCOMING_DATE,DURING,CASE_NO,NAME,TEL,CONTACT_TIME,TREATMENT,TREATMENT_MEMO,CASE_SOURCE,CASE_SOURCE_MEMO,GENDER,AGE,EDUCATION,CAREER,CAREER_MEMO,CITY,MARRIAGE,MARRIAGE_MEMO,PHYSIOLOGY,PHYSIOLOGY_MEMO,PSYCHOLOGY,PSYCHOLOGY_MEMO,VISITED,FAMILY,EXPERIENCE,HARASS,SOLVE_DEGREE,FEELING_MEMO,ABOUT_SELF,ABOUT_OTHERS,BEHAVIOR,ADDITION,INNER_DEMAND,INTERVENTION_MEMO,OPINION01,OPINION02,OPINION03,OPINION04,OPINION05,OPINION06,OPINION07,OPINION08,OPINION09,OPINION,CASE_STATUS,CDATE,CUSER,MDATE,MUSER)
    VALUES (@SN,@CASE_SN,@VOLUNTEER_SN,@INCOMING_DATE,@DURING,@CASE_NO,@NAME,@TEL,@CONTACT_TIME,@TREATMENT,@TREATMENT_MEMO,@CASE_SOURCE,@CASE_SOURCE_MEMO,@GENDER,@AGE,@EDUCATION,@CAREER,@CAREER_MEMO,@CITY,@MARRIAGE,@MARRIAGE_MEMO,@PHYSIOLOGY,@PHYSIOLOGY_MEMO,@PSYCHOLOGY,@PSYCHOLOGY_MEMO,@VISITED,@FAMILY,@EXPERIENCE,@HARASS,@SOLVE_DEGREE,@FEELING_MEMO,@ABOUT_SELF,@ABOUT_OTHERS,@BEHAVIOR,@ADDITION,@INNER_DEMAND,@INTERVENTION_MEMO,@OPINION01,@OPINION02,@OPINION03,@OPINION04,@OPINION05,@OPINION06,@OPINION07,@OPINION08,@OPINION09,@OPINION,@CASE_STATUS,GETDATE(),@CUSER,GETDATE(),@MUSER);
";
                        cmd = conn.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;

                        Db.AddInParameter(cmd, "SN", DbType.Int32, INTERVIEW_SN);
                        Db.AddInParameter(cmd, "CASE_SN", DbType.Int32, req.CASE.SN);
                        Db.AddInParameter(cmd, "VOLUNTEER_SN", DbType.String, interview.VOLUNTEER_SN);
                        Db.AddInParameter(cmd, "INCOMING_DATE", DbType.DateTime2, interview.INCOMING_DATE);
                        Db.AddInParameter(cmd, "DURING", DbType.Int16, interview.DURING);
                        Db.AddInParameter(cmd, "CASE_NO", DbType.String, SeqNo);
                        Db.AddInParameter(cmd, "NAME", DbType.String, interview.NAME);
                        Db.AddInParameter(cmd, "TEL", DbType.String, interview.TEL);
                        Db.AddInParameter(cmd, "CONTACT_TIME", DbType.String, interview.CONTACT_TIME);
                        Db.AddInParameter(cmd, "TREATMENT", DbType.String, interview.TREATMENT);
                        Db.AddInParameter(cmd, "TREATMENT_MEMO", DbType.String, interview.TREATMENT_MEMO);
                        Db.AddInParameter(cmd, "CASE_SOURCE", DbType.String, interview.CASE_SOURCE);
                        Db.AddInParameter(cmd, "CASE_SOURCE_MEMO", DbType.String, interview.CASE_SOURCE_MEMO);
                        Db.AddInParameter(cmd, "GENDER", DbType.String, interview.GENDER);
                        Db.AddInParameter(cmd, "AGE", DbType.String, interview.AGE);
                        Db.AddInParameter(cmd, "EDUCATION", DbType.String, interview.EDUCATION);
                        Db.AddInParameter(cmd, "CAREER", DbType.String, interview.CAREER);
                        Db.AddInParameter(cmd, "CAREER_MEMO", DbType.String, interview.CAREER_MEMO);
                        Db.AddInParameter(cmd, "CITY", DbType.String, interview.CITY);
                        Db.AddInParameter(cmd, "MARRIAGE", DbType.String, interview.MARRIAGE);
                        Db.AddInParameter(cmd, "MARRIAGE_MEMO", DbType.String, interview.MARRIAGE_MEMO);
                        Db.AddInParameter(cmd, "PHYSIOLOGY", DbType.String, interview.PHYSIOLOGY);
                        Db.AddInParameter(cmd, "PHYSIOLOGY_MEMO", DbType.String, interview.PHYSIOLOGY_MEMO);
                        Db.AddInParameter(cmd, "PSYCHOLOGY", DbType.String, interview.PSYCHOLOGY);
                        Db.AddInParameter(cmd, "PSYCHOLOGY_MEMO", DbType.String, interview.PSYCHOLOGY_MEMO);
                        Db.AddInParameter(cmd, "VISITED", DbType.String, interview.VISITED);
                        Db.AddInParameter(cmd, "FAMILY", DbType.String, interview.FAMILY);
                        Db.AddInParameter(cmd, "EXPERIENCE", DbType.String, interview.EXPERIENCE);
                        Db.AddInParameter(cmd, "HARASS", DbType.String, interview.HARASS);
                        Db.AddInParameter(cmd, "SOLVE_DEGREE", DbType.String, interview.SOLVE_DEGREE);
                        Db.AddInParameter(cmd, "FEELING_MEMO", DbType.String, interview.FEELING_MEMO);
                        Db.AddInParameter(cmd, "ABOUT_SELF", DbType.String, interview.ABOUT_SELF);
                        Db.AddInParameter(cmd, "ABOUT_OTHERS", DbType.String, interview.ABOUT_OTHERS);
                        Db.AddInParameter(cmd, "BEHAVIOR", DbType.String, interview.BEHAVIOR);
                        Db.AddInParameter(cmd, "ADDITION", DbType.String, interview.ADDITION);
                        Db.AddInParameter(cmd, "INNER_DEMAND", DbType.String, interview.INNER_DEMAND);
                        Db.AddInParameter(cmd, "INTERVENTION_MEMO", DbType.String, interview.INTERVENTION_MEMO);
                        Db.AddInParameter(cmd, "OPINION01", DbType.String, interview.OPINION01);
                        Db.AddInParameter(cmd, "OPINION02", DbType.String, interview.OPINION02);
                        Db.AddInParameter(cmd, "OPINION03", DbType.String, interview.OPINION03);
                        Db.AddInParameter(cmd, "OPINION04", DbType.String, interview.OPINION04);
                        Db.AddInParameter(cmd, "OPINION05", DbType.String, interview.OPINION05);
                        Db.AddInParameter(cmd, "OPINION06", DbType.String, interview.OPINION06);
                        Db.AddInParameter(cmd, "OPINION07", DbType.String, interview.OPINION07);
                        Db.AddInParameter(cmd, "OPINION08", DbType.String, interview.OPINION08);
                        Db.AddInParameter(cmd, "OPINION09", DbType.String, interview.OPINION09);
                        Db.AddInParameter(cmd, "OPINION", DbType.String, interview.OPINION);
                        Db.AddInParameter(cmd, "CASE_STATUS", DbType.String, interview.CASE_STATUS);
                        Db.AddInParameter(cmd, "CUSER", DbType.String, req.CASE.CUSER);
                        Db.AddInParameter(cmd, "MUSER", DbType.String, req.CASE.MUSER);

                        count += Db.ExecuteNonQuery(cmd);
                        interview.SN = (int?)INTERVIEW_SN;
                    }
                    else if (interview.DataRow == "edit")
                    {
                        sql = @"
UPDATE INTERVIEW 
    SET CASE_SN=@CASE_SN, VOLUNTEER_SN=@VOLUNTEER_SN, INCOMING_DATE=@INCOMING_DATE, DURING=@DURING, CASE_NO=@CASE_NO, NAME=@NAME, TEL=@TEL, CONTACT_TIME=@CONTACT_TIME, TREATMENT=@TREATMENT, TREATMENT_MEMO=@TREATMENT_MEMO, CASE_SOURCE=@CASE_SOURCE, CASE_SOURCE_MEMO=@CASE_SOURCE_MEMO, GENDER=@GENDER, AGE=@AGE, EDUCATION=@EDUCATION, CAREER=@CAREER, CAREER_MEMO=@CAREER_MEMO, CITY=@CITY, MARRIAGE=@MARRIAGE, MARRIAGE_MEMO=@MARRIAGE_MEMO, PHYSIOLOGY=@PHYSIOLOGY, PHYSIOLOGY_MEMO=@PHYSIOLOGY_MEMO, PSYCHOLOGY=@PSYCHOLOGY, PSYCHOLOGY_MEMO=@PSYCHOLOGY_MEMO, VISITED=@VISITED, FAMILY=@FAMILY, EXPERIENCE=@EXPERIENCE, HARASS=@HARASS, SOLVE_DEGREE=@SOLVE_DEGREE, FEELING_MEMO=@FEELING_MEMO, ABOUT_SELF=@ABOUT_SELF, ABOUT_OTHERS=@ABOUT_OTHERS, BEHAVIOR=@BEHAVIOR, ADDITION=@ADDITION, INNER_DEMAND=@INNER_DEMAND, INTERVENTION_MEMO=@INTERVENTION_MEMO, OPINION01=@OPINION01, OPINION02=@OPINION02, OPINION03=@OPINION03, OPINION04=@OPINION04, OPINION05=@OPINION05, OPINION06=@OPINION06, OPINION07=@OPINION07, OPINION08=@OPINION08, OPINION09=@OPINION09, OPINION=@OPINION, CASE_STATUS=@CASE_STATUS, MDATE=GETDATE(), MUSER=@MUSER
    WHERE SN=@SN;
";
                        cmd = conn.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;

                        Db.AddInParameter(cmd, "SN", DbType.Int32, interview.SN);
                        Db.AddInParameter(cmd, "CASE_SN", DbType.Int32, req.CASE.SN);
                        Db.AddInParameter(cmd, "VOLUNTEER_SN", DbType.String, interview.VOLUNTEER_SN);
                        Db.AddInParameter(cmd, "INCOMING_DATE", DbType.DateTime2, interview.INCOMING_DATE);
                        Db.AddInParameter(cmd, "DURING", DbType.Int16, interview.DURING);
                        Db.AddInParameter(cmd, "CASE_NO", DbType.String, interview.CASE_NO);
                        Db.AddInParameter(cmd, "NAME", DbType.String, interview.NAME);
                        Db.AddInParameter(cmd, "TEL", DbType.String, interview.TEL);
                        Db.AddInParameter(cmd, "CONTACT_TIME", DbType.String, interview.CONTACT_TIME);
                        Db.AddInParameter(cmd, "TREATMENT", DbType.String, interview.TREATMENT);
                        Db.AddInParameter(cmd, "TREATMENT_MEMO", DbType.String, interview.TREATMENT_MEMO);
                        Db.AddInParameter(cmd, "CASE_SOURCE", DbType.String, interview.CASE_SOURCE);
                        Db.AddInParameter(cmd, "CASE_SOURCE_MEMO", DbType.String, interview.CASE_SOURCE_MEMO);
                        Db.AddInParameter(cmd, "GENDER", DbType.String, interview.GENDER);
                        Db.AddInParameter(cmd, "AGE", DbType.String, interview.AGE);
                        Db.AddInParameter(cmd, "EDUCATION", DbType.String, interview.EDUCATION);
                        Db.AddInParameter(cmd, "CAREER", DbType.String, interview.CAREER);
                        Db.AddInParameter(cmd, "CAREER_MEMO", DbType.String, interview.CAREER_MEMO);
                        Db.AddInParameter(cmd, "CITY", DbType.String, interview.CITY);
                        Db.AddInParameter(cmd, "MARRIAGE", DbType.String, interview.MARRIAGE);
                        Db.AddInParameter(cmd, "MARRIAGE_MEMO", DbType.String, interview.MARRIAGE_MEMO);
                        Db.AddInParameter(cmd, "PHYSIOLOGY", DbType.String, interview.PHYSIOLOGY);
                        Db.AddInParameter(cmd, "PHYSIOLOGY_MEMO", DbType.String, interview.PHYSIOLOGY_MEMO);
                        Db.AddInParameter(cmd, "PSYCHOLOGY", DbType.String, interview.PSYCHOLOGY);
                        Db.AddInParameter(cmd, "PSYCHOLOGY_MEMO", DbType.String, interview.PSYCHOLOGY_MEMO);
                        Db.AddInParameter(cmd, "VISITED", DbType.String, interview.VISITED);
                        Db.AddInParameter(cmd, "FAMILY", DbType.String, interview.FAMILY);
                        Db.AddInParameter(cmd, "EXPERIENCE", DbType.String, interview.EXPERIENCE);
                        Db.AddInParameter(cmd, "HARASS", DbType.String, interview.HARASS);
                        Db.AddInParameter(cmd, "SOLVE_DEGREE", DbType.String, interview.SOLVE_DEGREE);
                        Db.AddInParameter(cmd, "FEELING_MEMO", DbType.String, interview.FEELING_MEMO);
                        Db.AddInParameter(cmd, "ABOUT_SELF", DbType.String, interview.ABOUT_SELF);
                        Db.AddInParameter(cmd, "ABOUT_OTHERS", DbType.String, interview.ABOUT_OTHERS);
                        Db.AddInParameter(cmd, "BEHAVIOR", DbType.String, interview.BEHAVIOR);
                        Db.AddInParameter(cmd, "ADDITION", DbType.String, interview.ADDITION);
                        Db.AddInParameter(cmd, "INNER_DEMAND", DbType.String, interview.INNER_DEMAND);
                        Db.AddInParameter(cmd, "INTERVENTION_MEMO", DbType.String, interview.INTERVENTION_MEMO);
                        Db.AddInParameter(cmd, "OPINION01", DbType.String, interview.OPINION01);
                        Db.AddInParameter(cmd, "OPINION02", DbType.String, interview.OPINION02);
                        Db.AddInParameter(cmd, "OPINION03", DbType.String, interview.OPINION03);
                        Db.AddInParameter(cmd, "OPINION04", DbType.String, interview.OPINION04);
                        Db.AddInParameter(cmd, "OPINION05", DbType.String, interview.OPINION05);
                        Db.AddInParameter(cmd, "OPINION06", DbType.String, interview.OPINION06);
                        Db.AddInParameter(cmd, "OPINION07", DbType.String, interview.OPINION07);
                        Db.AddInParameter(cmd, "OPINION08", DbType.String, interview.OPINION08);
                        Db.AddInParameter(cmd, "OPINION09", DbType.String, interview.OPINION09);
                        Db.AddInParameter(cmd, "OPINION", DbType.String, interview.OPINION);
                        Db.AddInParameter(cmd, "CASE_STATUS", DbType.String, interview.CASE_STATUS);
                        Db.AddInParameter(cmd, "MUSER", DbType.String, req.CASE.MUSER);

                        //Db.AddOutParameter(cmd, "SN", DbType.Int32, 1);
                        count += Db.ExecuteNonQuery(cmd);
                        //interview.SN = Db.GetParameterValue(cmd, "SN") as Int32? ?? null;
                    }
                    else if (interview.DataRow == "delete")
                    {
                        sql = @"
DELETE INTERVIEW 
    WHERE SN=@SN;
";
                        cmd = conn.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;
                        Db.AddInParameter(cmd, "SN", DbType.Int32, interview.SN);
                        count += Db.ExecuteNonQuery(cmd);
                    }

                    if (interview.DataRow == "add" || interview.DataRow == "edit" || interview.DataRow == "delete")
                    {
                        sql = @"
DELETE FROM SPECIAL_IDENTITY
    WHERE INTERVIEW_SN=@INTERVIEW_SN;
";
                        cmd = conn.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;
                        Db.AddInParameter(cmd, "INTERVIEW_SN", DbType.String, interview.SN);
                        count = Db.ExecuteNonQuery(cmd);

                        sql = @"
DELETE FROM INTERVIEW_CLASSIFY
    WHERE INTERVIEW_SN=@INTERVIEW_SN;
";
                        cmd = conn.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;
                        Db.AddInParameter(cmd, "INTERVIEW_SN", DbType.String, interview.SN);
                        count = Db.ExecuteNonQuery(cmd);

                        sql = @"
DELETE FROM FEELING
    WHERE INTERVIEW_SN=@INTERVIEW_SN;
";
                        cmd = conn.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;
                        Db.AddInParameter(cmd, "INTERVIEW_SN", DbType.String, interview.SN);
                        count = Db.ExecuteNonQuery(cmd);

                        sql = @"
DELETE FROM INTERVENTION
    WHERE INTERVIEW_SN=@INTERVIEW_SN;
";
                        cmd = conn.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;
                        Db.AddInParameter(cmd, "INTERVIEW_SN", DbType.String, interview.SN);
                        count = Db.ExecuteNonQuery(cmd);
                    }

                    if (interview.DataRow == "add" || interview.DataRow == "edit")
                    {
                        foreach (SPECIAL_IDENTITY special_identity in interview.SPECIAL_IDENTITY)
                        {
                            Int64? SPECIAL_IDENTITY_SN = new Sequence("SCC").GetSeqBigInt("SPECIAL_IDENTITY");
                            sql = @"
INSERT SPECIAL_IDENTITY (SN,INTERVIEW_SN,PHRASE_KEY,MEMO,CDATE,CUSER,MDATE,MUSER)
    VALUES (@SN,@INTERVIEW_SN,@PHRASE_KEY,@MEMO,GETDATE(),@CUSER,GETDATE(),@MUSER);
";
                            cmd = conn.CreateCommand();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = sql;

                            Db.AddInParameter(cmd, "SN", DbType.Int32, SPECIAL_IDENTITY_SN);
                            Db.AddInParameter(cmd, "INTERVIEW_SN", DbType.String, interview.SN);
                            Db.AddInParameter(cmd, "PHRASE_KEY", DbType.String, special_identity.PHRASE_KEY);
                            Db.AddInParameter(cmd, "MEMO", DbType.String, special_identity.MEMO);
                            Db.AddInParameter(cmd, "CUSER", DbType.String, req.CASE.CUSER);
                            Db.AddInParameter(cmd, "MUSER", DbType.String, req.CASE.MUSER);

                            count = Db.ExecuteNonQuery(cmd);
                            special_identity.SN = (int?)SPECIAL_IDENTITY_SN;
                        }

                        foreach (INTERVIEW_CLASSIFY interview_classify in interview.INTERVIEW_CLASSIFY)
                        {
                            Int64? INTERVIEW_CLASSIFY_SN = new Sequence("SCC").GetSeqBigInt("INTERVIEW_CLASSIFY");
                            sql = @"
INSERT INTERVIEW_CLASSIFY (SN,INTERVIEW_SN,PHRASE_KEY,MEMO,CDATE,CUSER,MDATE,MUSER)
    VALUES (@SN,@INTERVIEW_SN,@PHRASE_KEY,@MEMO,GETDATE(),@CUSER,GETDATE(),@MUSER);
";
                            cmd = conn.CreateCommand();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = sql;

                            Db.AddInParameter(cmd, "SN", DbType.Int32, INTERVIEW_CLASSIFY_SN);
                            Db.AddInParameter(cmd, "INTERVIEW_SN", DbType.String, interview.SN);
                            Db.AddInParameter(cmd, "PHRASE_KEY", DbType.String, interview_classify.PHRASE_KEY);
                            Db.AddInParameter(cmd, "MEMO", DbType.String, interview_classify.MEMO);
                            Db.AddInParameter(cmd, "CUSER", DbType.String, req.CASE.CUSER);
                            Db.AddInParameter(cmd, "MUSER", DbType.String, req.CASE.MUSER);

                            count = Db.ExecuteNonQuery(cmd);
                            interview_classify.SN = (int?)INTERVIEW_CLASSIFY_SN;
                        }

                        foreach (FEELING feeling in interview.FEELING)
                        {
                            Int64? FEELING_SN = new Sequence("SCC").GetSeqBigInt("FEELING");
                            sql = @"
INSERT FEELING (SN,INTERVIEW_SN,PHRASE_KEY,MEMO,CDATE,CUSER,MDATE,MUSER)
    VALUES (@SN,@INTERVIEW_SN,@PHRASE_KEY,@MEMO,GETDATE(),@CUSER,GETDATE(),@MUSER);
";
                            cmd = conn.CreateCommand();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = sql;

                            Db.AddInParameter(cmd, "SN", DbType.Int32, FEELING_SN);
                            Db.AddInParameter(cmd, "INTERVIEW_SN", DbType.String, interview.SN);
                            Db.AddInParameter(cmd, "PHRASE_KEY", DbType.String, feeling.PHRASE_KEY);
                            Db.AddInParameter(cmd, "MEMO", DbType.String, feeling.MEMO);
                            Db.AddInParameter(cmd, "CUSER", DbType.String, req.CASE.CUSER);
                            Db.AddInParameter(cmd, "MUSER", DbType.String, req.CASE.MUSER);

                            count = Db.ExecuteNonQuery(cmd);
                            feeling.SN = (int?)FEELING_SN;
                        }

                        foreach (INTERVENTION intervention in interview.INTERVENTION)
                        {
                            Int64? INTERVENTION_SN = new Sequence("SCC").GetSeqBigInt("INTERVENTION");
                            sql = @"
INSERT INTERVENTION (SN,INTERVIEW_SN,PHRASE_KEY,MEMO,CDATE,CUSER,MDATE,MUSER)
    VALUES (@SN,@INTERVIEW_SN,@PHRASE_KEY,@MEMO,GETDATE(),@CUSER,GETDATE(),@MUSER);
";
                            cmd = conn.CreateCommand();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = sql;

                            Db.AddInParameter(cmd, "SN", DbType.Int32, INTERVENTION_SN);
                            Db.AddInParameter(cmd, "INTERVIEW_SN", DbType.String, interview.SN);
                            Db.AddInParameter(cmd, "PHRASE_KEY", DbType.String, intervention.PHRASE_KEY);
                            Db.AddInParameter(cmd, "MEMO", DbType.String, intervention.MEMO);
                            Db.AddInParameter(cmd, "CUSER", DbType.String, req.CASE.CUSER);
                            Db.AddInParameter(cmd, "MUSER", DbType.String, req.CASE.MUSER);

                            count = Db.ExecuteNonQuery(cmd);
                            intervention.SN = (int?)INTERVENTION_SN;
                        }
                    }

                }
                #endregion

                trans.Commit();

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
            return count;
        }

        public int DataDelete(CaseModifyReq req)
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

                #region CASE_OWNER
                sql = @"
DELETE CASE_OWNER
    WHERE SN=@SN;
";
                cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;

                Db.AddInParameter(cmd, "SN", DbType.String, req.CASE.SN);
                count = Db.ExecuteNonQuery(cmd);
                #endregion

                #region INTERVIEW

                foreach (CASE_DETAIL interview in req.CASE.CASE_DETAIL)
                {
                    if (interview.DataRow == "query" ||  interview.DataRow == "edit" || interview.DataRow == "delete")
                    {
                        sql = @"
DELETE INTERVIEW 
    WHERE SN=@SN;
";
                        cmd = conn.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;

                        Db.AddInParameter(cmd, "SN", DbType.Int32, interview.SN);
                        count += Db.ExecuteNonQuery(cmd);

                        sql = @"
DELETE FROM SPECIAL_IDENTITY
    WHERE INTERVIEW_SN=@INTERVIEW_SN;
";
                        cmd = conn.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;
                        Db.AddInParameter(cmd, "INTERVIEW_SN", DbType.String, interview.SN);
                        count = Db.ExecuteNonQuery(cmd);

                        sql = @"
DELETE FROM INTERVIEW_CLASSIFY
    WHERE INTERVIEW_SN=@INTERVIEW_SN;
";
                        cmd = conn.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;
                        Db.AddInParameter(cmd, "INTERVIEW_SN", DbType.String, interview.SN);
                        count = Db.ExecuteNonQuery(cmd);

                        sql = @"
DELETE FROM FEELING
    WHERE INTERVIEW_SN=@INTERVIEW_SN;
";
                        cmd = conn.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;
                        Db.AddInParameter(cmd, "INTERVIEW_SN", DbType.String, interview.SN);
                        count = Db.ExecuteNonQuery(cmd);

                        sql = @"
DELETE FROM INTERVENTION
    WHERE INTERVIEW_SN=@INTERVIEW_SN;
";
                        cmd = conn.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;
                        Db.AddInParameter(cmd, "INTERVIEW_SN", DbType.String, interview.SN);
                        count = Db.ExecuteNonQuery(cmd);
                    }
                }
                #endregion

                trans.Commit();

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
            return count;
        }

        public CaseRetrieveRes PaginationRetrieve(CaseRetrieveReq req)
        {
            CaseRetrieveRes res = new CaseRetrieveRes()
            {
                CASE = new List<CASE>(),
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
SELECT TOP(@TOP) SN,NAME1,NAME2,NAME3,NAME4,NAME5,NAME6,TEL1,TEL2,TEL3,TEL4,TEL5,TEL6,YEARS_OLD,ADDRESS,TEACHER1,TEACHER2,TEACHER3,FAMILY,FAMILY_FILE,RESIDENCE,NOTE,PROBLEM,EXPERIENCE,SUGGEST,MERGE_REASON
    FROM CASE_OWNER{0}{1};";
                string where = "";
                Db.AddInParameter(cmd, "TOP", DbType.Int32, 1000);


                if (req.CASE.SN != null)
                {
                    where += " AND SN=@SN";
                    Db.AddInParameter(cmd, "SN", DbType.String, req.CASE.SN);
                }
                if (!string.IsNullOrEmpty(req.CASE.NAME1))
                {
                    where += " AND (NAME1 LIKE @NAME OR NAME2 LIKE @NAME OR NAME3 LIKE @NAME OR NAME4 LIKE @NAME OR NAME5 LIKE @NAME OR NAME6 LIKE @NAME)";
                    Db.AddInParameter(cmd, "NAME", DbType.String, "%" + req.CASE.NAME1 + "%");
                }
                if (!string.IsNullOrEmpty(req.CASE.TEL1))
                {
                    where += " AND (TEL1 LIKE @TEL OR TEL2 LIKE @TEL OR TEL3 LIKE @TEL OR TEL4 LIKE @TEL OR TEL5 LIKE @TEL OR TEL6 LIKE @TEL)";
                    Db.AddInParameter(cmd, "TEL", DbType.String, "%" + req.CASE.TEL1 + "%");
                }
                if (!string.IsNullOrEmpty(req.CASE.TEACHER1))
                {
                    where += " AND (TEACHER1 LIKE @TEACHER OR TEACHER2 LIKE @TEACHER OR TEACHER3 LIKE @TEACHER)";
                    Db.AddInParameter(cmd, "TEACHER", DbType.String, "%" + req.CASE.TEACHER1 + "%");
                }
                if (!string.IsNullOrEmpty(req.CASE.YEARS_OLD))
                {
                    where += " AND YEARS_OLD LIKE @YEARS_OLD";
                    Db.AddInParameter(cmd, "YEARS_OLD", DbType.String, "%" + req.CASE.YEARS_OLD + "%");
                }
                if (!string.IsNullOrEmpty(req.CASE.ADDRESS))
                {
                    where += " AND ADDRESS LIKE @ADDRESS";
                    Db.AddInParameter(cmd, "ADDRESS", DbType.String, "%" + req.CASE.ADDRESS + "%");
                }
                if (!string.IsNullOrEmpty(req.CASE.FAMILY))
                {
                    where += " AND FAMILY LIKE @FAMILY";
                    Db.AddInParameter(cmd, "FAMILY", DbType.String, "%" + req.CASE.FAMILY + "%");
                }
                if (!string.IsNullOrEmpty(req.CASE.RESIDENCE))
                {
                    where += " AND RESIDENCE LIKE @RESIDENCE";
                    Db.AddInParameter(cmd, "RESIDENCE", DbType.String, "%" + req.CASE.RESIDENCE + "%");
                }
                if (!string.IsNullOrEmpty(req.CASE.NOTE))
                {
                    where += " AND NOTE LIKE @NOTE";
                    Db.AddInParameter(cmd, "NOTE", DbType.String, "%" + req.CASE.NOTE + "%");
                }
                if (!string.IsNullOrEmpty(req.CASE.PROBLEM))
                {
                    where += " AND PROBLEM LIKE @PROBLEM";
                    Db.AddInParameter(cmd, "PROBLEM", DbType.String, "%" + req.CASE.PROBLEM + "%");
                }
                if (!string.IsNullOrEmpty(req.CASE.EXPERIENCE))
                {
                    where += " AND EXPERIENCE LIKE @EXPERIENCE";
                    Db.AddInParameter(cmd, "EXPERIENCE", DbType.String, "%" + req.CASE.EXPERIENCE + "%");
                }
                if (!string.IsNullOrEmpty(req.CASE.SUGGEST))
                {
                    where += " AND SUGGEST LIKE @SUGGEST";
                    Db.AddInParameter(cmd, "SUGGEST", DbType.String, "%" + req.CASE.SUGGEST + "%");
                }


                if (!string.IsNullOrEmpty(req.CASE.CASE_STATUS))
                {
                    where += " AND SN IN (SELECT DISTINCT CASE_SN FROM INTERVIEW WHERE CASE_STATUS=@CASE_STATUS)";
                    Db.AddInParameter(cmd, "CASE_STATUS", DbType.String, req.CASE.CASE_STATUS);
                }
                if (!string.IsNullOrEmpty(req.CASE.CONTACT_TIME))
                {
                    where += " AND SN IN (SELECT DISTINCT CASE_SN FROM INTERVIEW WHERE CONTACT_TIME=@CONTACT_TIME)";
                    Db.AddInParameter(cmd, "CONTACT_TIME", DbType.String, req.CASE.CONTACT_TIME);
                }
                if (req.CASE.START_DATE != null)
                {
                    where += " AND SN IN (SELECT DISTINCT CASE_SN FROM INTERVIEW WHERE INCOMING_DATE>=@START_DATE)";
                    Db.AddInParameter(cmd, "START_DATE", DbType.DateTime2, req.CASE.START_DATE);
                }
                if (req.CASE.END_DATE != null)
                {
                    where += " AND SN IN (SELECT DISTINCT CASE_SN FROM INTERVIEW WHERE INCOMING_DATE<=@END_DATE)";
                    Db.AddInParameter(cmd, "END_DATE", DbType.DateTime2, req.CASE.END_DATE);
                }
                if (!string.IsNullOrEmpty(req.CASE.NAME))
                {
                    where += " AND SN IN (SELECT DISTINCT CASE_SN FROM INTERVIEW WHERE NAME LIKE @NAME)";
                    Db.AddInParameter(cmd, "NAME", DbType.String, "%" + req.CASE.NAME + "%");
                }
                if (!string.IsNullOrEmpty(req.CASE.TEL))
                {
                    where += " AND SN IN (SELECT DISTINCT CASE_SN FROM INTERVIEW WHERE TEL LIKE @TEL)";
                    Db.AddInParameter(cmd, "TEL", DbType.String, "%" + req.CASE.TEL + "%");
                }
                if (!string.IsNullOrEmpty(req.CASE.VOLUNTEER_SN))
                {
                    where += " AND SN IN (SELECT DISTINCT CASE_SN FROM INTERVIEW WHERE VOLUNTEER_SN=@VOLUNTEER_SN)";
                    Db.AddInParameter(cmd, "VOLUNTEER_SN", DbType.String, req.CASE.VOLUNTEER_SN);
                }

                if (where.Length > 0)
                {
                    where = " WHERE" + where.Substring(4);
                }

                string[] orderColumn = { "SN", "TEL1", "TEL2", "TEL3", "TEL4", "TEL5", "TEL6", "NAME1", "NAME2", "NAME3", "NAME4", "NAME5", "NAME6", "TEACHER1", "TEACHER2", "TEACHER3" };
                string[] ascDesc = { "ASC", "DESC", ""};

                string order = "";

                if (!string.IsNullOrEmpty(req.CASE.ORDER_BY))
                {
                    if (Array.IndexOf(orderColumn, req.CASE.ORDER_BY) > -1 && Array.IndexOf(ascDesc, req.CASE.ASC_DESC) > -1)
                    {
                        order = " ORDER BY " + req.CASE.ORDER_BY + " " + req.CASE.ASC_DESC;
                    }
                }


                sql = String.Format(sql, where, order);
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
                                var row = new CASE
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
                                    MERGE_REASON = reader["MERGE_REASON"] as string
                                };
                                res.CASE.Add(row);
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

//        public CaseRetrieveRes ReportData(CaseRetrieveReq req)
//        {
//            CaseRetrieveRes res = new CaseRetrieveRes()
//            {
//                CASE = new List<CASE>(),
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
//SN,NAME1,NAME2,NAME3,NAME4,NAME5,NAME6,TEL1,TEL2,TEL3,TEL4,TEL5,TEL6,AGE,ADDRESS,FAMILY,EXPERIENCE,SUGGEST,
//    FROM CASE{0}
//    ORDER BY SN DESC;";
//                string where = "";
//                Db.AddInParameter(cmd, "TOP", DbType.Int32, 1000000);

//                //if (!string.IsNullOrEmpty(req.CASE.SN))
//                {
//                    where += " AND SN=@SN";
//                    Db.AddInParameter(cmd, "SN", DbType.String, req.CASE.SN);
//                }
//                if (!string.IsNullOrEmpty(req.CASE.NAME1))
//                {
//                    where += " AND NAME1=@NAME1";
//                    Db.AddInParameter(cmd, "NAME1", DbType.String, req.CASE.NAME1);
//                }
//                if (!string.IsNullOrEmpty(req.CASE.NAME2))
//                {
//                    where += " AND NAME2=@NAME2";
//                    Db.AddInParameter(cmd, "NAME2", DbType.String, req.CASE.NAME2);
//                }
//                if (!string.IsNullOrEmpty(req.CASE.NAME3))
//                {
//                    where += " AND NAME3=@NAME3";
//                    Db.AddInParameter(cmd, "NAME3", DbType.String, req.CASE.NAME3);
//                }
//                if (!string.IsNullOrEmpty(req.CASE.NAME4))
//                {
//                    where += " AND NAME4=@NAME4";
//                    Db.AddInParameter(cmd, "NAME4", DbType.String, req.CASE.NAME4);
//                }
//                if (!string.IsNullOrEmpty(req.CASE.NAME5))
//                {
//                    where += " AND NAME5=@NAME5";
//                    Db.AddInParameter(cmd, "NAME5", DbType.String, req.CASE.NAME5);
//                }
//                if (!string.IsNullOrEmpty(req.CASE.NAME6))
//                {
//                    where += " AND NAME6=@NAME6";
//                    Db.AddInParameter(cmd, "NAME6", DbType.String, req.CASE.NAME6);
//                }
//                if (!string.IsNullOrEmpty(req.CASE.TEL1))
//                {
//                    where += " AND TEL1=@TEL1";
//                    Db.AddInParameter(cmd, "TEL1", DbType.String, req.CASE.TEL1);
//                }
//                if (!string.IsNullOrEmpty(req.CASE.TEL2))
//                {
//                    where += " AND TEL2=@TEL2";
//                    Db.AddInParameter(cmd, "TEL2", DbType.String, req.CASE.TEL2);
//                }
//                if (!string.IsNullOrEmpty(req.CASE.TEL3))
//                {
//                    where += " AND TEL3=@TEL3";
//                    Db.AddInParameter(cmd, "TEL3", DbType.String, req.CASE.TEL3);
//                }
//                if (!string.IsNullOrEmpty(req.CASE.TEL4))
//                {
//                    where += " AND TEL4=@TEL4";
//                    Db.AddInParameter(cmd, "TEL4", DbType.String, req.CASE.TEL4);
//                }
//                if (!string.IsNullOrEmpty(req.CASE.TEL5))
//                {
//                    where += " AND TEL5=@TEL5";
//                    Db.AddInParameter(cmd, "TEL5", DbType.String, req.CASE.TEL5);
//                }
//                if (!string.IsNullOrEmpty(req.CASE.TEL6))
//                {
//                    where += " AND TEL6=@TEL6";
//                    Db.AddInParameter(cmd, "TEL6", DbType.String, req.CASE.TEL6);
//                }
//                if (!string.IsNullOrEmpty(req.CASE.YEARS_OLD))
//                {
//                    where += " AND YEARS_OLD=@YEARS_OLD";
//                    Db.AddInParameter(cmd, "YEARS_OLD", DbType.String, req.CASE.YEARS_OLD);
//                }
//                if (!string.IsNullOrEmpty(req.CASE.ADDRESS))
//                {
//                    where += " AND ADDRESS=@ADDRESS";
//                    Db.AddInParameter(cmd, "ADDRESS", DbType.String, req.CASE.ADDRESS);
//                }
//                if (!string.IsNullOrEmpty(req.CASE.FAMILY))
//                {
//                    where += " AND FAMILY=@FAMILY";
//                    Db.AddInParameter(cmd, "FAMILY", DbType.String, req.CASE.FAMILY);
//                }
//                if (!string.IsNullOrEmpty(req.CASE.EXPERIENCE))
//                {
//                    where += " AND EXPERIENCE=@EXPERIENCE";
//                    Db.AddInParameter(cmd, "EXPERIENCE", DbType.String, req.CASE.EXPERIENCE);
//                }
//                if (!string.IsNullOrEmpty(req.CASE.SUGGEST))
//                {
//                    where += " AND SUGGEST=@SUGGEST";
//                    Db.AddInParameter(cmd, "SUGGEST", DbType.String, req.CASE.SUGGEST);
//                }
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
//                        var row = new CASE
//                        {

//                            //SN = reader["SN"] as string,
//                            NAME1 = reader["NAME1"] as string,
//                            NAME2 = reader["NAME2"] as string,
//                            NAME3 = reader["NAME3"] as string,
//                            NAME4 = reader["NAME4"] as string,
//                            NAME5 = reader["NAME5"] as string,
//                            NAME6 = reader["NAME6"] as string,
//                            TEL1 = reader["TEL1"] as string,
//                            TEL2 = reader["TEL2"] as string,
//                            TEL3 = reader["TEL3"] as string,
//                            TEL4 = reader["TEL4"] as string,
//                            TEL5 = reader["TEL5"] as string,
//                            TEL6 = reader["TEL6"] as string,
//                            YEARS_OLD = reader["YEARS_OLD"] as string,
//                            ADDRESS = reader["ADDRESS"] as string,
//                            FAMILY = reader["FAMILY"] as string,
//                            EXPERIENCE = reader["EXPERIENCE"] as string,
//                            SUGGEST = reader["SUGGEST"] as string,
//                        };
//                        res.CASE.Add(row);
//                    }
//                }
//            }
//            res.Pagination.EndTime = DateTime.Now;

//            return res;
//        }

        public int DataMerge(MergeModifyReq req)
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

                #region ADD
                sql = @"
UPDATE CASE_OWNER
	SET YEARS_OLD=@YEARS_OLD,ADDRESS=@ADDRESS,TEACHER1=@TEACHER1,TEACHER2=@TEACHER2,TEACHER3=@TEACHER3,NAME1=@NAME1,NAME2=@NAME2,NAME3=@NAME3,NAME4=@NAME4,NAME5=@NAME5,NAME6=@NAME6,TEL1=@TEL1,TEL2=@TEL2,TEL3=@TEL3,TEL4=@TEL4,TEL5=@TEL5,TEL6=@TEL6
        ,FAMILY=@FAMILY,RESIDENCE=@RESIDENCE,NOTE=@NOTE,PROBLEM=@PROBLEM,EXPERIENCE=@EXPERIENCE,SUGGEST=@SUGGEST,MERGE_REASON=@MERGE_REASON,MDATE=GETDATE(), MUSER=@MUSER
    WHERE SN=@SN;
";
                cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;

                Db.AddInParameter(cmd, "SN", DbType.String, req.ADD.SN);
                Db.AddInParameter(cmd, "NAME1", DbType.String, req.ADD.NAME1);
                Db.AddInParameter(cmd, "NAME2", DbType.String, req.ADD.NAME2);
                Db.AddInParameter(cmd, "NAME3", DbType.String, req.ADD.NAME3);
                Db.AddInParameter(cmd, "NAME4", DbType.String, req.ADD.NAME4);
                Db.AddInParameter(cmd, "NAME5", DbType.String, req.ADD.NAME5);
                Db.AddInParameter(cmd, "NAME6", DbType.String, req.ADD.NAME6);
                Db.AddInParameter(cmd, "TEL1", DbType.String, req.ADD.TEL1);
                Db.AddInParameter(cmd, "TEL2", DbType.String, req.ADD.TEL2);
                Db.AddInParameter(cmd, "TEL3", DbType.String, req.ADD.TEL3);
                Db.AddInParameter(cmd, "TEL4", DbType.String, req.ADD.TEL4);
                Db.AddInParameter(cmd, "TEL5", DbType.String, req.ADD.TEL5);
                Db.AddInParameter(cmd, "TEL6", DbType.String, req.ADD.TEL6);
                Db.AddInParameter(cmd, "YEARS_OLD", DbType.String, req.ADD.YEARS_OLD);
                Db.AddInParameter(cmd, "ADDRESS", DbType.String, req.ADD.ADDRESS);
                Db.AddInParameter(cmd, "TEACHER1", DbType.String, req.ADD.TEACHER1);
                Db.AddInParameter(cmd, "TEACHER2", DbType.String, req.ADD.TEACHER2);
                Db.AddInParameter(cmd, "TEACHER3", DbType.String, req.ADD.TEACHER3);
                Db.AddInParameter(cmd, "FAMILY", DbType.String, req.ADD.FAMILY);
                Db.AddInParameter(cmd, "RESIDENCE", DbType.String, req.ADD.RESIDENCE);
                Db.AddInParameter(cmd, "NOTE", DbType.String, req.ADD.NOTE);
                Db.AddInParameter(cmd, "PROBLEM", DbType.String, req.ADD.PROBLEM);
                Db.AddInParameter(cmd, "EXPERIENCE", DbType.String, req.ADD.EXPERIENCE);
                Db.AddInParameter(cmd, "SUGGEST", DbType.String, req.ADD.SUGGEST);
                Db.AddInParameter(cmd, "MERGE_REASON", DbType.String, req.ADD.MERGE_REASON);
                Db.AddInParameter(cmd, "CUSER", DbType.String, req.ADD.CUSER);
                Db.AddInParameter(cmd, "MUSER", DbType.String, req.ADD.MUSER);

                //Db.AddOutParameter(cmd, "SN", DbType.Int32, 1);
                count = Db.ExecuteNonQuery(cmd);
                //req.CASE.SN = Db.GetParameterValue(cmd, "SN") as Int32? ?? null;
                #endregion

                #region REMOVE
                sql = @"
UPDATE INTERVIEW
	SET CASE_SN=@CASE_SN_ADD
    WHERE CASE_SN=@CASE_SN_REMOVE;
";
                cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;

                Db.AddInParameter(cmd, "CASE_SN_ADD", DbType.String, req.ADD.SN);
                Db.AddInParameter(cmd, "CASE_SN_REMOVE", DbType.String, req.REMOVE.SN);

                //Db.AddOutParameter(cmd, "SN", DbType.Int32, 1);
                count = Db.ExecuteNonQuery(cmd);
                //req.CASE.SN = Db.GetParameterValue(cmd, "SN") as Int32? ?? null;

                sql = @"
DELETE FROM CASE_OWNER
    WHERE SN=@SN;
";
                cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;

                Db.AddInParameter(cmd, "SN", DbType.String, req.REMOVE.SN);

                //Db.AddOutParameter(cmd, "SN", DbType.Int32, 1);
                count = Db.ExecuteNonQuery(cmd);
                //req.CASE.SN = Db.GetParameterValue(cmd, "SN") as Int32? ?? null;
                #endregion

                trans.Commit();

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
            return count;
        }

    }
}