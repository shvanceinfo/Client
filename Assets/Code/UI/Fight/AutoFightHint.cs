using UnityEngine;
using System.Collections;
using System;

public class AutoFightHint : MonoBehaviour 
{
    UISprite show;
    UISprite hint;

    void Awake()
    {
        show = transform.FindChild("AutoBottom").GetComponent<UISprite>();
        hint = transform.FindChild("AutoShow").GetComponent<UISprite>();
    }

	// Use this for initialization
	void Start () 
    {
        
	}
	
	// Update is called once per frame
	void Update () 
    {
        show.enabled = Global.m_bAutoFight;
        hint.enabled = Global.m_bAutoFight;
	}
}
