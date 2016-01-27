using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Peizhi.Models
{
    [Serializable]
    public class tb_login
    {
        /// <summary>
        /// 编号
        /// </summary>
        [DisplayName("编号")]
        public int id { get; set; }
        /// <summary>
        ///登录账户
        /// </summary>
        [DisplayName("登录账户")]
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [DisplayName("密码")]
        public string Password { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        [DisplayName("添加时间")]
        public DateTime Add_Date { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [DisplayName("更新时间")]
        public DateTime Update_Date { get; set; }  
        /// <summary>
        /// 备注
        /// </summary>
        [DisplayName("备注")]
        public string Bank { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        [DisplayName("电话号码")]
        public string  Tel { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        [DisplayName("等级")]
        public int Lvl { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [DisplayName("状态")]
        public int Status { get; set; }
        /// <summary>
        /// 额外字段
        /// </summary>
        [DisplayName("额外字段")]
        public string added1 { get; set; }
        /// <summary>
        /// 父编号
        /// </summary>
        [DisplayName("父编号")]
        public int Pid { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        [DisplayName("开始时间")]
        public DateTime  Start_Date { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [DisplayName("结束时间")]
        public DateTime End_Date { get; set; }
        /// <summary>
        /// 会议
        /// </summary>
        [DisplayName("会议")]
        public int Meeting { get; set; }
        /// <summary>
        /// 游戏进入编码
        /// </summary>
        [DisplayName("游戏进入编码")]
        public string GamePwd { get; set; }
    }
}