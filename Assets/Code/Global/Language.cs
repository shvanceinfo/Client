using UnityEngine;
using System.Collections;

public class Language : MonoBehaviour 
{

    bool isStarted = false;

	void Start () 
	{
        isStarted = true;
	}

    void OnEnable()
    {
        if (!isStarted)
        {
            UILabel lab = gameObject.GetComponent<UILabel>();
            LanguageManager.SetText(ref lab, lab.text);
        }
    }
}
