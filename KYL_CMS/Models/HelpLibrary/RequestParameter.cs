using System;
using System.Collections;
using System.Web;

namespace KYL_CMS.Models.HelpLibrary
{
    public class RequestParameter
    {
        private Hashtable _ht = new Hashtable();
        /// <summary>
        /// 載入參數
        /// </summary>
        /// <param name="Request"></param>
        public void Load(HttpRequestBase Request)
        {
            foreach (string key in Request.QueryString.AllKeys)
            {
                if (_ht.ContainsKey(key))
                    _ht.Remove(key);
                _ht.Add(key, Request.QueryString[key].Trim());
            }
            foreach (string key in Request.Form.AllKeys)
            {
                if (_ht.ContainsKey(key))
                    _ht.Remove(key);
                _ht.Add(key, Request.Form[key].Trim());
            }
        }

        /// <summary>
        /// 檢查必要參數是否有傳入
        /// </summary>
        /// <param name="Request">Request</param>
        /// <param name="RequireKeys">參數</param>
        public bool Require(string[] RequireKeys)
        {
            foreach (string key in RequireKeys)
            {
                if (!_ht.ContainsKey(key) || _ht[key].ToString().Length == 0)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 取出參數內容
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public string Item(string Key)
        {
            string value = null;
            if (_ht.ContainsKey(Key))
                value = _ht[Key].ToString();
            return value;
        }

        /// <summary>
        /// 顯示參數內容
        /// </summary>
        /// <returns></returns>
        public String PrintKeyValue()
        {
            string print = "";
            foreach (string k in _ht.Keys)
                print += k + "=" + this.Item(k) + ",";
            return print.TrimEnd(',');
        }
    }
}