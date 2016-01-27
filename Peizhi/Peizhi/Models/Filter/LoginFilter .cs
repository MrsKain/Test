using Peizhi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Peizhi.Models.Filter
{
    public class LoginFilter : ActionFilterAttribute, IActionFilter
    {
        /*Kain20150818登录拦截器，保留管理员登录接口Orderid！=1*/
        public int Orderid { get; set; }
        public string conn
        {
            get { return ConfigurationManager.AppSettings[_sqlcon]; }
            set { _sqlcon = value; }
        }

        private string _sqlcon = "sqlcon";

        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
              
            if (filterContext.HttpContext.Session["Lguser"] == null)
            {

                if (Orderid == 1)
                {
                    filterContext.Result = new ContentResult
                    {
                        Content = @"<script>window.top.location='/login/login'</script>"
                    };
                }
                else
                {
                    filterContext.Result = new RedirectResult("~/Adminlogin");//保留跳卖家端 Kain20150925
                }
            }
            else
            {

                string conname = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower();
                string actionname = filterContext.ActionDescriptor.ActionName.ToLower();


                LoginUser LgUser = filterContext.HttpContext.Session["Lguser"] as LoginUser;

                if (LgUser.UserLvl > 1)
                {
                    bool blqx = false;
                    if (LgUser.ListQxM != null)
                    {
                        foreach (string item in LgUser.ListQxM)
                        {
                            string[] items = item.Split('/');

                            if (items.Length <= 2)
                            {

                                if (items[1].ToLower() == conname.ToLower() && actionname.ToLower() == "index".ToLower())
                                {
                                    blqx = true;
                                    break;
                                }
                                else
                                {
                                    blqx = false;
                                }

                            }
                            else
                            {
                                if (items[1].ToLower() == conname.ToLower() &&
                                    items[2].ToLower() == actionname.ToLower())
                                {
                                    blqx = true;
                                    break;
                                }
                                else
                                {
                                    blqx = false;
                                }
                            }


                        }

                    }

                    if (LgUser.ListQx != null && blqx)
                    {

                        foreach (string item in LgUser.ListQx)
                        {
                            string[] items = item.Split('/');

                            if (items.Length <= 2)
                            {

                                if (items[1].ToLower() == conname.ToLower())
                                {
                                    blqx = true;
                                    break;
                                }
                            }
                            else
                            {

                                if (items[1].ToLower() == conname.ToLower() && items[2].ToLower() == actionname.ToLower())
                                {
                                    blqx = true;
                                    break;
                                }
                                else
                                {
                                    blqx = false;

                                }

                            }
                        }
                        if (!blqx)
                        {
                            filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { Controller = "qx", Action = "index" }));

                        }

                    }

                }
                else
                {
                    //  filterContext.Result = null;
                }

            }

        }


    }
}