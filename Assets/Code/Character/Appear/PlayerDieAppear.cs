using UnityEngine;
using System.Collections;
using manager;

public class PlayerDieAppear : DieAppear 
{
    float m_fDieTime;
    public bool show_ui_enable = false;


	public override void active() 
    {
        animation_name = "die2a";
        on_active(int.MaxValue);
        m_fDieTime = owner.animation["die2a"].length;
        show_ui_enable = true;


        
       

        if (Global.inMultiFightMap() && !CharacterPlayer.character_property.getHostComputer())
        {
            BattleMultiPlay.GetInstance().OnPlayerDie(owner);
        }
	}
   
    public override void update(float delta)
    {
        base.update(delta);

        if (!Global.InArena())
        {
            if (time_since_begin > m_fDieTime)
            {
                if (show_ui_enable)
                {
                    show_ui_enable = false;
                    if (Global.inTowerMap())            //恶魔洞窟
                    {
                        DemonManager.Instance.OpenDemonAnceWindow(DemonAnceView.DemonArceType.Death);
                    }
                    else if (Global.inMultiFightMap())  //多人副本
                    {

                    }
                    else {
                        DeathManager.Instance.OpenDeathWindow();//其他界面
                    }
                }
            }
        }
        
    }
}
