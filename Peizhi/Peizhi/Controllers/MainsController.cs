using Peizhi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Kain_class;
using Kain_class.DataFunction;
using Kain_class.Messageinfor;
using Peizhi.Models.Filter;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Xml;
using System.Net;
using System.IO;




namespace Peizhi.Controllers
{

    public class MainsController : Controller
    {
        #region 视图
        // GET: /Mains/
        /// <summary>
        ///上传文件视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload(string pash)
        {
            ViewData["pash"] = pash;
            return View("upload");
        }
        /// <summary>
        /// 返回     ViewData["year"]  年，
        /// </summary>
        /// <param name="yearid"></param>
        /// <param name="jdid"></param>
        /// <param name="yueid"></param>
        /// <returns></returns>
        public ActionResult DropDown(string yearid, string jdid, string yueid)
        {

            int yearnow = DateTime.Now.Year;
            int yearvalue = yearnow - 1975;
            StringBuilder sbBuilder = new StringBuilder();
            sbBuilder.Append(string.Format("<select id='{0}'>", yearid));
            for (int i = 0; i < yearvalue; i++)
            {
                sbBuilder.AppendFormat("<option value='{0}'>{0}年</option>\r\n", yearnow - i);
            }
            sbBuilder.Append("</select>");
            ViewData["year"] = sbBuilder;
            sbBuilder = new StringBuilder();
            sbBuilder.AppendFormat("<select id='{0}'>", jdid);
            sbBuilder.AppendFormat("<option value='{0}'>{0}月</option>\r\n", "1-3");
            sbBuilder.AppendFormat("<option value='{0}'>{0}月</option>\r\n", "1-6");
            sbBuilder.AppendFormat("<option value='{0}'>{0}月</option>\r\n", "1-9");
            sbBuilder.AppendFormat("<option value='{0}'>{0}月</option>\r\n", "1-12");
            sbBuilder.Append("</select>");
            ViewData["jd"] = sbBuilder;
            sbBuilder = new StringBuilder();
            sbBuilder.AppendFormat("<select id='{0}'>", yueid);
            for (int i = 1; i <= 12; i++)
            {
                sbBuilder.AppendFormat("<option value='{0}'>{0}月</option>\r\n", i);
            }
            sbBuilder.Append("</select>");
            ViewData["yue"] = sbBuilder;
            return View("DropDownListV");
        }

        public ActionResult CssJs(string id, string chart)
        {
            ViewData["cs"] = id;
            ViewData["chart"] = chart;
            return PartialView("~/Views/Shared/CssJs.ascx");
        }



        #endregion

        #region 上传文动作

        /// <summary>
        /// 生成上传文件唯一guid，并返回给前台
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadGenerateGuid()
        {
            string guid = Guid.NewGuid().ToString();
            XDocument doc = new XDocument();
            XElement root = new XElement("root");
            XElement XGuid = new XElement("guid", guid);
            root.Add(XGuid);
            doc.Add(root);
            return Content(doc.ToString(), "application/xml", Encoding.UTF8);

        }

        /// <summary>
        ///生成上传文件基本信息并返回给前台
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadAbout()
        {   
            string guid = Request.Form["guid"];
            bool abort = string.IsNullOrEmpty(Request.Form["abort"]) ? false : true;
            Kain_class.Upload.DownloadingFileInfo info = HttpContext.Cache[guid] as Kain_class.Upload.DownloadingFileInfo;
            if (info != null)
            {
                info.Abort = abort;
                HttpContext.Cache[guid] = info;
            }
            XDocument doc = new XDocument();
            XElement root = new XElement("root");
            XElement flag = new XElement("flag", info == null ? "false" : "true");
            root.Add(flag);
            doc.Add(root);
            return Content(doc.ToString(), "application/xml", Encoding.UTF8);


        }

