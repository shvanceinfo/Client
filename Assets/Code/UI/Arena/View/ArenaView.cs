/**该文件实现的基本功能等
function: 竞技场的UI视图控制
author:ljx
date:2013-11-09
**/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;
using manager;
using mediator;
using MVC.entrance.gate;
using helper;

public class ArenaView : MonoBehaviour
{
	Transform _trans;
	private Transform _leftPlayerInfo; //左边个人区域
	private Transform _topAwardInfo; //奖励显示区域
	private Transform _resultInfo; //战报信息区域
	private Transform _challengerList; //挑战玩家区域
	private Transform _honorInfo; //荣誉信息区域
	private TimeSpan _awardSpan; //凌晨

	private GameObject _awardPanel;
	private UIGrid _awardGrid;
	private GameObject _awardPre;
	private GameObject _btnAward;
	private GameObject _btnAwardDisable;
	private UILabel _resultMsg;
	private UILabel _time;
	private UILabel _timePrefix;
	GameObject _successItem;
	

	void Awake ()
	{
		this._trans = this.transform;
		_awardPanel = transform.Find ("middle/ResultPanel").gameObject;
		_awardGrid = transform.Find ("middle/ResultPanel/panel/grid").GetComponent<UIGrid> ();
		_awardPre = transform.Find ("middle/ResultPanel/panel/items").gameObject;
		_btnAward = transform.Find ("bottom/resultBtn").gameObject;
		_btnAwardDisable = transform.Find ("bottom/resultdisable").gameObject;
		_awardPre.SetActive (false);
		_awardPanel.SetActive (false);

		_leftPlayerInfo = transform.Find ("bottom/myInfo");
		_topAwardInfo = transform.Find ("middle/top");
		_resultInfo = transform.Find ("bottom/arenaMsg");
		_challengerList = transform.Find ("middle/center");
		_honorInfo = transform.Find ("middle/bottom");
		DateTime tomorrow = DateTime.Now.AddDays (1).Date;
//		tomorrow = new System.DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 0, 0, 0);
		_awardSpan = tomorrow.Subtract (DateTime.Now);
		_resultMsg = this._trans.FindChild ("bottom/resultBtn/msg").GetComponent<UILabel>();
		this._time = this._trans.FindChild ("bottom/time/time").GetComponent<UILabel> ();
		this._timePrefix = this._trans.FindChild ("bottom/time/timePrefix").GetComponent<UILabel> ();
		this._successItem = this._trans.FindChild ("bottom/time/successItem").gameObject;
		InvokeRepeating ("onTimer", 0f, 1f);
	}

	void Start(){
		_resultMsg.text = LanguageManager.GetText ("arena_award_msg");
		this._timePrefix.text = LanguageManager.GetText ("arena_timeSuffix");
	}

	void OnEnable ()
	{  
		Gate.instance.registerMediator (new ArenaMediator (this));
	}
	
	void OnDisable ()
	{
		Gate.instance.removeMediator (MediatorName.ARENA_MEDIATOR);
//		NPCManager.Instance.createCamera(false); //消除3D相机
	}

	public void SetAwardPanel (bool isActive)
	{
		_awardPanel.SetActive (isActive);
	}

	public void SetAwardBtn (bool isActive)
	{
		_btnAward.SetActive (isActive);
		_btnAwardDisable.SetActive (!isActive);
		this._successItem.SetActive (isActive);
		this._timePrefix.gameObject.SetActive (!isActive);
		this._time.gameObject.SetActive (!isActive);
	}

