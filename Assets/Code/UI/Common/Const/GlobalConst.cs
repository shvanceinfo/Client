/**该文件实现的基本功能等
function: 实现文件的相关常量定义
author:ljx
date:2013-11-09
**/
//Ui名称的常量
public class UiNameConst
{
	public const string ui_pack = "ui_pack";
	public const string ui_root = "ui_root";
	public const string ui_dialog = "ui_dialog";
    public const string ui_dialog1 = "ui_dialog1";
    public const string ui_dialog2 = "ui_dialog2";
	public const string ui_dialog_fight = "ui_dialog_fight";
	public const string ui_main = "ui_main";
	public const string ui_map = "ui_map";
	public const string ui_award = "ui_award";
	public const string ui_born = "ui_born";
	public const string ui_fight = "ui_fight";
	public const string ui_pause = "ui_pause";
	public const string ui_waitting = "ui_waitting";
	public const string ui_waitting_fight = "ui_waitting_fight";
	public const string ui_skill = "ui_skill";
	public const string ui_role = "ui_role";
	public const string ui_demon = "ui_demon";
    public const string ui_demonAnce = "ui_demonAnce";
	public const string ui_rank = "ui_rank";
	public const string ui_chat = "ui_chat";
	public const string ui_load = "ui_load";
	public const string ui_setting = "ui_setting";
	public const string ui_shop = "ui_shop";
	public const string ui_service = "ui_service";
	public const string ui_notice = "ui_notice";
	public const string ui_golden_goblin = "ui_golden_goblin";
    public const string ui_golden_gain = "ui_golden_gain";
    public const string ui_head_goblin_buff = "head_goblin_buff";
	public const string ui_task = "ui_task";
	public const string ui_task_dialog = "ui_task_dialog";
	public const string ui_scenario = "ui_scenario";
	public const string ui_wing = "ui_wing";
    public const string ui_arena = "ui_arena";
    public const string ui_sphere_effects = "ui_sphere_effects";
    public const string ui__cube_effects = "ui_cube_effects";
	public const string ui_hero_list = "ui_hero_list";
	public const string ui_honor_shop = "ui_honor_shop";
	public const string ui_arena_result = "ui_arena_result";
	public const string ui_map_info = "ui_map_info";
	public const string ui_sweep = "ui_sweep";
	public const string ui_battle_log = "ui_battle_log";
	public const string ui_duoren = "ui_duoren";
	public const string ui_dungeoninfo = "ui_dungeoninfo";
	public const string ui_dungeondeadinfo = "ui_dungeondeadinfo";
	public const string ui_camera = "Camera";
	public const string ui_raid = "ui_raid";
	public const string ui_raid_item = "ui_raid_item";
	public const string ui_bag = "ui_bag";
    public const string ui_email = "ui_email";
    public const string ui_furnace = "ui_furnace";
    public const string ui_luckstone = "ui_luck";
	public const string ui_common_tips = "ui_common_tips";
    public const string ui_equip = "ui_equip";
	public const string ui_sale = "ui_sale";
	public const string ui_equip_tips = "ui_equip_tips";
	public const string ui_open = "ui_open";
	public const string ui_chapter_award = "ui_chapter_award";
	public const string ui_common_notice = "ui_common_notice";
    public const string ui_power = "ui_power";
    public const string ui_quickbuy = "ui_quickbuy";
	public const string ui_new_item = "ui_new_item";
	public const string ui_event = "ui_event";
	public const string ui_boss_win = "ui_boss_win";
    public const string ui_vip = "ui_vip";
	public const string ui_boss_dead= "ui_boss_dead";
	public const string ui_new_item_use = "ui_new_item_use";
    public const string ui_friend = "ui_friend";
    public const string ui_guild = "ui_guild";
    public const string ui_pet = "ui_pet";
    public const string ui_monster_reward = "ui_monster_reward";
    public const string ui_guildcenter = "ui_guild_center";
    public const string ui_guildflag = "ui_guild_flag";
    public const string ui_guildskill = "ui_guild_skill";
    public const string ui_guildshop = "ui_guild_shop";
    public const string ui_guilddonate = "ui_guild_donate";
    public const string ui_guildlist = "ui_guild_list";
    public const string ui_guildcreate = "ui_create_guild";
	public const string ui_pandora = "ui_pandora";
    public const string ui_plot = "ui_plot";
    public const string ui_new_function = "ui_new_function";
    public const string ui_medal = "ui_medal";
    public const string ui_channel = "ui_channel";
	public const string ui_pet_equip_tips = "ui_pet_equip_tips";
}

