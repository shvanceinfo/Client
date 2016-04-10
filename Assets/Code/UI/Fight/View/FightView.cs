using UnityEngine;
using System.Collections;
using manager;
using model;
using MVC.entrance.gate;
using mediator;
using System.Collections.Generic;
using System;
using helper;
using System.Text;

public class FightView : MonoBehaviour
{
	public Vector3 _bottomBaseLocation = new Vector3 (306, 1.6f, 0);
	public Vector3 _bottomStepValue = new Vector3 (-80, 0, 0);
	public int _bottomCount = 0;
	const float MAX_AWARD_SHOW_TIME = 2;    //最大奖励显示时间
	const int MAX_OTHER_PLAYER_COUNT = 2;   //最大其他玩家
	private Transform _trans;
	private Transform _handFunc;    //人物头像功能
	private UISprite handIcon;      //人物头像
	private UILabel lbl_Level;      //人物等级
	private UILabel lbl_Name;       //人物名称


	private GameObject _skillObj;     //技能对象
	private FightSkillItem[] _skills; //技能组件
    private FightSkillItem _skillPet;   //宠物技能
	private UITexture _attackIcon;
	private HealthBar _HpBar;
	private HealthBar _MpBar;
	private FightUseItem _useItem;
	private HealthBar _expBar;
	private HealthBar _bossBar;
	private GameObject _bossObj;
	private FightDisplayItem _indexItem;       //第一个掉落组件
	private FightDisplayItem _secItem;         //第二个掉落组件

    
	private FightDisplayPlayer _player1;
	private FightDisplayPlayer _player2;
	private UILabel _lblTime;               //倒计时标签
	private UISprite _spTime;               //倒计时背景
	private GameObject _timeObj;            //倒计时组件
	private UILabel _mapName;               //地图名字
	private TweenAlpha _mapTw;              //地图渐变
	private TweenAlpha _mapLeft;            //+1
	private TweenAlpha _mapRight;           //+1
	private TweenAlpha _mapBLeft;
	private TweenAlpha _mapBRight;
	private TweenAlpha _mapBack;
	private GameObject _mapObj;             //地图组件

	private GameObject _goblinObj;          //哥布林组件
	private UILabel _goblinNum;             //哥布林金币标签
	private GameObject _btnExit;            //退出按钮
	private GameObject _itemFunction;       //物品掉落组件
	private Vector3 _itemPos;               //如果是多人的话，就需要更改物品组件的位置

	private GameObject _chatObj;
	private UILabel _talkLbl;
	private GameObject _itemFunObj;		//使用物品的界面
	#region 世界boss
	private Transform _buff;	//buff
	private Transform _left;	//伤害排名
	private Transform _bossHealth;	//boss血条
	private UITexture _bossIcon;	//boss头像
	private UILabel _bosslevel;		//boss等级
	private UILabel _bossName; 		//boss名字
	
	
	private Transform _me;		//自己伤害
	private Transform _temp;    //其他用户的伤害
	private HealthBar _bossHp;	//世界boss血条
	private TweenPosition _bossDamageTp;//boss伤害动画
	private float _subX = -295;	//需要减的距离
	private bool _isOpenBossDamageBtn = true;//默认是打开的状态
	private Transform _arrow;
	#endregion

	private UICombo _combo;
	private UIBaoji _baoji;
	//竞技场其他人头像
    #region ArenaHead
	private Transform _arenaHeadFunc;
	private UILabel _arenaName;
	private UILabel _arenaLvl;
	private UISprite _arenaHead;
	private HealthBar _arenaHp;
	private HealthBar _arenaMp;
    #endregion

	//换线功能
	private GameObject _channel;
	private GameObject _hide;
	//隐藏玩家功能
	private UILabel _channelLbl;
	private UISprite _hideLbl;

	#region 资源
	private UILabel _lblDiamond ;
	private UILabel _lblGold;
	private UILabel _lblShuiJing;
	private float  _currencyShakeTime;	   	//计时器
	private Color _goldColor = Color.white; //货币发生变化
	private Color _diamondcolor = Color.white;	//金钱发生变化
	private float  _moneyShakeTime;			//金钱效果计时器
	#endregion

