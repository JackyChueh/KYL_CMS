using System.ComponentModel;

namespace KYL_CMS.Models.DataClass
{
    public enum ReturnCode : short
    {
        [Description("密碼重置完成")]
        RESET_SUCCESS = 5,
        [Description("設定完成")]
        SET_SUCCESS = 4,
        [Description("資料刪除完成")]
        DEL_SUCCESS = 3,
        [Description("資料修改完成")]
        EDIT_SUCCESS = 2,
        [Description("資料新增成功")]
        ADD_SUCCESS = 1,
        [Description("交易成功")]
        SUCCESS = 0,
        [Description("交易失敗")]
        FAIL = -1,
        [Description("參數格式錯誤")]
        LACK_KEY = -2,
        [Description("帳號或密碼輸入錯誤")]
        LOGIN_FAIL = -3,
        [Description("嚴重系統錯誤")]
        SERIOUS_ERROR = -4,
        [Description("連線逾時")]
        SESSION_TIMEOUT = -5,
        [Description("帳號資料不存在")]
        NO_ACCOUNT = -6,
        [Description("註冊驗證失敗")]
        CONFIRM_FAIL = -7,
        [Description("帳號或電子郵件輸入錯誤")]
        FORGET_FAIL = -8,
        [Description("電子郵件輸入錯誤")]
        EMAIL_REQUIRED = -9,
        [Description("簡訊發送失敗")]
        SMS_FAIL = -10,
        [Description("檔案上傳失敗")]
        UPLOAD_FAIL = -11,
        [Description("不可刪除或變更正在使用中的資料項目")]
        ITEM_USED = -12
    }
}