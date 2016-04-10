/**该文件实现的基本功能等
function: 实现魔物悬赏的功能
author:zyl
date:2014-06-04
**/
using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using mediator;
using manager;
using model;
using System;
using System.Text;
using System.Collections.Generic;

public class MonsterRewardView : MonoBehaviour
{
	private Vector3 btn3= new Vector3(-130,-250,0);
	private Vector3 btn2 = new Vector3(-196,-250,0);
	private Vector3 btn1 = new Vector3(-264,-250,0);
	private string currentName = "currentcmd";
	private string prevName = "prevcmd";
	private string nextName = "nextcmd";
	private int currentDepth = 7;
	private int prevDepth = 6;
	private int nextDepth = 6;
	private Vector3 currentScale = new Vector3 (1, 1, 1);
	private Vector3 prevScale = new Vector3 (0.8f, 0.8f, 0.8f);
	private Vector3 nextScale = new Vector3 (0.8f, 0.8f, 0.8f);
	private float ItemX = 100f;
	private Transform _trans;
	private Transform _current;
	private Transform _next;
	private Transform _prev;
	private Transform _tempcmd;
	private Transform _btnNext;
	private Transform _btnPrev;
	private BoxCollider _btnNextCollider;
	private BoxCollider _btnPrevCollider;
	
	void Awake ()
	{
		_trans = this.transform;
		this._current = this._trans.FindChild ("cmdlist/current");
		this._next = this._trans.FindChild ("cmdlist/next");
		this._prev = this._trans.FindChild ("cmdlist/prev");
		this._tempcmd = this._trans.FindChild ("cmdlist/tempcmd"); //模板
		this._btnNext = this._trans.FindChild ("btn/next");
		this._btnPrev = this._trans.FindChild ("btn/prev");
		this._btnNextCollider = this._btnNext.GetComponent<BoxCollider> ();
		this._btnPrevCollider = this._btnPrev.GetComponent<BoxCollider> ();
		
		
	}
	
	void OnEnable ()
	{
		Gate.instance.registerMediator (new MonsterRewardMediator (this));
	}
	
	void OnDisable ()
	{
		Gate.instance.removeMediator (MediatorName.MONSTER_REWARD_MEDIATOR);
		 
	}
	
	public  void Show ()
	{
		CreateCurrentCMD ();
		CreatePrevCMD (false);
		CreateNextCMD (false);
		this.CheckIsShowBtnNext ();
		this.CheckIsShowBtnPrev ();
	}
	 
	public void UpdateCurrent ()
	{
		MonsterRewardVo currentVo = MonsterRewardManager.Instance.CurrentMonsterRewardVo;
		Transform currentTrans = this._trans.FindChild ("cmdlist/currentcmd");
		 
		#region 追缉消耗
		UpdateUseItemNum (currentTrans, currentVo);
		#endregion
 		
		#region 追缉次数
		UpdateZhuiJiTimes (currentTrans, currentVo);
		#endregion
	}

	public void UpdateNext ()
	{
		MonsterRewardVo currentVo = MonsterRewardManager.Instance.CurrentMonsterRewardVo;
		Transform nextTrans = this._trans.FindChild ("cmdlist/nextcmd");
		
		#region 追缉消耗
		UpdateUseItemNum (nextTrans, currentVo);
		#endregion
		
		#region 追缉次数
		UpdateZhuiJiTimes (nextTrans, currentVo);
		#endregion
	}

	public void UpdatePrev ()
	{
		MonsterRewardVo currentVo = MonsterRewardManager.Instance.CurrentMonsterRewardVo;
		Transform prevTrans = this._trans.FindChild ("cmdlist/prevcmd");
		
		#region 追缉消耗
		UpdateUseItemNum (prevTrans, currentVo);
		#endregion
		
		#region 追缉次数
		UpdateZhuiJiTimes (prevTrans, currentVo);
		#endregion
	}

