using UnityEngine;
using System.Collections;
using model;
using mediator;
using MVC.entrance.gate;
using NetGame;
namespace manager
{
    public class AwardManager
    {
        static AwardManager _instance;
        private AwardVo _vo;

        private AwardManager()
        {
            _vo = new AwardVo();
        }

        private bool IsExits(uint id)
        {
            for (int i = 0; i < _vo.CurAwardItemList.size; i++)
            {
                if (_vo.CurAwardItemList[i].tempId==id)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 添加奖励物品
        /// </summary>
        /// <param name="item"></param>
        public void AddAwardItem(ItemStruct item)
        {
            if (IsExits(item.tempId))
            {

                if ((item.tempId == 0) || (item.num == 0))
                {
                    _vo.CurAwardItemList.Remove(item);
                }
                else
                {
                    for (int i = 0; i < _vo.CurAwardItemList.size; i++)
                    {
                        if (_vo.CurAwardItemList[i].instanceId == item.instanceId)
                        { _vo.CurAwardItemList[i].num += item.num; break; }
                    }
                    
                }
            }
            else if ((item.tempId != 0) && (item.num > 0))
            {
                _vo.CurAwardItemList.Add(item);

                if (_vo.CurDisplayAwardList.size != 0)
                {
                    _vo.CurDisplayAwardList.Add(item);
                    if (_vo.CurDisplayAwardList.size>1)
                    {
                        Gate.instance.sendNotification(MsgConstant.MSG_FIGHT_DISPLAY_AWARD_ITEM_DATA_COROUTINE);
                    }
                }
                else {
                    _vo.CurDisplayAwardList.Add(item);
                    Gate.instance.sendNotification(MsgConstant.MSG_FIGHT_DISPLAY_AWARD_ITEM);
                } 
            }
            //Gate.instance.sendNotification(MsgConstant.MSG_AWARD_DISPALY_INFO);
        }
        /// <summary>
        /// 通过物品实例获取模板信息
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        public ItemTemplate GetTemplateByTempId(uint templateId)
        {
            ItemTemplate temp = ConfigDataManager.GetInstance().getItemTemplate().getTemplateData((int)templateId);
            return temp;
        }
        /// <summary>
        ///获取第N个奖励物品
        /// </summary>
        public ItemStruct GetIndexAwardItem(int n)
        {
            if (_vo.CurDisplayAwardList.size >= n)
            {
                return _vo.CurDisplayAwardList[n-1];
            }
            else {
                return null;
            }
        }

        public void DeleteAwardItem()
        {
            if (_vo.CurDisplayAwardList.size!=0)
            {
                _vo.CurDisplayAwardList.RemoveAt(0);
                Gate.instance.sendNotification(MsgConstant.MSG_FIGHT_DISPLAY_AWARD_ITEM);
            }
        }

        /// <summary>
        /// 发送通关信息
        /// </summary>
        public void SendGoHome()
        {
            NetBase net = NetBase.GetInstance();
            GCReportPassGate passGateMsg = new GCReportPassGate();

            passGateMsg.m_un32MapID = (uint)CharacterPlayer.character_property.getServerMapID();
            passGateMsg.m_un32SceneID = (uint)CharacterPlayer.character_property.getServerSceneID();
            passGateMsg.m_un32HPVessel = CharacterPlayer.character_property.getCurHPVessel(); ;
            passGateMsg.m_un32MPVessel = CharacterPlayer.character_property.getCurMPVessel();
            passGateMsg.m_un32GotExpNum = (uint)_vo.GetExp;
            passGateMsg.m_un32UseSecond = (uint)_vo.GetTime;
            passGateMsg.m_un32GotSilverNum = (uint)_vo.GetMoney;
            passGateMsg.m_bPickAllTempGoods = 1;
            net.Send(passGateMsg.ToBytes());
            TaskManager.Instance.checkTaskComplete(eTaskFinish.finishGate, SceneManager.Instance.nextMapID);
            ClearOtherInfo();

        }
        private void ClearOtherInfo()
        {
            _vo.CurAwardItemList.Clear();
            _vo.CurDisplayAwardList.Clear();
            _vo.EnterMoney = 0;
            _vo.EnterTime = 0;
            _vo.EnterExp = 0;
            _vo.IsUseRevive = false;
        }

        /// <summary>
        /// 显示面板的时候，调用次方法，计算通关信息
        /// </summary>
        public void InitialAwardInfo()
        {
            //_vo.GetTime = (int)(Time.time - _vo.EnterTime);
            _vo.GetTime = (int)(FightManager.Instance.ItemData.Time);
            MapDataItem data = ConfigDataManager.GetInstance().getMapConfig().getMapData(CharacterPlayer.character_property.getServerMapID());
            _vo.GetTime = (int)(Time.time - _vo.EnterTime);
            _vo.GetMoney = CharacterPlayer.character_asset.gold - _vo.EnterMoney;
           
			int nCurlevel = CharacterPlayer.character_property.getLevel();
			RoleDataItem rdi = ConfigDataManager.GetInstance().getRoleConfig().getRoleData((int)CharacterPlayer.character_property.career*Constant.LEVEL_RATIO + nCurlevel);
			int nAfterExp = FightManager.Instance.ItemData.CurExp + rdi.upgrade_exp;
			int nBeforeExp = _vo.EnterExp;
			_vo.GetExp = nAfterExp - nBeforeExp;

            _vo.GetMoney += DropOutManager.Instance.FindVoByDropOutId(data.dropOutId).Gold;
            int startTime = data.starTime;
            int lvl = 1;
            if (_vo.GetTime<startTime)
            {
                lvl++;
            }
            if (!_vo.IsUseRevive)
            {
                lvl++;
            }
            _vo.AssessLvl = (AwardView.AssessLevel)lvl;
        }



        /// <summary>
        /// 进入副本
        /// </summary>
        public void EnterPoint()
        {
            Vo.RevivalCount = 0;
            Vo.PlayerIsDead = false;
            Vo.IsUseRevive = false;
            Vo.EnterMoney = CharacterPlayer.character_asset.gold;
            Vo.EnterTime = Time.time;

			int level = CharacterPlayer.character_property.getLevel();
			RoleDataItem rdi = ConfigDataManager.GetInstance().getRoleConfig().getRoleData((int)CharacterPlayer.character_property.career*Constant.LEVEL_RATIO + level);
			Vo.EnterExp = CharacterPlayer.character_property.getExperience () + rdi.upgrade_exp;

            int mapid=CharacterPlayer.character_property.getServerMapID();
            MapDataItem md=ConfigDataManager.GetInstance().getMapConfig().getMapData(mapid);
            Vo.MapName=md.name+"-"+md.mapTip;
        }
    

        public AwardVo Vo
        {
            get { return _vo; }
            set { _vo = value; }
        }
        public static AwardManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AwardManager(); return _instance;
                }
                return AwardManager._instance;
            }
        }

        
    }
}
