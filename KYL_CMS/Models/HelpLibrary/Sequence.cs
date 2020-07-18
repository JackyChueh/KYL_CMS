using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using KYL_CMS.Models.DataAccess;
using KYL_CMS.Models.DataClass;
using KYL_CMS.Models.DataClass.Help;

namespace KYL_CMS.Models.HelpLibrary
{
    public class Sequence : DatabaseAccess
    {
        public Sequence(string connectionStringName) : base(connectionStringName)
        {
        }

        public string GetSeqNo(string Type, string Pre,int Len)
        {
            string NewSeqNo = "";
            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"USP_SEQ";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = sql;

                Db.AddInParameter(cmd, "CUR_TYPE", DbType.String, Type);
                Db.AddInParameter(cmd, "CUR_PRE", DbType.String, Pre);
                Db.AddInParameter(cmd, "LEN", DbType.Int32, Len);

                using (IDataReader reader = Db.ExecuteReader(cmd))
                {
                    reader.Read();
                    NewSeqNo = reader["NEW_NO"].ToString();
                }
            }
            return NewSeqNo;
        }

        public Int64? GetSeqBigInt(string Type)
        {
            Int64? NewSeqNo = null;
            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"USP_SEQ_BIGINT";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = sql;

                Db.AddInParameter(cmd, "CUR_TYPE", DbType.String, Type);

                using (IDataReader reader = Db.ExecuteReader(cmd))
                {
                    reader.Read();
                    NewSeqNo = reader["NEW_NO"] as Int64? ?? null;
                }
            }
            return NewSeqNo;
        }
    }
}