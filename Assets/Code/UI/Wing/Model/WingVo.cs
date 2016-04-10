/**该文件实现的基本功能等
function: 翅膀的XML数据，界面的数据
author:ljx
date:2013-11-09
**/
using manager;
using UnityEngine;

namespace model
{
	public class WingVO
	{
		public uint id;
		public BetterList<int> attrTypes; //增加的属性类型
		public BetterList<int> attrValues; //对应属性类型的具体数值
		public uint cultureExp;  //每次培养获得的经验
		public uint wingLvUpExp; //羽翼升级所需经验
		public uint cultureItemID;            //羽翼升级消耗的道具ID
		public uint cultureItemNum;           //羽翼升级消耗的道具数量
		public uint evoCostItem;             //羽翼进化消耗的道具ID
		public uint evoNum;                 //羽翼进化消耗的道具数量
		public uint costGold;                //消耗货币数量
		public uint costDiamond;             //消耗钻石数量
		public uint lowLimit;                  //进化幸运值下限
		public uint highLimit;                //幸运值上限值
		public uint sucRateAdd;               //幸运值≥下限后，每次进化增加的成功率（千分比）
		public string wingModle;                //翅膀模型
		public string playEffect;				//播放对应的特效
		public string wingName;					//羽翼的名称
        public Vector3 modelPos;				//羽翼的模型位置
        public Vector3 modelScale;				//羽翼的模型位置
		public bool isLimit;				//是否有限制
		
		
		public WingVO ()
		{
			attrTypes = new BetterList<int> ();
			attrValues = new BetterList<int> ();
		}
	}
	
	//翅膀配置文件解析
	public class DataReadWing : DataReadBase
	{		
		public override string getRootNodeName ()
		{
			return "RECORDS";
		}
	
		public override void appendAttribute (int key, string name, string value)
		{	
			WingVO wing;
			uint hashKey = (uint)key;
			if (WingManager.Instance.WingHash.Contains (hashKey))
				wing = WingManager.Instance.WingHash [hashKey] as WingVO;
			else {
				wing = new WingVO ();
				WingManager.Instance.WingHash.Add (hashKey, wing);
			}
			string[] splits = null;
			char[] charSeparators = new char[] {','};
			switch (name) {
			case "ID":
				wing.id = uint.Parse (value);
				break;
			case "stateType":
				splits = value.Split (charSeparators);
				foreach (string every in splits) {
					int addType = int.Parse (every);
					wing.attrTypes.Add (addType);
				}
				break;
			case "stateValue":
				splits = value.Split (charSeparators);
				foreach (string every in splits) {
					int addValue = int.Parse (every);
					wing.attrValues.Add (addValue);
				}
				break;
			case "fosterEXP":
				wing.cultureExp = uint.Parse (value);
				break;
			case "wingName":
				wing.wingName = value;
				break;
			case "wingLvUpEXP":
				wing.wingLvUpExp = uint.Parse (value);
				break;
			case "fosterCostItem":
				splits = value.Split (charSeparators);
				wing.cultureItemID = uint.Parse (splits [0]);
				wing.cultureItemNum = uint.Parse (splits [1]);
				break;
			case "evoCostItem":
				splits = value.Split (charSeparators);
				wing.evoCostItem = uint.Parse (splits [0]);
				wing.evoNum = uint.Parse (splits [1]);
				break;
			case "evoCost":
				splits = value.Split (charSeparators);
				int type = int.Parse (splits [0]);
				if (type == 1)
					wing.costGold = uint.Parse (splits [1]);
				else
					wing.costDiamond = uint.Parse (splits [1]);
				break;
			case "lowLimit":
				wing.lowLimit = uint.Parse (value);
				break;
			case "highLimit":
				wing.highLimit = uint.Parse (value);
				break;
			case "sucRateAdd":
				wing.sucRateAdd = uint.Parse (value);
				break;
			case "wingModle":
				wing.wingModle = value;
				break;
			case "playEffect":
				wing.playEffect = value;
				break;
			case "modelXYZ":
				splits = value.Split (charSeparators);
				wing.modelPos = new Vector3 (float.Parse (splits [0]), float.Parse (splits [1]), float.Parse (splits [2]));
				break;
            case "modelScale":
                splits = value.Split(charSeparators);
                wing.modelScale = new Vector3(float.Parse(splits[0]), float.Parse(splits[1]), float.Parse(splits[2]));
                break;
			case "qingling":
				if (value  == "0") {
					wing.isLimit = false;
				}else{
					wing.isLimit = true;
				}
				
				break;
			default:
				break;
			}
		}
	}
}