	private void Awake ()
	{


		this._trans = this.transform;
		_talkLbl = transform.FindChild ("Top_Right/Chat_Function/Label").GetComponent<UILabel> ();
		//头像
		_handFunc = transform.FindChild ("Top_Left/Hand_Function");
		handIcon = _handFunc.FindChild ("Icon_Hand").GetComponent<UISprite> ();
		lbl_Level = _handFunc.FindChild ("Lbl_Lvl").GetComponent<UILabel> ();
		lbl_Name = _handFunc.FindChild ("Lbl_Name").GetComponent<UILabel> ();
		//技能
		_skillObj = transform.FindChild ("Bottom_Right/Skill_Function").gameObject;
		_skills = new FightSkillItem[Constant.Fight_MaxSkillSize];
		_skills [0] = transform.FindChild ("Bottom_Right/Skill_Function/Skill_1").GetComponent<FightSkillItem> ();
		_skills [1] = transform.FindChild ("Bottom_Right/Skill_Function/Skill_2").GetComponent<FightSkillItem> ();
		_skills [2] = transform.FindChild ("Bottom_Right/Skill_Function/Skill_3").GetComponent<FightSkillItem> ();
		_skills [3] = transform.FindChild ("Bottom_Right/Skill_Function/Skill_4").GetComponent<FightSkillItem> ();
        _skillPet = transform.FindChild("Bottom_Right/Skill_Function/Skill_5").GetComponent<FightSkillItem>();
		_attackIcon = transform.FindChild ("Bottom_Right/Skill_Function/Skill_Attack/Icon").GetComponent<UITexture> ();
		
		//使用道具
		_itemFunObj = transform.FindChild ("Bottom_Right/Item_Function").gameObject;
 
		//血条，expbar
		_HpBar = _handFunc.FindChild ("Healt Bar").GetComponent<HealthBar> ();
		_MpBar = _handFunc.FindChild ("Magic Bar").GetComponent<HealthBar> ();
		_useItem = transform.FindChild ("Bottom_Right/Item_Function/Healt").GetComponent<FightUseItem> ();
		_expBar = transform.FindChild ("bottom/Exp_Function").GetComponent<HealthBar> ();
		_bossObj = transform.FindChild ("Top/Boss_Function").gameObject;

		//掉落物品
		_indexItem = transform.FindChild ("Top_Left/Item_Function/Panel/Grid/IndexItem").GetComponent<FightDisplayItem> ();
		_secItem = transform.FindChild ("Top_Left/Item_Function/Panel/Grid/Item").GetComponent<FightDisplayItem> ();

		_timeObj = transform.FindChild ("Top/Time_Function").gameObject;
		_lblTime = transform.FindChild ("Top/Time_Function/Label").GetComponent<UILabel> ();
		_spTime = transform.FindChild ("Top/Time_Function/Sprite").GetComponent<UISprite> ();

		//地图名称
		_mapName = transform.FindChild ("Top/MapName_Function/Label").GetComponent<UILabel> ();
		_mapTw = _mapName.GetComponent<TweenAlpha> ();
		_mapLeft = transform.FindChild ("Top/MapName_Function/Back").GetComponent<TweenAlpha> ();
		_mapRight = transform.FindChild ("Top/MapName_Function/Back_right").GetComponent<TweenAlpha> ();
		_mapBLeft = transform.FindChild ("Top/MapName_Function/b_Back_Left").GetComponent<TweenAlpha> ();
		_mapBRight = transform.FindChild ("Top/MapName_Function/b_Back_Right").GetComponent<TweenAlpha> ();
		_mapBack = transform.FindChild ("Top/MapName_Function/Back_Sp").GetComponent<TweenAlpha> ();
		_mapTw.enabled = false;
		_mapLeft.enabled = false;
		_mapRight.enabled = false;
		_mapBLeft.enabled = false;
		_mapBRight.enabled = false;
		_mapBack.enabled = false;
		_mapObj = transform.FindChild ("Top/MapName_Function").gameObject;
		//哥布林
		_goblinObj = transform.FindChild ("Top/Goblin_Function").gameObject;
		_goblinObj.SetActive (false);
		_goblinNum = transform.FindChild ("Top/Goblin_Function/lbl_GoldNum").GetComponent<UILabel> ();

		//多人副本
		_btnExit = transform.FindChild ("Top_Left/Btn_Exit").gameObject;
		_itemFunction = transform.FindChild ("Top_Left/Item_Function").gameObject;
		_itemPos = new Vector3 (0, -80, 0);

		_player1 = transform.FindChild ("Top_Left/Group_Function/Teammate1").GetComponent<FightDisplayPlayer> ();
		_player2 = transform.FindChild ("Top_Left/Group_Function/Teammate2").GetComponent<FightDisplayPlayer> ();

		_chatObj = transform.FindChild ("Top_Right/Chat_Function").gameObject;
		
		
		#region 世界boss
		this._buff = this._trans.FindChild ("bottom/Buff_Function");
		this._left = this._trans.FindChild ("Left");
		this._bossHealth = this._trans.FindChild ("Top/World_Boss_Function");
		this._bossIcon = this._bossHealth.FindChild ("icon").GetComponent<UITexture> ();
		this._bosslevel = this._bossHealth.FindChild ("Level").GetComponent<UILabel> ();
		this._bossName = this._bossHealth.FindChild ("Name").GetComponent<UILabel> ();
		
		this._me = this._trans.FindChild ("Left/ladder/me");
		this._temp = this._trans.FindChild ("Left/ladder/temp");
		this._bossHp = this._trans.FindChild ("Top/World_Boss_Function/Boss Bar").GetComponent<HealthBar> ();
		this._bossDamageTp = this._left.GetComponent<TweenPosition> ();
		var leftPos = this._left.localPosition;
		this._bossDamageTp.from = new Vector3 (leftPos.x, leftPos.y, leftPos.z);
		this._bossDamageTp.to = new Vector3 (leftPos.x + _subX, leftPos.y, leftPos.z);
		this._arrow = this._left.FindChild ("boss_damage_btn/arrow");
		
		#endregion

		_combo = transform.FindChild ("Top_Right/Right/combo").GetComponent<UICombo> ();

		//竞技场他人头像
		_arenaHeadFunc = transform.FindChild ("Top_Right/Hand_Function");
		_arenaHead = _arenaHeadFunc.FindChild ("Icon_Hand").GetComponent<UISprite> ();
		_arenaLvl = _arenaHeadFunc.FindChild ("Lbl_Lvl").GetComponent<UILabel> ();
		_arenaName = _arenaHeadFunc.FindChild ("Lbl_Name").GetComponent<UILabel> ();
		_arenaHp = _arenaHeadFunc.FindChild ("Healt Bar").GetComponent<HealthBar> ();
		_arenaMp = _arenaHeadFunc.FindChild ("Magic Bar").GetComponent<HealthBar> ();
		_arenaHeadFunc.gameObject.SetActive (false);

		_channel = transform.F ("Top_Left/Channel_Function");
		_hide = transform.F ("Top_Left/Hide_Function");
		_channelLbl = transform.F<UILabel> ("Top_Left/Channel_Function/Label");
		_hideLbl = transform.F<UISprite> ("Top_Left/Hide_Function/Sprite");
		_channel.SetActive (false);
		_hide.SetActive (false);


		#region 数值
		_lblDiamond = this._trans.Find ("Top_Left/data/diamond/lbl_diamond").GetComponent<UILabel> ();
		_lblGold = this._trans.Find ("Top_Left/data/gold/lbl_gold").GetComponent<UILabel> ();
		_lblShuiJing = this._trans.Find ("Top_Left/data/shuijing/lbl_shuijing").GetComponent<UILabel> ();
		#endregion
	}

