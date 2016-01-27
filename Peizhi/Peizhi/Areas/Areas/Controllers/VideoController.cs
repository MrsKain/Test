using Kain_class.Messageinfor;
using Kain_class.SqlPager;
using Peizhi.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Peizhi.Areas.Areas.Controllers
{
    public class VideoController : MainsController
    {
        //
        // GET: /Areas/Video/

        /// 视频列表
        /// </summary>
        /// <returns></returns>
        public ActionResult VideoList(string id="1")
        {
            ViewBag.id = id;
            string sql = @"select * from dbo.tb_CourseVideo  where isdel=0 and types in(select id from  tb_types where pid=@id)";

            MessasgeData mgdata = Datafun.MgfunctionData(sql,new SqlParameter("@id",id));
            ViewBag.table = mgdata.Mgdata;
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
            if (mgdataset.MgdataSet.Tables[0].Rows.Count > 0)
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
            ViewBag.table = mgdataset.MgdataSet.Tables[1];
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
                return RedirectToAction("jpmember", "Home");
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
    }
}
