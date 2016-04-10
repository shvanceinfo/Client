using UnityEngine;
using System.Collections;
using model;
using System.Collections.Generic;
using MVC.entrance.gate;
using helper;
using NetGame;

namespace manager
{

    public class StrengThenManager
    {
        static StrengThenManager _instance;

        private BetterList<EquipmentVo> _equips;   //当前装备


        private BetterList<EquipmentVo> _bags;     //背包物品

        private Hashtable _equipPart;

        

        private Hashtable _equipForge;              //强化表

        private EquipmentVo _selectVo;             //当前选中的物品

        private Table _selectTable;                 //当前选中标签

        private GCAskIntensifyEquip _ask15;         //强化装备协议

        public int ChooseItem;                  //当前选择的item

        private StrengThenManager()
        {
            _selectVo = null;
            _equips = new BetterList<EquipmentVo>();
            _bags = new BetterList<EquipmentVo>();
            _equipForge = new Hashtable();
            _ask15 = new GCAskIntensifyEquip();
            _equipPart = new Hashtable();
        }


        public void Initial()
        {
            LuckStoneManager.Instance.SetSelectStone(0);
            _selectTable = Table.None;
            _selectVo = null;
            AddEquipData();
            AddBagData();
            Gate.instance.sendNotification(MsgConstant.MSG_STRENGTHEN_DISPLAY_LIST_TABLE, Table.Table1);
            //Gate.instance.sendNotification(MsgConstant.MSG_STRENGTHEN_SELECT_INDEX);
        }
        public void UpdateView()
        {
            AddEquipData();
            AddBagData();
            Gate.instance.sendNotification(MsgConstant.MSG_STRENGTHEN_ENFORCE_DISPLAY_LIST, _selectTable);
        }

        /// <summary>
        /// 强化返回结果，刷新界面
        /// </summary>
        public void StrengthemResultCallBack()
        {
            AddEquipData();
            AddBagData();
            SelectStrengThenItem(_selectVo.InstanceId);
            Gate.instance.sendNotification(MsgConstant.MSG_STRENGTHEN_ENFORCE_DISPLAY_LIST, _selectTable);
            ViewHelper.DisplayMessageLanguage("msg_ref_succ");
            Gate.instance.sendNotification(MsgConstant.MSG_STRENGTHEN_ENFORCE_EFFECT, _selectVo.InstanceId);
        }

        /// <summary>
        /// 强化失败
        /// </summary>
        public void StrengthemResultLose()
        {
            AddEquipData();
            AddBagData();
            SelectStrengThenItem(_selectVo.InstanceId);
            Gate.instance.sendNotification(MsgConstant.MSG_STRENGTHEN_ENFORCE_DISPLAY_LIST, _selectTable);
            ViewHelper.DisplayMessageLanguage("msg_ref_error");
        }

        /// <summary>
        /// 选择需要强化的物品
        /// </summary>
        /// <param name="id"></param>
        public void SelectStrengThenItem(int id)
        {
            switch (_selectTable)
            {
                case Table.Table1:
                    _selectVo = GetVoByInstanceIdInEquip(id);
                    break;
                case Table.Table2:
                    _selectVo = GetVoByInstanceIdInBag(id);
                    break;
                default:
                    Debug.LogError("Fuck");
                    break;
            }
            LuckStoneManager.Instance.SetSelectStone(0);
            Gate.instance.sendNotification(MsgConstant.MSG_STRENGTHEN_DISPLAT_INFO);
        }

        /// <summary>
        /// 选择幸运石回调
        /// </summary>
        public void LuckStoneCallback()
        {
            Gate.instance.sendNotification(MsgConstant.MSG_STRENGTHEN_DISPLAT_INFO);
        }