        /// <summary>
        /// 生成上传文件进度状态并返回给前台
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadInfor()
        {
            string guid = Request.Form["guid"];
            Kain_class.Upload.DownloadingFileInfo info = HttpContext.Cache[guid] as Kain_class.Upload.DownloadingFileInfo;
            XDocument doc = new XDocument();
            XElement root = new XElement("root");
            if (info != null)
            {
                XElement fileName = new XElement("fileName", info.FileName);
                XElement upfileName = new XElement("upfileName", info.UpfileName);
                XElement fileFinished = new XElement("fileFinished", info.FileFinished);
                XElement fileSize = new XElement("fileSize", info.FileSize);
                XElement costTime = new XElement("costTime", info.CostTime);
                XElement fileState = new XElement("fileState", info.FileState);
                XElement speed = new XElement("speed", info.Speed);
                XElement percent = new XElement("percent", info.Percent);
                XElement abort = new XElement("abort", false);
                root.Add(fileName);
                root.Add(upfileName);
                root.Add(fileFinished);
                root.Add(fileSize);
                root.Add(costTime);
                root.Add(fileState);
                root.Add(speed);
                root.Add(percent);
                if (info.Abort)
                {
                    abort.Value = info.Abort.ToString();
                    HttpContext.Cache.Remove(guid);
                }
                if (info.FileState == "finished")
                {
                    HttpContext.Cache.Remove(guid);
                }


            }
            else
            {
                XElement none = new XElement("none", "no file");
                root.Add(none);
            }
            doc.Add(root);
            return Content(doc.ToString(), "application/xml", Encoding.UTF8);
        }

        /// <summary>
        /// 上传处理动作
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public void UploadAction(string pash, string mb)
        {
            string sd = Request["guid"];

            string guid = Request["guid"];
            pash = (pash == null || pash == "" ? "excel" : pash);
            FilePash = Server.MapPath(string.Format("~/Content/uploadfile/{0}/", pash));
            Kain_class.Upload.UploadAs UploadAsAction = new Kain_class.Upload.UploadAs(Request, HttpContext, sd, FilePash, _fileBl);
            if (mb != null)
            {
                //UploadAsAction.Mb = int.Parse(mb);//kain 20150819弃用

            }

            ViewData["uploadpash"] = UploadAsAction.Upload();


        }

        /// <summary>
        /// 获取上传文件大小
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public void UpFileSizeFile()
        {
            string guid = Request.Form["guid"]; ;//Request.Form["guid"];
            System.Web.HttpContext.Current.Cache[guid] = this.Request.Files[0].ContentLength.ToString();

        }

        public ActionResult UpFileSize()
        {
            string guid = Request.Form["guid"];
            return Content(HttpContext.Cache[guid] as string);
        }
        #endregion

        #region 属性



        private LoginUser _loginUser = null;
        /// <summary>
        /// 返回用户类实例（会员）
        /// </summary>
        public LoginUser LgUser
        {
            get
            {
                if (_loginUser == null)
                {
                    return (LoginUser)Session["Lguser"];
                }
                else
                {
                    return _loginUser;
                }

            }
        }

      
        private DataFunction _Datafun = null;
        /// <summary>
        /// 返回方法实例
        /// </summary>
        public DataFunction Datafun
        {

            get
            {


                return new DataFunction(ConfigurationManager.ConnectionStrings["connectstring"].ToString());



            }

        }
        private string _Imgkey = null;
        /// <summary>
        /// 返回合同图片路径
        /// </summary>
        public string Imgkey
        {

            get
            {


                return ConfigurationManager.AppSettings["ImgKey"];



            }

        }


        /// <summary>
        ///获取年份json 差值为当前时间-2000
        /// </summary>
        public string YearJson
        {
            get
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("text", typeof(string));
                dt.Columns.Add("value", typeof(string));


                for (int i = 0; i < Yearvalue; i++)
                {
                    DataRow dr = dt.NewRow();
                    int cz = YearNow - i;
                    dr["text"] = cz + "年";
                    dr["value"] = cz;
                    dt.Rows.Add(dr);
                }

                return Kain_class.Json.Jsonzs.DataTableToJson(dt);

            }

        }

        /// <summary>
        /// 获取月份json
        /// </summary>
        public string YueJson
        {
            get
            {
                string jsonvalue = "sd";

                DataTable dt = new DataTable();
                dt.Columns.Add("text", typeof(string));
                dt.Columns.Add("value", typeof(string));


                for (int i = 1; i <= 12; i++)
                {
                    DataRow dr = dt.NewRow();

                    dr["text"] = i + "月";
                    dr["value"] = i;
                    dt.Rows.Add(dr);
                }

                return Kain_class.Json.Jsonzs.DataTableToJson(dt);

            }

        }

