using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using manager;

public class KInt
{
    public int Key { get; set; }
    public int Value { get; set; }
}
class CacheUserInfo
{
    public string GUID;
    public string UserName="";
    public string UserPassword="";
    public bool isRememberPassword; //记住密码
    public int LastFightLevel = (int)Global.eFightLevel.Fight_Level1;
    public bool AudioActive;
    public bool MusicActive;
    public List<KInt> DisplayPeople;
    public List<string> FriendLog { get; set; } //好友日志
    public int ServerId = -1;
    public CacheUserInfo()
    {
        AudioActive = true;
        MusicActive = true;
        FriendLog = new List<string>();
        DisplayPeople = new List<KInt>();
    }
    public string ToJson()
    {
        string data = JsonMapper.ToJson(this);
        return data;
    }

    public static CacheUserInfo ToObject(string data)
    {
        CacheUserInfo obj = new CacheUserInfo();
        obj = JsonMapper.ToObject<CacheUserInfo>(data);
        return obj;
    }
}

class FileManager
{
    private string fileName;
    private FileInfo fileInfo;
    public FileManager(string name)
    {
        fileName = Application.persistentDataPath + "//" + name;
        fileInfo = new FileInfo(fileName);
    }

    public string Read()
    {
        StreamReader streRead = null;
        if (fileInfo.Exists)
        {
            try
            {
                streRead = File.OpenText(fileName);
            }
            catch
            {
                return "";
            }
            string txt ="";
            txt = streRead.ReadToEnd();
            streRead.Close();
            streRead.Dispose();
            return txt;
        }
        return "";
    }

    public void Write(string txt)
    {
        StreamWriter streWrite;
        if (fileInfo.Exists)
        {
            streWrite = fileInfo.AppendText();
        }
        else
        {
            streWrite = fileInfo.CreateText();
        }
        streWrite.WriteLine(txt);
        streWrite.Close();
        streWrite.Dispose();
    }

    public void Delete()
    {
        if (fileInfo.Exists)
        {
            File.Delete(fileName);
        }
    }
}

class CacheManager
{
    private static CacheManager instance;
    private CacheUserInfo userInfo;
    private Global.eFightLevel fightLevel;
    private CacheManager()
    {
        Init();
    }

    public void Init()
    {
        FileManager file = new FileManager(Global.CACHE_USER_INFO_FILE);
        string userJson = file.Read();
        if (userJson == "")
        {
            userInfo = new CacheUserInfo();
        }
        else
        {
            userInfo = CacheUserInfo.ToObject(userJson);
        }
        AudioManager.Instance.AudioActive = userInfo.AudioActive;
        AudioManager.Instance.MusicActive = userInfo.MusicActive;
        SettingManager.Instance.ReadCache(userInfo.DisplayPeople);
    }

    public static CacheManager GetInstance()
    {
        if (instance == null)
        {
            instance = new CacheManager();
        }
        return instance;
    }

    public void SetUserLoginInfo(CacheUserInfo info)
    {
        userInfo = info;
    }

    public void SetFightLevel(Global.eFightLevel level)
    {
        userInfo.LastFightLevel = (int)level;
    }

    public void SaveCache()
    {
        FileManager file = new FileManager(Global.CACHE_USER_INFO_FILE);
        file.Delete();
        file.Write(userInfo.ToJson());
    }

    public CacheUserInfo GetCacheInfo()
    {
        return userInfo;
    }

    public Global.eFightLevel GetLastFightLevel()
    {
        return (Global.eFightLevel)userInfo.LastFightLevel;
    }
    //设置记住密码
    public void SetRememberPassword(bool isRe)
    {
        userInfo.isRememberPassword = isRe;
    }
    public void SetMusic(bool b)
    {
        userInfo.MusicActive = b;
    }
    public void SetAudio(bool b)
    {
        userInfo.AudioActive = b;
    }
    public void SetDisplayOption(int key, int option)
    {
        foreach (KInt value in userInfo.DisplayPeople)
        {
            if (value.Key==key)
            {
                value.Value = option;
                return;
            }
        }
        userInfo.DisplayPeople.Add(new KInt() { Key = key, Value = option });
    }
    //获取和设置server_id
    public int GetServerId()
    {
        return userInfo.ServerId;
    }
    public void SetServerId(int id)
    {
        userInfo.ServerId = id;
    }

}