	private void Start ()
	{
		Gate.instance.sendNotification (MsgConstant.MSG_FIGHT_INITI_ENTER);
		AwardManager.Instance.EnterPoint ();
		DisplayHandIcon ();  //显示头像
		DisplaySkills ();
		DisplayMapName ();
		DisplayAttackIcon ();
		DisplayHealt_Magic ();
		DisplayTalkMsg ();
		//进入副本，更新下血瓶信息

		Gate.instance.sendNotification (MsgConstant.MSG_FIGHT_EXP_CHANGE);
		DisplayExpBar ();

		if (Global.inMultiFightMap ()) {   //如果是多人副本，就显示这个组件
			_timeObj.SetActive (true);
			_btnExit.SetActive (false);
			_itemFunction.transform.localPosition = _itemPos;
			DisplayOtherPlayer ();
			DisplayChangeOtherPlayer_HealtBar ();
		} else if (Global.inFightMap ()) {
			_btnExit.SetActive (true);
			_itemFunction.transform.localPosition = Vector3.zero;
			_player1.gameObject.SetActive (false);
			_player2.gameObject.SetActive (false);
		} else if (Global.InArena ()) {
			_arenaHeadFunc.gameObject.SetActive (true);
			DisplayArenaPlayInfo ();
			_chatObj.SetActive (false);
			_timeObj.SetActive (false);
			_mapObj.SetActive (false);               
			_useItem.gameObject.SetActive (false);
			_skillObj.SetActive (false);
			_btnExit.SetActive (false);
			_player1.gameObject.SetActive (false);
			_player2.gameObject.SetActive (false);
		} else if (Global.inTowerMap ()) {
			_timeObj.SetActive (true);
		} else if (Global.InWorldBossMap ()) {
			DisplayChange ();
			DisplayHide ();
			this._itemFunObj.SetActive (false);
			this._itemFunction.SetActive (false);
			this._me.transform.FindChild ("Label").GetComponent<UILabel> ().text = LanguageManager.GetText ("msg_world_boss_me_damage");
			this._buff.gameObject.SetActive (true);
			var normalBuff = BossManager.Instance.NormalBuffData;
			var diamondBuff = BossManager.Instance.DiamondBuffData;
			this._buff.FindChild ("Skill1/assetsp").GetComponent<UISprite> ().spriteName = SourceManager.Instance.getIconByType ((eGoldType)normalBuff.type1List [0]);
			this._buff.FindChild ("Skill1/assetlbl").GetComponent<UILabel> ().text = normalBuff.type1List [1].ToString ();
			this._buff.FindChild ("Skill2/assetsp").GetComponent<UISprite> ().spriteName = SourceManager.Instance.getIconByType ((eGoldType)diamondBuff.type1List [0]);
			this._buff.FindChild ("Skill2/assetlbl").GetComponent<UILabel> ().text = diamondBuff.type1List [1].ToString ();
			this.UpdateBuffInfo (BossManager.Instance.BuffValue);
		} else if (Global.InAwardMap ()) {
			_btnExit.SetActive (true);
			_itemFunction.transform.localPosition = Vector3.zero;
		}
		
		UpdateShowOrHideUi ();
	}

	private void OtherPlayerHealtBar ()
	{
		Gate.instance.sendNotification (MsgConstant.MSG_FIGHT_MULTI_PLAYER_HEALT_CHANGE);
		Gate.instance.sendNotification (MsgConstant.MSG_FIGHT_DISPLAY_ARENA_PLAYER_HEALTH);
	}

	private void OnEnable ()
	{
		FightManager.Instance.RegsiterEvent ();
		EventDispatcher.GetInstance ().PlayerProperty += OtherPlayerHealtBar;    
		Gate.instance.registerMediator (new FightMediator (this));
	}

	private void OnDisable ()
	{
		FightManager.Instance.RemoveEvent ();
		//暂时这样,不应该用回调
		EventDispatcher.GetInstance ().PlayerProperty -= OtherPlayerHealtBar; 
		Gate.instance.removeMediator (MediatorName.FIGHT_MEDIATOR);
	}

