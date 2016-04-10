/**该文件实现的基本功能等
function: MVC框架通信的基本消息
author:ljx
date:2013-11-09
**/

public class MsgConstant
{
	//通用的消息，1-100号预留通用的消息
	public const uint MSG_CLOSE_UI = 1; //关闭UI窗口
	public const uint MSG_SURE_DIALOG = 2; //确认对话框消息
	public const uint MSG_ENABLE_MODEL_CAMERA = 3; //激活模型相机
	public const uint MSG_DISABLE_MODEL_CAMERA = 4; //禁止模型相机
	public const uint MSG_DESTROY_MODEL_CAMERA = 5; //清除模型相机
	public const uint MSG_INIT_POWER_ENGERY = 6; //初始化体力

	//翅膀的消息常量
	public const uint MSG_OPEN_WING = 101; //打开翅膀界面
	public const uint MSG_WING_INIT = 102; //收到服务器消息，初始化翅膀界面
	public const uint MSG_WING_CULTURE = 103;   //点击了培养按钮
	public const uint MSG_WING_AUTO_CULTURE = 104;   //点击自动培养
	public const uint MSG_WING_EVOLUTION = 105; //进化
	public const uint MSG_WING_AUTO_EVOLUTION = 106; //自动进化
	public const uint MSG_SHOW_CULTURE = 107;	  	//显示培养界面
	public const uint MSG_SHOW_EVOLUTION = 108;	//显示进阶界面
	public const uint MSG_WING_UPDATE_EXP = 109;	  //经验条的变化
	public const uint MSG_WING_UPDATE_MONEY = 110;  //金钱钻石的变化
	public const uint MSG_WING_UPDATE_LUCK = 111;	  //成功率，幸运点的变化
	public const uint MSG_WING_UPDATE_MODEL_LADDER = 112;  //更新阶数的时候同时更新模型
	public const uint MSG_WING_SHOW_EFFECT = 113;  //每次培养进阶的时候显示文字以及特效
	public const uint MSG_WING_NOT_ENOUGH_MSG = 114;	  //弹出材料不足的信息
	public const uint MSG_WING_STOP_AUTO = 115;	  //退出自动培养，自动进阶
	public const uint MSG_WING_DRAG_NEXT = 116;	  //预览下一个翅膀
	public const uint MSG_WING_DRAG_PREV = 117;	  //预览上一个翅膀
	public const uint MSG_WING_SET_CAMERA = 118;    //设置摄像机
    public const uint MSG_WING_SHOW_TIME = 119;		//显示清空时间
    public const uint MSG_WING_SHOW_RETURN = 120;		//显示清空时间
    public const uint MSG_WING_SHOW_SUCCESS = 121;		//成功进阶

	//竞技场的消息常量
	public const uint MSG_OPEN_ARENA = 201;   		//点击打开竞技场
	public const uint MSG_OPEN_HERO_BOARD = 202;   	//打开英雄榜
	public const uint MSG_ARENA_CHALLENGE = 203; 	//挑战
	public const uint MSG_ARENA_CLEAR_CD = 204; 	//清除竞技场CD
	public const uint MSG_ARENA_BUY_NUM = 205; 		//购买挑战次数
	public const uint MSG_ARENA_COUNT_DOWN = 206;   //倒计时的消息
	public const uint MSG_ARENA_SET_CHALLENGE_NUM = 207; //挑战次数的变化
	public const uint MSG_ARENA_RESULT_INFO = 208; 		//战报信息
	public const uint MSG_ARENA_COUNT_DOWN_OVER = 209; 		//倒计时结束
    public const uint MSG_ARENA_OPEN_AWARD=210;             //打开奖励面板
    public const uint MSG_ARENA_SEND_RECEIVE_AWARD = 211;   //发送领取奖励
    public const uint MSG_ARENA_AWARD_BTN = 212;            //奖励按钮
    public const uint MSG_ARENA_AWARD_REFRESH = 213;            //奖励刷新

	//荣誉商店的消息常量
	public const uint MSG_OPEN_HONOR_SHOP = 301;   //点击打开荣誉商店
	public const uint MSG_HONOR_CLOSE = 302;   	//点击关闭按钮，可能要保存竞技场界面打开
	public const uint MSG_HONOR_EQUIP = 303;   	//点击装备标签
	public const uint MSG_HONOR_TOOL = 304;   	//点击工具标签
	public const uint MSG_HONOR_DIAMOND = 305; 	//点击宝石标签
	public const uint MSG_HONOR_OTHER = 306; 	//点击其他标签
	public const uint MSG_HONOR_BUY = 307; 		//点击兑换按钮

	//扫荡界面的常量
	public const uint MSG_OPEN_SMALL_MAP_INFO = 401;   //点击打开地图信息
	public const uint MSG_OPEN_SWEEP = 402;   //点击打开扫荡界面
	public const uint MSG_SWEEP_ADD_NUM = 403;   	//点击增加扫荡次数
	public const uint MSG_SWEEP_SUBSTRACT_NUM = 404;   	//点击减少扫荡次数
	public const uint MSG_SWEEP_START = 405;   	//点击开始扫荡
	public const uint MSG_SWEEP_STOP = 406; 	//点击停止扫荡
	public const uint MSG_SWEEP_ACCELERATE = 407; 	//点击加速扫荡
	public const uint MSG_ENTER_RAID = 408; 	//点击进入副本
	public const uint MSG_SWEEP_SHOW_RESULT = 409; 	//显示扫荡的结果
	public const uint MSG_SWEEP_MAX = 410;		//点击得到最大的扫荡数量
	public const uint MSG_SWEEP_SHOW_FINAL_RESULT = 411;	//显示扫荡的最终结果
	
