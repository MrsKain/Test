using Kain_class.Messageinfor;
using Kain_class.SqlPager;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Peizhi.Controllers
{
    public class HomeController : MainsController
    {
        //
        // GET: /Home/
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
        public ActionResult Newslist(string id)
        {
            int pageIndex = Convert.ToInt32(id ?? "0");
            string sql = @"select id,title,Contents,images,CONVERT(nvarchar(11),Add_Date,120) as Add_Date from tb_news where isdel=0 and types=1";
            MessasgeData mgdata = SqlPage.SqlPageAction(Datafun, sql, "id desc", pageIndex, 6);
            ViewBag.table = mgdata.Mgdata;
          
            ViewBag.recordCount = mgdata.Mgdatacount;
            ViewBag.pageindex = id ?? "0";
            ViewBag.ids ="";
            return View();
        }
        /// <summary>
        /// 重要通知
        /// </summary>
        /// <returns></returns>
        public ActionResult Notice(string id)
        {
            int pageIndex = Convert.ToInt32(id ?? "0");
            string sql = @"select id,title,Contents,images,CONVERT(nvarchar(11),Add_Date,120) as Add_Date from tb_news where isdel=0 and types=2";
            MessasgeData mgdata = SqlPage.SqlPageAction(Datafun, sql, "id desc", pageIndex, 6);
            ViewBag.table = mgdata.Mgdata;

            ViewBag.recordCount = mgdata.Mgdatacount;
            ViewBag.pageindex = id ?? "0";
            ViewBag.ids = "";
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
        public ActionResult DataLoad(string id,string ids)
        {
            int pageIndex = Convert.ToInt32(id ?? "0");
            string sql = @"select * from tb_DataLoad where isdel=0 ";
            if (ids != null)
            { 
            sql+=@" and Types='"+ids+"' ";
            }
            MessasgeData mgdata = SqlPage.SqlPageAction(Datafun, sql, "id desc", pageIndex, 8);
            ViewBag.table = mgdata.Mgdata;

            ViewBag.recordCount = mgdata.Mgdatacount;
            ViewBag.pageindex = id ?? "0";
            ViewBag.ids =ids;
            return View();
        }
        /// <summary>
        /// 提升为金牌会员
        /// </summary>
        /// <returns></returns>
        public ActionResult Jpmember()
        {
            return View();
        }
        /// <summary>
        /// 搜索结果
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchResult(string id)
        {
            ViewBag.id = id;
            if (id == null)
            {
                return RedirectToAction("SearchNoResult","Home");
            }
            MessasgeData mgdata = Datafun.MgfunctionData(@"select '微课中心' as types,title,id ,'1'as ts from tb_CourseVideo where title like '%" + id + "%'  union all  select '考试中心' as types,title,id ,'2'as ts from tb_Examlist where title like '%" + id + "%'");
            if (mgdata.Mgdatacount > 0)
            {
                ViewBag.table = mgdata.Mgdata;
            }
            else
            {
                return RedirectToAction("SearchNoResult", "Home");
            }
            return View();
        }
        /// <summary>
        /// 搜索无结果
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchNoResult()
        {
            return View();
        }
        /// <summary>
        /// 下载资料出现异常
        /// </summary>
        /// <returns></returns>
        public ActionResult catchs()
        {
            return View();
        }
    }
}
