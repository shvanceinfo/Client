/**该文件实现的基本功能等
function: 实现按钮点击的消息传送
author:zyl
date:2014-5-17
**/
using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using manager;

public class BtnNewItemUseMsg : MonoBehaviour
{
	public const string CLOSE = "close";
	public const string BTNEQUIP = "btnEquip";
	public const string BTNUSE = "btnUse";
	
	void OnClick ()
	{
		switch (gameObject.name) {
		case CLOSE:
			NewitemManager.Instance.NewItemUseCloseUI ();
			break;
			
		case BTNEQUIP:
			NewitemManager.Instance.QuickEquipItem();
			break;
			
		case BTNUSE:
			NewitemManager.Instance.QuickUseItem();
			break;
			
		default:
			break;
			
			
		}
		 
	}
}
