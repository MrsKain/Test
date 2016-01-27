using Kain_class.Json;
using Kain_class.Messageinfor;
using Peizhi.Controllers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Peizhi.Areas.Areas.Controllers
{
    public class HomeController : MainsController
    {
        //
        // GET: /Areas/Home/
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            MessasgeDataSet mgdataset = Datafun.MgfunctionDataSet(@"select top 2 * from dbo.tb_CourseVideo where isdel=0 order by NEWID();
                                                                  select  top 6 id, title,CONVERT(nvarchar(11),Add_Date,120) as Add_Date,Contents from tb_news  where isdel=0  order by id desc;;
                                                                   select  top 7 * from tb_examlist  where isdel=0 and Types in(select id from tb_types where pid=4) order by id desc;
                                                                   select  top 7 * from tb_CourseVideo  where isdel=0 and Types in(select id from tb_types where pid=4)  order by id desc;
                                                                    select * from tb_baner where isdel=0;");
            ViewBag.table1 = mgdataset.MgdataSet.Tables[0];
            ViewBag.table2 = mgdataset.MgdataSet.Tables[1];
            ViewBag.table3 = mgdataset.MgdataSet.Tables[2];
            ViewBag.table4 = mgdataset.MgdataSet.Tables[3];
            ViewBag.table5 = mgdataset.MgdataSet.Tables[4];
            return View();
        }
        /// <summary>
        /// 头部
        /// </summary>
        /// <returns></returns>
        public PartialViewResult Head(string id="1")
        {
            ViewBag.ids = id;
            ViewBag.uid = "";
            ViewBag.bank = "";
            if (LgUser != null)
            {
                ViewBag.uid = LgUser.UserId;
                ViewBag.bank = LgUser.Bank;
            }
            return PartialView();
        }
        /// <summary>
        /// 栏目分类选择
        /// </summary>
        /// <returns></returns>
        public PartialViewResult LeftMenu()
        {
            MessasgeData mgdata = Datafun.MgfunctionData("select * from tb_types where isdel=0 and lvl=1");
            ViewBag.table = mgdata.Mgdata;
            return PartialView();
        }
        /// <summary>
        /// 关于我们
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            MessasgeData mgdata = Datafun.MgfunctionData("select * from tb_about where types='关于我们'");
            ViewBag.about = mgdata.Mgdata.Rows[0]["Contents"];
            ViewBag.types = mgdata.Mgdata.Rows[0]["types"];
            return View();
        }
        /// <summary>
        /// 认证中心
        /// </summary>
        /// <returns></returns>
        public ActionResult Authentication()
        {
            MessasgeData mgdata = Datafun.MgfunctionData("select * from tb_about where types='教师资格证'");
            ViewBag.about = mgdata.Mgdata.Rows[0]["Contents"];
            ViewBag.types = mgdata.Mgdata.Rows[0]["types"];
            return View();
        }
        /// <summary>
        /// 新闻详情
        /// </summary>
        /// <returns></returns>
        public ActionResult Newslist(string id="1")
        {
            ViewBag.id = id;
            string sql = @"select id,title,Contents,images,CONVERT(nvarchar(11),Add_Date,120) as Add_Date from tb_news where isdel=0 and types="+id+"";
            MessasgeData mgdata = Datafun.MgfunctionData(sql);
            ViewBag.table = mgdata.Mgdata;
            return View();
        }
        /// <summary>
        /// 新闻详细
        /// </summary>
        /// <returns></returns>
        public ActionResult NewsInfor(string id)
        {
            MessasgeData mgdata = Datafun.MgfunctionData("select Contents,title,CONVERT(nvarchar(11),Add_Date,120) as Add_Date from tb_news where id=@id", new SqlParameter("@id", id));
            ViewBag.Contents = "";
            ViewBag.title = "";
            ViewBag.date = "";
            if (mgdata.Mgdatacount > 0)
            {
                ViewBag.Contents = mgdata.Mgdata.Rows[0]["Contents"];
                ViewBag.title = mgdata.Mgdata.Rows[0]["title"];
                ViewBag.date = mgdata.Mgdata.Rows[0]["Add_Date"];

            }
            return View();
        }
        /// <summary>
        /// 资料下载
        /// </summary>
        /// <returns></returns>
        public ActionResult DataLoad(string id="1")
        {

            string sql = @"select * from tb_DataLoad where isdel=0  and Types in(select id from tb_types where pid="+id+") ";

            MessasgeData mgdata = Datafun.MgfunctionData(sql);
            ViewBag.table = mgdata.Mgdata;

            ViewBag.id = id;
            return View();
        }

        /// <summary>
        /// 类别
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Gettypes()
        {
            string ReturnValue = string.Empty;
            MessasgeData mgdata = Datafun.MgfunctionData(" select id,title from tb_types where lvl=1");

            ReturnValue = Jsonzs.DataTableToJson(mgdata.Mgdata, '"');
            return ContentJson(ReturnValue);

        }
        /// <summary>
        /// 学科
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Getsubject(string id)
        {
            string ReturnValue = string.Empty;
            MessasgeData mgdata = Datafun.MgfunctionData("  select  id,title from tb_types where pid="+id+"");

            ReturnValue = Jsonzs.DataTableToJson(mgdata.Mgdata, '"');
            return ContentJson(ReturnValue);

        }

    }
}