//存储路径的常量
public class PathConst
{
    public const string MEDAL_ICON_PATH = "Picture/xunzhang/";
    public const string VIP_PIC_PATH = "Picture/VIPpic/";
    public const string SKILL_PATH = "Picture/Skill/";
	public const string TALENT_PATH = "Picture/Tianfu/";
	public const string ICON_PATH = "Picture/Icon/";
	public const string NPC_ICON_PATH = "Picture/NPCpicture/";
	public const string RAID_CHAPTER_PATH = "Picture/background/";
	public const string RAID_PREVIEW_PATH = "Picture/mapPreview/";
    public const string OTHER_PATH = "Picture/Other/";
	public const string PET_SKILL_PATH = "Picture/Petpicture/";
	public const string LANGUAGE_PATH = "Config/Language/zh_cn";
	public const string RAND_NAME_PATH = "Config/Language/name";
	public const string UI_PREFAB_PATH = "Config/Component/Ui_Path"; //UI预制件的位置
	public const string HEAD_BOARD_PREFAB_PATH = "UI/HeadBoard/head_board"; //UI预制件的位置
    public const string TRANSFER_POINT_HEAD_BOARD_PREFAB_PATH = "UI/HeadBoard/transfer_point_head_board"; //UI预制件的位置
	public const string EASY_TOUCH_PATH = "UI/Effect/Input/EasyTouchJoyStick";	//摇杆
	public const string PLAYER_CAMERA = "Model/prefab/playerCamera";			//摄像机
	public const string FLOW_MSG_PATH = "UI/Effect/FloatMsg/new_flow_msg";  //漂字
	public const string WATER_EFFECT_PATH = "Model/customs pass/textures_bg/water1/WATER_"; //注意这个可能没打包到picture中
	public const string NPC_TAG_PATH = "Effect/Effect_Prefab/Task/"; //NPC标记
	public const string FLOAT_BLOOD_NUM = "UI/Effect/FloatMsg/float_blood_num"; //飘血的标记
	public const string ARENA_COUNT_DOWN = "UI/Effect/countDown"; //竞技场倒计时
	public const string ARENA_BEGIN_EFFECT = "Effect/Effect_Prefab/UI/JingJiChang_KaiZhan"; //竞技场开始界面
    public const string MODEL_CAMERA = "Model/prefab/modelCamera"; //模型相机
    public const string LEAD_WAY_CAMERA = "Effect/Effect_Prefab/Role/Other/JianTou"; //指引道路的model
    public const string SKILL_CD_PARTICLE = "Effect/Effect_Prefab/Role/Other/CD_lengque"; //指引道路的model
	
