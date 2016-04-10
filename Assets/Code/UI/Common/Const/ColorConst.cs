using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace helper
{
    public class ColorConst
    {
        static StringBuilder sb = new StringBuilder();
        public const string Color_Green = "25da13";     //绿色
        //public const string Color_Green = "5bdf24";     //绿色
        public const string Color_Red = "ff0000";       //红色
        public const string Color_HeSe = "d7cfa6";      //褐色
        public const string Color_Juhuang = "d27910";   //橘黄
        public const string Color_LiangHuang = "fce449";//亮黄
        public const string Color_Pink = "c43e86";      //粉色
        public const string Color_Blue = "41b1ff";      //蓝色
        public const string Color_DanHuang = "ffe44d";  //蛋黄
        public const string Color_Qianlan = "34c099";   //浅蓝
        public const string ColorRank1 = "fffb00";       //排名一的颜色
        public const string ColorRank2 = "fc9201";       //排名二的颜色
        public const string ColorRank3 = "ef5703";       //排名三的颜色
        public const string ColorRankOther = "d8cea6";        //其他玩家的颜色
        public const string ColorRankOfline = "888888";     //下线颜色
        public static string Format(string color,string value)
        {
            return string.Format("[{0}]{1}[-]",color,value);
        }
        public static string Format(string color, object value)
        {
            return string.Format("[{0}]{1}[-]", color, value);
        }
        public static string Format(string color, params object[] args)
        {
            sb = new StringBuilder();
            string start = string.Format("[{0}]", color);
            for (int i = 0; i < args.Length; i++)
            {
                sb.Append(args[i].ToString());
            }
            string end = "[-]";
            return start + sb.ToString() + end;
        }
    }
}
