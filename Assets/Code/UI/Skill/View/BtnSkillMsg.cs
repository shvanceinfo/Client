using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using manager;

public class BtnSkillMsg : MonoBehaviour
{

	const string CLOSE_SKILLPAGE = "Btn_close";
	const string TABLE1_CLICK = "btn_Table1";
	const string TABLE2_CLICK = "btn_Table2";

	void OnClick ()
	{
		switch (gameObject.name) {
		case CLOSE_SKILLPAGE:
			UIManager.Instance.closeWindow (UiNameConst.ui_skill);
            SkillTalentManager.Instance.ClearEffect();
			break;
		case TABLE1_CLICK:  //切换界面
			if (!FastOpenManager.Instance.CheckFunctionIsOpen (FunctionName.Talent)) {
				return;
			}
			Gate.instance.sendNotification (MsgConstant.MSG_SKILL_TABLE_SWITCHING, 2);
			break;
		case TABLE2_CLICK:
			if (!FastOpenManager.Instance.CheckFunctionIsOpen (FunctionName.SKILL)) {
				return;
			}
			Gate.instance.sendNotification (MsgConstant.MSG_SKILL_TABLE_SWITCHING, 1);
			break;
		default:
			break;
		}
	}
}