	public void DisplayAwardPanel (AwardMsg awardMsg)
	{
		SetAwardPanel (true);
		List<List<TypeStruct>> list = new List<List<TypeStruct>> ();
		awardMsg.award.Sort ();
		List<TypeStruct>.Enumerator it = awardMsg.award.GetEnumerator ();
		while (it.MoveNext()) {
			if (list.Count == 0 || list [list.Count - 1].Count >= 4) {
				list.Add (new List<TypeStruct> ());
			}
			list [list.Count - 1].Add (it.Current);
		}
		ViewHelper.FormatTemplate<List<List<TypeStruct>>, List<TypeStruct>>
            (_awardPre, _awardGrid.transform, list,
            (List<TypeStruct> vo, Transform t) =>
		{
			ItemLabel item;
			for (int i = 0; i < vo.Count; i++) {
				item = t.F<ItemLabel> (i.ToString ());
				if (vo [i].Type == ConsumeType.Gold) {
					item.Quality = eItemQuality.eOrange;
					item.Icon = SourceManager.Instance.getIocnStringByType ((eGoldType)vo [i].Id);
					item.Lable = vo [i].Value.ToString ();
				} else {
					item.Quality = ItemManager.GetInstance ().GetTemplateByTempId ((uint)vo [i].Id).quality;
					item.Icon = ItemManager.GetInstance ().GetTemplateByTempId ((uint)vo [i].Id).icon;
					item.Lable = vo [i].Value.ToString ();
				}
                     
			}
			if (vo.Count < 4) {
				for (int i = vo.Count; i < 4; i++) {
					item = t.F<ItemLabel> (i.ToString ());
					item.Quality = eItemQuality.eWhite;
					item.Icon = null;
					item.Lable = null;
				}
			}
		});
	}

	
	//设置奖励面板信息
	public void setAwardInfo (AwardMsg awardMsg)
	{
		if (awardMsg != null) {
			_topAwardInfo.Find ("rank").GetComponent<UILabel> ().text = awardMsg.name;
            
			string award = "";
			foreach (TypeStruct ts in awardMsg.award) {
				if (ts.Type == ConsumeType.Gold) {
					award += string.Format ("{0}{1}", ts.Value, ViewHelper.GetResoucesString ((eGoldType)ts.Id));
				}
			}
			_topAwardInfo.Find ("award").GetComponent<UILabel> ().text = award;
		}
	}

    public void RefreshHonor(string s)
    {
        _leftPlayerInfo.Find("myHonor/num").GetComponent<UILabel>().text = s;
    }

	//设置人物信息区域
	public void setArenaInfo ()
	{
		ArenaInfo info = ArenaManager.Instance.ArenaVo.ArenaInfo;
		_leftPlayerInfo.Find ("name").GetComponent<UILabel> ().text = CharacterPlayer.character_property.getNickName ();
		_leftPlayerInfo.Find ("continuosWin/num").GetComponent<UILabel> ().text = info.continuousWin.ToString ();
		_leftPlayerInfo.Find ("myHonor/num").GetComponent<UILabel> ().text = info.currentHonor.ToString ();
		_leftPlayerInfo.Find ("myRank/num").GetComponent<UILabel> ().text = info.currentRank.ToString ();
	 
		_leftPlayerInfo.Find ("honor/honorNum").GetComponent<UILabel> ().text = info.honorLevel.ToString (); 
//		int hashKey = (int)info.honorLevel + 1;
//		if (ArenaManager.Instance.NeedHonorHash.Contains (hashKey)) {
//			uint nextHonor = (ArenaManager.Instance.NeedHonorHash [hashKey] as HonorLevel).needHonor;		
//			uint needHonor = nextHonor - info.currentHonor;
//			if (nextHonor < info.currentHonor)
//				needHonor = nextHonor;
//			_leftPlayerInfo.Find ("nextLvl/honorNum").GetComponent<UILabel> ().text = needHonor.ToString ();	
//		}
		
		
	}
	
	//设置挑战信息区域
	public void setCheallengeInfo ()
	{
		BetterList<ChallengeSigle> singles = ArenaManager.Instance.ArenaVo.ChallengerList;
		int len = singles.size;
		if (len > 0) {
			int counter = 1;
			UISprite icon = null;
			foreach (ChallengeSigle single in singles) {
				string name = "challenger" + (counter);
				Transform trans = _challengerList.Find (name);
				trans.Find ("level/lblNum").GetComponent<UILabel> ().text = single.level.ToString ();
				trans.Find ("name").GetComponent<UILabel> ().text = single.roleName;
				trans.Find ("rank/num").GetComponent<UILabel> ().text = LanguageManager.GetText ("arena_peopleRank") + single.rank.ToString ();
				trans.Find ("rank/num1").GetComponent<UILabel> ().text = single.rank.ToString ();
				CHARACTER_CAREER career = (CHARACTER_CAREER)single.vocation;
//                setModel(counter, career, single.suitID, single.weaponID, single.wingID);


				icon = trans.Find ("player").GetComponent<UISprite> ();
				SetPeopleIcon (icon, career);

				trans.Find ("battle/Label").GetComponent<UILabel> ().text = single.power.ToString ();

				counter++;
			}
		}
		for (int i=len; i<5; i++) {
			string name = "challenger" + (i + 1);
			_challengerList.Find (name).gameObject.SetActive (false); //隐藏没有玩家的区域
		}
	}

