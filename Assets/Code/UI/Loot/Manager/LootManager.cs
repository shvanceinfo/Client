using UnityEngine;
using System.Collections;
using model;

namespace manager
{
    public class LootManager
    {
        private Hashtable _lootHash;

        
        private LootManager()
        {
            _lootHash = new Hashtable();
        }   
        #region 单例
        private static LootManager _instance;
        public static LootManager Instance
        {
            get
            {
                if (_instance == null) _instance = new LootManager();
                return _instance;
            }
        }
        #endregion

        public Hashtable LootHash
        {
            get { return _lootHash; }
            set { _lootHash = value; }
        }

        public LootVo this[int id]
        {
            get {
                return _lootHash[id] as LootVo;
            }
        }
      
    }
}