	public const string PATH_PREFIX = "Config/Data/"; 	//数据配置文件的前缀
	public const string TIP_PATH = PATH_PREFIX + "Tips";
	public const string SKILL_DATA_PATH = PATH_PREFIX + "Skilldata";
	public const string SKILL_EXPRESSION_PATH = PATH_PREFIX + "Skillexpression";
	public const string SKILL_EFFECT_PATH = PATH_PREFIX + "Skilleffect";
	public const string MONSTER_DATA_PATH = PATH_PREFIX + "Monsterdata";
	public const string EQUIP_PATH = PATH_PREFIX + "Equipment";
	public const string ITEM_PATH = PATH_PREFIX + "Item";
	public const string MAP_PATH = PATH_PREFIX + "Map";
	public const string CHAPTER_PATH = PATH_PREFIX + "Zhangjie";
	public const string ENERGY_PATH = PATH_PREFIX + "TiLi";
	public const string STRENGHTEN_PATH = PATH_PREFIX + "Intensify";
	public const string ROLE_PATH = PATH_PREFIX + "Role"; //角色数据
    public const string DIAMOND_PATH = PATH_PREFIX + "Gem";
	public const string FORGE_PATH = PATH_PREFIX + "equipmentforge";
	public const string TOWER_PATH = PATH_PREFIX + "Tower";
	public const string MISSION_PATH = PATH_PREFIX + "Mission";
	public const string HELP_PATH = PATH_PREFIX + "Help";
	public const string MODEL_POS_PATH = PATH_PREFIX + "ModelPos";
	public const string FORMULA_PATH = PATH_PREFIX + "Formula";
	public const string TASK_PATH = PATH_PREFIX + "Task";
	public const string NPC_PATH = PATH_PREFIX + "NPC";
	public const string SKILL_POINT_PATH = PATH_PREFIX + "SkillPoint";
	public const string SCENARIO_PATH = PATH_PREFIX + "Plot";
	public const string WING_PATH = PATH_PREFIX + "Wing";
	public const string HONOR_SHOP_PATH = PATH_PREFIX + "HonorShop";
	public const string ARENA_AWARD_PATH = PATH_PREFIX + "HonorRank_reward";
	public const string HONOR_RANK_PATH = PATH_PREFIX + "HonorRank";
	public const string MAP_TIPS_PATH = PATH_PREFIX + "MapTips";
	public const string AI_PATH = PATH_PREFIX + "AI";
	public const string MONSTER_INSTANCE_PATH = PATH_PREFIX + "monsterInstance";
	public const string VIP_PATH = PATH_PREFIX + "VIP";
    public const string VIP_PRG_PATH = PATH_PREFIX + "VIP_duibi";
	public const string GBL_PATH = PATH_PREFIX + "Gbl_goumai";
	public const string HP_MP_PATH = PATH_PREFIX + "Xuelanyao";
	public const string ARENA_POS_PATH = PATH_PREFIX + "ArenaPos";
	public const string SKILL_TALENT_PATH = PATH_PREFIX + "Skill_tianfu";
    public const string TOWER_RANK_PATH = PATH_PREFIX + "TowerRank_reward";
	public const string RELIVE_PATH = PATH_PREFIX + "Fuhuo";
    public const string SHOP_PATH = PATH_PREFIX + "Shop";
    public const string LUCKSTONE_PATH = PATH_PREFIX + "LuckStone";
    public const string EQUIPMENT_PATH = PATH_PREFIX + "EquipmentPart";
    public const string REFINE_PATH = PATH_PREFIX + "EquipmentIdentify";
	public const string BUNDLE_PATH = PATH_PREFIX + "AssetBundles";
	public const string EVENT_PATH = PATH_PREFIX + "Huodong";
    public const string LEACHDIRTY = PATH_PREFIX + "LeachDirty";
	public const string PUBLICDATA_PATH = PATH_PREFIX + "PublicData";
    public const string LOOT_PATH = PATH_PREFIX + "Loot";
    public const string FUNC_PATH = PATH_PREFIX + "Function";
    public const string MEDAL_PATH = PATH_PREFIX+"Xunzhang";
    public const string GUILD_BASE_PATH = PATH_PREFIX + "GongHuiBase";
    public const string GUILD_BUILD_PATH = PATH_PREFIX + "GongHuiBuild";
    public const string GUILD_BUILDCENTER_PATH = PATH_PREFIX + "GongHuiBuildCentre";
    public const string GUILD_BUILDFLAG_PATH = PATH_PREFIX + "GongHuiBuildFlag";
    public const string GUILD_BUILDSHOP_PATH = PATH_PREFIX + "GongHuiBuildShop";
    public const string GUILD_BUILDSKILL_PATH = PATH_PREFIX + "GongHuiBuildSkill";
    public const string GUILD_EVENT_PATH = PATH_PREFIX + "GongHuiEvent";
    public const string GUILD_LOG_PATH = PATH_PREFIX + "GongHuiLog";
    public const string GUILD_OFFICER_PATH = PATH_PREFIX + "GongHuiOfficer";
    public const string GUILD_SHOP_PATH = PATH_PREFIX + "GongHuiShop";
    public const string GUILD_SKILL_PATH = PATH_PREFIX + "GongHuiSkill";
    public const string PET_PATH = PATH_PREFIX + "PetBase";
    public const string MONSTER_REWARD_PATH = PATH_PREFIX + "Xuanshang";
    public const string SETTING_DISPLAY_PATH = PATH_PREFIX + "LineMaxPerson";
    public const string CHANNEL_PATH = PATH_PREFIX + "GameLine";
    public const string DROPOUT_PATH = PATH_PREFIX + "DropOut";
    public const string GUIDE_PATH = PATH_PREFIX + "Guide";
    public const string JIGUAN_PATH = PATH_PREFIX + "Jiguan";
	public const string PANDORA_PATH = PATH_PREFIX + "PandoraMap";
	public const string PANDORANUM_PATH = PATH_PREFIX + "PandoraNum";
    public const string GUIDEINFO_PATH = PATH_PREFIX + "GuideInfo";

