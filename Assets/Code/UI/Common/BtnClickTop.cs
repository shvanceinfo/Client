#define DungeonTest
 

using UnityEngine;
using System.Collections;
using manager;
using MVC.entrance.gate;

public class BtnClickTop : MonoBehaviour 
{
	void OnClick()
	{
		if(this.name.Equals("divine")) //竞技场
		{
	        ArenaManager.Instance.askArenaInfo();
            //		OpenRightMenu.isFuncOpen = false;
//            funcMgr.isChildOpen = false;
		}
#if DungeonTest
		else if (this.name.Equals("dragonTreasure")) {
			DungeonManager.Instance.ShowDungeonView();
//			funcMgr.isChildOpen = false;
		}
#endif
		else
		{
			FloatMessage.GetInstance().PlayFloatMessage(LanguageManager.GetText("wait_open"),
			                                           UIManager.Instance.getRootTrans(), Vector3.zero, Vector3.zero);
	//		OpenRightMenu.isFuncOpen = false;
//			funcMgr.isChildOpen = false;
		}
	}
}
