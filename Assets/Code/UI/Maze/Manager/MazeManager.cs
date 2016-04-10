using UnityEngine;
using System.Collections;

namespace manager
{
    public class MazeManager
    {

        private MazeManager()
        {

        }

        #region 单例
        private static MazeManager _instance;
        public static MazeManager Instance
        {
            get
            {
                if (_instance == null) _instance = new MazeManager();
                return _instance;
            }
        }
        #endregion


        public void OpenWindow()
        {
            if (CheckData())
            {
                
            }
        }

        public void WaitDataForOpenWindow()
        { 
            
        }

        private bool CheckData()
        {
            return true;
        }
    }
}
