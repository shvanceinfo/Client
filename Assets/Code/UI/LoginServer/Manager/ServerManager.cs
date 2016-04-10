using UnityEngine;
using System.Collections;
using model;
using MVC.entrance.gate;
using System.Collections.Generic;
using System;

namespace manager
{
    
    public class Servers : IComparable<Servers>
    {
        public ServState State { get; set; }

        public int CompareTo(Servers other)
        {
            int myState = StateNow(State);
            int others = StateNow(other.State);
            if (myState > others)
            {
                return -1;
            }
            else if (myState == others)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        private int StateNow(ServState s)
        {
            switch (s)
            {
                case ServState.CLOSED:
                    return 0;
                    break;
                case ServState.NORMAL_SERVER:
                    return 1;
                    break;
                case ServState.HOT_SERVER:
                    return 3;
                    break;
                case ServState.RECENT_SERVER:
                    return 5;
                    break;
                case ServState.NEW_SERVER:
                    return 2;
                    break;
                case ServState.RECOMMEND_SERVER:
                    return 4;
                    break;
                default:
                    return 0;
                 break;
            }
        }
    }


    public class ServerManager
    {
        private BetterList<ServerVo> _serverList;
        private static ServerManager _instace;
        public static ServerManager Instance
        {
            get
            {
                if (_instace == null)
                    _instace = new ServerManager();
                return _instace;
            }
        }

        private Hashtable _serverHash;
        private ServerManager()
        {
            _serverHash = new Hashtable();
            _serverList = new BetterList<ServerVo>();
        }
        public Hashtable ServerHash
        {
            get { return _serverHash; }
        }

        

        public ServerVo FindVoById(int id)
        {
            foreach (ServerVo vo in ServerHash.Values)
            {
                if (vo.Id == id)
                {
                    //_curVo = vo;
                    return vo;
                }
            }
            return null;
        }

        public ServerVo ChooseVo;

        private ServerVo _curVo;
        public ServerVo CurrentVo
        {
            get { return _curVo; }
            set { _curVo = value; }
        }

        public ServerVo lastVo;
        
        public ServerVo FindVoByOrderId(int id)
        {
            foreach (ServerVo vo in ServerHash.Values)
            {
                if (vo.OrderId == id)
                {
                    //_curVo = vo;
                    return vo;
                }
            }
            return null;
        }
        public bool isRecommend = false;    //是否右推荐登录
        public bool isRecent = false;       //是否有最近登录

        public void SetServerList()
        {
            _serverList.Clear();
            foreach (ServerVo info in ServerHash.Values)
            {
                ServerVo vo = new ServerVo();
                vo.Id = info.Id;
                vo.ServerId = info.ServerId;
                vo.Name = info.Name;
                vo.IpAddr = info.IpAddr;
                vo.ServerOpenState = info.ServerOpenState;
                vo.ServerState = info.ServerState;
                vo.OrderId = info.OrderId;
                _serverList.Add(vo);

                //若没有最近登录，选择开放的服务器作为默认值
                if (vo.ServerOpenState && ServerManager.Instance.CurrentVo == null)
                {
                    if (vo.ServerState == ServState.RECENT_SERVER)
                    {
                        isRecent = true;
                        ServerManager.Instance.CurrentVo = vo;
                        SelectServer.sSelect.DefaultServer();
                    }
                    else if (vo.ServerState == ServState.RECOMMEND_SERVER)
                    {
                        isRecommend = true;
                        ServerManager.Instance.CurrentVo = vo;
                        SelectServer.sSelect.DefaultServer();
                    }
                    else if (_serverList.size == ServerHash.Count)
                    {
                        ServerManager.Instance.CurrentVo = vo;
                        SelectServer.sSelect.DefaultServer();
                    }
                    else
                    {
                        ServerManager.Instance.CurrentVo = vo;
                        SelectServer.sSelect.DefaultServer();
                    }
                }
                else
                {
                    SelectServer.sSelect.DefaultServer();                    
                }
            }

        }

        public BetterList<ServerVo> ServerList
        {
            get { return _serverList; }
        }


    }

    public class DataReadServer
    {

        private static DataReadServer _instace;
        public static DataReadServer Instance
        {
            get
            {
                if (_instace == null)
                    _instace = new DataReadServer();
                return _instace;
            }
        }

        public List<ServerVo> _serverList = new List<ServerVo>();
        public void DataInfo()
        {
            ServerManager.Instance.ServerHash.Clear();
            ServerVo vo0 = new ServerVo();
            vo0.Id = 10001;
            vo0.Name = "策划服";
            vo0.IpAddr = "127.0.0.1";
            vo0.Port = 9999;
            vo0.OrderId = 0;
            vo0.ServerOpenState = true;
            vo0.ServerState = ServState.RECENT_SERVER;
            _serverList.Add(vo0);

            ServerVo vo1 = new ServerVo();
            vo1.Id = 10002;
            vo1.Name = "测试服";
            vo1.IpAddr = "192.168.1.117";
            vo1.Port = 18062;
            vo1.OrderId = 1;
            vo1.ServerOpenState = true;
            vo1.ServerState = ServState.RECOMMEND_SERVER;
            _serverList.Add(vo1);

            ServerVo vo2 = new ServerVo();
            vo2.Id = 10003;
            vo2.Name = "刘景校";
            vo2.IpAddr = "127.0.0.1";
            vo2.Port = 9999;
            vo2.OrderId = 2;
            vo2.ServerOpenState = true;
            vo2.ServerState = ServState.NEW_SERVER;
            _serverList.Add(vo2);

            ServerVo vo3 = new ServerVo();
            vo3.Id = 10004;
            vo3.Name = "吕晓军";
            vo3.IpAddr = "127.0.0.1";
            vo3.Port = 9999;
            vo3.OrderId = 3;
            vo3.ServerOpenState = true;
            vo3.ServerState = ServState.NEW_SERVER;
            _serverList.Add(vo3);

            List<Servers> servers = new List<Servers>();
            for (int i = 0; i < _serverList.Count; i++)
            {
                servers.Add(new Servers { State = _serverList[i].ServerState });
            }
            servers.Sort();

            for (int i = 0; i < _serverList.Count; i++)
            {
                _serverList[i].OrderId = i;
                if(_serverList[i].ServerState == ServState.RECENT_SERVER)
                    _serverList[i].Id = 10003;
                else if (_serverList[i].ServerState == ServState.RECOMMEND_SERVER)
                    _serverList[i].Id = 10000;
                else 
                    _serverList[i].Id = i+10;
                ServerManager.Instance.ServerHash.Add(i + 1, _serverList[i]);
                //Debug.Log(_serverList[i].Name + _serverList[i].OrderId);
            }

            ServerManager.Instance.SetServerList();
        }
    }



}