    //特效prefab
    public const string FLOAT_MESSAGE = "UI/Effect/FloatMsg/float_message"; //飘字
	public const string FLOAT_BLAST_MESSAGE = "UI/Effect/FloatMsg/float_blast_message"; //暴击
	public const string FLOAT_MESSAGE_OLD = "UI/Effect/FloatMsg/float_message_old"; //飘字
    public const string FLOAT_MESSAGE_AWARD = "UI/Effect/FloatMsg/float_award_item"; //飘奖励
	public const string FLOAT_LEVELUP = "UI/Effect/FloatMsg/float_levelup"; //升级
	public const string FLOAT_ADVANCE = "UI/Effect/FloatMsg/float_advance"; //升阶	
    public const string GOBULIN_BUFF = "UI/HeadBoard/head_goblin_buff"; //哥布林buff
    public const string GOBULIN_GOLD = "Model/prefab/BattlePref/gold_gbl/gold_gbl"; //哥布林金币
    public const string TOLL_GATE_ITEM1 = "Effect/Effect_Prefab/UI/UI_guanka_wupin1"; //关卡物品1
    public const string TOLL_GATE_ITEM2 = "Effect/Effect_Prefab/UI/UI_guanka_wupin2"; //关卡物品2
    public const string MOSTER_SELECT_FLAG = "Effect/Effect_Prefab/Monster/xuanzhong";   //怪物选中标志
    public const string MOSTER_HP_BAR = "Prefab/UiFight/ui_hp_bar";    //怪物血条
    public const string GOBULIN_UI = "huangjingebulin_UI"; //哥布林UI
    public const string SKILLEFFECT = "Effect_Prefab/UI/UI_Trail";

    //技能prefab
    public const string FIRE_RAIN_PREFAB = "Skill/FireRain";

    //音乐的prefab
    public const string AUDIO_BREAK = "break";
    public const string AUDIO_UP_LEVEL = "shengji";
    public const string AUDIO_XUANFENGZHAN = "xuanfengzhan";
    public const string AUDIO_DOOR_OPEN = "door_open";
    public const string AUDIO_DOOR_CLOSE = "door_close";
}

public class SpriteNameConst
{
    public const string COMMON_ATLAS = "Common Atlas";
	public const string RAID_ACTIVE_SUFFIX = "_liang";
	public const string RAID_INACTIVE_SUFFIX = "_an";
	public const string NORMAL_RAID_ACTIVE_SP = "guanka_pt_liang"; 		//普通激活按钮
	public const string HARD_RAID_ACTIVE_SP = "guanka_jy_liang";			//精英激活按钮
	public const string NORMAL_RAID_INACTIVE_SP = "guanka_pt_an";		//普通非激活按钮
	public const string HARD_RAID_INACTIVE_SP = "guanka_jy_an";			//精英非激活按钮
	public const string GATE_TAG_ICON = "guanka";
}