	private void Update ()
	{
		UpdateTime ();
       
	}

	//显示频道线路
	public void DisplayChange ()
	{
		if (Global.InWorldBossMap ()) {
			_channel.SetActive (true);
			_channelLbl.text = ViewHelper.FormatLanguage ("channel_line", ChannelManager.Instance.CurLine.Id);
		}
     
	}

	//显示隐藏其他玩家功能按钮
	public void DisplayHide ()
	{
		if (Global.InWorldBossMap ()) {
			_hide.SetActive (true);
			if (SettingManager.Instance.Hide_Display) {
				_hideLbl.spriteName = ViewHelper.FormatLanguage ("setting_display");
			} else {
				_hideLbl.spriteName = ViewHelper.FormatLanguage ("setting_hide");
			}
            
		}
	}

	public void DisplayCombo ()
	{
		_combo.Play ();
	}


	public void DisplayArenaPlayInfo ()
	{ 
		_arenaHead.spriteName = ViewHelper.GetHandIcon (BattleArena.GetInstance ().m_kChallenger.character_other_property.career);
		_arenaName.text = BattleArena.GetInstance ().m_kChallenger.character_other_property.getNickName ();
		_arenaLvl.text = BattleArena.GetInstance ().m_kChallenger.character_other_property.getLevel ().ToString ();
	}

	public void DisplayArenaHealthMagic ()
	{
		if (BattleArena.GetInstance ().m_kChallenger != null) {
			_arenaHp.MaxValue = BattleArena.GetInstance ().m_kChallenger.GetProperty ().getHPMax ();
			_arenaHp.Value = BattleArena.GetInstance ().m_kChallenger.GetProperty ().getHP ();

			_arenaMp.MaxValue = BattleArena.GetInstance ().m_kChallenger.GetProperty ().fightProperty.fightData [eFighintPropertyCate.eFPC_MaxMP];
			_arenaMp.Value = BattleArena.GetInstance ().m_kChallenger.GetProperty ().GetMP ();
		}
	}
    

	/// <summary>
	/// 更新聊天消息
	/// </summary>
	public void DisplayTalkMsg ()
	{
		BetterList<TalkVo> talks = TalkManager.Instance.Contents;
		if (talks.size > 0) {
			_talkLbl.text = TalkManager.FormatShotMsg (talks [talks.size - 1]);
		} else {
			_talkLbl.text = "";
		}
	}

	/// <summary>
	/// 刷新显示头像信息
	/// </summary>
	public void DisplayHandIcon ()
	{

		handIcon.spriteName = helper.ViewHelper.GetHandIcon (CharacterPlayer.character_property.career);
		lbl_Level.text = CharacterPlayer.character_property.getLevel ().ToString ();
		lbl_Name.text = CharacterPlayer.character_property.getNickName ();
	}

	/// <summary>
	/// 显示人物等级
	/// </summary>
	public void DisplayLevel ()
	{
		lbl_Level.text = CharacterPlayer.character_property.getLevel ().ToString ();
	}

	/// <summary>
	/// 刷新生命魔法
	/// </summary>
	public void DisplayHealt_Magic ()
	{
		_HpBar.MaxValue = CharacterPlayer.character_property.fightProperty.GetValue (eFighintPropertyCate.eFPC_MaxHP);
		_HpBar.Value = CharacterPlayer.character_property.getHP ();

		_MpBar.MaxValue = CharacterPlayer.character_property.fightProperty.GetValue (eFighintPropertyCate.eFPC_MaxMP);
		_MpBar.Value = CharacterPlayer.character_property.GetMP ();
	}

	//刷新技能界面
	public void DisplaySkills ()
	{

		BetterList<SkillVo> skillData = SkillTalentManager.Instance.ActiveSkills;
		SkillVo sv;
		//if (skillData.size == 0) {
		//    Debug.LogError ("no skill data");
		//    return;
		//}

		for (int i = 0; i < _skills.Length; i++) {
			if (i < skillData.size) {
				sv = skillData [i];
				_skills [i].DisplaySkill (sv.XmlID, sv.Cool_Down, (CHARACTER_CAREER)sv.WeaponType, sv.Icon);
			} else {
				_skills [i].DisplaySkillIsLock ();
			}

        }

        DisplayPetSkill();
    }
    public void DisplayPetSkill()
    {
        PetVo vo = PetManager.Instance.CurrentPet;
        if (vo == null)
        {
            _skillPet.gameObject.SetActive(false);
        }
        else
        {
            _skillPet.gameObject.SetActive(true);
            SkillVo sv = vo.PetSkillVO;
            _skillPet.DisplaySkill(sv.XmlID, sv.Cool_Down, (CHARACTER_CAREER)sv.WeaponType, sv.Icon);
        }
    }


	//显示普通攻击图标
	public void DisplayAttackIcon ()
	{
		SkillVo vo = SkillTalentManager.Instance.GetCareerAttackSkill ();
		if (vo != null) {
			string icon = SkillTalentManager.Instance.GetCareerAttackSkill ().Icon;
			_attackIcon.mainTexture = SourceManager.Instance.getTextByIconName (icon, PathConst.SKILL_PATH);
		}
	}

    
	/// <summary>
	/// //使用药瓶
	/// </summary>
	/// <param name="parame">
	/// -1初始化 
	/// 0使用血瓶
	/// >0使用购买的价格
	/// </param>
	public void DisplayUseItem (int parame)
	{
		int count = FightManager.Instance.ItemData.CurHpMpItemCount;
		if (count > 0) {
			_useItem.DisplayItemChange (count);
		} else if (parame == -1) {
			_useItem.Initial (count);
		} else {
			_useItem.DisplayItemChange (parame, true);
		}

	}

