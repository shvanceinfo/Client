/**该文件实现的基本功能等
function:实现通用的窗口滑动关闭
author:ljx
date:2013-11-20
**/
using UnityEngine;
using System.Collections;

public class DragPanel:MonoBehaviour
{
	public delegate void dragCallBack();
	private GameObject _dragPanel; 	//需要拖拽的面板
	private Vector3 _origPos;		//每个窗口的原始位置
	private float _totalDeltaY;		//累积的位移量
	private float _precision;		//关闭时滑动的精度
	private bool _hasDrag;			//是否进行了拖拉操作
	private dragCallBack _callBack;
	
	//给相应物体添加拖拉事件，拖拉成功后的响应
	public void initDrag(GameObject dragObj, dragCallBack callBack, float precision = Constant.CLOSE_ECLIPSION)
	{
		_dragPanel = dragObj;
		_totalDeltaY = 0;
		_precision = precision;
		_callBack = callBack;
		_hasDrag = false;
		if(_dragPanel != null)
		{
			_origPos = _dragPanel.transform.localPosition;
			UIEventListener.Get(_dragPanel).onDrag += dragCloseWindow;
			UIEventListener.Get(_dragPanel).onPress += pressCloseWindow;
			Component[] components = _dragPanel.GetComponentsInChildren<Component>(false);
			if(components != null && components.Length > 0)
			{
				for(int i=0; i<components.Length; i++)
				{
					GameObject obj = components[i].gameObject;
					UIEventListener.Get(obj).onDrag += dragCloseWindow;
					UIEventListener.Get(obj).onPress += pressCloseWindow;
				}
			}
			Transform funcBg = _dragPanel.transform.Find("self_ui/func_bg"); //背包的背景
			if(funcBg != null)
			{
				UISprite sp = funcBg.gameObject.GetComponent<UISprite>();
				if(sp != null && sp.spriteName == "func_bg")
				{
					DealTexture.Instance.mirrorSprite(sp, DealTexture.MIRROR_POS.topRight, false, 2);
					DealTexture.Instance.mirrorSprite(sp, DealTexture.MIRROR_POS.bottomLeft, false, 2);
					DealTexture.Instance.mirrorSprite(sp, DealTexture.MIRROR_POS.bottomRight, false, 2);
				}
			}
		}
	}
	
	//移除Object上的拖拉事件
	public void removeDrag()
	{
		if(_dragPanel != null)
		{
			UIEventListener.Get(_dragPanel).onDrag -= dragCloseWindow;
			UIEventListener.Get(_dragPanel).onPress -= pressCloseWindow;
			Component[] components = _dragPanel.GetComponentsInChildren<Component>(false);
			if(components != null && components.Length > 0)
			{
				for(int i=0; i<components.Length; i++)
				{
					GameObject obj = components[i].gameObject;
					UIEventListener.Get(obj).onDrag -= dragCloseWindow;
					UIEventListener.Get(obj).onPress -= pressCloseWindow;
				}
			}			
		}
	}
		
	//响应手指拖拉
	void dragCloseWindow(GameObject obj, Vector2 vec)
	{
		_hasDrag = true;
		_totalDeltaY += vec.y;
		_dragPanel.transform.localPosition = new Vector3(_origPos.x, _origPos.y + _totalDeltaY, +_origPos.z);
		if(_totalDeltaY < _precision) //滑动一定距离关闭
		{
			_hasDrag = false;
			if(_callBack != null)
			{
				_callBack();
//				UiManager.GetInstance().showCamera(true); //刚出现就显示相机
				_callBack = null; //保证只做一次callBack
			}
		}
	}
	
	void pressCloseWindow(GameObject obj, bool press)
	{
		if(_hasDrag && !press)
		{
			_dragPanel.transform.localPosition = new Vector3(_origPos.x, _origPos.y, +_origPos.z);
			_totalDeltaY = 0;
			_hasDrag = false;
		}
	}
}
