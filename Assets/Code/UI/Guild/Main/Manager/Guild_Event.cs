using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using model;

namespace manager
{
    /// <summary>
    /// 公会活动
    /// </summary>
    public partial class GuildManager
    {
        private BetterList<GuildEventVo> _eventShowList;

        /// <summary>
        /// 用于显示活动列表数据
        /// </summary>
        public BetterList<GuildEventVo> EventShowList
        {
            get {
                if (_eventShowList==null)
                {
                    _eventShowList = new BetterList<GuildEventVo>();
                    foreach (GuildEventVo eve in _xmlGuildEvent.Values)
                    {
                        _eventShowList.Add(eve);
                    }
                }
                return _eventShowList;
            }
        }


        private void InitialEvent()
        {
        }
    }
}