	public void DisplayExpBar ()
	{
		_expBar.Value = FightManager.Instance.ItemData.CurExp;
		_expBar.MaxValue = FightManager.Instance.ItemData.NextExp;
	}

	public void ShowBossBar ()
	{
		if (!Global.InWorldBossMap ()) {
			_bossObj.SetActive (true);
			_bossBar = _bossObj.transform.FindChild ("Boss Bar").GetComponent<HealthBar> ();
		}
	}

	public void HiddenBossBar ()
	{
		_bossObj.SetActive (false);
	}

	public void DisplayBossBar ()
	{
		if (_bossBar != null) {
			if (FightManager.Instance.ItemData.Boss != null) {
				_bossBar.Value = FightManager.Instance.ItemData.Boss.getHP ();
				_bossBar.MaxValue = FightManager.Instance.ItemData.Boss.getHPMax ();
			}

		}
        
	}

	public void DisplayTip (string tips)
	{
		transform.FindChild ("Top/Tip_Function").gameObject.SetActive (true);
		UILabel lbl = transform.FindChild ("Top/Tip_Function/lbl_BossSay").GetComponent<UILabel> ();
		lbl.text = tips;

		TweenAlpha twlbl = lbl.gameObject.GetComponent<TweenAlpha> ();
		twlbl.callWhenFinished = "TweenTipComplate";

		TweenAlpha tw = transform.FindChild ("Top/Tip_Function/BossSaySp").GetComponent<TweenAlpha> ();
		TweenAlpha tt = transform.FindChild ("Top/Tip_Function/Title").GetComponent<TweenAlpha> ();
		twlbl.enabled = true;
		twlbl.ResetToBeginning ();
		tw.enabled = true;
		tw.ResetToBeginning ();
		tt.enabled = true;
		tt.ResetToBeginning ();
	}

	public void TweenTipComplate ()
	{
		transform.FindChild ("Top/Tip_Function").gameObject.SetActive (false);
	}

	// 显示奖励物品
	public void DisplayAwardItem ()
	{
		ItemStruct item = null;
		ItemTemplate it;
		item = AwardManager.Instance.GetIndexAwardItem (1);
		bool isShowItem = false;
		if (item == null) {
			_indexItem.IsDisplay (false);
			_secItem.IsDisplay (false);
		} else { 
			it = AwardManager.Instance.GetTemplateByTempId (item.tempId);
			_indexItem.Display (it.icon, ViewHelper.GetBoderById ((int)item.tempId), (int)item.num, it.name);
			isShowItem = true;
			item = AwardManager.Instance.GetIndexAwardItem (2);
			if (item == null) {
				_secItem.IsDisplay (false);
			} else {
				it = AwardManager.Instance.GetTemplateByTempId (item.tempId);
				_secItem.Display (it.icon, ViewHelper.GetBoderById ((int)item.tempId), (int)item.num, it.name);
			}
		}
		if (isShowItem) {
			StartCoroutine (DeleteAwardItem ());
		}
	}

	/// <summary>
	/// 这里使用了协同来计时，会出现数据不同步问题,需要同步数据
	/// </summary>
	public void DisplayAwardItemSec ()
	{
		ItemStruct item = null;
		ItemTemplate it;
		item = AwardManager.Instance.GetIndexAwardItem (2);
		if (item == null) {
			_secItem.IsDisplay (false);
		} else {
			it = AwardManager.Instance.GetTemplateByTempId (item.tempId);
			_secItem.Display (it.icon, "common_lanse_wu", (int)item.num, it.name);
		}
	}

	public IEnumerator DeleteAwardItem ()
	{
		yield return new WaitForSeconds (MAX_AWARD_SHOW_TIME);
		Gate.instance.sendNotification (MsgConstant.MSG_FIGHT_DELETE_AWARD_ITEM);
	}




	//显示其他玩家
	public void DisplayOtherPlayer ()
	{
        
		_player1.gameObject.SetActive (true);
		_player2.gameObject.SetActive (true);
		List<CharacterPlayerOther> other = PlayerManager.Instance.player_other_list;
		if (other.Count != 0) {
			_player1.Display (helper.ViewHelper.GetHandIcon (other [0].character_other_property.career), other [0].character_other_property.getNickName (), other [0].character_other_property.getLevel ().ToString ());
			if (other.Count > 1) {
				_player2.Display (helper.ViewHelper.GetHandIcon (other [1].character_other_property.career), other [1].character_other_property.getNickName (), other [1].character_other_property.getLevel ().ToString ());
			} else {
				_player2.gameObject.SetActive (false);
			}
		} else {
			_player1.gameObject.SetActive (false);
			_player2.gameObject.SetActive (false);
		}
		DisplayChangeOtherPlayer_HealtBar ();
	}

