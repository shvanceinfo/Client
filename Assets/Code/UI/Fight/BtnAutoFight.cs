using UnityEngine;
using System.Collections;
using System;

public class BtnAutoFight : MonoBehaviour 
{
    private UISprite autofight;
  
	void Start () 
    {
        autofight = transform.FindChild("Background").GetComponent<UISprite>();

        if (Global.m_bAutoFight)
        {
            autofight.spriteName = "AutoDark";
        }
        else
        {
            autofight.spriteName = "InAuto";
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        
	}

    private void OnClick()
    {
        //if (Global.m_bAutoFight)
        //{
        //    Global.m_bAutoFight = false;
        //    autofight.spriteName = "InAuto";
        //    //autofight.transform.localScale = new Vector3(114, 114, 0);
        //}
        //else
        //{
        //    Global.m_bAutoFight = true;
        //    autofight.spriteName = "AutoDark";
        //    //autofight.transform.localScale = new Vector3(90, 90, 0);
        //}
        // 用来做攻击见
    }
}
