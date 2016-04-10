using UnityEngine;
using System.Xml;
using model;
using manager;

public class ConfigDataManager
{

    static private ConfigDataManager sConfigDataManager;

    private DataReadSkill skill_config;
    private DataReadSkillExpression skill_expression_config;
    private DataReadSkillEffect skill_effect_config;
    private DataReadMonster monster_config;
    private DataReadEquip equip_config;
    private DataReadGoods goods_config;
    private DataReadMap map_config;
    private DataReadIntensify intensify_config;
    private DataReadRole role_config;
    private DataReadSkillPoint skill_point_config;

    /// 新模版 2013/8/23
    private DataReadItem item_config;
    private DataReadEquipment equipment_config;
    private DataReadGem gem_config;
    private DataReadFormula formula_config;
    private DataReadEquipmentForge equipment_forge_config;

    private DataReadTower tower_config;

    private DataReadLoadingTips loading_tips_config;
    private DataReadHelp helpConfig;
    private DataModelPos modelPos;

    private DataReadMapTips map_tips_config; 

    private DataReadAI ai_config;


    private DataReadMonsterInstance monster_instance_config;

    private DataReadVIP readVip;//vip信息表
    private DataReadPrivileges readPrg; //特权表
    private DataReadGBL readGbl;//哥布林购买次数信息表

    private DataReadHPMP hp_mp_config;

    private DataReadRelive relive_config;


    private DataArenaPos readArenaPos;//竞技场界面人物位置

    private DataReadTalent readTalent;  //天赋属性表

    private DataReadRankReward readRank;

    private DataReadLuckStone readLuckStone;

    private DataReadEquipmentPart readPart;

    private DataReadEquipmentRefine readRefine;
	
	//private DataReadPreloadEffects m_ReadEffects;
	
	private DataReadEvent readEvent;

    private DataReadPublicData readPublicData;

    private DataReadLoot readLoot;


    private DataReadFunction readFuc;
    private DataReadMedal readMedal;

	
	private DataReadPet readPet;
    private DataReadGuildBase readGuildBase;
    private DataReadGuildBulid readGuildBulid;
    private DataReadGuildCenter readGuildCenter;
    private DataReadGuildEvent readGuildEvent;
    private DataReadGuildFlag readGuildFlag;
    private DataReadGuildOffcer readGuildOffcer;
    private DataReadGuildShop readGuildShop;
	private DataReadMonsterReward readMonsterReward;

    private DataReadSettingDisplay readDisplay;
    private DataReadChannel readChannel;
    private DataReadDropOut readDropOut;
    private DataReadGuide readGuide;

    private DataReadJiGuan readJiGuan;

	private DataReadPandora readPandora;
	private DataReadPandoraNum readPandoraNum;
    private DataReadGuideInfo readGuideInfo;
    private ConfigDataManager()
    {
        //init();
    }

    static public ConfigDataManager GetInstance()
    {
        if (sConfigDataManager == null)
        {
            sConfigDataManager = new ConfigDataManager();
        }
        return sConfigDataManager;
    }