	//哥布林的消息常量
	public const uint MSG_OPEN_GOBLIN = 501;
	public const uint MSG_GOBLIN_INIT = 502;    //哥布林界面的初始化
	public const uint MSG_GOBLIN_UPDATE_ENTER_TIMES = 503;//剩余的进入副本的次数
	public const uint MSG_ENTER_GOBLIN = 504; //进入哥布林副本
	public const uint MSG_GOBLIN_CAN_BUY_NUM = 505; //可以购买的次数
	public const uint MSG_GOBLIN_BUY_PRICE = 506; //当前副本的价格
	public const uint MSG_CLEAR_GOBLIN_BUY_PRICE = 507;//清空哥布林价格的信息
	public const uint MSG_GOBLIN_BUY_TIMES = 508;//购买次数



	//技能界面的消息常量 
	public const uint MSG_SKILL_ASK_TALENTDATA = 601; //请求天赋数据
	/// <summary>
	/// 刷新天赋界面
	/// </summary>
	public const uint MSG_SKILL_INITIAL_TALENTTABLE = 602;//天赋数据请求得到，刷新天赋页面
	public const uint MSG_SKILL_LEVEL_TALENT = 603;     //升级天赋
    public const uint MSG_SKILL_ASK_SKILL_LIST = 604;//请求技能列表
	/// <summary>
	/// 刷新技能UI
	/// </summary>
    public const uint MSG_SKILL_INITIAL_SKILL_LIST = 605;//技能列表已经获取，刷新界面
    public const uint MSG_SKILL_LEVEL_SKILL = 606;        //升级技能
    public const uint MSG_SKILL_TABLE_SWITCHING = 607;  //切换界面
    public const uint MSG_SKILL_GET_DATA = 608;            //向服务器获取信息
    public const uint MSG_SKILL_UI_LOAD = 609;             //没有数据，第一次加载界面
    public const uint MSG_SKILL_LEVEL_SKILL_COMPLATE = 610;    //升级技能成功
    public const uint MSG_SKILL_LEVEL_TALENT_COMPLATE = 611;    //升级天赋成功
    public const uint MSG_SKILL_UNLOCK_SKILL = 612;          //解锁技能
    public const uint MSG_SKILL_ERROE_MSG = 613;               //报错
    public const uint MSG_SKILL_CALLBACK_DISPLAY_INFO = 614;
    public const uint MSG_SKILL_EFFECT_INFO = 615;          //技能特效
    public const uint MSG_SKILL_EFFECT_TALENTINFO = 616;//请求天赋列表
    

	//多人副本的消息常量
	public const uint MSG_OPEN_DUNGEON = 701;	//打开副本页面
	public const uint MSG_DUNGEON_INIT = 702;	//初始化副本页面
	public const uint MSG_UPDATE_DUNGEON = 703;	//更新副本信息
	public const uint MSG_DUNGEON_NEXT = 704;	//查看下个副本
	public const uint MSG_DUNGEON_PREV = 705;	//查看上个副本
	public const uint MSG_UPDATE_TEAM_LIST = 706;//更新打副本的队伍信息
	public const uint MSG_CREATE_TEAM = 707; 	//创建队伍
	public const uint MSG_JOIN_TEAM = 708; 		//进入队伍
	public const uint MSG_QUICK_JOIN = 709; 	//快速进入队伍
	public const uint MSG_START_DUNGEON = 710;	//开始副本
	public const uint MSG_LEAVE_TEAM = 711;		//离开队伍
	public const uint MSG_OPEN_DUNGEONINFO = 712;//开打详细副本界面
	public const uint MSG_UPDATE_DUNGEON_PEOPLE_LIST = 713;//更新副本人员信息
	public const uint MSG_SHOW_CD = 714;		//开始副本前的倒计时面板
	public const uint MSG_OPEN_DUNGEONDEAD = 715;//死亡界面
	public const uint MSG_DUNGEON_FAIL = 716;	//全部死亡
	public const uint MSG_DUNGEON_BUY_REVIVE = 717;//购买复活
	public const uint MSG_DUNGEONDEAD_ERROR = 718;//副本死亡错误信息
	public const uint MSG_CLOSE_TEAM = 719;		//从组队中退出
	public const uint MSG_DUNGEON_MAIN_PLAYER_DROP = 720;	//战斗中主机掉线

	//恶魔洞窟消息常量
	public const uint MSG_DEMON_DISPLAY_ITEMLIST = 801; // 刷新今日挑战目标
	public const uint MSG_DEMON_RECEIVE_ITEM = 802; // 发送接收挑战物品MSG
	public const uint MSG_DEMON_DISPLAY_CUR_RANK = 803; // 设置显示今日排名
	public const uint MSG_DEMON_DISPLAY_RANK = 804; // 设置显示昨日排名奖励
	public const uint MSG_DEMON_RERUSH_CUR_RANK_UI = 805; // 刷新今日排名对应的排名列表
	public const uint MSG_DEMON_RECEIVE_AWARD = 806;      //收到服务器返回结果
	public const uint MSG_DEMON_ENTER_TOWER = 807;            //进入恶魔洞窟
	public const uint MSG_DEMON_ENTER_HISTORY_AWARD = 808;  //请求昨日奖励数据
	public const uint MSG_DEMON_DIALOG_SURE = 809;          //确定消耗钻石或水晶
	public const uint MSG_DEMON_INITIAL_DATA = 810;           //打开恶魔洞窟面板初始化数据

