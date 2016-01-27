using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using Kain_class.Messageinfor;
using Kain_class.SqlPager;
using System.Data;
using System.Text;


namespace Peizhi.Controllers
{
    public class PCController : MainsController
    {
        //
        // GET: /PC/
        /// <summary>
        /// 会员管理
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberIndex(string id,string ids)
        {
           
            int pageIndex = Convert.ToInt32(id ?? "0");
            string sql = @"select id, UserName,Add_Date,Bank,Sex ,Email,lvl,case lvl when '1' then '普通会员' else '金牌会员' end as added1   from tb_login where Lvl<>0 and isdel=0";
            if (ids != null && ids != "")
            {
                sql += " and UserName like '%" + ids + "%' ";
            }
            MessasgeData mgdata = SqlPage.SqlPageAction(Datafun, sql, "id desc", pageIndex, 15);
            ViewBag.recordCount = mgdata.Mgdatacount;
            ViewBag.ids = ids;
            ViewBag.pageindex = id ?? "0";
            ViewBag.table = mgdata.Mgdata;
            return View();
        }
        /// <summary>
        /// 认证中心
        /// </summary>
        /// <returns></returns>
        public ActionResult Authentication(string id = "教师资格证")
        {
            MessasgeData mgdata = Datafun.MgfunctionData("select * from tb_about where types=@types",new SqlParameter("@types",id));
            ViewBag.Contents = mgdata.Mgdata.Rows[0]["Contents"];
            return View();
        }
        /// <summary>
        /// 关于我们
        /// </summary>
        /// <returns></returns>
        public ActionResult About(string id = "关于我们")
        {
            MessasgeData mgdata = Datafun.MgfunctionData("select * from tb_about where types=@types", new SqlParameter("@types", id));
            ViewBag.Contents = mgdata.Mgdata.Rows[0]["Contents"];
            return View();
        }
        /// <summary>
        /// 页面优化
        /// </summary>
        /// <returns></returns>
        public ActionResult SEO()
        {
            MessasgeData mgdata = Datafun.MgfunctionData("select Contents from tb_SEO where isdel=0");
            ViewBag.Contents = mgdata.Mgdata.Rows[0]["Contents"];
            return View();
        }
        /// <summary>
        /// 密码修改
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdatePssword()
        {
           
            return View();
        }
        /// <summary>
        /// 底部信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Foot()
        {
            MessasgeData mgdata = Datafun.MgfunctionData("select * from tb_foot");
            ViewBag.copyright = mgdata.Mgdata.Rows[0]["CopyRight"];
            ViewBag.Tel = mgdata.Mgdata.Rows[0]["Tel"];
            ViewBag.Address = mgdata.Mgdata.Rows[0]["Address"];
            return View();
        }
        /// <summary>
        /// 首页baner展示图
        /// </summary>
        /// <returns></returns>
        public ActionResult Baner()
        {
            MessasgeData mgdata = Datafun.MgfunctionData("select * from tb_baner where isdel=0");
            ViewBag.table = mgdata.Mgdata;
            return View();
        }
        /// <summary>
        /// 报名咨询
        /// </summary>
        /// <returns></returns>
        public ActionResult Bmzx()
        {
            MessasgeData mgdata = Datafun.MgfunctionData("select top 1 * from tb_bmzx where isdel=0");
            if (mgdata.Mgdatacount > 0)
            {
                ViewBag.Tel = mgdata.Mgdata.Rows[0]["Tel"];
                ViewBag.QQone = mgdata.Mgdata.Rows[0]["QQone"];
                ViewBag.QQtwo = mgdata.Mgdata.Rows[0]["QQtwo"];
                ViewBag.Img = mgdata.Mgdata.Rows[0]["Img"];
            }
            return View();
        }
        /// <summary>
        /// 试卷管理
        /// </summary>
        /// <returns></returns>
        public ActionResult ExamList()
        {
            return View();
        }
        /// <summary>
        /// 新闻管理
        /// </summary>
        /// <returns></returns>
        public ActionResult NewsManager(string id,string ids)
        {
            int pageIndex = Convert.ToInt32(id ?? "0");
            string sql = @"select id,title,Add_Date,case types when '1' then '新闻咨询' else '重要通知' end as added1 from dbo.tb_news where isdel=0 ";
            if (ids != null && ids != "")
            {
                sql += " and title like '%" + ids + "%' ";
            }
            MessasgeData mgdata = SqlPage.SqlPageAction(Datafun, sql, "id desc", pageIndex, 15);
            ViewBag.recordCount = mgdata.Mgdatacount;
            ViewBag.ids = ids;
            ViewBag.pageindex = id ?? "0";
            ViewBag.table = mgdata.Mgdata;
            return View();
        }
        /// <summary>
        /// 添加新闻
        /// </summary>
        /// <returns></returns>
        public ActionResult AddNews()
        {
            return View();
        }
        /// <summary>
        /// 资料下载管理
        /// </summary>
        /// <returns></returns>
        public ActionResult DataLoadManager(string id, string ids)
        {
            int pageIndex = Convert.ToInt32(id ?? "0");
            string sql = @"select a.id, a.title,dataurl,Add_Date,b.title as added1 from tb_Dataload a  left join tb_types b on a.Types=b.id where a.isdel=0 ";
            if (ids != null && ids != "")
            {
                sql += " and a.title like '%" + ids + "%' ";
            }
            MessasgeData mgdata = SqlPage.SqlPageAction(Datafun, sql, "a.id desc", pageIndex, 15);
            ViewBag.recordCount = mgdata.Mgdatacount;
            ViewBag.ids = ids;
            ViewBag.pageindex = id ?? "0";
            ViewBag.table = mgdata.Mgdata;
            return View();
        }
        /// <summary>
        /// 资料上传
        /// </summary>
        /// <returns></returns>
        public ActionResult DataUpload()
        {
            return View();
        }
        /// <summary>
        /// 微课管理
        /// </summary>
        /// <returns></returns>
        public ActionResult VideoManager(string id, string ids)
        {
            int pageIndex = Convert.ToInt32(id ?? "0");
            string sql = @"select a.id, b.title as typesname,a.title,counts,sumtime,tearcher,a.introduce, Add_Date from dbo.tb_CourseVideo a left join tb_types b  on a.Types=b.id  where a.isdel=0";
            if (ids != null && ids != "")
            {
                sql += " and a.title like '%" + ids + "%' ";
            }
            MessasgeData mgdata = SqlPage.SqlPageAction(Datafun, sql, "a.id desc", pageIndex, 15);
            ViewBag.recordCount = mgdata.Mgdatacount;
            ViewBag.ids = ids;
            ViewBag.pageindex = id ?? "0";
            ViewBag.table = mgdata.Mgdata;
            return View();
        }
        /// <summary>
        /// 微课添加
        /// </summary>
        /// <returns></returns>
        public ActionResult AddVideo()
        {
            return View();
        }
        /// <summary>
        /// 微课添加
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadVideo(string id)
        {
            if (id == null)
            {
                return RedirectToAction("VideoManager","pc");
            }
            ViewBag.id = id;
            return View();
        }
        /// <summary>
        /// 试卷管理
        /// </summary>
        /// <returns></returns>
        public ActionResult ExamManager(string id, string ids)
        {
            int pageIndex = Convert.ToInt32(id ?? "0");
            string sql = @"select a.id, b.title as typesname,a.title, Add_Date from dbo.tb_Examlist a left join tb_types b  on a.Types=b.id  where a.isdel=0";
            if (ids != null && ids != "")
            {
                sql += " and a.title like '%" + ids + "%' ";
            }
            MessasgeData mgdata = SqlPage.SqlPageAction(Datafun, sql, "a.id desc", pageIndex, 15);
            ViewBag.recordCount = mgdata.Mgdatacount;
            ViewBag.ids = ids;
            ViewBag.pageindex = id ?? "0";
            ViewBag.table = mgdata.Mgdata;
            return View();
        }
        /// <summary>
        /// 题目管理
        /// </summary>
        /// <returns></returns>
        public ActionResult ExamInforManager(string id, string ids)
        {
            int pageIndex = Convert.ToInt32(id ?? "0");
            string sql = @"select a.id,Question,A,B,C,D,Answer,Analysis,Add_Date,case types when '1' then '选择题' when '2' then '判断题' else '材料分析题' end as titletype,
              trades , b.title from tb_Examinfor  a left join tb_types b on a.Types=b.id where a.isdel=0";
            if (ids != null && ids != "")
            {
                sql += " and a.Question like '%" + ids + "%' ";
            }
            MessasgeData mgdata = SqlPage.SqlPageAction(Datafun, sql, "a.id desc", pageIndex, 15);
            ViewBag.recordCount = mgdata.Mgdatacount;
            ViewBag.ids = ids;
            ViewBag.pageindex = id ?? "0";
            ViewBag.table = mgdata.Mgdata;
            return View();
        }
        /// <summary>
        ///试卷添加
        /// </summary>
        /// <returns></returns>
        public ActionResult AddExam()
        {
            return View();
        }
        /// <summary>
        ///题目添加
        /// </summary>
        /// <returns></returns>
        public ActionResult AddQuestion(string id)
        {
            if (id == null)
            {
                return RedirectToAction("ExamManager", "pc");
            }
            ViewBag.id = id;
            return View();
        }
        /// <summary>
        /// 课时视频管理
        /// </summary>
        /// <returns></returns>
        public ActionResult VideoInforManager(string id,string ids)
        {
            int pageIndex = Convert.ToInt32(id ?? "0");
            string sql = @"select a.id, b.title as sskc,a.title,a.Add_Date,brief,a.introduce,a.videourl from dbo.tb_CourseVideoInfor a left join  tb_CourseVideo b on a.courseid=b.id where a.isdel=0";
            if (ids != null && ids != "")
            {
                sql += " and a.title like '%" + ids + "%' ";
            }
            MessasgeData mgdata = SqlPage.SqlPageAction(Datafun, sql, "a.id desc", pageIndex, 15);
            ViewBag.recordCount = mgdata.Mgdatacount;
            ViewBag.ids = ids;
            ViewBag.pageindex = id ?? "0";
            ViewBag.table = mgdata.Mgdata;
            return View();
        }
        /// <summary>
        /// 修改课时
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateVideoInfor(string id)
        {
            if (id == null)
            {
                return RedirectToAction("VideoInforManager", "PC");
            }
            ViewBag.id=id;
            MessasgeData mgdata = Datafun.MgfunctionData("select * from tb_CourseVideoInfor where id=@id", new SqlParameter("@id", id));
            if (mgdata.Mgdatacount > 0)
            {
                ViewBag.titles = mgdata.Mgdata.Rows[0]["title"];
                ViewBag.brief = mgdata.Mgdata.Rows[0]["brief"];
                ViewBag.introduce = mgdata.Mgdata.Rows[0]["introduce"];
            }
            return View();
        }
        /// <summary>
        /// 类别设置
        /// </summary>
        /// <returns></returns>
        public ActionResult Types()
        {
            ViewBag.html = TreeData();
            return View();
        }

