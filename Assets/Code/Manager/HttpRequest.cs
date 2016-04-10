using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

class HttpRequest : MonoBehaviour
{

    private string data = "";

    public HttpRequest()
    {

    }

    public void ReadHttpData(string url, string method, Dictionary<string, string> requestData)
    {
        url = Global.resourceUrl + url;
        if (method == "POST")
        {
          StartCoroutine(RequestPost(url, requestData));
        }
        if (method == "GET")
        {
           StartCoroutine(RequestGet(url, requestData));
        }
    }

    public string GetData()
    {
        return data;
    }
    /// <summary>
    /// GET请求
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    IEnumerator RequestGet(string url, Dictionary<string, string> post)
    {
        if (post != null)
        {
            bool first = true;
            foreach (KeyValuePair<string, string> data in post)
            {
                if (first)
                {
                    url += "?";
                    first = false;
                }
                else
                {
                    url += "&";
                }
                url += data.Key + "=" + data.Value;
            }
        }
        WWW reponse = new WWW(url);
        yield return reponse;
        if (reponse.error != null)
        {
            Loger.Notice("[GET] http request error:{0},{1}", reponse.error, url);
        }
        else
        {
            Loger.Log("[GET] http request ok:{0}", reponse.text);
        }
    }
    /// <summary>
    /// post请求
    /// </summary>
    /// <param name="url"></param>
    /// <param name="post">请求表单数据</param>
    /// <returns></returns>
    IEnumerator RequestPost(string url, Dictionary<string, string> post)
    {
        WWWForm form = new WWWForm();
        if (post != null)
        {
            foreach (KeyValuePair<string, string> data in post)
            {
                form.AddField(data.Key, data.Value);
            }
        }
        WWW reponse = new WWW(url, form);
        yield return reponse;
        if (reponse.error != null)
        {
            Loger.Notice("[POST] http request error:{0}, {1}", reponse.error, url);
        }
        else
        {
            Loger.Log("[POST] http request ok:{0}", reponse.text);
            data = reponse.text;
        }
    }
}

