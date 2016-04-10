using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using manager;

public class BtnFightMsg : MonoBehaviour {




    const string Btn_Exit = "Btn_Exit";
    const string UseAttack = "Skill_Attack";    //点击普通攻击
    const string Btn_Chat = "Button_Chat";


    void OnClick()
    {

        switch (gameObject.name)
        {

            case Btn_Exit:
                UIManager.Instance.ShowDialog(eDialogSureType.eExitFight, "是否退出副本");
                break;
            case Btn_Chat:
                TalkManager.Instance.OpenWindow();
                break;
            default:
                break;
        }


    }
    void OnPress(bool press)
    {
        switch (gameObject.name)
        {
            case UseAttack:
                // 攻击键按着不动 进入强制攻击状态
                CharacterPlayer.sPlayerMe.GetComponent<FightProperty>().m_bForceAttack = press;
                if (press)
                {
                    if (CharacterPlayer.sPlayerMe.GetAI().GetCharacterState() == CharacterAI.CHARACTER_STATE.CS_ATTACK ||
                    CharacterPlayer.sPlayerMe.GetAI().GetCharacterState() == CharacterAI.CHARACTER_STATE.CS_SKILL)
                    {
                        EventDispatcher.GetInstance().OnAttackActived();
                    }
                    else
                        CharacterPlayer.sPlayerMe.UseAction();
                }
                break;
            default:
                break;
        }
    }
}