	//战斗界面
	public const uint MSG_FIGHT_SEND_USE_SKILL = 901;       //发送使用技能
	public const uint MSG_FIGHT_REFRESH_LEVEL = 902;      //刷新等级显示
	public const uint MSG_FIGHT_REFRESH_HEALT_MAGIC = 903;//刷新生命魔法值
	public const uint MSG_FIGHT_USE_HHEAL_MAGIC_ITEM = 904;   //使用血瓶物品
	public const uint MSG_FIGHT_REFRESH_HPMP_VIEW = 905;      //刷新血瓶界面
	public const uint MSG_FIGHT_INITI_ENTER = 906;            //进入副本，更新一次血瓶信息
	public const uint MSG_FIGHT_EXP_CHANGE = 907;             //经验变化
	public const uint MSG_FIGHT_BOSS_SHOW = 908;            //显示BOSS血条
	public const uint MSG_FIGHT_BOSS_HIDEEN = 909;          //隐藏BOSS血条
	public const uint MSG_FIGHT_BOSS_HEALTBAR = 910;        //刷新boss血量
	public const uint MSG_FIGHT_DISPLAY_AWARD_ITEM = 911;     //显示奖励物品
	public const uint MSG_FIGHT_DELETE_AWARD_ITEM = 912;        //删除奖励物品 
	public const uint MSG_FIGHT_DISPLAY_AWARD_ITEM_DATA_COROUTINE = 913;//同步显示数据
	public const uint MSG_FIGHT_MULTI_PLAYER_DEAD = 914;          //玩家死亡
	public const uint MSG_FIGHT_MULTI_PLAYER_DISCONNECT = 915;  //玩家断线
	public const uint MSG_FIGHT_MULTI_PLAYER_HEALT_CHANGE = 916;//血量改变
	public const uint MSG_FIGHT_DISPLAY_TIME = 917;               //开始计时
	public const uint MSG_FIGHT_STOP_TIME = 918;                  //停止计时
	public const uint MSG_FIGHT_DISPLAY_GOBLIN_FUCNTION = 919;    //显示哥布林组件
	public const uint MSG_FIGHT_DISPLAY_GOBLIN_GOLD_NUM = 920;  //显示哥布林金币
	public const uint MSG_FIGHT_DISPLAY_BOSS_TIPS = 921;          //显示Tip
	public const uint MSG_FIGHT_DISPLAY_MAP_NAME_MSG = 922;          //显示地图名称
	public const uint MSG_FIGHT_DISPLAY_TALK_MSG = 923;             //更新聊天信息
	public const uint MSG_FIGHT_WORLD_BOSS_UPDATE_INFO = 924;		//更新世界BOSS信息
	public const uint MSG_FIGHT_BOSS_BTN_SWITCH = 925;			//boss伤害按钮切换
    public const uint MSG_FIGHT_COMBO = 926;
	public const uint MSG_FIGHT_BUFF_COUNT = 927;				//buff的次数
	public const uint MSG_FIGHT_UPDATE_FUNCTION = 928;			//更新功能模块显示
    public const uint MSG_FIGHT_DISPLAY_ARENA_PLAYER = 929;     //竞技场其他人个人信息
    public const uint MSG_FIGHT_DISPLAY_ARENA_PLAYER_HEALTH = 930;//竞技场其他人血量
	public const uint MSG_FIGHT_UPDATE_ASSET = 931;				//资源更新
    public const uint MSG_FIGHT_DISPLAY_PET_SKILL = 932;

	public const uint MSG_AWARD_INITIAL_DATA = 1001;            //发送进入副本，初始化记录数据
	public const uint MSG_AWARD_GO_HOME = 1002;                   //发送回城
	public const uint MSG_AWARD_DISPALY_INFO = 1003;            //显示详细信息
	//主界面
	public const uint MSG_MAIN_PEOPLEINFO = 1101;	//更新人员信息
	public const uint MSG_MAIN_UPDATE_ENGERY = 1102;//更新人员的体力
	public const uint MSG_MAIN_UPDATE_ASSET = 1103;	//更新人员的资源信息
	public const uint MSG_MAIN_UPDATE_PROPERTY = 1104;//更新人员的属性信息
	public const uint MSG_MAIN_UPDATE_CURRENCY = 1105;//更新人员的金钱
	public const uint MSG_MAIN_UPDATE_MONEY = 1106;	//更新人员的钻石
	public const uint MSG_MAIN_UPDATE_EMAIL = 1107; //更新邮箱状态
	public const uint MSG_MAIN_UPDATA_TALK = 1108;  //更新聊天信息
	public const uint MSG_MAIN_UPDATE_VIP = 1109;
	public const uint MSG_MAIN_UPDATE_FUNCTION = 1110;//更新功能模块显示
    public const uint MSG_MIAN_UPDATE_CHANNEL = 1111;//更新频道信息
    public const uint MSG_MAIN_CLOSE_MENU = 1112;       //关闭menu
    public const uint MSG_MAIN_PUSH_TRIGGER = 1113;     //添加新手提示

	public const uint MSG_DEATH_TO_CITY = 1201;     //回城
	public const uint MSG_DEATH_REVIVE = 1202;      //复活


	//创建角色
	public const uint MSG_CREATEROLE_SETNAME = 1301;              //设置名字
	public const uint MSG_CREATEROLE_CREATE = 1302;             //创建角色
	public const uint MSG_CREATEROLE_SELECT_CAREER = 1303;      //选择角色
	public const uint MSG_CREATEROLE_DISPLAY_NAME = 1304;         //显示名字
	public const uint MSG_CREATEROLE_DISPLAY_PLAYER = 1305;     //显示换角色
	public const uint MSG_CREATEROLE_ROLL_NAME = 1306;            //随机名字
	public const uint MSG_CREATEROLE_ERROR = 1307;                  //显示名字错误

