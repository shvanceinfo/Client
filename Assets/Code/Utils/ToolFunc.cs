/**该文件实现的基本功能等
function: 一些时间转换等工具函数
author:ljx
date:2013-11-01
**/
using System;
using UnityEngine;

public class ToolFunc 
{
	//获取时间直接的秒数差值
	public static int getDeltaSecond(DateTime lastDateTime)
	{
		TimeSpan span = DateTime.Now - lastDateTime;
		//lastDateTime = DateTime.Now;
		return span.Minutes*60 + span.Seconds;
	}
	
	//反射机制拷贝对象
	public static Component CopyComponent(Component original, GameObject destination)
	{
	    System.Type type = original.GetType();
	    Component copy = destination.AddComponent(type);
	    // Copied fields can be restricted with BindingFlags
	    System.Reflection.FieldInfo[] fields = type.GetFields(); 
	    foreach (System.Reflection.FieldInfo field in fields)
	    {
	       field.SetValue(copy, field.GetValue(original));
	    }
	    return copy;
	}
	
	//uinix时间戳转换为dateTime
	public static DateTime GetDateTime(UInt32 uiStamp)
	{
		DateTime dt = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddSeconds(uiStamp);
		return dt;
	}

    //获取所有prefab的最后的名称
    public static string TrimPath(string origName)
    {
        if (origName.IndexOf('/') != -1)
            return origName.Substring(origName.LastIndexOf('/') + 1);
        return origName;
    }

    //调整层次
    public static void SetLayerRecursively(GameObject go, int layerNumber)
    {
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }
}
