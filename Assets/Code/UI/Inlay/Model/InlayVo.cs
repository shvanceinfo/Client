using UnityEngine;
using System.Collections;

namespace model
{
    public class InlayVo
    {

        /// <summary>
        /// 宝石模板
        /// </summary>
        public GemVo Gem { get; set; }

        public ItemTemplate GemItem { get; set; }

        public ItemInfo ItemInfo { get; set; }
    }

    public class InlayEquipVo
    {
        public BetterList<BoolStruct> Gems { get; set; }

        public InlayEquipVo()
        {
            Gems = new BetterList<BoolStruct>();
            for (int i = 0; i < 4; i++)
            {
                Gems.Add(new BoolStruct(0, false));
            }
        }
    }
}