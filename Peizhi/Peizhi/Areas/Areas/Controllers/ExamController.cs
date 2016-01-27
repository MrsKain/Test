using Kain_class.Messageinfor;
using Peizhi.Controllers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Data;
using Kain_class.SqlPager;

namespace Peizhi.Areas.Areas.Controllers
{
    public class ExamController : MainsController
    {
        //
        // GET: /Areas/Exam/
        /// <summary>
        /// 考试首页
        /// </summary>
        /// <returns></returns>
        public ActionResult ExamIndex()
        {
            return View();
        }
        /// <summary>
        /// 考试列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Examlist(string id="1")
        {
            ViewBag.id = id;
            MessasgeData mgdata = Datafun.MgfunctionData("select * from tb_Examlist where Types in(select id from tb_types where pid="+id+")");
            ViewBag.table = mgdata.Mgdata;
            return View();
        }
        /// <summary>
        /// 练习列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Testlist(string id="1")
        {
            ViewBag.id = id;
            MessasgeData mgdata = Datafun.MgfunctionData("select * from tb_Examlist where Types in(select id from tb_types where pid=" + id + ")");
            ViewBag.table = mgdata.Mgdata;
            return View();
        }
        /// <summary>
        /// 综合练习列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Zhlist()
        {
            return View();
        }
        /// <summary>
        /// 练习答题页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Testinfor(string id)
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
            MessasgeDataSet mgdataset = Datafun.MgfunctionDataSet("select SUM(trades),COUNT(id) from tb_examinfor where ExamType=@id;select title from tb_Examlist where id=@id;select top 1 * from tb_examinfor where ExamType=@id ", new SqlParameter("@id", id));
            ViewBag.grades = mgdataset.MgdataSet.Tables[0].Rows[0][0];
            ViewBag.counts = mgdataset.MgdataSet.Tables[0].Rows[0][1];
            ViewBag.title = mgdataset.MgdataSet.Tables[1].Rows[0][0];
            ViewBag.question = mgdataset.MgdataSet.Tables[2];
            ViewBag.id = id;
            //SqlParameter[] pms = {
            //                     new SqlParameter("@exam",id+","),  
            //                     new SqlParameter("@id",LgUser.UserId)
            //                     };
            //MessasgeInfor mginfor = Datafun.Mgfunctioninfor("Update tb_login set exam=(exam+@exam) where id=@id", pms);//触屏版暂时不将试卷写入tb_login
            return View();
        }

