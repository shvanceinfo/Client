using UnityEngine;
using System.Collections;
using model;
using NetGame;
using MVC.entrance.gate;
using helper;
using System.Collections.Generic;

namespace manager
{
    public class SkillTalentManager
    {

        static SkillTalentManager _skillTalentManager;
        public bool isUpLevel;
        private Hashtable _skillHash;       //储存所有的表
        private Hashtable _talentHash;      //全部天赋
        private Hashtable _careerSkillList; //对应的职业技能表
        private CHARACTER_CAREER _type;
        private BetterList<SkillVo> _activeSkills;  //当前已有的技能表
        private BetterList<SkillVo> _lockSkills;    //当前锁定的技能表
        private BetterList<TalentVo> _activeTalents;//当前已有天赋
        private GCAskUseItem _askLevel;

        private int _skillPoint;

        public List<GameObject> effectObjList = new List<GameObject>(); //技能特效
        public void ClearEffect()
        {
            for (int i = 0; i < effectObjList.Count; i++)
            {
                if (effectObjList[i] != null)
                    DestroyObject.DestroyImmediate(effectObjList[i]);
            }
        }


        public SkillTalentManager()
        {
            _askLevel = new GCAskUseItem(0,0);

            _type = CHARACTER_CAREER.CC_BEGIN;
			isUpLevel=false;
            _skillHash = new Hashtable();
            _careerSkillList = new Hashtable();

            _talentHash = new Hashtable();
            _activeSkills = new BetterList<SkillVo>();

            _lockSkills = new BetterList<SkillVo>();
            _activeTalents = new BetterList<TalentVo>();


            int begin = (int)CHARACTER_CAREER.CC_BEGIN;
            int end = (int)CHARACTER_CAREER.CC_END;
            for (int i = begin+1; i < end; i++)
			{
			    _careerSkillList.Add((CHARACTER_CAREER)i,new Hashtable());
			}
            //CharacterPlayer.character_property.career
        }

        public void SetActiveSkill(uint id, int level)
        {
            SkillVo v=SkillHash[(int)id] as SkillVo;
            _type = (CHARACTER_CAREER)v.WeaponType;
            //if (isUpLevel)
            //{         
            //    uint oldID = id - 1;
            //    DeleteOldSkill(oldID);
            //    isUpLevel = false;
            //}
            
            //Hashtable  hs=GetCareerHash();
            //if(hs!=null)
            //{
            //    SkillVo sv = hs[(int)id] as SkillVo;
            //    if (sv.IsShow)
            //    {
            //        _activeSkills.Add(sv);  
            //    }
                
            //}
            //如果不存在列表，添加到列表中
            if (checkIsActive(v.SID))
            {
                for (int i = 0; i < _activeSkills.size; i++)
                {
                    if (_activeSkills[i].SID==v.SID)
                    {
                        _activeSkills[i] = v;
                        break;
                    }
                }
            }
            else {
                if (v.IsShow)
                {
                    _activeSkills.Add(v);
                    GuideInfoManager.Instance.AddGuideTrigger(v);
                }
            }


        }

        public void SetActiveTalents(uint id, int level)
        {
            if (isUpLevel)
            {
                uint oldId = id - 1;
                DeleteOldTalent(id);
                isUpLevel = false;
            }
            TalentVo tv=_talentHash[(int)id] as TalentVo;
            _activeTalents.Add(tv);
            GuideInfoManager.Instance.AddGuideTrigger(tv);
        }

        public void SetLockSkills()
        {
            _lockSkills.Clear();
            Hashtable hs = GetCareerHash();
            if (hs!=null)
            {
                foreach (SkillVo sv in hs.Values)
                {
                    if (!checkIsActive(sv.SID)&&!CheckIsLockExit(sv.SID)&&sv.IsShow)
                    {
                        int id = sv.SID * 1000 + 001;
                        SkillVo sdata= _skillHash[id] as SkillVo;
                        _lockSkills.Add(sdata);
                    }
                }
            }
            Gate.instance.sendNotification(MsgConstant.MSG_SKILL_INITIAL_SKILL_LIST);
        }

        public void SetRerushTalentUI()
        {
            Gate.instance.sendNotification(MsgConstant.MSG_SKILL_INITIAL_TALENTTABLE);
        }



