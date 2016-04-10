using UnityEngine;
using System.Collections;


namespace model
{
    /// <summary>
    /// 商品的分类，用于显示在哪个页签
    /// </summary>
    public enum SellShopType:byte
    { 
        None=0,
        HotSell,
        Equip,
        Item,
        Diamon,
        Shuijing
    }
    public enum ShopStateType : byte
    { 
        /// <summary>
        /// 无活动
        /// </summary>
        None=0,     
        /// <summary>
        /// 热卖
        /// </summary>
        HotSell,    
        /// <summary>
        /// 优惠
        /// </summary>
        Prefer,     
        /// <summary>
        /// 折扣
        /// </summary>
        DisCount    
    }
    public class ShopVo
    {
        /// <summary>
        /// 商品编号
        /// </summary>
        public int Id { get; set; }
        public string Name { get; set; }
        public SellShopType Table { get; set; }
        /// <summary>
        /// 商品的显示顺序
        /// </summary>
        public int DisplayId { get; set; }

        /// <summary>
        /// 售卖物品的ID
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// 物品获得的数量
        /// </summary>
        public int GetItemCount { get; set; }

        public int GetDiamonCount { get; set; }

        public eGoldType SellMoneyType { get; set; }

        public int SellPrice { get; set; }

        public int RmbPrice { get; set; }

        public int Region { get; set; }

        
        /// <summary>
        /// 售卖状态
        /// </summary>
        public ShopStateType SellState { get; set; }

        /// <summary>
        /// 商品状态图标
        /// </summary>
        public string StateIcon { get; set; }

        /// <summary>
        /// 商品状态描述
        /// </summary>
        public string StateDescription { get; set; }


        public ShopVo()
        {

        }
    }
}