	//弹出对话框
	public const uint MSG_DIALOG_SURE = 1401;	//确认对话框
	public const uint MSG_DIALOG_CANCEL = 1402;	//取消对话框
	public const uint MSG_DIALOG_SHOW = 1403;	//显示

	//副本的相应常量
	public const uint MSG_RAID_OPEN = 1501; //打开关卡界面
	public const uint MSG_RAID_BTN_CLICK_NORMAL = 1502; //点击普通按钮
	public const uint MSG_RAID_BTN_CLICK_HARD = 1503; //点击精英按钮
	public const uint MSG_RAID_BTN_CLICK_MAP = 1504; //点击地图关卡按钮
	public const uint MSG_RAID_BTN_CLICK_ADD_ENERGY = 1505; //点击增加体力
	public const uint MSG_RAID_BTN_SHOW_PREV = 1506; //显示上一关卡
	public const uint MSG_RAID_BTN_SHOW_NEXT = 1507; //显示上一关卡
	public const uint MSG_RAID_BTN_CLICK_CLOSE = 1508; //点击关闭按钮
	public const uint MSG_RAID_BTN_UPDATE_CHAPTER_AWARD = 1509;	//更新关卡奖励信息

	//背包功能
	public const uint MSG_BAG_SHOWCAREERMODEL = 1601;	//显示模型
	public const uint MSG_BAG_SHOWEQUIPDATA = 1602;		//显示装备数据
	public const uint MSG_BAG_SHOWALLITEM = 1603;		//显示所有道具
	public const uint MSG_BAG_SWITCHTAB = 1604;			//切换tab按钮
	public const uint MSG_BAG_SHOWEQUIPITEM = 1605;		//显示所有装备的道具信息
	public const uint MSG_BAG_SHOWNORMALITEM = 1606;	//显示所有非装备的道具信息
	public const uint MSG_BAG_SALE = 1607;				//出售道具
	public const uint MSG_BAG_HIDECAREERMODEL = 1608;	//隐藏模型
	public const uint MSG_BAG_UPDATE_MODEL_WEAPON = 1609;//更新武器模型
    public const uint MSG_BAG_UPDATE_MODEL_ARMOR = 1610; //更新衣服模型
    public const uint MSG_BAG_UPDATE_DIAMOND_GOLD = 1611; //更新钻石和金币排列顺序

	//商城
	public const uint MSG_SHOP_SET_BUY_ITEM = 1701;         //购买某个商品 int
	public const uint MSG_SHOP_DISPLAY_BUY_INFO = 1702; //显示购买商品信息
	public const uint MSG_SHOP_DISPLAY_BUY_COUNT = 1703;//显示购买商品数量
	public const uint MSG_SHOP_SET_BUY_COUNT = 1704;//设置购买数量
	public const uint MSG_SHOP_BUY_PANEL_OPTION = 1705; //购买的操作
	public const uint MSG_SHOP_TABLE_SWTING = 1706;           //切换页签
	public const uint MSG_SHOP_DISPLAY_TABLE = 1707;        //显示页签内容
	public const uint MSG_SHOP_DISPLAY_MONEY = 1708;        //金钱变化
	public const uint MSG_SHOP_ADD_RMB = 1709;              //冲钱

	//恶魔洞窟死亡界面
	public const uint MSG_DEMON_ARCE_DISPLAY_PAGE = 1801;   //显示恶魔洞窟通关信息界面

	//角色界面

	public const uint MSG_ROLE_SHOWCAREERMODEL = 1901;	//显示模型
	public const uint MSG_ROLE_HIDECAREERMODEL = 1902;	//隐藏模型
	public const uint MSG_ROLE_SHOWEQUIPDATA = 1903;	//显示当前装备的数据
	public const uint MSG_ROLE_SHOWPEOPLEINFO = 1904;	//显示用户信息


	//邮件页面
	public const uint MSG_EMAIL_DISPLAY_EMAIL_INFO = 2001;  //显示邮件详细信息
	public const uint MSG_EMAIL_READ_EMAIL = 2002;          //读取邮件
	public const uint MSG_EMAIL_RECEIVE = 2003;             //领取或删除
	public const uint MSG_EMAIL_HIDE_INFO = 2004;           //隐藏详细信息
	public const uint MSG_EMAIL_CLOSE = 2005;               //关闭窗口
	public const uint MSG_EMAIL_DISPLAY_EMAIL_LIST = 2006;  //刷新邮件列表
    public const uint MSG_SELECT_INDEX_EMAIL = 2007;        //选中第一个邮件
    public const uint MSG_SELECT_EMAIL_COUNT = 2008;        //修改邮件数量

	//熔炉
	public const uint MSG_FURNACE_SWING_TABLE = 2010;       //切换标签（切换每个功能）
	public const uint MSG_FURNACE_SWING_MERGE_TABLE = 2011; //切换合成二级标签
    public const uint MSG_FURNACE_DISPLAY_GEM = 2012;       //强制显示宝石页面
    public const uint MSG_FURNACE_DISPLAY_FORMULA = 2013;       //强制显示道具页面

	//熔炉分页-合成宝石
	public const uint MSG_MERGE_DISPLAY_LIST = 2020;         //显示标签
	public const uint MSG_MERGE_SELECT_ITEM = 2021;                     //选中某个物体
	public const uint MSG_MERGE_DISPLAY_GEM_INFO = 2022;                //显示宝石详细信息
	public const uint MSG_MERGE_SET_MERGE_COUNT = 2023;                 //设置合成数量
	public const uint MSG_MERGE_BUTTON_MERGE = 2024;                    //合成
	public const uint MSG_MERGE_SELECT_INDEX_ITEM = 2025;               //选中第一个物品

