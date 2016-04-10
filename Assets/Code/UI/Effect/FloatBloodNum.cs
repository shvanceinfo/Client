using UnityEngine;
using System.Collections;
using MVC.entrance.gate;

public enum eHurtType
{
	normalHurt = 0,	//正常攻击
	doubleHurt,		//暴击
	escapeHurt,		//闪避
	withstandHurt,		//招架	
	goblinMoney, //哥布林金钱
	golbinPoison  //哥布林中毒
}

public class FloatBloodNum
{
	const int PLUS_NUM = 10; //加号
	const int SUBSTRACT_NUM = 11; //减号
	const int DOUBLE_HIT_NUM = 12; //暴击
	const int ESCAPE_HIT_NUM = 13; //闪避
	const int WITHSTAND_HIT_NUM = 14; //招架	
	const int NUM_SP = 9;
	
	private UISprite[] _numSps;
	private GameObject _escapeSp; //闪避的Sprite
	private GameObject _doubleSp;
	private GameObject _withstandSp;
	private GameObject _prefab;
	
	private static FloatBloodNum _instance;
	
	private FloatBloodNum()
	{
		_numSps = new UISprite[NUM_SP];
        _prefab = BundleMemManager.Instance.getPrefabByName(PathConst.FLOAT_BLOOD_NUM, EBundleType.eBundleCommon);        	  
	}
	
	//美术字体飘血
    public void PlayFloatBood(bool isPlayer, int hurtNum, Transform parent, eHurtType type=eHurtType.normalHurt)
    {
		if (isPlayer&&(type==eHurtType.doubleHurt||type==eHurtType.normalHurt))
        {
            Gate.instance.sendNotification(MsgConstant.MSG_FIGHT_COMBO);
        }

    	GameObject parentObj = new GameObject();
        parentObj.transform.position = parent.position;
        parentObj.AddComponent("BillBoard");

        GameObject obj = createBloodNum(isPlayer, hurtNum, type);
        obj.transform.parent = parentObj.transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        GameObject.Destroy(parentObj, 1.1f);

    }
    
    //战斗力计算
    public void setPowerSp(int fightPower, UISprite[] sps)
    {
    	int len = sps.Length;
    	int count = getNumCount(fightPower);
    	for(int i=count; i<len; i++)
    		sps[i].gameObject.SetActive(false);
    	for(int i=0; i<count; i++)
    	{
    		sps[i].gameObject.SetActive(true);
    		sps[i].spriteName = getDigitSpName(false, fightPower, i, count, eHurtType.goblinMoney);
			sps[i].MakePixelPerfect();
    	}
    }
	
	//生成相应飘血对象
	private GameObject createBloodNum(bool isPlayer, int hurtNum,  eHurtType type)
	{
        GameObject obj = BundleMemManager.Instance.instantiateObj(_prefab);
		_escapeSp = obj.transform.Find("escape").gameObject;
		_doubleSp = obj.transform.Find("double").gameObject;
		_withstandSp = obj.transform.Find("withstand").gameObject;
		setTweenParam(obj, type);
		switch (type) 
		{
			case eHurtType.normalHurt:
			case eHurtType.doubleHurt:
			case eHurtType.goblinMoney:
			case eHurtType.golbinPoison:
			{
				_escapeSp.SetActive(false);
				_withstandSp.SetActive(false);
                if (type == eHurtType.doubleHurt)
                {
                    _doubleSp.SetActive(true);
                }
                else
                    _doubleSp.SetActive(false);
				for(int i=1; i<=NUM_SP; i++)
				{
					UISprite sp = obj.transform.Find("sp"+i).GetComponent<UISprite>();
					_numSps[i-1] = sp;
				}
				int numCount = getNumCount(hurtNum);
				int firstNum = (NUM_SP - numCount - 1)/2; //第一个显示的符号位置，扣除一个符号位
				for(int i=0; i<firstNum;  i++)
					_numSps[i].active = false;
				for(int i=firstNum+numCount+1; i<NUM_SP; i++)
					_numSps[i].active = false;
				int index= -1;
//				if(type == eHurtType.goblinMoney || type == eHurtType.golbinPoison) //中毒跟前没有符号位
//					index = 0;
				for(int i=firstNum; i<=firstNum+numCount; i++)
				{
					_numSps[i].active = true;
					_numSps[i].spriteName = getDigitSpName(isPlayer, hurtNum, index, numCount, type);
                    
					if(index == -1)
						_numSps[i].transform.localScale = new Vector3(0.05f, 0.022f, 0.25f);
					else
						_numSps[i].transform.localScale = new Vector3(0.05f, 0.08f, 0.25f);
                    index++; 
                    if (_numSps[i].spriteName.Equals("number_red_-"))
                    {
                        _doubleSp.transform.localPosition = _numSps[i].transform.localPosition + new Vector3(-3.8f,0,0);
                    }
				}
				break;
			}
			case eHurtType.withstandHurt:
			case eHurtType.escapeHurt:
			{
				for(int i=1; i<=NUM_SP; i++)
				{
					GameObject spObj = obj.transform.Find("sp"+i).gameObject;
					spObj.SetActive(false);
				}
				_doubleSp.SetActive(false);
				if(type == eHurtType.escapeHurt)
				{
					_escapeSp.SetActive(true);
					_withstandSp.SetActive(false);
				}
				else
				{
					_escapeSp.SetActive(false);
					_withstandSp.SetActive(true);
				}
				break;
			}
			default:
				break;
		}
		return obj;
	}	
	