        public void UnLockSkill(int XmlId)
        {
            SkillVo sv = _skillHash[XmlId] as SkillVo;

            if (!CheckIsHava(sv.UnLockType, sv.UnLockValue)) return;

            ViewHelper.DisplayMessageLanguage("skill_unlock_sucess");
            _askLevel.m_un16OperateID = (ushort)ResourceEnum.ReLockSkill;
            _askLevel.m_u32MaterialID = (uint)XmlId;
            NetBase.GetInstance().Send(_askLevel.ToBytes());

        }

        /// <summary>
        /// 发送升级解锁技能
        /// </summary>
        /// <param name="XmlId">长ID</param>
        public void SendLevelSkill(int XmlId)
        {
            SkillVo sv = _skillHash[XmlId] as SkillVo;

            if (sv.Level==100)
            {
                ViewHelper.DisplayMessage("技能已满级!");
                return;
            }
            if (sv.Level>=CharacterPlayer.character_property.level )
            {
                Gate.instance.sendNotification(MsgConstant.MSG_SKILL_ERROE_MSG, 
                    LanguageManager.GetText("skill_level_not"));
                return;
            }
            foreach (TypeStruct item in sv.Consume)
            {
                if (item.Type == ConsumeType.Gold)
                {
                    if (!ViewHelper.CheckIsHava((eGoldType)item.Id,item.Value))
                    {
                        return;
                    }
                }
                else {
                    if (!manager.FeebManager.Instance.CheckIsHave((uint)item.Id, item.Value))
                    {
                        return;
                    }
                }
            }
            ViewHelper.DisplayMessageLanguage("skill_lvlup_sucess");
            _askLevel.m_un16OperateID = (ushort)ResourceEnum.LevelSkill;
            _askLevel.m_u32MaterialID = (uint)XmlId;
            NetBase.GetInstance().Send(_askLevel.ToBytes());
            Gate.instance.sendNotification(MsgConstant.MSG_SKILL_EFFECT_INFO);
        }

        /// <summary>
        /// 获取对应ID的技能实例
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SkillVo GetSkillVo(int id)
        {
            if (_skillHash.ContainsKey(id))
            {
                return _skillHash[id] as SkillVo;
            }
            return null;
        }


        /// <summary>
        /// 升级天赋
        /// </summary>
        /// <param name="XmlId"></param>
        public void SendLevelTalent(int XmlId)
        {
            TalentVo tv = TalentHash[XmlId] as TalentVo;
            if (tv.Level >=CharacterPlayer.character_property.level)
            {
                Gate.instance.sendNotification(MsgConstant.MSG_SKILL_ERROE_MSG,
                   LanguageManager.GetText("skill_level_not"));
                return;
            }
            if (!CheckIsHava((eGoldType)tv.ComsumeType, tv.ComsumeValue)) return;

            ViewHelper.DisplayMessageLanguage("skill_talentlvlup_sucess");
            _askLevel.m_un16OperateID = (ushort)ResourceEnum.LevelTalent;
            _askLevel.m_u32MaterialID = (uint)XmlId;
            NetBase.GetInstance().Send(_askLevel.ToBytes());
            Gate.instance.sendNotification(MsgConstant.MSG_SKILL_EFFECT_TALENTINFO,XmlId);
        }

        private bool CheckIsHava(eGoldType t, int gold)
        {
            switch (t)
            {
                case eGoldType.gold:
                    if (CharacterPlayer.character_asset.gold < gold)
                    {
                        Gate.instance.sendNotification(MsgConstant.MSG_SKILL_ERROE_MSG, LanguageManager.GetText("msg_money_not_enough"));
                        return false;
                    }
                    break;
                case eGoldType.zuanshi:
                    if (CharacterPlayer.character_asset.diamond < gold)
                    {
                        Gate.instance.sendNotification(MsgConstant.MSG_SKILL_ERROE_MSG, LanguageManager.GetText("-99999945"));
                        return false;
                    }
                    break;
                case eGoldType.rongyu:
                    Gate.instance.sendNotification(MsgConstant.MSG_SKILL_ERROE_MSG, LanguageManager.GetText("-99999945"));
                    break;
                case eGoldType.fushi:
                    if (SkillPoint < gold)
                    {
                        Gate.instance.sendNotification(MsgConstant.MSG_SKILL_ERROE_MSG, LanguageManager.GetText("-99999956"));
                        return false;
                    }
                    break;
                default:
                    break;
            }
            return true;
        }
        //检查未解锁技能是否存在
        private bool CheckIsLockExit(int sid)
        {
            for (int i = 0; i < _lockSkills.size; i++)
            {
                if (_lockSkills[i].SID==sid)
                {
                    return true;
                }
            }
            return false;
        }
        private bool checkIsActive(int sid)
        {
            for (int i = 0; i < _activeSkills.size; i++)
            {
                if (sid==_activeSkills[i].SID)
                {
                    return true;
                }
            }
            return false;
        }

