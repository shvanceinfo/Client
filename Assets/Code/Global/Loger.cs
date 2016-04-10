using UnityEngine;
using System.Collections;

public class Loger {
    /// <summary>
    /// 信息
    /// </summary>
    /// <param name="message"></param>
    /// <param name="value"></param>
	static public void Log(string message, params object[] value)
    {
        string msg = string.Format(message, value);
        //Debug.Log("Log:" + msg);
    }
    /// <summary>
    /// 错误
    /// </summary>
    /// <param name="message"></param>
    /// <param name="value"></param>
    static public void Error(string message, params object[] value)
    {
        string msg = string.Format(message, value);
        Debug.LogError("Error:" + msg);
    }
    /// <summary>
    /// 警告
    /// </summary>
    /// <param name="message"></param>
    /// <param name="value"></param>
    static public void Notice(string message, params object[] value)
    {
        string msg = string.Format(message, value);
        Debug.LogWarning("Notice:" + msg);
    }
}
