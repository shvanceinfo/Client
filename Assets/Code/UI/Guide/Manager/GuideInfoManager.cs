using UnityEngine;
using System.Collections;
using model;
using System.Collections.Generic;

namespace manager
{
    public class GuideInfoManager
    {
        private Hashtable _infoHash;

        public Hashtable InfoHash
        {
            get { return _infoHash; }
            set { _infoHash = value; }
        }

        private List<GuideInfoData> _triggers;
        private List<GuideInfoData> _triggerComplates;  //完成trigger
        private bool _isFristLogin;

        public bool IsFristLogin
        {
            get { return _isFristLogin; }
            set { _isFristLogin = value; }
        }
        private GuideInfoManager()
        {
            _isFristLogin = true;
            _infoHash = new Hashtable();
            _triggers = new List<GuideInfoData>();
            _triggerComplates = new List<GuideInfoData>();
        }
        #region 单例
        private static GuideInfoManager _instance;
        public static GuideInfoManager Instance
        {
            get
            {
                if (_instance == null) _instance = new GuideInfoManager();
                return _instance;
            }
        }
        #endregion



        /// <summary>
        /// 添加触发
        /// </summary>
        public void PushTrigger(GuideInfoData data)
        {
            if (!_isFristLogin)
            {
                if(data.Vo.CheckLevelMin<=CharacterPlayer.character_property.getLevel()
                    && CharacterPlayer.character_property.getLevel() <= data.Vo.CheckLevelMax)
                _triggers.Add(data);
            }
        }

        //根据类型找当前激活列表数据
        public GuideInfoData FindTriggerByType(GuideInfoTrigger trigger)
        {
            foreach (var item in _triggers)
            {
                if (item.Type == trigger)
                    return item;
            }
            return null;
        }

        //检查是否在当今激活列表中
        public bool FindTriggerByType(GuideInfoTrigger trigger,int id)
        {
            foreach (var item in _triggers)
            {
                if (item.Type == trigger&&item.Id==id)
                    return true;
            }
            return false;
        }

        //检查是否触发过了
        public bool FindTriggerComplateByTypeId(GuideInfoTrigger trigger, int id)
        {
            foreach (var item in _triggerComplates)
            {
                if (item.Type == trigger && item.Id == id)
                    return true;
            }
            return false;
        }

        public GuideInfoVo FindVoByType(GuideInfoTrigger trigger)
        {
            foreach (GuideInfoVo item in _infoHash.Values)
            {
                if (item.Trigger == trigger)
                    return item;
            }
            return null;
        }

        public List<GuideInfoVo> FindVosByType(GuideInfoTrigger trigger)
        {
            List<GuideInfoVo> list = new List<GuideInfoVo>();
            foreach (GuideInfoVo item in _infoHash.Values)
            {
                if (item.Trigger == trigger)
                    list.Add(item);
            }
            return list;
        }


        

        public GuideInfoData FindTop()
        {
            if (_triggers.Count > 0)
                return _triggers[0];
            return null;
        }

        public void ProcessTrigger()
        {
            if (_triggers.Count>0)
            {
               GuideInfoData data=_triggers[0];
               FastOpenManager.Instance.OpenWindow(FindVoByType(data.Type).FunctionId);
               _triggerComplates.Add(data);
               _triggers.RemoveAt(0);
            }
        }

        //检查装备触发
        public void AddGuideTrigger(ItemStruct item)
        {
            ItemTemplate tm = ItemManager.GetInstance().GetTemplateByTempId(item.tempId);

            //必须是装备
            if (tm.itemType == eItemType.eEquip)
            {
                //必须是和自己一个职业
                if (tm.career==CharacterPlayer.character_property.career||tm.career==CHARACTER_CAREER.CC_BEGIN)
                {
                    int value=BagManager.Instance.GetPowerCompareValue(item.instanceId);
                    if (value>0)
                    {
                        PushTrigger(new GuideInfoData {
                          Id=(int)item.tempId,
                           Name=tm.name,
                            Icon=tm.icon,
                             Quality=tm.quality,
                              Type=GuideInfoTrigger.Power,
                          Vo = FindVoByType(GuideInfoTrigger.Power)
                        });
                    }
                }
            }
        }