    public void init()
    {
        //DataReadLoadingTips 放置在data中
        loading_tips_config = new DataReadLoadingTips();
        loading_tips_config.path = PathConst.TIP_PATH;
        loading_tips_config.init();
        read_config(loading_tips_config);

        //DataReadSkill 放置在data中
        skill_config = new DataReadSkill();
        skill_config.path = PathConst.SKILL_DATA_PATH;
        skill_config.init();
        read_config(skill_config);

        //DataReadSkillExpression 放置在data中
        skill_expression_config = new DataReadSkillExpression();
        skill_expression_config.path =  PathConst.SKILL_EXPRESSION_PATH;
        skill_expression_config.init();
        read_config(skill_expression_config);

        //DataReadSkillEffect 放置在data中
        skill_effect_config = new DataReadSkillEffect();
        skill_effect_config.path = PathConst.SKILL_EFFECT_PATH;
        skill_effect_config.init();
        read_config(skill_effect_config);

        //DataReadMonster 放置在data中
        monster_config = new DataReadMonster();
        monster_config.path = PathConst.MONSTER_DATA_PATH; 
        monster_config.init();
        read_config(monster_config);

        //DataReadEquip 放置在data中
        equip_config = new DataReadEquip();
        equip_config.path = PathConst.EQUIP_PATH;
        equip_config.init();
        read_config(equip_config);

        //物品模版 放置在data中
        item_config = new DataReadItem();
        item_config.path = PathConst.ITEM_PATH;
        item_config.init();
        read_config(item_config);

        //先读章节,再读地图，不放置在data中
        DataReadChapter chaptermap_config = new DataReadChapter();
        chaptermap_config.path = PathConst.CHAPTER_PATH;
        chaptermap_config.init();
        read_config(chaptermap_config);

        //地图放置在data中
        map_config = new DataReadMap();
        map_config.path = PathConst.MAP_PATH;
        map_config.init();
        read_config(map_config);

        //体力 不放置在data中
        DataReadEngery engeryConfig = new DataReadEngery();
        engeryConfig.path = PathConst.ENERGY_PATH; 
        engeryConfig.init();
        read_config(engeryConfig);

        //强化 放置在data中
        intensify_config = new DataReadIntensify();
        intensify_config.path = PathConst.STRENGHTEN_PATH; 
        intensify_config.init();
        read_config(intensify_config);

        //角色数据 放置在data中
        role_config = new DataReadRole();
        role_config.path = PathConst.ROLE_PATH; 
        role_config.init();
        read_config(role_config);

        //装备模版 放置在data中
        equipment_config = new DataReadEquipment();
        equipment_config.path = PathConst.EQUIP_PATH;
        equipment_config.init();
        read_config(equipment_config);

        //宝石模版 不放置在data中
        gem_config = new DataReadGem();
        gem_config.path = PathConst.DIAMOND_PATH; 
        gem_config.init();
        read_config(gem_config);

        //道具模板 不放置在data中
        formula_config = new DataReadFormula();
        formula_config.path = PathConst.FORMULA_PATH;
        formula_config.init();
        read_config(formula_config);

        //强化模版 不放置在data中
        equipment_forge_config = new DataReadEquipmentForge();
        equipment_forge_config.path = PathConst.FORGE_PATH; 
        equipment_forge_config.init();
        read_config(equipment_forge_config);

        //tower_config 不放置在data中
        tower_config = new DataReadTower();
        tower_config.path = PathConst.TOWER_PATH; 
        tower_config.init();
        read_config(tower_config);

        //帮助模板
        helpConfig = new DataReadHelp();
        helpConfig.path =PathConst.HELP_PATH;
        helpConfig.init();
        read_config(helpConfig, 1);

        //位置信息
        modelPos = new DataModelPos();
        modelPos.path = PathConst.MODEL_POS_PATH; 
        modelPos.init();
        read_config(modelPos);

        //任务
        DataReadTask readTask = new DataReadTask();
        readTask.path = PathConst.TASK_PATH; 
        read_config(readTask);
        TaskManager.Instance.sortTaskOrder(); //根据顺序排序所有的任务

        //NPC
        DataReadNPC readNPC = new DataReadNPC();
        readNPC.path = PathConst.NPC_PATH;
        read_config(readNPC);

        //技能点
        skill_point_config = new DataReadSkillPoint();
        skill_point_config.path = PathConst.SKILL_POINT_PATH; 
        skill_point_config.init();
        read_config(skill_point_config);

        //剧情
        DataReadScenario readScenario = new DataReadScenario();
        readScenario.path = PathConst.SCENARIO_PATH;
        read_config(readScenario, 2);

        //读取翅膀数据
        DataReadWing readWing = new DataReadWing();
        readWing.path = PathConst.WING_PATH; 
        read_config(readWing);

        //读取荣誉商店
        DataReadHonorShop readHonorShop = new DataReadHonorShop();
        readHonorShop.path = PathConst.HONOR_SHOP_PATH; 
        read_config(readHonorShop);

        //读取竞技场奖励
        DataReadArenaAward arenaAward = new DataReadArenaAward();
        arenaAward.path = PathConst.ARENA_AWARD_PATH; 
        read_config(arenaAward);

        //读取对应荣誉值
        DataReadHonorLevel readHonorLevel = new DataReadHonorLevel();
        readHonorLevel.path = PathConst.HONOR_RANK_PATH; 
        read_config(readHonorLevel);

        map_tips_config = new DataReadMapTips();
        map_tips_config.path = PathConst.MAP_TIPS_PATH; 
        map_tips_config.init();
        read_config(map_tips_config);

        ai_config = new DataReadAI();
        ai_config.path = PathConst.AI_PATH;
        ai_config.init();
        read_config(ai_config);

        //放置在 data当中
        monster_instance_config = new DataReadMonsterInstance();
        monster_instance_config.path = PathConst.MONSTER_INSTANCE_PATH;
        monster_instance_config.init();
        read_config(monster_instance_config);
        monster_instance_config.ParseData();
		monster_instance_config.ParseMapData();

        readVip = new DataReadVIP();
        readVip.path = PathConst.VIP_PATH;
        readVip.init();
        read_config(readVip);

        readPrg = new DataReadPrivileges();
        readPrg.path = PathConst.VIP_PRG_PATH;
        readPrg.init();
        read_config(readPrg);

        readGbl = new DataReadGBL();
        readGbl.path = PathConst.GBL_PATH; 
        readGbl.init();
        read_config(readGbl);

        hp_mp_config = new DataReadHPMP();
        hp_mp_config.path = PathConst.HP_MP_PATH;
        hp_mp_config.init();
        read_config(hp_mp_config);

        relive_config = new DataReadRelive();
        relive_config.path = PathConst.RELIVE_PATH;
        relive_config.init();
        read_config(relive_config);

        readArenaPos = new DataArenaPos();
        readArenaPos.path = PathConst.ARENA_POS_PATH; 
        readArenaPos.init();
        read_config(readArenaPos);

        //读取天赋技能
        readTalent = new DataReadTalent();
        readTalent.path = PathConst.SKILL_TALENT_PATH; 
        readTalent.init();
        read_config(readTalent);

        //读取恶魔洞窟排名奖励表
        readRank = new DataReadRankReward();
        readRank.path = PathConst.TOWER_RANK_PATH;
        readRank.init();
        read_config(readRank);

        readLuckStone = new DataReadLuckStone();
        readLuckStone.path = PathConst.LUCKSTONE_PATH;
        readLuckStone.init();
        read_config(readLuckStone);

		//monster_instance_config.ParseData();

        readPart = new DataReadEquipmentPart();
        readPart.path = PathConst.EQUIPMENT_PATH;
        readPart.init();
        read_config(readPart);

        //读取洗练表
        readRefine = new DataReadEquipmentRefine();
        readRefine.path = PathConst.REFINE_PATH;
        readRefine.init();
        read_config(readRefine);
		
        //m_ReadEffects = new DataReadPreloadEffects();
        //m_ReadEffects.path = PathConst.PRELOADEFFECT_PATH;
        //m_ReadEffects.init();
        //read_config(m_ReadEffects);
		
		readEvent = new DataReadEvent();
		readEvent.path = PathConst.EVENT_PATH;
		readEvent.init();
		read_config(readEvent);
		
		readPublicData = new DataReadPublicData();
		readPublicData.path = PathConst.PUBLICDATA_PATH;
		readPublicData.init();
		read_config(readPublicData);

        readLoot = new DataReadLoot();
        readLoot.path = PathConst.LOOT_PATH;
        readLoot.init();
        read_config(readLoot);

        readFuc = new DataReadFunction();
        readFuc.path = PathConst.FUNC_PATH;
        readFuc.init();
        read_config(readFuc);
		FastOpenManager.Instance.Init();

        readMedal = new DataReadMedal();
        readMedal.path = PathConst.MEDAL_PATH;
        readMedal.init();
        read_config(readMedal);
		
		readPet = new DataReadPet();
		readPet.path = PathConst.PET_PATH;
		readPet.init();
		read_config(readPet);

        readGuildBase = new DataReadGuildBase();
        readGuildBase.path = PathConst.GUILD_BASE_PATH;
        readGuildBase.init();
        read_config(readGuildBase);

        readGuildBulid = new DataReadGuildBulid();
        readGuildBulid.path = PathConst.GUILD_BUILD_PATH;
        readGuildBulid.init();
        read_config(readGuildBulid);

        readGuildCenter = new DataReadGuildCenter();
        readGuildCenter.path = PathConst.GUILD_BUILDCENTER_PATH;
        readGuildCenter.init();
        read_config(readGuildCenter);

        readGuildEvent = new DataReadGuildEvent();
        readGuildEvent.path = PathConst.GUILD_EVENT_PATH;
        readGuildEvent.init();
        read_config(readGuildEvent);

        readGuildFlag = new DataReadGuildFlag();
        readGuildFlag.path = PathConst.GUILD_BUILDFLAG_PATH;
        readGuildFlag.init();
        read_config(readGuildFlag);

        readGuildOffcer = new DataReadGuildOffcer();
        readGuildOffcer.path = PathConst.GUILD_OFFICER_PATH;
        readGuildOffcer.init();
        read_config(readGuildOffcer);

        readGuildShop = new DataReadGuildShop();
        readGuildShop.path = PathConst.GUILD_SHOP_PATH;
        readGuildShop.init();
        read_config(readGuildShop);
		
		readMonsterReward = new DataReadMonsterReward();
		readMonsterReward.path = PathConst.MONSTER_REWARD_PATH;
		readMonsterReward.init();
		read_config(readMonsterReward);

        readDisplay = new DataReadSettingDisplay();
        readDisplay.path = PathConst.SETTING_DISPLAY_PATH;
        readDisplay.init();
        read_config(readDisplay);

        readChannel = new DataReadChannel();
        readChannel.path = PathConst.CHANNEL_PATH;
        readChannel.init();
        read_config(readChannel);

        readDropOut = new DataReadDropOut();
        readDropOut.path = PathConst.DROPOUT_PATH;
        readDropOut.init();
        read_config(readDropOut);

        readJiGuan = new DataReadJiGuan();
        readJiGuan.path = PathConst.JIGUAN_PATH;
        readJiGuan.init();
        read_config(readJiGuan);

        readGuide = new DataReadGuide();
        readGuide.path = PathConst.GUIDE_PATH;
        readGuide.init();
        read_config(readGuide);
        GuideManager.Instance.Init();   

		readPandora = new DataReadPandora ();
		readPandora.path = PathConst.PANDORA_PATH;
		readPandora.init ();
		read_config(readPandora);

		readPandoraNum = new DataReadPandoraNum ();
		readPandoraNum.path = PathConst.PANDORANUM_PATH;
		readPandoraNum.init ();
		read_config(readPandoraNum);

        readGuideInfo = new DataReadGuideInfo();
        readGuideInfo.path = PathConst.GUIDEINFO_PATH;
        readGuideInfo.init();
        read_config(readGuideInfo);


		LeachDirtyManager.Instance.DoSplit("freeConfig");
    }