        //删除旧的技能
        private void DeleteOldSkill(uint id)
        {
            for (int i = 0; i < _activeSkills.size; i++)
            {
                if (_activeSkills[i].XmlID==(int)id)
                {
                    _activeSkills.RemoveAt(i);
                    break;
                }
            }
        }

        //删除旧的天赋
        private void DeleteOldTalent(uint id)
        {
            for (int i = 0; i < _activeTalents.size; i++)
            {
                if (_activeTalents[i].XmlId==(int)id)
                {
                    _activeTalents.RemoveAt(i);
                    break;
                }
            }
        }

        /// <summary>
        /// 获取对应的hash职业列表
        /// </summary>
        /// <returns></returns>
        public Hashtable GetCareerHash()
        {
            if (_type!=CHARACTER_CAREER.CC_BEGIN||_type!=CHARACTER_CAREER.CC_END)
            {
                Hashtable h = _careerSkillList[_type] as Hashtable;
                return h;
            }
            return null;
        }

        /// <summary>
        /// 读取对应的职业的技能
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SkillVo GetCareerSkill(int id)
        {
            Hashtable hash = GetCareerHash();
			if(hash != null)
			{
	            foreach (int key in hash.Keys)
	            {
	                if (key==id)
	                {
	                    return hash[key] as SkillVo;
	                }
	            }
			}
            return null;
        }

        /// <summary>
        /// 读取对应职业的普通攻击
        /// </summary>
        /// <returns></returns>
        public SkillVo GetCareerAttackSkill()
        {
            int id = 0;
            switch (CharacterPlayer.character_property.career)
            {
                case CHARACTER_CAREER.CC_BEGIN:
                    break;
                case CHARACTER_CAREER.CC_SWORD:
                    id = Constant.Fight_WarriorAttackId;
                    break;
                case CHARACTER_CAREER.CC_ARCHER:
                    id = Constant.Fight_ArcherAttackId;
                    break;
                case CHARACTER_CAREER.CC_MAGICIAN:
                    id = Constant.Fight_MagicAttackId;
                    break;
                case CHARACTER_CAREER.CC_END:
                    break;
            }
            return GetCareerSkill(id);
        }

        /// <summary>
        /// 读取对应职业的图集
        /// </summary>
        /// <returns></returns>
        public UIAtlas GetCareerAtals()
        { 
            string atlasPath="";
            switch (CharacterPlayer.character_property.career)
            {
                case CHARACTER_CAREER.CC_BEGIN:
                    break;
                case CHARACTER_CAREER.CC_SWORD:
                    atlasPath = Constant.Fight_Warrior_Atlas;
                    break;
                case CHARACTER_CAREER.CC_ARCHER:
                    atlasPath = Constant.Fight_Archer_Atlas;
                    break;
                case CHARACTER_CAREER.CC_MAGICIAN:
                    atlasPath = Constant.Fight_Magic_Atlas;
                    break;
                case CHARACTER_CAREER.CC_END:
                    break;
            }
            return BundleMemManager.Instance.loadResource(atlasPath, typeof(UIAtlas)) as UIAtlas;
        }

        public BetterList<SkillVo> ActiveSkills
        {
            get { return _activeSkills; }

        }


        public BetterList<SkillVo> LockSkills
        {
            get { return _lockSkills; }
        }


        public BetterList<TalentVo> ActiveTalents
        {
            get { return _activeTalents; }
        }

        public Hashtable CareerSkillList
        {
            get { return _careerSkillList; }
        }
        public Hashtable SkillHash
        {
            get { return _skillHash; }
        }
        public Hashtable TalentHash
        {
            get { return _talentHash; }
        }

        public bool IsNoData()
        {
            if (_activeSkills.size == 0 && _lockSkills.size == 0)
                return true;
            return false;
        }
        public static SkillTalentManager Instance
        {
            get {
                if (_skillTalentManager == null) _skillTalentManager = new SkillTalentManager();
                return _skillTalentManager;
            }

        }
        public int SkillPoint
        {
            get { return _skillPoint; }
            set { _skillPoint = value; }
        }

    }

}

