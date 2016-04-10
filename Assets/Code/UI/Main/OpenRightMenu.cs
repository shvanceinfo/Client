using UnityEngine;
using System.Collections;

public class OpenRightMenu : MonoBehaviour 
{
	private GameObject _controlTarget; //控制的目标
//	public static bool isFuncOpen = false; //子菜单是否处于打开状态
	private bool _isOpen;
	private Vector3 _hiddenPos;
	private Vector3 _showPos;
	private bool _canClick; //点击自身不更新
	
	void Awake () 
	{
	 	Transform allFun = transform.parent.Find("allFunPanel/allFunction");
		_hiddenPos = new Vector3(400f, allFun.localPosition.y, 0f);
		_showPos = new Vector3(0, allFun.localPosition.y,0);
		_isOpen = false;
		_canClick = true;
		_controlTarget = allFun.gameObject;
		_controlTarget.transform.localPosition = _hiddenPos;
		_controlTarget.SetActive(false);
	}
	void Update () 
	{
//		if(_isOpen && !funcMgr.isChildOpen && Input.GetButtonDown("Fire1"))
//		{
//		 	if (UICamera.hoveredObject != null)
//		 	{
//	 			if(!checkClickMenu(UICamera.hoveredObject.name))
//	 			{
//			 		if(checkClickSubMenu(UICamera.hoveredObject.name))
//			 			funcMgr.isChildOpen = true;
//			 		else
//			 			close();
//	 			}
//		 	}
//		 	else if(UICamera.selectedObject != null)
//		 	{
//		 		if(!checkClickMenu(UICamera.hoveredObject.name))
//		 		{
//			 		if(checkClickSubMenu(UICamera.selectedObject.name))
//			 			funcMgr.isChildOpen = true;
//			 		else
//			 			close();
//		 		}
//		 	}
//		 	else
//		 		close();
//		 }
	}

    public void CloseMenu()
    {
        if (_isOpen)
            close();
    }
        


	void OnClick()
	{	
		if(_canClick)
		{
			if(_isOpen)
				close();
			else
				open();
			_canClick = false;
		}
	}
	
	void open()
    {
        _isOpen = true;
        _controlTarget.SetActive(true);
        TweenPosition positon = UITweener.Begin<TweenPosition>(_controlTarget, 0.3f);
        positon.from = _hiddenPos;
        positon.to = _showPos;
        positon.eventReceiver = gameObject;
        positon.callWhenFinished = "enableClick";
    }
	
	void close()
    {
        _isOpen = false;
//        isFuncOpen = false;
        foreach (Transform child in _controlTarget.transform) 
		{
			BoxCollider collider = child.GetComponent<BoxCollider>();
			if(collider != null)
				collider.enabled = false;
		}
        TweenPosition positon = UITweener.Begin<TweenPosition>(_controlTarget, 0.3f);
        positon.from = _showPos;
        positon.to = _hiddenPos;
        positon.eventReceiver = gameObject;
        positon.callWhenFinished = "hiddenObj";
    }    
	
	void hiddenObj()
	{
		_controlTarget.SetActive(false);
		_canClick = true;
	}
	
	void enableClick()
	{
		_canClick = true;
		foreach (Transform child in _controlTarget.transform) 
		{
			BoxCollider collider = child.GetComponent<BoxCollider>();
			if(collider != null)
				collider.enabled = true;
		}
	}
    
	bool checkClickMenu(string checkName)
    {
    	if(checkName.Equals("menu") || checkName.Equals("showFunc"))
    		return true;
    	return false;
    }
    
	// check the click target is the sub menu
    bool checkClickSubMenu(string checkName)
    {
//    	if(checkName.Equals("devilCave") || checkName.Equals("golden_goblin") || checkName.Equals("divine") 
//			|| checkName.Equals("dragonTreasure") || checkName.Equals("luckStar") || checkName.Equals("wing"))
		if(checkName.Equals("devilCave") || checkName.Equals("golden_goblin") || checkName.Equals("divine") 
		  || checkName.Equals("dragonTreasure") || checkName.Equals("luckStar") || checkName.Equals("wing")
		 || checkName.Equals("award") || checkName.Equals("mission") || checkName.Equals("package") || checkName.Equals("role") 
			|| checkName.Equals("setting") || checkName.Equals("skill") || checkName.Equals("bg") || checkName.Equals("chat")
			|| checkName.Equals("upSkill") || checkName.Equals("task"))
    		return true;
    	return false;
    }
}