	public void CreateCurrentCMD ()
	{
		if (MonsterRewardManager.Instance.CurrentMonsterRewardVo != null) {
			this._tempcmd.gameObject.SetActive (true);
			MonsterRewardVo currentVo = MonsterRewardManager.Instance.CurrentMonsterRewardVo;
			
			GameObject currentObj = NGUITools.AddChild (this._tempcmd.parent.gameObject, this._tempcmd.gameObject);
			currentObj.name = currentName;
			currentObj.GetComponent<UIPanel> ().depth = this.currentDepth;
			Transform currentTrans = currentObj.transform;
			currentTrans.localPosition = this._current.localPosition;
			currentTrans.localScale = this.currentScale;
			CreateCmdInfo (currentVo, currentTrans);
			CheckVIPFunction (currentVo, currentTrans);
			CheckCanMonsterReward (currentVo, currentTrans);
			currentTrans.FindChild("touch").gameObject.SetActive(true);
			this._tempcmd.gameObject.SetActive (false);
		}
	}
	 
	public void CreatePrevCMD (bool playAnimation)
	{
		if (MonsterRewardManager.Instance.PrevMonsterRewardVo != null) {
			this._tempcmd.gameObject.SetActive (true);
			MonsterRewardVo prevVo = MonsterRewardManager.Instance.PrevMonsterRewardVo;
			
			GameObject prevObj = NGUITools.AddChild (this._tempcmd.parent.gameObject, this._tempcmd.gameObject);
			prevObj.name = prevName;
			prevObj.GetComponent<UIPanel> ().depth = this.prevDepth;
			Transform prevTrans = prevObj.transform;
			if (playAnimation) {
				#region 动画
				TweenScale ts = prevObj.GetComponent<TweenScale> ();
//				ts.duration = 0.4f;
				ts.from = Vector3.zero;
				ts.to = prevScale;
				TweenPosition tp = prevObj.GetComponent<TweenPosition> ();
//				tp.duration = 0.4f;
				tp.from = _current.localPosition;
				tp.to = _prev.localPosition;
				ts.Play (true);
				tp.Play (true);
				#endregion
			} else {
				prevTrans.localPosition = this._prev.localPosition;
				prevTrans.localScale = this.prevScale;
			}
  
			prevTrans.FindChild ("mask").gameObject.SetActive (true);
			
			CreateCmdInfo (prevVo, prevTrans);
			CheckVIPFunction (prevVo, prevTrans);
			CheckCanMonsterReward (prevVo, prevTrans);
			this._tempcmd.gameObject.SetActive (false);
 
		}
	}
	 
	public void CreateNextCMD (bool playAnimation)
	{
		if (MonsterRewardManager.Instance.NextMonsterRewardVo != null) {
			this._tempcmd.gameObject.SetActive (true);
			MonsterRewardVo nextVo = MonsterRewardManager.Instance.NextMonsterRewardVo;
			
			GameObject nextObj = NGUITools.AddChild (this._tempcmd.parent.gameObject, this._tempcmd.gameObject);
			nextObj.name = nextName;
			nextObj.GetComponent<UIPanel> ().depth = this.nextDepth;
			Transform nextTrans = nextObj.transform;
			if (playAnimation) {
				#region 动画
				TweenScale ts = nextObj.GetComponent<TweenScale> ();
//				ts.duration = 0.4f;
				ts.from = Vector3.zero;
				ts.to = nextScale;
				TweenPosition tp = nextObj.GetComponent<TweenPosition> ();
//				tp.duration = 0.4f;
				tp.from = _current.localPosition;
				tp.to = _next.localPosition;
				ts.Play (true);
				tp.Play (true);
				#endregion
			} else {
				nextTrans.localPosition = this._next.localPosition;
				nextTrans.localScale = this.nextScale;
			}
 

			nextTrans.FindChild ("mask").gameObject.SetActive (true);
			CreateCmdInfo (nextVo, nextTrans);
			CheckVIPFunction (nextVo, nextTrans);
			CheckCanMonsterReward (nextVo, nextTrans);
			this._tempcmd.gameObject.SetActive (false);
			
			
		}
	}
	
	public void CheckVIPFunction (MonsterRewardVo currentVo, Transform currentTrans)
	{
		int btnCount =1;
		Transform btnClear = currentTrans.FindChild ("btn/btnclear");
		Transform btnQuick = currentTrans.FindChild ("btn/btnquick");
        if (currentVo.IsCanQuick)
        {
            btnQuick.gameObject.SetActive(true);
            btnCount++;
        }
        else
        {
            btnQuick.gameObject.SetActive(false);
        }

        if (currentVo.IsCanOneClear)
        {
            btnClear.gameObject.SetActive(true);
            btnCount++;
        }
        else
        {
            btnClear.gameObject.SetActive(false);
        }
		
		switch (btnCount) {
		case 1:
			currentTrans.FindChild("btn").localPosition  = btn1;
			break;
		case 2:
			currentTrans.FindChild("btn").localPosition  = btn2;
			break;
		case 3:
			currentTrans.FindChild("btn").localPosition  = btn3;
			break;
		default:
			break;
		}
 
	}
	
