using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using manager;
using model;

public class BtnShopMsg : MonoBehaviour {
	
	private float _delayTime = .5f;		//长按多少时间启动
	private float _smoothTime = .1f;	//长按加减值时候的平滑时间
	private float _lastTime = 0;
	private bool _isPress = false;
	
    const string Btn_Buy = "Button_Buy";            //点击购买列表中的商品
    const string Btn_Add = "Button_Add";            //添加数量
    const string Btn_Minus = "Button_Minus";        //减少数量
    const string Btn_Max = "Button_Max";            //最大数量
    const string Btn_BuyItem = "Button_BuyItem";    //购买当前物品
    const string Btn_Cancel = "Button_Cancel";      //取消购买当前物品
    const string Btn_Close = "Btn_Close";
    const string Table1="Table1";
    const string Table2 = "Table2";
    const string Table3 = "Table3";
    const string Table4 = "Table4";
    const string Table5 = "Table5";
    const string AddRMB = "Button_AddMoney";
    void OnClick()
    {
        switch (gameObject.name)
        {
            case Btn_Close:
                UIManager.Instance.closeWindow(UiNameConst.ui_shop);
                break;
            case Btn_Buy:
                int id = gameObject.transform.parent.gameObject.GetComponent<ShopDisplayItem>().Id;
                Gate.instance.sendNotification(MsgConstant.MSG_SHOP_SET_BUY_ITEM, id);
                break;
            case Btn_Add:
                Gate.instance.sendNotification(MsgConstant.MSG_SHOP_SET_BUY_COUNT,
                    ShopManager.Instance.SelectItemBuyCount + 1);
                break;
            case Btn_Minus:
                Gate.instance.sendNotification(MsgConstant.MSG_SHOP_SET_BUY_COUNT,
                    ShopManager.Instance.SelectItemBuyCount - 1);
                break;
            case Btn_Max:
                Gate.instance.sendNotification(MsgConstant.MSG_SHOP_SET_BUY_COUNT,
                    ShopManager.Instance.SelectItemBuyCount + 99);
                break;
            case Btn_BuyItem:
                Gate.instance.sendNotification(MsgConstant.MSG_SHOP_BUY_PANEL_OPTION, true);
                break;
            case Btn_Cancel:
                Gate.instance.sendNotification(MsgConstant.MSG_SHOP_BUY_PANEL_OPTION, false);
                break;
            case Table1:
                Gate.instance.sendNotification(MsgConstant.MSG_SHOP_TABLE_SWTING, SellShopType.HotSell);
                break;
            case Table2:
                Gate.instance.sendNotification(MsgConstant.MSG_SHOP_TABLE_SWTING, SellShopType.Equip);
                break;
            case Table3:
                Gate.instance.sendNotification(MsgConstant.MSG_SHOP_TABLE_SWTING, SellShopType.Item);
                break;
            case Table4:
                Gate.instance.sendNotification(MsgConstant.MSG_SHOP_TABLE_SWTING, SellShopType.Shuijing);
                break;
            case Table5:
                Gate.instance.sendNotification(MsgConstant.MSG_SHOP_TABLE_SWTING, SellShopType.Diamon);
                break;
            case AddRMB:
                Gate.instance.sendNotification(MsgConstant.MSG_SHOP_ADD_RMB);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 购买物品提交
    /// </summary>
    public void OnBuyCountSubmit()
    {
        Gate.instance.sendNotification(MsgConstant.MSG_SHOP_SET_BUY_COUNT, GetComponent<UIInput>().value);
    }
	
	
	
	void Update ()
	{
		if (!_isPress) {
			return ;
		}
		
		if (Time.time - _lastTime < _delayTime) {  
			return ;
		}									//如果长按则执行下面的操作
		
		switch (gameObject.name) {
		case Btn_Minus:
			Gate.instance.sendNotification(MsgConstant.MSG_SHOP_SET_BUY_COUNT,
                    ShopManager.Instance.SelectItemBuyCount - 1);
			break;
		case Btn_Add:
			 Gate.instance.sendNotification(MsgConstant.MSG_SHOP_SET_BUY_COUNT,
                    ShopManager.Instance.SelectItemBuyCount + 1);
			break;
		default:
			break;
		}
		_lastTime += this._smoothTime;
	}
	
	void OnPress (bool isPress)
	{
		if (isPress) {			//按下的时候开始赋状态
			_lastTime = Time.time;
			this._isPress = true;
		} else {
			_lastTime = 0;		//放开的时候还原值
			this._isPress = false;
		}
	}
	
	void OnDisable(){
		this.OnPress(false);
		this.transform.localScale = Vector3.one;
	}
	
	
	
	
	
}