	public void SetPeopleIcon (UISprite icon, CHARACTER_CAREER career)
	{

		switch (career) {
		case CHARACTER_CAREER.CC_SWORD:
			icon.spriteName = Constant.Fight_WarriorHandIcon;
			break;
		case  CHARACTER_CAREER.CC_ARCHER:
			icon.spriteName = Constant.Fight_ArcherHandIcon;
			break;
		case CHARACTER_CAREER.CC_MAGICIAN:
			icon.spriteName = Constant.Fight_MagicHandIcon;
			break;
		default:
			break;
		}
	}




	//设置荣誉信息区域
	public void setHonorAreaInfo ()
	{
		ArenaInfo info = ArenaManager.Instance.ArenaVo.ArenaInfo;
//		_leftPlayerInfo.Find("nextLvl/honorNum").GetComponent<UILabel>().text = info.honorLevel.ToString();
		_honorInfo.Find ("challenge/num").GetComponent<UILabel> ().text = info.remainChallengeNum.ToString ();
//		int hashKey = (int)info.honorLevel+1;
//		if(ArenaManager.Instance.NeedHonorHash.Contains(hashKey))
//		{
//			uint nextHonor = (ArenaManager.Instance.NeedHonorHash[hashKey] as HonorLevel).needHonor;		
//			uint needHonor = nextHonor - info.currentHonor;
//			if(nextHonor < info.currentHonor)
//				needHonor = nextHonor;
//			_leftPlayerInfo.Find("nextLvl/honorNum").GetComponent<UILabel>().text = needHonor.ToString();	
//		}
	}
	
	//设置战报信息
	public void setChallengeResult ()
	{
		BetterList<ResultInfo> results = ArenaManager.Instance.ArenaVo.ResultList;
		int len = results.size;
		GameObject resultSp = transform.Find ("bottom/msgSp").gameObject;
		if (len > 0) {
			resultSp.SetActive (true);
			int resultLen = len < 3 ? len : 3;
			int i = 1;
			for (i=resultLen; i>=1; i++) {
				ResultInfo result = results [i];
				_resultInfo.Find ("msg" + i).gameObject.SetActive (true);
				UILabel lbl = _resultInfo.Find ("msg" + i + "/msg").GetComponent<UILabel> ();
				string showText = "";
				if (result.beFight) { //被挑战
//					showText = result.fightTime + "分钟" +
				} else {
//					showText = result.fightTime + "分钟" +
				}
				lbl.text = showText;
			}
			for (int j=resultLen; j<=3; j++)
				_resultInfo.Find ("msg" + j).gameObject.SetActive (false);
		} else {
			resultSp.SetActive (false); //隐藏战报信息
			for (int i=1; i<=3; i++)
				_resultInfo.Find ("msg" + i).gameObject.SetActive (false);
		}
	}
	
	//设置挑战次数
	public void setChallengeNum (uint challengeNum)
	{
		_honorInfo.Find ("challenge/num").GetComponent<UILabel> ().text = challengeNum.ToString ();
		DateTime tomorrow = DateTime.Now.AddDays (1).Date;
		_awardSpan = tomorrow.Subtract (DateTime.Now);
		onTimer (); //重置倒计时时间以及CD时间
	}
	
