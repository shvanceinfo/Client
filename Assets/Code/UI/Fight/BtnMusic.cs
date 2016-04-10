using UnityEngine;
using System.Collections;

public class BtnMusic : MonoBehaviour {

    public bool isOpen = true;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnClick()
    {
        if (isOpen)
        {
            isOpen = false;
            transform.FindChild("icon").GetComponent<UISprite>().spriteName = "close";
        }
        else
        {
            isOpen = true;
            transform.FindChild("icon").GetComponent<UISprite>().spriteName = "open";
        }
        GameObject.Find("Logic").GetComponent<SystemManager>().setMusicStatus(isOpen);
    }
}