	public void CheckCanMonsterReward (MonsterRewardVo currentVo, Transform currentTrans)
	{
		Transform btnClear = currentTrans.FindChild ("btn/btnclear");
		Transform btnGo = currentTrans.FindChild ("btn/btngo");
		Transform btnQuick = currentTrans.FindChild ("btn/btnquick");
		if (currentVo.CheckCondition) {
			btnClear.GetComponent<BoxCollider> ().enabled = true;
			btnGo.GetComponent<BoxCollider> ().enabled = true;
			btnQuick.GetComponent<BoxCollider> ().enabled = true;
			btnClear.FindChild ("Sprite").GetComponent<UISprite> ().spriteName = SourceManager.Instance.GetCommonButton1SpriteNameByStatus (true);
			btnGo.FindChild ("Sprite").GetComponent<UISprite> ().spriteName = SourceManager.Instance.GetCommonButton1SpriteNameByStatus (true);
			btnQuick.FindChild ("Sprite").GetComponent<UISprite> ().spriteName = SourceManager.Instance.GetCommonButton1SpriteNameByStatus (true);
		} else { 
			btnClear.GetComponent<BoxCollider> ().enabled = false;
			btnGo.GetComponent<BoxCollider> ().enabled = false;
			btnQuick.GetComponent<BoxCollider> ().enabled = false;
			btnClear.FindChild ("Sprite").GetComponent<UISprite> ().spriteName = SourceManager.Instance.GetCommonButton1SpriteNameByStatus (false);
			btnGo.FindChild ("Sprite").GetComponent<UISprite> ().spriteName = SourceManager.Instance.GetCommonButton1SpriteNameByStatus (false);
			btnQuick.FindChild ("Sprite").GetComponent<UISprite> ().spriteName = SourceManager.Instance.GetCommonButton1SpriteNameByStatus (false);
		}
	}
	
	public void CreateCmdInfo (MonsterRewardVo currentVo, Transform currentTrans)
	{
		#region 追缉条件
		UpdateCondition (currentTrans, currentVo);
		#endregion
		
		#region 追缉消耗
		UpdateUseItemNum (currentTrans, currentVo);
		#endregion
		
		#region 头像
		UpdateIcon (currentTrans, currentVo);
		#endregion
		
		#region 详细信息
		UpdateDes (currentTrans, currentVo);
		#endregion
		
		#region 道具
		CreateItemList (currentTrans, currentVo);
		#endregion
		
		#region 名字
		UpdateName (currentTrans, currentVo);
		#endregion
		
		#region 追缉次数
		UpdateZhuiJiTimes (currentTrans, currentVo);
		#endregion
	}
 
	
	/// <summary>
	/// 更新追缉消耗
	/// </summary>
	/// <param name='currentVo'>
	/// Current vo.
	/// </param>
	public void UpdateUseItemNum (Transform currentTrans, MonsterRewardVo currentVo)
	{
		currentTrans.FindChild ("consume/name").GetComponent<UILabel> ().text = currentVo.UseItem.name;
		uint itemNum = ItemManager.GetInstance ().GetItemNumById (currentVo.ItemId);
		currentTrans.FindChild ("consume/content").GetComponent<UILabel> ().text = itemNum + "/" + currentVo [MonsterRewardNumType.Item];
	}
	
