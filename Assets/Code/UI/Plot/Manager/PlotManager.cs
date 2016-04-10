using UnityEngine;
using System.Collections;
using model;
using MVC.entrance.gate;
namespace manager
{
    public class PlotManager
    {
        private BetterList<PlotVo> _maps;
        private PlotVo _raidVo;             //扫荡的关卡
        private bool _isRaid;

        
        
        
        private PlotManager()
        {
            _maps = new BetterList<PlotVo>();
        }

        private void Initial()
        {
            _maps.Clear();
            DataReadMap drm= ConfigDataManager.GetInstance().getMapConfig();
            foreach (int key in drm.Keys)
            {
                MapDataItem md=drm.getMapData(key);
                if (md.mapCate==MapCate.Plot)
                {
                    PlotVo vo = new PlotVo
                    {
                        Id = md.id,
                        Name = md.name,
                        Icon = md.icon,
                        ComsumeStrength = md.engeryConsume,
                        UnLockLevel = md.nEnterLevel,
                    };
                    if (!string.IsNullOrEmpty(md.dropItem) && md.dropItem != "0")
                    {
                        string[] sps = md.dropItem.Split(',');
                        for (int i = 0; i < sps.Length; i++)
                        {
                            vo.Awards.Add(int.Parse(sps[i]));
                        }
                    }

                    _maps.Add(vo);
                }
            }
        }

        public void OpenWindow()
        {
            Initial();
            UIManager.Instance.openWindow(UiNameConst.ui_plot);
            Gate.instance.sendNotification(MsgConstant.MSG_PLOT_DISPLAY_MAIN);
        }
        public void CloseWindow()
        {
            _maps.Clear();
            UIManager.Instance.closeWindow(UiNameConst.ui_plot);
        }
        public BetterList<PlotVo> Maps
        {
            get { return _maps; }
            set { _maps = value; }
        }

        public PlotVo RaidVo
        {
            get { return _raidVo; }
            set { _raidVo = value; }
        }

        /// <summary>
        /// 是否有关卡在扫荡
        /// </summary>
        public bool IsRaid
        {
            get { return _raidVo == null ? false : true; }
        }
        #region 单例
        private static PlotManager _instance;
        public static PlotManager Instance
        {
            get
            {
                if (_instance == null) _instance = new PlotManager();
                return _instance;
            }
        }
        #endregion
      
      
    }
}