        /// <summary>
        /// 综合练习
        /// </summary>
        /// <returns></returns>
        public ActionResult zhtestinfor()
        {
            return View();
        }
        /// <summary>
        /// 交卷
        /// </summary>
        /// <returns></returns>
        public ActionResult Jiaojuan(string id)
        {
            string sql = "select *from tb_ExamInfor where id in(" + id.Substring(0, id.Length - 1) + ")";
            MessasgeData mgdata = Datafun.MgfunctionData(sql );
            ViewBag.title = QuestionList(mgdata.Mgdata);
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
            MessasgeDataSet mgdataset = Datafun.MgfunctionDataSet("select SUM(trades),COUNT(id) from tb_examinfor where ExamType=@id;select title from tb_Examlist where id=@id;select top 1 * from tb_examinfor where ExamType=@id ", new SqlParameter("@id", id));
            ViewBag.grades = mgdataset.MgdataSet.Tables[0].Rows[0][0];
            ViewBag.counts = mgdataset.MgdataSet.Tables[0].Rows[0][1];
            ViewBag.title = mgdataset.MgdataSet.Tables[1].Rows[0][0];
            ViewBag.question = mgdataset.MgdataSet.Tables[2];
            ViewBag.id = id;
            //SqlParameter[] pms = {
            //                     new SqlParameter("@exam",id+","),  
            //                     new SqlParameter("@id",LgUser.UserId)
            //                     };
            //MessasgeInfor mginfor = Datafun.Mgfunctioninfor("Update tb_login set exam=(exam+@exam) where id=@id", pms);//触屏版暂时不将试卷写入tb_login
            return View();
        }
        /// <summary>
        /// 题目页面
        /// </summary>
        /// <returns></returns>
        public string Question(string id="1")
        {
            int pageIndex = Convert.ToInt32(id ?? "0");
           
            string Examtype=Request["Examtype"];
            string sql = "select * from tb_examinfor where ExamType=" + Examtype + " ";
            MessasgeData mgdata = SqlPage.SqlPageAction(Datafun, sql, "Types", pageIndex, 1);

            StringBuilder sc = new StringBuilder();
            string sa = "";
            string sd = "<div style='text-align:center;font-size:20px;font-family:Arial'>考试已经结束。请选择交接吧！</div>";
            foreach (DataRow dr in mgdata.Mgdata.Rows)
            {
                if (dr["Types"].ToString() == "1")
                {
                    sa = @"  
                    <section class='multiple-choice'>
      <div class='topic-name'>选择题</div>
      <div class='topic'>
        <p class='p1'>
          {$Question$}（ ）
        </p>
        <p class='pa'>A. {$A$}</p>
        <p class='pa'>B. {$B$}</p>
        <p class='pa'>C. {$C$}</p>
        <p class='pa'>D. {$D$}</p>
        <ul>
          <li class='active' onclick='answer(this)'><a href='#'>A</a></li>
          <li onclick='answer(this)'><a href='#'>B</a></li>
          <li onclick='answer(this)'><a href='#'>C</a></li>
          <li onclick='answer(this)'><a href='#'>D</a></li>
          <a class='next' href='###' onclick='next({$tc$})'>下一题</a>
        </ul>
<div class='clear'></div>
                    <div style='margin: 10px 0;width:100%;' class='hide'>
					<label class='yes' style='color: #d4020c;'>正确: {$Answer$}</label>
					<p  style='color: #d4020c;'>解析：{$Analysis$}</p>
                         </div>
      </div>
    </section>
 
";
                    sd = sa.Replace("{$Question$}", dr["Question"].ToString()).Replace("{$A$}", dr["A"].ToString()).Replace("{$B$}", dr["B"].ToString()).Replace("{$C$}", dr["C"].ToString()).Replace("{$D$}", dr["D"].ToString()).Replace("{$Answer$}", dr["Answer"].ToString()).Replace("{$Analysis$}", dr["Analysis"].ToString()).Replace("{$tc$}", (Convert.ToInt32(id) + 1).ToString());
                }
                else if (dr["Types"].ToString() == "2")
                {
                    sa = @"  
                   <section class='multiple-choice'>
      <div class='topic-name'>判断题</div>
      <div class='topic'>
        <p class='p1'>
          {$Question$}。
        </p>
        <div class='clear h20'></div>
        <ul>
          <li  onclick='answer(this)'><a href='#'>&radic;</a></li>
          <li  onclick='answer(this)'><a href='#'>X</a></li>
          <a class='next' href='###' onclick='next({$tc$})'>下一题</a>
        </ul>
<div class='clear'></div>
                    <div style='margin: 10px 0;width:100%;' class='hide'>
					<label class='yes' style='color: #d4020c;'>正确: {$Answer$}</label>
					<p  style='color: #d4020c;'>解析：{$Analysis$}</p>
                         </div>
      </div>
    </section>
 
";
                    sd = sa.Replace("{$Question$}", dr["Question"].ToString()).Replace("{$Answer$}", dr["Answer"].ToString()).Replace("{$Analysis$}", dr["Analysis"].ToString()).Replace("{$tc$}", (Convert.ToInt32(id) + 1).ToString());
                }
                else
                {
                    sa = @"  
                    <section class='multiple-choice'>
      <div class='topic-name'>问答题</div>
      <div class='topic'>
        <p class='p1'>
        {$Question$}。
        </p>
        <textarea class='questions-answers'></textarea>
        <ul>
       <a class='next' href='###' onclick='answer(this)'>查看答案</a>
          <a class='next' href='###' onclick='next({$tc$})'>下一题</a>
        </ul>
<div class='clear'></div>
                    <div style='margin: 10px 0;width:100%;' class='hide'>
					<label class='yes' style='color: #d4020c;'>正确: {$Answer$}</label>
					<p  style='color: #d4020c;'>解析：{$Analysis$}</p>
                         </div>
      </div>
    </section>
 
";
                    sd = sa.Replace("{$Question$}", dr["Question"].ToString()).Replace("{$Answer$}", dr["Answer"].ToString()).Replace("{$Analysis$}", dr["Analysis"].ToString()).Replace("{$tc$}", (Convert.ToInt32(id) + 1).ToString());
                }

                sc.Append(sd);
            }
            return sc.ToString();
        }


