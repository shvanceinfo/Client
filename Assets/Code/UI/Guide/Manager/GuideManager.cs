using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using model;
using NetGame;
namespace manager
{
    public class GuideManager
    {
        private Hashtable _hash;

        private List<TipBind> _binds;         //绑定显示数据

        private List<GuideVo> _enforceList;             //强制列表

        private List<GuideVo> _noEnforceList;           //非强制列表

        private List<GuideVo> _activeEnforceList;       //激活的强制列表

       

        private List<GuideVo> _activeNoEnforceList;     //激活的非强制列表

        //唯一显示
        private TipBind _onlyBind;

        public bool IsEnforceUI { get; set; }          //是否屏蔽UI事件
        public int EnforceTagId { get; set; }

        private GCAskGuideComplate _ask;
        private GuideManager()
        {
            _ask = new GCAskGuideComplate();
            IsEnforceUI = false;
            _hash = new Hashtable();
            _binds = new List<TipBind>();
            _enforceList = new List<GuideVo>();
            _noEnforceList = new List<GuideVo>();
            _activeEnforceList = new List<GuideVo>();
            _activeNoEnforceList = new List<GuideVo>();
        }

        public void Init()
        {
            foreach (GuideVo vo in _hash.Values)
            {
                if (vo.Enforce)
                {
                    _enforceList.Add(vo);
                }
                else {
                    _noEnforceList.Add(vo);
                }
            }
            _enforceList.Sort();
            _noEnforceList.Sort();

            
        }

        public void ReLogin()
        {
            foreach (GuideVo vo in _hash.Values)
            {
                vo.Status = GuideStatus.None;
            }
            _activeEnforceList.Clear();
            _activeNoEnforceList.Clear();
            IsEnforceUI = false;
            EnforceTagId = -1;
            _onlyBind = null;
        }

        //处理服务器数据
        public void ProcessServerData(int groupid,bool iscomplate)
        {
            if (iscomplate)
            {
                foreach (GuideVo vo in Hash.Values)
                {
                    if (vo.Group == groupid)
                    {
                        vo.Status = GuideStatus.Success;
                    }
                }
            }
            
        }


