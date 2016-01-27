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
    public class LoginController : MainsController
    {
        //
        // GET: /Areas/Login/
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        public ActionResult Regist()
        {
            return View();
        }
        /// <summary>
        /// 找回密码
        /// </summary>
        /// <returns></returns>
        public ActionResult PasswordBack()
        {
            return View();
        }
        /// <summary>
        /// 找回密码动作
        /// </summary>
        /// <returns></returns>
        public string PasswordBackAjax()
        {
            string IsSuccess = "0";
            string PassWord = Request["PassWord"];
            string tel = Request["tel"];
        


            MessasgeInfor mginfor = new MessasgeInfor();
            SqlParameter[] pm ={                            
                              
                              new SqlParameter("@PassWord",Passwd.SetPass(PassWord)),                           
                                new SqlParameter("@tel",tel)
                               
                              };

            string sql = @"update tb_login set Password=@PassWord where UserName=@tel";
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
    }
}
