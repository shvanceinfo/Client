using UnityEngine;
using System.Collections;
using model;
using MVC.entrance.gate;
using System.Collections.Generic;
using helper;
using NetGame;
namespace manager
{
    public class RefineManager
    {
        public const int MAX_REFINE_SIZE = 6;
        private Hashtable _refineHash;

        private Table _selectTable;
        private EquipmentVo _selectVo;             //当前选中的物品
        private BetterList<RefineInfoVo> _equipInfo;

        private BetterList<int> _resetList;         //重置条目

        private GCAskWashEquip _ask29;

        private int _selectId;

        private bool NextMsgIsReset = false;
        private int _selectRefineItem;          //当前选择的洗练按钮
        public int SelectRefineItem;          //当前选择的洗练item

        private RefineManager()
        {
            _resetList = new BetterList<int>();
            _refineHash = new Hashtable();
            _equipInfo = new BetterList<RefineInfoVo>();
            _ask29 = new GCAskWashEquip();
        }

        public void Initial()
        {
            _resetList.Clear();
            _selectTable = Table.None;
            _selectVo = null;
            StrengThenManager.Instance.AddEquipData();
            StrengThenManager.Instance.AddBagData();
            Gate.instance.sendNotification(MsgConstant.MSG_REFINE_DISPLAY_LIST_TABLE,Table.Table1);
        }
        public void UpdateView()
        {
            StrengThenManager.Instance.AddEquipData();
            StrengThenManager.Instance.AddBagData();
            Gate.instance.sendNotification(MsgConstant.MSG_REFINE_ENFORCE_DISPLAY_LIST, _selectTable);
        }

        public void SelectItem(int id)
        {
            switch (_selectTable)
            {
                case Table.Table1:
                    _selectVo = StrengThenManager.Instance.GetVoByInstanceIdInEquip(id);
                    break;
                case Table.Table2:
                    _selectVo = StrengThenManager.Instance.GetVoByInstanceIdInBag(id);
                    break;
                default:
                    Debug.LogError("Fuck");
                    break;
            }
            if (_selectVo != null)
            {
                _selectId = _selectVo.InstanceId; 
            }
            GetAttribute();
            Gate.instance.sendNotification(MsgConstant.MSG_REFINE_DISPLAT_INFO);
        }

        /// <summary>
        /// 重新显示当前物品
        /// </summary>
        public void UpdateSelectVoInfo(bool success)
        {
            if (NextMsgIsReset)     //重置成功
            {
                NextMsgIsReset = false;
                ViewHelper.DisplayMessageLanguage("refine_reset_success");
            }else if (success)      //洗练成功
            {
                ViewHelper.DisplayMessageLanguage("refine_success");
                Gate.instance.sendNotification(MsgConstant.MSG_REFINE_EFFECT_INFO, _selectRefineItem);
            }
            if (_selectVo != null)
            {

                StrengThenManager.Instance.AddEquipData();
                StrengThenManager.Instance.AddBagData();
                SelectItem(_selectId);
            }
            
        }

        public void GetAttribute()
        {
            _equipInfo.Clear();

            if (_selectVo == null) return;
            Dictionary<int,AttributeValue> ra =_selectVo.InstanceEquipData.RefineAtrb;
            for (int i = 0; i < MAX_REFINE_SIZE; i++)
            {
                _equipInfo.Add(new RefineInfoVo());
                if (ra.ContainsKey(i))
                {
                    if (ra[i].Type == eFighintPropertyCate.eFPC_None)
                    {
                        _equipInfo[i].Status = RefineStatus.Standby;
                    }
                    else
                    {
                        _equipInfo[i].Type = ra[i].Type;
                        _equipInfo[i].BaseValue = ra[i].Value;
                        _equipInfo[i].Status = RefineStatus.UnLock;
                    }
                }
				_equipInfo[i].Vo = FindVoById(i+1);
            }
            
        }
        public RefineVo FindVoById(int id)
        {
            return _refineHash[id] as RefineVo;
        }

        //清除，重置列表
        public void ClaerResetList()
        {
            _resetList.Clear();
        }
        //选择，重置列表条目
        public void SelectResetItem(int index)
        {
            bool b= _resetList.Contains(index);
            if (b)
            {
                _resetList.Remove(index);
            }
            else {
                _resetList.Add(index);
            }
        }