        /// <summary>
        /// 题目页面
        /// </summary>
        /// <returns></returns>
        public string QuestionExam(string id = "1")
        {
            int pageIndex = Convert.ToInt32(id ?? "0");

            string Examtype = Request["Examtype"];
            string sql = "select * from tb_examinfor where ExamType=" + Examtype + " ";
            MessasgeData mgdata = SqlPage.SqlPageAction(Datafun, sql, "Types", pageIndex, 1);
          
            StringBuilder sc = new StringBuilder();
            string sa = "";
            string sd = "";
           
                sc.Append("<div style='text-align:center;font-size:20px;font-family:Arial'>考试已经结束。请选择交接吧！</div>");
          
            foreach (DataRow dr in mgdata.Mgdata.Rows)
            {
                sc.Clear();
                if (dr["Types"].ToString() == "1")
                {
                    sa = @"  
                    <section class='multiple-choice'>
      <div class='topic-name'>选择题</div>
      <div class='topic'>
        <p class='p1'>
          {$Question$}（ ）
        </p>
        <p class='pa'>A. {$A$}</p>
        <p class='pa'>B. {$B$}</p>
        <p class='pa'>C. {$C$}</p>
        <p class='pa'>D. {$D$}</p>
        <ul>
          <li onclick='answer(this)' title='A'><a href='#'>A</a></li>
          <li onclick=answer(this) title='B'><a href='#'>B</a></li>
          <li onclick=answer(this) title='C'><a href='#'>C</a></li>
          <li onclick=answer(this) title='D'><a href='#'>D</a></li>
          <a class='next' href='###' onclick=next({$tc$},'{$Answer$}','{$trades$}','{$id$}')>下一题</a>
        </ul>

    </section>
 
";
                    sd = sa.Replace("{$Question$}", dr["Question"].ToString()).Replace("{$A$}", dr["A"].ToString()).Replace("{$B$}", dr["B"].ToString()).Replace("{$C$}", dr["C"].ToString()).Replace("{$D$}", dr["D"].ToString()).Replace("{$Answer$}", dr["Answer"].ToString()).Replace("{$id$}", dr["id"].ToString()).Replace("{$trades$}", dr["trades"].ToString()).Replace("{$tc$}", (Convert.ToInt32(id) + 1).ToString());
                }
                else if (dr["Types"].ToString() == "2")
                {
                    sa = @"  
                   <section class='multiple-choice'>
      <div class='topic-name'>判断题</div>
      <div class='topic'>
        <p class='p1'>
          {$Question$}。
        </p>
        <div class='clear h20'></div>
        <ul>
          <li  onclick=answer(this)  title='A'><a href='#'>&radic;</a></li>
          <li  onclick=answer(this)  title='B'><a href='#'>X</a></li>
          <a class='next' href='###' onclick=next({$tc$},'{$Answer$}','{$trades$}','{$id$}')>下一题</a>
        </ul>

    </section>
 
";
                    sd = sa.Replace("{$Question$}", dr["Question"].ToString()).Replace("{$Answer$}", dr["Answer"].ToString()).Replace("{$id$}", dr["id"].ToString()).Replace("{$trades$}", dr["trades"].ToString()).Replace("{$tc$}", (Convert.ToInt32(id) + 1).ToString());
                }
                else
                {
                    sa = @"  
                    <section class='multiple-choice'>
      <div class='topic-name'>问答题</div>
      <div class='topic'>
        <p class='p1'>
        {$Question$}。
        </p>
        <textarea class='questions-answers'></textarea>
        <ul>
          <a class='next' href='###' onclick=next({$tc$},0,0,{$id$})>下一题</a>
        </ul>

    </section>
 
";
                    sd = sa.Replace("{$Question$}", dr["Question"].ToString()).Replace("{$id$}", dr["id"].ToString()).Replace("{$tc$}", (Convert.ToInt32(id) + 1).ToString());
                }

                sc.Append(sd);
            }
            return sc.ToString();
        }

