using UnityEngine;
using System.Collections;
using model;
using MVC.entrance.gate;

namespace manager
{
    /// <summary>
    /// 公会大厅
    /// </summary>
    public partial class GuildManager
    {

        private GuildFlagType _centerSelectTable;  //当前选择的table

        private int _centerCurlvl;
        private int _centerNextlvl;
        private int _centerMaxLevel;

        /// <summary>
        /// 当前公会大厅信息
        /// </summary>
        public GuildCenterVo CtCurCenterVo
        {
            get {
                return FindCenterVoByType((uint)_centerCurlvl);
            }
        }

        public int CenterCurlvl
        {
            get
            {
                return _centerCurlvl;
            }
        }

        public int CenterNextLvl
        {
            get
            {
                return (_centerCurlvl + 1) > CenterMaxLvl ? CenterMaxLvl : (_centerCurlvl + 1);
            }
        }

        public int CenterMaxLvl
        {
            get
            {
                return _centerMaxLevel;
            }
            set
            {
                if (value > _centerMaxLevel)
                    _centerMaxLevel = value;
            }
        }

        public GuildCenterVo GuildCenterNextVo
        {
            get
            {
                return FindCenterVoByType((uint)CenterNextLvl);
            }
        }


        public void CenterTableSwting(GuildFlagType type)
        {
            switch (type)
            {
                case GuildFlagType.GuildFlag:
                    _centerSelectTable = type;
                    Gate.instance.sendNotification(MsgConstant.MSG_COMMON_NOTICE_CENTER_REFRESH, _centerSelectTable);
                    break;
            }
        }

        public void InitialCenter()
        {
            _flagSelectTable = GuildFlagType.GuildFlag;
        }

        public void CenterGetServderData(int id)
        {
            _centerCurlvl = id;
        }
    }
}