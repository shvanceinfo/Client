using UnityEngine;
using System.Collections;
using manager;

public class BtnFightMenuMsg : MonoBehaviour
{
	private const string PLAYER = "Icon_Hand";
	private const string BOSS_DAMAGE_BTN = "boss_damage_btn";
	private const string SKILL1 = "Skill1";
	private const string SKILL2 = "Skill2";
	
	void OnClick ()
	{
		switch (gameObject.name) {
 
		case PLAYER:
			this.transform.parent.parent.parent.FindChild("bottom/func").GetComponent<MenuFunView>().OnOpenFunc();//执行打开或者关闭的操作
			break;	
		case BOSS_DAMAGE_BTN:
			BossManager.Instance.SwitchBossDamageBtn ();
			break;
		case SKILL1:
			BossManager.Instance.UseItemInWorldBoss (UseItemInWorldBossType.Rongyu);
			break;
		case SKILL2:
			BossManager.Instance.UseItemInWorldBoss (UseItemInWorldBossType.Zhuanshi);
			break;
			
		default:
			break;
		}
	}
}
