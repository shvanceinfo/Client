using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using model;
using helper;
using MVC.entrance.gate;
using NetGame;

namespace manager
{
    public class VipManager
    {
        public static uint MAX_VIP_LEVEL = 10;
        public const uint MAX_VIP_IMG_LEN = 3;

        private Hashtable _vips;        //VIP表
        private Hashtable _privileges;  //特权显示排序表
        private BetterList<PrivilegeVo> _displayList;

        
        private int _vipLevel;          //玩家VIP等级
        private int _sumDiamond;        //历史总计充值钻石数量

        private int _showLevel;         //展示选择

        private Table _selectTable;

        private BetterList<AttributeKeyValue> _attributes;  //祝福属性对比


        private BetterList<int> _titleVips;                  //用于显示对比表VIP列表

        private CGAskVipAward _ask72;
        
        
        private VipManager()
        {
            _ask72 = new CGAskVipAward();
            _displayList = new BetterList<PrivilegeVo>();
            _selectTable = Table.Table1;
            _showLevel = 1;
            _vipLevel = 0;
            _sumDiamond = 0;
            _vips = new Hashtable();
            _privileges = new Hashtable();
            _attributes = new BetterList<AttributeKeyValue>();
            _titleVips = new BetterList<int>();
            for (int i = 1; i <= MAX_VIP_LEVEL; i++)
                _titleVips.Add(i);
        }

        public void SortPrivilege()
        {
            _displayList.Clear();
            foreach (PrivilegeVo vo in _privileges.Values)
            {
                _displayList.Add(vo);
            }
            _displayList.Sort((PrivilegeVo v1, PrivilegeVo v2) =>
                {
                    if (v1.OrderId > v2.OrderId)
                        return 1;
                    return -1;
                });
        }

        public void OpenWindow()
        {
            SortPrivilege();

            _selectTable = Table.Table1;
            _showLevel = 1;

            VipVo vo;
            foreach (int key in _vips.Keys)
            {
                vo = _vips[key] as VipVo;
                uint loot = ItemManager.GetInstance().GetTemplateByTempId((uint)vo.ItemId).itemloot;
                vo.LootItem = LootManager.Instance[(int)loot];
            }
            UIManager.Instance.openWindow(UiNameConst.ui_vip);
            Gate.instance.sendNotification(MsgConstant.MSG_VIP_DISPLAY_TABLE, _selectTable);
        }
        public void CloseWindow()
        {
            NPCManager.Instance.createCamera(false);
            UIManager.Instance.closeWindow(UiNameConst.ui_vip);
            EasyTouchJoyStickProperty.ShowJoyTouch(true);
        }

        public void DisplayCurTable()
        {
            Gate.instance.sendNotification(MsgConstant.MSG_VIP_DISPLAY_TABLE, _selectTable);
        }


        //添加属性类型
        public void AddAttribute(int index,int type)
        {
            if (index >= _attributes.size)
            {
                _attributes.Add(new AttributeKeyValue((int)MAX_VIP_LEVEL+1) { Type = (eFighintPropertyCate)type });
            }
            else {
                _attributes[index].Type = (eFighintPropertyCate)type;
            }
        }
        //添加属性值
        public void AddValue(int vip, int index, int value)
        {
            _attributes[index].Values[vip] = value;
        }

        /// <summary>
        /// 更新当前VIP礼包是否领取
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isreceive"></param>
        public void UpdateVipInfo(int id, bool isreceive)
        {
            if (_vipLevel<id)
            {
                FindVoById(id).IsReceive = VipState.None;
                return;
            }
            if (isreceive)
            {
                FindVoById(id).IsReceive = VipState.Receiveed;
            }
            else
            {
                FindVoById(id).IsReceive = VipState.CanReveive;
            }
        }

        public void ReveiveAward()
        {
            _ask72.VipId = (byte)ShowVip.VipId;
            NetBase.GetInstance().Send(_ask72.ToBytes());
        }