	//强化石
	public const uint MSG_LUCKSTONE_SELECT_STONE = 2031;                //选择合成石(int id)
	public const uint MSG_LUCKSTONE_WINDOW_OPTION = 2032;               //是否打开关闭面板(bool opt)

	//制作书
	public const uint MSG_FORMULA_SELECT_INDEX_ITEM = 2040;             //选中第一个
	public const uint MSG_FORMULA_INITIAL_DATA = 2041;                  //初始化数据
	public const uint MSG_FORMULA_DISPLAY_LIST = 2042;                  //刷新列表
	public const uint MSG_FORMULA_DISPLAY_INFO = 2043;                  //刷新详细信息

	public const uint MSG_FORMULA_SELECT_LIST_ITEM = 2044;              //选中列表物品
	public const uint MSG_FORMULA_SET_MERGE_COUNT = 2045;               //设置合成数量
	public const uint MSG_FORMULA_MERGE_BUTTON = 2046;                  //合成按钮

	public const uint MSG_EQUIP_SWITCHING_TABLE = 2050;                 //装备界面切换

	//tips页面
	public const uint MSG_COMMON_TIPS_SHOWINFO = 2101;		//显示信息
	public const uint MSG_EQUIP_TIPS_SHOWINFO = 2102;		//显示装备信息
	public const uint MSG_PET_EQUIP_TIPS_SHOWINFO = 2103;	//显示宠物装备信息

	//出售界面
	public const uint MSG_SALE_SHOWINFO = 2201;				//显示出售信息
	public const uint MSG_SALE_MINUS = 2202;				//减1
	public const uint MSG_SALE_ADD = 2203;					//加1
	public const uint MSG_SALE_MAX = 2204;					//加到最大值
	public const uint MSG_SALE_UPDATEINFO = 2205;			//更新出售信息
	public const uint MSG_SALE_ITEM = 2206;					//出售道具
	public const uint MSG_OPEN_ITEM = 2207;					//打开或者使用道具


	// 强化
	public const uint MSG_STRENGTHEN_DISPLAY_LIST_TABLE = 2300; //显示选中的分页数据(Table table)
	public const uint MSG_STRENGTHEN_SELECT_ITEM = 2301;        //选中列表中的物品
	public const uint MSG_STRENGTHEN_DISPLAT_INFO = 2302;         //显示装备详细信息
	public const uint MSG_STRENGTHEN_SELECT_INDEX = 2303;       //选择列表第一个装备
	public const uint MSG_STRENGTHEN_LUCKSTONE_CALLBACK = 2304; //幸运石选择结果
	public const uint MSG_STRENGTHEN_ASK_ST = 2305;               //发送请求强化
	public const uint MSG_STRENGTHEN_ASK_CALLBACK_RESULT = 2306;  //强化请求返回结果
    public const uint MSG_STRENGTHEN_ENFORCE_DISPLAY_LIST = 2307;//强制刷新UI
    public const uint MSG_STRENGTHEN_ENFORCE_EFFECT = 2308;     //强化特效

	//进阶
	public const uint MSG_ADVANCED_DISPLAY_LIST_TABLE = 2400; //显示选中的分页数据(Table table)
	public const uint MSG_ADVANCED_SELECT_ITEM = 2401;        //选中列表中的物品
	public const uint MSG_ADVANCED_DISPLAT_INFO = 2402;         //显示装备详细信息
	public const uint MSG_ADVANCED_SELECT_INDEX = 2403;       //选择列表第一个装备
	public const uint MSG_ADVANCED_LUCKSTONE_CALLBACK = 2404; //幸运石选择结果
	public const uint MSG_ADVANCED_ASK_ST = 2405;               //发送请求强化
	public const uint MSG_ADVANCED_ASK_CALLBACK_RESULT = 2406;  //强化请求返回结果
    public const uint MSG_ADVANCED_ENFORCE_DISPLAY_LIST = 2407;//强制刷新UI
    public const uint MSG_ADVANCED_EFFECT_INFO = 2408;          //进阶特效显示

	//系统设置
	public const uint MSG_SETTING_SWTICHING_TABLE = 2500;       //切换页签
	public const uint MSG_SETTING_CHECK_BOX = 2501;             //选择选项
	public const uint MSG_SETTING_SUBMIT = 2502;                //提交bug
	public const uint MSG_SETTING_RELOGIN = 2503;               //重新登录
	public const uint MSG_SETTING_SETAUDIO = 2504;              //设置音效
	public const uint MSG_SETTING_SETMUSIC = 2505;              //设置背景音乐
	public const uint MSG_SETTING_MOVEHELP = 2506;              //移动帮助
    public const uint MSG_SETTING_HIDE = 2507;                  //隐藏其他玩家
    public const uint MSG_SETTING_PEOPLE_OPTION = 2508;         //设置显示玩家人数
    public const uint MSG_SETTING_PEOPLE_SWITCH = 2509;         //显示开关
    public const uint MSG_SHOW_BAOJI_ANIM = 2510;               //显示暴击动画

    

	//镶嵌
	public const uint MSG_INLAY_DISPLAY_EQUIPLIST = 2600;       //显示背包列表
	public const uint MSG_INLAY_SELECT_ITEM = 2601;               //选择物品
	public const uint MSG_INLAY_DISPLAY_INFO = 2602;            //显示详细信息
	public const uint MSG_INLAY_SELECT_GEM = 2603;              //选中宝石
	public const uint MSG_INLAY_REMOVE_GEM = 2604;              //移除宝石
    public const uint MSG_INLAY_GEM_INLAY = 2605;               //插入宝石
    public const uint MSG_INLAY_EFFECT_INFO = 2606;               //镶嵌特效显示