	/// <summary>
	/// 更新头像
	/// </summary>
	/// <param name='currentVo'>
	/// Current vo.
	/// </param>
	public void UpdateIcon (Transform currentTrans, MonsterRewardVo currentVo)
	{
		currentTrans.FindChild ("icon").GetComponent<UITexture> ().mainTexture = SourceManager.Instance.getTextByIconName (RaidManager.Instance.getRaidVo ((uint)currentVo.Map.id).gateIcon, PathConst.RAID_PREVIEW_PATH);
	}
	
	
	/// <summary>
	/// 更新条件
	/// </summary>
	/// <param name='currentVo'>
	/// Current vo.
	/// </param>
	public void UpdateCondition (Transform currentTrans, MonsterRewardVo currentVo)
	{
		if (currentVo.CheckCondition) {
			switch (currentVo.Type) {
			case MonsterRewardType.None:
				currentTrans.FindChild ("condition/content").GetComponent<UILabel> ().text = string.Format (LanguageManager.GetText ("monster_reward_condition_pass"), "无限制");
				break;
			case MonsterRewardType.Level:
				currentTrans.FindChild ("condition/content").GetComponent<UILabel> ().text = string.Format (LanguageManager.GetText ("monster_reward_condition_pass"), "人物达到" + currentVo.TypeValue + "级");
				break;
			case MonsterRewardType.Vip:
				currentTrans.FindChild ("condition/content").GetComponent<UILabel> ().text = string.Format (LanguageManager.GetText ("monster_reward_condition_pass"), "VIP达到" + currentVo.TypeValue + "级");
				break;
			case MonsterRewardType.Raid:
				currentTrans.FindChild ("condition/content").GetComponent<UILabel> ().text = string.Format (LanguageManager.GetText ("monster_reward_condition_pass"), "通过关卡" + ConfigDataManager.GetInstance ().getMapConfig ().getMapData ((int)currentVo.TypeValue).name);
				break;
			default:
				break;
			}
		} else {
			switch (currentVo.Type) {
			 
			case MonsterRewardType.Level:
				currentTrans.FindChild ("condition/content").GetComponent<UILabel> ().text = string.Format (LanguageManager.GetText ("monster_reward_condition_no_pass"), "人物达到" + currentVo.TypeValue + "级");
				break;
			case MonsterRewardType.Vip:
				currentTrans.FindChild ("condition/content").GetComponent<UILabel> ().text = string.Format (LanguageManager.GetText ("monster_reward_condition_no_pass"), "VIP达到" + currentVo.TypeValue + "级");
				break;
			case MonsterRewardType.Raid:
				currentTrans.FindChild ("condition/content").GetComponent<UILabel> ().text = string.Format (LanguageManager.GetText ("monster_reward_condition_no_pass"), "通过关卡" + ConfigDataManager.GetInstance ().getMapConfig ().getMapData ((int)currentVo.TypeValue).name);
				break;
			default:
				break;
			}
		}
	}


	
	/// <summary>
	/// 更新详细信息
	/// </summary>
	/// <param name='currentVo'>
	/// Current vo.
	/// </param>
	public void UpdateDes (Transform currentTrans, MonsterRewardVo currentVo)
	{
		currentTrans.FindChild ("info").GetComponent<UILabel> ().text = currentVo.Des;
	}
	
	/// <summary>
	/// 创建道具队列
	/// </summary>
	/// <param name='currentTrans'>
	/// Current trans.
	/// </param>
	/// <param name='currentVo'>
	/// Current vo.
	/// </param>
	public void CreateItemList (Transform currentTrans, MonsterRewardVo currentVo)
	{
		Transform tempItem = currentTrans.FindChild ("item/itemlist/tempitem");
		
		var itemArray = currentVo.Map.dropItem.Split (',');
		List<uint> ItemKeyList = new List<uint> ();
		foreach (string i in itemArray) {
			ItemKeyList.Add (uint.Parse (i.Trim ()));
		}
		tempItem.gameObject.SetActive (true);
		for (int i = 0; i < ItemKeyList.Count; i++) {
			var itemModel = MonsterRewardManager.Instance.Item.getTemplateData ((int)ItemKeyList [i]);
			GameObject itemObj = NGUITools.AddChild (tempItem.parent.gameObject, tempItem.gameObject);
			itemObj.name = ItemKeyList [i].ToString ();
			Transform itemObjTrans = itemObj.transform;
			itemObjTrans.localPosition = new Vector3 (tempItem.localPosition.x + i * ItemX, tempItem.localPosition.y, tempItem.localPosition.z);
			UITexture itemObjTex = itemObjTrans.FindChild ("item").GetComponent<UITexture> ();
			if (!string.IsNullOrEmpty (itemModel.icon)) {
				DealTexture.Instance.setTextureToIcon (itemObjTex, itemModel, false); //设置ICON的图片
				itemObjTrans.FindChild ("Sprite").GetComponent<UISprite> ().spriteName = BagManager.Instance.getItemBgByType (itemModel.quality, true);
			}
			
		}
		tempItem.gameObject.SetActive (false);
	}