        /// <summary>
        ///获取季度json
        /// </summary>
        public string JdJson
        {

            get
            {

                DataTable dt = new DataTable();
                dt.Columns.Add("text", typeof(string));
                dt.Columns.Add("value", typeof(string));
                DataRow dr = dt.NewRow();

                dr["text"] = "1-3月";
                dr["value"] = "1-3";
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr["text"] = "1-6月";
                dr["value"] = "1-6";
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr["text"] = "1-9月";
                dr["value"] = "1-9";
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr["text"] = "1-12月";
                dr["value"] = "1-12";
                dt.Rows.Add(dr);

                return Kain_class.Json.Jsonzs.DataTableToJson(dt);
            }
        }

        public enum MyDate
        {
            DataYear,
            DataYue,
            DataJd
        }
        /// <summary>
        /// 获取分页条数。
        /// </summary>
        public int PageSize
        {
            get { return Convert.ToInt32(Request["rows"]); }
        }
        /// <summary>
        /// 获取分页索引
        /// </summary>
        public int PageIndex
        {
            get { return Convert.ToInt32(Request["page"]); }
        }

        /// <summary>
        /// 判断是否可以修改
        /// </summary>
        public bool QyEditbl = false;

        private string _filePash = "upload_file_pash";

        /// <summary>
        /// 获取或设置文件路径。默认值为appSettings[upload_file_pash]
        /// </summary>
        public string FilePash
        {

            get { return _filePash; }
            set { _filePash = value; }
        }

        private bool _fileBl = false;

        /// <summary>
        /// 获取或设置是否为每个上传文件在保存路径里创建新的文件夹保
        /// 存默认值：false
        /// </summary>
        public bool FileBl
        {

            get { return _fileBl; }
            set { _fileBl = value; }
        }



        /// <summary>
        /// 当年时间
        /// </summary>
        private int YearNow = DateTime.Now.Year;

        /// <summary>
        /// 年度差值
        /// </summary>
        private int Yearvalue
        {
            get { return YearNow - 2000; }
        }

        #endregion

        #region 基本方法


        /// <summary>
        /// 返回对应的select年。或者月，或者季度
        /// </summary>
        /// <param name="id"></param>
        /// <param name="myDate"></param>
        /// <returns></returns>
        public string SelectAction(string id, MyDate myDate)
        {
            StringBuilder sbBuilder = new StringBuilder();
            sbBuilder.Append(string.Format("<select id='{0}'  class='easyui-combobox' >", id));
            if (MyDate.DataYear == myDate)
            {

                for (int i = 0; i < Yearvalue; i++)
                {
                    sbBuilder.AppendFormat("<option value='{0}'>{0}年</option>\r\n", YearNow - i);
                }

            }
            else if (MyDate.DataJd == myDate)
            {
                sbBuilder.AppendFormat("<option value='{0}'>{0}月</option>\r\n", "1-3");
                sbBuilder.AppendFormat("<option value='{0}'>{0}月</option>\r\n", "1-6");
                sbBuilder.AppendFormat("<option value='{0}'>{0}月</option>\r\n", "1-9");
                sbBuilder.AppendFormat("<option value='{0}'>{0}月</option>\r\n", "1-12");

            }
            else if (MyDate.DataYue == myDate)
            {
                for (int i = 1; i <= 12; i++)
                {
                    string sd = i.ToString();
                    if (i < 10)
                    {
                        sd = "0" + i;
                    }
                    sbBuilder.AppendFormat("<option value='{0}'>{1}月</option>\r\n", sd, i);
                }

            }

            sbBuilder.Append("</select>");
            return sbBuilder.ToString();
        }



