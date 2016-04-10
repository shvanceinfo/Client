using UnityEngine;
using System.Collections;
using model;
namespace manager
{
    /// <summary>
    /// 信息页面
    /// 当前类，所有变量方法以Info开始
    /// </summary>
    public partial class GuildManager
    {
        #region 我的信息
        /// <summary>
        /// 我的信息-职位
        /// </summary>
        public GuildOffice InfoOwnType
        {
            get { return _infoOwnType; }
            set { _infoOwnType = value; }
        }
        
        /// <summary>
        /// 我的信息-贡献度
        /// </summary>
        public IdStruct InfoOwnContribution
        {
            get { return _infoOwnContribution; }
            set { _infoOwnContribution = value; }
        }
        /// <summary>
        /// 我的信息-历史总计贡献度
        /// </summary>
        public IdStruct InfoOwnTotalContribution
        {
            get { return _infoOwnTotalContribution; }
            set { _infoOwnTotalContribution = value; }
        }

        private GuildOffice _infoOwnType;       //职位

        private IdStruct _infoOwnContribution;  //当前贡献度

        private IdStruct _infoOwnTotalContribution;//历史总计贡献度


        
        #endregion

        #region 公会信息
        private string _infoLeaderName;         //会长名称

        /// <summary>
        /// 会长名称
        /// </summary>
        public string InfoLeaderName
        {
            get { return _infoLeaderName; }
            set { _infoLeaderName = value; }
        }

        private uint _infoGuildLevel;           //公会等级

        /// <summary>
        /// 公会当前等级
        /// </summary>
        public uint InfoGuildLevel
        {
            get { return _infoGuildLevel; }
            set { _infoGuildLevel = value; }
        }

        private uint _infoRankLevel;            //公会排名

        /// <summary>
        /// 公会排名
        /// </summary>
        public uint InfoRankLevel
        {
            get { return _infoRankLevel; }
            set { _infoRankLevel = value; }
        }

        /// <summary>
        /// 公会当前人数
        /// </summary>
        public uint InfoGuildCurMember
        {
            get { return (uint)MbList.Count; }
        }

        /// <summary>
        /// 公会最大人数
        /// </summary>
        public uint InfoGuildMaxMember
        {
            get {
                return (uint)FindBaseVoByLevel(_infoGuildLevel).MaxMember;
            }
        }

        private uint _infoGuildGold;            //公会金币

        /// <summary>
        /// 公会当前金币
        /// </summary>
        public uint InfoGuildGold
        {
            get { return _infoGuildGold; }
            set { _infoGuildGold = value; }
        }

        private uint _infoGuildTotalGold;       //公会总计财富

        /// <summary>
        /// 公会总计财富
        /// </summary>
        public uint InfoGuildTotalGold
        {
            get { return _infoGuildTotalGold; }
            set { _infoGuildTotalGold = value; }
        }

        private string _infoGuildPost;

        /// <summary>
        /// 公会公告
        /// </summary>
        public string InfoGuildPost
        {
            get { return _infoGuildPost; }
            set { _infoGuildPost = value; }
        }


        #endregion

        private void InitialInfo()
        {
            _infoOwnType = GuildOffice.DeputyLeader;
            _infoOwnContribution = new IdStruct(8,99);
            _infoOwnTotalContribution = new IdStruct(8,100);
            _infoLeaderName = "金刚狼";
            _infoGuildLevel = 1;
            _infoGuildGold = 10;
            _infoGuildPost = "我是公告.....";
            _infoGuildTotalGold = 100;
            _infoRankLevel = 1002;
        }

        

    }
}