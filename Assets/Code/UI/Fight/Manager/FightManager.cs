
using UnityEngine;
using System.Collections;
using model;
using NetGame;
using MVC.entrance.gate;
namespace manager
{
    /// <summary>
    /// 战斗管理
    /// </summary>
    public class FightManager
    {
        static FightManager _instance;

        private FightVo _itemData;

        private GCAskUseItem _askUseItem;
        private FightManager()
        {
            _itemData = new FightVo();
            _askUseItem = new GCAskUseItem(0, 0);
        }
       

        //发送使用技能信息
        public void SendUseSkill(int id)
        {
            if (id == 0)
            {
                SendUseAttack(id);
            }
            else {
                int cur_mp = CharacterPlayer.character_property.GetMP();
                CharacterPlayer.character_property.SetMP(cur_mp - SkillTalentManager.Instance.GetSkillVo(id).Mp_Cost);
                CharacterPlayer.sPlayerMe.player_ai.SendStateMessage(CharacterAI.CHARACTER_STATE.CS_SKILL, id);
            }
        }
        public void SendUseAttack(int id)
        {
            
        }

        public void UseHpMpItem()
        {
            if (CharacterAI.IsInState(CharacterPlayer.sPlayerMe, CharacterAI.CHARACTER_STATE.CS_DIE))
            {
                return;
            }

            if (CharacterPlayer.sPlayerMe.GetProperty().HPMPIsEnough())
            {
                FloatMessage.GetInstance().PlayNewFloatMessage(LanguageManager.GetText("cur_hp_mp_enough"), true,
                UIManager.Instance.getRootTrans());
                return;
            }
            if (_itemData.CurHpMpItemCount > 0)
            {
                _itemData.CurHpMpItemCount--;
                this.SendUseItem();
                if (_itemData.CurHpMpItemCount == 0)
                {
                    int pre = ConfigDataManager.GetInstance().GetHPMPConfig().getHPMPData(_itemData.CurBuyHpMpItemCount + 1).dia_price;
                    Gate.instance.sendNotification(MsgConstant.MSG_FIGHT_REFRESH_HPMP_VIEW, pre);
                }
                else {
                    Gate.instance.sendNotification(MsgConstant.MSG_FIGHT_REFRESH_HPMP_VIEW, 0);
                }
                
            }
            else {
                int price= ConfigDataManager.GetInstance().GetHPMPConfig().getHPMPData(_itemData.CurBuyHpMpItemCount+1).dia_price;
                int nextPrice= ConfigDataManager.GetInstance().GetHPMPConfig().getHPMPData(_itemData.CurBuyHpMpItemCount+2).dia_price;
                if (CharacterPlayer.character_asset.diamond >= price)
                {
                    this.SendUseItem();
                    _itemData.CurBuyHpMpItemCount++;
                    Gate.instance.sendNotification(MsgConstant.MSG_FIGHT_REFRESH_HPMP_VIEW, nextPrice);
                }
                else {
                    //钻石不足
                    FloatMessage.GetInstance().PlayNewFloatMessage(LanguageManager.GetText("use_hp_mp_diamond_not_enough"), true,
               UIManager.Instance.getRootTrans());
                }

            }
           
        }
        private void SendUseItem()
        {
            
            _askUseItem.m_un16OperateID = (ushort)ResourceEnum.UseHealItem;
            _askUseItem.m_u32MaterialID = 0;
            NetBase.GetInstance().Send(_askUseItem.ToBytes());
        }

        /// <summary>
        /// 每次进入副本初始化一次物品数量
        /// </summary>
        public void InitHpMpCount()
        {
            _itemData.CurHpMpItemCount  = ConfigDataManager.GetInstance().getMapConfig().getMapData(CharacterPlayer.character_property.getServerMapID()).nXuePing;
            _itemData.CurBuyHpMpItemCount = 0;
            Gate.instance.sendNotification(MsgConstant.MSG_FIGHT_REFRESH_HPMP_VIEW, -1);
        }


        public void UpdateExpChange()
        {
            int nextTempId = (int)CharacterPlayer.character_property.career * 10000 + CharacterPlayer.character_property.level;
            RoleDataItem roleProp = ConfigDataManager.GetInstance().getRoleConfig().getRoleData(nextTempId);
            _itemData.CurExp = CharacterPlayer.character_property.getExperience();
            _itemData.NextExp = roleProp.upgrade_exp;
        }


        public void AddBossInfo(MonsterProperty mp)
        {
            _itemData.Boss = mp;
        }


		/// <summary>
		/// 更新功能UI
		/// </summary>
		public void UpdateFunction(){
			Gate.instance.sendNotification (MsgConstant.MSG_FIGHT_UPDATE_FUNCTION); 
		}


		public void UpdatePlayerAsset ()
		{
			Gate.instance.sendNotification (MsgConstant.MSG_FIGHT_UPDATE_ASSET);
		}


		#region 注册事件
		public void RegsiterEvent ()
		{
			EventDispatcher.GetInstance ().PlayerAsset += UpdatePlayerAsset;
		}
		#endregion
		
		#region 取消注册事件
		public void RemoveEvent ()
		{
			EventDispatcher.GetInstance ().PlayerAsset -= UpdatePlayerAsset;
		}
		#endregion



        public static FightManager Instance
        {
            get {
                if (_instance==null)
                {
                    _instance = new FightManager();
                }
                return _instance;
            }
        }

        public FightVo ItemData
        {
            get { return _itemData; }
            set { _itemData = value; }
        }
    }
}
