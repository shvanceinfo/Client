using UnityEngine;
using System.Collections;

public class UiFight : MonoBehaviour {
    public Global.eFightLevel fightLevel;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnSelect()
    {
        EventDispatcher.GetInstance().OnSelectFightLevel(fightLevel);
    }
}