        /// <summary>
        /// 发送强化装备
        /// </summary>
        public void AskStrengthenItem()
        {
            if (_selectVo.StrengThenLevel == _selectVo.EquipData.maxForgeLevel)
            {
                return;
            }
            if (_selectVo.StrengThenLevel>=CharacterPlayer.character_property.getLevel())
            {
                ViewHelper.DisplayMessage("等级不足!");
                return;
            }
            BetterList<TypeStruct> item = _selectVo.NextStrengThen.ConsumeItem;
            foreach (TypeStruct ts in item)
            {
                if (ts.Type == ConsumeType.Item)
                {
                    //if (!ViewHelper.CheckIsHave(ts.Id, ts.Value))
                    //    return;
                    if (!FeebManager.Instance.CheckIsHave((uint)ts.Id, ts.Value))
                        return;
                }
                else {
                    if (!ViewHelper.CheckIsHava((eGoldType)ts.Id, ts.Value))
                        return;
                }
            }

            if (LuckStoneManager.Instance.SelectedStone != null)
            {
                TypeStruct ts = LuckStoneManager.Instance.SelectedStoneConsume;
                if (ts.Type == ConsumeType.Item)
                {
                    if (!ViewHelper.CheckIsHave(ts.Id, ts.Value)) return;
                }
                else
                {
                    if (!ViewHelper.CheckIsHava((eGoldType)ts.Id, ts.Value)) return;
                    
                }
            }

            _ask15.u16bagPos = (ushort)_selectVo.InstanceId;
            if (LuckStoneManager.Instance.SelectedStone != null)
            {
                _ask15.u32LuckStoneId =(uint) LuckStoneManager.Instance.SelectedStone.Id;
            }
            _ask15.option = 0x00;       //强化装备
            NetBase.GetInstance().Send(_ask15.ToBytes());
        }



        public void AddBagData()
        {
            _bags.Clear();
            IList<ItemInfo> items = BagManager.Instance.GetEquipItem();

            foreach (ItemInfo info in items)
            {
                EquipmentVo vo = new EquipmentVo();
                vo.Info = info;
                vo.Id = (int)info.Id;
                vo.InstanceId = info.InstanceId;
                vo.Item = info.Item;
                vo.EquipData = EquipmentManager.GetInstance().GetTemplateByTempId(info.Id);
                vo.InstanceEquipData = EquipmentManager.GetInstance().GetDataByItemId((uint)info.InstanceId);
                EquipmentForgeVo next = GetForgeVo(vo.StrengThenLevel + 1, vo.EquipType);
                if (next==null)
                {
                    next = GetForgeVo(vo.StrengThenLevel, vo.EquipType);

                }
                vo.CurStrengThen = GetForgeVo(vo.StrengThenLevel, vo.EquipType);
                vo.NextStrengThen = next;
                vo.NextAdvancedItem = ItemManager.GetInstance().GetTemplateByTempId(vo.EquipData.EquipmentUpId);
                vo.NextAdvanceEquip = EquipmentManager.GetInstance().GetTemplateByTempId(vo.EquipData.EquipmentUpId);
                _bags.Add(vo);
                switch (vo.EquipType)
                {
                    case eEquipPart.eNone:
                        vo.SortId = 0;
                        break;
                    case eEquipPart.eSuit:
                        vo.SortId = 2;
                        break;
                    case eEquipPart.eLeggings:
                        vo.SortId = 5;
                        break;
                    case eEquipPart.eShoes:
                        vo.SortId = 6;
                        break;
                    case eEquipPart.eNecklace:
                        vo.SortId = 3;
                        break;
                    case eEquipPart.eRing:
                        vo.SortId = 4;
                        break;
                    case eEquipPart.eGreatSword:
                        vo.SortId = 1;
                        break;
                    case eEquipPart.eArcher:
                        vo.SortId = 1;
                        break;
                    case eEquipPart.eDoublePole:
                        vo.SortId = 1;
                        break;
                    default:
                        break;
                }
            }
            _bags.Sort((EquipmentVo v1, EquipmentVo v2) =>
            {
                if (v1.SortId > v2.SortId)
                {
                    return 1;
                }
                return -1;
            });
        }


