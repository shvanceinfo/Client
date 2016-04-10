using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using model;
using MVC.entrance.gate;
using helper;
using NetGame;

namespace manager
{
    public class InlayManager
    {
        private Dictionary<eEquipPart, InlayEquipVo> _inlayInfo;

        

        private EquipmentVo _selectVo;

        private InlayVo _selectGemVo;       //选中的宝石

        private BetterList<InlayVo> _allGems;   //当前所有的宝石

       
        private BetterList<InlayVo> _selectGems;    //当前装备使用的宝石集合

        GCAskInlayGem _ask30;
        private InlayManager()
        {
            _allGems = new BetterList<InlayVo>();
            _selectGems = new BetterList<InlayVo>();
            _ask30 = new GCAskInlayGem();
            _inlayInfo = new Dictionary<eEquipPart, InlayEquipVo>();
            int start=(int)eEquipPart.eNone+1;
            int end=(int)eEquipPart.eEnd;
            for (int i = start; i < end; i++)
            {
                _inlayInfo[(eEquipPart)i] = new InlayEquipVo();
            }
        }

        public void Initial()
        {
            _selectGemVo = null;
            _selectVo = null;
            StrengThenManager.Instance.AddEquipData();
            Gate.instance.sendNotification(MsgConstant.MSG_INLAY_DISPLAY_EQUIPLIST);
        }
        public void UpdateView()
        {
            StrengThenManager.Instance.AddEquipData();
            Gate.instance.sendNotification(MsgConstant.MSG_INLAY_DISPLAY_EQUIPLIST);
        }


        /// <summary>
        /// 镶嵌宝石
        /// </summary>
        public void InlayGem()
        {
            if (_selectGemVo==null)     //没有选择宝石
            {
                ViewHelper.DisplayMessageLanguage("inlay_no_select_gem");
                return;
            }
            int inlayIndex = -1;
            for (int i = 0; i < _inlayInfo[_selectVo.EquipType].Gems.size; i++)
            {
                if (_inlayInfo[_selectVo.EquipType].Gems[i].Value &&
                    _inlayInfo[_selectVo.EquipType].Gems[i].Id == 0
                    )
                {
                    inlayIndex = i;
                    break;
                }   
            }
            if (inlayIndex != -1)
            {
                _ask30.part = (byte)_selectVo.EquipType;
                _ask30.index = (byte)inlayIndex;
                _ask30.id = (uint)_selectGemVo.Gem.Id;
                NetBase.GetInstance().Send(_ask30.ToBytes());
            }
            else {
                ViewHelper.DisplayMessageLanguage("inlay_no_gemCount");
            }
        }

        /// <summary>
        /// 移除当前装备的宝石
        /// </summary>
        /// <param name="id"></param>
        public void RemoveGem(int id)
        {
            _ask30.part = (byte)_selectVo.EquipType;
            _ask30.index = (byte)id;
            _ask30.id = 0;
            NetBase.GetInstance().Send(_ask30.ToBytes());
        }
        /// <summary>
        /// 选择要镶嵌的宝石
        /// </summary>
        /// <param name="id"></param>
        public void SelectGemItem(int id)
        {
            if (id==0)
            {
                _selectGemVo = null;
            }
            _selectGemVo = FindInlayVoById(id);

        }
        private InlayVo FindInlayVoById(int id)
        {
            foreach (InlayVo vo in _selectGems)
            {
                if (vo.ItemInfo.InstanceId == id)
                    return vo;
            }
            return null;
        }

        /// <summary>
        /// 选择要镶嵌的装备
        /// </summary>
        /// <param name="id"></param>
        public void SelectItem(int id)
        {
            if (id==0)
            {
                _selectVo = null;
            }else
                _selectVo = StrengThenManager.Instance.GetVoByInstanceIdInEquip(id);
            SortGems();
            SortCurSelectGem();
            Gate.instance.sendNotification(MsgConstant.MSG_INLAY_DISPLAY_INFO);
        }
        public void ResetData()
        {
            SortGems();
            SortCurSelectGem();
        }


        //添加当前所有的宝石
        public void SortGems()
        {
            _allGems.Clear();
            eEquipPart part= _selectVo.EquipType;//装备位置
            IList<ItemInfo> list= BagManager.Instance.GetAllItem();

            foreach (ItemInfo item in list)
            {
                if (item.Item.itemType==eItemType.eGem)
                {
                    InlayVo vo = new InlayVo();
                    vo.ItemInfo = item;
                    vo.Gem = MergeManager.Instance.GetGemVoById((int)item.Id);
                    vo.GemItem = ItemManager.GetInstance().GetTemplateByTempId(item.Id);
                    _allGems.Add(vo);
                }
            }
        }

        public void SortCurSelectGem()
        {
            _selectGems.Clear();

            foreach (InlayVo vo in _allGems)
            {
                foreach (eEquipPart part in vo.Gem.Equips)
                {
                    if (part==_selectVo.EquipType)
                    {
                        _selectGems.Add(vo);
                        break;
                    }
                }
            }
        }


        public BetterList<EquipmentVo> Equips
        {
            get { return StrengThenManager.Instance.Equips; }
        }
        public EquipmentVo SelectVo
        {
            get { return _selectVo; }
            set { _selectVo = value; }
        }
        public BetterList<InlayVo> SelectGems
        {
            get { return _selectGems; }
            set { _selectGems = value; }
        }
        public BetterList<InlayVo> AllGems
        {
            get { return _allGems; }
            set { _allGems = value; }
        }
        public Dictionary<eEquipPart, InlayEquipVo> InlayInfo
        {
            get { return _inlayInfo; }
            set { _inlayInfo = value; }
        }
        #region 单例
        static InlayManager _instance;
        public static InlayManager Instance
        {
            get {
                if (_instance==null)
                {
                    _instance = new InlayManager();
                }
                return InlayManager._instance; 
            }
        }
        #endregion
    }
}   