	/// <summary>
	/// 更新名字
	/// </summary>
	/// <param name='currentTrans'>
	/// Current trans.
	/// </param>
	/// <param name='currentVo'>
	/// Current vo.
	/// </param>
	public void UpdateName (Transform currentTrans, MonsterRewardVo currentVo)
	{
		currentTrans.FindChild ("name").GetComponent<UILabel> ().text = currentVo.Map.name;
	}
	
	/// <summary>
	/// 更新追缉次数
	/// </summary>
	/// <param name='currentVo'>
	/// Current vo.
	/// </param>
	public void UpdateZhuiJiTimes (Transform currentTrans, MonsterRewardVo currentVo)
	{
		currentTrans.FindChild ("times/content").GetComponent<UILabel> ().text = currentVo.CurrentClearNum + "/" + currentVo.MaxClearNum;
	}
	
	
	
	#region 翻页操作
	
	public void NextPage ()
	{
 
		Transform next = this._trans.FindChild ("cmdlist/nextcmd");
		TweenScale tsn = next.GetComponent<TweenScale> ();
		tsn.from = nextScale;
		tsn.to = currentScale;
		TweenPosition tpn = next.GetComponent<TweenPosition> ();
		tpn.from = next.localPosition;
		tpn.to = _current.localPosition;
		
		
		Transform current = this._trans.FindChild ("cmdlist/currentcmd");
		TweenScale ts = current.GetComponent<TweenScale> ();
		ts.from = currentScale;
		ts.to = prevScale;
		TweenPosition tp = current.GetComponent<TweenPosition> ();
		tp.from = current.localPosition;
		tp.to = _prev.localPosition;
		
		
		Transform prev = this._trans.FindChild ("cmdlist/prevcmd"); 
		if (prev) {
			prev.GetComponent<UIPanel> ().depth = prevDepth - 1;
			TweenScale prevTsn = prev.GetComponent<TweenScale> ();
			prevTsn.from = prevScale;
			prevTsn.to = Vector3.zero;
			TweenPosition prevTP = prev.GetComponent<TweenPosition> ();
			prevTP.from = prev.localPosition;
			prevTP.to = _current.localPosition;
			prevTP.ResetToBeginning ();
			prevTsn.ResetToBeginning ();
			prevTsn.Play (true);
			prevTP.Play (true);
		}
		
		tsn.ResetToBeginning ();
		tpn.ResetToBeginning ();
		ts.ResetToBeginning ();
		tp.ResetToBeginning ();
		
 		#region 屏蔽item
		BoxCollider[] itemBoxList = next.FindChild("item/itemlist").GetComponentsInChildren<BoxCollider>();
		for (int i = 0,max = itemBoxList.Length; i < max; i++) {
			itemBoxList[i].enabled = false;
		}
		#endregion
		
		this._btnNextCollider.enabled = !this._btnNextCollider.enabled;
		this._btnPrevCollider.enabled = !this._btnPrevCollider.enabled;
		next.GetComponent<UIPanel> ().depth = currentDepth;
		current.GetComponent<UIPanel> ().depth = prevDepth;
		
		current.FindChild ("mask").gameObject.SetActive (true);
		next.FindChild ("mask").gameObject.SetActive (false);
		current.FindChild("touch").gameObject.SetActive(false);
		next.FindChild("touch").gameObject.SetActive(false);
		
		tpn.SetOnFinished (new EventDelegate (() => {
			next.name = currentName;
			next.FindChild("touch").gameObject.SetActive(true);
			#region 激活item
			for (int i = 0,max = itemBoxList.Length; i < max; i++) {
				itemBoxList[i].enabled =true;
			}
			#endregion
			MonsterRewardManager.Instance.IsLock = false;
		}));
		tp.SetOnFinished (new EventDelegate (() => {
			DeletePrev ();
			current.name = prevName;
			
			
			this._btnNextCollider.enabled = !this._btnNextCollider.enabled;
			this._btnPrevCollider.enabled = !this._btnPrevCollider.enabled;
		}));
		
		tsn.Play (true);
		tpn.Play (true);
		ts.Play (true);
		tp.Play (true);
		CreateNextCMD (true);
		
		this.CheckIsShowBtnNext ();
		this.CheckIsShowBtnPrev ();
	}
	