	//洗练
	public const uint MSG_REFINE_DISPLAY_LIST_TABLE = 2700;     //
	public const uint MSG_REFINE_SELECT_ITEM = 2701;
	public const uint MSG_REFINE_DISPLAT_INFO = 2702;
	public const uint MSG_REFINE_SEND_REFINE = 2703;            //发送洗练
	public const uint MSG_REFINE_RESET = 2704;                  //重置
	public const uint MSG_REFINE_SELECT_RESET_ITEM = 2705;      //选中重置属性条目
	public const uint MSG_REFINE_DISPLAY_SELECT_VO = 2706;      //刷新当前选中的数据
	public const uint MSG_REFINE_RESET_OK = 2707;               //确认重置
	public const uint MSG_REFINE_DIALOG_CALLBACK = 2708;        //重置属性回调
	public const uint MSG_REFINE_ENFORCE_DISPLAY_LIST = 2709;   //强制刷新UI
    public const uint MSG_REFINE_DISPLAY_RESET_LIST_CONSUME = 2710;//刷新重置消耗
    public const uint MSG_REFINE_EFFECT_INFO = 2711;            //洗练特效显示

	//章节奖励
	public const uint MSG_CHAPTER_AWARD_SHOWINFO = 2801;		//章节奖励显示
	public const uint MSG_CHAPTER_AWARD_ASK_AWARD = 2802;		//请求章节奖励

	//公共公告
	public const uint MSG_COMMON_NOTICE_STOPTWEEN = 2901;			//停止动画
	public const uint MSG_COMMON_NOTICE_RESUMETWEEN = 2902;			//恢复动画
	public const uint MSG_COMMON_NOTICE_HIDEUI = 2903;				//隐藏UI

	public const uint MSG_COMMON_NOTICE_SHOP = 2904;                //公会商城
	public const uint MSG_COMMON_NOTICE_SHOP_DISPLAYTABLE = 2905;   //公会商城界面显示
	public const uint MSG_COMMON_NOTICE_SHOP_SETBUYITEM = 2906;     //公会商城购买界面
	public const uint MSG_COMMON_NOTICE_DISPLAY_BUYINFO = 2907;     //显示购买商品信息
	public const uint MSG_COMMON_NOTICE_SHOP_BUYCOUNT = 2908;       //设置购买数量
	public const uint MSG_COMMON_NOTICE_SHOP_BUYPANEL_OPTION = 2909;         //购买的操作
	public const uint MSG_COMMON_NOTICE_SHOP_DISPLAY_MONEY = 2910;        //金钱变化
	public const uint MSG_COMMON_NOTICE_SHOP_ADD_RMB = 2911;              //冲钱

    public const uint MSG_COMMON_NOTICE_FLAG_DISPLAYTABLE = 2912;   //公会旗帜界面切换
    public const uint MSG_COMMON_NOTICE_FLAG_SHOW = 2913;              //旗帜界面显示
    public const uint MSG_COMMON_NOTICE_CENTER_SHOW = 2914;              //公会大厅
    public const uint MSG_COMMON_NOTICE_CENTER_REFRESH = 2915;              //公会大厅界面显示
    public const uint MSG_COMMON_NOTICE_SKILL_REFRESH = 2916;              //公会技能界面显示
    public const uint MSG_COMMON_NOTICE_SKILL_DISPLAYTABLE = 2917;   //公会技能TABLE
    public const uint MSG_COMMON_NOTICE_DONATE_DISPLAYTABLE = 2918;   //公会捐献TABLE
    public const uint MSG_COMMON_NOTICE_DONATE_REFRESH = 2919;   //公会捐献TABLE
    public const uint MSG_COMMON_NOTICE_APPLY_DISPLAYTABLE = 2920;   //公会申请TABLE
    public const uint MSG_COMMON_NOTICE_APPLY_REFRESH = 2921;   //公会刷新TABLE
    public const uint MSG_COMMON_NOTICE_CREATE_GUILD = 2922;   //公会创建按钮
    public const uint MSG_COMMON_NOTICE_CREATE_FINAL = 2923;   //公会确认创建

	//战斗力
	public const uint MSG_POWER_SHOW = 3001;
	public const uint MSG_POWER_CLOSE = 3002;

	//便捷购买(物品不足提示)
	public const uint MSG_FEEB_DISPLAY_TABLE = 3100;            //显示提示
	public const uint MSG_FEEB_CLOSE = 3101;                    //关闭
	public const uint MSG_FEEB_SHOW_SUM_PRICE = 3102;           //显示总计价格
	public const uint MSG_FEEB_SET_COUNT = 3103;                //设置个数
	public const uint MSG_FEEB_SHOW_TABLE = 3104;               //显示标签
	public const uint MSG_FEEB_BUY_ITEM = 3105;                 //购买商品
	public const uint MSG_FEEB_GO_SHOP = 3106;                  //充值页面
	public const uint MSG_FEEB_FAST_OPEN = 3107;                //快速打开(id)

	//新物品
	public const uint MSG_NEWITEM_SHOW = 3201;					//新物品提示
	public const uint MSG_NEWITEM_USE_SHOW = 3202;				//新物品使用提示

