/**该文件实现的基本功能等
function: 关卡奖励的数据表现
author:zyl
date:date:2014-04-30
**/
using UnityEngine;
using manager;
using model;
using MVC.entrance.gate;
using mediator;

public class ChapterAwardView : MonoBehaviour
{
	const int SPACE_WIDTH = 90;//行间距
	const int SPACE_HEIGHT = -100;//宽间距
	const int LINE_NUM = 4;		//一行的数量
 
	
	Transform _trans;
	Transform _awardTemp;			//奖励模板
	Transform _awardList;			//奖励列表
	Transform _btn;			//奖励按钮
	
	void Awake ()
	{
		this._trans = this.transform;
		this._awardTemp = this._trans.Find("awardlist/award");
		this._awardList = this._awardTemp.parent;
		this._btn = this._trans.Find("btn");
		
	}
	
	void OnEnable ()
	{
		ChapterAwardManager.Instance.RegisterEvent();
		Gate.instance.registerMediator (new ChapterAwardMediator (this));
	}
	
	void OnDisable ()
	{
		ChapterAwardManager.Instance.CancelEvent();
		Gate.instance.removeMediator (MediatorName.CHAPTER_AWARD_MEDIATOR);
	}
	
	
	public 	void ShowInfo(BetterList<ItemInfo> itemList,bool isCanTake){
		
		for (int i = 0; i < itemList.size; i++) {
			var award = NGUITools.AddChild(this._awardList.gameObject,this._awardTemp.gameObject); //复制一个对象
			award.SetActive(true);
			UITexture awardTex = award.transform.Find("img").GetComponent<UITexture>();
			DealTexture.Instance.setTextureToIcon(awardTex, itemList[i].Item, false); //设置ICON的图片
			award.transform.Find("bgMask").GetComponent<UISprite> ().spriteName = BagManager.Instance.getItemBgByType(itemList[i].Item.quality,false);//根据品质得到背景
			var pos = _awardTemp.localPosition;
			award.transform.localPosition = new Vector3(pos.x+(i%LINE_NUM)*SPACE_WIDTH,pos.y+(i/LINE_NUM)*SPACE_HEIGHT,pos.z); //设置位置
			award.transform.Find("num").GetComponent<UILabel>().text = itemList[i].Num.ToString(); //设置数量
			award.transform.Find("img").GetComponent<BtnTipsMsg>().Iteminfo = new ItemInfo(itemList[i].Id,0,0);
		}
		
		if (isCanTake) {
			this._btn.GetComponent<MouseAction>().enabled =true;
			this._btn.GetComponent<BoxCollider>().enabled = true;
			this._btn.Find("Sprite").GetComponent<UISprite>().spriteName = SourceManager.Instance.GetCommonButton1SpriteNameByStatus(true);
		}else{
			this._btn.GetComponent<MouseAction>().enabled =false;
			this._btn.GetComponent<BoxCollider>().enabled = false;
			this._btn.Find("Sprite").GetComponent<UISprite>().spriteName = SourceManager.Instance.GetCommonButton1SpriteNameByStatus(false);
		}
		
	}
	
	
	
}