        /// <summary>
        /// 更新当前VIP等级
        /// </summary>
        /// <param name="level"></param>
        public void UpdateVipLevel(int level)
        {
            _vipLevel = level;
            VipVo vo;
            for (int i = 1; i <= _vipLevel; i++)
            {
               vo=FindVoById(i);
                if (vo.IsReceive==VipState.None)
                {
                    vo.IsReceive = VipState.CanReveive;
                }
            }
            Gate.instance.sendNotification(MsgConstant.MSG_MAIN_UPDATE_VIP);
            GuideInfoManager.Instance.AddGuideTriggerVip(_vipLevel);
        }

        /// <summary>
        /// 玩家充值钱数
        /// </summary>
        /// <param name="dm"></param>
        public void UpdateSumDiamond(int dm)
        {
            _sumDiamond = dm*10;
        }

        public VipVo FindVoById(int id)
        {
            return _vips[id] as VipVo;
        }

        //设置当前预览等级
        public bool SetShowVip(int lvl)
        {
            bool displayUi = true;
            if (lvl < 1)
            {
                displayUi = false;
                lvl = 1;
            }
            else if(lvl>MAX_VIP_LEVEL) {
                displayUi = false;
                lvl = (int)MAX_VIP_LEVEL;
            }
            _showLevel = lvl;
            return displayUi;
        }

        public void SwtingTable(Table tab)
        {
            _selectTable = tab;
            Gate.instance.sendNotification(MsgConstant.MSG_VIP_DISPLAY_TABLE, _selectTable);
        }


        /// <summary>
        /// 当前VIP冲了多少钱(历史总计-当前等级的金钱数)
        /// </summary>
        public int HaveMoney
        {
            get {
                if (MAX_VIP_LEVEL==_vipLevel)
                {
                    return 0;
                }
                VipVo cur = FindVoById(_vipLevel);
                return _sumDiamond - (int)cur.Price ;
            }
        }
        /// <summary>
        /// return 下一级金钱-当前等级金钱
        /// </summary>
        public int MaxMoney
        {
            get {
                int temp = _vipLevel + 1;
                if (temp > MAX_VIP_LEVEL)
                    return 1;
                return (int)FindVoById(_vipLevel + 1).Price - (int)FindVoById(_vipLevel).Price;
            }
        }


        /// <summary>
        /// 背包最大容量
        /// </summary>
        public int BagMaxSize
        {
            get {
                return CurVip.Privileges["Bag_num"].GetInt();
            }
            
        }

        public int GblBuyCount
        {
            get
            {
                return CurVip.Privileges["GBL_num"].GetInt();
            }
        }


        /// <summary>
        /// VIP好友上限
        /// </summary>
        public int FriendMaxNumber
        {
            get
            {
                return CurVip.Privileges["FriendNum"].GetInt();
            }
        }



        /// <summary>
        /// VIP好友体力赠送次数
        /// </summary>
        public int FriendMaxTiliSong
        {
            get
            {
                return CurVip.Privileges["FriendTiLi_song"].GetInt();
            }
        }



        /// <summary>
        /// VIP好友体力领取次数
        /// </summary>
        public int FriendMaxTiliLing
        {
            get
            {
                return CurVip.Privileges["FriendTiLi_ling"].GetInt();
            }
        }



        /// <summary>
        /// VIP好友一键赠送
        /// </summary>
        public bool FriendAllSong
        {
            get
            {
                return CurVip.Privileges["FriendFree_song"].GetBoolean();
            }
        }



        /// <summary>
        /// VIP好友一键领取
        /// </summary>
        public bool FriendAllLing
        {
            get
            {
                return CurVip.Privileges["FriendFree_ling"].GetBoolean();
            }
        }



        /// <summary>
        /// 好友全部同意
        /// </summary>
        public bool FriendAllAgree
        {
            get
            {
                return CurVip.Privileges["FriendAgree_yes"].GetBoolean();
            }
        }




