/**该文件实现的基本功能等
function:实现图标闪烁
author:ljx
date:2013-11-12
**/
using UnityEngine;
using System.Collections;
using model;
using manager;

public class GlinkNotify : MonoBehaviour 
{
	private bool _addAlpha = false;
	private UISprite _notifySp;
	
	void Awake()
    {
        _addAlpha = false;
        _notifySp = gameObject.GetComponent<UISprite>();
        //switch (EmailState)
        //{
        //    default:
        //        break;
        //}

        EmailManager.Instance.RequestEmailList();

	}
	
	public void activeNotify(bool isActive)
    {
		if(_notifySp == null)
			_notifySp = gameObject.GetComponent<UISprite>();
    	if(isActive)
    	{
            _notifySp.active = true; 
            StopCoroutine("GlintNotify");
    		StartCoroutine("GlintNotify");
    		_addAlpha = true;
    	}
    	else
    	{
    		_notifySp.active = false;
    		StopCoroutine("GlintNotify");
    		_addAlpha = false;
    	}
    }
	 
	IEnumerator GlintNotify()
	{
		while (true) 
		{
			yield return new WaitForSeconds(Constant.BLINK_EMAIL_BUTTON/4);
			if(_addAlpha)
			{
				if(_notifySp.alpha >=1)
					_addAlpha = false;
				else
					_notifySp.alpha += 3*Constant.BLINK_EMAIL_BUTTON;
			}
			else
			{
				if(_notifySp.alpha <=0)
					_addAlpha = true;
				else
					_notifySp.alpha -= 3*Constant.BLINK_EMAIL_BUTTON;
			}
		}
	}

}
