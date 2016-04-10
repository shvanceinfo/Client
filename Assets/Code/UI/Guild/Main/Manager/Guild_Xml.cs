using UnityEngine;
using System.Collections;
using model;
using helper;

namespace manager
{
    /// <summary>
    /// 公会配置表
    /// 表数据设计为只读，无特殊需要请不要任意修改表数据
    /// </summary>
    public partial class GuildManager
    {

        private GuildManager()
        {
            InitialXml();
            InitialInfo();
            InitialBulid();
            InitialEvent();
            InitialSkill();
            InitialShop();
            InitialMemberCheck();
            InitialFlag();
            InitialCenter();
        }
        #region 单例
        private static GuildManager _instance;
        public static GuildManager Instance
        {
            get
            {
                if (_instance == null) _instance = new GuildManager();
                return _instance;
            }
        }
        #endregion

        #region Hashtables
        
        private Hashtable _xmlGuildBase;

        /// <summary>
        /// 公会Base表
        /// </summary>
        public Hashtable XmlGuildBase
        {
            get { return _xmlGuildBase; }
        }

        private Hashtable _xmlGuildOffice;

        /// <summary>
        /// 公会官职表
        /// </summary>
        public Hashtable XmlGuildOffice
        {
            get { return _xmlGuildOffice; }
        }


        private Hashtable _xmlGuildSkill;

        /// <summary>
        /// 公会技能表
        /// </summary>
        public Hashtable XmlGuildSkill
        {
            get { return _xmlGuildSkill; }
            set { _xmlGuildSkill = value; }
        }

        private Hashtable _xmlGuildBuildFlag;

        /// <summary>
        /// 公会旗帜表
        /// </summary>
        public Hashtable XmlGuildBuildFlag
        {
            get { return _xmlGuildBuildFlag; }
            set { _xmlGuildBuildFlag = value; }
        }

        private Hashtable _xmlGuildBuildCenter;

        /// <summary>
        /// 公会大厅
        /// </summary>
        public Hashtable XmlGuildBuildCenter
        {
            get { return _xmlGuildBuildCenter; }
            set { _xmlGuildBuildCenter = value; }
        }

        private Hashtable _xmlGuildEvent;

        /// <summary>
        /// 公会活动
        /// </summary>
        public Hashtable XmlGuildEvent
        {
            get { return _xmlGuildEvent; }
            set { _xmlGuildEvent = value; }
        }

        private Hashtable _xmlGuildShop;

        /// <summary>
        /// 公会商店
        /// </summary>
        public Hashtable XmlGuildShop
        {
            get { return _xmlGuildShop; }
            set { _xmlGuildShop = value; }
        }

        private Hashtable _xmlGuildBuild;

        /// <summary>
        /// 公会建筑列表
        /// </summary>
        public Hashtable XmlGuildBuild
        {
            get { return _xmlGuildBuild; }
            set { _xmlGuildBuild = value; }
        }
        #endregion

        #region Finds

        /// <summary>
        /// 根据公会等级查找base表
        /// </summary>
        /// <param name="guildLevel">公会等级</param>
        /// <returns></returns>
        public GuildBaseVo FindBaseVoByLevel(uint guildLevel)
        {
            GuildBaseVo vo=null;
            IDictionaryEnumerator it=_xmlGuildBase.GetEnumerator();
            while (it.MoveNext())
            {
                vo=it.Value as GuildBaseVo;
                if ((uint)vo.Level == guildLevel)
                    return vo;
            }
#if UNITY_EDITOR
            Debug.LogError("Function \"FindBaseVoByLevel\"  guildLevel is not in _xmlGuildBase!");
#endif
            return null;
        }

        /// <summary>
        /// 根据职位类型找实例
        /// </summary>
        /// <param name="of">官职</param>
        /// <returns></returns>
        public GuildOffcerVo FindOffcerVoByType(GuildOffice of)
        {
            GuildOffcerVo vo = null;
            IDictionaryEnumerator it = _xmlGuildOffice.GetEnumerator();
            while (it.MoveNext())
            {
                vo = it.Value as GuildOffcerVo;
                if (vo.OfficeLevel == of)
                    return vo;
            }
#if UNITY_EDITOR
            Debug.LogError("Function \"FindOffcerVoByType\"  GuildOffice is not in _xmlGuildOffice!");
#endif
            return null;
        }

