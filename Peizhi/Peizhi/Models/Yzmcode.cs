using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;

namespace peizhi.Models.Columns
{
    public class Yzmcode
    {
        ///  <summary>  
        ///  生成随机码  
        ///  </summary>  
        ///  <param  name="length"></param>  
        ///  <returns></returns>  

        public static string RndNum(int VcodeNum)
        {
            string Vchar = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,W,X,Y,Z";

            string[] VcArray = Vchar.Split(',');

            //由于字符串很短，就不用StringBuilder了

            string VNum = string.Empty;
            int temp = -1; //记录上次随机数值，尽量避免生产几个一样的随机数


            // 采用一个简单的算法以保证生成随机数的不同

            Random rand = new Random();
            for (int i = 1; i <= VcodeNum; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i*temp*unchecked((int) DateTime.Now.Ticks));
                }

                int t = rand.Next(VcArray.Length);
                if (temp != -1 && temp == t)
                {
                    return RndNum(VcodeNum);
                }
                temp = t;
                VNum += VcArray[t];
            }
            return VNum;
        }

        ///  <summary>  
        ///  创建随机码图片  
        ///  </summary>  
        ///  <param  name="randomcode">随机码</param>  
        public static byte[] CreateImage(string VNum)
        {

            //gheight为图片宽度,根据字符长度自动更改图片宽度
            int Gheight = (int) (VNum.Length*15.5);
            System.Drawing.Bitmap Img = new System.Drawing.Bitmap(Gheight, 30);
            Graphics g = Graphics.FromImage(Img);
            g.DrawString(VNum, new System.Drawing.Font("Courier New", 16), new System.Drawing.SolidBrush(Color.Red), 1,
                1);
            //在矩形内绘制字串（字串，字体，画笔颜色，左上x.左上y） 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            Img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ContentType = "image/Png";
            g.Dispose();
            Img.Dispose();
            return ms.ToArray();

        }
    }
}