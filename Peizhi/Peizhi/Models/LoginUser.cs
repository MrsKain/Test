using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Peizhi.Models
{
    [Serializable]
    public class LoginUser
    {
        /// <summary>
        /// 用户id
        /// </summary>
        [DisplayName("用户id")]
        public string UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        [DisplayName("用户名")]
        public string UserName { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [DisplayName("用户密码")]
        public string UserPwd { get; set; }
        /// <summary>
        /// 用户备注
        /// </summary>
        [DisplayName("用户备注")]
        public string Bank { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        [DisplayName("电话")]
        public string Tel { get; set; }
        /// <summary>
        /// 用户等级
        /// </summary>
        [DisplayName("用户等级")]
        public int UserLvl { get; set; }
        /// <summary>
        /// 会议
        /// </summary>
        [DisplayName("会议")]
        public string Meetting { get; set; }
        /// <summary>
        /// 会议
        /// </summary>
        [DisplayName("签到号")]
        public string qdh { get; set; }
        /// <summary>
        /// 桌号
        /// </summary>
        [DisplayName("桌号")]
        public string znum { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        [DisplayName("图片")]
        public string image { get; set; }
        /// <summary>
        /// 权限
        /// </summary>
        public List<string> ListQx { get; set; }
        /// <summary>
        /// 全部权限
        /// </summary>
        public List<string> ListQxM { get; set; }

        private int _status = 0;

        /// <summary>
        /// 用户审核状态，企业以上默认3.
        /// </summary>
        public int Status
        {
            get
            {
                if (UserLvl < 4)
                    return 3;
                return _status;
            }
            set { _status = value; }
        }
    }
}