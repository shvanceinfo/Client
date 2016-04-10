/**该文件实现的基本功能等
function: 实现新物品界面
author:zyl
date:2014-5-9
**/
using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using mediator;
using manager;
using model;
using System.Collections.Generic;

public class NewItemView : MonoBehaviour
{
	public int MaxCount = 10;
	public float aniTime1 = 0.3f;
	public float aniTime2 = 0.7f;
	public float aniTime3 = 0.5f;
	public float firstSectionVal = 0.3f;

//	const int speed = 50;
//	bool isFirst = true;


	Transform _trans;
	Transform _itemTemp;
	Transform _playerIcon;

	#region 显示的参数
	bool isPlaying = false;
	Queue<ItemInfo> itemQueue = new Queue<ItemInfo> ();
	Dictionary<ItemInfo,GameObject> igList = new Dictionary<ItemInfo, GameObject> ();
	int Length;
	#endregion

 	
	Vector3 playerLocationPos = Vector3.zero;  //头像位置
	Vector3 itemTempPos;						//模板位置
	Vector3 firstSection;                      //第一个节点的位置

	void Awake ()
	{
		this._trans = this.transform;
		this._itemTemp = this._trans.Find ("itemTemp");
		this._playerIcon = UIManager.Instance.Camera2D.transform.FindChild ("ui_main/top_left/player/playerInfo/player");
		this.itemTempPos = this._itemTemp.localPosition;
	}



	void OnEnable ()
	{
		Gate.instance.registerMediator (new NewItemMediator (this));
	}
	
	void OnDisable ()
	{	 
		Gate.instance.removeMediator (MediatorName.NEWITEM_MEDIATOR);
	}
	
	public void Show (IList<ItemInfo> itemList)
	{
		if (Length==MaxCount) {
			return;
		}
		this._itemTemp.gameObject.SetActive (true);
		for (int i = 0,max = itemList.Count; i < max; i++) {
			itemQueue.Enqueue(itemList[i]);
			Length++;
			GameObject newItem = NGUITools.AddChild (this._itemTemp.parent.gameObject, this._itemTemp.gameObject);
			Transform newItamTrans = newItem.transform;
			newItamTrans.localPosition = new Vector3 (0, this._itemTemp.localPosition.y, 0);
			newItamTrans.Find ("bg2").GetComponent<UISprite> ().spriteName = BagManager.Instance.getItemBgByType (itemList[i].Item.quality, false);
			UITexture itemTex = newItem.transform.Find ("item").GetComponent<UITexture> ();
			DealTexture.Instance.setTextureToIcon (itemTex, itemList[i].Item);
			newItamTrans.Find ("num").GetComponent<UILabel> ().text = itemList[i].Num.ToString ();
			igList.Add (itemList[i], newItem);
			if (Length==MaxCount) {
				break;
			}
		}
		   
		itemList.Clear ();
		this._itemTemp.gameObject.SetActive (false);
	}
	
	void Update ()
	{
		if (this.isPlaying == false && itemQueue.Count > 0) {
			this.isPlaying = true;
			
			this.PlayAnimation ();
		}
	}
	
	void PlayAnimation ()
	{
		//		lenth /= speed;
		if (this.playerLocationPos == Vector3.zero) {
			this.playerLocationPos = UIManager.Instance.Camera2D.transform.InverseTransformPoint (this._playerIcon.position);
			this.firstSection = (this.playerLocationPos - this.itemTempPos) * firstSectionVal;	   //得到距离的0.3位置的值,飞行的第一段距离
		}



//		if (isFirst) {
//			yield return new WaitForSeconds(1f);
//			isFirst = false;
//		}
		var item = itemQueue.Dequeue();//移出队列
		Transform itemTrans = igList [item].transform;
		igList.Remove(item);//移出键值对
//		for (int j = 0; j < speed; j++) {
//			if (speed/5 == j) {
//				this.isPlaying = false;
//			}
//			itemTrans.localPosition += lenth;
//			yield return 1;
//		}
		
//		UIManager.Instance.Camera2D.transform.FindChild("ui_main/top_left/player/playerInfo/player").position
		TweenPosition tp = UITweener.Begin<TweenPosition> (itemTrans.gameObject, aniTime1); //动画1
		tp.to = this.itemTempPos + this.firstSection;
		tp.SetOnFinished (new EventDelegate (() => {
			this.isPlaying = false;  //开始下个新道具的动画
			
			
			TweenPosition tp2 = UITweener.Begin<TweenPosition> (itemTrans.gameObject, aniTime2); //动画2
			tp2.from = tp.to;
			tp2.to = playerLocationPos;
			tp2.SetOnFinished (new EventDelegate (() => {
				  
				TweenScale ts = UITweener.Begin<TweenScale> (itemTrans.gameObject, aniTime3); //动画3
				ts.SetOnFinished (new EventDelegate (() => {
					Length--;
					if (Length == 0) {
						UIManager.Instance.closeWindow (UiNameConst.ui_new_item, true, false);
					}
					Destroy (itemTrans.gameObject);
				}));
 
			}));
 
		}));
		
 
	}
 
 
	
}