	//更改血量
	public void DisplayChangeOtherPlayer_HealtBar ()
	{
		if (Global.inMultiFightMap ()) {
			List<CharacterPlayerOther> other = PlayerManager.Instance.player_other_list;
			if (other.Count != 0) {
				if (other [0].character_other_property.getLevel () == 0) {
					DisplayOtherPlayer ();
				}
				_player1.SetHealt (other [0].character_other_property.getHP (), other [0].character_other_property.getHPMax ());
				if (other.Count > 1) {
					if (other [1].character_other_property.getLevel () == 0) {
						DisplayOtherPlayer ();
					}
					_player2.SetHealt (other [1].character_other_property.getHP (), other [1].character_other_property.getHPMax ());
				} else {
				}
			} else {

			}
		}
	}

    
	private bool isStopTime = false;//停止倒计时
	private void UpdateTime ()
	{ 
		float oldTime = 0;
		//更新时间
		if (!isStopTime) {
			if (FightManager.Instance.ItemData.IsReTime) {//更新倒计时
				FightManager.Instance.ItemData.ReTime -= Time.deltaTime;
				FightManager.Instance.ItemData.ReTime = Mathf.Max (FightManager.Instance.ItemData.ReTime, 0);
				if (FightManager.Instance.ItemData.ReTime == 0) {
					DisplayTime (FightManager.Instance.ItemData.ReTime);
					StopTime ();
					return;
				} else {
					if ((int)oldTime != (int)FightManager.Instance.ItemData.ReTime) {
						oldTime = FightManager.Instance.ItemData.ReTime;
						DisplayTime (oldTime);
					}
				}
			} else {  //更新正计时
				FightManager.Instance.ItemData.Time += Time.deltaTime;
				if ((int)oldTime != (int)FightManager.Instance.ItemData.Time) {
					oldTime = FightManager.Instance.ItemData.Time;
					DisplayTime (oldTime);
				}
			}
		}
	}

	private void DisplayTime (float Time)
	{
		string coolTime = string.Format ("{0:00}:{1:00}", (int)Time / 60, (int)Time % 60);
		_lblTime.text = coolTime;
	}

	public void StartTime ()
	{
		if (Global.InArena ()) {
			_timeObj.SetActive (true);
		}
		isStopTime = false;
		FightManager.Instance.ItemData.Time = 0;
		if (FightManager.Instance.ItemData.IsReTime)
			_spTime.spriteName = Constant.Fight_ReTime_LblName;
		else
			_spTime.spriteName = Constant.Fight_Time_LblName;
		_spTime.MakePixelPerfect (); 
	}

	public void StopTime ()
	{
		isStopTime = true;
		#region 关闭其他界面
		if (!Global.InArena ()) {
			UIManager.Instance.closeAllUI ();
			NPCManager.Instance.createCamera (false); //消除3D相机
		}
		#endregion
		
		if (Global.inTowerMap ()) {
			//计时结束退出副本
			MessageManager.Instance.sendMessageReturnCity ();
			//						ReturnToCity.Instance.ReturnType = ReturnToCity.RETURN_TYPE.TIME_OUT; //恶魔洞窟全部挑战完毕
		} else if (Global.inGoldenGoblin ()) {
			// 打开哥布林收益面板
			UIManager.Instance.openWindow (UiNameConst.ui_golden_gain);
		} else if (Global.inFightMap ()) {
			UIManager.Instance.openWindow (UiNameConst.ui_award);
		} else if (Global.inMultiFightMap ()) {

			UIManager.Instance.openWindow (UiNameConst.ui_award);
		} else if (Global.InArena ()) {
			_timeObj.SetActive (false);
		} else if (Global.InAwardMap ()) {
			UIManager.Instance.openWindow (UiNameConst.ui_award);
		}
		FightManager.Instance.ItemData.Time = 0;
		FightManager.Instance.ItemData.ReTime = 0;
		FightManager.Instance.ItemData.IsReTime = true;
	}


	/// <summary>
	/// 显示地图名称
	/// </summary>
	public void DisplayMapName ()
	{
		if (Global.inTowerMap ()) {
			string langStr = LanguageManager.GetText ("devil_current_wave")
				+ LanguageManager.GetText ("devil_wave_prefix") + LanguageManager.GetText ("devil_wave_color") + 
				DemonManager.Instance.GetDemonVoById (Global.cur_TowerId).Level.ToString ()
				+ Constant.COLOR_END + LanguageManager.GetText ("devil_wave_suffix");
			if (_mapName) {
				_mapName.text = langStr;
			}
			//当前波数
//            UiFightMainMgr.Instance.lblCurrentWave.text = langStr;
			TweenMapName ();
		} else if (Global.inGoldenGoblin ()) {
			string langStr = LanguageManager.GetText ("golden_goblin_begin_gain_gold");
			if (_mapName) {
				_mapName.text = langStr;
			}
			TweenMapName ();
		} else if (Global.InArena ()) {


		} else if (Global.inMultiFightMap ()) {
			if (_mapName) {
				int id = CharacterPlayer.character_property.getServerMapID ();
				_mapName.text = ConfigDataManager.GetInstance ().getMapConfig ().getMapData (id).name;
			}
			TweenMapName ();
		} else if (Global.InWorldBossMap ()) {
			if (_mapName) {
				int id = CharacterPlayer.character_property.getServerMapID ();
				_mapName.text = ConfigDataManager.GetInstance ().getMapConfig ().getMapData (id).name;
			}
			TweenMapName ();
		} else if (Global.InAwardMap ()) {
			if (_mapName) {
				int id = CharacterPlayer.character_property.getServerMapID ();
				_mapName.text = ConfigDataManager.GetInstance ().getMapConfig ().getMapData (id).name;
			}
			TweenMapName ();
		} else {
			if (_mapName) {
				_mapName.text = Global.lastFightMap.name;
			}
			ScenarioManager.Instance.showScenario (TaskManager.Instance.MainTask, TweenMapName, eTriggerType.startGate);

		}
	}
	/// <summary>
	/// 显示地图名称
	/// </summary>
	/// <param name="msg"></param>
	public void DisplayMapName (string msg)
	{
		if (_mapName) {
			_mapName.text = msg;
			TweenMapName ();
		}
	}


