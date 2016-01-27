using Peizhi.Models;
using Kain_class.Messageinfor;
using Kain_class.Pass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using peizhi.Models.Columns;

namespace Peizhi.Controllers
{
    public class LoginController : MainsController
    {
        //
        // GET: /Login/
        private static string staticYzm = "kainjie";
        #region 视图
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
        public ActionResult LoginAdmin()
        {
            return View();
        }
        #endregion
        #region 动作
        [HttpPost]
        public string LoginActionUser()
        {
            string username = Request["va1"];
            string password = Request["va2"];
            string IsSuccess = "0";
            string time = DateTime.Now.ToString();
            string sql = @"select * from tb_login where UserName=@UserName and Password=@Password and isdel=0";
            SqlParameter[] pms = { 
                                        new SqlParameter("@UserName",username), 
                                       
                                         new SqlParameter("@Password",Passwd.SetPass(password))
                                        
                                        };
            MessasgeData mgdata = Datafun.MgfunctionData(sql, pms);

            if (mgdata.Mgdatacount > 0)
            {
                DataRow dr = mgdata.Mgdata.Rows[0];
                LoginUser lguser = new LoginUser
                {
                    UserId = dr["id"].ToString(),
                    UserName = dr["UserName"].ToString(),
                    UserPwd = dr["Password"].ToString(),
                    UserLvl = int.Parse(dr["Lvl"].ToString()),
                    Bank = dr["Bank"].ToString(),
                    image=dr["image"].ToString()         
                };
                Session["Lguser"] = lguser;


                Response.Cookies["loginname"].Value = Passwd.SetPass(username);
                Response.Cookies["loginpwd"].Value = password;
                Response.Cookies["loginname"].Expires = DateTime.Now.AddDays(10);
                Response.Cookies["loginpwd"].Expires = DateTime.Now.AddDays(10);

                mgdata.Mgcontent = "登陆成功";

                IsSuccess = "0";
            }
            else
            {
                mgdata.Mgcontent = "用户名密码错误";
                IsSuccess = "1";
            }
            return IsSuccess;
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        public string RegistAjax()
        {
            string IsSuccess = "0";
            string bank = Request["bank"];
            string sex = Request["sex"];
            string PassWord = Request["PassWord"];
            string Email = Request["Email"];
            string tel = Request["tel"];
          
          
            MessasgeInfor mginfor = new MessasgeInfor();
            SqlParameter[] pm ={                            
                              new SqlParameter("@bank",bank),
                              new SqlParameter("@PassWord",Passwd.SetPass(PassWord)),
                              new SqlParameter("@sex",sex),
                               new SqlParameter("@Email",Email),
                                new SqlParameter("@tel",tel)
                               
                              };

            string sql = @"IF not EXISTS( select * from tb_login where UserName=@tel ) begin insert into tb_login (Bank,PassWord,Sex,Email,UserName,Lvl,Wrong,exam) values(@bank,@PassWord,@sex,@Email,@tel,1,'0,','0,') end";
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
        /// 手机发送验证码
        /// </summary>
        /// <returns></returns>
        public string TelYzm()
        {
            string IsSuccess = "0";
            string MobilePhone = Request["tel"];
            Random ran = new Random();
            int RandKey = ran.Next(100001, 999999);
            staticYzm = RandKey.ToString();
            send(MobilePhone, RandKey);
            return IsSuccess;
        }
        /// <summary>
        /// 手机发送验证码
        /// </summary>
        /// <returns></returns>
        public string YzmAjax()
        {
            string IsSuccess = "0";
            string yzm = Request["yzm"];
            if (staticYzm != yzm)
            {
                IsSuccess = "1";
            }
            return IsSuccess;
        }
        /// 验证验证码
        /// </summary>
        /// <returns></returns>
        public string NumsAjax()
        {
            string IsSuccess = "0";
            string YZM = System.Web.HttpUtility.UrlDecode(Request["nums"]).ToUpper();
            if (YZM != Session["code"].ToString().ToUpper() || YZM == null)
            {
                IsSuccess = "1";
            }
            return IsSuccess;
        }
        /// <summary>
        /// 验证码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult YzmAction()
        {
            string code = Yzmcode.RndNum(4);
            Session["code"] = code;
            byte[] bytes = Yzmcode.CreateImage(code);
            return File(bytes, @"image/jpeg");
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public string PasswordAjax()
        {
            string IsSuccess = "1";
            string password = Request["password"];
            string tel = Request["tel"];
            SqlParameter[] pms = { 
                                 new SqlParameter("@password",Passwd.SetPass(password)),
                                  new SqlParameter("@tel",tel)
                                 };
            MessasgeInfor mginfor = Datafun.Mgfunctioninfor("Update tb_login set Password=@password where UserName=@tel",pms);
            if (mginfor.Mgdatacount > 0)
            {
                IsSuccess = "0";
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
            return RedirectToAction("Login","Login");
        }
        #endregion

    }
}
