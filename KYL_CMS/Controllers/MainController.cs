using System;
//using System.Web;
using System.Text;
using System.Security.Cryptography;
using System.Web.Mvc;
//using System.Web.Security;
using KYL_CMS.Models.EntityDefinition;
using KYL_CMS.Models.BusinessLogic;
using KYL_CMS.Models.DataClass;
using KYL_CMS.Models.DataClass.Users;
using KYL_CMS.Models.DataClass.Help;
using Newtonsoft.Json;

namespace KYL_CMS.Controllers
{
    public class MainController : BaseController
    {
        ////[Authorize]
        [ChildActionOnly]
        public ActionResult Sidebar()
        {
            try
            {
                //FormsIdentity id = (FormsIdentity)User.Identity;
                //FormsAuthenticationTicket ticket = id.Ticket;
                //USER_INFO users = JsonConvert.DeserializeObject<USER_INFO>(ticket.UserData);

                USER_INFO userInfo = JsonConvert.DeserializeObject<USER_INFO>(Session["INFO"].ToString());

                ViewBag.Sidebar = new Authority("SCC").UserFunctionAuthority(userInfo);
            }
            catch (Exception ex)
            {
                Log("Err=" + ex.Message);
                Log(ex.StackTrace);
            }
            return View();
        }

        ////[Authorize]
        public ActionResult Dashboard()
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Login", "Main");
            }
            else
            {
                return View();
            }

        }

        ////[Authorize]
        public ActionResult ForceChange()
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Login", "Main");
            }
            else
            {
                return View();
            }
        }

        public ActionResult Login()
        {
            //Session.RemoveAll();
            //FormCollection data = new FormCollection();
            //data.Add("account", "Admin");
            //data.Add("password", "TWtaxi");

            //return Login(data);

            //FormsAuthentication.SignOut();

            Session.Clear();
            Session.Abandon();
            return View();

        }

        //[ValidateAntiForgeryToken]
        [AcceptVerbs("POST")]
        public ActionResult Login(FormCollection data)
        {
            ReturnStatus res = new ReturnStatus();
            string account = data["account"];
            string password = data["password"];
            SHA256 sha256 = new SHA256CryptoServiceProvider();
            byte[] source = Encoding.Default.GetBytes(data["password"]);
            byte[] crypto = sha256.ComputeHash(source); //進行SHA256加密
            string passwordSha256 = Convert.ToBase64String(crypto);

            string ip = "";
            try
            {
                //使用者瀏覽器
                Log("UserAgent={0}", Request.UserAgent);

                //使用者來源IP
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (!string.IsNullOrEmpty(ipAddress))
                {
                    string[] addresses = ipAddress.Split(',');
                    if (addresses.Length != 0)
                    {
                        ip = addresses[0];
                    }
                }

                ip = context.Request.ServerVariables["REMOTE_ADDR"];
            }
            catch
            { }


            try
            {
                Log("account={0}, password={1}, IP={2}", account, passwordSha256, ip);
                

                USER_INFO userInfo = new Authority("SCC").UserLoginAuthority(account, password);
                if (userInfo != null)
                {
 
                    var json = JsonConvert.SerializeObject(userInfo);

                    //var aaa = Session["ID"] == null ? "" : Session["ID"].ToString();
                    //FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    //       version: 1,
                    //       name: account,   //Session["ID"].ToString()
                    //       issueDate: DateTime.Now,
                    //       expiration: DateTime.Now.AddMinutes(1440),
                    //       isPersistent: false, //remeber-me
                    //       userData: json,  //ticket.UserData
                    //       cookiePath: FormsAuthentication.FormsCookiePath
                    //       );

                    //string encryptTicket = FormsAuthentication.Encrypt(ticket);

                    //Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encryptTicket));

                    Session["ID"] = account;
                    Session["INFO"] = json;

                    if (userInfo.FORCE_PWD == 1)
                    {
                        return RedirectToAction("ForceChange", "Main");
                    }
                    else
                    {
                        KYL_CMS.Models.HelpLibrary.Log.Write("Global", "Main/Login  SessionID:" + Session.SessionID);
                        return RedirectToAction("Dashboard", "Main");
                    }
                    
                    //return RedirectToAction("GrantsIndex", "Grants");
                    //return RedirectToAction("MergeIndex", "Merge");
                }
                else
                {
                    res = new ReturnStatus
                    {
                        Code = ReturnCode.LOGIN_FAIL
                    };
                }
            }
            catch (Exception ex)
            {
                Log("Err=" + ex.Message);
                Log(ex.StackTrace);
                res = new ReturnStatus(ReturnCode.SERIOUS_ERROR);
            }
            Log("Res=" + JsonConvert.SerializeObject(res));
            return View(res);
        }

        ////[Authorize]
        [AcceptVerbs("POST")]
        public ActionResult ForceChange(FormCollection data)
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Login", "Main");
            }
            else
            {
                ReturnStatus res = new ReturnStatus(ReturnCode.FAIL);
                string password = data["PASSWORD"];
                string password2 = data["PASSWORD2"];

                //FormsIdentity id = (FormsIdentity)User.Identity;
                //FormsAuthenticationTicket ticket = id.Ticket;
                //USER_INFO users = JsonConvert.DeserializeObject<USER_INFO>(ticket.UserData);
                USER_INFO users = JsonConvert.DeserializeObject<USER_INFO>(Session["INFO"].ToString());

                Log("password={0}, password2={1}, sn={2}", password, password2, users.SN);

                if (password == password2)
                {
                    try
                    {
                        UsersModifyReq userinfo = new UsersModifyReq
                        {
                            USERS = new USERS()
                        };
                        userinfo.USERS.SN = users.SN;
                        userinfo.USERS.PASSWORD = password;
                        userinfo.USERS.MUSER = Session["ID"].ToString();
                        int i = new KYL_CMS.Models.BusinessLogic.Users("SCC").DataReset(userinfo, 0);
                        return RedirectToAction("Dashboard", "Main");
                    }
                    catch (Exception ex)
                    {
                        Log("Err=" + ex.Message);
                        Log(ex.StackTrace);
                        res = new ReturnStatus(ReturnCode.SERIOUS_ERROR);
                    }
                    Log("Res=" + JsonConvert.SerializeObject(res));
                }
                return View(res);
            }
        }

        ////[Authorize]
        public string ItemListRetrieve(ItemListRetrieveReq req)
        {
            ItemListRetrieveRes res = new ItemListRetrieveRes();

            if (Session["ID"] == null)
            {
                res.ReturnStatus = new ReturnStatus(ReturnCode.SESSION_TIMEOUT);
            }
            else
            {
                try
                {
                    res = new Models.HelpLibrary.QueryItems("SCC").ItemListQuery(req);
                    res.ReturnStatus = new ReturnStatus(ReturnCode.SUCCESS);
                }
                catch (Exception ex)
                {
                    Log("Err=" + ex.Message);
                    Log(ex.StackTrace);
                    res.ReturnStatus = new ReturnStatus(ReturnCode.SERIOUS_ERROR);
                }
            }
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
            };
            var json = JsonConvert.SerializeObject(res, Formatting.Indented);
            Log("Res=" + json);
            return json;
        }

    }
}