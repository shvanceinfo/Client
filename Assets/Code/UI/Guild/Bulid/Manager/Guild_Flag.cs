using UnityEngine;
using System.Collections;
using model;
using helper;
using MVC.entrance.gate;

namespace manager
{
    /// <summary>
    /// 公会旗帜
    /// </summary>
    public partial class GuildManager
    {
        private GuildFlagType _flagSelectTable;  //当前选择的table
        private int _flagCurlvl;
        private int _flagMaxLevel;

        /// <summary>
        /// 公会旗帜等级
        /// </summary>
        public int FlagGuildLevel
        {
            get {
                return _flagCurlvl;
            }
        }
        
        /// <summary>
        /// 公会旗帜下个等级
        /// </summary>
        public int NextFlagGuildLevel
        {
            get {
                return (_flagCurlvl + 1) > FlagMaxLevel ? FlagMaxLevel : (_flagCurlvl + 1) ;
            }
        }

        private int FlagMaxLevel
        {
            get { return _flagMaxLevel; }
            set
            {
                if (value > _flagMaxLevel)
                {
                    _flagMaxLevel = value;
                }
            }
        }

        /// <summary>
        /// 当前公会的旗帜数据
        /// </summary>
        public GuildFlagVo FlagCurFlagVo
        {
            get {
                return FindFlagVoByLevel(FlagGuildLevel);
            }
        }

        /// <summary>
        /// 下一级公会的旗帜数据
        /// </summary>
        public GuildFlagVo FlagNextFlagVo
        {
            get
            {
                return FindFlagVoByLevel(NextFlagGuildLevel);
            }
        }

        public void FlagGetServderData(int id)
        {
            _flagCurlvl = id;
        }

        private void InitialFlag()
        {
            _flagSelectTable = GuildFlagType.GuildFlag;
        }

        public void FlagTableSwting(GuildFlagType type)
        {
            switch (type)
            { 
                case GuildFlagType.GuildFlag:
                    _flagSelectTable = type;
                    Gate.instance.sendNotification(MsgConstant.MSG_COMMON_NOTICE_FLAG_DISPLAYTABLE, _flagSelectTable);
                    break;
            }
        }

    }
}