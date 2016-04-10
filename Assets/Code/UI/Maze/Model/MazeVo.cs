using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace model
{
    /// <summary>
    /// 迷宫奖励
    /// </summary>
    public class MazeAwardVo
    {
        public int Index { get; set; }

        public int AwardId { get; set; }

        public List<TypeStruct> AwardItems { get; set; }

        public MazeAwardVo()
        {
            AwardItems = new List<TypeStruct>();
        }
    }
    public enum MazeTaskType
    { 
        None=0,
        Start,
        End,
        Normal,
        Task,
    }
    public class MazeTaskVo
    {
        public int Id { get; set; }

        public MazeTaskType Type { get; set; }

        /// <summary>
        /// 立即完成价格
        /// </summary>
        public int NowComplatePrice { get; set; }


    }
}
