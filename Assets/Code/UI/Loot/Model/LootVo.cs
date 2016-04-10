using UnityEngine;
using System.Collections;

namespace model
{
    public class LootVo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public BetterList<ItemCount> AwardItems { get; set; }
        public LootVo()
        {
            AwardItems = new BetterList<ItemCount>();
        }
    }
    public class ItemCount
    {
        public ItemTemplate Item { get; set; }
        public int Count { get; set; }
        public ItemCount()
        {
            Count = 0;
        }
    }
}