        public void PushTrigger(Trigger trigger)
        {
			//激活非强制引导
			foreach (GuideVo vo in _noEnforceList)
			{
				if (trigger.Type == vo.Trigger
				    && !vo.Enforce && vo.Status == GuideStatus.None)
				{
					bool isCheck = false;
					switch (trigger.Type)
					{
					case TriggerType.LevelTo:
						isCheck = int.Parse(trigger.Param) >= int.Parse(vo.TriggerParams);
						break;
					default:
						isCheck = trigger.Param.Equals(vo.TriggerParams);
						break;
					}
					if (isCheck)
					{
						if (CheckCanTrigger(vo.Group, vo.Step))
						{
							vo.Status = GuideStatus.OnTrigger;
							_activeNoEnforceList.Add(vo);
						}
					}
				}
			}

            //激活强制引导
            foreach (GuideVo vo in _enforceList)
            {
                if (trigger.Type==vo.Trigger
                    &&vo.Enforce&&vo.Status==GuideStatus.None)
                {
                    bool isCheck = false;
                    switch (trigger.Type)
                    {
                        case TriggerType.LevelTo:
                            isCheck = int.Parse(trigger.Param)>=int.Parse(vo.TriggerParams);
                            break;
                        default:
                            isCheck = trigger.Param.Equals(vo.TriggerParams);
                            break;
                    }
                    if (isCheck)
                    {
                        //检查是否可以激活
                        if (CheckCanTrigger(vo.Group,vo.Step))
                        {
                            vo.Status = GuideStatus.OnTrigger;
                            _activeEnforceList.Add(vo);
                        }
                    }
                }   
            }

            //激活非强制引导
            foreach (GuideVo vo in _noEnforceList)
            {
                if (trigger.Type == vo.Trigger
                    && !vo.Enforce && vo.Status == GuideStatus.None)
                {
                    bool isCheck = false;
                    switch (trigger.Type)
                    {
                        case TriggerType.LevelTo:
                            isCheck = int.Parse(trigger.Param) >= int.Parse(vo.TriggerParams);
                            break;
                        default:
                            isCheck = trigger.Param.Equals(vo.TriggerParams);
                            break;
                    }
                    if (isCheck)
                    {
                        if (CheckCanTrigger(vo.Group, vo.Step))
                        {
                            vo.Status = GuideStatus.OnTrigger;
                            _activeNoEnforceList.Add(vo);
                        }
                    }
                }
            }

            //检查完成强制引导
            for (int i = 0; i < _activeEnforceList.Count; i++)
            {
                GuideVo vo = _activeEnforceList[i];
                if (vo.ComplateType==trigger.Type)
                {
                    bool isCheck = false;
                    switch (trigger.Type)
                    {
                        case TriggerType.LevelTo:
                            isCheck = int.Parse(trigger.Param) >= int.Parse(vo.ComplateParams);
                            break;
                        default:
                            isCheck = trigger.Param.Equals(vo.ComplateParams);
                            break;
                    }
                    if (isCheck)
                    {
                        //如果完成一个强制引导，解除UI屏蔽
                        IsEnforceUI = false;
                        vo.Status = GuideStatus.Success;


                        //判断前面是否有可忽略的引导
                        if (_activeEnforceList[0].Id != vo.Id)
                        {
                            List<GuideVo> removeList = new List<GuideVo>();
                            //移除强制可跳过新手引导
                            for (int j = 0; j < i + 1; j++)
                            {
                                if (_activeEnforceList[j].Id == vo.Id)
                                    continue;

                                //必须同一组
                                if (_activeEnforceList[j].Group == vo.Group)
                                {
                                    //必须小于当前Setp
                                    if (_activeEnforceList[j].Step < vo.Step)
                                    {
                                        //必须是可忽略
                                        if (_activeEnforceList[j].Ignore)
                                            removeList.Add(_activeEnforceList[j]);
                                        else
                                            Debug.LogError("新手引导:Id" + vo.Id + " 不可忽略，逻辑错误");
                                    }
                                    else
                                    {
                                        Debug.LogError("新手引导:Id" + vo.Id + " 忽略步奏大于当前完成引导，逻辑错误");
                                    }

                                }
                            }
                            //移除列表
                            foreach (GuideVo remove in removeList)
                            {
                                _activeEnforceList.Remove(remove);
                            }

                        }

                        //移除非强制可跳过新手引导
                        for (int j = 0; j < _activeNoEnforceList.Count; j++)
                        {
                            //必须相同组
                            if (_activeNoEnforceList[j].Group == vo.Group)
                            {
                                //必须非强制
                                if (!_activeNoEnforceList[j].Enforce)
                                {
                                    //必须小于当前步奏
                                    if (_activeNoEnforceList[j].Step < vo.Step)
                                    {
                                        _activeNoEnforceList.RemoveAt(j);
                                    }
                                }
                            }
                        }

                        _activeEnforceList.Remove(vo);

                        if (CheckGroupComplate(vo.Group))
                        {
                            NetSendComplate(vo.Group);
                        }
                        break;
                    }
                    
                }
            }

            //检查完成非强制引导
            for (int i = 0; i < _activeNoEnforceList.Count; i++)
            {
                GuideVo vo = _activeNoEnforceList[i];
                if (vo.ComplateType == trigger.Type)
                {
                    bool isCheck = false;
                    switch (trigger.Type)
                    {
                        case TriggerType.LevelTo:
                            isCheck = int.Parse(trigger.Param) >= int.Parse(vo.ComplateParams);
                            break;
                        default:
                            isCheck = trigger.Param.Equals(vo.ComplateParams);
                            break;
                    }
                    if (isCheck)
                    {
                        vo.Status = GuideStatus.Success;
                        //强制移除前面所有的提示
                        _activeNoEnforceList.RemoveRange(0, i + 1);

                        if (CheckGroupComplate(vo.Group))
                        {
                            NetSendComplate(vo.Group);
                        }
                        break;
                    }
                }
            }

            _activeEnforceList.Sort();
            _activeNoEnforceList.Sort(); 
        }
        
        #region 单例
        private static GuideManager _instance;
        public static GuideManager Instance
        {
            get
            {
                if (_instance == null) _instance = new GuideManager();
                return _instance;
            }
        }
        #endregion


        public void NetSendComplate(int group)
        {
            _ask.GroupId = (ushort)group;
            NetBase.GetInstance().Send(_ask.ToBytes(),false);
        }


        private GuideVo FindVoByIdSetp(int group, int step)
        {
            foreach (GuideVo vo in _hash.Values)
            {
                if (vo.Group == group && vo.Step == step)
                {
                    return vo;
                }
            }
            return null;
        }

        /// <summary>
        /// 检查是否可以触发当前Step,如果前面没有完成，则false
        /// </summary>
        /// <param name="group"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        public bool CheckCanTrigger(int group, int step)
        {
            bool cantrigger = true;
            for (int i = 0; i < step; i++)
            {
                GuideVo vo= FindVoByIdSetp(group, i);
                if (vo != null)
                {
                    cantrigger = !(vo.Status == GuideStatus.None);
                }
                else {
                    Debug.LogError("Cant Find CheckTrigger");
                    cantrigger = false;
                }
            }
            return cantrigger;
        }

        /// <summary>
        /// 检查当前组是否都完成
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public bool CheckGroupComplate(int group)
        {
            bool complate = true;
            int maxSetp = CheckGroupMaxSetp(group);
            GuideVo vo = null;
            for (int i = 0; i <maxSetp; i++)
            {
                 vo= FindVoByIdSetp(group, i);
                 if (vo != null && complate)
                 {
                     if (vo.Ignore)
                     {

                     }
                     else {
                         if (vo.Status != GuideStatus.Success)
                         {
                             complate = false;
                         }
                     }
                 }
                 else {
                     if(vo==null)
                     Debug.LogError("Cant Find CheckGroupComplate:"+group);
                 }
            }
            return complate;
        }

