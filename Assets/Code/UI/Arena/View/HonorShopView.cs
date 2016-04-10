/**该文件实现的基本功能等
function: 荣誉商店的视图控制
author:ljx
date:2013-11-09
**/
using UnityEngine;
using System.Collections;
using manager;
using model;
using mediator;
using MVC.entrance.gate;

public class HonorShopView : MonoBehaviour
{
	const int PER_ROW_HEIGHT = 205;			//每行高度
	const int PER_ROW_NUM = 3; 				//每行数目
//	const string ACTIVE_SP = "tabBtn"; 		//标签激活状态的Sprite
//	const string INACTIVE_SP = "tabBtnBg"; 	//非激活状态的Sprite
	
	private UISprite _equipTab; //装备标签
	private UISprite _toolTab; //道具标签
	private UISprite _diamondTab; //宝石标签
	private UISprite _otherTab; //其他标签
	private GameObject _shopTemplate; //装备商店的行模板
	private UILabel _currentHonor; //当前荣誉值
	private BetterList<HonorItem> _equipItems; //装备的物品
	private BetterList<HonorItem> _toolItems; //道具的物品
	private BetterList<HonorItem> _diamondItems; //宝石的物品
	private BetterList<HonorItem> _otherItems; //其他的物品
	private eHonorItemType _currentType; //当前显示的物品类型
	
	void Awake () 
	{
		_equipTab = transform.Find("middle/tab/equip/btnBg").GetComponent<UISprite>();
		_toolTab = transform.Find("middle/tab/tools/btnBg").GetComponent<UISprite>();
		_diamondTab = transform.Find("middle/tab/diamond/btnBg").GetComponent<UISprite>();
		_otherTab = transform.Find("middle/tab/other/btnBg").GetComponent<UISprite>();
		_shopTemplate = transform.Find("middle/showList/dragPanel/rowTemplate").gameObject;
		_currentHonor = transform.Find("bottom/ownHonor/num").GetComponent<UILabel>();
		_equipItems = new BetterList<HonorItem>();
		_toolItems = new BetterList<HonorItem>();
		_diamondItems = new BetterList<HonorItem>();
		_otherItems = new BetterList<HonorItem>();
		_currentType = eHonorItemType.equip;
		initHonorShop();
	}
	
	void OnEnable()
	{
		Gate.instance.registerMediator(new HonorShopMediator(this));
	}
	
	void OnDisable()
	{
		Gate.instance.removeMediator(MediatorName.HONOR_SHOP_MEDIATOR);
	}
	
	//初始英雄榜商店
	void initHonorShop()
	{
		BetterList<HonorItem> items = ArenaManager.Instance.ArenaVo.HonorItemList;
		int len = items.size;
		if(len > 0)
		{
			bool setTab = true;
			for(int i=0; i<len; i++)
			{
				HonorItem item = items[i];
				HonorItem configItem = ArenaManager.Instance.HonorShopHash[item.id] as HonorItem;
				configItem.itemPrice = item.itemPrice;
				switch (configItem.type) 
				{
					case eHonorItemType.equip:
						_equipItems.Add(configItem);
						break;
					case eHonorItemType.tools:
						_toolItems.Add(configItem);
						break;
					case eHonorItemType.diamond:
						_diamondItems.Add(configItem);
						break;
					case eHonorItemType.other:
						_otherItems.Add(configItem);
						break;
					default:
						break;
				}
				if(setTab)
				{
					_currentType = configItem.type;
					setTab = false;
				}
			}
		}
		_currentHonor.text = ArenaManager.Instance.ArenaVo.ArenaInfo.currentHonor.ToString();
		switchTab(_currentType, false);
	}
	
