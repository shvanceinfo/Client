using UnityEngine;
using System.Collections;
using model;
using MVC.entrance.gate;
namespace manager
{
    public class LuckStoneManager
    {
        static LuckStoneManager _instance;

        private Hashtable _luckStoneHash;

        private BetterList<LuckStoneVo> _sortLuckStones;    //排序后的强化石列表

        private LuckStoneVo _selectedStone;                 //选中的强化石
        private TypeStruct _selectedStoneConsume;           //选中石头的消耗

        
        
       
        private LuckStoneManager()
        {
            _selectedStoneConsume = null;
            _selectedStone = null;
            _luckStoneHash = new Hashtable();
            _sortLuckStones = new BetterList<LuckStoneVo>();
        }

        public void OpenWindow()
        {
            //初始化数据
            _sortLuckStones.Clear();
            foreach (int key in _luckStoneHash.Keys)
            {
                _sortLuckStones.Add(_luckStoneHash[key] as LuckStoneVo);
            }
            _sortLuckStones.Sort((LuckStoneVo v1, LuckStoneVo v2) =>
            {
                if (v1.Id > v2.Id)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            });


            UIManager.Instance.openWindow(UiNameConst.ui_luckstone);
           
            
        }
        //关闭窗口
        public void CloseWindow()
        {
            _selectedStone = null;
            _selectedStoneConsume = null;
            UIManager.Instance.closeWindow(UiNameConst.ui_luckstone);
            //回调刷新UI
            LuckStone_CallBack();
        }

        //点击了确定按钮
        public void SureButton()
        {
            UIManager.Instance.closeWindow(UiNameConst.ui_luckstone);
            
            LuckStone_CallBack();
        }

        /// <summary>
        /// 处理强化石选择后的回调
        /// </summary>
        public void LuckStone_CallBack()
        { 
            //TODE:处理强化石选择后的回调
            Gate.instance.sendNotification(MsgConstant.MSG_MERGE_DISPLAY_GEM_INFO);

            Gate.instance.sendNotification(MsgConstant.MSG_FORMULA_DISPLAY_INFO);

            Gate.instance.sendNotification(MsgConstant.MSG_STRENGTHEN_LUCKSTONE_CALLBACK);
            Gate.instance.sendNotification(MsgConstant.MSG_ADVANCED_LUCKSTONE_CALLBACK);
        }

        public void SetSelectStone(int id)
        {
            
            if (id == 0)
            {
                _selectedStone = null;
                return;
            }

            LuckStoneVo vo = _luckStoneHash[id] as LuckStoneVo;
            
            if (_selectedStone != null)     //如果不为空，则判断是否是一样的
            {
                if (_selectedStone.Id == vo.Id)//如果为一样的，则做取消选中功能
                {
                    _selectedStone = null;
                }
                else {
                    _selectedStone = vo;
                }
            }
            else {
                _selectedStone = vo;
            }

            //计算消耗
            if (_selectedStone!=null)
            {
                _selectedStoneConsume = new TypeStruct();

                if (IsHaveItem())
                {
                    _selectedStoneConsume.Id = _selectedStone.ConsumeItem[0].Id;
                    _selectedStoneConsume.Type = ConsumeType.Item;
                    _selectedStoneConsume.Value= _selectedStone.ConsumeItem[0].Value;
                }
                else {
                    _selectedStoneConsume.Id = (int)eGoldType.zuanshi;
                    _selectedStoneConsume.Type = ConsumeType.Gold;
                    _selectedStoneConsume.Value = _selectedStone.ConsumeDiamond;
                }
            }
        }



        /// <summary>
        /// 根据消耗物品类型获取数量
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public int GetConsumeItem(int itemId)
        {
            return (int)ItemManager.GetInstance().GetItemNumById((uint)itemId);
        }
        /// <summary>
        /// 获取当前共有多少符石
        /// </summary>
        /// <returns></returns>
        public int GetConsumeItem()
        {
            return (int)ItemManager.GetInstance().GetItemNumById((uint)_sortLuckStones[0].ConsumeItem[0].Id);
        }

        /// <summary>
        /// 检查当前选中的物品是否拥有
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsHaveItem()
        {
            //获取自己拥有的物品
            int haveCount= GetConsumeItem(_selectedStone.ConsumeItem[0].Id);
            int needCount = _selectedStone.ConsumeItem[0].Value;
            if (haveCount>=needCount)
            {
                return true;
            }
            return false;
        }
        public bool IsHaveItem(int count)
        {
            //获取自己拥有的物品
            int haveCount= GetConsumeItem(_selectedStone.ConsumeItem[0].Id);
            int needCount = _selectedStone.ConsumeItem[0].Value*count;
            if (haveCount>=needCount)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取当前选择的物品，自己拥有多少
        /// </summary>
        /// <returns></returns>
        public int GetSelectCount()
        {
            return GetConsumeItem(_selectedStone.ConsumeItem[0].Id);
        }

        public static LuckStoneManager Instance
        {
            get {
                if (_instance==null)
                {
                    _instance = new LuckStoneManager();
                }
                return LuckStoneManager._instance; 
            }
        }

        /// <summary>
        /// 幸运石hash表
        /// </summary>
        public Hashtable LuckStoneHash
        {
            get { 
                return _luckStoneHash; 
            }
        }
        public BetterList<LuckStoneVo> SortLuckStones
        {
            get { return _sortLuckStones; }
        }

        /// <summary>
        /// 当前选择的强化石，未选中则为null
        /// </summary>
        public LuckStoneVo SelectedStone
        {
            get { return _selectedStone; }
        }
        /// <summary>
        /// 选择物品的消耗物品
        /// </summary>
        public TypeStruct SelectedStoneConsume
        {
            get { return _selectedStoneConsume; }
            set { _selectedStoneConsume = value; }
        }
    }
}