	public const uint MSG_TALK_SWTING_TABLE = 3300;             //切换table(Table)
	public const uint MSG_TALK_UPDATE_TEXT = 3301;              //如果收到消息，更新当前的选项信息(TalkVo)
	public const uint MSG_TALK_DISPLAY_CHANNEL = 3302;          //更新频道标签
	public const uint MSG_TALK_DISPALY_CHANNELLIST = 3303;      //频道列表(bool)
	public const uint MSG_TALK_SELECT_CHANNEL = 3304;           //选择频道(TalkType)
	public const uint MSG_TALK_SEND_MSG = 3305;                 //发送消息(string)
	public const uint MSG_TALK_CLICK_URL = 3306;                //获取连接
	public const uint MSG_TALK_HIDDEN_PLAYER_TIP = 3307;        //隐藏玩家TIP   
	public const uint MSG_TALK_DISPLAY_FRIEND_INPUT = 3308;     //显示/隐藏 私聊面板(bool)
	public const uint MSG_TALK_ENTER_FRIEDN_NAME = 3309;        //确认输入好友名字
	public const uint MSG_TALK_DISPLAY_FRIEND_LIST = 3310;      //显示/隐藏 好友列表面板(bool)
	public const uint MSG_TALK_WHISPER_PLAYER = 3311;           //快速私聊
	public const uint MSG_TALK_FAST_ADD_FRIEND = 3312;          //快速添加好友
     
	//活动界面显示
	public const uint MSG_EVENT_SHOW = 3501;					//显示活动界面
	public const uint MSG_EVENT_UPDATEINFO = 3502;				//更新
	public const uint MSG_EVENT_ASKJOIN = 3503;					//请求加入活动

	//世界boss胜利界面显示
	public const uint MSG_BOSS_WIN_SHOW = 3601;					//显示世界boss胜利的信息
	public const uint MSG_BOSS_WIN_BACK_CITY = 3602;			//返回主城
	//世界boss失败界面显示
	public const uint MSG_BOSS_DEAD_SHOW = 3701;				//显示世界boss失败的信息
	public const uint MSG_BOSS_DEAD_ERROR = 3702;				//显示世界boss失败的错误信息
	public const uint MSG_BOSS_DEAD_BUY_REVIVE = 3703;			//复活
	public const uint MSG_BOSS_DEAD_BACK_CITY = 3704;			//回城
	public const uint MSG_BOSS_DEAD_TIME_REVIVE = 3705;			//时间到复活
	
	public const uint MSG_VIP_DISPLAY_TABLE = 3800;             //显示(Table)
	public const uint MSG_VIP_SWTING_VIP_SHOW = 3801;           //显示总览切换当前VIP等级(int)
	public const uint MSG_VIP_SWTING_TABLES = 3802;             //切换table(Table)
	public const uint MSG_VIP_RECEIVE_AWARD = 3803;             //领取礼包
	public const uint MSG_VIP_HID_TIP = 3804;                   //隐藏所有的TIP
	public const uint MSG_VIP_SHOW_TIP = 3805;                  //显示指定ID的TIP

	public const uint MSG_MEDAL_DISPLAY_VIEW = 3900;
	public const uint MSG_MEDAL_BAG_MEDAL_SHOW = 3902;
	public const uint MSG_MEDAL_SWING_MEDAL_LEVELUP = 3903;

	//好友

	public const uint MSG_FRIEND_DISPLAY_MAIN = 4000;     //显示主界面
	public const uint MSG_FRIEND_DISPLAY_ADDFRIEND = 4001;          //添加好友 界面
	public const uint MSG_FRIEND_ADD_TRUE = 4003;          //确定添加好友
	public const uint MSG_FRIEND_RECORD_SHOW = 4004;       //申请记录 界面
	public const uint MSG_FRIEND_RECORD_CLOSE = 4005;      //申请记录 界面, x关闭
	public const uint MSG_FRIEND_DISPLAY_LIST_CLOSE = 4006;  //好友界面，x按钮关闭
 
    public const uint MSG_FRIEND_DELETE = 4007;           //删除好友界面,按钮开启
    public const uint MSG_FRIEND_DELETE_RUFUSE = 4008;    //删除好友界面， x或 取消  关闭
    public const uint MSG_FRIEND_DELETE_TRUE = 4009;      //确定删除好友
    public const uint MSG_FRIEND_TABEL_CHANGE = 4010;     //申请页面切换
    public const uint MSG_FRIEND_TABEL_RECORD = 4011;     //刷新申请页面
    public const uint MSG_FRIEND_TABEL_RESULT = 4012;     //刷新结果页面
    public const uint MSG_FRIEND_RECORD_NAME = 4013;       //申请栏，玩家name
    public const uint MSG_FRIEND_RECORD_LIST = 4014;      //刷新申请列表
    public const uint MSG_FRIEND_RECORD_ADD_FRIENT = 4015; //申请界面，添加好友
    public const uint MSG_FRIEND_RECORD_RESULT_FRIENT = 4016; //申请界面，拒绝好友
    public const uint MSG_FRIEND_RESULT_LIST = 4017;      //申请结果刷新
    public const uint MSG_FRIEND_DISPLAY_INFO = 4018;     //好友信息
    public const uint MSG_FRIEND_MESSAGE_CLOSE = 4019;    //关闭好像信息
    public const uint MSG_FRIEND_ALL_AGREE = 4020;     //全部加为好友
    public const uint MSG_FRIEND_ALL_REFUSE = 4021;    //全部拒绝添加
    public const uint MSG_FRIEND_SEND_TILI = 4022;     //赠送体力
    public const uint MSG_FRIEND_RECEIVE_TILI = 4023;   //领取体力
    public const uint MSG_FRIEND_ALL_SEND_TILI = 4024;  //一键赠送
    public const uint MSG_FRIEND_ALL_RECEIVE_TILI = 4025;//一键领取
    public const uint MSG_FRIEND_EX_INFO = 4026;            //刷新详细信息
    public const uint MSG_FRIEND_OPT = 4027;             //VIP一键功能
    public const uint MSG_FRIEND_WHISPER = 4028;            //快速私聊
    public const uint MSG_FRIEND_NOTIFY = 4029;         //状态显示