	//切换物品的标签， isClick是否点击触发，默认都是点击触发
	public void switchTab(eHonorItemType newType, bool isClick = true)
	{
		if(isClick && _currentType == newType) //原来已经在这个标签，就不切换
			return;
		_currentType = newType;
		BetterList<HonorItem> items = null;
		switch(_currentType)
		{
			case eHonorItemType.equip:
//				_equipTab.spriteName = ACTIVE_SP;
//				_toolTab.spriteName = INACTIVE_SP;
//				_diamondTab.spriteName = INACTIVE_SP;
//				_otherTab.spriteName = INACTIVE_SP;
                //_equipTab.gameObject.SetActive(true);
                //_toolTab.gameObject.SetActive(false);
                //_diamondTab.gameObject.SetActive(false);
                //_otherTab.gameObject.SetActive(false);
				items = _equipItems;
				break;
			case eHonorItemType.tools:
//				_equipTab.spriteName = INACTIVE_SP;
//				_toolTab.spriteName = ACTIVE_SP;
//				_diamondTab.spriteName = INACTIVE_SP;
//				_otherTab.spriteName = INACTIVE_SP;
                //_equipTab.gameObject.SetActive(false);
                //_toolTab.gameObject.SetActive(true);
                //_diamondTab.gameObject.SetActive(false);
                //_otherTab.gameObject.SetActive(false);
				items = _toolItems;
				break;
			case eHonorItemType.diamond:
//				_equipTab.spriteName = INACTIVE_SP;
//				_toolTab.spriteName = INACTIVE_SP;
//				_diamondTab.spriteName = ACTIVE_SP;
//				_otherTab.spriteName = INACTIVE_SP;
                //_equipTab.gameObject.SetActive(false);
                //_toolTab.gameObject.SetActive(false);
                //_diamondTab.gameObject.SetActive(true);
                //_otherTab.gameObject.SetActive(false);
				items = _diamondItems;
				break;
			case eHonorItemType.other:
//				_equipTab.spriteName = INACTIVE_SP;
//				_toolTab.spriteName = INACTIVE_SP;
//				_diamondTab.spriteName = INACTIVE_SP;
//				_otherTab.spriteName = ACTIVE_SP;
                //_equipTab.gameObject.SetActive(false);
                //_toolTab.gameObject.SetActive(false);
                //_diamondTab.gameObject.SetActive(false);
                //_otherTab.gameObject.SetActive(true);
				items = _otherItems;
				break;
			default:
				break;	
		}	
		Transform trans = _shopTemplate.transform;
		_shopTemplate.SetActive(true);
		for(int i=1; i<=PER_ROW_NUM; i++)
		{
			BoxCollider collider = trans.Find("item"+i+"/buyItem").GetComponent<BoxCollider>();
			collider.enabled = true;
		}
		foreach (Transform child in trans.parent)  //先清除原来的列表
		{
			if(child != trans)
				GameObject.Destroy(child.gameObject); 
		}
		if(items != null && items.size > 0)
		{
			int len = items.size;
			int row = len/PER_ROW_NUM; 		//要显示的行数
			int lastColNum = len%PER_ROW_NUM; 	//最后一行的剩余数目
			int counter = 0;
			for(int i=0; i<=row; i++)
			{
				HonorItem item = items[counter];
				GameObject go = NGUITools.AddChild(trans.parent.gameObject, _shopTemplate);
				Transform childTrans = go.transform;
				childTrans.localPosition = new Vector3(trans.localPosition.x, trans.localPosition.y - i*PER_ROW_HEIGHT, trans.localPosition.z);
				childTrans.localScale = new Vector3(trans.localScale.x, trans.localScale.y, trans.localScale.z);
				if(i < row || lastColNum == 0)  //最后一行剩余数目为零，也是满格显示
				{
					for(int j=1; j<=PER_ROW_NUM; j++)
					{					
						setEachItem(item, childTrans.Find("item"+j));
						counter++;
					}
				}
				else
				{
					for(int j=1; j<=lastColNum; j++)
					{
						setEachItem(item, childTrans.Find("item"+j));
						counter++;
					}
					for(int j=lastColNum+1; j<=PER_ROW_NUM; j++) //后面没有的物品隐藏
					{
						childTrans.Find("item"+j).gameObject.SetActive(false);
						BoxCollider collider = childTrans.Find("item"+j+"/buyItem").GetComponent<BoxCollider>();
						collider.enabled = false;
					}
				}
			}
		}
		_shopTemplate.SetActive(false);
		for(int i=1; i<=PER_ROW_NUM; i++)
		{
			BoxCollider collider = _shopTemplate.transform.Find("item"+i+"/buyItem").GetComponent<BoxCollider>();
			collider.enabled = false;
		}
	}
	
	//设置每一个商品的信息
	private void setEachItem(HonorItem item, Transform trans)
	{
		trans.Find("name").GetComponent<UILabel>().text = item.itemName;		
		UITexture texture = trans.Find("icon/Texture").GetComponent<UITexture>();
		ItemTemplate item1 = ItemManager.GetInstance().GetTemplateByTempId(item.itemID);
		DealTexture.Instance.setTextureToIcon(texture, item1, false);
		trans.Find("consume/consumeNum").GetComponent<UILabel>().text = item.itemPrice.ToString();
		trans.name = item.itemID.ToString(); //重置名称给购买物品使用
	}
}
