using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using manager;
using mediator;
using MVC.entrance.gate;
using model;

public class DungeonView : MonoBehaviour
{
	public  float HEIGHT = -98.5f; //间隔的高度
	
 
	
	private Transform _trans;	//当前对象的transform组件
	private UILabel _bossName;	//boss的名字
	private UITexture _bossIcon;//boss的图片
	private UITexture _itemIcon;//道具的图片
	private UILabel _needLevel; //需要的等级
	private UILabel _passNum; 	//通关次数
	private Transform _teamTemplate;//右边区域
	private Transform _createTeam; //创建队伍
	private Transform _quickJoin;  //快速加入队伍
	private Transform _prevDungeon;//前一个副本
	private Transform _nextDungeon;//下一个副本
	
	private BetterList<GameObject> cacheList = new BetterList<GameObject> ();//用来缓存生成的队伍信息 
	
	
	void Awake ()
	{
		this._trans = this.transform;
		this._bossName = this._trans.Find ("left/boss_name/name").GetComponent<UILabel> ();
		this._bossIcon = this._trans.Find ("left/left_bg/bossIcon").GetComponent<UITexture> ();
		this._needLevel = this._trans.Find ("left/level/num").GetComponent<UILabel> ();
		this._passNum = this._trans.Find ("left/cishu/passNum").GetComponent<UILabel> ();
		this._teamTemplate = this._trans.Find ("right_bg/teamlist/dragobj/right_duiwu");//得到模板对象
		this._createTeam = this._trans.Find ("Btn/create_team");
		this._quickJoin = this._trans.Find ("Btn/quick_join");
		this._prevDungeon = this._trans.Find ("left/left_jiantou/prev_dungeon");
		this._nextDungeon = this._trans.Find ("left/left_jiantou/next_dungeon");

	}
	
	void OnEnable ()
	{
		Gate.instance.registerMediator (new DungeonMediator (this));
	}
	
	void OnDisable ()
	{
		Gate.instance.removeMediator (MediatorName.DUNGEON_MEDIATOR);
		NPCManager.Instance.createCamera (false); //消除3D相机
	}
	
 	
	
	//更新副本
	public	void UpdateDungeon (MapDataItem item, IList<ItemTemplate> itemList, ushort passNum, bool btnCanUse, bool btnCanShowPrev, bool btnCanShowNext)
	{
		 
		this._bossName.text = item.name; //设置副本BOSS名字
		this._bossIcon.mainTexture  = SourceManager.Instance.getTextByIconName( item.icon, PathConst.RAID_PREVIEW_PATH);	//设置ICON的图片
		//DealTexture.Instance.setTextureToIcon (this._bossIcon, item.icon, false); 
		for (int i = 1; i <= itemList.Count; i++) {
			this._itemIcon = this._trans.Find ("left/item/item" + i).GetComponent<UITexture> ();
			this._itemIcon .GetComponent<BtnTipsMsg>().Iteminfo =  new ItemInfo(itemList [i - 1].id,0,0);
			this._trans.Find ("left/item/Sprite" + i).GetComponent<UISprite> ().spriteName = BagManager.Instance.getItemBgByType(itemList [i - 1].quality,true);//根据品质得到背景
			DealTexture.Instance.setTextureToIcon (this._itemIcon, itemList [i - 1], false); //设置ICON的图片
		}
		
		
		this._passNum.text = passNum + "/3";
		
		if (btnCanUse) {
			this._createTeam.GetComponent<BoxCollider> ().enabled = true;
			this._quickJoin.GetComponent<BoxCollider> ().enabled = true;
			this._createTeam.Find ("Sprite").GetComponent<UISprite> ().spriteName = SourceManager.Instance.GetCommonButton1SpriteNameByStatus(true);
			this._quickJoin.Find ("Sprite").GetComponent<UISprite> ().spriteName = SourceManager.Instance.GetCommonButton1SpriteNameByStatus(true);
			this._needLevel.text = "["+Constant.WHITE+"]"+item.nEnterLevel.ToString ();
		} else {
			this._createTeam.GetComponent<BoxCollider> ().enabled = false;
			this._quickJoin.GetComponent<BoxCollider> ().enabled = false;
			this._createTeam.Find ("Sprite").GetComponent<UISprite> ().spriteName = SourceManager.Instance.GetCommonButton1SpriteNameByStatus(false);
			this._quickJoin.Find ("Sprite").GetComponent<UISprite> ().spriteName = SourceManager.Instance.GetCommonButton1SpriteNameByStatus(false);
			this._needLevel.text = "["+Constant.RED+"]"+item.nEnterLevel.ToString ();
		}
		this._prevDungeon.gameObject.SetActive (btnCanShowPrev);	
		this._nextDungeon.gameObject.SetActive (btnCanShowNext);
	}
	
	//更新队伍列表
	public void UpdateTeamList (BetterList<DungeonTeamInfo> teamList, bool btnCanUse)
	{
 
		#region 清空上次生成的对象
		foreach (var item in cacheList) {
			Destroy (item);
		}
		cacheList.Release ();
		#endregion
 		
		this._teamTemplate.gameObject.SetActive (true);
		int count = 0;
		foreach (DungeonTeamInfo item in teamList) {
			GameObject team = NGUITools.AddChild (this._teamTemplate.parent.gameObject, this._teamTemplate.gameObject);//复制到父类下
			Transform teamTrans = team.transform;
			teamTrans.localPosition = new Vector3 (teamTrans.position.x, count * HEIGHT, teamTrans.position.z);
			teamTrans.Find ("join_team/Teamid").GetComponent<UILabel> ().text = item.teamId.ToString ();//设置队伍ID
			teamTrans.Find ("word/name").GetComponent<UILabel> ().text = item.name;//设置名字
			teamTrans.Find ("word/num").GetComponent<UILabel> ().text = item.playerNum + "/3";//当前默认设置一个队伍最多3个人
			
			if (btnCanUse) {
				teamTrans.Find ("join_team").GetComponent<BoxCollider> ().enabled = true;
				teamTrans.Find ("join_team/Sprite").GetComponent<UISprite> ().spriteName = SourceManager.Instance.GetCommonButton1SpriteNameByStatus(true);
				
			} else {
				teamTrans.Find ("join_team").GetComponent<BoxCollider> ().enabled = false;
				teamTrans.Find ("join_team/Sprite").GetComponent<UISprite> ().spriteName = SourceManager.Instance.GetCommonButton1SpriteNameByStatus(false);
			}
				
			
			count++;
			this.cacheList.Add (team);
		}
		this._teamTemplate.gameObject.SetActive (false);
	}
	
	
	
	
}
