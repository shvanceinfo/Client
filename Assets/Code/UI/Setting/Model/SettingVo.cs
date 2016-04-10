using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace model
{
    public enum PostType
    { 
        None=0,
        System,     //系统公告
        Post,       //客服公告
        Annoucement,//运维公告
    }

    public class SettingVo
    {
        public int Id { get; set; }

        public int MapId { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 当前默认选项
        /// </summary>
        public int CurOption { get; set; }

		public int LastOption { set; get;}

        /// <summary>
        /// 默认选项
        /// </summary>
        public int DefaultOption { get; set; }

        public List<int> Options { get; set; }

        /// <summary>
        /// 当前显示的人数设置
        /// </summary>
        public int DisplayPeopleCount
        {
            get {
                return Options[CurOption];
            }
        }

        public SettingVo()
        {
            Options = new List<int>();
        }

    }

    /// <summary>
    /// 公告
    /// </summary>
    public class PostVo
    {
        public int Id { get; set; }
        public PostType Type { get; set; }
        public string MsgTime { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
    }
}
