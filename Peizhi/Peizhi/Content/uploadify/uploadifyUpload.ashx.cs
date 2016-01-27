using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UploadTest.Scripts.uploadify
{
    /// <summary>
    /// uploadifyUpload 的摘要说明
    /// </summary>
    public class uploadifyUpload : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            context.Response.ContentType = "video/x-la-asf";
            context.Response.Charset = "utf-8";
            HttpFileCollection file = HttpContext.Current.Request.Files;
            string result = "";
            string uploadPath = context.Server.MapPath("/Content/UploadFiles/images" + "\\");
            if (file != null)
            {   
                try
                {

                    if (!System.IO.Directory.Exists(uploadPath))
                    {
                        System.IO.Directory.CreateDirectory(uploadPath);
                    }
                    DateTime dtnow = System.DateTime.Now;
                    string filename = dtnow.Year.ToString() + dtnow.Month.ToString() + dtnow.Day.ToString() + dtnow.Hour.ToString() + dtnow.Minute.ToString() + dtnow.Second.ToString() + dtnow.Millisecond.ToString();
                    string ExtName = getFileExt(file[0].FileName).ToUpper();
                    filename += "." + ExtName;
                    file[0].SaveAs(uploadPath + filename);
                    result = "1|" + filename + "";
                }
                catch
                {
                    result = "0|";
                }
            }
            else
            {
                result = "0|";
            }
            context.Response.Write(result); //标志位1标识上传成功，后面的可以返回前台的参数，比如上传后的路径等，中间使用|隔开
        }
        private string getFileExt(string fileName)
        {
            if (fileName.IndexOf(".") == -1)
                return "";
            string[] temp = fileName.Split('.');
            return temp[temp.Length - 1].ToLower();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}