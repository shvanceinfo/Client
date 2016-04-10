using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace model
{
    public class TalentVo
    {
        public int XmlId { get; set; }
        public int SId { get; set; }
        public int Level { get {
            if (XmlId.ToString().Length >= 3 && XmlId.ToString().Substring(XmlId.ToString().Length - 3, 3) == "000")
                return 0;
            else
                return XmlId % 100 == 0 ? 100 : XmlId % 100;
        } }
        public string Name { get; set; }
        public string Icon { get; set; }
        public int DisplayLevel { get; set; }//当天赋等级大于次值，可以显示在列表
        public int MaxLevel { get; set; }
        public int TalentType { get; set; }  //天赋类型
        public int TalentValue { get; set; } //天赋所附加的值
        public int ComsumeType { get; set; } //升级消耗需要的物品类型
        public int ComsumeValue { get; set; }//升级消耗需要的数量
        public string Description { get; set; }//描述
    }
}
