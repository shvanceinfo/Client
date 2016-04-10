using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using model;
namespace manager
{
    /// <summary>
    /// 成员审核
    /// </summary>
    public partial class GuildManager
    {
        /// <summary>
        /// 公会申请成员
        /// </summary>
        private List<GuildMemberVo> _mbcList;

        /// <summary>
        /// 公会申请成员
        /// </summary>
        public List<GuildMemberVo> MbcList
        {
            get { return _mbcList; }
            set { _mbcList = value; }
        }

        private void InitialMemberCheck()
        {
            _mbcList = new List<GuildMemberVo>();
        }
	}
}