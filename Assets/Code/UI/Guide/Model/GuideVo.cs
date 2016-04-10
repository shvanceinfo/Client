using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace model
{
    /// <summary>
    /// 触发器类型
    /// </summary>
    public enum TriggerType
    { 
        None=0,
        /// <summary>
        /// 按钮按下
        /// </summary>
        ButtonClick,

        /// <summary>
        /// 任务可以领取
        /// </summary>
        QuestionCanReceive,

        /// <summary>
        /// 任务完成
        /// </summary>
        QuestionComplate,

        /// <summary>
        /// 等级到达
        /// </summary>
        LevelTo,

        /// <summary>
        /// 打开UI
        /// </summary>
        UIOpen,

        /// <summary>
        /// 关闭UI
        /// </summary>
        UIClose,

        /// <summary>
        /// 怪物刷新
        /// </summary>
        MonsterArea,

        /// <summary>
        /// 任务进行中
        /// </summary>
        QuestInProgress,
    }

    /// <summary>
    /// 引导状态
    /// </summary>
    public enum GuideStatus
    { 
        /// <summary>
        /// 未激活
        /// </summary>
        None,

        /// <summary>
        /// 触发
        /// </summary>
        OnTrigger,

        /// <summary>
        /// 已完成
        /// </summary>
        Success
    }

    public enum SpecialType
    { 
        /// <summary>
        /// 正常类型
        /// </summary>
        Normal=0,

        /// <summary>
        /// 背包类型
        /// </summary>
        Bag,
    }

    public class GuideVo : IComparable<GuideVo>
    {
        public int Id { get; set; }


        /// <summary>
        /// 组ID
        /// </summary>
        public int Group { get; set; }

        /// <summary>
        /// 执行步奏
        /// </summary>
        public int Step { get; set; }

        /// <summary>
        /// 引导类型-是否强制
        /// </summary>
        public bool Enforce { get; set; }

        /// <summary>
        /// 是否可以跳过
        /// </summary>
        public bool Ignore { get; set; }

        /// <summary>
        /// 启用触发器
        /// </summary>
        public TriggerType Trigger { get; set; }

        /// <summary>
        /// 启用触发器参数
        /// </summary>
        public string TriggerParams { get; set; }

        /// <summary>
        /// 完成触发器
        /// </summary>
        public TriggerType ComplateType { get; set; }

        /// <summary>
        /// 完成触发器参数
        /// </summary>
        public string ComplateParams { get; set; }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string Tip { get; set; }

        /// <summary>
        /// 持续时间
        /// </summary>
        public int Time { get; set; }

        /// <summary>
        /// 布局
        /// </summary>
        public TextAnchor Anchor { get; set; }

        /// <summary>
        /// 信息提示绑定ID
        /// </summary>
        public int TipBindId { get; set; }

        /// <summary>
        /// 特别类型
        /// </summary>
        public SpecialType Special { get; set; }

        /// <summary>
        /// 特别类型参数
        /// </summary>
        public string SpecialParams { get; set; }


        /// <summary>
        /// 需要关闭所有UI
        /// </summary>
        public bool CloseUI { get; set; } 

        /// <summary>
        /// 引导状态
        /// </summary>
        public GuideStatus Status { get; set; }

        public GuideVo()
        {
            Status = GuideStatus.None;
        }



        public int CompareTo(GuideVo other)
        {
            if (this.Id > other.Id)
            {
                return 1;
            }
            else if (this.Id < other.Id)
            {
                return -1;
            }
            else
            {
                if (this.Step > other.Step)
                {
                    return 1;
                }
                else if (this.Step > other.Step)
                {
                    return -1;
                }
                else {
                    return 0;
                }
            }
        }
    }
}
