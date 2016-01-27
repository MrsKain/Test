using Kain_class.Messageinfor;
using Kain_class.Pass;
using Peizhi.Controllers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Peizhi.Areas.Areas.Controllers
{
    public class MemberController : MainsController
    {
        //
        // GET: /Areas/Member/

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
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public ActionResult Updatepass()
        {
            return View();
        }

        /// <summary>
        /// 修改密码动作
        /// </summary>
        /// <returns></returns>
        public string PassAjax()
        {
            string IsSuccess = "0";
            string newp = Request["newp"];
            string pass = Request["pass"];


            MessasgeInfor mginfor = new MessasgeInfor();
            SqlParameter[] pm ={                            
                           
                              new SqlParameter("@newp",Passwd.SetPass(newp)),
                               new SqlParameter("@pass",Passwd.SetPass(pass)),
                               new SqlParameter("@UserId",LgUser.UserId)
                               
                               
                              };

            string sql = @"IF  EXISTS( select * from tb_login where id=@UserId and Password=@pass ) begin update tb_login set Password=@newp where id=@UserId end";
            mginfor = Datafun.Mgfunctioninfor(sql, pm);
            if (mginfor.Mgdatacount == 1)
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
        /// 退出清空缓存
        /// </summary>
        /// <returns></returns>
        public ActionResult ExtOutAction()
        {
            Session.RemoveAll();
            return RedirectToAction("Login", "Login");
        }
        /// <summary>
        /// 我的考试
        /// </summary>
        /// <returns></returns>
        public ActionResult Exam()
        {
            ViewBag.image = LgUser.image;
            ViewBag.bank = LgUser.Bank;
            MessasgeData mgdata = new MessasgeData();
            mgdata = Datafun.MgfunctionData(@"select * from tb_login where  id=@id", new SqlParameter("@id", LgUser.UserId));
            string exam = "0";
            if (mgdata.Mgdatacount > 0)
            {
                exam = mgdata.Mgdata.Rows[0]["exam"].ToString().Substring(0, mgdata.Mgdata.Rows[0]["exam"].ToString().Length - 1);
            }
            mgdata = Datafun.MgfunctionData(@"select * from tb_examlist where  id in (" + exam + ")");
            ViewBag.table = mgdata.Mgdata;
            return View();
        }
    }
}