	public void PrevPage ()
	{
		Transform prev = this._trans.FindChild ("cmdlist/prevcmd");
		TweenScale tsn = prev.GetComponent<TweenScale> ();
		tsn.from = prevScale;
		tsn.to = currentScale;
		TweenPosition tpn = prev.GetComponent<TweenPosition> ();
		tpn.from = prev.localPosition;
		tpn.to = _current.localPosition;
		
		
		Transform current = this._trans.FindChild ("cmdlist/currentcmd");
		TweenScale ts = current.GetComponent<TweenScale> ();
		ts.from = currentScale;
		ts.to = nextScale;
		TweenPosition tp = current.GetComponent<TweenPosition> ();
		tp.from = current.localPosition;
		tp.to = _next.localPosition;
		
		Transform next = this._trans.FindChild ("cmdlist/nextcmd"); 
		if (next) {
			next.GetComponent<UIPanel> ().depth = nextDepth - 1;
			TweenScale nextTsn = next.GetComponent<TweenScale> ();
			nextTsn.from = nextScale;
			nextTsn.to = Vector3.zero;
			TweenPosition nextTP = next.GetComponent<TweenPosition> ();
			nextTP.from = next.localPosition;
			nextTP.to = _current.localPosition;
			nextTP.ResetToBeginning ();
			nextTsn.ResetToBeginning ();
			nextTsn.Play (true);
			nextTP.Play (true);
		}
	 
		tsn.ResetToBeginning ();
		tpn.ResetToBeginning ();
		ts.ResetToBeginning ();
		tp.ResetToBeginning ();
		
		#region 屏蔽item
		BoxCollider[] itemBoxList = prev.FindChild("item/itemlist").GetComponentsInChildren<BoxCollider>();
		for (int i = 0,max = itemBoxList.Length; i < max; i++) {
			itemBoxList[i].enabled  = false;
		}
		#endregion
		this._btnNextCollider.enabled = !this._btnNextCollider.enabled;
		this._btnPrevCollider.enabled = !this._btnPrevCollider.enabled;	 
		prev.GetComponent<UIPanel> ().depth = currentDepth;
		current.GetComponent<UIPanel> ().depth = nextDepth;
		
		current.FindChild ("mask").gameObject.SetActive (true);
		prev.FindChild ("mask").gameObject.SetActive (false);
		current.FindChild("touch").gameObject.SetActive(false);
		prev.FindChild("touch").gameObject.SetActive(false);
		
		tpn.SetOnFinished (new EventDelegate (() => {
			prev.name = currentName;
			prev.FindChild("touch").gameObject.SetActive(true);
			#region 激活item
			for (int i = 0,max = itemBoxList.Length; i < max; i++) {
				itemBoxList[i].enabled = true;
			}
			#endregion
			MonsterRewardManager.Instance.IsLock = false;
		}));
		
		tp.SetOnFinished (new EventDelegate (() => {
			DeleteNext ();
			current.name = nextName;
			
			this._btnNextCollider.enabled = !this._btnNextCollider.enabled;
			this._btnPrevCollider.enabled = !this._btnPrevCollider.enabled;
		}));
		
		tsn.Play (true);
		tpn.Play (true);
		ts.Play (true);
		tp.Play (true);
		CreatePrevCMD (true);
		
		this.CheckIsShowBtnNext ();
		this.CheckIsShowBtnPrev ();
	}
	
	void DeletePrev ()
	{
		Transform prev = this._trans.FindChild ("cmdlist/prevcmd");
		if (prev) {
			Destroy (prev.gameObject);
		}
		
	}
	
	void DeleteNext ()
	{
		Transform next = this._trans.FindChild ("cmdlist/nextcmd");
		if (next) {
			Destroy (next.gameObject);
		}
		
	}
	#endregion
	
	void CheckIsShowBtnNext ()
	{
		if (MonsterRewardManager.Instance.IsShowBtnNext) {
			this._btnNext.gameObject.SetActive (true);
		} else {
			this._btnNext.gameObject.SetActive (false);
		}
	}
	
	void CheckIsShowBtnPrev ()
	{
		if (MonsterRewardManager.Instance.IsShowBtnPrev) {
			this._btnPrev.gameObject.SetActive (true);
		} else {
			this._btnPrev.gameObject.SetActive (false);
		}
	}
	
}
