using UnityEngine;
using System.Collections;
using model;
using helper;
using MVC.entrance.gate;

namespace manager
{
    /// <summary>
    /// 公会技能
    /// </summary>
    public partial class GuildManager
    {
        private GuildSkillType _skillSelectTable;  //当前选择的table
        private int _skillCurId;        //技能ID
        private int _skillCurLevel;   //公会技能当前等级
        private int _skillMaxLevel;

        private int _skillCurType;
        private int _skillMaxType;


        public BetterList<GuildSkillVo> SkillNextVo;
        public BetterList<GuildSkillVo> SkillBeforeVo;

        public int SkillCurLevel
        {
            get { return _skillCurLevel; }
            set { _skillCurLevel = value; }
        }

        public int SkillNextType
        {
            get
            {
                return (_skillCurType + 1) > SkillMaxType ? SkillMaxType : (_skillCurType + 1);
            }
        }

        public int SkillMaxType
        {
            get
            {
                return _skillMaxType;
            }
            set
            {
                if (value > _skillMaxType)
                    _skillMaxType = value;
            }
        }
        

        public int SkillNextLevel
        {
            get
            {
                return _skillCurLevel + 1 > SkillMaxLevel ? SkillMaxLevel : _skillCurLevel + 1;
            }
        }

        public int SkillMaxLevel
        {
            get 
            {
                return _skillMaxLevel;
            }
            set
            {
                if (value > _skillMaxLevel)
                    _skillMaxLevel = value;
            }
        }

        public GuildSkillVo SkillCurVo
        {
            get
            {
                return FindSkillVoById(_skillCurId);
            }
        }

        public void ShowSkillCurVo()
        {
            SkillNextVo.Clear();
            for (int i = 0; i < SkillMaxType; i++)
            {
                SkillBeforeVo.Add(FindSkillVoByIdLevel(i, _skillCurLevel));
            }
        }

        public void ShowSkillNextVo()
        {
            SkillNextVo.Clear();
            for (int i = 0; i < SkillMaxType; i++)
            {
                SkillNextVo.Add(FindSkillVoByIdLevel(i, SkillNextLevel));
            }
        }

        public void SkillTableSwting(GuildSkillType type)
        {
            switch (type)
            {
                case GuildSkillType.GuildSkillLearn:
                    _skillSelectTable = type;
                    Gate.instance.sendNotification(MsgConstant.MSG_COMMON_NOTICE_SKILL_DISPLAYTABLE, _skillSelectTable);
                    break;
                case GuildSkillType.GuildSkillFocus:
                    _skillSelectTable = type;
                    Gate.instance.sendNotification(MsgConstant.MSG_COMMON_NOTICE_SKILL_DISPLAYTABLE, _skillSelectTable);
                    break;
            }
        }

        private void InitialSkill()
        {
            SkillNextVo = new BetterList<GuildSkillVo>();
            SkillBeforeVo = new BetterList<GuildSkillVo>();
            SkillFocusNextVo = new BetterList<GuildSkillVo>();
            _skillSelectTable = GuildSkillType.GuildSkillLearn;
        }
    }
}