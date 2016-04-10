using UnityEngine;
using System.Collections;
using model;
using manager;
using MVC.entrance.gate;
using mediator;
using helper;

public class MedalView : MonoBehaviour {
	private GameObject medalPrefab;
    private UIGrid medalGridLeft, medalGridRight;
    private GameObject medalEffect;

    #region LEFT
    private UILabel _leftName;      // 左边勋章名称
    private UITexture _leftIcon;    //左边得勋章图片
    private UILabel _rightName;      // 右边勋章名称
    private UITexture _rightIcon;    //右边得勋章图片
    private UILabel _leftFightPower;   //左边攻击力
    private UILabel _rightFightPower;  //右边攻击力
    private GameObject currentIconName;      //图标名称
    private GameObject nextIconName;      //下一级图标名称
    private UILabel _leftHonorText;       //左边荣誉数值
    private UILabel _rightHonorText;      //右边荣誉数值


    #endregion


    void Awake(){
        medalPrefab = transform.FindChild("Attributes").gameObject;
        medalGridLeft = transform.FindChild("Panel/GridLeft").GetComponent<UIGrid>();
        medalGridRight = transform.FindChild("Panel/GridRight").GetComponent<UIGrid>();

        //左边
        _leftName = transform.FindChild("CurrentMedal/MedalName").GetComponent<UILabel>();
        _leftIcon = transform.FindChild("CurrentMedal/MedalSprite").GetComponent<UITexture>();
        
        //右边
        //if (transform.FindChild("NextMedal"))
        //{
            _rightName = transform.FindChild("NextMedal/MedalName").GetComponent<UILabel>();
            _rightIcon = transform.FindChild("NextMedal/MedalSprite").GetComponent<UITexture>();

            _leftFightPower = transform.FindChild("CurrentMedal/MedalPlusScore").GetComponent<UILabel>();
            _rightFightPower = transform.FindChild("NextMedal/MedalPlusScore").GetComponent<UILabel>();
            currentIconName = transform.FindChild("CurrentMedal/MedalGoldIcon").gameObject;
            nextIconName = transform.FindChild("NextMedal/MedalGoldIcon").gameObject;

            _leftHonorText = transform.FindChild("CurrentMedal/CurrentHonor").GetComponent<UILabel>();
            _rightHonorText = transform.FindChild("NextMedal/NextHonor").GetComponent<UILabel>();
        //}

        medalEffect = transform.parent.FindChild("MedalEffect").gameObject;
        DisplayEffect();
	}

    void OnEnable() {
        Gate.instance.registerMediator(new MedalMediator(this));
    }
    void OnDisable() {
        Gate.instance.removeMediator(MediatorName.MEDAL_MEDIATOR);
    }

    public void Display_MedalLvlup()
    {
        //MedalVo vo = MedalManager.Instance.FindVoByLevel(MedalManager.Instance.MedalLevelUp);
    }

    public void Display_Medal()
    {
        MedalVo vo= MedalManager.Instance.FindVoByLevel(MedalManager.Instance.CurLevel);
    }
	
    public void Display_NextMedal(){
        MedalVo nextVo = MedalManager.Instance.FindVoByLevel(MedalManager.Instance.NextLevel);
    }
	
    //public void Display_MedalUI(MedalVo vo,bool isCurrentLevel){
    //    medalPrefab.SetActive(true);
    //    for(int i=0;i<vo.Attributes.size;i++)
    //    {
    //        GameObject obj = Instantiate(medalPrefab) as GameObject;
    //        if(vo.Attributes.size<6)
    //        {
    //            if(isCurrentLevel)
    //                obj.transform.parent = medalGridLeft.transform;
    //            else 
    //                obj.transform.parent = medalGridLeftNext.transform;
    //        }
    //        else
    //        {
    //            if(isCurrentLevel)
    //                obj.transform.parent = medalGridRight.transform;
    //            else
    //                obj.transform.parent = medalGridRightNext.transform;
    //        }
    //        obj.transform.localPosition = new Vector3(0,0,0);
    //        obj.transform.localScale = new Vector3(1,1,1);
    //        string s1 = EquipmentManager.GetEquipAttributeName( (eFighintPropertyCate)(vo.Attributes[i].Type) );
    //        string s2 = vo.Attributes[i].Value.ToString();
    //        obj.GetComponent<UILabel>().text = s1+s2;
    //    }
    //    medalPrefab.SetActive(false);
    //    refreshUI();
    //}
	
    //public void refreshUI(){
    //    medalGridLeft.GetComponent<UIGrid>().repositionNow = true;
    //    medalGridRight.GetComponent<UIGrid>().repositionNow = true;
    //}



