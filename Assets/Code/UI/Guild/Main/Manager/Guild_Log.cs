using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using model;
namespace manager
{
    /// <summary>
    /// 公会日志
    /// </summary>
    public partial class GuildManager
    {
        private List<GuildLogVo> _logList;

        /// <summary>
        /// 公会日志
        /// </summary>
        public List<GuildLogVo> LogList
        {
            get { return _logList; }
            set { _logList = value; }
        }

        private void InitialLog()
        {
            _logList = new List<GuildLogVo>();
        }
    }
}