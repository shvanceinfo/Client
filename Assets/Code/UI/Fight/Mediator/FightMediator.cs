using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MVC.entrance;
using MVC.core;
using MVC.entrance.gate;
using MVC.interfaces;
using manager;
using model;

namespace mediator
{
	public class FightMediator:ViewMediator
	{
		FightView _fightView;

		public FightMediator (FightView view, uint id=MediatorName.FIGHT_MEDIATOR):base(id,view)
		{
			_fightView = view;
		}

		public override IList<uint> listReferNotification ()
		{
			return new List<uint>
            { 
                MsgConstant.MSG_FIGHT_SEND_USE_SKILL,       //发送使用技能
                MsgConstant.MSG_SKILL_INITIAL_SKILL_LIST,   //技能数据获取
                MsgConstant.MSG_FIGHT_REFRESH_LEVEL,
                MsgConstant.MSG_FIGHT_REFRESH_HEALT_MAGIC,
                MsgConstant.MSG_FIGHT_USE_HHEAL_MAGIC_ITEM,
                MsgConstant.MSG_FIGHT_REFRESH_HPMP_VIEW,
                MsgConstant.MSG_FIGHT_INITI_ENTER,
                MsgConstant.MSG_FIGHT_EXP_CHANGE,
                MsgConstant.MSG_FIGHT_BOSS_SHOW,
                MsgConstant.MSG_FIGHT_BOSS_HIDEEN,
                MsgConstant.MSG_FIGHT_BOSS_HEALTBAR,
                MsgConstant.MSG_FIGHT_DISPLAY_AWARD_ITEM,
                MsgConstant.MSG_FIGHT_DELETE_AWARD_ITEM,
                MsgConstant.MSG_FIGHT_DISPLAY_AWARD_ITEM_DATA_COROUTINE,
                MsgConstant.MSG_FIGHT_MULTI_PLAYER_DEAD,
                MsgConstant.MSG_FIGHT_MULTI_PLAYER_DISCONNECT,
                MsgConstant.MSG_FIGHT_MULTI_PLAYER_HEALT_CHANGE,
                MsgConstant.MSG_FIGHT_DISPLAY_TIME,
                MsgConstant.MSG_FIGHT_STOP_TIME,
                MsgConstant.MSG_FIGHT_DISPLAY_GOBLIN_FUCNTION,
                MsgConstant.MSG_FIGHT_DISPLAY_GOBLIN_GOLD_NUM,
                MsgConstant.MSG_FIGHT_DISPLAY_BOSS_TIPS,
                MsgConstant.MSG_FIGHT_DISPLAY_MAP_NAME_MSG,
                MsgConstant.MSG_FIGHT_DISPLAY_TALK_MSG,
				MsgConstant.MSG_FIGHT_WORLD_BOSS_UPDATE_INFO,
				MsgConstant.MSG_FIGHT_BOSS_BTN_SWITCH,
                MsgConstant.MSG_FIGHT_COMBO,
				MsgConstant.MSG_FIGHT_BUFF_COUNT,
				MsgConstant.MSG_FIGHT_UPDATE_FUNCTION,
                MsgConstant.MSG_FIGHT_DISPLAY_ARENA_PLAYER,
                MsgConstant.MSG_FIGHT_DISPLAY_ARENA_PLAYER_HEALTH,
                MsgConstant.MSG_MIAN_UPDATE_CHANNEL,
                MsgConstant.MSG_SETTING_PEOPLE_SWITCH,
                MsgConstant.MSG_SHOW_BAOJI_ANIM,
				MsgConstant.MSG_FIGHT_UPDATE_ASSET,
                MsgConstant.MSG_FIGHT_DISPLAY_PET_SKILL,
            };
		}

