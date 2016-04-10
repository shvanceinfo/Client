using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using manager;
using mediator;
using MVC.entrance.gate;
using model;

public class DungeonInfoView : MonoBehaviour
{
	public int CDTime = 3;
	private const int HEIGHT = -100; //间隔的高度
	private  const int MAXSIZE = 3; 	 //最大人数
	private const string SWORD = "common_zhanshi";
	private const string MAGIC = "common_mofashi";
	private const string ARCHER = "common_gongjianshou";
	private Transform _trans;	//当前对象的transform组件
	private UILabel _bossName;	//boss的名字
	private UITexture _bossIcon;//boss的图片
	private UITexture _itemIcon;//道具的图片
	private UILabel _needLevel; //需要的等级
	private UILabel _passNum; 	//通关次数
	private Transform _peopleTemplate;	//右边区域
	private Transform _btnStartBattle;	//开始战斗的按钮
	private Transform _cd; 		//显示cd界面
	
	
	private BetterList<GameObject> cacheList = new BetterList<GameObject> ();//用来缓存生成的队伍信息 

	
	void Awake ()
	{
		this._trans = this.transform;
		this._bossName = this._trans.Find ("left/boss_name/name").GetComponent<UILabel> ();
		this._bossIcon = this._trans.Find ("left/left_bg/bossIcon").GetComponent<UITexture> ();
		this._needLevel = this._trans.Find ("left/level/num").GetComponent<UILabel> ();
		this._passNum = this._trans.Find ("left/cishu/passNum").GetComponent<UILabel> ();
		this._peopleTemplate = this._trans.Find ("right_duiyuan/peopleInfo");//得到模板对象
		this._btnStartBattle = this._trans.Find ("Btn/start_battle"); //开始战斗的按钮
		this._cd = this._trans.Find ("cd");//倒计时cd
	}
	
	void OnEnable ()
	{
		Gate.instance.registerMediator (new DungeonInfoMediator (this));
	}
	
	void OnDisable ()
	{
		Gate.instance.removeMediator (MediatorName.DUNGEONINFO_MEDIATOR);
		NPCManager.Instance.createCamera (false); //消除3D相机
	}
	
 
	
	
	//更新副本
	public	void UpdateDungeon (MapDataItem item, IList<ItemTemplate> itemList, ushort passNum)
	{
		 
		this._bossName.text = item.name; //设置副本BOSS名字
		this._bossIcon.mainTexture  = SourceManager.Instance.getTextByIconName( item.icon, PathConst.RAID_PREVIEW_PATH);	//设置ICON的图片
		//DealTexture.Instance.setTextureToIcon (this._bossIcon, item.icon, false); //设置ICON的图片
		for (int i = 1; i <= itemList.Count; i++) {
			this._itemIcon = this._trans.Find ("left/item/item" + i).GetComponent<UITexture> ();
			this._itemIcon.GetComponent<BtnTipsMsg>().Iteminfo =  new ItemInfo(itemList [i - 1].id,0,0);
			this._trans.Find ("left/item/Sprite" + i).GetComponent<UISprite> ().spriteName = BagManager.Instance.getItemBgByType(itemList [i - 1].quality,true);//根据品质得到背景
			DealTexture.Instance.setTextureToIcon (this._itemIcon, itemList [i - 1], false); //设置ICON的图片
		}
		
		this._needLevel.text = item.nEnterLevel.ToString ();
		this._passNum.text = passNum + "/3";
 
	}
	
	//更新队伍列表
	public void UpdatePeopleList (BetterList<PeopleInfo> peopleList)
	{
 		#region 清空上次生成的对象
		foreach (var item in cacheList) {
			Destroy (item);
		}
		cacheList.Release ();
		#endregion
 
		this._peopleTemplate.gameObject.SetActive (true);
		int count = 0; //位置索引
		foreach (PeopleInfo people in peopleList) {
			GameObject peopleTemp = NGUITools.AddChild (this._peopleTemplate.parent.gameObject, this._peopleTemplate.gameObject);//复制到父类下
			Transform teamTrans = peopleTemp.transform;
			teamTrans.localPosition = new Vector3 (teamTrans.localPosition.x, count * HEIGHT+this._peopleTemplate.localPosition.y, teamTrans.localPosition.z);
			
			if (people.leader) {
				teamTrans.Find ("duizhang").gameObject.SetActive (true);
				
				if (people.name == CharacterPlayer.character_property.nick_name) {
					this._btnStartBattle.gameObject.SetActive (true);
				}
				
			} else {
				teamTrans.Find ("duizhang").gameObject.SetActive (false);
			}

			teamTrans.Find ("word/level/num").GetComponent<UILabel> ().text = people.level.ToString ();//设置名字
			teamTrans.Find ("word/name").GetComponent<UILabel> ().text = people.name;//设置名字
			teamTrans.Find ("word/zhandouli/num").GetComponent<UILabel> ().text = people.battlePower.ToString ();//设置名字
			teamTrans.Find ("touxiang/emptyImg").gameObject.SetActive (false);
			var playImg = teamTrans.Find ("touxiang/playImg").GetComponent<UISprite> ();
			switch (people.career) {
			case CHARACTER_CAREER.CC_SWORD:
				playImg.spriteName = SWORD;
				break;
			case  CHARACTER_CAREER.CC_ARCHER:
				playImg.spriteName = ARCHER;
				break;
			case CHARACTER_CAREER.CC_MAGICIAN:
				playImg.spriteName = MAGIC;
				break;
			default:
				break;
			}
			playImg.transform.gameObject.SetActive (true);
			cacheList.Add (peopleTemp);
			count++;
		}
		
		
		int emptySize = MAXSIZE - peopleList.size;
		for (int i = 0; i < emptySize; i++) {
			GameObject peopleTemp = NGUITools.AddChild (this._peopleTemplate.parent.gameObject, this._peopleTemplate.gameObject);//复制到父类下
			Transform teamTrans = peopleTemp.transform;
			teamTrans.localPosition = new Vector3 (teamTrans.localPosition.x, count * HEIGHT+this._peopleTemplate.localPosition.y, teamTrans.localPosition.z);
			teamTrans.Find ("duizhang").gameObject.SetActive (false);
			teamTrans.Find ("word").gameObject.SetActive (false);
			teamTrans.Find ("workempty").gameObject.SetActive (true);
			cacheList.Add (peopleTemp);
			count++;
		}
		
 	
		this._peopleTemplate.gameObject.SetActive (false);
	}
	
	//显示倒计时cd
	public void ShowCD ()
	{
		var time = this._cd.Find ("time").GetComponent<UILabel> ();//得到time对象
		var title = this._cd.Find ("title").GetComponent<UILabel> ();//得到标题名字
		title.text = this._bossName.text;
		this._cd.gameObject.SetActive (true);
		StartCoroutine (ColdDown (time));
	}
	
	//倒计时开始
	IEnumerator  ColdDown (UILabel time)
	{
		WaitForSeconds waitTime = new WaitForSeconds (0.9f);
		for (int i = this.CDTime; i >=0; i--) {
			time.text = i.ToString ();
			yield return waitTime;
			if (i == 1) {
				DungeonManager.Instance.SetNormalPing ();
			}
		}
		//恢复正常ping值
		
	}
	
	
}