        /// <summary>
        /// 根据旗帜等级找旗帜数据
        /// </summary>
        /// <param name="level">旗帜等级</param>
        /// <returns></returns>
        public GuildFlagVo FindFlagVoByLevel(int level)
        {
            GuildFlagVo vo = null;
            IDictionaryEnumerator it = _xmlGuildBuildFlag.GetEnumerator();
            while (it.MoveNext())
            {
                vo = it.Value as GuildFlagVo;
                if ((int)vo.Level == level)
                if ((uint)vo.Level == level)
                    return vo;
            }
#if UNITY_EDITOR
            Debug.LogError("Function \"FindFlagVoByType\"  GuildFlagVo is not in _xmlGuildBuildFlag!");
#endif
            return null;
        }


        /// <summary>
        /// 根据技能id,level找公会技能实例
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public GuildSkillVo FindSkillVoByIdLevel(int type,int level)
        {
            GuildSkillVo vo = null;
            IDictionaryEnumerator it = _xmlGuildSkill.GetEnumerator();
            while (it.MoveNext())
            {
                vo = it.Value as GuildSkillVo;
                if ((int)vo.Type == type && vo.Level == level)
                    return vo;
            }
#if UNITY_EDITOR
            Debug.LogError("Function \"FindFlagVoByType\"  GuildFlagVo is not in _xmlGuildBuildFlag!");
#endif
            return null;
        }

        public GuildSkillVo FindSkillVoById(int id)
        {
            GuildSkillVo vo = null;
            IDictionaryEnumerator it = _xmlGuildSkill.GetEnumerator();
            while (it.MoveNext())
            {
                vo = it.Value as GuildSkillVo;
                if ((int)vo.ID == id)
                    return vo;
            }
#if UNITY_EDITOR
            Debug.LogError("Function \"FindFlagVoByType\"  GuildFlagVo is not in _xmlGuildBuildFlag!");
#endif
            return null;
        }



        /// <summary>
        /// 根据公会等级找公会大厅实例
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public GuildCenterVo FindCenterVoByType(uint guildLevel)
        {
            GuildCenterVo vo = null;
            IDictionaryEnumerator it = _xmlGuildBuildCenter.GetEnumerator();
            while (it.MoveNext())
            {
                vo = it.Value as GuildCenterVo;
                if ((uint)vo.Level == guildLevel)
                    return vo;
            }
#if UNITY_EDITOR
            Debug.LogError("Function \"FindCenterVoByType\"  GuildCenterVo is not in _xmlGuildBuildCenter!");
#endif
            return null;
        }

        /// <summary>
        /// 根据ID找工会建筑数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public GuildBulidVo FindBulidVoById(int id)
        {
            GuildBulidVo vo = null;
            IDictionaryEnumerator it = XmlGuildBuild.GetEnumerator();
            while (it.MoveNext())
            {
                vo = it.Value as GuildBulidVo;
                if ((uint)vo.Id == id)
                    return vo;
            }
#if UNITY_EDITOR
            Debug.LogError("Function \"FindBulidVoById\"  GuildBulidVo is not in XmlGuildBuild!");
#endif
            return null;
        }


        #endregion

        private void InitialXml()
        {
            _xmlGuildBase = new Hashtable();
            _xmlGuildOffice = new Hashtable();
            _xmlGuildBuildFlag = new Hashtable();
            _xmlGuildBuildCenter = new Hashtable();
            _xmlGuildEvent = new Hashtable();
            _xmlGuildShop = new Hashtable();
            _xmlGuildBuild = new Hashtable();
            _xmlGuildSkill = new Hashtable();
        }

        #region FormatString

        /// <summary>
        /// 获取职位对应的名称
        /// </summary>
        /// <param name="of"></param>
        /// <returns></returns>
        public static string FormatOffcerString(GuildOffice of)
        {
            return Instance.FindOffcerVoByType(of).Name;
        }

        /// <summary>
        /// 格式化公会大厅需要财富数量
        /// </summary>
        /// <param name="gold"></param>
        /// <returns></returns>
        public static string FormatCenterNeed(BetterList<TypeStruct> consume)
        {
            string f1=LanguageManager.GetText("guild_center_level_up_gold");
            string f2=LanguageManager.GetText("guild_center_level_up_item");
            string text = "";
            for (int i = 0; i < consume.size; i++)
            {
                if (consume[i].Type == ConsumeType.Gold)
                {
                    text+=string.Format(f1,consume[i].Value);
                }
                else { 
                    text+=string.Format(f2,ItemManager.GetInstance().GetTemplateByTempId((uint)consume[i].Id).name,consume[i].Value);
                }
            }
            return text;
        }

        public static string FormatBuildOption(GuildBulidVo vo)
        {
            if (vo.OpenLevel > Instance.InfoGuildLevel)
            {
                return ViewHelper.FormatLanguage("guild_build_open_level",vo.OpenLevel);
            }
            else {
                return ViewHelper.FormatLanguage("guild_build_level_title", vo.Level);
            }
        }

        
        #endregion
    }
}