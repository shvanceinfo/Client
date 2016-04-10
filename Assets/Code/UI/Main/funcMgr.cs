using UnityEngine;
using System.Collections;
using manager;

public class funcMgr : MonoBehaviour
{
//	private static funcMgr _instance;
	public float time = 0f;
	private float timeOut = 2f;
//	public static bool isChildOpen = false; //子菜单是否处于打开状态
    
	public static bool isOpen = false; //菜单是否处于打开状态
//	private bool isOpenFunc = false;
	private GameObject menu;

	void OnEnable ()
	{
		//EventDispatcher.GetInstance ().EventOpenFunc += OnOpenFunc;
	}
	
	void OnDisable ()
	{
		//EventDispatcher.GetInstance ().EventOpenFunc -= OnOpenFunc;
	}

//	public static funcMgr Instance {
//		get {
//			return _instance;
//		}
//	}
	
	void Awake ()
	{
//		_instance = this;
		menu = transform.parent.FindChild ("menu").gameObject;
	}
	// Use this for initialization
	void Start ()
	{
	}

	public void ResetTime ()
	{
		time = 0f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
//       if ((time >= timeOut) && isOpen)
//       {
//           Close();
//       }
//       else if (time < timeOut)
//       {
//           time += Time.deltaTime;
//       }       
//		 if(!isChildOpen && isOpen && Input.GetButtonDown("Fire1"))
//		 {
//		 	if (UICamera.hoveredObject != null)//判断是否击中UI
//		 	{
//		 		if(!checkClickMenu(UICamera.hoveredObject.name))
//		 		{
//			 		if(checkClickSubMenu(UICamera.hoveredObject.name))
//			 			isChildOpen = true;
//			 		else
//			 			Close();
//		 		}
//		 	}
//		 	else if(UICamera.selectedObject != null)
//		 	{
//		 		if(!checkClickMenu(UICamera.selectedObject.name))
//		 		{
//			 		if(checkClickSubMenu(UICamera.selectedObject.name))
//			 			isChildOpen = true;
//			 		else
//			 			Close();
//		 		}
//		 	}
//		 	else
//		 		Close();
//		 }

        if (GuideManager.Instance.IsEnforceUI)
        {
            return;
        }
		if (isOpen && Input.GetButtonDown ("Fire1")) {
			if (UICamera.hoveredObject == null) {//判断是否击中UI,只要击中空白处则隐藏
				Close ();
			}  
//			Debug.Log (UICamera.hoveredObject == null);
//			Debug.Log (UICamera.selectedObject == null);
		}
		
		
		
		
		
	}

	public void Open ()
	{
//		ResetTime ();
		isOpen = true;
//        OpenRightMenu.isFuncOpen = false;
		TweenPosition positon = UITweener.Begin<TweenPosition> (gameObject, 0.3f);
		positon.from = new Vector3 (0f, -70f, 0f);
		positon.to = new Vector3 (0f, 45f, 0f);
		SetMenuStatus (false);
		EasyTouchJoyStickProperty.ShowJoyTouch(false);
	}

	public bool IsOpen ()
	{
		return isOpen;
	}

	public void Close ()
	{
		isOpen = false;
		TweenPosition positon = UITweener.Begin<TweenPosition> (gameObject, 0.3f);
		positon.from = new Vector3 (0f, 45f, 0f);
		positon.to = new Vector3 (0f, -70f, 0f);
		SetMenuStatus (true);

        if (!UIManager.Instance.getUIFromMemory(UiNameConst.ui_pet) &&
            !UIManager.Instance.getUIFromMemory(UiNameConst.ui_wing) &&
            !UIManager.Instance.getUIFromMemory(UiNameConst.ui_skill))
        {
            EasyTouchJoyStickProperty.ShowJoyTouch(true);
        }
	}

	void CallClose ()
	{
		SetMenuStatus (true);
	}

	void SetMenuStatus (bool open)
	{
		menu.SetActive (open);
        //关闭所有按钮
        transform.FindChild("active").gameObject.SetActive(!open);
	}
	
	/// <summary>
	/// 开关按钮
	/// </summary>
	public  void OnOpenFunc ()
	{
		if (!isOpen) {
			Open ();
		}else{
			Close();
		}
	}
    
//	bool checkClickMenu (string checkName)
//	{
//		if (checkName.Equals ("menu") || checkName.Equals ("showFunc"))
//			return true;
//		return false;
//	}
    
	
//	bool checkClickSubMenu (string checkName)
//	{
////    	if(checkName.Equals("award") || checkName.Equals("mission") || checkName.Equals("package") || checkName.Equals("role") 
////			|| checkName.Equals("setting") || checkName.Equals("skill") || checkName.Equals("bg") || checkName.Equals("chat")
////			|| checkName.Equals("upSkill") || checkName.Equals("task"))
//		
//		
//		
//		if (checkName.Equals ("devilCave") || checkName.Equals ("golden_goblin") || checkName.Equals ("divine") 
//		  || checkName.Equals ("dragonTreasure") || checkName.Equals ("luckStar") || checkName.Equals ("wing")
//		 || checkName.Equals ("award") || checkName.Equals ("mission") || checkName.Equals ("package") || checkName.Equals ("role") 
//			|| checkName.Equals ("setting") || checkName.Equals ("skill") || checkName.Equals ("bg") || checkName.Equals ("chat")
//			|| checkName.Equals ("upSkill") || checkName.Equals ("task"))
//			return true;
//		return false;
//	}
}