    void read_config(DataReadBase dataBase, int special = 0)
    {

        Debug.Log("begin read config : " + dataBase.path);
        TextAsset ta = null;
        if (BundleMemManager.debugVersion)
        {
            ta = BundleMemManager.Instance.loadResource(dataBase.path) as TextAsset;
        }
        else
        {
            AssetBundle bundle = BundleMemManager.Instance.ConfigBundle;
            ta = bundle.Load(ToolFunc.TrimPath(dataBase.path)) as TextAsset;
        }
        if (ta)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(ta.text);
            XmlNodeList nodeList = xmlDoc.SelectSingleNode(dataBase.getRootNodeName()).ChildNodes;
            for (int k = 0; k < nodeList.Count; k++)
            {
                XmlElement xe = nodeList.Item(k) as XmlElement;
                if (xe == null)
                    continue;
                string key = xe.GetAttribute("ID");
                for (int i = 0; i < xe.Attributes.Count; i++)
                {
                    XmlAttribute attr = xe.Attributes[i];
                    try
                    {
                        dataBase.appendAttribute(int.Parse(key), attr.Name, attr.Value);
                    }
                    catch (System.Exception ex)
                    {
                        int iii = 1;
                    }

                }
                if (special == 1) //读取帮助
                {
                    DataReadHelp help = (DataReadHelp)dataBase;
                    XmlNodeList childNodes = xe.ChildNodes;
                    foreach (XmlElement xe1 in childNodes)
                    {
                        XmlAttribute attr1 = xe1.Attributes[0];
                        help.getHelpInfo(int.Parse(key)).helpContent.Add(attr1.Value);
                    }
                }
                else if (special == 2) //读取剧情
                {
                    DataReadScenario scenario = (DataReadScenario)dataBase;
                    XmlNodeList childNodes = xe.ChildNodes;
                    foreach (XmlElement xe1 in childNodes)
                    {
                        int step = int.Parse(xe1.GetAttribute("ID"));
                        for (int i = 0; i < xe1.Attributes.Count; i++)
                        {
                            XmlAttribute attr = xe1.Attributes[i];
                            scenario.appendSubAttri(uint.Parse(key), step, attr.Name, attr.Value);
                        }
                    }
                }
            }
            Loger.Log("end read config : " + dataBase.path);
        }
    }

    public DataReadLoadingTips getLoadingTipsConfig()
    {
        return loading_tips_config;
    }

    public DataReadSkill getSkillConfig()
    {
        return skill_config;
    }

    public DataReadSkillExpression getSkillExpressionConfig()
    {
        return skill_expression_config;
    }

    public DataReadSkillEffect getSkillEffectConfig()
    {
        return skill_effect_config;
    }

    public DataReadMonster getMonsterConfig()
    {
        return monster_config;
    }

    public DataReadEquip getEquipmentConfig()
    {
        return equip_config;
    }

    public DataReadGoods getGoodConfig()
    {
        return goods_config;
    }

    public DataReadMap getMapConfig()
    {
        return map_config;
    }

    public DataReadIntensify getIntensifyConfig()
    {
        return intensify_config;
    }

    public DataReadRole getRoleConfig()
    {
        return role_config;
    }

    /// <summary>
    /// 物品模版
    /// </summary>
    /// <returns></returns>
    public DataReadItem getItemTemplate()
    {
        return item_config;
    }
    /// <summary>
    /// 宝石
    /// </summary>
    /// <returns></returns>
    public DataReadGem getGemTemplate()
    {
        return gem_config;
    }
    /// <summary>
    /// 装备强化
    /// </summary>
    /// <returns></returns>
    public DataReadEquipmentForge getEquipmentForgeTemplate()
    {
        return equipment_forge_config;
    }
    /// <summary>
    /// 装备
    /// </summary>
    /// <returns></returns>
    public DataReadEquipment getEquipmentTemplate()
    {
        return equipment_config;
    }

    public DataReadTower getTowerConfig()
    {
        return tower_config;
    }

    //帮助配置文件
    public DataReadHelp getHelpConfig()
    {
        return helpConfig;
    }

    //pos,rotate
    public DataModelPos getModelPos()
    {
        return modelPos;
    }

    public DataReadSkillPoint getSkillPointConfig()
    {
        return skill_point_config;
    }

    public DataReadMapTips getMapTipsConfig()
    {
        return map_tips_config;
    }


    public DataReadAI getAIConfig()
    {
        return ai_config;
    }

    public DataReadMonsterInstance getMonsterInstanceConfig()
    {
        return monster_instance_config;
    }

    public DataReadVIP getVIPConfig()
    {
        return this.readVip;
    }

    public DataReadGBL getGBLConfig()
    {
        return this.readGbl;
    }

    public DataReadHPMP GetHPMPConfig()
    {
        return hp_mp_config;
    }

    public DataArenaPos getArenaPos()
    {
        return this.readArenaPos;
    }

    public DataReadRelive GetReliveConfig()
    {
        return relive_config;
    }

    /// <summary>
    /// 获取天赋配置
    /// </summary>
    /// <returns></returns>
    public DataReadTalent getTalent()
    {
        return this.readTalent;
    }
 	
    //public DataReadPreloadEffects GetPreloadConfig()
    //{
    //    return this.m_ReadEffects;
    //}
	
	public DataReadPublicData GetPublicDataConfig(){
		return this.readPublicData;
	}


    public DataReadJiGuan GetJiGuanConfig()
    {
        return readJiGuan;
    }
}

