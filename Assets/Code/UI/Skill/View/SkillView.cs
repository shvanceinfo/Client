using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using model;
using manager;
using MVC.entrance.gate;
using mediator;
using helper;

public class SkillView : MonoBehaviour {

    private Transform table1Obj;
    private Transform table2Obj;
    private GameObject TalentItemPrefab;
    private Transform _talentgrid;

    private GameObject SkillItemPrefab;
    private Transform _skillGrid;

    private UITexture _descIconSprite;//详细信息icon
    private UILabel _descLblTitle;  //详细信息名字
    private UILabel _descLblLevel;
    private UILabel _descLblDescription;
    private UILabel _descLblAttack;

    private UILabel _descButtonLabel;

    private UILabel _descLbl1;          //升级提升
    private UILabel _descLbl2;          //升级消耗

    private int skill_SelectID;//选择的ID

    private const string COLOR_RED = "ff0000";
    private const string COLOR_GREEN = "12ef08";
    public int TableID { get; set; }

    private ItemLabel[] comsumeitem;
    private const int MAX_COMSUME = 4;
    private GameObject prefEffect;
    private GameObject prefEffectCircle;
    private GameObject btnLevelUp;

    private GameObject obj;
    private const string UI_TRAIL = "Effect/Effect_Prefab/UI/UI_Trail";
    private void Awake()
    {
        _descIconSprite = transform.FindChild("Table_1/Description/Func/Title/icon").GetComponent<UITexture>();
        _descLblTitle = transform.FindChild("Table_1/Description/Func/Title/name").GetComponent<UILabel>();
        _descLblLevel = transform.FindChild("Table_1/Description/Func/Title/Lbl_Level").GetComponent<UILabel>();
        _descLblDescription = transform.FindChild("Table_1/Description/Func/Title/desc").GetComponent<UILabel>();
        _descLblAttack = transform.FindChild("Table_1/Description/Func/Desc/lbl_xiaoguo").GetComponent<UILabel>();

        btnLevelUp = transform.FindChild("Table_1/Description/Func/Desc/Btn_Level").gameObject;
        _descButtonLabel = btnLevelUp.transform.FindChild("Label").GetComponent<UILabel>();
        _descLbl1 = transform.FindChild("Table_1/Description/Func/Desc/lbl1").GetComponent<UILabel>();
        _descLbl2 = transform.FindChild("Table_1/Description/Func/Desc/lbl2").GetComponent<UILabel>();
        comsumeitem = new ItemLabel[MAX_COMSUME];
        for (int i = 0; i < MAX_COMSUME; i++)
        {
            comsumeitem[i] = transform.FindChild("Table_1/Description/Func/comsume/" + i).GetComponent<ItemLabel>();
        }

        table1Obj = transform.FindChild("Table_1");
        table2Obj = transform.FindChild("Table_2");

        prefEffectCircle = transform.FindChild("skill_bk/EffectLevelUp").gameObject;


        if (BundleMemManager.Instance.isTypeInCache(EBundleType.eBundleUIEffect))
        {
            BundleMemManager.Instance.loadPrefabViaWWW<GameObject>(EBundleType.eBundleUIEffect, UI_TRAIL,
                (asset) =>
                {
                    SetStart((GameObject)asset);
                });
        }
        else
        {
            GameObject asset = BundleMemManager.Instance.getPrefabByName(UI_TRAIL, EBundleType.eBundleUIEffect);
            SetStart(asset);
        }


        TalentItemPrefab = transform.FindChild("Table_2/Func/ListPanel/TalentItem").gameObject;
        _talentgrid = transform.FindChild("Table_2/Func/ListPanel/Grid");

        SkillItemPrefab = transform.FindChild("Table_1/Skill_ScorllView/DragPanel/Skill").gameObject;
        _skillGrid = transform.FindChild("Table_1/Skill_ScorllView/DragPanel/Grid");
    }

    private void SetStart(GameObject obj)
    {
        GameObject objNew = BundleMemManager.Instance.instantiateObj(obj);
        objNew.transform.parent = transform.FindChild("UI_Trail");
        objNew.transform.localPosition = Vector3.zero;
        objNew.transform.localScale = Vector3.one;
    }

    private void OnEnable()
    {
        Gate.instance.registerMediator(new SkillMediator(this));
    }
    private void OnDisable()
    {
        Gate.instance.removeMediator(MediatorName.SKILL_MEDIATOR);
    }
    private void Start()
    {
//        Gate.instance.sendNotification(MsgConstant.MSG_SKILL_TABLE_SWITCHING, 1);
    }

    //切换到table2
    public void OnTableToTalent()
    {
        table1Obj.gameObject.SetActive(false);
        table2Obj.gameObject.SetActive(true);
        //发送请求天赋数据
        Gate.instance.sendNotification(MsgConstant.MSG_SKILL_INITIAL_TALENTTABLE);
        
    }