        /// <summary>
        /// 修改视频
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateVideo(string id)
        {
            MessasgeData mgdata = Datafun.MgfunctionData("select * from dbo.tb_CourseVideoInfor where id=@id",new SqlParameter("@id",id));
            ViewBag.table = mgdata.Mgdata;
            ViewBag.id = id;
            return View();
        }
        /// <summary>
        /// 修改题目
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateExamInfor(string id)
        {
            ViewBag.types = "";
            MessasgeData mgdata = Datafun.MgfunctionData("select * from tb_Examinfor where id=@id",new SqlParameter("@id",id));
            ViewBag.table = mgdata.Mgdata;
            if (mgdata.Mgdatacount > 0)
            {
                ViewBag.types = mgdata.Mgdata.Rows[0]["Types"];
            }
            ViewBag.id = id;
            return View();
        }
        #region 树
        /// <summary>
        /// 树控件加载数据
        /// </summary>
        /// <returns></returns>
        public string TreeData()
        {
            StringBuilder sc = new StringBuilder();
            string so = "";
            string sa = @"<div class='tree-folder' style='display: block;'>
                                            <div class='tree-folder-header'>
                                                <i class='fa fa-folder'></i>
                                                <div class='tree-folder-name'>{$title$}<div class='tree-actions'><i class='fa fa-plus' onclick=add({$id$},'{$title$}')></i><i class='fa fa-trash-o' onclick='deleted({$id$})'></i><i class='fa fa-rotate-right'></i></div></div>
                                            </div>
                                            <div class='tree-folder-content'>   
                                             {$content$}                                                                                           
                                            </div>
                                            <div class='tree-loader' style='display: none;'><div class='tree-loading'><i class='fa fa-rotate-right fa-spin'></i></div></div>
                                        </div>";
            MessasgeData mgdata = Datafun.MgfunctionData("select * from tb_types where lvl=1 and isdel=0");
            foreach (DataRow dr in mgdata.Mgdata.Rows)
            {
                so = sa.Replace("{$title$}", dr["title"].ToString()).Replace("{$id$}", dr["id"].ToString());
                string sb = TreeChildern(dr["id"].ToString());
                so = so.Replace("{$content$}", sb);
                sc.Append(so);
            }
            return sc.ToString();
        }
        /// <summary>
        /// 树子级别
        /// </summary>
        /// <returns></returns>
        public string TreeChildern(string id)
        {
            StringBuilder sc = new StringBuilder();
            string sv = "";
            string sd = @"<div class='tree-item' style='display: block;'>
                                            <i class='tree-dot'></i>
                                             <div class='tree-item-name'><i class='fa fa-book'></i>{$title$} <div class='tree-actions'></div></div>
                                        </div>";
            MessasgeData mgdata = Datafun.MgfunctionData("select * from tb_types where pid=@id and isdel=0", new SqlParameter("@id", id));
            foreach (DataRow dr in mgdata.Mgdata.Rows)
            {
                sv = sd.Replace("{$title$}", dr["title"].ToString());
                sc.Append(sv);
            }
            return sc.ToString();
        }
        #endregion
    }
}