    public const uint MSG_GUILD_DISPLAY_TABLE = 4100;   //显示页签(Table)
    public const uint MSG_GUILD_DISPLAY_MEMBER_TOGGLE = 4101;//显示离线成员(Bool)
    public const uint MSG_GUILD_DISPLAY_MEMBER_INFO = 4102; //显示详细信息(int index)
    public const uint MSG_GUILD_DISPLAY_MEMBER_OFFICE = 4103;   //显示职位管理(bool)
 	
	//宠物
	public const uint MSG_CLOSE_PET_UI = 4200;			//关闭宠物UI
	public const uint MSG_PET_INIT = 4201;				//初始化
	public const uint MSG_PET_AUTO_EVOLUTION = 4202;		//自动进化
	public const uint MSG_PET_EVOLUTION = 4203;			//进化
	public const uint MSG_PET_SHOW_EVOLUTION = 4204;		//显示进阶界面
	public const uint MSG_PET_UPDATE_MONEY = 4205;  		//金钱钻石的变化
	public const uint MSG_PET_UPDATE_LUCK = 4206;	  	//成功率，幸运点的变化
	public const uint MSG_PET_UPDATE_MODEL_LADDER = 4207;//更新阶数的时候同时更新模型
	public const uint MSG_PET_SHOW_EFFECT = 4208;		//每次培养进阶的时候显示文字以及特效
	public const uint MSG_PET_NOT_ENOUGH_MSG = 4209;	  	//弹出材料不足的信息
	public const uint MSG_PET_STOP_AUTO = 4210;			//退出自动培养，自动进阶
	public const uint MSG_PET_DRAG_NEXT = 4211;			//下个宠物
	public const uint MSG_PET_DRAG_PREV = 4212;			//上个宠物
	public const uint MSG_PET_SET_CAMERA = 4213;			//设置摄像机
	public const uint MSG_PET_SHOW_TIME = 4214;			//显示时间
	public const uint MSG_PET_RET 		= 4215;			//回到当前最高阶的宠物
    public const uint MSG_PET_FOLLOW = 4216;			//宠物跟随
    public const uint MSG_PET_SUCCESS = 4217;			//宠物进阶成功
	public const uint MSG_PET_SWITCH_TAB = 4218;		//切换TAB
	public const uint MSG_PET_SHOWPETEQUIP = 4219;		//显示宠物装备
	public const uint MSG_PET_SHOWPETEQUIPITEM = 4220;  //显示宠物装备的物品
	public const uint MSG_PET_CLOSE_PET_SKILL = 4221;	//关闭面板
	public const uint MSG_PET_OPEN_PET_SKILL = 4222;	//开打面板
	public const uint MSG_PET_UPDATE_PREVIEW_ATTR = 4223;	//更新预览的属性信息

	//魔物悬赏
	public const uint MSG_CLOSE_MONSTER_REWARD_UI = 4300;	//关闭界面
	public const uint MSG_MONSTER_REWARD_SHOW = 4301;	//显示界面
	public const uint MSG_MONSTER_REWARD_ASK_ZHUIJI	 =4302;	//请求追缉
	public const uint MSG_MONSTER_REWARD_NEXT = 4303;	//查看下一个
	public const uint MSG_MONSTER_REWARD_PREV = 4304;	//查看上一个
	public const uint MSG_MONSTER_REWARD_UPDATE_CURRENT  = 4305;	//更新当前界面
	public const uint MSG_MONSTER_REWARD_UPDATE_NEXT  = 4306;	//更新下个界面
	public const uint MSG_MONSTER_REWARD_UPDATE_PREV  = 4307;	//更新上个界面

	//潘多拉
	public const uint MSG_CLOSE_PANDORA_UI = 4400;		//关闭界面
	public const uint MSG_PANDORA_SHOW = 4401;			//显示潘多拉魔盒的数据
	public const uint MSG_PANDORA_CHALLENGE_PANDORA = 4402;	//前往挑战潘多拉
	public const uint MSG_PANDORA_RESET_PANDORA_NUM = 4403;	//重置次数
	public const uint MSG_PANDORA_CHALLENGE_ALL_PANDORA = 4404;	//全部挑战的次数
	public const uint MSG_PANDORA_OPEN_PANDORA = 4405;		//开启潘多拉宝盒
 

    //多人剧情
    public const uint MSG_PLOT_DISPLAY_MAIN = 4500;     //显示主页面

    //换线
    public const uint MSG_CHANNEL_CHANGE_LINE = 4600;   //换线
    public const uint MSG_CHANNEL_CHANGE_SUBMIT = 4601; //提交换线

    //新手引导
    public const uint MSG_GUIDE_SEND_TRIGGER = 4700;    //发送触发器(Trigger)
    public const uint MSG_GUIDE_DISPLAY = 4701;         //显示界面

    //选择服务器
    public const uint MSG_SELECT_SERVER_SHOW = 4800;      //选择服务器界面
    public const uint MSG_CHOOSE_SERVER_OPTION = 4801;      //发送选择服务器
    public const uint MSG_CHOOSE_SERVER_OPTION_UNIQUE = 4802;      //发送选择特殊服务器

}