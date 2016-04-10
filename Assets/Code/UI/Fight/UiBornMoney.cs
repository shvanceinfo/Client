using UnityEngine;
using System.Collections;

public class UiBornMoney : MonoBehaviour {
    private UILabel lblDiamond;
    private bool started = false;
	void Start () {
        lblDiamond = transform.FindChild("lbl_need_diamond").GetComponent<UILabel>();        
	}
	
	void Update () {
	    if (!started)
	    {
            started = true;
            lblDiamond.text = "x" + Global.GetBornDiamond().ToString();
            if (ItemManager.GetInstance().awardItems.Count == 0)
            {
                transform.Find("msg").gameObject.SetActive(true);
            }
            else
            {
                transform.Find("msg").gameObject.SetActive(false);
            }
	    }
	}

    void OnDisable()
    {
        started = false;
    }
}