        public string SelectAction(string id, MyDate myDate, bool bl)
        {
            StringBuilder sbBuilder = new StringBuilder();
            string check = "selected='selected'";
            DateTime dt = DateTime.Now;
            sbBuilder.Append(string.Format("<select id='{0}'  class='easyui-combobox' >", id));
            if (MyDate.DataYear == myDate)
            {

                for (int i = 0; i < Yearvalue; i++)
                {


                    string op = ((dt.Year == YearNow - i) && bl) ? check : "";
                    sbBuilder.AppendFormat("<option value='{0}' {1} >{0}年</option>\r\n", YearNow - i, op);
                }

            }
            else if (MyDate.DataJd == myDate)
            {
                int mt = dt.Month;
                string c1 = "";
                string c2 = "";
                string c3 = "";
                string c4 = "";
                if (bl)
                {
                    c1 = (mt >= 1 && mt <= 3) ? check : "";
                    c2 = (mt > 3 && mt <= 6) ? check : "";
                    c3 = (mt > 6 && mt <= 9) ? check : "";
                    c4 = (mt > 9 && mt <= 12) ? check : "";

                }

                sbBuilder.AppendFormat("<option value='{0}' {1}>{0}月</option>\r\n", "1-3", c1);
                sbBuilder.AppendFormat("<option value='{0}' {1}>{0}月</option>\r\n", "1-6", c2);
                sbBuilder.AppendFormat("<option value='{0}' {1}>{0}月</option>\r\n", "1-9", c3);
                sbBuilder.AppendFormat("<option value='{0}' {1}>{0}月</option>\r\n", "1-12", c4);

            }
            else if (MyDate.DataYue == myDate)
            {
                for (int i = 1; i <= 12; i++)
                {
                    string sd = i.ToString();
                    if (i < 10)
                    {
                        sd = "0" + i;
                    }
                    string op = ((dt.Month == i) && bl) ? check : "";
                    sbBuilder.AppendFormat("<option value='{0}' {1}>{2}月</option>\r\n", sd, op, i);
                }

            }
            sbBuilder.Append("</select>");
            return sbBuilder.ToString();
        }

        /// <summary>
        /// 添加select 标签事件
        /// </summary>
        /// <param name="selectvalue"></param>
        /// <param name="eventtype"></param>
        /// <param name="eventvalue"></param>
        public string SelectEventAdd(string selectvalue, string eventtype, string eventvalue)
        {
            int i = selectvalue.ToLower().IndexOf("<select") + 8;
            if (i >= 0)
            {
                selectvalue = selectvalue.Insert(i, string.Format(" {0}=\"{1}\" ", eventtype, eventvalue));
            }
            return selectvalue;
        }

        /// <summary>
        /// 返回json
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        protected ContentResult ContentJson(string content)
        {
            return new ContentResult
            {
                Content = content,
                ContentType = "application/json;",
                ContentEncoding = Encoding.UTF8
            };
        }

        /// <summary>
        /// 返回解码后的Request[key]
        /// System.Web.HttpUtility.UrlDecode(Request[key]);
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string RequestDecode(string key)
        {
            return System.Web.HttpUtility.UrlDecode(Request[key]);
        }
        /// <summary>
        /// 去除特殊字符K
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string SpacialWord(string key)
        {
            key = key.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");
            return key;
        }

        #endregion




