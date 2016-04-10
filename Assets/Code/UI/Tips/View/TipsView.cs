using UnityEngine;
using System.Collections;
using manager;

public class TipsView : MonoBehaviour
{
	float offsetY = 10;
	protected Transform _trans;
	UISprite _bg;			//背景
	protected void Awake ()
	{
		this._trans = this.transform;
		this._bg = this._trans.Find ("bg/bg").GetComponent<UISprite> ();
	}


	/// <summary>
	/// 确认tips需要隐藏否
	/// </summary>
	/// <param name='currentObj'>
	/// 当前tips 对象
	/// </param>
	public  void CheckShowOrHide ()
	{
		if (Input.GetButtonDown ("Fire1")) {
			if (UICamera.hoveredObject == null) {
				TipsManager.Instance.CloseAllTipsUI (); //如果点击的不是tips区域，则需要隐藏tips
			} else if (UICamera.lastHit.collider != null) {
				Transform clickObj = UICamera.lastHit.collider.transform;//点击的对象
				do {
					if (clickObj.name == "ui_sale" || clickObj.name == "ui_open") {
						return;
					}
					if (clickObj.gameObject == this.gameObject) {       //如果点击的是tips区域则跳出循环
						return;
					}
					clickObj = clickObj.parent;
				} while (clickObj.parent!=null); 
				TipsManager.Instance.CloseAllTipsUI (); //如果点击的不是tips区域，则需要隐藏tips
			}
		}
	}

	  
	/// <summary>
	/// 设置位置
	/// </summary>
	public void SetPosition ()
	{
		this._trans.position = TipsManager.Instance.IconTrans.position;
		Vector3 screenPoint = UICamera.current.camera.WorldToScreenPoint (this._trans.position);

		Vector2 world2dZero = new Vector2 (Screen.width/2,Screen.height/2);  //屏幕坐标中心点

#if UNITY_EDITOR

		Vector2 bgSize = new Vector2 (Screen.width*this._bg.localSize.x/1024,this._bg.localSize.y*Screen.width/768);


//		print (Screen.GetResolution[0].width);
//		print (screenPoint);
//		print (Screen.width + " " +Screen.height);
//		print (world2dZero);
//		print (bgSize); 

		//判断象限
		if (screenPoint.x>world2dZero.x && screenPoint.y>world2dZero.y) { //第一象限
			screenPoint += new Vector3 (-this._bg.localSize.x / 2, -this._bg.localSize.y / 2,0);
		}else if (screenPoint.x<world2dZero.x && screenPoint.y>world2dZero.y){ //第二象限
			screenPoint += new Vector3 (this._bg.localSize.x / 2, -this._bg.localSize.y / 2,0);
		}else if(screenPoint.x<world2dZero.x && screenPoint.y< world2dZero.y ){ //第三象限
			screenPoint += new Vector3 (this._bg.localSize.x / 2, this._bg.localSize.y / 2,0);
		}else{																	//第四象限
			screenPoint += new Vector3 (-this._bg.localSize.x / 2, this._bg.localSize.y / 2,0);
		}
#else
		//判断象限
		if (screenPoint.x>world2dZero.x && screenPoint.y>world2dZero.y) { //第一象限
			screenPoint += new Vector3 (-this._bg.localSize.x / 2, -this._bg.localSize.y / 2,0);
		}else if (screenPoint.x<world2dZero.x && screenPoint.y>world2dZero.y){ //第二象限
			screenPoint += new Vector3 (this._bg.localSize.x / 2, -this._bg.localSize.y / 2,0);
		}else if(screenPoint.x<world2dZero.x && screenPoint.y< world2dZero.y ){ //第三象限
			screenPoint += new Vector3 (this._bg.localSize.x / 2, this._bg.localSize.y / 2,0);
		}else{																	//第四象限
			screenPoint += new Vector3 (-this._bg.localSize.x / 2, this._bg.localSize.y / 2,0);
		}
#endif
 
 

		if ((screenPoint.y + this._bg.localSize.y / 2+offsetY) >= Screen.height) { //超过高度的最大值
			screenPoint.y = Screen.height - this._bg.localSize.y / 2 - 40 ;
		}
		if ((screenPoint.y - this._bg.localSize.y / 2-offsetY) <= 0) { //低于高度的最小值
			screenPoint.y = this._bg.localSize.y / 2 + 40;
		} 
		if ((screenPoint.x + this._bg.localSize.x / 2) >= Screen.width) { //超过宽度的最大值
			screenPoint.x = Screen.width - this._bg.localSize.x / 2;
		}
		if ((screenPoint.x - this._bg.localSize.x / 2) <= 0) { //低于宽度的最小值
			screenPoint.x = this._bg.localSize.x / 2;
		}

		
		this._trans.position = UICamera.current.camera.ScreenToWorldPoint (screenPoint); //修正后的位置
		
		
	}

}