        //检查物品触发
        public void CheckItemTrigger()
        {
            GuideInfoVo vo = FindVoByType(GuideInfoTrigger.UseItem);
            List<int> list = new List<int>();
            string[] sps = vo.Params.Split(',');
            for (int i = 0; i < sps.Length; i++)
            {
                list.Add(int.Parse(sps[i]));
            }

            foreach (int tempId in list)
            {
                ItemStruct item=ItemManager.GetInstance().FindItemInBagByTempId(tempId);
                int count = (int)ItemManager.GetInstance().GetItemNumById((uint)tempId);
                if (item != null && count>0)
                {
                    ItemTemplate tt=ItemManager.GetInstance().GetTemplateByTempId((uint)tempId);
                    if (tt.usedLevel<=CharacterPlayer.character_property.getLevel())
                    {
                        if (FindTriggerByType(GuideInfoTrigger.UseItem, (int)tempId))
                            continue;
                        if (FindTriggerComplateByTypeId(GuideInfoTrigger.UseItem, (int)tempId))
                            continue;
                        PushTrigger(new GuideInfoData
                        {
                            Id = (int)tempId,
                            Name = tt.name,
                            Icon = tt.icon,
                            Quality = tt.quality,
                            Type = GuideInfoTrigger.UseItem,
                            Vo = vo
                        });
                    }
                }
            }
        }

        //检查技能触发
        public void AddGuideTrigger(SkillVo vo)
        {
            PushTrigger(new GuideInfoData
            {
                Id = (int)vo.XmlID,
                Name = vo.Name,
                Icon = vo.Icon,
                Quality = eItemQuality.eOrange,
                Type = GuideInfoTrigger.UnLockSkill,
                Vo = FindVoByType(GuideInfoTrigger.UnLockSkill)
            });
        }

        //检查天赋触发
        public void AddGuideTrigger(TalentVo vo)
        {
            PushTrigger(new GuideInfoData
            {
                Id = (int)vo.XmlId,
                Name = vo.Name,
                Icon = vo.Icon,
                Quality = eItemQuality.eOrange,
                Type = GuideInfoTrigger.UnLockTelent,
                 Vo=FindVoByType(GuideInfoTrigger.UnLockTelent)
            });
        }

        //检查等级触发
        public void AddGuideTrigger(int level)
        {
            List<GuideInfoVo> list = FindVosByType(GuideInfoTrigger.Level);

            foreach (GuideInfoVo vo in list)
            {
                int lvl = int.Parse(vo.Params);
                if (level==lvl)
                {
                    if (FindTriggerByType(GuideInfoTrigger.Level, (int)lvl))
                        continue;
                    if (FindTriggerComplateByTypeId(GuideInfoTrigger.Level, lvl))
                        continue;
                    PushTrigger(new GuideInfoData 
                    {
                     Id=lvl,
                     Vo=vo,
                     Quality=eItemQuality.eOrange,
                      Type=GuideInfoTrigger.Level,
                       Icon=vo.IconName,
                        Name=vo.TipName
                    });
                }
            }
        }
        public void AddGuideTriggerVip(int vipLevel)
        {
            List<GuideInfoVo> list = FindVosByType(GuideInfoTrigger.VipLevel);

            foreach (GuideInfoVo vo in list)
            {
                int lvl = int.Parse(vo.Params);
                if (FindTriggerByType(GuideInfoTrigger.VipLevel, (int)lvl))
                    continue;
                if (FindTriggerComplateByTypeId(GuideInfoTrigger.VipLevel, lvl))
                    continue;
                if (vipLevel == lvl)
                {
                    PushTrigger(new GuideInfoData
                    {
                        Id = lvl,
                        Vo = vo,
                        Quality = eItemQuality.eOrange,
                        Type = GuideInfoTrigger.VipLevel,
                        Icon = vo.IconName,
                        Name = vo.TipName
                    });
                }
            }
        }

        public void AddGuideTriggerTask(int taskId)
        {
            List<GuideInfoVo> list = FindVosByType(GuideInfoTrigger.TaskCompalte);

            foreach (GuideInfoVo vo in list)
            {
                int TaskId = int.Parse(vo.Params);
                if (FindTriggerByType(GuideInfoTrigger.TaskCompalte, (int)TaskId))
                    continue;
                if (FindTriggerComplateByTypeId(GuideInfoTrigger.TaskCompalte, TaskId))
                    continue;
                if (taskId == TaskId)
                {
                    PushTrigger(new GuideInfoData
                    {
                        Id = TaskId,
                        Vo = vo,
                        Quality = eItemQuality.eOrange,
                        Type = GuideInfoTrigger.TaskCompalte,
                        Icon = vo.IconName,
                        Name = vo.TipName
                    });
                }
            }
        }
    }
}
