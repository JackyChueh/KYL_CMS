using System;
using System.Web;
using System.IO;
using KYL_CMS.Models.DataClass.Case;

namespace KYL_CMS.Models.HelpLibrary
{
    public class UploadFile
    {
        public string FamilyFileUpload(HttpRequestBase request)
        {
            string fileName = "";

            string _LogPath = HttpContext.Current.Request.PhysicalApplicationPath + "img\\upload\\";
            if (request.Files.AllKeys.Length > 0)
            {
                var httpPostedFile = request.Files["file"];
                string extension = Path.GetExtension(httpPostedFile.FileName);
                if (httpPostedFile != null && httpPostedFile.ContentLength != 0)
                {
                    fileName = Guid.NewGuid() + extension;
                    httpPostedFile.SaveAs(_LogPath + fileName);
                }
            }
            return fileName;
        }

    }
}