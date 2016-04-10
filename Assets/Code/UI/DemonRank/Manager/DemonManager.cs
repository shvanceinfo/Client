using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using model;
using System.Collections;
using NetGame;
using MVC.entrance.gate;
using helper;
namespace manager
{
    public class DemonManager
    {
        static DemonManager _instance;

        const int GATE1 = 1;
        const int GATE31 = 31;
        const int GATE61 = 61;
        const int Level1Id = 10001;
        const int Level2Id = 20001;
        const int Level3Id = 30001;

        private Hashtable _demonHash;       //所有难度表数据
        private Hashtable _rankRewardHash;  //排名奖励配置表

        private DemonDiffEnum _curLevel;    //当前选择的等级

        private Hashtable _curDemonList;    //当前挑战目标奖励列表
        private Hashtable _rankList;        //排名表
        private DemonInfoVo _infoVo;        //当前玩家通关信息

        private int _receiveId;             //当前接收ID,用于领取奖励了之后，改变状态

        private GCAskEnterTowerInstance _ask45;     //45号协议
        private GCReportTowerInstanceScore _ask46;
        private GCAskTowerInstanceRank _ask47;
        private GCAskTowerInstanceAward _ask48;
        private int TempEnterID { get; set; }
        private byte TempEnterWave { get; set; }
        private bool IsFristRequestCurRank ;   //每次打开窗口只请求一次
        private bool IsFristRequestHistoryData;//请求昨日排名数据
        public DemonManager()
        {
            IsFristRequestHistoryData = true;
            IsFristRequestCurRank = true;
            _receiveId = 0;
            _ask45 = new GCAskEnterTowerInstance();
            _ask46 = new GCReportTowerInstanceScore();
            _ask47 = new GCAskTowerInstanceRank();
            _ask48 = new GCAskTowerInstanceAward();
            _infoVo = new DemonInfoVo();
            _rankRewardHash = new Hashtable();
            _curLevel = DemonDiffEnum.Level1;
            _demonHash = new Hashtable();
            _curDemonList = new Hashtable();
            _rankList = new Hashtable();
            int begin = (int)DemonDiffEnum.Begin;
            int end = (int)DemonDiffEnum.End;

            //初始化目标奖励
            for (int i = begin + 1; i < end; i++)
            {
                _curDemonList.Add((DemonDiffEnum)i, new Hashtable());
            }
            //初始化排名
            for (int i = begin+1; i < end; i++)
            {
                _rankList.Add((DemonDiffEnum)i, new BetterList<RankVo>() {});
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public void Initial()
        { 
            //计算恶魔洞窟消耗数量
            TickCount();
        }

        public void TickCount()
        {
            //当前恶魔契约数量
            _infoVo.TickCount[DemonDiffEnum.Level1] = GetTickCountById(GetDemonVoById(Level1Id));
            _infoVo.TickCount[DemonDiffEnum.Level2] = GetTickCountById(GetDemonVoById(Level2Id));
            _infoVo.TickCount[DemonDiffEnum.Level3] = GetTickCountById(GetDemonVoById(Level3Id));
            

            int gate = (int)_infoVo.CurGate[_curLevel]; //当前波数
            //消耗凭证
            BetterList<DemonVoItem> consume = _infoVo.ConsumeTickCount[_curLevel] as BetterList<DemonVoItem>;
            consume.Clear();

            //继续的恶魔契约数量
            if (gate == 0)
            {
                consume.Add(new DemonVoItem() { Id = GetDemonVoById(GetIdByGateAndDiff(_curLevel, 1)).ConsumeItems[0].Id, Nums=0 });
            }
            else
            {   
                //因为是下一波  +1
                if (gate == 100)
                {
                    gate = 1;
                    consume.Add(GetDemonVoById(GetIdByGateAndDiff(_curLevel, gate)).ConsumeItems[0]);
                }
                else {
                    consume.Add(GetDemonVoById(GetIdByGateAndDiff(_curLevel, gate + 1)).ConsumeItems[0]);
                }
               
            }
            //添加数据
            consume.Add(GetDemonVoById(GetIdByGateAndDiff(_curLevel, GATE1)).ConsumeItems[0]);
            consume.Add(GetDemonVoById(GetIdByGateAndDiff(_curLevel, GATE31)).ConsumeItems[0]);
            consume.Add(GetDemonVoById(GetIdByGateAndDiff(_curLevel, GATE61)).ConsumeItems[0]);
            
        }

        private int GetTickCountById(DemonVo vo)
        {
            //绑定物品个数
            int bindItem1Count = (int)ItemManager.GetInstance().GetItemNumById((uint)vo.ConsumeItems[0].Id);
            //未绑定物品个数
            int item1Count = (int)ItemManager.GetInstance().GetItemNumById((uint)vo.ConsumeItems[1].Id);
            return bindItem1Count + item1Count;
        }

        /// <summary>
        /// 设置当前选择等级
        /// </summary>
        /// <param name="lvl"></param>
        public void SetLevel(DemonDiffEnum lvl)
        {
            _curLevel = lvl;
        }

        /// <summary>
        /// 请求接收挑战奖励物品
        /// </summary>
        /// <param name="id"></param>
        public void SendReceiveItem(int id)
        {

            DemonVo dv = _demonHash[id] as DemonVo;
            if (dv.IsComplate)
            {
                if (!dv.IsReceive)
                {
                    _receiveId = id;
                    _ask48.m_u32TowerID = (uint)id;
                    _ask48.m_u16TowerType = 0;
                    NetBase.GetInstance().Send(_ask48.ToBytes());
                }
            }
            

            
           
        }
        //返回结果
        public void CallBackReceiveItem()
        {
            if (_receiveId!=0)
            {
                AddAwardInfo(_receiveId, true);
                _receiveId = 0;
                Gate.instance.sendNotification(MsgConstant.MSG_DEMON_DISPLAY_ITEMLIST,_curLevel);
            }
        }

        /// <summary>
        /// 添加奖励列表
        /// </summary>
        /// <param name="lvl"></param>
        /// <param name="id"></param>
        /// <param name="isGet"></param>
        public void AddAwardInfo(DemonDiffEnum lvl, int id, bool isGet)
        {
            DemonVo dv= _demonHash[id] as DemonVo;
            dv.IsReceive = isGet;
            dv.IsComplate = true;
            Hashtable _curLvl= GetLevelHash(lvl);

            if (_curLvl.ContainsKey(dv.Id))
            {
                _curLvl[dv.Id]=dv;                
            }
            else {
                dv.IsReceive = isGet;
            }
        }
        public void AddAwardInfo( int id, bool isGet)
        {
            DemonVo dv = _demonHash[id] as DemonVo;
            dv.IsReceive = isGet;

            Hashtable _curLvl = GetLevelHash((DemonDiffEnum)dv.Diff);

            if (_curLvl.ContainsKey(dv.Id))
            {
                _curLvl[dv.Id] = dv;
            }
            else
            {
                dv.IsReceive = isGet;
            }
        }

        /// <summary>
        /// 请求爬塔基本数据
        /// </summary>
        public void RequestDemonInfo()
        {
                _ask47.m_un16Type = 1;
                NetBase.GetInstance().Send(_ask47.ToBytes(), true);
        }

        /// <summary>
        /// 发送进入恶魔洞窟
        /// </summary>
        /// <param name="option">
        /// 0=继续
        /// 1=第一波
        /// 2=第31波
        /// 3=第61波
        /// </param>
        /// <param name="isReGo"></param>
        public void RequestEnterTower(int option)
        {
            DemonVo dv=new DemonVo();
            int id=0;
            int gate=0;      //今日最大等级
            bool isSelectIndex = false;
            switch (option)
            {
                case 0:
                    gate = (int)_infoVo.CurGate[_curLevel];
                    if (gate == 100)
                    {
                        gate = 1;
                    }
                    else
                    {
                        gate++;
                    }
                    dv = DemonHash[GetIdByGateAndDiff(_curLevel, gate)] as DemonVo;
                    if (gate==0||gate>100)//标示从没打过，所以不能继续,或者通关了100层，不可以在继续
                    {
                        ViewHelper.DisplayMessage(string.Format(LanguageManager.GetText("demon_rank_no_enough"),
                        GetLevelString((DemonDiffEnum)dv.Diff),
                        dv.UnLockId_Level));
                        return;
                    }
                    break;
                case 1:
                    id = GetIdByGateAndDiff(_curLevel, 1);
                    dv = DemonHash[id] as DemonVo;
                    isSelectIndex = true;
                    break;
                case 2:
                    id = GetIdByGateAndDiff(_curLevel, 31);
                    dv = DemonHash[id] as DemonVo;
                    break;
                case 3:
                    id = GetIdByGateAndDiff(_curLevel, 61);
                    dv = DemonHash[id] as DemonVo;
                    break;
                default:
                    break;
            }
            if (dv.UnLockLevel <= CharacterPlayer.character_property.getLevel())//lvl
            {
                int curId = 0;
                if (isSelectIndex)
                {
                    curId = GetIdByGateAndDiff(_curLevel-1, (int)_infoVo.CurGate[_curLevel-1] + 1); 
                }
                else {
                    curId = GetIdByGateAndDiff(_curLevel, (int)_infoVo.CurGate[_curLevel] + 1);
                }

                if (dv.UnLockId <= curId)
                {
                    if (option == 0)
                    {
                        //继续下一波
                       // CheckIsHaveItem(dv.Id, 0x01);
                        SendEnterDemon(dv.Id, 0x01);
                    }
                    else {
                        CheckIsHaveItem(dv.Id, 0x00);
                    }
                    
                }else{
                    if (isSelectIndex)
                    {
                        //显示需要通关前面的层次
                        ViewHelper.DisplayMessage(string.Format(LanguageManager.GetText("demon_rank_no_enough"),
                            GetLevelString((DemonDiffEnum)(dv.Diff-1)),
                            dv.UnLockId_Level));
                    }
                    else {
                        //显示需要通关前面的层次
                        ViewHelper.DisplayMessage(string.Format(LanguageManager.GetText("demon_rank_no_enough"),
                            GetLevelString((DemonDiffEnum)dv.Diff),
                            dv.UnLockId_Level));
                    }
                   
				}
            }
            else { 
                //TODE::等级不足
                string msg = string.Format(LanguageManager.GetText("demon_level_no_enough"), dv.UnLockLevel);
                ViewHelper.DisplayMessage(msg);
            }
        }

        public int GetIdByGateAndDiff(DemonDiffEnum diff, int gate)
        {
            return ((int)diff) * 10000 + gate;
        }
        /// <summary>
        /// 检查下一关是否等级足够
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckCanbeGoing(int id)
        {
            DemonVo vo = GetDemonVoById(id);
            if (vo.UnLockLevel>CharacterPlayer.character_property.getLevel())
            {
                return false;
            }
            return true;
        }

        public void SendEnterDemon(int id, byte curWave)
        {
            _ask45.m_un32TowerId = (uint)id;
            _ask45.m_bCurWave = curWave;
            Global.cur_TowerId = (uint)id;
            NetBase.GetInstance().Send(_ask45.ToBytes());
        }

        /// <summary>
        /// 需要进入的层数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public void CheckIsHaveItem(int id,byte curWave)
        {
            DemonVo vo= GetDemonVoById(id);
            //绑定物品个数
            int bindItem1Count=(int)ItemManager.GetInstance().GetItemNumById((uint)vo.ConsumeItems[0].Id);
            //未绑定物品个数
            int item1Count = (int)ItemManager.GetInstance().GetItemNumById((uint)vo.ConsumeItems[1].Id);

            if (bindItem1Count>=vo.ConsumeItems[0].Nums)    //绑定的够了
            {
                SendEnterDemon(id, curWave);
                return;
            }else  if (item1Count+bindItem1Count>=vo.ConsumeItems[0].Nums) //绑定+未绑定的足够了
            {
                SendEnterDemon(id, curWave);
                return;
            }

            if (CharacterPlayer.character_asset.Crystal >= vo.ConsumeGolds[0].Nums)
            {
                //提示消耗水晶进入
                TempEnterID = id;
                TempEnterWave = curWave;
                UIManager.Instance.ShowDialog(eDialogSureType.eSureComsumeCry,
                    string.Format(LanguageManager.GetText("comsume_crystal"), vo.ConsumeGolds[0].Nums));
                return;
            }
            else if (CharacterPlayer.character_asset.diamond >= vo.ConsumeGolds[1].Nums)
            {
                TempEnterID = id;
                TempEnterWave = curWave;
                UIManager.Instance.ShowDialog(eDialogSureType.eSureComsumeCry,
                    string.Format(LanguageManager.GetText("consume_dm"), vo.ConsumeGolds[1].Nums));
                return;
            }
            ViewHelper.DisplayMessage(LanguageManager.GetText("consume_noenough"));
            
        }
        /// <summary>
        /// 消耗水晶，或钻石
        /// </summary>
        public void DialogSure()
        {
            if (TempEnterID!=-1)
            {
                SendEnterDemon(TempEnterID, TempEnterWave);
                TempEnterID = -1;
            }
        }


        /// <summary>
        /// 添加排名
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="career"></param>
        /// <param name="lvl"></param>
        /// <param name="name"></param>
        /// <param name="gate"></param>
        public void AddCurRank(DemonDiffEnum diff,int rank,CHARACTER_CAREER career,int lvl,string name,int gate)
        {
            rank++;//从1开始排
            DemonVoItem dv = FindAward(rank);
            RankVo vo = new RankVo();
            vo.Id = rank;
            vo.Career = career;
            vo.Level = lvl;
            vo.Name = name;
            vo.RankTower = gate;
            vo.ItemIcon = FindIcon(dv.Id);
            vo.ItemCount = dv.Nums;

            BetterList<RankVo> rvs= _rankList[diff] as BetterList<RankVo>;
            rvs.Add(vo);
        }
        public void ClearCurRank()
        {
            _rankList[DemonDiffEnum.Level1] = new BetterList<RankVo>();
            _rankList[DemonDiffEnum.Level2] = new BetterList<RankVo>();
            _rankList[DemonDiffEnum.Level3] = new BetterList<RankVo>();
        }

        //根据排名找奖励
        public DemonVoItem FindAward(int rank)
        {
            foreach (RankXmlVo xv in _rankRewardHash.Values)
            {
                if (rank<=xv.MaxRank&&rank>=xv.MinRank)
                {
                    return xv.Items[0];
                }
            }
            return new DemonVoItem();
        }
        //根据ID找icon
        public string FindIcon(int id)
        {
            return ConfigDataManager.GetInstance().getItemTemplate().getTemplateData(id).icon;
        }
        public string FindBoderById(int id)
        {
            return BagManager.Instance.getItemBgByType(ConfigDataManager.GetInstance().getItemTemplate().getTemplateData(id).quality, true);
        }

        /// <summary>
        /// 请求今日排名
        /// </summary>
        public void RequestCurRankData()
        {
            if (IsFristRequestCurRank)
            {
                IsFristRequestCurRank = false;
                _ask47.m_un16Type = 3;
                NetBase.GetInstance().Send(_ask47.ToBytes());
            }
            else {
                Gate.instance.sendNotification(MsgConstant.MSG_DEMON_DISPLAY_CUR_RANK, true);
            }
            
        }

        /// <summary>
        /// 添加昨日排名数据
        /// </summary>
        /// <param name="diff"></param>
        /// <param name="rank"></param>
        /// <param name="gate"></param>
        /// <param name="type"></param>
        public void AddHistoryRankData(DemonDiffEnum diff,int rank,int gate,int type)
        {
            HistoryRankVo vo = new HistoryRankVo();
            vo.Id = rank;
            vo.Gate = gate;
            vo.IsReceive = Convert.ToBoolean(type);
            if (vo.Id == 0)
            {
                vo.IsAward = false; 
            }
            else {
                vo.IsAward = true;
                DemonVoItem dv= FindAward(vo.Id);
                vo.AwardIcon= FindIcon(dv.Id);
                vo.AwardCount = dv.Nums;
            }
            _infoVo.HistoryRank[diff]=vo;
        }

        //请求昨日排名数据
        public void RequestHistoryData()
        {
            if (IsFristRequestHistoryData)
            {
                IsFristRequestHistoryData = false;
                _ask47.m_un16Type = 2;
                NetBase.GetInstance().Send(_ask47.ToBytes());
            }
            else {
                Gate.instance.sendNotification(MsgConstant.MSG_DEMON_DISPLAY_RANK, true);
            }  
        }

        //打开窗口
        public void OpenWindow()
        {
            UIManager.Instance.openWindow(UiNameConst.ui_demon);
        }

        /// <summary>
        /// 打开通关信息界面
        /// </summary>
        /// <param name="type"></param>
        public void OpenDemonAnceWindow(DemonAnceView.DemonArceType type)
        {
            UIManager.Instance.openWindow(UiNameConst.ui_demonAnce);
            Gate.instance.sendNotification(MsgConstant.MSG_DEMON_ARCE_DISPLAY_PAGE, type);
        }

        public void CloseWindow()
        {
            IsFristRequestHistoryData = true;
            IsFristRequestCurRank = true;
            UIManager.Instance.closeWindow(UiNameConst.ui_demon);
        }

       

        public Hashtable DemonHash
        {
            get { return _demonHash; }
        }

        public Hashtable CurDemonList
        {
            get { return _curDemonList; }
        }


        public Hashtable RankRewardHash
        {
            get { return _rankRewardHash; }
        }
        public Hashtable GetLevelHash(DemonDiffEnum dd)
        {
            return _curDemonList[dd] as Hashtable;
        }

        public BetterList<RankVo> GetRankLevel(DemonDiffEnum dd)
        { 
            return _rankList[dd] as BetterList<RankVo>;
        }

        public DemonVo GetDemonVoById(uint id)
        {
            return _demonHash[(int)id] as DemonVo;
        }
        public DemonVo GetDemonVoById(int id)
        {
            return _demonHash[id] as DemonVo;
        }
        
        /// <summary>
        /// 排名列表
        /// </summary>
        public Hashtable RankList
        {
            get { return _rankList; }
        }
        public static DemonManager Instance
        {
            get {
                if (_instance==null)
                {
                    _instance = new DemonManager();
                }
                return DemonManager._instance; 
            }
        }


        public static string GetLevelString(DemonDiffEnum dd)
        {
            string level="null";
            switch (dd)
            {
                case DemonDiffEnum.Begin:
                    break;
                case DemonDiffEnum.Level1:
                    level= "噩梦";
                    break;
                case DemonDiffEnum.Level2:
                    level = "地狱";
                    break;
                case DemonDiffEnum.Level3:
                    level = "炼狱";
                    break;
                case DemonDiffEnum.End:
                    break;
                default:
                    break;
            }
            return level;
        }
        public DemonDiffEnum CurLevel
        {
            get { return _curLevel; }
        }
        public DemonInfoVo InfoVo
        {
            get { return _infoVo; }
            set { _infoVo = value; }
        }
    }
}