    //显示勋章界面
    public void DisplayMedal()
    {
        DisplayLeft();
        DisplayRight();
        modifyFightPower();
        changeIconPic();
        changeHonor();
        disappearLevelUpBtn();
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

    //升级按钮事件
    public void DisplayMedalLevelup() 
    {
        DisplayMedal();
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
        BetterList<AttributeValue> list=MedalManager.Instance.CurVo.Attributes;
        BetterList<DoubleText> _sList = new BetterList<DoubleText>();
        for (int i = 0; i < list.size; i+=2)
        {
            DoubleText text = new DoubleText();
            string str = EquipmentManager.GetEquipAttributeName(list[i].Type);
            string snew = PowerManager.Instance.ChangeInfoData(list[i].Type, list[i].Value);
            text.A1 = string.Format("[{0}]{1}:[-]{2}", ColorConst.Color_HeSe, str, snew); 
            if (i + 1 < list.size)
            {
                str = EquipmentManager.GetEquipAttributeName(list[i + 1].Type);
                string snew0 = PowerManager.Instance.ChangeInfoData(list[i + 1].Type, list[i + 1].Value);
                text.A2 = string.Format("[{0}]{1}:[-]{2}", ColorConst.Color_HeSe, str, snew0);
            }
            else 
            {
                text.A2=null;
            }
            _sList.Add(text);
        }

        ViewHelper.FormatTemplate<BetterList<DoubleText>, DoubleText, MedalViewAttribute>(medalPrefab, medalGridLeft.transform, _sList,
            (DoubleText vo, MedalViewAttribute d) =>
            {
                d.LeftText = vo.A1;
                if (vo.A2!=null)
                {
                    d.RightText = vo.A2;
                }
            });
        medalGridLeft.Reposition();
    }



    private void DisplayRight()     //显示右边
    {
        DisplayRightTitle();
        DisplayRightList();
    }

    private void DisplayRightTitle()
    {
        _rightName.text = ColorConst.Format(ColorConst.Color_DanHuang, MedalManager.Instance.NextVo.Name);
        _rightIcon.mainTexture = SourceManager.Instance.getTextByIconName(MedalManager.Instance.NextVo.Icon, PathConst.MEDAL_ICON_PATH);
    }

    private void DisplayRightList()
    {
        BetterList<AttributeValue> list0 = MedalManager.Instance.CurVo.Attributes;
        BetterList<AttributeValue> list = MedalManager.Instance.NextVo.Attributes;
        BetterList<DoubleText> _sList = new BetterList<DoubleText>();
        string s = "";
        for (int i = 0; i < list.size; i += 2)
        {
            DoubleText texts = new DoubleText();
            string str = EquipmentManager.GetEquipAttributeName(list[i].Type);
            string snew = PowerManager.Instance.ChangeInfoData(list[i].Type, list[i].Value);
            string snewnew = PowerManager.Instance.ChangeInfoData(list[i].Type, list[i].Value - list0[i].Value);
            if ((MedalManager.Instance.CurLevel != MedalManager.Instance.MaxLevel)){
				if(i<list0.size)
                    texts.A1 = string.Format("[{0}]{1}:[-]{2}[{3}]{4}", ColorConst.Color_HeSe, str, snew, ColorConst.Color_Green, "(+" + (snewnew).ToString() + ")");
			}
			else
                texts.A1 = string.Format("[{0}]{1}:[-]{2}", ColorConst.Color_HeSe, str, snew);
			if (i + 1 < list.size)
            {
                if (list.size >= list0.size)
                {
                    str = EquipmentManager.GetEquipAttributeName(list[i + 1].Type);
                    string snew1 = PowerManager.Instance.ChangeInfoData(list[i+1].Type, list[i+1].Value);
                    string snewnew1 = PowerManager.Instance.ChangeInfoData(list[i+1].Type, list[i+1].Value - list0[i+1].Value);

	                if((MedalManager.Instance.CurLevel != MedalManager.Instance.MaxLevel)){
						if(i+1<list0.size)
                            texts.A2 = string.Format("[{0}]{1}:[-]{2}[{3}]{4}", ColorConst.Color_HeSe, str, snew1, ColorConst.Color_Green, "(+" + (snewnew1).ToString() + ")");
						else
                            texts.A2 = string.Format("[{0}]{1}:[-]{2}[{3}]{4}", ColorConst.Color_HeSe, str, snew1, ColorConst.Color_Green, "(+" + (snew1).ToString() + ")");
					}
					else{
                        texts.A2 = string.Format("[{0}]{1}:[-]{2}", ColorConst.Color_HeSe, str, snew1);
					}
				}
                else
                {
                    if (texts.A2 != null)
                    {
                        str = EquipmentManager.GetEquipAttributeName(list[i + 1].Type);
                        string snew1 = PowerManager.Instance.ChangeInfoData(list[i + 1].Type, list[i + 1].Value);
                        texts.A2 = string.Format("[{0}]{1}:[-]{2}", ColorConst.Color_HeSe, str, snew1);
					}
                }
            }
            else
            {
                texts.A2 = null;
            }
            _sList.Add(texts);
        }

        ViewHelper.FormatTemplate<BetterList<DoubleText>, DoubleText, MedalViewAttribute>(medalPrefab, medalGridRight.transform, _sList,
            (DoubleText vo, MedalViewAttribute d) =>
            {
                d.LeftText = vo.A1;
                if (vo.A2 != null)
                {
                    d.RightText = vo.A2;
                }
            });
        medalGridRight.Reposition();
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
        string s = "战斗力";
        _leftFightPower.text = string.Format("[{0}]{1}[-]{2}", ColorConst.Color_HeSe,s, ("（+"+CharacterPlayer.character_property.getFightPower().ToString()+"）").ToString());
        //_leftFightPower.text = getFightPower(vo).ToString();
        _rightFightPower.text = string.Format("[{0}]{1}[-]{2}", ColorConst.Color_HeSe, s, ("（+" + CharacterPlayer.character_property.getFightPower().ToString() + "）").ToString());
        //_rightFightPower.text = getFightPower(vo1).ToString();
        
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

    //显示ICON
    private void changeIconPic() 
    {
        MedalVo vo = MedalManager.Instance.FindVoByLevel(MedalManager.Instance.CurLevel);
        MedalVo vo1 = MedalManager.Instance.FindVoByLevel(MedalManager.Instance.NextLevel);
        currentIconName.GetComponent<UISprite>().spriteName = SourceManager.Instance.getIconByType(vo.Consumes[0].Type);
        nextIconName.GetComponent<UISprite>().spriteName = SourceManager.Instance.getIconByType(vo1.Consumes[0].Type);
    }

    private void disappearLevelUpBtn()
    {
        if (MedalManager.Instance.CurLevel == MedalManager.Instance.MaxLevel)
        {
            transform.FindChild("ShengjiButton").gameObject.SetActive(false);
            transform.FindChild("fullLevelLabel").gameObject.SetActive(true);
            transform.FindChild("NextMedal/NextHonorName").gameObject.SetActive(false);
            transform.FindChild("NextMedal/NextHonor").gameObject.SetActive(false);
            transform.FindChild("NextMedal/MedalGoldIcon").gameObject.SetActive(false);
        }
    }

    //显示荣誉值
    private void changeHonor()
    {
        MedalVo vo = MedalManager.Instance.FindVoByLevel(MedalManager.Instance.CurLevel);
        MedalVo vo1 = MedalManager.Instance.FindVoByLevel(MedalManager.Instance.NextLevel);
        ArenaInfo info = ArenaManager.Instance.ArenaVo.ArenaInfo;
        _leftHonorText.text = info.currentHonor.ToString();
        //_leftHonorText.text = vo.Consumes[0].Value.ToString();
        if (info.currentHonor >= vo.Consumes[0].Value)
            _rightHonorText.text = string.Format("[{0}]{1}[-]", ColorConst.Color_DanHuang, "需求" + (vo.Consumes[0].Value).ToString());
        else
            _rightHonorText.text = string.Format( "[{0}]{1}[-]",ColorConst.Color_Red,"需求"+(vo.Consumes[0].Value).ToString() );
    }

    public void DisplayEffect()
    {
        StartCoroutine(DisplayRotateEffect("xunzhang6_houjue", "xunzhang7_gongjue"));
    }

    IEnumerator DisplayRotateEffect(string currentSprite,string nextSprite)
    {
        medalEffect.GetComponent<UISprite>().spriteName = currentSprite;
        Object obj = medalEffect.GetComponent<TweenRotation>();
        Destroy(obj, 0.5f);
        yield return new WaitForSeconds(0.5f);
        medalEffect.transform.localRotation = Quaternion.identity;
        StartCoroutine(changeSpriteEffect(nextSprite));

    }

    IEnumerator changeSpriteEffect(string nextSprite)
    {
        yield return new WaitForSeconds(0.8f);
        Object obj = medalEffect.GetComponent<TweenScale>();
        Destroy(obj);
        medalEffect.AddComponent<TweenRotation>();
        medalEffect.GetComponent<TweenRotation>().from = new Vector3(0, 0, 0);
        medalEffect.GetComponent<TweenRotation>().to = new Vector3(0, 90, 0);
        medalEffect.GetComponent<TweenRotation>().duration = 0.5f;
        yield return new WaitForSeconds(0.5f);
        medalEffect.transform.localRotation = Quaternion.identity;
        medalEffect.GetComponent<UISprite>().spriteName = nextSprite;

        medalEffect.AddComponent<TweenScale>();
        medalEffect.GetComponent<TweenScale>().from = new Vector3(2, 2, 2);
        medalEffect.GetComponent<TweenScale>().to = new Vector3(1,1,1);
        medalEffect.GetComponent<TweenScale>().duration = 0.6f;

    }
}
