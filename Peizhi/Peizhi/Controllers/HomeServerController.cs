using Kain_class.Messageinfor;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Peizhi.Controllers
{
    public class HomeServerController : MainsController
    {
        //
        // GET: /HomeServer/

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 关于我们
        /// </summary>
        /// <returns></returns>
        public string AboutAjax()
        {
            string types = Request["types"];
            MessasgeData mgdata = Datafun.MgfunctionData("select * from tb_about where types=@types",new SqlParameter("@types",types));
            string IsSuccess = mgdata.Mgdata.Rows[0]["Contents"].ToString();
            return IsSuccess;
        }
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <returns></returns>
        public ActionResult FileDownload(string id, string ids)
        {
            if (LgUser == null)
            {
                return RedirectToAction("jpmember", "Home");
            }
            if (LgUser.UserLvl == 1)
            {
                return RedirectToAction("jpmember", "Home");
            }
            try { 
            string[] types = id.Split('.');
            string absoluFilePath = Server.MapPath("/Content/UploadFiles/dataload/" + id);
            return File(new FileStream(absoluFilePath, FileMode.Open), "application/x-zip-compressed", Server.UrlEncode(ids + "." + types[1]));    
                }
            catch(Exception ex)
            {
                return RedirectToAction("catchs", "Home");
            }
         
        }
        public string Exam()
        {
            string Types = Request["id"];
            StringBuilder sc = new StringBuilder();
            string sd = "";
            string sa = @"<div class='lists'>
            <div class='left'>{$title$}</div>
            <a class='right' href='/Exam/ExamInfor/{$id$}'>考试</a>
          </div>";
            MessasgeData mgdata = Datafun.MgfunctionData(" select  top 7 * from tb_examlist  where isdel=0 and Types in(select id from tb_types where pid=@Types) order by id desc", new SqlParameter("@Types",Types));
            foreach (DataRow dr in mgdata.Mgdata.Rows)
            { 
                sd=sa.Replace("{$title$}",dr["title"].ToString()).Replace("{$id$}",dr["id"].ToString());
                sc.Append(sd);
            }


            return sc.ToString();
        }
        public string Video()
        {
            string Types = Request["id"];
            StringBuilder sc = new StringBuilder();
            string sd = "";
            string sa = @"<div class='lists last'>
            <div class='left'>{$title$}</div>
            <div class='middle'>           
            </div>
            <div class='right'>
              <a class='a1' href='###' onclick=enroll('{$title$}','{$introduce$}')>报名</a>
              <a class='a2' href='/Video/Videoinfor/{$id$}'>试看</a>
              <a class='a3' href='/Video/Videoinfor/{$id$}'>学习</a>
            </div>
          </div>";
            MessasgeData mgdata = Datafun.MgfunctionData("select * from dbo.tb_CourseVideo where Types in(select id from tb_types where pid=@Types)", new SqlParameter("@Types", Types));
            foreach (DataRow dr in mgdata.Mgdata.Rows)
            {
                sd = sa.Replace("{$title$}", dr["title"].ToString()).Replace("{$id$}", dr["id"].ToString()).Replace("{$introduce$}", dr["introduce"].ToString());
                sc.Append(sd);
            }


            return sc.ToString();
        }

    }
}