        /// <summary>
        /// 查找组中最大Setp
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public int CheckGroupMaxSetp(int group)
        {
            GuideVo vo = null;
            int setp = 0;
            do
            {
                vo = FindVoByIdSetp(group, setp);
                if (vo == null)
                {
                    return setp;
                }
                setp++;
            } while (true);
            
        }


        public TextAnchor FindAnchorVByBindId(int bindId)
        {
            foreach (GuideVo vo in _hash)
            {
                if (vo.TipBindId==bindId)
                {
                    return vo.Anchor;
                }
            }
            Debug.LogError("查找新手引导布局错误");
            return TextAnchor.MiddleCenter;
        }

        public GuideVo FindVoVByBindId(int bindId)
        {
            foreach (GuideVo vo in _hash.Values)
            {
                if (vo.TipBindId == bindId)
                {
                    return vo;
                }
            }
            Debug.LogError("查找新手引导绑定ID错误");
            return null;
        }

        public void AddTipBind(TipBind bind)
        {
            if (!_binds.Contains(bind))
            {
                _binds.Add(bind);
            }
            else {
                Debug.LogError("添加新手引导提示绑定，错误!");
            }
            
        }

        public void RemoveTipBind(TipBind bind)
        {
            if (_binds.Contains(bind))
            {
                _binds.Remove(bind);
            }
            else
            {
                Debug.LogError("移除新手引导提示绑定，错误!");
            }
        }


        public GuideVo FindDisplayVo(int bindId)
        {
            GuideVo vo = null;

            //先载入可忽略强制列表中第一个
            foreach (GuideVo gv in GuideManager.Instance.CurIgnoreEnforceList)
            {
                if (gv.TipBindId == bindId)
                {
                    vo = gv;
                }
            }

			//检查非强制列表中第一个对象是否为
			if (vo == null)
			{
				if (GuideManager.Instance.CurNoEnforceStep != null)
					if (GuideManager.Instance.CurNoEnforceStep.TipBindId == bindId)
						vo = GuideManager.Instance.CurNoEnforceStep;
			}

            //强制列表如果没有可忽略的对象
            if (vo == null)
            {
                //检查第一个强制引导是否为当前
                if (GuideManager.Instance.CurNoIgnoreEnforceStep != null)
                    if (GuideManager.Instance.CurNoIgnoreEnforceStep.TipBindId == bindId)
                        vo = GuideManager.Instance.CurNoIgnoreEnforceStep;
            }

            
            return vo;
        }

       


        public bool TriggerLock(TipBind bind)
        {
            //如果为空， 则塞入
            if (_onlyBind == null)
            {
                _onlyBind = bind;
                return true;
            }
            else {
                //顶掉上一个
                if (bind.Id<_onlyBind.Id)
                {
                    _onlyBind.OnHook();
                    _onlyBind = bind;
                    return true;
                }
                else if (bind == _onlyBind)
                {
                    return true;
                }
                return false;
            }


        }

        //解锁
        public void TriggerUnLock(TipBind bind)
        {
            if (_onlyBind!=null)
            {
                if (_onlyBind.Id == bind.Id)
                    _onlyBind = null;
            }
        }


        /// <summary>
        /// 配置表
        /// </summary>
        public Hashtable Hash
        {
            get { return _hash; }
        }

        /// <summary>
        /// 当前已激活强制列表中的第一个
        /// </summary>
        public GuideVo CurEnforceStep
        {
            get {
                return _activeEnforceList.Count > 0 ? _activeEnforceList[0] : null; 
            }
        }

        /// <summary>
        /// 返回当前强制列表中第一个不可忽略的引导
        /// 如果没有则返回列表中第一个
        /// </summary>
        public GuideVo CurNoIgnoreEnforceStep
        {
            get {
                foreach (GuideVo vo in _activeEnforceList)
                {
                    if (!vo.Ignore)
                        return vo;
                }
                return CurEnforceStep;
            }
        }

        /// <summary>
        /// 返回强制列表中可忽略列表
        /// </summary>
        public List<GuideVo> CurIgnoreEnforceList
        {
            get {
                List<GuideVo> list = new List<GuideVo>();
                foreach (GuideVo vo in _activeEnforceList)
                {
                    if (vo.Ignore) list.Add(vo);
                }
                return list;
            }
        }
        

        /// <summary>
        /// 当前已激活非强制
        /// </summary>
        public GuideVo CurNoEnforceStep
        {
            get
            {
                return _activeNoEnforceList.Count > 0 ? _activeNoEnforceList[0] : null;
            }
        }

        public List<GuideVo> ActiveEnforceList
        {
            get { return _activeEnforceList; }
        }


        public List<GuideVo> ActiveNoEnforceList
        {
            get { return _activeNoEnforceList; }
        }

        public bool CheckOpenNotice
        {
            get {
                return ActiveNoEnforceList.Count == 0 && ActiveEnforceList.Count == 0;
            }
        }
    }
}
