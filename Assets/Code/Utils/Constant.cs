/**该文件实现的基本功能等
function: 程序中使用到的一些常量
author:ljx
date:2013-11-01
**/

public class Constant
{
	//创角所用到的常量
	public const string INIT_SCENE = "InitLevel"; //初始化场景
    public const string CREATE_ROLE_SCENE = "CreateRole"; //创角的场景名称
	public const string CREATE_SERVER_ROLE = "server_role";
	public const string CHOOSE_SERVER = "server_panel";
	public const string CREATE_ROLE = "role_panel";
	public const string SELECT_VOCATION1 = "select_vocation1";
	public const string SELECT_VOCATION2 = "select_vocation2";
	public const string SELECT_VOCATION3 = "select_vocation3";
	public const string SERVER_ROLE_TITLE = "titleLabel";
	
	//角色所用到的常量
	public const int SEX_MALE = 1; //男性
	public const int SEX_FEMALE = 0; //女性
	public const string SWORD_PREFAB = "Model/prefab/jianshi"; //剑士
	public const string SWORD_UI = "Model/prefab/jianshiUI";
	public const string BOW_PREFAB = "Model/prefab/longnv"; //弓箭手
	public const string BOW_UI = "Model/prefab/longnvUI";
	public const string RABBI_PREFAB = "Model/prefab/jingling"; //法师
	public const string RABBI_UI = "Model/prefab/jinglingUI";
	
	//宝石镶嵌摘除使用的常量
	public const int DIAMOND_POS1 = 0;
	public const int DIAMOND_POS2 = 1;
	public const int DIAMOND_POS3 = 2;
	public const int EXTRA_ATTR_HEIGHT = 22; //附加属性的高度
	public const int SEPERATE_LINE_HEIGHT = 7; //分割线的高度	
	
	//一些鼠标的灵敏度设置
	public const float CLOSE_ECLIPSION = -200f; //关闭窗体需要滑动的像素
	public const float APPEAR_TIME = 0.5f; //UI窗体出现的时间
	public const int  ABOVE_SHADER_LEVEL = 4000;
	public const int BELOW_SHADER_LEVEL = 3000;
	
	//网络连接的相关设置
	public const float WAIT_TIME = 3f; //等待服务器响应的时间
	public const int   RECONNECT_NUM = 3;   //重连服务器的次数
	public const int  DISCONNECT_CHECK = 118; //5分钟没有ping必须检测服务器是否断开socket
	public const float  PING_TIME = 2f; //客户端ping服务端的时间
	public const float  PING_DUNGEON_TIME = 0.1F;//多人副本的ping 的时间
	//字体变换的一些常量
	public const float BLINK_TIME = 1.0f; //金钱货币发生变化时发生变化
	public const float BLINK_DISTANCE = 5f; //开始闪烁的的上下位移量
	public const float PANLE_ZORDER_DELTA = 10f; //panel出现时深度点的变化,深度点需要仔细调节，不能随便设 -8字体有问题，-10没问题，滑动有问题
	
	//UI闪烁等的一些变量
	public const float BLINK_INTEVAL = 0.1f; //闪烁的间隔
	public const float BLINK_EMAIL_BUTTON = 0.1f; //邮件按钮的闪烁
	public const float PROGRESS_UPDATE = 0.02f; //loading进度条至少20帧闪烁
	public const float UI_PACK_POS = -400f; //背包中UI的其实结束位置
	
	//关于Loading的一些变量
	public const float INIT_LOADING_X = -412f;
	public const float INIT_LOADING_Y = -210f;
	public const string LOADING_SCENE = "Loading";
	public const int LOAD_TIP_FIX = 0;
	public const int LOAD_TIP_MIN = 1;
	public static int LOAD_TIP_MAX = 12;
    public const string IN_ACTIVE_PREFAB_NAME = "attach_inactive";
	//恶魔洞窟的相关常量
	public const string COLOR_END = "[-]";
	public const string DEVIL_WAVE_FIRST = "1" + COLOR_END;
	public const string DEVIL_WAVE_SECOND = "31" + COLOR_END;
	public const string DEVIL_WAVE_THIRD = "61" + COLOR_END;
	//任务的一些常量
	public const string COLOR_RED = "[ff0000]";
	public const string COLOR_GREEN = "[00ff00]";
	public const string COLOR_ORANGE = "[ffab40]";
	public const string COLOR_HUE = "[0000ff]";
	public const string LEFT_PARENTHESIS = "(";
	public const string RIGHT_PARENTHESIS = ")";
	public const string NUM_PREFIX = "X";
	public const string TASK_EFFECT = "Effect/Effect_Prefab/Task/";


	//不释放的资源图集
	public const string CHAT_ATLAS = "ChatDialog-Atlas"; //聊天图集
	public const string HP_ATLAS = "hpAtlas"; //hp动画序列
	public const string MP_ATLAS = "mpAtlas"; //mp动画序列
	public const string SKILL_ATLAS = "ico_skill"; //技能图集
	public const string COMMON_ATLAS = "item_icon"; //公用的icon图集
	public const string EQUIP_ATLAS = "item_icon01"; //装备图集
	public const string DIAMOND_ATLAS = "item_icon02"; //钻石图集
	public const float CLEAR_TIME = 0.5f; 			//半秒后开始清除场景
	public const int FREE_ATLAS_SIZE = 10; 			//释放的图集的最大数量
	
	//资源的图型
	public const string IMAGE_EXP = "icon_exp";
	public const string IMAGE_GOLD = "gold";
	public const string IMAGE_DIAMOND = "diamond";
	public const string MAP_PIC_POS = "mappicture/";
	
