using UnityEngine;
using System.Collections;
using model;

namespace manager
{
    public class DropOutManager
    {
        private Hashtable _dropHash;

        
        private DropOutManager()
        {
            _dropHash = new Hashtable();
        }
        #region 单例
        private static DropOutManager _instance;
        public static DropOutManager Instance
        {
            get
            {
                if (_instance == null) _instance = new DropOutManager();
                return _instance;
            }
        }
        #endregion

        public Hashtable DropHash
        {
            get { return _dropHash; }
            set { _dropHash = value; }
        }

        public DropOutVo FindVoByDropOutId(int mapid)
        {
            return DropHash[mapid] as DropOutVo;
        }
    }
}