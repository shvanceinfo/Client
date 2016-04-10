using UnityEngine;
using System.Collections;
using model;
using manager;
using MVC.entrance.gate;
using mediator;
using helper;

public class MedalTipsView : MonoBehaviour
{
    private GameObject medalPrefab;
    private UIGrid medalGridLeft;

    #region LEFT
    private UILabel _leftName;      // 左边勋章名称
    private UITexture _leftIcon;    //左边得勋章图片
    private UILabel _leftFightPower;   //左边攻击力
    private GameObject currentIconName;      //图标名称
    private UILabel _leftHonorText;       //左边荣誉数值


    #endregion


    void Awake()
    {
        medalPrefab = transform.FindChild("Abs/Attributes").gameObject;
        medalGridLeft = transform.FindChild("Panel/GridLeft").GetComponent<UIGrid>();

        
        _leftName = transform.FindChild("CurrentMedal/MedalName").GetComponent<UILabel>();
        _leftIcon = transform.FindChild("CurrentMedal/MedalSprite").GetComponent<UITexture>();

        

        _leftFightPower = transform.FindChild("CurrentMedal/MedalPlusScore").GetComponent<UILabel>();

    }

    void OnEnable()
    {
        Gate.instance.registerMediator(new MedalTipsMediator(this));
    }
    void OnDisable()
    {
        Gate.instance.removeMediator(MediatorName.MEDAL_TIP_MEDIATOR);
    }

    public void Display_MedalLvlup()
    {
        //MedalVo vo = MedalManager.Instance.FindVoByLevel(MedalManager.Instance.MedalLevelUp);
    }

    public void Display_Medal()
    {
        MedalVo vo = MedalManager.Instance.FindVoByLevel(MedalManager.Instance.CurLevel);
    }

    public void Display_NextMedal()
    {
        MedalVo nextVo = MedalManager.Instance.FindVoByLevel(MedalManager.Instance.NextLevel);
    }

    

    public void DisplayMedalTips()
    {
        DisplayLeft();
        modifyFightPower();
    }

    //显示背包中勋章界面
    public void DisplayMedal_Bag()
    {
        DisplayLeft();
        modifyFightPower();
    }

    
    private void DisplayLeft()      //显示左边
    {
        DisplayLeftTitle();
        DisplayLeftList();

    }

    private void DisplayLeftTitle() //显示左边头像
    {
        _leftName.text = ColorConst.Format(ColorConst.Color_DanHuang, MedalManager.Instance.CurVo.Name);
        _leftIcon.mainTexture = SourceManager.Instance.getTextByIconName(MedalManager.Instance.CurVo.Icon, PathConst.MEDAL_ICON_PATH);

    }

    private void DisplayLeftList()  //显示左边列表
    {
        BetterList<AttributeValue> list = MedalManager.Instance.CurVo.Attributes;
        BetterList<DoubleText> _sList = new BetterList<DoubleText>();
        for (int i = 0; i < list.size; i += 2)
        {
            DoubleText text = new DoubleText();
            string str = EquipmentManager.GetEquipAttributeName(list[i].Type);
            text.A1 = string.Format("[{0}]{1}:[-]{2}", ColorConst.Color_HeSe, str, list[i].Value);
            if (i + 1 < list.size)
            {
                str = EquipmentManager.GetEquipAttributeName(list[i + 1].Type);
                text.A2 = string.Format("[{0}]{1}:[-]{2}", ColorConst.Color_HeSe, str, list[i + 1].Value);
            }
            else
            {
                text.A2 = null;
            }
            _sList.Add(text);
        }

        ViewHelper.FormatTemplate<BetterList<DoubleText>, DoubleText, MedalViewAttribute>(medalPrefab, medalGridLeft.transform, _sList,
            (DoubleText vo, MedalViewAttribute d) =>
            {
                d.LeftText = vo.A1;
                if (vo.A2 != null)
                {
                    d.RightText = vo.A2;
                }
            });
        medalGridLeft.Reposition();
    }


    private class DoubleText
    {
        public string A1 { get; set; }
        public string A2 { get; set; }
    }

    //计算战斗力数值
    private void modifyFightPower()
    {
        MedalVo vo = MedalManager.Instance.FindVoByLevel(MedalManager.Instance.CurLevel);
        MedalVo vo1 = MedalManager.Instance.FindVoByLevel(MedalManager.Instance.NextLevel);
        //string s = "";
        //_leftFightPower.text = string.Format("[{0}]{1}[-]{2}", ColorConst.Color_HeSe, s, ("+" + getFightPower(vo).ToString()).ToString());
    }

    //显示战斗力数值
    //private int getFightPower(MedalVo vo)
    //{
    //    int power = 0;
    //    for (int i = 0; i < vo.Attributes.size; i++)
    //    {
    //        switch (vo.Attributes[i].Type)
    //        {
    //            case eFighintPropertyCate.eFPC_Precise:
    //            case eFighintPropertyCate.eFPC_Dodge:
    //            case eFighintPropertyCate.eFPC_BlastAttackAdd:
    //            case eFighintPropertyCate.eFPC_BlastAttackReduce:

    //            case eFighintPropertyCate.eFPC_IceAttack:
    //            case eFighintPropertyCate.eFPC_AntiIceAttack:
    //            case eFighintPropertyCate.eFPC_IceImmunity:
    //            case eFighintPropertyCate.eFPC_FireAttack:
    //            case eFighintPropertyCate.eFPC_AntiFireAttack:
    //            case eFighintPropertyCate.eFPC_FireImmunity:
    //            case eFighintPropertyCate.eFPC_PoisonAttack:
    //            case eFighintPropertyCate.eFPC_AntiPoisonAttack:
    //            case eFighintPropertyCate.eFPC_PoisonImmunity:
    //            case eFighintPropertyCate.eFPC_ThunderAttack:
    //            case eFighintPropertyCate.eFPC_AntiThunderAttack:
    //            case eFighintPropertyCate.eFPC_ThunderImmunity:
    //                power += vo.Attributes[i].Value * 3;
    //                break;
    //            case eFighintPropertyCate.eFPC_BlastAttack:
    //            case eFighintPropertyCate.eFPC_Tenacity:
    //            case eFighintPropertyCate.eFPC_AntiFightBreak:
    //            case eFighintPropertyCate.eFPC_FightBreak:
    //                power += vo.Attributes[i].Value * 2;
    //                break;
    //            case eFighintPropertyCate.eFPC_Defense:
    //            case eFighintPropertyCate.eFPC_Attack:
    //                power += vo.Attributes[i].Value * 3;
    //                break;
    //            case eFighintPropertyCate.eFPC_MaxHP:
    //                power += vo.Attributes[i].Value / 10;
    //                break;
    //            default:
    //                break;
    //        }
    //    }
    //    return power;
    //}


    //显示荣誉值
    private void changeHonor()
    {
        MedalVo vo = MedalManager.Instance.FindVoByLevel(MedalManager.Instance.CurLevel);
        MedalVo vo1 = MedalManager.Instance.FindVoByLevel(MedalManager.Instance.NextLevel);
        ArenaInfo info = ArenaManager.Instance.ArenaVo.ArenaInfo;
        _leftHonorText.text = info.currentHonor.ToString();
    }
}
