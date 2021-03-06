﻿using System;
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
    public class QueryItems : DatabaseAccess
    {
        private const string functionParentSn = "SELECT SN AS ITEM_KEY,NAME+'('+CATEGORY+')' AS ITEM_VALUE FROM FUNCTIONS ORDER BY SORT";
        private const string userName = "SELECT ID AS ITEM_KEY,ID AS ITEM_VALUE FROM USERS ORDER BY SN DESC";
        private const string UserIdName = "SELECT SN AS ITEM_KEY,ID + ' ' + NAME AS ITEM_VALUE FROM USERS ORDER BY SN DESC";
        private const string RolesName = "SELECT SN AS ITEM_KEY,NAME AS ITEM_VALUE FROM ROLES ORDER BY SN DESC";
        private Dictionary<string, string> sql = new Dictionary<string, string>();

        public QueryItems(string connectionStringName) : base(connectionStringName)
        {
            sql.Add("functionParentSn", functionParentSn);
            sql.Add("userName", userName);
            sql.Add("UserIdName", UserIdName);
            sql.Add("RolesName", RolesName);
        }

        private Dictionary<string, object> pharse = new Dictionary<string, object>();

        public ItemListRetrieveRes ItemListQuery(ItemListRetrieveReq req)
        {
            if (req.TableItem != null)
            {
                foreach (string key in req.TableItem)
                {
                    if (sql.ContainsKey(key))
                    {
                        TableQuery(key, sql[key]);
                    }
                }
            }
            if (req.PhraseGroup != null)
            {
                //foreach (string key in req.PhraseGroup)
                {
                    PhraseQuery(req.PhraseGroup);
                }
            }

            ItemListRetrieveRes res = new ItemListRetrieveRes
            {
                ItemList = pharse
            };
            return res;
        }

        private void TableQuery(string key, string sql)
        {
            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                using (IDataReader reader = Db.ExecuteReader(cmd))
                {
                    List<ItemList> rows = new List<ItemList>();
                    while (reader.Read())
                    {
                        ItemList row = new ItemList
                        {
                            Key = reader["ITEM_KEY"].ToString(),
                            Value = reader["ITEM_VALUE"].ToString()
                        };
                        rows.Add(row);
                    }
                    pharse.Add(key, rows);
                }
            }

        }

        private void PhraseQuery(string[] phrase_group)
        {

            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
SELECT PHRASE_GROUP,PHRASE_KEY,PHRASE_VALUE,PHRASE_DESC
    FROM PHRASE 
        WHERE PHRASE_GROUP IN ({0}) AND MODE='Y'
    ORDER BY PHRASE_GROUP,SORT";
                string values = "";
                foreach (string s in phrase_group)
                {
                    values += ",@" + s;
                    Db.AddInParameter(cmd, s, DbType.String, s);
                }
                sql = string.Format(sql, values.TrimStart(','));

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                using (IDataReader reader = Db.ExecuteReader(cmd))
                {
                    List<ItemList> rows = null;
                    string group = null;
                    while (reader.Read())
                    {
                        if (group != reader["PHRASE_GROUP"] as string)
                        {
                            rows = new List<ItemList>();
                            group = reader["PHRASE_GROUP"] as string;
                            pharse.Add(group, rows);
                        }
                        ItemList row = new ItemList
                        {
                            Key = reader["PHRASE_KEY"].ToString(),
                            Value = reader["PHRASE_VALUE"].ToString(),
                            Desc = reader["PHRASE_DESC"].ToString()
                        };
                        rows.Add(row);
                    }
                }
            }

        }

    }
}