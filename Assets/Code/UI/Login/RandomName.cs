/**该文件实现的基本功能等
function:
author:ljx
date:
**/
using UnityEngine;
using System.Collections;
using System;

public class RandomName : MonoBehaviour {

	private string[] familyNames; //姓
	private string[] names; //名
	// Use this for initialization
	void Start () 
	{
		TextAsset txt = BundleMemManager.Instance.loadResource(PathConst.RAND_NAME_PATH, typeof(TextAsset)) as TextAsset;
        string[] names = txt.text.Split('=');
        if(names.Length == 2)
        {
        	char[] split = { '\r', '\n' };
        	this.names = names[0].Split(split, StringSplitOptions.RemoveEmptyEntries);
        	this.familyNames = names[1].Split(split, StringSplitOptions.RemoveEmptyEntries);
        }
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void OnClick()
	{
		int familyIndex = UnityEngine.Random.Range(0, familyNames.Length);
		int nameIndex = UnityEngine.Random.Range(0, names.Length);
		UIInput input = GameObject.Find("nameInput").GetComponent<UIInput>();
		if(input != null)
		{
			UILabel tipLable = GameObject.Find("tipLable").GetComponent<UILabel>();
			tipLable.fontSize = 18;
			//tipLable.font.dynamicFontSize = 18;
			input.text = familyNames[familyIndex] +  names[nameIndex];
			CheckInput.firstPress = false; //点击了随机名字就不清空
		}
	}
	
	void OnMouseOut()
	{
		
	}
	
	void OnMouseOver()
	{
		
	}
}
