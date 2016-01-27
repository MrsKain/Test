using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using Kain_class.Messageinfor;
using Kain_class.SqlPager;
using System.Linq.Expressions;
using System.Data;

namespace Peizhi.Controllers
{
    public class VideoController : MainsController
    {
        //
        // GET: /Video/
        /// <summary>
        /// 视频列表
        /// </summary>
        /// <returns></returns>
        public ActionResult VideoList(string id,string ids)
        {
            int pageIndex = Convert.ToInt32(id ?? "0");
            string sql = @"select * from dbo.tb_CourseVideo  where isdel=0";
            if (ids != null)
            {
                sql += @" and  types='" + ids + "'";
            }
            MessasgeData mgdata = SqlPage.SqlPageAction(Datafun, sql, "id desc", pageIndex, 6);
            ViewBag.table = mgdata.Mgdata;

            ViewBag.recordCount = mgdata.Mgdatacount;
            ViewBag.pageindex = id ?? "0";
            ViewBag.ids = ids;
            return View();
        }
        /// <summary>
        /// 详情页
        /// </summary>
        /// <returns></returns>
        public ActionResult VideoInfor(string id)
        {
            ViewBag.id = "";
            ViewBag.image = "";
            ViewBag.title = "";
            ViewBag.introduce = "";
            ViewBag.counts = "";
            ViewBag.sumtime = "";
            ViewBag.tearcher = "";
            MessasgeDataSet mgdataset = Datafun.MgfunctionDataSet(@"select * from dbo.tb_CourseVideo where id=@id;select id,videourl,title from tb_CourseVideoInfor where courseid=@id;
select Comment ,a.Add_Date,Bank,image from tb_Comment a left join tb_login b on a.Userid=b.id  where types=@id order by a.id desc", new SqlParameter("@id", id));
            if (mgdataset.MgdataSet.Tables[0].Rows.Count>0)
            {
                ViewBag.id = mgdataset.MgdataSet.Tables[0].Rows[0]["id"];
                ViewBag.image = mgdataset.MgdataSet.Tables[0].Rows[0]["images"];
                ViewBag.title = mgdataset.MgdataSet.Tables[0].Rows[0]["title"];
                ViewBag.introduce = mgdataset.MgdataSet.Tables[0].Rows[0]["introduce"];
                ViewBag.counts = mgdataset.MgdataSet.Tables[0].Rows[0]["counts"];
                ViewBag.sumtime = mgdataset.MgdataSet.Tables[0].Rows[0]["sumtime"];
                ViewBag.teacher = mgdataset.MgdataSet.Tables[0].Rows[0]["tearcher"];
            }
            ViewBag.ids = id;
            ViewBag.table=mgdataset.MgdataSet.Tables[1];
            ViewBag.Comment = mgdataset.MgdataSet.Tables[2];
            return View();
        }
        /// <summary>
        /// 视频播放
        /// </summary>
        /// <returns></returns>
        public ActionResult VideoPlay(string id)
        {

            if (LgUser == null)
            {
                return RedirectToAction("jpmember","Home");
            }
            if (LgUser.UserLvl == 1)
            {
                return RedirectToAction("jpmember", "Home");
            }
            MessasgeDataSet mgdataset = Datafun.MgfunctionDataSet("select * from  tb_CourseVideoInfor where courseid =(select courseid from tb_CourseVideoInfor where id=@id);select * from  tb_CourseVideo where id =(select courseid from tb_CourseVideoInfor where id=@id)", new SqlParameter("@id", id));
            ViewBag.table = mgdataset.MgdataSet.Tables[0];

            DataRow[] dr = mgdataset.MgdataSet.Tables[0].Select("id='" + id + "'");
            ViewBag.brief = dr[0]["brief"].ToString();
            ViewBag.introduce = dr[0]["introduce"].ToString();
            ViewBag.id = id;
            ViewBag.lvl = "";
            ViewBag.image = mgdataset.MgdataSet.Tables[1].Rows[0]["images"];
            ViewBag.title = mgdataset.MgdataSet.Tables[1].Rows[0]["title"];
            ViewBag.introduce = mgdataset.MgdataSet.Tables[1].Rows[0]["introduce"];
            ViewBag.counts = mgdataset.MgdataSet.Tables[1].Rows[0]["counts"];
            if (LgUser != null)
            {
                ViewBag.lvl = LgUser.UserLvl;
            }
            return View();
        }
        /// <summary>
        /// 添加视频评论
        /// </summary>
        /// <returns></returns>
        public string AddCommentAjax()
        {

            string id = Request["id"];
            string Comment = Request["Comment"];
          
            string IsSuccess = "1";
            SqlParameter[] pms = {                  
                                 new SqlParameter("@id",id),
                                  new SqlParameter("@Comment",Comment),
                                   new SqlParameter("@Userid",LgUser.UserId)  
                                 };
            MessasgeInfor mginfor = Datafun.Mgfunctioninfor(@"insert into tb_Comment(Userid,types,Comment)values (@Userid,@id,@Comment)", pms);
            if (mginfor.Mgdatacount > 0)
            {
                IsSuccess = "0";
            }

            return IsSuccess;
        }

    }
}