        /// <summary>
        /// 好友全部拒绝
        /// </summary>
        public bool FriendAllRefuse
        {
            get
            {
                return CurVip.Privileges["FriendAgree_no"].GetBoolean();
            }
        }
 

        /// <summary>
        /// 关卡复活次数
        /// </summary>
        public int BornCount
        {
            get {
                return CurVip.Privileges["FH_num"].GetInt();
            }
        }

        /// <summary>
        /// 体力购买次数
        /// </summary>
        public int TiliCount
        {
            get {
                return CurVip.Privileges["TL_num"].GetInt();
            }
        }
		
		/// <summary>
		/// 魔物悬赏的最大次数
		/// </summary>
		/// <value>
		/// The monster reward count.
		/// </value>
		public int MonsterRewardCount{
			get{
				return CurVip.Privileges["xuanshang_num"].GetInt();
			}
		}
		
		public bool MonsterRewardQuick{
			get{
				return CurVip.Privileges["xuanshang_kuaisu"].GetBoolean();
			}
		}
		
		public bool MonsterRewardOneClear{
			get{
				return CurVip.Privileges["xuanshang_yijian"].GetBoolean();
			}
		}

		public int SweepJiaSu{
			get{
				return CurVip.Privileges["SD_jiasu"].GetInt();
			}
		}


        #region 单例
        private static VipManager _instance;
        public static VipManager Instance
        {
            get
            {
                if (_instance == null) _instance = new VipManager();
                return _instance;
            }
        }
        #endregion

        #region 属性

        /// <summary>
        /// 特权表
        /// </summary>
        public BetterList<PrivilegeVo> DisplayList
        {
            get { return _displayList; }
            set { _displayList = value; }
        }

        //标题排序
        public BetterList<int> TitleVips
        {
            get { return _titleVips; }
        }

        /// <summary>
        /// 玩家属性对比,index=显示顺序
        /// </summary>
        public BetterList<AttributeKeyValue> Attributes
        {
            get { return _attributes; }
        }

        public VipVo ShowVip
        {
            get { return FindVoById(_showLevel); }
        }

        /// <summary>
        /// 当前自己的等级
        /// </summary>
        public VipVo CurVip
        {
            get { return FindVoById(_vipLevel); }
        }

        /// <summary>
        /// 当前VIP等级
        /// </summary>
        public int VipLevel
        {
            get { return _vipLevel; }
            set { _vipLevel = value; }
        }
        /// <summary>
        /// 历史总计充值钻石
        /// </summary>
        public int SumDiamond
        {
            get { return _sumDiamond; }
            set { _sumDiamond = value; }
        }
        /// <summary>
        /// VIP表hash
        /// </summary>
        public Hashtable Vips
        {
            get { return _vips; }
            set { _vips = value; }
        }
        /// <summary>
        /// 特权表hash
        /// </summary>
        public Hashtable Privileges
        {
            get { return _privileges; }
            set { _privileges = value; }
        }
        #endregion

        #region Static
        /// <summary>
        /// 获取还需要多少钻石
        /// </summary>
        /// <returns></returns>
        public static string FromatNextMoney()
        {
            return string.Format(LanguageManager.GetText("vip_format_title"), ColorConst.Color_HeSe, ColorConst.Color_Green,
                Instance.MaxMoney-Instance.HaveMoney);
        }

        public static string FormatItemLabel()
        {
            return string.Format(LanguageManager.GetText("vip_format_title2"), ColorConst.Color_Green,
                Instance.ShowVip.Price,Instance.ShowVip.VipId);
        }

        public static string FormatAttribute(string text)
        {
            return string.Format(LanguageManager.GetText("vip_attribute"), ColorConst.Color_HeSe, text);
        }

        /// <summary>
        /// 判断是不是为真
        /// </summary>
        /// <param name="num"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static bool Bit(int num, int x)
        {
            int t = 1 << x;
            int z = num & t;
            if (z == 0)
                return false;
            else
                return true;
        }
        #endregion
    }
}
