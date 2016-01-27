using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using Kain_class.Messageinfor;
using Kain_class.Pass;
using System.Collections;

namespace Peizhi.Controllers
{
    public class MemberController : MainsController
    {
        //
        // GET: /Member/
        /// <summary>
        /// 个人中心
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberIndex()
        {
            ViewBag.image = LgUser.image;
            return View();
        }
        /// <summary>
        /// 个人设置
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberSet()
        {
            ViewBag.image = "";
            ViewBag.username = "";
            ViewBag.bank = "";
            ViewBag.email = "";
            ViewBag.sex = "";
            MessasgeData mgdata = Datafun.MgfunctionData("select * from  tb_login where id=@id", new SqlParameter("@id", LgUser.UserId));
            if (mgdata.Mgdatacount > 0)
            {
                ViewBag.image = mgdata.Mgdata.Rows[0]["image"];
                ViewBag.username = mgdata.Mgdata.Rows[0]["UserName"];
                ViewBag.bank = mgdata.Mgdata.Rows[0]["Bank"];
                ViewBag.email = mgdata.Mgdata.Rows[0]["Email"];
                ViewBag.sex = mgdata.Mgdata.Rows[0]["Sex"];
            }
            return View();
        }
        /// <summary>
        /// 我的考试
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberExam()
        {
            ViewBag.image = LgUser.image;
            ViewBag.bank = LgUser.Bank;
            MessasgeData mgdata = new MessasgeData();
             mgdata = Datafun.MgfunctionData(@"select * from tb_login where  id=@id",new SqlParameter("@id",LgUser.UserId));
             string exam = "0";
            if (mgdata.Mgdatacount > 0)
            {
                 exam = mgdata.Mgdata.Rows[0]["exam"].ToString().Substring(0, mgdata.Mgdata.Rows[0]["exam"].ToString().Length - 1);
            }
            mgdata = Datafun.MgfunctionData(@"select * from tb_examlist where  id in ("+exam+")");
            ViewBag.table = mgdata.Mgdata;
            return View();
        }
        /// <summary>
        /// 我的错题
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberWrong()
        {
            ViewBag.image = LgUser.image;
            ViewBag.bank = LgUser.Bank;
            MessasgeData mgdata = new MessasgeData();
            mgdata = Datafun.MgfunctionData(@"select * from tb_login where  id=@id", new SqlParameter("@id", LgUser.UserId));
            string exam = "0";
            if (mgdata.Mgdatacount > 0)
            {
                exam = mgdata.Mgdata.Rows[0]["Wrong"].ToString().Substring(0, mgdata.Mgdata.Rows[0]["Wrong"].ToString().Length - 1);
            }
            mgdata = Datafun.MgfunctionData(@"select * from tb_Examinfor where  id in (" + exam + ")");
            ViewBag.table = mgdata.Mgdata;
            return View();
        }
        /// <summary>
        /// 我的错题
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberCollect()
        {
            ViewBag.image = LgUser.image;
            return View();
        }
        #region 动作
        public string MembersetAjax()
        {
            string image = Request["imgurl"];
            string bank = Request["bank"];
            string Email = Request["Email"];
            string sex = Request["sex"];
            string IsSuccess = "1";
            SqlParameter[] pms = { 
                             new SqlParameter("@image",image),    
                                 new SqlParameter("@bank",bank),   
                                  new SqlParameter("@Email",Email),   
                                   new SqlParameter("@sex",sex),
                                    new SqlParameter("@id",LgUser.UserId)
                                 };
            MessasgeInfor mginfor = Datafun.Mgfunctioninfor("update tb_login set Bank=@bank,image=@image,Sex=@sex,Email=@Email where id=@id", pms);
            if (mginfor.Mgdatacount > 0)
            {
                IsSuccess = "0";
            }
            return IsSuccess;
        }
        public string PasswordAjax()
        {
            string newpassword = Request["newpassword"];
         
            string IsSuccess = "1";
            SqlParameter[] pms = { 
                           
                                   new SqlParameter("@newpassword",Passwd.SetPass(newpassword)),
                                    new SqlParameter("@id",LgUser.UserId)
                                 };
            MessasgeInfor mginfor = Datafun.Mgfunctioninfor("update tb_login set Password=@newpassword where id=@id", pms);
            if (mginfor.Mgdatacount > 0)
            {
                IsSuccess = "0";
            }
            return IsSuccess;
        }
        /// <summary>
        /// 我的考试删除
        /// </summary>
        /// <returns></returns>
        public string Deletes()
        {
            string id = Request["id"];
            string IsSuccess = "0";
            string sql = "";
            string exam = "";
            string ids = "";
            MessasgeInfor mginfor = new MessasgeInfor();
            MessasgeData mgdata = Datafun.MgfunctionData("select  exam from tb_login where id=@id", new SqlParameter("@id",LgUser.UserId));
            if (mgdata.Mgdatacount > 0)
            {
                exam = mgdata.Mgdata.Rows[0]["exam"].ToString();
            }
           
            string[] eid = exam.Substring(0,exam.Length-1).Split(',');
            
            foreach (string str in eid)
            {
                if (str != id)
                {
                    ids += str+",";
                }
            }

            SqlParameter[] pms = { 
                                 new SqlParameter("@id",LgUser.UserId),
                                  new SqlParameter("@ids",ids)
                                 };
            mginfor = Datafun.Mgfunctioninfor("update tb_login set exam=@ids where id=@id",pms);
            if (mginfor.Mgdatacount > 0)
            {
                IsSuccess = "0";
            }
            else
            {

                IsSuccess = "1";
            }
            return IsSuccess;
        }
        /// <summary>
        /// 我的错题删除
        /// </summary>
        /// <returns></returns>
        public string DeletesWrong()
        {
            string id = Request["id"];
            string IsSuccess = "0";
            string sql = "";
            string exam = "";
            string ids = "";
            MessasgeInfor mginfor = new MessasgeInfor();
            MessasgeData mgdata = Datafun.MgfunctionData("select  Wrong from tb_login where id=@id", new SqlParameter("@id", LgUser.UserId));
            if (mgdata.Mgdatacount > 0)
            {
                exam = mgdata.Mgdata.Rows[0]["Wrong"].ToString();
            }

            string[] eid = exam.Substring(0, exam.Length - 1).Split(',');

            foreach (string str in eid)
            {
                if (str != id)
                {
                    ids += str + ",";
                }
            }

            SqlParameter[] pms = { 
                                 new SqlParameter("@id",LgUser.UserId),
                                  new SqlParameter("@ids",ids)
                                 };
            mginfor = Datafun.Mgfunctioninfor("update tb_login set Wrong=@ids where id=@id", pms);
            if (mginfor.Mgdatacount > 0)
            {
                IsSuccess = "0";
            }
            else
            {

                IsSuccess = "1";
            }
            return IsSuccess;
        }
        #endregion
    }
}