	//其它的一些常量
	public const string REPLACE_LATER = "replace_later"; //一会儿要替换名称的UI
	public const int SCENE_JUDGE_PARAM = 100000000; //用来判断场景的参数
	public const int SCENE_DEVIL_WAVE = 300200010; //恶魔洞窟的地图名称
	public const int SCENE_GOLDEN_GOBLIN = 300100010; //黄金哥布林玩法
	public const int SCENE_ARENA = 300300010; // 竞技场
    
	public const string ITEM_NAME = "_白";		  //装备本身的icon
//	public const string ITEM_BORDER_NAME = "框";	  //装备颜色的icon
	public const string ITEM_BORDER_PNG = "Frame";	  //装备颜色的PNG
	public const string ITEM_BORDER_SPRITE_NAME = "new_color"; //新生成颜色Sprite的名称
	public const string PARAGRAPH_BEGIN_SIMBOL = "     "; //每个段落开始添加空格符
	public const string PARAGRAPH_END_SIMBOL = "\n\n"; //每个段落结束双行换
	public const string SECTION_BEGIN_SIMBOL = "\n     "; //每个章节添加空格
	public const string PLAYER_NAME = "%p";
    public const string REPLACE_PARAMETER_1 = "%1"; 	//要替换的第一个参数
    public const string REPLACE_PARAMETER_2 = "%2"; 	//要替换的第二个参数


	// 竞技场
	public const string Player_Born_Point = "PlayerPoint1";
	public const string Enemy_Born_Point = "enymyPoint1";
    public const string Arena_Effect_Win = "Effect/Effect_Prefab/UI/UI_guanka_shengli";
    public const string Arena_Effect_Lose = "Effect/Effect_Prefab/UI/UI_guanka_shibai";
    public const int Arena_Effect_time=2;

    //回城常量
	public const int BACK_CITY_SCENE = 100;


	//战斗界面

	public const int FIGHT_MAX_HHEALT_MAGIC_ITEM_CD = 5;        //血瓶CD时间
	public const int Fight_MaxSkillSize = 4;                    //最多技能数量
	public const int Fight_WarriorAttackId = 101001001;         //每个职业的普通攻击ID
	public const int Fight_ArcherAttackId = 102001001;
	public const int Fight_MagicAttackId = 103001001;
	public const string Fight_WarriorHandIcon = "common_zhanshi";         //每个职业的头像
	public const string Fight_ArcherHandIcon = "common_gongjianshou";
	public const string Fight_MagicHandIcon = "common_mofashi";
	public const string Fight_Warrior_Atlas = "UI/Skill/Warrior/Warrior Atlas";         //每个职业的头像
    public const string Fight_Archer_Atlas = "UI/Skill/Archer/Archer Atlas";
    public const string Fight_Magic_Atlas = "UI/Skill/Magic/Magic Atlas";
	public const string Fight_Is_Lock = "fight_suoding";        //锁定技能背景
	public const string Fight_No_Lock = "fight_jinengkuang";    //未锁定技能背景
	public const string Fight_Time_LblName="fight_jishi";
    public const string Fight_ReTime_LblName = "fight_daojishi";
	
	//主界面常量
	public const int ENGERY = 200;	//体力常量,以后要改为读配置文件的体力
	
	//颜色常量
	public const string RED = "ff0000"; //红色
	public const string WHITE = "ffffff";//白色
    public const string YELLOW = "fff555";  //黄色

    //登陆
    public const string AccountRegex = @"^([a-zA-Z0-9]|[_]){6,16}$";
    public const string PasswordRegex = @"^([a-zA-Z0-9]|[_]){6,16}$";
    public const int MIN_SIZE = 6;                          //账号最小字符
    public const int MAX_SIZE = 16;                         //最大字符

    //角色名
    public const int MAX_NICE_NAME_LEN = 12;                //角色名最大字符
    public const string NICK_NAME_Regex = @"[^\u4E00-\u9FA5]";

    public const string RMB = "￥";


    public const float PLAYER_INIT_POSITION_X = 0.0f;
    public const float PLAYER_INIT_POSITION_Y = 100.0f;
    public const float PLAYER_INIT_POSITION_Z = 0.0f;
	
	//角色职业等级系数 剑士10000开头 弓箭手20000开头 法师30000开头
	public const int LEVEL_RATIO = 10000;
	
	//任务符号
	public const string SIMBOL_ACCENT_TANHAO_AN = "main_tanhao_an";   //主线任务等级不满足
	public const string SIMBOL_ACCENT_TANHAO_LIANG = "main_tanhao_liang";   //可领取阶段
	public const string SIMBOL_PROCESSING_WENHAO_AN = "main_wenhao_an";   //进行中
	public const string SIMBOL_COMPLETE_WENHAO_LIANG = "main_wenhao_liang";   //已完成
	
	
    // NavMesh半径的最大值
    public const float NAVMESH_RADIUS_MAX = 0.3f;

    //强化最大等级
    public const int STRENG_MAX_LEVEL = 10;

    

    /// <summary>
    /// VIP显示图片数量3
    /// </summary>
    public const int VIP_PIC_LEN = 3;

    // 战斗的视野范围 (在视野范围内也在攻击范围就直接攻击，在视野范围内，不在攻击范围 需要寻过去)
    public const float FIGHT_SWORD_VIEW_RANGE = 10.0f;
    public const float FIGHT_ARCHER_VIEW_RANGE = 10.0f;
    public const float FIGHT_MAGIC_VIEW_RANGE = 10.0f;

    public const float DAMAGE_BASE_UNIT = 10000.0f; // 伤害计算基准单位 (万分比)
}