    public void OnTableToSkill()
    {
        table2Obj.gameObject.SetActive(false);
        table1Obj.gameObject.SetActive(true);
        Gate.instance.sendNotification(MsgConstant.MSG_SKILL_INITIAL_SKILL_LIST);
    }
    


   
    //技能列表被点击
    public void OnSkillItemClick(GameObject obj)
    {
        skill_SelectID = int.Parse(obj.name);
        DisplayInfo();
    }

    
    //升级解锁按钮事件
    public void OnSkillItemLevelUp()
    {
        bool islock=false;
        int id = GetXmlID(skill_SelectID, out islock);
        if (islock)
        {
            Gate.instance.sendNotification(MsgConstant.MSG_SKILL_UNLOCK_SKILL, id);
        }
        else {
            Gate.instance.sendNotification(MsgConstant.MSG_SKILL_LEVEL_SKILL, id);
        }
        
    }

    public void ShowErrMsg(string msgSymbol)
    {
        
        FloatMessage.GetInstance().PlayFloatMessage(
            msgSymbol, 
            UIManager.Instance.getRootTrans(),
            new Vector3(0f, 0, -150f), new Vector3(0f, 80f, -150f));
    }



    public void RerushSkillUI()
    {
        BetterList<SkillVo> actives = SkillTalentManager.Instance.ActiveSkills;
        actives.Sort((SkillVo v1,SkillVo v2) => 
        {
            if (v1.Active_Level > v2.Active_Level)
            {
                return 1;
            }
            return -1;
        });
        BetterList<SkillVo> lockItems = SkillTalentManager.Instance.LockSkills;
        lockItems.Sort((SkillVo v1, SkillVo v2) =>
        {
            if (v1.Active_Level > v2.Active_Level)
            {
                return 1;
            }
            return -1;
        });
        for (int i = 0; i < actives.size; i++)
        {
            DisplayItem(actives[i],false);
        }
        for (int i = 0; i < lockItems.size; i++)
        {
            DisplayItem(lockItems[i], true);
        }
        if (skill_SelectID == 0)
        {
            if (_skillGrid.childCount > 0)
            {
                Transform t = _skillGrid.GetChild(0);
                if (t != null)
                {
                    skill_SelectID = int.Parse(t.name);
                }
            }
        }
        _skillGrid.GetComponent<UIGrid>().Reposition();
        DisplayInfo();
    }

    //根据短ID获取长ID
    private int GetXmlID(int SID,out bool Islock)
    {
        
        Transform t = _skillGrid.FindChild(SID.ToString());
        if (t != null)
        {
            SkillItem item = t.GetComponent<SkillItem>();
            bool isLock = item.Islock;
            Islock = isLock;
            if (isLock)
            {
                BetterList<SkillVo> lockItems = SkillTalentManager.Instance.LockSkills;
                for (int i = 0; i < lockItems.size; i++)
                {
                    if (lockItems[i].SID == SID)
                    {
                        return lockItems[i].XmlID;
                    }
                }
            }
            else
            {
                BetterList<SkillVo> actives = SkillTalentManager.Instance.ActiveSkills;
                for (int i = 0; i < actives.size; i++)
                {
                    if (actives[i].SID == SID)
                    {
                        return actives[i].XmlID;
                    }
                }
            }
        }
		Islock=false;
        return 0;
    }

    public void DisplayEffectTalent(int id)
    {
        TalentVo tv = SkillTalentManager.Instance.TalentHash[id] as TalentVo;
        Transform t = _talentgrid.FindChild(tv.SId.ToString());
        Vector3 pos0 = ViewHelper.UIPositionToCameraPosition(UIManager.Instance.getRootTrans().FindChild("Camera").GetComponent<Camera>(),
                        NPCManager.Instance.ModelCamera, t.FindChild("Button").position);
        Vector3 pos1 = ViewHelper.UIPositionToCameraPosition(UIManager.Instance.getRootTrans().FindChild("Camera").GetComponent<Camera>(),
                        NPCManager.Instance.ModelCamera, t.FindChild("icon_bg").position);
        EffectShow(pos0, pos1, t.FindChild("icon_bg").position);
    }

