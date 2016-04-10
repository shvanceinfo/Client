/**该文件实现的基本功能等
function: 实现潘多拉的功能
author:zyl
date:2014-06-10
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

public class PandoraView : MonoBehaviour
{
	float _tempX = 100;
	Transform _trans;
	UILabel _name;
	UITexture _img;
	UILabel _desc;
	UILabel _title;
	UILabel _content;   //玩法说明
	UILabel _ruleContent;  //玩法规则
	UILabel _time;		//世界显示

	Transform _tempItem; //模板
	Transform _itemList; //列表容器


	void Awake ()
	{
		_trans = this.transform;
		this._name = this._trans.FindChild ("left/info/name").GetComponent<UILabel> ();
		this._img = this._trans.FindChild ("left/info/img").GetComponent<UITexture> ();
		this._desc = this._trans.FindChild ("left/info/des").GetComponent<UILabel> ();
		this._title = this._trans.FindChild ("left/info/title").GetComponent<UILabel> ();
		this._content = this._trans.FindChild ("right/info/introduce/content").GetComponent<UILabel> ();
		this._ruleContent = this._trans.FindChild ("right/info/rule/content").GetComponent<UILabel> ();

		this._tempItem = this._trans.FindChild ("right/info/award/itemlist/tempitem");
		this._itemList = this._trans.FindChild ("right/info/award/itemlist");
		this._time = this._trans.FindChild ("right/info/time/content").GetComponent<UILabel> ();


	}
	
	void OnEnable ()
	{
		Gate.instance.registerMediator (new PandoraMediator (this));
	}
	
	void OnDisable ()
	{
		Gate.instance.removeMediator (MediatorName.PANDORA_MEDIATOR);
		
	}

	public void Show (PandoraVo pandora)
	{
		this._name.text = pandora.MonsterName;
		this._img.mainTexture = SourceManager.Instance.getTextByIconName (pandora.Map.icon, PathConst.RAID_PREVIEW_PATH);	//设置ICON的图片
		this._desc.text = pandora.Desc;
		this._title.text =  LanguageManager.GetText("pandora_defeat")+ pandora.MonsterName;
		this._content.text = string.Format (LanguageManager.GetText ("pandora_intro"), pandora.Map.nEnterLevel);
		this._ruleContent.text = pandora.RuleDesc;

		this.AddItem (pandora);
		this.AddTime (pandora);

	}

	/// <summary>
	/// 添加时间信息
	/// </summary>
	/// <param name="pandora">Pandora.</param>
	void AddTime (PandoraVo pandora)
	{
		bool isFull = false;
		//00:00-23:59
		StringBuilder sb = new StringBuilder ();
		for (int i = 0, max = pandora.Schedule.Count; i < max; i++) {
			SActivityTime st = pandora.Schedule [i];
			if (st.kDayTime.u8BeginHour == 0 && st.kDayTime.u8BeginMinute == 0 && st.kDayTime.u8EndHour == 23 && st.kDayTime.u8EndMinute == 59) {
				//全天的逻辑
				sb.Remove (0, sb.Length);
				sb.Append (LanguageManager.GetText ("pandora_full_time"));
				isFull = true;
				break;
			}
			else {
				//否则走普通时间的逻辑
				sb.Append (string.Format ("{0:00}:{1:00}-{2:00}:{3:00}", st.kDayTime.u8BeginHour, st.kDayTime.u8BeginMinute, st.kDayTime.u8EndHour, st.kDayTime.u8EndMinute));
				sb.Append (",");
			}
		}
		if (isFull) {
			this._time.text = sb.ToString ();
		}
		else {
			if (sb.Length > 0) {
				this._time.text = sb.Remove (sb.Length - 1, 1).ToString ();
			}
			else {
				this._time.text = string.Empty;
			}
		}
	}

	/// <summary>
	/// 添加道具信息
	/// </summary>
	/// <param name="pandora">Pandora.</param>
	void AddItem (PandoraVo pandora)
	{
		for (int i = 0,max=pandora.DropItem.Count; i < max; i++) {
			var obj = NGUITools.AddChild (this._itemList.gameObject, this._tempItem.gameObject);
			obj.name = pandora.DropItem [i].id.ToString ();
			obj.SetActive (true);
			var objTrans = obj.transform;
			objTrans.localPosition = new Vector3 (this._tempItem.localPosition.x + i * _tempX, this._tempItem.localPosition.y, 0);
			objTrans.FindChild ("item").GetComponent<UITexture> ().mainTexture = SourceManager.Instance.getTextByIconName (pandora.DropItem [i].icon);
			objTrans.FindChild ("Sprite").GetComponent<UISprite> ().spriteName = BagManager.Instance.getItemBgByType (pandora.DropItem [i].quality, true);
		}
	}
}