	/// <summary>
	/// 渐变地图名称
	/// </summary>
	public void TweenMapName ()
	{
		_mapTw.enabled = true;
		_mapTw.ResetToBeginning ();
		_mapLeft.enabled = true;
		_mapLeft.ResetToBeginning ();
		_mapRight.enabled = true;
		_mapRight.ResetToBeginning ();
		_mapBLeft.enabled = true;
		_mapBLeft.ResetToBeginning ();
		_mapBRight.enabled = true;
		_mapBRight.ResetToBeginning ();
		_mapBack.enabled = true;
		_mapBack.ResetToBeginning ();
	}

	/// <summary>
	/// 显示哥布林组件
	/// </summary>
	public void DisplayGoblin_Fucntion ()
	{
		_goblinObj.SetActive (true);
		_goblinNum.text = "0";
	}
	/// <summary>
	/// 显示哥布林已经打到的金币
	/// </summary>
	/// <param name="gold"></param>
	public void DisplayGoblin_Gold (int gold)
	{
		_goblinNum.text = gold.ToString ();
	}
	
	/// <summary>
	/// 更新排名，更新BOSS血量
	/// </summary>
	/// <param name='bossDVo'>
	/// Boss D vo.
	/// </param>
	public void UpdateWorldBossInfo (BossDamageVo bossDVo)
	{
		InitWorldBossInfo ();
		this.StartCoroutine (this.UpdateWorldBossDamageLadder (bossDVo));
		UpdateWorldBossHealth (bossDVo);
		
	}

	public IEnumerator UpdateWorldBossDamageLadder (BossDamageVo bossDVo)
	{
		const int height = -25;
		
		this._me.transform.FindChild ("dps").GetComponent<UILabel> ().text = BossManager.Instance.GetLerpValueString (BossManager.Instance.GetLerpValue (0, BossManager.Instance.WorldBossData.hp
																																, (int)bossDVo.MeDamage), bossDVo.MeDamage.ToString ());
		Transform list = this._temp.parent.FindChild ("playelist");
		if (list != null) {
			Destroy (list.gameObject);
		}
		
		GameObject playList = new GameObject ();
		playList.name = "playelist";
		playList.transform.parent = this._temp.parent;
		playList.transform.localPosition = Vector3.one;
		playList.transform.localScale = Vector3.one;
		
		for (int i = 0,max = bossDVo.PlayerDamageList.Count; i < max; i++) {
			int ladderNum = i + 1;
			GameObject obj = NGUITools.AddChild (playList, this._temp.gameObject);
			obj.name = ladderNum.ToString ();
			Transform objTrans = obj.transform;
			objTrans.localPosition = new Vector3 (this._temp.localPosition.x, this._temp.localPosition.y + i * height, this._temp.localPosition.z);
			if (ladderNum <= 3) {
				objTrans.FindChild ("name").GetComponent<UILabel> ().text = string.Format (LanguageManager.GetText ("msg_world_boss_num" + ladderNum), ladderNum, bossDVo.PlayerDamageList [i].Name);
			} else {
				objTrans.FindChild ("name").GetComponent<UILabel> ().text = string.Format (LanguageManager.GetText ("msg_world_boss_num"), ladderNum, bossDVo.PlayerDamageList [i].Name);
			}
			objTrans.FindChild ("dps").GetComponent<UILabel> ().text = BossManager.Instance.GetLerpValueString (BossManager.Instance.GetLerpValue (0, BossManager.Instance.WorldBossData.hp
																						, (int)bossDVo.PlayerDamageList [i].TotalDamage), bossDVo.PlayerDamageList [i].TotalDamage.ToString ());
			
		}
		 
		yield return new WaitForSeconds (0.1f);
		BossManager.Instance.IsUpdateBossDamageUI = false;
	}

	public  void InitWorldBossInfo ()
	{
		if (this._left.gameObject.activeSelf == false) {
			this._left.gameObject.SetActive (true);
		}
		if (this._bossHealth.gameObject.activeSelf == false) {
			
			this._bossIcon.mainTexture = SourceManager.Instance.getTextByIconName (BossManager.Instance.WorldBossData.pszDisplayIcon, PathConst.OTHER_PATH);	
//			this._bossIcon.MakePixelPerfect ();
			this._bossName.text = BossManager.Instance.WorldBossData.desName;
			this._bosslevel.text = "Lv." + BossManager.Instance.WorldBossData.level;
			this._bossHealth.gameObject.SetActive (true);
		}
	}

	public void UpdateWorldBossHealth (BossDamageVo bossDVo)
	{
		this._bossHp.MaxValue = BossManager.Instance.WorldBossData.hp;
		this._bossHp.Value = (int)bossDVo.BossRemainHp;
	}
	
