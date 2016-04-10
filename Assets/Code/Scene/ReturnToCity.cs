/**该文件实现的基本功能等
function:人物死亡，战斗胜利，或者计时时间结束回到主城的相关表现
author:ljx
date:2013-11-26
**/
using UnityEngine;
using System.Collections;

public class ReturnToCity : MonoBehaviour 
{
	private static ReturnToCity _instance;
	//private static GameObject _selfObj; //整个控件的对象
	
	public enum RETURN_TYPE 
	{
		WIN_BATTLE = 0,
		LOSE_BATTLE,
		PASS_DEVIL,
		LOSE_DEVIL,
	    TIME_OUT
	} 
	
	private UILabel _dieMsg;	//游戏中间的消息
	private UILabel _floatMsg;  //倒计时消息
	private GameObject _reviveObj; //复活的面板只在哪个有效
	private GameObject _commonBack; //正常的退出面板
	private RETURN_TYPE _returnType;
	private string _msgPrefix;
	private	string _msgSuffix;
	private string _timeColor;
	private int _countSecond; //倒计时的时间
	
	void Awake()
	{
		_instance = this;
		_dieMsg = GameObject.Find("dieMsg").GetComponent<UILabel>();
		_floatMsg = GameObject.Find("floatMsg").GetComponent<UILabel>();
		_reviveObj = GameObject.Find("revive");
//		_commonBack = GameObject.Find("commonBack");
		_msgPrefix = LanguageManager.GetText("return_to_city_prefix");
		_msgSuffix = LanguageManager.GetText("return_to_city_suffix");
		_timeColor = LanguageManager.GetText("consume_color");
		_floatMsg.alpha = 0f;
		_dieMsg.alpha = 0f;
	}
	
	void Start () 
	{
		
	}
	
	//开始倒计时
	void countdown()
	{
//		if(!_floatMsg.gameObject.activeSelf)
//			_floatMsg.gameObject.SetActive(true);
//		_floatMsg.text = _msgPrefix + _timeColor + _countSecond.ToString() + Constant.COLOR_END + _msgSuffix;
//        TweenAlpha alpha = UITweener.Begin<TweenAlpha>(_floatMsg.gameObject, 1.5f);
//        alpha.duration = 1.5f;
//        alpha.from = 1f;
//        alpha.to = 0f;
//        TweenPosition tween = UITweener.Begin<TweenPosition>(_floatMsg.gameObject, 2f);
//        tween.method = UITweener.Method.EaseInOut; 
//        tween.from = new Vector3(0f, 0f, 0f);
//        tween.to = new Vector3(0f, 200f, 0f);
//        if(_countSecond > 0)
//        {
//	        tween.eventReceiver = gameObject;
//	        tween.callWhenFinished = "countdown";
//	        _countSecond--;
//        }
//        else
//        {
//        	MessageManager.Instance.sendMessageReturnCity();	 //播放完毕才开始真正回主城
//        	controlCamera(false);
//        }
	}
	
	//控制相机变色图像
	void controlCamera(bool isAdd)
	{
		if(isAdd)
		{
			StartCoroutine("changeCarema");
		}
		else
		{
			//Destroy(_selfObj); //释放自身的内存
		}
	}
	
	//淡入死亡或复活信息
	void tweenMsg()
	{
        TweenAlpha tweenAlpha = UITweener.Begin<TweenAlpha>(_dieMsg.gameObject, 0f);
        tweenAlpha.method = UITweener.Method.Linear;
        tweenAlpha.duration = 0.5f;
        tweenAlpha.from = 0f;
        tweenAlpha.to = 1f;
		_floatMsg.text = _msgPrefix + _timeColor + _countSecond.ToString() + Constant.COLOR_END + _msgSuffix;
		TweenAlpha floatAlpha = UITweener.Begin<TweenAlpha>(_floatMsg.gameObject, 0f);
        floatAlpha.method = UITweener.Method.Linear;
        floatAlpha.duration = 0.5f;
        floatAlpha.from = 0f;
        floatAlpha.to = 1f;
        StartCoroutine(countDownQuit());
	}
	
	//淡入淡化相机
	IEnumerator changeCarema()
	{
		while (true) 
		{	
			yield return new WaitForSeconds(0.1f);
			bool end = false;			
			foreach (Camera camera in Camera.allCameras) 
			{
				if(camera.name != "dieCarema")
				{
					ColorCorrectionCurves colorCurve = camera.GetComponent("ColorCorrectionCurves") as ColorCorrectionCurves;					
					if(colorCurve != null)
					{
						colorCurve.saturation -= 0.05f;
						if(colorCurve.saturation <= 0f)
						{
							colorCurve.saturation = 0f;
							end = true;
						}
					}
					else
						end = true;
				}
			}
			if(end)
			{
				StopCoroutine("changeCarema");
				break;
			}	
		}
	}
	
	//倒计时退出
	IEnumerator countDownQuit()
	{
		while (_countSecond >= 0) 
		{
			_floatMsg.text = _msgPrefix + _timeColor + _countSecond.ToString() + Constant.COLOR_END + _msgSuffix;
			if(_countSecond <= 0)
			{
				UIManager.Instance.showWaitting(true); //回主城强制显示窗口
				MessageManager.Instance.sendMessageReturnCity();	 //播放完毕才开始真正回主城
				controlCamera(false);
				StopAllCoroutines();
			}
			yield return new WaitForSeconds(1f);
			_countSecond--;
		}
	}

	//getter and setter
	public static ReturnToCity Instance
	{
		get 
		{ 
			return _instance; 
		}
	}
	
	public ReturnToCity.RETURN_TYPE ReturnType
	{
		set 
		{ 
			_returnType = value; 
			_countSecond = 5;
//			controlCamera(true); //不管什么情况都控制相机
			switch (_returnType) 
			{
				case RETURN_TYPE.WIN_BATTLE:
				case RETURN_TYPE.PASS_DEVIL:
					_dieMsg.text = LanguageManager.GetText("pass_success");
					break;
				case RETURN_TYPE.LOSE_BATTLE:
				case RETURN_TYPE.LOSE_DEVIL:
					_dieMsg.text = LanguageManager.GetText("pass_fialure");
					break;
				case RETURN_TYPE.TIME_OUT:
					_dieMsg.text = LanguageManager.GetText("pass_timeout");
					break;
				default:				
					break;
			}
			tweenMsg();
//			if(_returnType != RETURN_TYPE.LOSE_BATTLE) //不是关卡失败，弹出复活框
//			{
//				_reviveObj.SetActive(false);
//				_commonBack.SetActive(true);
//			}
//			else
//			{
//				_reviveObj.SetActive(true);
//				_commonBack.SetActive(false);
//				tweenDieMsg();
//			}
		}
	}
}
