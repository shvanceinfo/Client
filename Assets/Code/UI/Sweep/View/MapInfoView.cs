/**该文件实现的基本功能等
function: 实现点击关卡按钮弹出小地图信息
author:ljx
date:2014-03-12
**/
using UnityEngine;
using System.Collections;
using manager;
using helper;
using model;

public class MapInfoView : MonoBehaviour
{
	const int AWARD_NUM = 4;
	public const int STAR_NUM = 3;
	private UILabel _mapTitle; 		//地图标题
	private UILabel _recommendPower; //推荐战力
	private UILabel _raidType; 		//地图标题
	private UILabel _enterLevel; 	//地图标题
	private UILabel _limitTime; 	//地图标题
	private UILabel _raidConsume; 	//地图标题
	private GameObject[] _awards; 	//地图标题
	private UISprite[] _starSps;	//评价星星
	private GameObject _enterBtn;	//进入副本按钮
	private GameObject _sweepBtn;	//扫荡按钮
	private UITexture _mapPic; 		//地图信息
		
	void Awake ()
	{
		_awards = new GameObject[AWARD_NUM];
		_starSps = new UISprite[STAR_NUM];
		_mapTitle = transform.Find ("topInfo/title").GetComponent<UILabel> ();
		_recommendPower = transform.Find ("topInfo/recommend/power").GetComponent<UILabel> ();
		_raidType = transform.Find ("info/raidType/value").GetComponent<UILabel> ();
		_enterLevel = transform.Find ("info/enterLevel/value").GetComponent<UILabel> ();
		_limitTime = transform.Find ("info/limitTime/value").GetComponent<UILabel> ();
		_raidConsume = transform.Find ("info/raidConsume/value").GetComponent<UILabel> ();
		_enterBtn = transform.Find ("btn/enter").gameObject;
		_sweepBtn = transform.Find ("btn/sweep").gameObject;
		_mapPic = transform.Find ("topInfo/mapBg").GetComponent<UITexture> ();
		for (int i=1; i<=STAR_NUM; i++) {
			UISprite star = transform.Find ("topInfo/starInfo/star" + i).GetComponent<UISprite> ();
			_starSps [i - 1] = star;
		}
		for (int i=1; i<=AWARD_NUM; i++) {
			GameObject obj = transform.Find ("award/award" + i).gameObject;
			_awards [i - 1] = obj;
		}
	}
	
	//初始化小地图详细信息
	public void initView ()
	{
		MapDataItem mapData = SweepManager.Instance.CurrentMap;
		if (mapData != null) {
			_mapTitle.text = mapData.name;
			_recommendPower.text = mapData.recommondPower.ToString () + "战力";
			_enterLevel.text = mapData.nEnterLevel.ToString () + "级";
			int minute = mapData.limitTime / 60;
			if (minute == 0) {
				_limitTime.text = "无限制";
			}else{
				_limitTime.text = minute.ToString () + "分钟";
			}
			_raidConsume.text = mapData.engeryConsume.ToString () + "体力";
			if (RaidManager.Instance.CurrentRaid.isHard)
				_raidType.text = LanguageManager.GetText ("label_tollgate_elite_level");
			else
				_raidType.text = LanguageManager.GetText ("label_tollgate_normal_level");
			showStar (mapData);
			showAward (mapData);
			if (!RaidManager.Instance.PassMapHash.ContainsKey ((uint)mapData.id / 10)) { //该副本还未通关，隐藏扫荡按钮
				_sweepBtn.GetComponent<BoxCollider> ().enabled = false;
				_sweepBtn.SetActive (false);
				_enterBtn.transform.localPosition = Vector3.zero; //进入副本居中
			}
			_mapPic.mainTexture = SourceManager.Instance.getTextByIconName (mapData.icon, PathConst.RAID_PREVIEW_PATH);
		}	   
	}
	
	//显示星星等级
	private void showStar (MapDataItem mapData)
	{
//		 ChapterVo chapterVo = RaidManager.Instance.CurrentChapter;
//			chapterVo.mapVos
		uint mapid = (uint)mapData.id / 10;
		if (RaidManager.Instance.PassMapHash.ContainsKey (mapid)) {
			var raid = RaidManager.Instance.CurrentRaid;
			for (int i = 0; i < raid.FormatStarNum; i++) {
				_starSps [i].gameObject.SetActive (true);
			}
		} 
			
	}
	
	//显示掉落物品
	
	private void showAward (MapDataItem mapData)
	{
		string dropItems = mapData.dropItem;
		if (!string.IsNullOrEmpty (dropItems)) {
			string[] items = dropItems.Split (',');
			for (int i = 0; i < AWARD_NUM; i++) {
				_awards [i].SetActive (true);
				if (i < items.Length) {
					ItemTemplate itemTemp = ItemManager.GetInstance ().GetTemplateByTempId (uint.Parse (items [i]));
					_awards [i].transform.Find ("bg").GetComponent<UISprite> ().spriteName = ViewHelper.GetBoderById ((int)itemTemp.id);
					_awards [i].transform.Find ("Texture").GetComponent<BtnTipsMsg> ().Iteminfo = new ItemInfo (itemTemp.id, 0, 0);
					DealTexture.Instance.setTextureToIcon (_awards [i].transform.Find ("Texture").GetComponent<UITexture> (), itemTemp);
				} else
					_awards [i].SetActive (false);
			}
		} else {
			for (int i=0; i<AWARD_NUM; i++)
				_awards [i].SetActive (false);
		}
	}
}
