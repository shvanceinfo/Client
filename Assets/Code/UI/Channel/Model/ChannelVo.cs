using UnityEngine;
using System.Collections;
using manager;

namespace model
{
    public class ChannelVo
    {
        public int Id { get; set; }

        public int MapId { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 爆满最大人数
        /// </summary>
        public int MaxPeople { get; set; }

        /// <summary>
        /// 流畅最大人数
        /// </summary>
        public int NormalPeople { get; set; }

        /// <summary>
        /// 空闲最大人数
        /// </summary>
        public int FreePeople { get; set; }
    }

    public class ChannelLineVo
    {
        /// <summary>
        /// 线路ID
        /// </summary>
        public int Id { get; set; }

        public int CurPeople { get; set; }

        /// <summary>
        /// 获取当前流畅度
        /// </summary>
        public ChannelType Type
        {
            get {
                ChannelVo vo= ChannelManager.Instance.FindVoByMapId(MessageManager.Instance.my_property.getServerMapID());
                if (CurPeople >= 0 && CurPeople <= vo.FreePeople)
                {
                    return ChannelType.Free;
                }
                else if (CurPeople > vo.FreePeople && CurPeople <= vo.NormalPeople)
                {
                    return ChannelType.Normal;
                }
                else if (CurPeople > vo.NormalPeople && CurPeople <= vo.MaxPeople)
                {
                    return ChannelType.Max;
                }
                else
                    return ChannelType.Max;
            }
        }
    }
    public enum ChannelType
    { 
        /// <summary>
        /// 爆满
        /// </summary>
        Max,
        /// <summary>
        /// 流畅
        /// </summary>
        Normal,
        /// <summary>
        /// 空闲
        /// </summary>
        Free
    }
}