		public override void handleNotification (INotification notification)
		{
			if (View != null) {
				switch (notification.notifyId) {
				case MsgConstant.MSG_FIGHT_SEND_USE_SKILL:
					FightManager.Instance.SendUseSkill ((int)notification.body);
					break;
				case MsgConstant.MSG_SKILL_INITIAL_SKILL_LIST:
					View.DisplaySkills ();
					break;
				case MsgConstant.MSG_FIGHT_REFRESH_LEVEL:
					View.DisplayLevel ();        //刷新等级
					break;
				case MsgConstant.MSG_FIGHT_REFRESH_HEALT_MAGIC:
					View.DisplayHealt_Magic ();  //刷新生命魔法
					break;
				case MsgConstant.MSG_FIGHT_USE_HHEAL_MAGIC_ITEM:
					FightManager.Instance.UseHpMpItem ();
					break;
				case MsgConstant.MSG_FIGHT_REFRESH_HPMP_VIEW:
					View.DisplayUseItem ((int)notification.body);
					break;
				case MsgConstant.MSG_FIGHT_INITI_ENTER:
					FightManager.Instance.UpdatePlayerAsset();
					FightManager.Instance.InitHpMpCount ();                        
					break;
				case MsgConstant.MSG_FIGHT_EXP_CHANGE:
					FightManager.Instance.UpdateExpChange ();
					View.DisplayExpBar ();
					break;
				case MsgConstant.MSG_FIGHT_BOSS_SHOW:
					FightManager.Instance.AddBossInfo ((MonsterProperty)notification.body);
					View.ShowBossBar ();
					View.DisplayBossBar ();
					break;
				case MsgConstant.MSG_FIGHT_BOSS_HIDEEN:
					FightManager.Instance.AddBossInfo (null);
					View.HiddenBossBar ();
					break;
				case MsgConstant.MSG_FIGHT_BOSS_HEALTBAR:
					View.DisplayBossBar ();
					break;
				case MsgConstant.MSG_FIGHT_DISPLAY_AWARD_ITEM:
					View.DisplayAwardItem ();
					break;
				case MsgConstant.MSG_FIGHT_DISPLAY_AWARD_ITEM_DATA_COROUTINE:
					View.DisplayAwardItemSec ();
					break;
				case MsgConstant.MSG_FIGHT_DELETE_AWARD_ITEM:
					AwardManager.Instance.DeleteAwardItem ();
					break;
				case MsgConstant.MSG_FIGHT_MULTI_PLAYER_DEAD:
					break;
				case MsgConstant.MSG_FIGHT_MULTI_PLAYER_DISCONNECT:
					break;
				case MsgConstant.MSG_FIGHT_MULTI_PLAYER_HEALT_CHANGE:
					View.DisplayChangeOtherPlayer_HealtBar ();
					break;
				case MsgConstant.MSG_FIGHT_DISPLAY_TIME:
					View.StartTime ();
					break;
				case MsgConstant.MSG_FIGHT_STOP_TIME:
					View.StopTime ();
					break;
				case MsgConstant.MSG_FIGHT_DISPLAY_GOBLIN_FUCNTION:
					View.DisplayGoblin_Fucntion ();
					break;
				case MsgConstant.MSG_FIGHT_DISPLAY_GOBLIN_GOLD_NUM:
					View.DisplayGoblin_Gold ((int)notification.body);
					break;
				case MsgConstant.MSG_FIGHT_DISPLAY_BOSS_TIPS:
					View.DisplayTip ((string)notification.body);
					break;
				case MsgConstant.MSG_FIGHT_DISPLAY_MAP_NAME_MSG:
					View.DisplayMapName (notification.body.ToString ());
					break;
				case MsgConstant.MSG_FIGHT_DISPLAY_TALK_MSG:
					View.DisplayTalkMsg ();
					break;
				case MsgConstant.MSG_FIGHT_WORLD_BOSS_UPDATE_INFO:
					View.UpdateWorldBossInfo (BossManager.Instance.BossDamageVo);
					break;
				case MsgConstant.MSG_FIGHT_BOSS_BTN_SWITCH:
					View.SwitchBossBtn ();
					break;
				case MsgConstant.MSG_FIGHT_COMBO:
					View.DisplayCombo ();
					break;
				case MsgConstant.MSG_FIGHT_BUFF_COUNT:
					View.UpdateBuffInfo (BossManager.Instance.BuffValue);
					break;
				case MsgConstant.MSG_FIGHT_UPDATE_FUNCTION:
					View.UpdateShowOrHideUi ();
					break;
				case MsgConstant.MSG_FIGHT_DISPLAY_ARENA_PLAYER:
					View.DisplayArenaPlayInfo ();
					break;
				case MsgConstant.MSG_FIGHT_DISPLAY_ARENA_PLAYER_HEALTH:
					View.DisplayArenaHealthMagic ();
					break;
				case MsgConstant.MSG_MIAN_UPDATE_CHANNEL:
					View.DisplayChange ();
					break;
				case MsgConstant.MSG_SETTING_PEOPLE_SWITCH:
					View.DisplayHide ();
					break;
				case MsgConstant.MSG_SHOW_BAOJI_ANIM:
					
					break;
				case MsgConstant.MSG_FIGHT_UPDATE_ASSET:
					View.UpdatePlayerAsset (CharacterPlayer.character_asset.diamond, CharacterPlayer.character_asset.gold, CharacterPlayer.character_asset.Crystal);
					break;
                    case MsgConstant.MSG_FIGHT_DISPLAY_PET_SKILL:
                    View.DisplayPetSkill();
                    break;
				} 
			}
            
		}

		public FightView View {
			get {
				return _fightView;
			}
		}
	}
}
