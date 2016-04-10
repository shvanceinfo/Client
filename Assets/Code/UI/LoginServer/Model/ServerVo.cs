using UnityEngine;
using System.Collections;

namespace model
{
    public enum ServState
    {
        NORMAL_SERVER=0,
        HOT_SERVER,
        RECENT_SERVER,
        NEW_SERVER,
        RECOMMEND_SERVER,
        CLOSED,
    }


    public class ServerVo
    {
        public int Id { get; set; }
        public int ServerId { get; set; }
        public string Name { get;set; }
        public string IpAddr { get;set; }
        public int Port { get; set; }
        public int OrderId { get; set; }
        public bool ServerOpenState { get; set; }
        public ServState ServerState { get; set; }

    }

}