	public void SwitchBossBtn ()
	{
		this._bossDamageTp.Play (this._isOpenBossDamageBtn);
		var lolScale = this._arrow.localScale;
		lolScale.x *= -1;
		this._arrow.localScale = lolScale;
		_isOpenBossDamageBtn = !_isOpenBossDamageBtn;
	}
	
	public void UpdateBuffInfo (int val)
	{
		if (val != 0) {
			this._buff.FindChild ("info").GetComponent<UILabel> ().text = string.Format (LanguageManager.GetText ("msg_world_boss_buff_info"), val);
		}
	
		
	}
	
	/// <summary>
	/// 更新功能块的显示或者隐藏
	/// </summary>
	public void UpdateShowOrHideUi ()
	{
		this._bottomCount = 0;
		FastOpenVo func;
		for (int i = 0, max = FastOpenManager.Instance.FastOpenList.Count; i < max; i++) {
			func = FastOpenManager.Instance.FastOpenList [i];
			if (!string.IsNullOrEmpty (func.UIUrl)) {
				Transform funTrans = this._trans.FindChild (func.UIUrl);
				if (funTrans == null && func.Location != LocationType.RightTop) { //在战斗界面，右上角需要特殊处理
					continue;
				}
				if (!FastOpenManager.Instance.DirOpenTypeValue.ContainsKey (func.Type)) {
					if (funTrans != null) {
						funTrans.gameObject.SetActive (false);
					}
					continue;
				} 
//				print(FastOpenManager.Instance.DirOpenTypeValue [func.Type]);
				if (func.Param > FastOpenManager.Instance.DirOpenTypeValue [func.Type]) {
					if (funTrans != null) {
						funTrans.gameObject.SetActive (false);
					}
				} else {
					switch (func.Location) {
					case LocationType.RightTop:
						FastOpenManager.Instance.AddFunctionEffect (func);
						break;
					case LocationType.Bottom:
						funTrans.gameObject.SetActive (true);
						Vector3 bottomStepVal = this._bottomCount * this._bottomStepValue;
						this._trans.FindChild (func.UIUrl).localPosition = new Vector3 (this._bottomBaseLocation.x + bottomStepVal.x, this._bottomBaseLocation.y + bottomStepVal.y, this._bottomBaseLocation.z + bottomStepVal.z);
						this._bottomCount++;
						FastOpenManager.Instance.AddFunctionEffect (func);
						break; 
					default:
						break;
					}
				}
			} else { //无路径,内部的处理
				if (!FastOpenManager.Instance.DirOpenTypeValue.ContainsKey (func.Type)) {
					continue;
				} 
				if (func.Param <= FastOpenManager.Instance.DirOpenTypeValue [func.Type]) {
					switch (func.Location) {
					case LocationType.RightTop:
						FastOpenManager.Instance.AddFunctionEffect (func);
						break;
					case LocationType.Bottom: 
						FastOpenManager.Instance.AddFunctionEffect (func);
						break;
					default:
						break;
					}
				}
			}
		}
		FastOpenManager.Instance.OpenNewFunctionWindow ();//开始效果
	}


	#region 资源方法
	//更新资源
	public 	void UpdatePlayerAsset (int diamond, int gold, int crystal)
	{
		_lblDiamond.text = diamond >= 10000 ? diamond / 10000 + "万" : diamond.ToString ();
		_lblGold.text = gold >= 10000 ? gold / 10000 + "万" : gold.ToString ();
		_lblShuiJing.text = crystal >= 10000 ? crystal / 10000 + "万" : crystal.ToString ();
	}
	
	//更新游戏金钱
	public void UpdatCurrencyChange (int changeNum)
	{
		if (_lblGold != null && _lblGold.active) {
			_currencyShakeTime = 0f;
			if (changeNum < 0) { //消耗游戏币
				_goldColor = Color.red;
			} else {
				_goldColor = Color.green;
			}
			_lblGold.color = _goldColor;
			StartCoroutine (shakeLabel (_lblGold, false));
		}
	}
	
	//更新钻石
	public void UpdatMoneyChange (int changeNum)
	{
		if (_lblDiamond != null && _lblDiamond.active) {
			_moneyShakeTime = 0f;
			if (changeNum < 0) { //消耗钻石
				_diamondcolor = Color.red;
			} else {
				_diamondcolor = Color.green;
			}
			_lblDiamond.color = _diamondcolor;
			StartCoroutine (shakeLabel (_lblDiamond, true));
		}
	}
	
	//动画效果
	IEnumerator shakeLabel (UILabel label, bool isMoney)
	{
		float calTime;
		if (isMoney)
			calTime = _moneyShakeTime;
		else
			calTime = _currencyShakeTime;
		while (calTime < Constant.BLINK_TIME) {
			yield return new WaitForSeconds (Constant.BLINK_INTEVAL);
			if (isMoney) {
				if (label.color == Color.white) {
					label.color = _diamondcolor;
				} else {
					label.color = Color.white;
				}
			} else {
				if (label.color == Color.white) {
					label.color = _goldColor;
				} else {
					label.color = Color.white;
				}
			}		
			calTime += Constant.BLINK_INTEVAL;
			if (calTime >= Constant.BLINK_TIME) {
				label.color = Color.white; //还原label的颜色
			}
		} 
	}
	#endregion

}