        /// <summary>
        /// 将datatable列全部生成ViewData["列名"],datatable 只能有一条数据
        /// </summary>
        /// <param name="dt"></param>
        public void DatatableView(DataTable dt)
        {

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                ViewData[dt.Columns[i].ColumnName] = dt.Rows[0][dt.Columns[i].ColumnName];
            }
        }
        #region 菜单
     
        /// <summary>
        /// 编号枚举
        /// </summary>
        public enum BHenum
        {
            /// <summary>
            /// 商品编号
            /// </summary>
            Goods_BH = 0,
            /// <summary>
            /// 订单编号
            /// </summary>
            Order_BH = 1,
            /// <summary>
            /// 购物车编号
            /// </summary>
            Buyshop_BH = 2

        }
        /// <summary>
        /// 编号计算
        /// </summary>
        /// <param name="bh"></param>
        /// <returns></returns>
        public string BhCreateAction(BHenum bh)
        {
            string sd = "GS";
            switch (bh)
            {
                case BHenum.Goods_BH:
                    sd = "GS"; break;
                case BHenum.Order_BH:
                    sd = "OE"; break;
                case BHenum.Buyshop_BH:
                    sd = "BS"; break;
                default: break;
            }
            return sd + DateTime.Now.ToString("yyyyMMddHHmmssfff");

        }
        #endregion
        #region 分页
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="step"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageitem"></param>
        /// <param name="pageUrl">方法路径</param>
        /// <param name="strWhere">参数</param>
        /// <returns></returns>
        public string Pagesz(int recordCount, int pageitem, string pageUrl, string strWhere, int pageIndex = 0, int pagesize = 10)
        {
            int step = 2;//偏移量   
            int leftNum = 0;//左界限   
            int rightNum = 0;//右界限

            int pageCount = recordCount / pagesize;
            if (recordCount % pagesize != 0)
            {
                pageCount = pageCount + 1;
            }
            strWhere = strWhere == "" ? "" : "" + strWhere;

            if (pageIndex - step < 1)
            {
                leftNum = 1;
            }
            else
            {
                int lf = (pageIndex + step) > pageCount ? (pageCount - step * 2 + 1) : pageIndex - step + 1;
                leftNum = lf < 0 ? 1 : lf;

            }
            if (pageIndex + step > pageCount)
            {
                rightNum = pageCount;
            }
            else
            {
                int rg = pageIndex < step ? step * 2 : pageIndex + step;
                rightNum = rg > pageCount ? pageCount : rg;
            }
            string outPut = "";


            //if (pageIndex > 1)
            //{
            //   outPut += "<li class=\"item prevs\"><a href=\"" + pageUrl + (pageIndex - 1) + "\" class=\"num prev-disableds\"  ></a></li>";
            //}
            //else if (pageIndex == 1)
            //{
            //    outPut += "<li class=\"item prev\"><a class=\"num prev-disableds\"></a></li>";
            //}

            for (int i = leftNum; i <= rightNum; i++)
            {
                if (i == pageIndex)
                {
                    outPut += "<li class=\"item\">" + i + "</li>";
                }
                else
                {
                    outPut += "<li class=\"item\"><a  style=\"color:#0F0F0F\" href=\"" + pageUrl + "/" + i + "/" + strWhere + "\" >" + i + "</a></li>";
                }
            }
            if (pageIndex < pageCount - rightNum)
            {
                outPut += " <li class=\"item dot\">...</li>";

            }
            int last;
            last = recordCount / pageitem + (recordCount % pageitem == 0 ? 0 : 1);


            StringBuilder sb = new StringBuilder();


            sb.Append("<ul class=\"items page_l\">");

            sb.Append("<li class=\"item\"><a  style=\"color:#0F0F0F\"  href=\"" + pageUrl + "/" + (pageIndex - 1) + "/" + strWhere + "\" >上一页</a></li>");
            sb.Append("<li class=\"item\"><a  style=\"color:#0F0F0F\"  href=\"" + pageUrl + "/1/" + strWhere + "\" >首页</li>");
            sb.Append(outPut);

            sb.Append("<li class=\"item\">共" + pageCount + "页</li>");
            sb.Append("<li class=\"item\"><a style=\"color:#0F0F0F\"  href=\"" + pageUrl + "/" + pageCount + "/" + strWhere + "\" >尾页</a></li>");
            sb.Append("<li class=\"item\"><a style=\"color:#0F0F0F\"  href=\"" + pageUrl + "/" + (pageIndex + 1) + "/" + strWhere + "\" >下一页</a></li>");

            sb.Append(" </ul>");

            //sb.Append("<div class=\"form\">");
            //sb.Append("<span class=\"text\">到第</span>");
            //sb.Append("<input class=\"input\" type=\"text\" />");
            //sb.Append("<span class=\"text\">页</span>");
            //sb.Append("<a  class=\"button\" mrc=\"" + pageUrl + "{$page$}" + strWhere + "\" onclick='OnclikUrlPage(this)'>确定</a>");
            sb.Append("</div>");

            return sb.ToString();

        }
        #endregion
        #region 获取IP所在的城市
        public static string GetstringIpAddress(string ip) //strIP为IP 
        {
           
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            Byte[] pageData = wc.DownloadData("http://whois.pconline.com.cn/ip.jsp?callback=testJson&ip="+ip+"");
            return Encoding.Default.GetString(pageData);//.ASCII.GetString
          
        }
        /// <summary>
        /// 保存查询信息
        /// </summary>
        /// <returns></returns>
        public void   SaveSearch(string id,string uid,string cid,string cids,string pid,string xlh,string province,string city)
        {
            string zd = "电脑";
            string status = "";
            //string ip = Request.ServerVariables.Get("Remote_Addr");
            //string ip = System.Web.HttpContext.Current.Request.UserHostAddress;
            //string ip = GetUserIP();
            string ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (ip == null)
            {
                ip = Request.ServerVariables.Get("Remote_Addr");
            }
            bool fs = Request.Browser.IsMobileDevice;//是否手机访问 false 为PC true为手机
            if (fs == true)
            {
                zd = "手机";
            }
            string ids = GetstringIpAddress(ip);
            MessasgeData mgdata = Datafun.MgfunctionData("select * from bm_region where ccode=@city",new SqlParameter("@city",city));
            if (mgdata.Mgdatacount > 0)
            {
              
                    bool isdels = ids.Contains(mgdata.Mgdata.Rows[0]["cname"].ToString());
                    if (isdels == true)
                    {
                        status = "未窜货";
                    }
                    else
                    {
                        status = "窜货";
                    }
                
            }
            else
            {
                status = "无效防伪码";
            }
            SqlParameter[] pms = { 
                                     new SqlParameter("@Bsid",id),
                                  new SqlParameter("@Ip",ip),
                                   new SqlParameter("@Zd",zd),
                                    new SqlParameter("@City",ids),
                                    new SqlParameter("@Uid",uid),
                                    new SqlParameter("@Cid",cid),
                                    new SqlParameter("@Cids",cids),
                                    new SqlParameter("@Pid",pid),
                                    new SqlParameter("@xlh",xlh),
                                    new SqlParameter("@province",province),
                                    new SqlParameter("@citycode",city),
                                     new SqlParameter("@status",status)
                                 };

            MessasgeInfor mginfor = Datafun.Mgfunctioninfor("insert into tb_Bsidrecord(Bsid,Ip,Zd,City,Uid,Cid,Cids,Pid,xlh,province,citycode,Status) values(@Bsid,@Ip,@Zd,@City,@Uid,@Cid,@Cids,@Pid,@xlh,@province,@citycode,@Status)", pms);
           
        }
        /// <summary>
        /// 获取IP
        /// </summary>
        /// <returns></returns>
        public string GetUserIP()
        {
            string _userIP;
            try
            {
                if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] == null)
                {
                    _userIP = System.Web.HttpContext.Current.Request.UserHostAddress;
                }
                else
                {
                    _userIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
            }
            catch (Exception)
            {
                _userIP = "无法获取此IP";
            }
            return _userIP;
        }
        #endregion
        #region 短信数据发送
        protected void send(string mobilephone, int nums)
        {

            string strContent = "您好,您的验证码是：" + nums + "";
            #region 接口登录
            string url = "http://222.73.117.158/msg/HttpBatchSendSM?account=jiekou-clcs-03&pswd=Tch123456&mobile=" + mobilephone + "&msg=" + strContent + "&needstatus=true";
            HttpWebResponse hp = GetResponse(url);
            string htmlval = Html(hp);


            #endregion
        }
        /// <summary>
        /// 获取响应的html
        /// </summary>
        /// <param name="reqResponse"></param>
        /// <returns></returns>
        string Html(HttpWebResponse reqResponse)
        {

            using (var stream = reqResponse.GetResponseStream())
            {
                using (var sr = new StreamReader(stream, Encoding.UTF8))
                {
                    return sr.ReadToEnd();
                }
            }
        }
        public HttpWebResponse GetResponse(string url)
        {


            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; Trident/4.0; .NET CLR 2.0.50727; .NET CLR 3.0.04506.30; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729; .NET CLR 1.1.4322; .NET4.0C; .NET4.0E)";
            request.Accept = "*/*";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Method = "GET";

            return (HttpWebResponse)request.GetResponse();
        }


        #endregion
    }
}
