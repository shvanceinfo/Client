using UnityEngine;
using System.Collections;
using NetGame;
public class BtnMainSkill : MonoBehaviour {
    //GameObject label;
	// Use this for initialization
	void Start () {
        //label = GameObject.Find("test").gameObject;
        //label.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnClickFinish()
    {
        transform.parent.GetComponent<funcMgr>().ResetTime();
        UIManager.Instance.openWindow(UiNameConst.ui_skill);       
    }
        
    public void OnFinished()
    {
        //label.SetActive(false);
    }
}