    private void EffectShow(Vector3 pos0,Vector3 pos1,Vector3 pos2)
    {
        prefEffect = transform.FindChild("UI_Trail").gameObject;
        prefEffect.transform.GetChild(0).gameObject.SetActive(true);
        NPCManager.Instance.createCamera(true);
        GameObject obj = BundleMemManager.Instance.instantiateObj(prefEffect.transform.GetChild(0).gameObject) as GameObject;
        obj.layer = LayerMask.NameToLayer("TopUI");
        obj.transform.parent = NPCManager.Instance.ModelCamera.transform;
        obj.transform.localPosition = new Vector3(pos0.x, pos0.y, 10);
        obj.transform.localScale = Vector3.one;

        obj.GetComponent<TweenPosition>().from = new Vector3(pos0.x, pos0.y, 10);
        obj.GetComponent<TweenPosition>().to = new Vector3(pos1.x, pos1.y, 10);

        obj.GetComponent<TweenPosition>().ResetToBeginning();
        obj.GetComponent<TweenPosition>().PlayForward();

        for (int i = 0; i < obj.transform.childCount; i++)
        {
            obj.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("TopUI");
        }
        StartCoroutine(DisplayEffectCircle(pos2));
        SkillTalentManager.Instance.effectObjList.Add(obj);
        prefEffect.transform.GetChild(0).gameObject.SetActive(false);
        Destroy(obj, 1.5f);
    }

    public void DisplayEffect()
    {
        Vector3 pos0 = ViewHelper.UIPositionToCameraPosition(UIManager.Instance.getRootTrans().FindChild("Camera").GetComponent<Camera>(),
                        NPCManager.Instance.ModelCamera, btnLevelUp.transform.position);
        Vector3 pos1 = ViewHelper.UIPositionToCameraPosition(UIManager.Instance.getRootTrans().FindChild("Camera").GetComponent<Camera>(),
                        NPCManager.Instance.ModelCamera, _descIconSprite.transform.position);
        EffectShow(pos0, pos1, _descIconSprite.transform.position);
    }

    IEnumerator DisplayEffectCircle(Vector3 pos)
    {
        yield return new WaitForSeconds(1.2f);
        prefEffectCircle.SetActive(true);
        GameObject obj = BundleMemManager.Instance.instantiateObj(prefEffectCircle) as GameObject;
        obj.transform.parent = transform.FindChild("skill_bk");
        obj.transform.position = pos;
        obj.transform.localScale = Vector3.one;
        prefEffectCircle.SetActive(false);
        Destroy(obj,1f);
    }