        //发送洗练属性
        public void SendRefine(int id)
        {

            _selectRefineItem = id;
            int mId = id;
            id = id * 2;
            RefineVo vo=FindVoById(id+1);
            if (CheckIsCantGo(_equipInfo[id],_equipInfo[id+1]))
            {
                ViewHelper.DisplayMessageLanguage("refine_attribute_max");
                return;
            }
            
            int needCount = vo.ConsumeItems[0].Value;
            int haveCount = LuckStoneManager.Instance.GetConsumeItem(vo.ConsumeItems[0].Id);

            if (FeebManager.Instance.CheckIsHave((uint)vo.ConsumeItems[0].Id,needCount))
            {
                _ask29.instanceId = (ushort)_selectVo.InstanceId;
                _ask29.isReset = 0x00;
                _ask29.index = (byte)mId;
                NetBase.GetInstance().Send(_ask29.ToBytes());
            }
            //else {

            //    ViewHelper.DisplayMessage(string.Format(LanguageManager.GetText("refine_consume_nothave"), StrengThenManager.GetItemById(vo.ConsumeItems[0].Id).name));
            //}
        }
        private bool CheckIsCantGo(RefineInfoVo vo1, RefineInfoVo vo2)
        {
            if (vo1.Status==RefineStatus.UnLock&&vo2.Status==RefineStatus.UnLock&&
                IsMax(vo1)&&IsMax(vo2)
                )
            {
                return true;
            }
            else if (vo1.Status == RefineStatus.UnLock && vo2.Status == RefineStatus.Lock &&
               IsMax(vo1) )
            {
                return true;
            }
            else if (vo1.Status == RefineStatus.Lock && vo2.Status == RefineStatus.UnLock &&
                IsMax(vo2))
            {
                return true;
            }
            else {
                return false;
            }
        }
        private bool IsMax(RefineInfoVo vo)
        {
            if (vo.Status==RefineStatus.UnLock&&vo.BaseValue==vo.Vo[vo.Type])
            {
                return true;
            }
            return false;
        }

        //重置选择的物品
        public void SendResetRefine()
        {
            if (_resetList.size==0)
            {
                ViewHelper.DisplayMessageLanguage("refine_not_reset");
                return;
            }

            int need = 0;
            int have = LuckStoneManager.Instance.GetConsumeItem(ResetItemID);
            BetterList<int> bs = ResetList;
            for (int i = 0; i < bs.size; i++)
            {
                need += EquipInfo[bs[i]].Vo.ResetConsume[0].Value;
            }
            if (!FeebManager.Instance.CheckIsHave((uint)EquipInfo[bs[0]].Vo.ResetConsume[0].Id,need))
            {
                return;
            }
            if (have >= need)
            {
                UIManager.Instance.ShowDialog(eDialogSureType.eRefine_Reset, LanguageManager.GetText("refine_areyousure"));
            }
            else {
                ViewHelper.DisplayMessage(string.Format(LanguageManager.GetText("refine_consume_nothave"), StrengThenManager.GetItemById(ResetItemID).name));
            }

        }

        public void SendResetNet()
        {
            
            int sun = 0;
            foreach (int index in _resetList)
            {
                sun += (int)Mathf.Pow(2.0f,(float)index);
            }
            _ask29.instanceId = (ushort)_selectVo.InstanceId;
            _ask29.isReset = 0x01;
            _ask29.index = (byte)sun;
            NetBase.GetInstance().Send(_ask29.ToBytes());
            Gate.instance.sendNotification(MsgConstant.MSG_REFINE_RESET, false);
            NextMsgIsReset = true;
        }

        #region Attribute

        public int ResetItemID
        {
            get {
                return FindVoById(1).ResetConsume[0].Id;
            }
        }

        public BetterList<int> ResetList
        {
            get { return _resetList; }
            set { _resetList = value; }
        }
        public BetterList<RefineInfoVo> EquipInfo
        {
            get { return _equipInfo; }
            set { _equipInfo = value; }
        }

        public Hashtable RefineHash
        {
            get { return _refineHash; }
            set { _refineHash = value; }
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
            get { return StrengThenManager.Instance.Bags; }
        }

        public BetterList<EquipmentVo> Equips
        {
            get { return StrengThenManager.Instance.Equips; }
        }
        #endregion

        #region 单例
        private static RefineManager _instance;
        public static RefineManager Instance
        {
            get
            {
                if (_instance == null) _instance = new RefineManager();
                return _instance;
            }
        }
        #endregion
        
    }

}