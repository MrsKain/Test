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
    public class ExamController : MainsController
    {
        //
        // GET: /Exam/
        /// <summary>
        /// 考试中心
        /// </summary>
        /// <returns></returns>
        public ActionResult ExamIndex(string id,string ids)
        {
            int pageIndex = Convert.ToInt32(id ?? "0");
            string sql = @"select * from tb_examlist where isdel=0";
            if (ids != null)
            {
                sql += " and Types='"+ids+"' ";
            }
            MessasgeData mgdata = SqlPage.SqlPageAction(Datafun, sql, "id desc", pageIndex, 6);
            ViewBag.table = mgdata.Mgdata;

            ViewBag.recordCount = mgdata.Mgdatacount;
            ViewBag.pageindex = id ?? "0";
            ViewBag.ids = ids;
            return View();
        }
        /// <summary>
        /// 考试页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ExamInfor(string id)
        {
            if (id == null)
            {
                return RedirectToAction("ExamIndex","Exam");
            }
            if (LgUser == null)
            {
                return RedirectToAction("jpmember", "Home");
            }
            if (LgUser.UserLvl == 1)
            {
                return RedirectToAction("jpmember", "Home");
            }
            MessasgeDataSet mgdataset = Datafun.MgfunctionDataSet("select SUM(trades),COUNT(id) from tb_examinfor where ExamType=@id;select title from tb_Examlist where id=@id;select * from tb_examinfor where ExamType=@id and types=1;select * from tb_examinfor where ExamType=@id and types=2;select * from tb_examinfor where ExamType=@id and types=3", new SqlParameter("@id", id));
            ViewBag.grades = mgdataset.MgdataSet.Tables[0].Rows[0][0];
            ViewBag.counts = mgdataset.MgdataSet.Tables[0].Rows[0][1];
            ViewBag.title = mgdataset.MgdataSet.Tables[1].Rows[0][0];
            ViewBag.xzt = mgdataset.MgdataSet.Tables[2];
            ViewBag.pdt = mgdataset.MgdataSet.Tables[3];
            ViewBag.clt = mgdataset.MgdataSet.Tables[4];
            ViewBag.id = id;
            SqlParameter[] pms = {
                                 new SqlParameter("@exam",id+","),  
                                 new SqlParameter("@id",LgUser.UserId)
                                 };
            MessasgeInfor mginfor = Datafun.Mgfunctioninfor("Update tb_login set exam=(exam+@exam) where id=@id", pms);//将试卷写入tb_login
            return View();
        }
        /// <summary>
        /// 练习页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ExamTestInfor(string id)
        {
            if (id == null)
            {
                return RedirectToAction("ExamIndex", "Exam");
            }
            if (LgUser == null)
            {
                return RedirectToAction("jpmember", "Home");
            }
            if (LgUser.UserLvl == 1)
            {
                return RedirectToAction("jpmember", "Home");
            }
            MessasgeDataSet mgdataset = Datafun.MgfunctionDataSet("select SUM(trades),COUNT(id) from tb_examinfor where ExamType=@id;select title from tb_Examlist where id=@id;select * from tb_examinfor where ExamType=@id and types=1;select * from tb_examinfor where ExamType=@id and types=2;select * from tb_examinfor where ExamType=@id and types=3", new SqlParameter("@id", id));
            ViewBag.grades = mgdataset.MgdataSet.Tables[0].Rows[0][0];
            ViewBag.counts = mgdataset.MgdataSet.Tables[0].Rows[0][1];
            ViewBag.title = mgdataset.MgdataSet.Tables[1].Rows[0][0];
            ViewBag.xzt = mgdataset.MgdataSet.Tables[2];
            ViewBag.pdt = mgdataset.MgdataSet.Tables[3];
            ViewBag.clt = mgdataset.MgdataSet.Tables[4];
           
            return View();
        }
        /// <summary>
        /// 模拟练习
        /// </summary>
        /// <returns></returns>
        public ActionResult Examtest(string id,string ids)
        {
            int pageIndex = Convert.ToInt32(id ?? "0");
            string sql = @"select * from tb_examlist where isdel=0 and Types=" + ids + "";
            if (ids == null)
            {
                 sql = @"select * from tb_examlist where isdel=0";
            }

            MessasgeData mgdata = SqlPage.SqlPageAction(Datafun, sql, "id desc", pageIndex, 6);
            ViewBag.table = mgdata.Mgdata;
            ViewBag.id = ids;
            ViewBag.recordCount = mgdata.Mgdatacount;
            ViewBag.pageindex = id ?? "0";
            ViewBag.ids = ids;
            return View();
        }
        /// <summary>
        /// 综合练习
        /// </summary>
        /// <returns></returns>
        public ActionResult Examzhtest(string id)
        {
            if (LgUser == null)
            {
                return RedirectToAction("jpmember", "Home");
            }
            if (LgUser.UserLvl == 1)
            {
                return RedirectToAction("jpmember", "Home");
            }
            string sql = "select top 1 * from tb_examinfor where Types=1 order by NEWID()";
            if (id != null)
            {
                sql = "select top 1 * from tb_examinfor where ExamType in (select id from tb_Examlist where Types=" + id + " and isdel=0)";
            }

            MessasgeData mgdata = Datafun.MgfunctionData(sql);
            ViewBag.table = mgdata.Mgdata;
            ViewBag.id =1;
           
            return View();
        }
        /// <summary>
        /// 交卷 时候将错题写入错题库
        /// </summary>
        /// <returns></returns>
        public ActionResult ExamWrong(string id ,string ids)
        {
            string sql = "select *from tb_ExamInfor where id in(" + ids.Substring(0, ids.Length - 1)+ ")";
            MessasgeDataSet mgdataset = Datafun.MgfunctionDataSet("select * from tb_ExamInfor where types=3 and ExamType=@id;select *from tb_ExamInfor where id in(" + ids.Substring(0, ids.Length - 1) + ");  select   (select sum(trades)from tb_ExamInfor where ExamType=@id and Types <>3)-(select sum(trades)from tb_ExamInfor where id in(" + ids.Substring(0, ids.Length - 1) + "))", new SqlParameter("@id", id));
            ViewBag.zgt = mgdataset.MgdataSet.Tables[0];
         
            ViewBag.kgt = Kgt(mgdataset.MgdataSet.Tables[1]);
            ViewBag.trade = mgdataset.MgdataSet.Tables[2].Rows[0][0];
            SqlParameter[] pms = {
                                 new SqlParameter("@ids",ids),  
                                 new SqlParameter("@id",LgUser.UserId)
                                 };
            MessasgeInfor mginfor = Datafun.Mgfunctioninfor("Update tb_login set Wrong=(Wrong+@ids) where id=@id",pms);//将错题写入tb_login表
            return View();
        }

        /// <summary>
        /// 综合练习
        /// </summary>
        /// <returns></returns>
        public string ExamzhtestAjax()
        {
            string id = Request["id"];
            string ids = Request["ids"];
            int pageIndex = Convert.ToInt32(id ?? "0");
            string sql = "";
            if (ids != null)
            {
                sql = @"select  * from tb_examinfor where ExamType in (select id from tb_Examlist where Types=" + ids + " and isdel=0)";
            }
            else
            {
                sql = @"select  * from tb_examinfor where ExamType in (select id from tb_Examlist where isdel=0 and)";
            }
            MessasgeData mgdata = SqlPage.SqlPageAction(Datafun, sql, "id asc", pageIndex, 1);
            int nextid= Convert.ToInt32(id ?? "0") + 1;
            string IsSuccess = Question(mgdata.Mgdata, nextid);

            return IsSuccess;
        }

        public string Question(DataTable dt,int id)
        {
            StringBuilder sc = new StringBuilder();
           
            string sa = "";
            string sd = "";
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["Types"].ToString() == "1")
                { 
                sa= @"  
  <ul class='explain_type01'>
<li class='li02'>
					培知教育 
				</li>
                    <li class='li05'>
					<p class='main'>
						1.{$Question$}（）
					</p>
					<div>
						<span>A. {$A$} </span><br/>
						<span>B. {$B$}</span><br/>
						<span>C. {$C$}</span><br/>
						<span>D. {$D$}</span>
					</div> 
				</li>
				<li class='li06'>
					<span onclick='answer()' >
						<img class='wrong' src='/Content/images/r_examp_wrong.png'/>
						
						<label>A</label>
					</span>
					<span onclick='answer()' >
						<img class='wrong' src='/Content/images/r_examp_wrong.png'/>
						
						<label>B</label>
					</span>
					<span  onclick='answer()'>
						<img class='wrong' src='/Content/images/r_examp_wrong.png'/>
						
						<label>C</label>
					</span>
					<span  onclick='answer()'>
						<img class='wrong' src='/Content/images/r_examp_wrong.png'/>
						
						<label>D</label>
					</span>
				</li>
<li class='li07' style='display:none;'>
					<label class='yes'>正确 {$Answer$}</label>
					<p>解析：{$Analysis$}</p>
					<a href='###' onclick='next({$id$})'>下一题</a>
				</li>
</ul>";
                sd = sa.Replace("{$Question$}", dr["Question"].ToString()).Replace("{$A$}", dr["A"].ToString()).Replace("{$B$}", dr["B"].ToString()).Replace("{$C$}", dr["C"].ToString()).Replace("{$D$}", dr["D"].ToString()).Replace("{$Answer$}", dr["Answer"].ToString()).Replace("{$Analysis$}", dr["Analysis"].ToString()).Replace("{$id$}", id.ToString());
                }
                else if (dr["Types"].ToString() == "2")
                {
                    sa = @"  
  <ul class='explain_type01'> 
<li class='li02'>
					培知教育 
				</li>
                    <li class='li05'>
					<p class='main'>
						1.{$Question$}（）
					</p>
					
				</li>
				<li class='li06'>
					<span  onclick='answer()'>
						<img class='wrong' src='/Content/images/r_examp_wrong.png'/>
						
						<label>√</label>
					</span>
					<span onclick='answer()'>
						<img class='wrong' src='/Content/images/r_examp_wrong.png'/>
						
						<label>X</label>
					</span>
				</li>
<li class='li07' style='display:none;'>
<label class='yes'>正确 {$Answer$}</label>
					<p>解析：{$Analysis$}</p>
					<a href='###' onclick='next({$id$})'>下一题</a>
				</li>
</ul>";
                    sd = sa.Replace("{$Question$}", dr["Question"].ToString()).Replace("{$Answer$}", dr["Answer"].ToString()).Replace("{$Analysis$}", dr["Analysis"].ToString()).Replace("{$id$}", id.ToString());
                }
                else
                {
                    sa = @"
  <ul class='explain_type01'>
<li class='li02'>
					培知教育 
				</li>
                    <li class='li03'>
					<p class='main'>
						1.{$Question$}（）
					</p>
					
					<textarea name='clt'  onchange='answer()' rows="" cols=""></textarea>	
				</li>
<li class='li07' style='display:none;'>
                <label class='yes'>正确 {$Answer$}</label>
					<p>解析：{$Analysis$}</p>
					<a href='###' onclick='next({$id$})'>下一题</a>
				</li>
</ul>";
                    sd = sa.Replace("{$Question$}", dr["Question"].ToString()).Replace("{$Answer$}", dr["Answer"].ToString()).Replace("{$Analysis$}", dr["Analysis"].ToString()).Replace("{$id$}", id.ToString());
                }
                sc.Append(sd);
            }
            return sc.ToString();
        }

        public string Kgt(DataTable dt)
        {
            StringBuilder sc = new StringBuilder();

            string sa = "";
            string sd = "";
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["Types"].ToString() == "1")
                {
                    sa = @"  


                    <li class='li05'>
					<p class='main'>
						1.{$Question$}（）
					</p>
					<div>
						<span>A. {$A$} </span><br/>
						<span>B. {$B$}</span><br/>
						<span>C. {$C$}</span><br/>
						<span>D. {$D$}</span>
					</div> 
				</li>
				<li class='li06'>
					<span onclick='answer()' >
						<img class='wrong' src='/Content/images/r_examp_wrong.png'/>
						
						<label>A</label>
					</span>
					<span onclick='answer()' >
						<img class='wrong' src='/Content/images/r_examp_wrong.png'/>
						
						<label>B</label>
					</span>
					<span  onclick='answer()'>
						<img class='wrong' src='/Content/images/r_examp_wrong.png'/>
						
						<label>C</label>
					</span>
					<span  onclick='answer()'>
						<img class='wrong' src='/Content/images/r_examp_wrong.png'/>
						
						<label>D</label>
					</span>
				</li>

 <div class='clear'></div>
                    <div style='margin: 10px 0;width:100%;'>
					<label class='yes' style='color: #d4020c;'>正确: {$Answer$}</label>
					<p  style='color: #d4020c;'>解析：{$Analysis$}</p>
                         </div>
";
                    sd = sa.Replace("{$Question$}", dr["Question"].ToString()).Replace("{$A$}", dr["A"].ToString()).Replace("{$B$}", dr["B"].ToString()).Replace("{$C$}", dr["C"].ToString()).Replace("{$D$}", dr["D"].ToString()).Replace("{$Answer$}", dr["Answer"].ToString()).Replace("{$Analysis$}", dr["Analysis"].ToString());
                }
                else 
                {
                    sa = @"  


                    <li class='li05'>
					<p class='main'>
						1.{$Question$}（）
					</p>
					
				</li>
				<li class='li06'>
					<span  onclick='answer()'>
						<img class='wrong' src='/Content/images/r_examp_wrong.png'/>
						
						<label>√</label>
					</span>
					<span onclick='answer()'>
						<img class='wrong' src='/Content/images/r_examp_wrong.png'/>
						
						<label>X</label>
					</span>
				</li>
 <div class='clear'></div>
                    <div style='margin: 10px 0;width:100%;'>
					<label class='yes' style='color: #d4020c;'>正确: {$Answer$}</label>
					<p  style='color: #d4020c;'>解析：{$Analysis$}</p>
                         </div>
";
                    sd = sa.Replace("{$Question$}", dr["Question"].ToString()).Replace("{$Answer$}", dr["Answer"].ToString()).Replace("{$Analysis$}", dr["Analysis"].ToString());
                }
               
                sc.Append(sd);
            }
            return sc.ToString();
        }

    }
}
