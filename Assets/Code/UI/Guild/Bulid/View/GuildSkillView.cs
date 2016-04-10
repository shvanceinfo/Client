using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using mediator;
using manager;
using model;
using helper;
using System.Collections.Generic;

public class GuildSkillView : MonoBehaviour {
    private GameObject skillObj, skillRightObj;
    private GameObject leftGrid, rightGrid;

    private GameObject skillFocusObj, focusGrid;
    private GameObject skillShow, focusShow;
    private void Awake()
    {
        skillObj = transform.FindChild("Study/DoubleItem").gameObject;
        skillRightObj = transform.FindChild("Study/Right/DoubleItem").gameObject;
        skillFocusObj = transform.FindChild("Focus/DoubleItem").gameObject;

        leftGrid = transform.FindChild("Study/ItemPanel/Grid").gameObject;
        rightGrid = transform.FindChild("Study/Right/ItemPanel/Grid").gameObject;
        focusGrid = transform.FindChild("Focus/ItemPanel/Grid").gameObject;

        skillShow = transform.FindChild("Study").gameObject;
        focusShow = transform.FindChild("Focus").gameObject;
    }
    void OnEnable()
    {
        Gate.instance.registerMediator(new GuildSkillMediator(this));
    }
    void OnDisable()
    {
        Gate.instance.removeMediator(MediatorName.GUILDSKILL_MEDIATOR);
    }

    public void SwitchSkillTable(GuildSkillType type)
    {
        switch (type)
        {
            case GuildSkillType.GuildSkillLearn:
                skillShow.SetActive(true);
                focusShow.SetActive(false);
                DisplayView();
                break;
            case GuildSkillType.GuildSkillFocus:
                focusShow.SetActive(true);
                skillShow.SetActive(false);
                DisplayFocusItem();
                break;
            default:
                break;
        }
    }

    public void DisplayView()
    {
        DisplayLeftItem();
        DisplayRightItem();
    }


    private void DisplayLeftItem()
    {
        GuildManager.Instance.ShowSkillNextVo();

        for (int i = 0; i < GuildManager.Instance.SkillNextVo.size;i++ )
        {

            skillObj.SetActive(true);
            GameObject obj = UnityEngine.Object.Instantiate(skillObj) as GameObject;
            obj.transform.parent = leftGrid.transform;
            obj.transform.localPosition = new Vector3(0, 0, 0);
            obj.transform.localScale = new Vector3(1, 1, 1);
            skillObj.SetActive(false);

            GuildSkillVo vo = GuildManager.Instance.SkillNextVo[i];
            BetterList<AttributeValue> list = vo.Attributes;

            GuildSkillDisplayItem item = obj.transform.FindChild("0").GetComponent<GuildSkillDisplayItem>();

            item.DisplayItem(vo.Name, vo.Level, vo.Attributes[0], vo.Icon);

            item.DisplayCommonItem();

            for(int k=0;k<vo.SkillConsumeLearn.Count;k++)
            {
                item.DisplayItemTypestruct(vo.SkillConsumeLearn[k]);
            }
        }

        leftGrid.GetComponent<UIGrid>().Reposition();
    }

    private void DisplayRightItem()
    {
        GuildManager.Instance.ShowSkillCurVo();
        
        for (int i = 0; i < GuildManager.Instance.SkillBeforeVo.size; i++)
        {
            skillRightObj.SetActive(true);
            GameObject obj = UnityEngine.Object.Instantiate(skillRightObj) as GameObject;
            obj.transform.parent = rightGrid.transform;
            obj.transform.localPosition = new Vector3(0, 0, 0);
            obj.transform.localScale = new Vector3(1, 1, 1);
            skillRightObj.SetActive(false);

            GuildSkillVo vo = GuildManager.Instance.SkillBeforeVo[i];
            BetterList<AttributeValue> list = vo.Attributes;

            GuildSkillDisplayItem item = obj.transform.FindChild("1").GetComponent<GuildSkillDisplayItem>();

            item.DisplayItem(vo.Name, vo.Level, vo.Attributes[0], vo.Icon);

        }

        rightGrid.GetComponent<UIGrid>().Reposition();
    }

    public void DisplayFocusItem()
    {
        GuildManager.Instance.ShowSkillFocusNextVo();

        for (int i = 0; i < GuildManager.Instance.SkillFocusNextVo.size; i++)
        {

            skillFocusObj.SetActive(true);
            GameObject obj = UnityEngine.Object.Instantiate(skillFocusObj) as GameObject;
            obj.transform.parent = focusGrid.transform;
            obj.transform.localPosition = new Vector3(0, 0, 0);
            obj.transform.localScale = new Vector3(1, 1, 1);
            skillFocusObj.SetActive(false);

            GuildSkillVo vo = GuildManager.Instance.SkillFocusNextVo[i];
            BetterList<AttributeValue> list = vo.Attributes;

            GuildSkillFocusDisplayItem item = obj.transform.FindChild("0").GetComponent<GuildSkillFocusDisplayItem>();

            item.DisplayItem(vo.Name, vo.Level, vo.Attributes[0], vo.Icon);

            item.DisplayCommonItem();

            for (int k = 0; k < vo.SkillConsumeLearn.Count; k++)
            {
                item.DisplayItemTypestruct(vo.SkillConsumeLearn[k]);
            }
        }

        focusGrid.GetComponent<UIGrid>().Reposition();
    }
}