    public void DisplayInfo()
    {
        Transform t = _skillGrid.FindChild(skill_SelectID.ToString());
        if (t!=null)
        {
            SkillItem item = t.GetComponent<SkillItem>();
            bool isLock= item.Islock;

            if (isLock)
            {
                BetterList<SkillVo> lockItems = SkillTalentManager.Instance.LockSkills;
                for (int i = 0; i < lockItems.size; i++)
                {
                    if (lockItems[i].SID==skill_SelectID)
                    {
                        DisPlayItemInfo(lockItems[i], isLock);
                        break;
                    }
                }
            }
            else {
                BetterList<SkillVo> actives = SkillTalentManager.Instance.ActiveSkills;
                for (int i = 0; i < actives.size; i++)
                {
                    if (actives[i].SID == skill_SelectID)
                    {
                        DisPlayItemInfo(actives[i], isLock);
                        break;
                    }
                }
            }


        }

    }
    private void DisPlayItemInfo(SkillVo sv,bool isLock)
    {
        _descIconSprite.mainTexture = SourceManager.Instance.getTextByIconName(sv.Icon, PathConst.SKILL_PATH);//详细信息icon
        _descLblTitle.text = sv.Name;  //详细信息名字
        _descLblLevel.text = sv.Level.ToString();
        
        _descLblAttack.text = sv.SkillLevelDescription;
        if (isLock)
        {
            _descButtonLabel.text = "解锁";
            _descLbl1.text = "[fff555]技能效果:[-]";
            _descLbl2.text = "[fff555]解锁消耗:[-]";
            //_descGold1.spriteName = SourceManager.Instance.getIconByType((eGoldType)sv.UnLockType);
            //_descGold2.enabled = false;
            //_descComsumeValue1.text = formatColor((eGoldType)sv.UnLockType, sv.UnLockValue);
            //_descComsumeValue2.text = "";
            _descLblDescription.text = string.Format("[{0}]{1}[-]级自动解锁,消耗钻石可直接解锁",COLOR_RED,sv.Active_Level);
            for (int i = 1; i < comsumeitem.Length; i++)
            {
                comsumeitem[i].gameObject.SetActive(false);
            }
            comsumeitem[0].QualityNoBoder = eItemQuality.eOrange;
            comsumeitem[0].Icon = SourceManager.Instance.getIocnStringByType((eGoldType)sv.UnLockType);
            comsumeitem[0].Lable = formatColor((eGoldType)sv.UnLockType, sv.UnLockValue);
        }
        else
        {
            _descButtonLabel.text = "升级";
            _descLbl1.text = "[fff555]升级提升:[-]";
            _descLbl2.text = "[fff555]升级消耗:[-]";

            for (int i = 0; i < comsumeitem.Length; i++)
            {
                comsumeitem[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < sv.Consume.size; i++)
            {
                comsumeitem[i].gameObject.SetActive(true);
                TypeStruct ts=sv.Consume[i];
                if (ts.Type == ConsumeType.Gold)
                {
                    comsumeitem[i].QualityNoBoder = eItemQuality.eOrange;
                    comsumeitem[i].Icon = SourceManager.Instance.getIocnStringByType((eGoldType)ts.Id);
                    comsumeitem[i].Lable = formatColor((eGoldType)ts.Id, ts.Value);
                }
                else {
                    comsumeitem[i].QualityNoBoder = ItemManager.GetInstance().GetTemplateByTempId((uint)ts.Id).quality;
                    comsumeitem[i].Icon = ItemManager.GetInstance().GetTemplateByTempId((uint)ts.Id).icon;
                    comsumeitem[i].Lable = formatColor(ts.Id, ts.Value);
                }
            }


            _descLblDescription.text = sv.SzDesc;
        }
    }

    private void DisplayItem(SkillVo sv,bool isLock)
    {
        Transform t= _skillGrid.FindChild(sv.SID.ToString());

        if (t == null)
        {
            AddSkillItem(sv.SID);
        }
        t = _skillGrid.FindChild(sv.SID.ToString());
        SkillItem si= t.GetComponent<SkillItem>();
        if (isLock)
        {
            si.Display(sv.Icon, sv.Name,
            "0/100",
            string.Format("[{0}]{1}[-]级解锁", COLOR_RED, sv.Active_Level), isLock);
        }
        else {
            si.Display(sv.Icon, sv.Name,
            string.Format("{0}/100",sv.Level),
            sv.SkillDescription, isLock);
        }
        
    }

    private void AddSkillItem(int id)
    {
		SkillItemPrefab.SetActive(true);
        GameObject obj = BundleMemManager.Instance.instantiateObj(SkillItemPrefab);
        obj.transform.parent = _skillGrid;
        obj.name = id.ToString();
        obj.transform.localPosition = new Vector3(0, 0, 0);
        obj.transform.localScale = new Vector3(1, 1, 1);
		SkillItemPrefab.SetActive(false);
    }



    //天赋升级按钮事件
    public void OnTalentClick(GameObject obj)
    {
        int id = obj.transform.parent.GetComponent<TalentItem>().ID;

        Gate.instance.sendNotification(MsgConstant.MSG_SKILL_LEVEL_TALENT,
            id);
    }

    public void RerushTalentUI()
    {
        BetterList<TalentVo> tvs = SkillTalentManager.Instance.ActiveTalents;

        for (int i = 0; i < tvs.size; i++)
        {
            DisplayTalent(tvs[i]);
        }
    }

    private void DisplayTalent(TalentVo tv)
    {
        Transform t = _talentgrid.FindChild(tv.SId.ToString());
        if (t == null)
        {
            AddTalentObj(tv.SId);
        }
        
        t = _talentgrid.FindChild(tv.SId.ToString());
        TalentItem ta = t.GetComponent<TalentItem>();
        string type = EquipmentManager.GetEquipAttributeName((eFighintPropertyCate)tv.TalentType);
        string money=formatColor((eGoldType)tv.ComsumeType, tv.ComsumeValue);
        ta.Display(tv.XmlId,tv.Icon,
            string.Format("[fff000]{0}[-]", tv.Name),
            string.Format("{0}/100", tv.Level),
            money, type, string.Format("+{0}", PowerManager.Instance.ChangeInfoData((eFighintPropertyCate)tv.TalentType, tv.TalentValue)), tv.Description, SourceManager.Instance.getIconByType((eGoldType)tv.ComsumeType));
        _talentgrid.GetComponent<UIGrid>().Reposition();
    }



    private void AddTalentObj(int id)
    {
        TalentItemPrefab.SetActive(true);
        GameObject obj = BundleMemManager.Instance.instantiateObj(TalentItemPrefab);
        obj.transform.parent = _talentgrid;
        obj.name = id.ToString();
        obj.transform.localPosition = new Vector3(0, 0, 0);
        obj.transform.localScale = new Vector3(1, 1, 1);
        TalentItemPrefab.SetActive(false);
    }


    public string formatColor(eGoldType type, int comsumeNum)
    {
        if (ViewHelper.CheckIsHava(type, comsumeNum, false))
        {
            return string.Format("[{0}]{1}[-]", ColorConst.Color_Green, comsumeNum);
        }
        else {
            return string.Format("[{0}]{1}[-]", ColorConst.Color_Red, comsumeNum);
        }
        
    }
    public string formatColor(int id, int comsumeNum)
    {
        if (ViewHelper.CheckIsHave(id, comsumeNum,false))
        {
            return string.Format("[{0}]{1}[-]", ColorConst.Color_Green, comsumeNum);
        }
        else
        {
            return string.Format("[{0}]{1}[-]", ColorConst.Color_Red, comsumeNum);
        }

    }

  
}