	//获取伤害位数
	private int getNumCount(int num)
	{
		int i = 0;
		while(num > 0)
		{
			num /= 10;
			i++;
		}
		return i;
	}
	
	//获取各位数上的伤害数字, isPlayer代表在哪个身上飘雪
	private string getDigitSpName(bool isPlayer, int hurtNum, int digit, int numCount, eHurtType type=eHurtType.normalHurt)
	{
		string prefix = "number_"; //飘雪的前缀
		string color = "yellow_"; 
		if(isPlayer)
			color = "blue_";
		else
			color = "yellow_";
		if(type == eHurtType.doubleHurt) //暴击字体
			color = "red_";
		else if(type == eHurtType.golbinPoison)  //中毒都是绿色
			color = "red_";
		else if(type == eHurtType.goblinMoney)  //哥布林掉钱,战斗力显示
			color = "gold_";
		prefix = prefix + color;
		if(digit == -1) //符号位
		{
            if (hurtNum > 0)
            {
                return prefix + "-";
            }
            else
            {
                return prefix + "+";
            }
		}
		else
		{
			while (numCount - digit > 1) 
			{
				hurtNum /=10;
				numCount--;
			}
			return prefix + (hurtNum%10).ToString();
		}
	}
	
	//设置动画参数
	private void setTweenParam(GameObject obj, eHurtType type)
	{
		TweenScale scale = obj.GetComponent<TweenScale>();
		TweenPosition position = obj.GetComponent<TweenPosition>();
		switch (type) 
		{
			case eHurtType.normalHurt:
			case eHurtType.goblinMoney:
			{
				scale.delay = 0f;
				scale.duration = 1f;
				scale.from = new Vector3(0.06f, 0.06f,0.06f);
				scale.to = new Vector3(0.12f, 0.12f, 0.12f);
				position.delay = 0.3f;
				position.duration = 0.7f;
				position.from = new Vector3(0f, 0f, 0f);
				position.to = new Vector3(0f, 0.6f, 0f);
				break;
			}
			case eHurtType.doubleHurt:
			case eHurtType.golbinPoison:
			{
				scale.delay = 0f;
				scale.duration = 0.2f;
				scale.from = new Vector3(0.07f, 0.07f,0.07f);
				scale.to = new Vector3(0.15f, 0.15f, 0.15f);
				TweenScale newScale = obj.AddComponent<TweenScale>();
				newScale.delay = 0.2f;
				newScale.duration = 0.4f;
				newScale.from = new Vector3(0.15f, 0.15f,0.15f);
				newScale.to = new Vector3(0.11f, 0.11f, 0.11f);
				position.delay = 0f;
				position.duration = 1f;
				position.from = new Vector3(0f, 0.3f, 0f);
				position.to = new Vector3(0f, 0.8f, 0f);
				break;
			}
			case eHurtType.escapeHurt:
			{
				scale.delay = 0f;
				scale.duration = 0.3f;
				scale.from = new Vector3(0.07f, 0.07f,0.07f);
				scale.to = new Vector3(0.13f, 0.13f, 0.13f);
				TweenScale newScale = obj.AddComponent<TweenScale>();
				newScale.delay = 0.3f;
				newScale.duration = 0.2f;
				newScale.from = new Vector3(0.13f, 0.13f,0.13f);
				newScale.to = new Vector3(0.08f, 0.08f, 0.08f);
				position.delay = 0f;
				position.duration = 0.7f;
				position.from = new Vector3(0f, 0.2f, 0f);
				position.to = new Vector3(0f, 0.6f, 0f);
				break;
			}
			case eHurtType.withstandHurt:
			{
				scale.delay = 0f;
				scale.duration = 0.3f;
				scale.from = new Vector3(0.07f, 0.07f,0.07f);
				scale.to = new Vector3(0.13f, 0.13f, 0.13f);
				TweenScale newScale = obj.AddComponent<TweenScale>();
				newScale.delay = 0.3f;
				newScale.duration = 0.2f;
				newScale.from = new Vector3(0.13f, 0.13f,0.13f);
				newScale.to = new Vector3(0.08f, 0.08f, 0.08f);
				position.delay = 0f;
				position.duration = 0.7f;
				position.from = new Vector3(0f, 0.2f, 0f);
				position.to = new Vector3(0f, 0.6f, 0f);
				break;
			}
			default:
				break;
		}
	}
	
	//getter and setter
	public static FloatBloodNum Instance
	{
		get 
		{ 
			if(_instance == null)
				_instance = new FloatBloodNum();
			return _instance; 
		}
	}	
}