        public void AddEquipData()
        {
            _equips.Clear();
            Dictionary<eEquipPart, EquipmentStruct> datas = BagManager.Instance.EquipData;

            foreach (EquipmentStruct info in datas.Values)
            {
                EquipmentVo vo = new EquipmentVo();
                vo.Id = (int)info.templateId;
                vo.InstanceId = info.instanceId;
                vo.Item = ItemManager.GetInstance().GetTemplateByTempId(info.templateId);
                vo.EquipData = EquipmentManager.GetInstance().GetTemplateByTempId(info.templateId);
                vo.InstanceEquipData = info;
                EquipmentForgeVo next = GetForgeVo(vo.StrengThenLevel + 1, vo.EquipType);
                if (next==null)
                {
                    next = GetForgeVo(vo.StrengThenLevel , vo.EquipType);

                }
                vo.CurStrengThen = GetForgeVo(vo.StrengThenLevel, vo.EquipType);
                vo.NextStrengThen = next;
                vo.NextAdvancedItem = ItemManager.GetInstance().GetTemplateByTempId(vo.EquipData.EquipmentUpId);
                vo.NextAdvanceEquip = EquipmentManager.GetInstance().GetTemplateByTempId(vo.EquipData.EquipmentUpId);
                _equips.Add(vo);
                switch (vo.EquipType)
                {
                    case eEquipPart.eNone:
                        vo.SortId = 0;
                        break;
                    case eEquipPart.eSuit:
                        vo.SortId = 2;
                        break;
                    case eEquipPart.eLeggings:
                        vo.SortId = 5;
                        break;
                    case eEquipPart.eShoes:
                        vo.SortId = 6;
                        break;
                    case eEquipPart.eNecklace:
                        vo.SortId = 3;
                        break;
                    case eEquipPart.eRing:
                        vo.SortId = 4;
                        break;
                    case eEquipPart.eGreatSword:
                        vo.SortId = 1;
                        break;
                    case eEquipPart.eArcher:
                        vo.SortId = 1;
                        break;
                    case eEquipPart.eDoublePole:
                        vo.SortId = 1;
                        break;
                    default:
                        break;
                }
            }
            _equips.Sort((EquipmentVo v1, EquipmentVo v2) =>
            {
                if (v1.SortId > v2.SortId)
                {
                    return 1;
                }
                return -1;
            });

        }

        #region Public

        /// <summary>
        /// 查找强化列表
        /// </summary>
        /// <param name="sgtLvl">强化等级</param>
        /// <param name="type">强化位置</param>
        /// <returns></returns>
        public EquipmentForgeVo GetForgeVo(int sgtLvl, eEquipPart type)
        {

            foreach (EquipmentForgeVo vo in _equipForge.Values)
            {
                if (vo.StrengThenLevel==sgtLvl&&vo.EquipType==type)
                {
                    return vo;
                }
            }
            return null;
        }

        public EquipmentVo GetVoByInstanceIdInEquip(int id)
        {
            foreach (EquipmentVo vo in _equips)
            {
                if (vo.InstanceId==id)
                {
                    return vo;
                }
            }
            return null;
        }
        public EquipmentVo GetVoByInstanceIdInBag(int id)
        {
            foreach (EquipmentVo vo in _bags)
            {
                if (vo.InstanceId == id)
                {
                    return vo;
                }
            }
            return null;
        }
        #endregion

        #region Attribute

        public Hashtable EquipPart
        {
            get { return _equipPart; }
            set { _equipPart = value; }
        }

        public Table SelectTable
        {
            get { return _selectTable; }
            set { _selectTable = value; }
        }

        public EquipmentVo SelectVo
        {
            get { return _selectVo; }
            set { _selectVo = value; }
        }
        
        public BetterList<EquipmentVo> Bags
        {
            get { return _bags; }
            set { _bags = value; }
        }
        public BetterList<EquipmentVo> Equips
        {
            get { return _equips; }
            set { _equips = value; }
        }

        /// <summary>
        /// 强化表
        /// </summary>
        public Hashtable EquipForge
        {
            get { return _equipForge; }
            set { _equipForge = value; }
        }

        public static StrengThenManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new StrengThenManager();
                }
                return StrengThenManager._instance;
            }
        }
        /// <summary>
        /// 根据职业获取，职业名
        /// </summary>
        /// <param name="career"></param>
        /// <returns></returns>
        public static string GetStringByCareer(CHARACTER_CAREER career)
        {
            switch (career)
            {
                case CHARACTER_CAREER.CC_BEGIN:
                    return "无限制";
                case CHARACTER_CAREER.CC_SWORD:
                    return "剑士";
                case CHARACTER_CAREER.CC_ARCHER:
                    return "弓箭手";
                case CHARACTER_CAREER.CC_MAGICIAN:
                    return "魔法师";
                case CHARACTER_CAREER.CC_END:
                    break;
                default:
                    break;
            }
            return "";
        }
        public static ItemTemplate GetItemById(int id)
        {
            return ConfigDataManager.GetInstance().getItemTemplate().getTemplateData(id);
        }

        #endregion
        
    }
}