        /// <summary>
        /// 题目页面
        /// </summary>
        /// <returns></returns>
        public string QuestionList(DataTable dt)
        {
               
            StringBuilder sc = new StringBuilder();
            string sa = "";
            string sd = "";
            foreach (DataRow dr in dt.Rows)
            {
             
                if (dr["Types"].ToString() == "1")
                {
                    sa = @"  
                    <section class='multiple-choice'>
      <div class='topic-name'>选择题</div>
      <div class='topic'>
        <p class='p1'>
          {$Question$}（ ）
        </p>
        <p class='pa'>A. {$A$}</p>
        <p class='pa'>B. {$B$}</p>
        <p class='pa'>C. {$C$}</p>
        <p class='pa'>D. {$D$}</p>
      <div class='clear'></div>
                    <div style='margin: 10px 0;width:100%;' class='hide'>
					<label class='yes' style='color: #d4020c;'>正确: {$Answer$}</label>
					<p  style='color: #d4020c;'>解析：{$Analysis$}</p>
                         </div>
      </div>

    </section>
 
";
                    sd = sa.Replace("{$Question$}", dr["Question"].ToString()).Replace("{$A$}", dr["A"].ToString()).Replace("{$B$}", dr["B"].ToString()).Replace("{$C$}", dr["C"].ToString()).Replace("{$D$}", dr["D"].ToString()).Replace("{$Answer$}", dr["Answer"].ToString()).Replace("{$Analysis$}", dr["Analysis"].ToString());
                }
                else if (dr["Types"].ToString() == "2")
                {
                    sa = @"  
                   <section class='multiple-choice'>
      <div class='topic-name'>判断题</div>
      <div class='topic'>
        <p class='p1'>
          {$Question$}。
        </p>
        <div class='clear h20'></div>
       <div class='clear'></div>
                    <div style='margin: 10px 0;width:100%;' class='hide'>
					<label class='yes' style='color: #d4020c;'>正确: {$Answer$}</label>
					<p  style='color: #d4020c;'>解析：{$Analysis$}</p>
                         </div>
      </div>

    </section>";
                    sd = sa.Replace("{$Question$}", dr["Question"].ToString()).Replace("{$Answer$}", dr["Answer"].ToString()).Replace("{$Analysis$}", dr["Analysis"].ToString());
                }
                else
                {
                    sa = @"  
                    <section class='multiple-choice'>
      <div class='topic-name'>问答题</div>
      <div class='topic'>
        <p class='p1'>
        {$Question$}。
        </p>
     
       <div class='clear'></div>
                    <div style='margin: 10px 0;width:100%;' class='hide'>
					<label class='yes' style='color: #d4020c;'>正确: {$Answer$}</label>
					<p  style='color: #d4020c;'>解析：{$Analysis$}</p>
                         </div>
      </div>

    </section>
 
";
                    sd = sa.Replace("{$Question$}", dr["Question"].ToString()).Replace("{$Analysis$}", dr["Analysis"].ToString()).Replace("{$Answer$}", dr["Answer"].ToString());
                }

                sc.Append(sd);
            }
            return sc.ToString();
        }
    }
}
