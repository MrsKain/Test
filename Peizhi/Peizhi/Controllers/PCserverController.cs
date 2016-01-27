using Kain_class.Messageinfor;
using Kain_class.Pass;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Data;

namespace Peizhi.Controllers
{
    public class PCserverController : MainsController
    {
        //
        // GET: /PCserver/
        /// <summary>
        /// 会员管理
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberIndex()
        {
            return View();
        }
        /// <summary>
        /// 认证中心选择
        /// </summary>
        /// <returns></returns>
        public string AuthenAjax()
        {  
            string types=Request["types"];
            string IsSuccess = "";
            MessasgeData mgdata = Datafun.MgfunctionData("select * from tb_about where types=@types", new SqlParameter("@types", types));
            IsSuccess = mgdata.Mgdata.Rows[0]["Contents"].ToString();
            return IsSuccess;
        }
        /// <summary>
        /// 认证中心编辑
        /// </summary>
        /// <returns></returns>
        public string AuthenSaveAjax()
        {
            string types = Request["types"];
            string Contents = Request.Unvalidated.Form["Contents"]; ;
            string IsSuccess = "1";
            SqlParameter[] pms = { 
                                 new SqlParameter("@types",types),
                                 new SqlParameter("@Contents",Contents)
                                 }; 
            MessasgeInfor mginfor = Datafun.Mgfunctioninfor("Update tb_about set Contents=@Contents where types=@types",pms);
            if (mginfor.Mgdatacount > 0)
            {
                IsSuccess = "0";
            }
           
            return IsSuccess;
        }
        /// <summary>
        /// 关于我们编辑
        /// </summary>
        /// <returns></returns>
        public string AboutSaveAjax()
        {

            string Contents = Request.Unvalidated.Form["Contents"]; ;
            string IsSuccess = "1";
            SqlParameter[] pms = {                  
                                 new SqlParameter("@Contents",Contents)
                                 };
            MessasgeInfor mginfor = Datafun.Mgfunctioninfor("Update tb_about set Contents=@Contents where types='关于我们'", pms);
            if (mginfor.Mgdatacount > 0)
            {
                IsSuccess = "0";
            }

            return IsSuccess;
        }
       /// <summary>
       /// SEO优化编辑
       /// </summary>
       /// <returns></returns>
        public string SEOSaveAjax()
        {

            string Contents = Request["Contents"];
            string IsSuccess = "1";
            SqlParameter[] pms = {                  
                                 new SqlParameter("@Contents",Contents)
                                 };
            MessasgeInfor mginfor = Datafun.Mgfunctioninfor("Update tb_SEO set Contents=@Contents ", pms);
            if (mginfor.Mgdatacount > 0)
            {
                IsSuccess = "0";
            }

            return IsSuccess;
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public string PassSaveAjax()
        {

            string OldPass = Request["OldPass"];
            string NewPass = Request["NewPass"];
            string UserName = LgUser.UserName;
            string IsSuccess = "1";
            SqlParameter[] pms = {                  
                                 new SqlParameter("@OldPass",Passwd.SetPass(OldPass)),
                                  new SqlParameter("@NewPass",Passwd.SetPass(NewPass)),
                                   new SqlParameter("@UserName",UserName)
                                 };
            MessasgeInfor mginfor = Datafun.Mgfunctioninfor(@"IF  EXISTS( select * from tb_login where UserName=@UserName and Password=@OldPass ) begin 
           update tb_login set PassWord=@NewPass where UserName=@UserName  end  ", pms);
            if (mginfor.Mgdatacount > 0)
            {
                IsSuccess = "0";
            }

            return IsSuccess;
        }
        /// <summary>
        /// 修改报名咨询
        /// </summary>
        /// <returns></returns>
        public string BmzxSaveAjax()
        {

            string Img = Request["Img"];
            string QQone = Request["QQone"];
            string QQtwo = Request["QQtwo"];
            string Tel = Request["Tel"];
            string IsSuccess = "1";
            SqlParameter[] pms = {                  
                                 new SqlParameter("@Img",Img),
                                  new SqlParameter("@QQone",QQone),
                                   new SqlParameter("@QQtwo",QQtwo),
                                    new SqlParameter("@Tel",Tel)
                                 };
            MessasgeInfor mginfor = Datafun.Mgfunctioninfor(@"update tb_bmzx set Img=@Img,QQone=@QQone,QQtwo=@QQtwo,Tel=@Tel", pms);
            if (mginfor.Mgdatacount > 0)
            {
                IsSuccess = "0";
            }

            return IsSuccess;
        }
        /// <summary>
        /// 修改Banner
        /// </summary>
        /// <returns></returns>
        public string BanerSaveAjax()
        {

            string imgurl = Request["imgurl"];
            string IsSuccess = "0";
            string[] img=imgurl.Split(',');
            MessasgeInfor mginfor = new MessasgeInfor();
            mginfor = Datafun.Mgfunctioninfor("delete from  tb_baner");
            foreach (string imageurl in img)
            {
                SqlParameter[] pms = {                  
                                 new SqlParameter("@Img",imageurl),
                                
                                 };
                mginfor = Datafun.Mgfunctioninfor(@"insert into tb_baner(imageurl)values (@Img)", pms);
            }     
            return IsSuccess;
        }
        /// <summary>
        /// 添加新闻
        /// </summary>
        /// <returns></returns>
        public string NewsSaveAjax()
        {

            string title = Request["title"];
            string Types = Request["Types"];
            string images = Request["images"];
            string Contents = Request.Unvalidated.Form["Contents"];
            string IsSuccess = "1";                 
                SqlParameter[] pms = {                  
                                 new SqlParameter("@title",title),
                                  new SqlParameter("@Types",Types),
                                    new SqlParameter("@Contents",Contents),
                                      new SqlParameter("@images",images)
                                 };
                MessasgeInfor mginfor = Datafun.Mgfunctioninfor(@"insert into tb_news(title,Types,Contents,images)values (@title,@Types,@Contents,@images)", pms);
                if (mginfor.Mgdatacount > 0)
                {
                    IsSuccess = "0";
                }
          
            return IsSuccess;
        }
        /// <summary>
        /// 上传资料
        /// </summary>
        /// <returns></returns>
        public string UploadSaveAjax()
        {

            string title = Request["title"];
            string Types = Request["Types"];
            string dataurl = Request["dataurl"];
            string IsSuccess = "1";
            SqlParameter[] pms = {                  
                                 new SqlParameter("@title",title),
                                  new SqlParameter("@Types",Types),
                                    new SqlParameter("@dataurl",dataurl),
                                 };
            MessasgeInfor mginfor = Datafun.Mgfunctioninfor(@"insert into tb_Dataload(title,Types,dataurl)values (@title,@Types,@dataurl)", pms);
            if (mginfor.Mgdatacount > 0)
            {
                IsSuccess = "0";
            }

            return IsSuccess;
        }
        /// <summary>
        /// 提升金牌会员
        /// </summary>
        /// <returns></returns>
        public string GoldSaveAjax()
        {

            string id = Request["id"];
           
            string IsSuccess = "1";
            SqlParameter[] pms = {                  
                                 new SqlParameter("@id",id)                             
                                 };
            MessasgeInfor mginfor = Datafun.Mgfunctioninfor(@"update tb_login set lvl=2 where id=@id", pms);
            if (mginfor.Mgdatacount > 0)
            {
                IsSuccess = "0";
            }

            return IsSuccess;
        }
        /// <summary>
        /// 降为普通会员
        /// </summary>
        /// <returns></returns>
        public string GoldlowSaveAjax()
        {

            string id = Request["id"];

            string IsSuccess = "1";
            SqlParameter[] pms = {                  
                                 new SqlParameter("@id",id)                             
                                 };
            MessasgeInfor mginfor = Datafun.Mgfunctioninfor(@"update tb_login set lvl=1 where id=@id", pms);
            if (mginfor.Mgdatacount > 0)
            {
                IsSuccess = "0";
            }

            return IsSuccess;
        }
        /// <summary>
        /// 添加微课
        /// </summary>
        /// <returns></returns>
        public string AddvideoSaveAjax()
        {

            string title = Request["title"];
            string teacher = Request["teacher"];
            string counts = Request["counts"];
            string Types = Request["Types"];
            string sumtime = Request["sumtime"];
            string introduce = Request["introduce"];
            string images = Request["images"];

            string IsSuccess = "0";
            SqlParameter[] pms = {                  
                                 new SqlParameter("@title",title),
                                 new SqlParameter("@teacher",teacher),
                                 new SqlParameter("@counts",counts),    
                                 new SqlParameter("@Types",Types),   
                                 new SqlParameter("@sumtime",sumtime),  
                                 new SqlParameter("@introduce",introduce),    
                                 new SqlParameter("@images",images)
                                 };
            MessasgeData mgdata = Datafun.MgfunctionData(@"insert into tb_CourseVideo(title,tearcher,counts,Types,sumtime,introduce,images)values(@title,@teacher,@counts,@Types,@sumtime,@introduce,@images) select SCOPE_IDENTITY()", pms);
            if (mgdata.Mgdatacount > 0)
            {
                IsSuccess = mgdata.Mgdata.Rows[0][0].ToString();
            }

            return IsSuccess;
        }
        /// <summary>
        /// 上传课时视频
        /// </summary>
        /// <returns></returns>
        public string UploadVideoAjax()
        {

            string title = Request["title"];
            string kcjj = Request["kcjj"];
            string kcjs = Request["kcjs"];
            string videourl = Request["videourl"];
            string courseid = Request["courseid"];
          
            string IsSuccess = "1";
            SqlParameter[] pms = {                  
                                 new SqlParameter("@title",title),
                                 new SqlParameter("@kcjj",kcjj),
                                 new SqlParameter("@kcjs",kcjs),    
                                 new SqlParameter("@videourl",videourl),   
                                 new SqlParameter("@courseid",courseid)                              
                                 };
            MessasgeInfor mginfor = Datafun.Mgfunctioninfor(@"insert into tb_CourseVideoInfor(title,brief,introduce,videourl,courseid)values(@title,@kcjj,@kcjs,@videourl,@courseid)", pms);
            if (mginfor.Mgdatacount > 0)
            {
                IsSuccess = "0";
            }

            return IsSuccess;
        }
        /// <summary>
        /// 修改课时
        /// </summary>
        /// <returns></returns>
        public string UpdateVideoAjax()
        {

            string id = Request["id"];
            string title = Request["title"];
            string kcjj = Request["kcjj"];
            string kcjs = Request["kcjs"];
            string IsSuccess = "1";
            SqlParameter[] pms = {                  
                                 new SqlParameter("@id",id),
                                 new SqlParameter("@title",title),
                                 new SqlParameter("@kcjj",kcjj),    
                                 new SqlParameter("@kcjs",kcjs)                                                       
                                 };
            MessasgeInfor mginfor = Datafun.Mgfunctioninfor(@"update  tb_CourseVideoInfor set title=@title,brief=@kcjj,introduce=@kcjs where id=@id", pms);
            if (mginfor.Mgdatacount > 0)
            {
                IsSuccess = "0";
            }

            return IsSuccess;
        }
        /// <summary>
        /// 添加试卷
        /// </summary>
        /// <returns></returns>
        public string AddExamSaveAjax()
        {

            string title = Request["title"];       
            string Types = Request["Types"];       
            string IsSuccess = "0";
            SqlParameter[] pms = {                  
                                 new SqlParameter("@title",title),                                 
                                 new SqlParameter("@Types",Types)                              
                                 };
            MessasgeData mgdata = Datafun.MgfunctionData(@"insert into tb_Examlist(title,Types)values(@title,@Types) select SCOPE_IDENTITY()", pms);
            if (mgdata.Mgdatacount > 0)
            {
                IsSuccess = mgdata.Mgdata.Rows[0][0].ToString();
            }

            return IsSuccess;
        }
        /// <summary>
        /// 添加题目
        /// </summary>
        /// <returns></returns>
        public string AddQuestionSaveAjax()
        {

            string ExamType = Request["ExamType"];
            string Question = Request["Question"];
            string Types = Request["Types"];
            string A = Request["A"];
            string B = Request["B"];
            string C = Request["C"];
            string D = Request["D"];
            string Answer = Request["Answer"];
            string trades = Request["trades"];
            string Analysis = Request["Analysis"];
            string IsSuccess = "1";
            SqlParameter[] pms = {                  
                                 new SqlParameter("@ExamType",ExamType),                                 
                                 new SqlParameter("@Question",Question),
                                 new SqlParameter("@Types",Types),
                                 new SqlParameter("@A",A),
                                 new SqlParameter("@B",B),
                                 new SqlParameter("@C",C),
                                 new SqlParameter("@D",D),
                                 new SqlParameter("@Answer",Answer),
                                 new SqlParameter("@trades",trades),
                                 new SqlParameter("@Analysis",Analysis)                              
                                 };
            MessasgeInfor mginfor = Datafun.Mgfunctioninfor(@"insert into tb_Examinfor(ExamType,Question,Types,A,B,C,D,Answer,trades,Analysis)values(@ExamType,@Question,@Types,@A,@B,@C,@D,@Answer,@trades,@Analysis)", pms);
            if (mginfor.Mgdatacount > 0)
            {
                IsSuccess = "0";
            }

            return IsSuccess;
        }
        /// <summary>
        /// 添加类别
        /// </summary>
        /// <returns></returns>
        public string AddTypesSaveAjax()
        {

            string title = Request["title"];
            string pid = Request["pid"];
           
            string IsSuccess = "1";
            SqlParameter[] pms = {                  
                                 new SqlParameter("@title",title),                                 
                                 new SqlParameter("@pid",pid),
                                             
                                 };
            MessasgeInfor mginfor = Datafun.Mgfunctioninfor(@"insert into tb_types(title,pid,lvl)values(@title,@pid,2)", pms);
            if (mginfor.Mgdatacount > 0)
            {
                IsSuccess = "0";
            }

            return IsSuccess;
        }
        /// <summary>
        /// 修改底部信息
        /// </summary>
        /// <returns></returns>
        public string FootSaveAjax()
        {

            string CopyRight = Request["company"];
            string Tel = Request["tel"];
            string Address = Request["address"];
            string IsSuccess = "1";
            SqlParameter[] pms = {                  
                                 new SqlParameter("@CopyRight",CopyRight),
                                 new SqlParameter("@Tel",Tel),
                                   new SqlParameter("@Address",Address)
                                 };
            MessasgeInfor mginfor = Datafun.Mgfunctioninfor(@"update tb_foot set CopyRight=@CopyRight,Tel=@Tel,Address=@Address ", pms);
            if (mginfor.Mgdatacount > 0)
            {
                IsSuccess = "0";
            }

            return IsSuccess;
        }
        /// <summary>
        /// 修改试卷
        /// </summary>
        /// <returns></returns>
        public string UpdateExamAjax()
        {

            string id = Request["va1"];
            string Types = Request["va2"];
            string title = Request["va3"];
            string IsSuccess = "1";
            SqlParameter[] pms = {                  
                                 new SqlParameter("@id",id),
                                 new SqlParameter("@Types",Types),
                                   new SqlParameter("@title",title)
                                 };
            MessasgeInfor mginfor = Datafun.Mgfunctioninfor(@"update tb_Examlist set Types=@Types,title=@title where id=@id ", pms);
            if (mginfor.Mgdatacount > 0)
            {
                IsSuccess = "0";
            }

            return IsSuccess;
        }
        /// <summary>
        /// 修改视频
        /// </summary>
        /// <returns></returns>
        public string UpdateVideosAjax()
        {

            string id = Request["id"];
            string brief = Request["brief"];
            string title = Request["title"];
            string introduce = Request["introduce"];
            string imgurl = Request["imgurl"];
            string IsSuccess = "1";
            SqlParameter[] pms = {                  
                                 new SqlParameter("@id",id),
                                 new SqlParameter("@imgurl",imgurl),
                                   new SqlParameter("@title",title),
                                     new SqlParameter("@introduce",introduce),
                                          new SqlParameter("@brief",brief)
                                 };
            MessasgeInfor mginfor = Datafun.Mgfunctioninfor(@"update tb_CourseVideoInfor set videourl=@imgurl,title=@title,introduce=@introduce,brief=@brief where id=@id ", pms);
            if (mginfor.Mgdatacount > 0)
            {
                IsSuccess = "0";
            }

            return IsSuccess;
        }
        /// <summary>
        /// 修改题目
        /// </summary>
        /// <returns></returns>
        public string updateQuestionSaveAjax()
        {

          
            string Question = Request["Question"];
            string id = Request["id"];
            string A = Request["A"];
            string B = Request["B"];
            string C = Request["C"];
            string D = Request["D"];
            string Answer = Request["Answer"];
            string trades = Request["trades"];
            string Analysis = Request["Analysis"];
            string IsSuccess = "1";
            SqlParameter[] pms = {                               
                                 new SqlParameter("@Question",Question),
                                 new SqlParameter("@A",A),
                                 new SqlParameter("@B",B),
                                 new SqlParameter("@C",C),
                                 new SqlParameter("@D",D),
                                   new SqlParameter("@id",id),
                                 new SqlParameter("@Answer",Answer),
                                 new SqlParameter("@trades",trades),
                                 new SqlParameter("@Analysis",Analysis)                              
                                 };
            MessasgeInfor mginfor = Datafun.Mgfunctioninfor(@"update  tb_Examinfor set Question=@Question,A=@A,B=@B,C=@C,D=@D,Answer=@Answer,trades=@trades,Analysis=@Analysis where id=@id", pms);
            if (mginfor.Mgdatacount > 0)
            {
                IsSuccess = "0";
            }

            return IsSuccess;
        }
    }
}
