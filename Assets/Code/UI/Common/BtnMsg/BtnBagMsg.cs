/**该文件实现的基本功能等
function: 实现按钮点击的消息传送
author:zyl
date:2014-4-8
**/
using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using manager;

public class BtnBagMsg : MonoBehaviour
{
	public const string CLOSE_BAG = "close_bag";
	public const string TAB1 = "tab1";
	public const string TAB2 = "tab2";
	public const string TAB3 = "tab3";
	public const string SALE = "sale";
	public const string TOGGLE = "Toggle"; //是否出售选择框
	public const string ROLE = "eat";
	
	Transform _trans;
	
	void Awake ()
	{
		this._trans = this.transform;
	}
	
	void OnClick ()
	{
		switch (gameObject.name) {
		case CLOSE_BAG:
			Gate.instance.sendNotification (MsgConstant.MSG_CLOSE_UI);//关闭界面
			NPCManager.Instance.createCamera (false); //消除3D相机
			break;
		case TAB1:
			Gate.instance.sendNotification (MsgConstant.MSG_BAG_SWITCHTAB, BagView.Tab.all);//切换全部
			break;
		case TAB2:
			Gate.instance.sendNotification (MsgConstant.MSG_BAG_SWITCHTAB, BagView.Tab.equip);//切换装备
			break;
		case TAB3:
			Gate.instance.sendNotification (MsgConstant.MSG_BAG_SWITCHTAB, BagView.Tab.item);//切换道具
			break;	
		case SALE:
			UILabel hiddenSaleValue = this._trans.parent.parent.Find ("itemlist/gird/hiddenSale").GetComponent<UILabel> ();//缓存需要卖掉的道具的隐藏域
 
			if (string.IsNullOrEmpty (hiddenSaleValue.text)) {
				FloatMessage.GetInstance ().PlayFloatMessage (LanguageManager.GetText ("item_sell_empty_msg"),
			                                           UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);
			} else {
				Gate.instance.sendNotification (MsgConstant.MSG_BAG_SALE, hiddenSaleValue.text);//贩卖道具
			}
			break;
			
		case TOGGLE:
			var toggle = this.GetComponent<UIToggle> (); //得到选择框
			UILabel hiddenSale = this._trans.parent.parent.parent.Find ("hiddenSale").GetComponent<UILabel> ();//缓存需要卖掉的道具的隐藏域
			string instanceid = this._trans.Find ("instanceid").GetComponent<UILabel> ().text; //得到道具的实例ID
			if (toggle.value) {  //如果选中则缓存实例ID
				hiddenSale.text += instanceid + ",";
			} else {			 //如果取消选中则删除实例ID
				hiddenSale.text = hiddenSale.text.Replace (instanceid + ",", "");
			}
			break;
		case ROLE:
			Gate.instance.sendNotification (MsgConstant.MSG_CLOSE_UI);//关闭界面
			NPCManager.Instance.createCamera (false); //消除3D相机
			RoleManager.Instance.OpenRole();
			break;
		default:
			break;
		}
		
	}
}
