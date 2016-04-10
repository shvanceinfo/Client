using UnityEngine;
using System.Collections;

/// <summary>
/// 显示其他玩家信息
/// </summary>
public class FightDisplayPlayer : MonoBehaviour {


    private UISprite _handIcon;
    private UILabel _name;
    private UILabel _level;
    private HealthBar _bar;

    private void Awake()
    {
        _handIcon = transform.FindChild("Hand_Icon").GetComponent<UISprite>();
        _level = transform.FindChild("Lbl_Lvl").GetComponent<UILabel>();
        _name = transform.FindChild("Lbl_Name").GetComponent<UILabel>();
        _bar = transform.FindChild("Healt Bar").GetComponent<HealthBar>();
    }


    public void Display(string icon, string name, string lvl)
    {
        _handIcon.spriteName = icon;
        _name.text = name;
        _level.text = lvl;
    }


    public void SetHealt(int hp,int maxHp)
    {
        if (_bar!=null)
        {
            _bar.Value = hp;
            _bar.MaxValue = maxHp;
            if (hp <= 0)
            {
                SetDead();
            }
        }
    }
    private void SetDead()
    { 
        
    }
}