	//设置挑战者的相关模型
	private void setModel (int pos, CHARACTER_CAREER vocation, uint suitID, uint weaponID, uint wingID)
	{
		string modelName = "";

		switch (vocation) 
		{
			case CHARACTER_CAREER.CC_SWORD:
				modelName = Constant.SWORD_UI;
				break;
			case CHARACTER_CAREER.CC_ARCHER:
				modelName = Constant.BOW_UI;
				break;
			case CHARACTER_CAREER.CC_MAGICIAN:
				modelName = Constant.RABBI_UI;
				break;
			default:
				break;
		}
        //人物模型都是放在武器特效中
        if (BundleMemManager.Instance.isTypeInCache(EBundleType.eBundleWeaponEffect))
        {
            BundleMemManager.Instance.loadPrefabViaWWW<GameObject>(EBundleType.eBundleWeaponEffect, modelName,
                (asset) =>
                {
                    GameObject model = BundleMemManager.Instance.instantiateObj(asset, Vector3.zero, Quaternion.identity);
                    model.AddComponent("CharacterPlayerOther");
                    CharacterPlayerOther playerOther = model.GetComponent<CharacterPlayerOther>();
                    if (weaponID > 0)
                        playerOther.equipItem((int)weaponID, vocation);
                    if (suitID > 0)
                        playerOther.equipItem((int)suitID, vocation);
                    if (wingID > 0)
                        playerOther.character_avatar.installWing(wingID);
                    ToolFunc.SetLayerRecursively(model, LayerMask.NameToLayer("TopUI"));
                    model.transform.parent = NPCManager.Instance.ModelCamera.transform;
                    ArenaPos arenaPos = ConfigDataManager.GetInstance().getArenaPos().getArenaInfo(pos);
                    NPCManager.Instance.ModelCamera.fieldOfView = arenaPos.view;
                    model.transform.localScale = arenaPos.scale;
                    playerOther.enabled = false; //取消控件才能旋转
                    model.transform.localEulerAngles = arenaPos.rotate;
                    model.transform.localPosition = arenaPos.pos;
                }, CharacterPlayer.character_property.career);
        }
        else
        {
            GameObject asset = BundleMemManager.Instance.getPrefabByName(modelName, EBundleType.eBundleWeaponEffect);
            GameObject model = BundleMemManager.Instance.instantiateObj(asset, Vector3.zero, Quaternion.identity);
            model.AddComponent("CharacterPlayerOther");
            CharacterPlayerOther playerOther = model.GetComponent<CharacterPlayerOther>();
            if (weaponID > 0)
                playerOther.equipItem((int)weaponID, vocation);
            if (suitID > 0)
                playerOther.equipItem((int)suitID, vocation);
            if (wingID > 0)
                playerOther.character_avatar.installWing(wingID);
            ToolFunc.SetLayerRecursively(model, LayerMask.NameToLayer("TopUI"));
            model.transform.parent = NPCManager.Instance.ModelCamera.transform;
            ArenaPos arenaPos = ConfigDataManager.GetInstance().getArenaPos().getArenaInfo(pos);
            NPCManager.Instance.ModelCamera.fieldOfView = arenaPos.view;
            model.transform.localScale = arenaPos.scale;
            playerOther.enabled = false; //取消控件才能旋转
            model.transform.localEulerAngles = arenaPos.rotate;
            model.transform.localPosition = arenaPos.pos;
        }				

//		switch (pos) 
//		{
//			case 1:
//				model.transform.localPosition = new Vector3(ArenaManager.posX1, ArenaManager.posY, ArenaManager.posZ);
//				break;
//			case 2:
//				model.transform.localPosition = new Vector3(ArenaManager.posX2, ArenaManager.posY, ArenaManager.posZ);
//				break;
//			case 3:
//				model.transform.localPosition = new Vector3(ArenaManager.posX3, ArenaManager.posY, ArenaManager.posZ);
//				break;
//			case 4:
//				model.transform.localPosition = new Vector3(ArenaManager.posX4, ArenaManager.posY, ArenaManager.posZ);
//				break;
//			case 5:
//				model.transform.localPosition = new Vector3(ArenaManager.posX5, ArenaManager.posY, ArenaManager.posZ);
//				break;
//			default:				
//				break;
//		}
	}
	
	//定时更新挑战时间跟cd时间
	private void onTimer ()
	{		
		TimeSpan coolSpan = ArenaManager.Instance.TimeSpan;
		string coolTime = string.Format ("{0:00}:{1:00}", coolSpan.Minutes, coolSpan.Seconds);
		string getAwardTime = string.Format ("{0:00}:{1:00}:{2:00}", ArenaManager.Instance.AwardSpan.Hours, ArenaManager.Instance.AwardSpan.Minutes, ArenaManager.Instance.AwardSpan.Seconds); //具体时间格式化

		this._time.text = "[ff0000]" + getAwardTime + "[-]";

		_honorInfo.Find ("cdInfo/coolTime").GetComponent<UILabel> ().text = coolTime;
		TimeSpan unitSpan = new TimeSpan (0, 0, 1);
		if (ArenaManager.Instance.TimeSpan.TotalSeconds > 0) { //没有倒计时到0
			ArenaManager.Instance.TimeSpan = coolSpan.Subtract (unitSpan); //倒计时减去1s
            
		}
		if (ArenaManager.Instance.AwardSpan.TotalSeconds > 0) {
			ArenaManager.Instance.AwardSpan = ArenaManager.Instance.AwardSpan.Subtract (unitSpan);
		}

		if (_awardSpan.TotalSeconds > 0) {
			_awardSpan = _awardSpan.Subtract (unitSpan);
		}
			
	}
}
