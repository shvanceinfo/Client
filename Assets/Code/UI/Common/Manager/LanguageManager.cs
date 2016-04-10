/**该文件实现的基本功能等
function: 实现多语言的管理
author:ljx
date:2013-11-09
**/
using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;


public class LanguageManager
{
	 //static private LanguageManager instance;
    private static Dictionary<string, string> dictLang = new Dictionary<string, string>();
    private static bool needLoad = true;

    static public void LoadTxt()
    {
    	if(needLoad)
    	{
	        TextAsset txt = BundleMemManager.Instance.loadResource(PathConst.LANGUAGE_PATH, typeof(TextAsset)) as TextAsset;
	        char[] split = { '\r', '\n' };
	        string[] lines = txt.text.Split(split, StringSplitOptions.RemoveEmptyEntries);
	        int len = lines.Length;
	        for (int i = 0; i < len; i++)
	        {
	            string[] langDef = lines[i].Split('=');
	            if (langDef.Length == 2)
	            {
	                if (dictLang.ContainsKey(langDef[0]))
	                {
	                    Loger.Error("key error {0}", langDef[0]);
	                }
	                else
	                {
	                    dictLang.Add(langDef[0].Trim(), langDef[1].Trim());
	                }                
	            }
	        }
	        needLoad = false;
    	}
    }

    static public void SetText(ref UILabel label, string name)
    {
        if (dictLang.ContainsKey(name))
        {
            label.text = dictLang[name];
        }
        else
        {
            label.text = name;
        }
    }

    static public string GetText(string name)
    {
        if (dictLang.ContainsKey(name))
        {
           return dictLang[name];
        }
        else
        {
            return name;
        }
    }
}
