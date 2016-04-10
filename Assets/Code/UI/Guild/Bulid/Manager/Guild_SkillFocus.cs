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
        private int _skillFocusCurId;        //技能研究ID
        private int _skillFocusCurLevel;   //公会技能研究当前等级
        private int _skillFocusMaxLevel;

        private int _skillFocusCurType;
        private int _skillFocusMaxType;


        public BetterList<GuildSkillVo> SkillFocusNextVo;

        public int SkillFocusCurLevel
        {
            get { return _skillFocusCurLevel; }
            set { _skillFocusCurLevel = value; }
        }

        public int SkillFocusNextType
        {
            get
            {
                return (_skillFocusCurType + 1) > SkillFocusMaxType ? SkillFocusMaxType : (_skillFocusCurType + 1);
            }
        }

        public int SkillFocusMaxType
        {
            get
            {
                return _skillFocusMaxType;
            }
            set
            {
                if (value > _skillFocusMaxType)
                    _skillFocusMaxType = value;
            }
        }


        public int SkillFocusNextLevel
        {
            get
            {
                return _skillFocusCurLevel + 1 > SkillFocusMaxType ? SkillFocusMaxType : _skillFocusCurLevel + 1;
            }
        }

        public int SkillFocusMaxLevel
        {
            get
            {
                return _skillFocusMaxLevel;
            }
            set
            {
                if (value > _skillFocusMaxLevel)
                    _skillFocusMaxLevel = value;
            }
        }

        public GuildSkillVo SkillFocusCurVo
        {
            get
            {
                return FindSkillVoById(_skillFocusCurId);
            }
        }

        public void ShowSkillFocusNextVo()
        {
            SkillFocusNextVo.Clear();
            for (int i = 0; i < SkillFocusMaxType; i++)
            {
                SkillFocusNextVo.Add(FindSkillVoByIdLevel(i, SkillFocusNextLevel));
            }
        }

    }
}