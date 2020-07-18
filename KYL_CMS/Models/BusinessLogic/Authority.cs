using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KYL_CMS.Models.EntityDefinition;
using KYL_CMS.Models.DataAccess;
using System.Data;
using System.Data.Common;

namespace KYL_CMS.Models.BusinessLogic
{
    public class Authority : DatabaseAccess
    {
        public Authority(string connectionStringName) : base(connectionStringName) { }

        public USER_INFO UserLoginAuthority(string UserId, string Password)
        {
            USER_INFO users = null;
            string sql = @"
SELECT U.SN, U.NAME, U.PASSWORD, U.FORCE_PWD, G.ROLES_SN
    FROM USERS U LEFT JOIN GRANTS G
		ON U.SN=G.USERS_SN
    WHERE U.ID=@ID AND U.MODE='Y'
";
            using (DbCommand cmd = Db.GetSqlStringCommand(sql))
            {
                Db.AddInParameter(cmd, "ID", DbType.String, UserId);
                using (IDataReader reader = this.Db.ExecuteReader(cmd))
                {
                    if (reader.Read() && reader["PASSWORD"] as string == Password)
                    {
                        users = new USER_INFO()
                        {
                            SN = reader["SN"] as Int16? ?? null,
                            ID = UserId,
                            NAME = reader["NAME"] as string,
                            FORCE_PWD = reader["FORCE_PWD"] as Int16? ?? null,
                            ROLES_SN = reader["ROLES_SN"] as Int32? ?? null
                        };
                    }
                }
            }
            return users;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string UserFunctionAuthority(USER_INFO UserInfo)
        {
            List<AUTHORITY> authority = new List<AUTHORITY>();
            try
            {
                string sql = @"
SELECT F.SN, F.NAME, F.CATEGORY, F.URL, F.PARENT_SN, A.ROLES_SN
	FROM FUNCTIONS F LEFT JOIN AUTHORITY A ON F.SN=A.FUNCTIONS_SN AND A.ROLES_SN=@ROLES_SN
        WHERE F.MODE='Y'
	ORDER BY F.SORT
";
                using (DbCommand cmd = Db.GetSqlStringCommand(sql))
                {
                    Db.AddInParameter(cmd, "ROLES_SN", DbType.String, UserInfo.ROLES_SN);
                    using (IDataReader reader = this.Db.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            AUTHORITY data = new AUTHORITY
                            {
                                //USERS_SN = reader["USERS_SN"] as Int16? ?? null,
                                //FUNCTIONS_SN = reader["SN"] as Int16? ?? null,
                                ROLES_SN = reader["ROLES_SN"] as Int32? ?? null,
                                FUNCTIONS = new FUNCTIONS()
                                {
                                    SN = reader["SN"] as Int16? ?? null,
                                    NAME = reader["NAME"] as string,
                                    CATEGORY = reader["CATEGORY"] as string,
                                    URL = reader["URL"] as string,
                                    PARENT_SN = reader["PARENT_SN"] as Int16? ?? null
                                }
                            };
                            authority.Add(data);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            return ConstructSidebar(authority, 0, -2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authority"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        private string ConstructSidebar(List<AUTHORITY> authority, Int16? parent, Int16 level)
        {
            List<AUTHORITY> currentItems = authority.Where(a => a.FUNCTIONS.PARENT_SN == parent && (a.ROLES_SN != null || a.FUNCTIONS.CATEGORY == "S" || a.FUNCTIONS.CATEGORY == "D")).ToList();
            string currentHtml = "", subHtml = "";
            if (currentItems.Count() > 0)
            {
                level += 2;
                foreach (var item in currentItems)
                {
                    subHtml = ConstructSidebar(authority, item.FUNCTIONS.SN, level);
                    if (item.FUNCTIONS.CATEGORY == "P")
                    {
                        currentHtml += "<li class='ml-" + (level - 1).ToString() + "'><a href='" + item.FUNCTIONS.URL + "'>" + item.FUNCTIONS.NAME + "</a></li>";
                    }
                    else if (subHtml.Length > 0)
                    {
                        currentHtml += "<li class='ml-" + level.ToString() + "'><a href='#id" + item.FUNCTIONS.SN + "' data-toggle='collapse' aria-expanded='false' class='dropdown-toggle'>" + item.FUNCTIONS.NAME + "</a><ul class='collapse list-unstyled' id='id" + item.FUNCTIONS.SN + "'>" + subHtml + "</ul></li>";
                    }
                }
            }
            return currentHtml;
        }

    }

}