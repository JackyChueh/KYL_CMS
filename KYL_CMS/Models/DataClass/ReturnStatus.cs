using System;
using System.Reflection;
using System.ComponentModel;

namespace KYL_CMS.Models.DataClass
{
    public class ReturnStatus
    {
        public ReturnStatus()
        {
            this._code = ReturnCode.FAIL;
        }

        public ReturnStatus(ReturnCode Code)
        {
            this.Code = Code;
        }
        public ReturnStatus(ReturnCode Code,string Message)
        {
            this._code = Code;
            this.Message = Message;
        }

        private ReturnCode _code ;

        public ReturnCode Code
        {
            get
            {
                return this._code;
            }
            set
            {
                this._code = value;
                FieldInfo fi = value.GetType().GetField(value.ToString());
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                this.Message = attributes.Length > 0 ? attributes[0].Description : value.ToString();
            }
        }
        public string Message { get; set; }
    }
}