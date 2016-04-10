/**该文件实现的基本功能等
function: 实现哥布林任务功能
author:zyl
date:2014-3-11
**/
using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using mediator;
using manager;
using model;

public class GoblinView : MonoBehaviour
{
	const string GOBLIN_MODEL = "Model/prefab/CharacterModel/huangjingebulin_UI";
	private UILabel _times;//剩下的使用次数
	private UILabel _canBuyNum;//可以购买的次数
	private UILabel _diamondNum;//购买需要的钻石价格
	private GameObject _model;//显示的模型
  
	void Awake ()
	{
		this._times = transform.Find ("middle/right/btn/enter/times").GetComponent<UILabel> ();
		this._canBuyNum = transform.Find ("middle/right/btn/buy/buyinfo").GetComponent<UILabel> ();
		this._diamondNum = transform.Find ("middle/right/btn/buy/diamondNum").GetComponent<UILabel> ();
	}

	void HandleDialogCancel (eDialogSureType type)
	{
		_model.SetActive (true);
	}

	 

	void HandleDialogSure (eDialogSureType type)
	{ 
		if (type == eDialogSureType.ePurchaseGoldenGoblinTicket) { 
			MessageManager.Instance.SendAskBuyGoldenGoblinTimes ();
		}
	}
	
	void OnEnable ()
	{
		Gate.instance.registerMediator (new GoblinMediator (this));
		EventDispatcher.GetInstance ().DialogSure += HandleDialogSure;
		EventDispatcher.GetInstance ().DialogCancel += HandleDialogCancel;
	}
	
	void OnDisable ()
	{
		Gate.instance.removeMediator (MediatorName.GOBLIN_MEDIATOR);
		EventDispatcher.GetInstance ().DialogSure -= HandleDialogSure;
		EventDispatcher.GetInstance ().DialogCancel -=  HandleDialogCancel;
		NPCManager.Instance.createCamera (false); //消除3D相机
	}
	
	//显示哥布林模型
	public void ShowGoblinModel ()
	{
		if (this._model ==null)
		{
		    if (BundleMemManager.Instance.isTypeInCache(EBundleType.eBundleUIEffect))
		    {
                BundleMemManager.Instance.loadPrefabViaWWW<GameObject>(EBundleType.eBundleUIEffect, GOBLIN_MODEL,
		            (asset) =>
		            {                      
                        _model = BundleMemManager.Instance.instantiateObj(asset, Vector3.zero, Quaternion.identity);
                        _model.name = "huangjingebulin_UI";
                        ModelPos modelPos = ConfigDataManager.GetInstance().getModelPos().getModelInfo(1001);
                        NPCManager.Instance.ModelCamera.fieldOfView = modelPos.cameraView;
                        _model.transform.parent = NPCManager.Instance.ModelCamera.transform;
                        _model.transform.localPosition = modelPos.modelPos;
                        _model.transform.localScale = Vector3.one;
                        _model.transform.localRotation = Quaternion.Euler(modelPos.modelRolate);
                        ToolFunc.SetLayerRecursively(_model, LayerMask.NameToLayer("TopUI"));
		            });
		    }
		    else
		    {
		        GameObject asset = BundleMemManager.Instance.getPrefabByName(GOBLIN_MODEL, EBundleType.eBundleUIEffect);
		        _model = BundleMemManager.Instance.instantiateObj(asset, Vector3.zero, Quaternion.identity);
		        _model.name = "huangjingebulin_UI";
		        ModelPos modelPos = ConfigDataManager.GetInstance().getModelPos().getModelInfo(1001);
		        NPCManager.Instance.ModelCamera.fieldOfView = modelPos.cameraView;
		        _model.transform.parent = NPCManager.Instance.ModelCamera.transform;
		        _model.transform.localPosition = modelPos.modelPos;
		        _model.transform.localScale = Vector3.one;
		        _model.transform.localRotation = Quaternion.Euler(modelPos.modelRolate);
		        ToolFunc.SetLayerRecursively(_model, LayerMask.NameToLayer("TopUI"));
		    }
		}else{
			_model.SetActive (true);
		}
		
	}
	
	
	//更新可以进入哥布林副本的次数
	public void UpdateGoblinRemainTimes (uint remainTimes, uint todayBuy)
	{
		this._times.text = "可使用" + remainTimes + "次";
	}
	
	//进入哥布林副本
	public void EnterGoblin ()
	{
		MessageManager.Instance.SendAskEnterGoldenGoblin ();
	}
	
	//显示错误信息
	public void ShowErr (string errMessage)
	{
		FloatMessage.GetInstance ().PlayNewFloatMessage (LanguageManager.GetText (errMessage), true, UIManager.Instance.getRootTrans());
	}
	
	//更新可购买的次数
	public void UpdateGoblinCanBuyNum (uint canBuyNum)
	{
		this._canBuyNum.text = "可购买" + canBuyNum + "次";
	}
	
	//更新购买哥布林副本需要的钻石数量
	public void UpdateGoblinBuyPrice (uint price)
	{
		this._diamondNum.text = "X" + price;
	}
	
	//清空购买的价格显示
	public void UpdateClearGoblinBuyPrice ()
	{
		this._diamondNum.text = "";
	}
	
	public void ShowBuyTimesDialog (uint price)
	{
		UIManager.Instance.ShowDialog (eDialogSureType.ePurchaseGoldenGoblinTicket,
                    string.Format (LanguageManager.GetText ("msg_buy_golden_goblin_ticket"), price));
					
		// here we should keep golden goblin hide
		_model.SetActive (false);
 	
			
	}
	
	
	
}
