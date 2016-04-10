/**该文件实现的基本功能等
function: 宠物界面的数据
author:zyl
date:2014-06-03
**/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using manager;

namespace model
{
	public class PetVo
	{
		private uint _id;
		private List<int> attrTypes; //增加的属性类型
		private List<int> attrValues; //对应属性类型的具体数值
		private string _petName;					//宠物的名称
		private uint _petParam;	//阶数
		private uint _petLv;	//等级
		private uint _evoCostItem;             //宠物进化消耗的道具ID 
		private uint _evoNum;                 //宠物进化消耗的道具数量
		private uint _costGold;                //消耗货币数量
		private uint _costDiamond;             //消耗钻石数量
		private uint _lowLimit;                  //进化幸运值下限
		private uint _highLimit;                //幸运值上限值
		private uint _sucRateAdd;               //幸运值≥下限后，每次进化增加的成功率（千分比）
		private string _petModle;                //宠物模型
		private string _petEffect;				//播放对应的特效
		private Vector3 _modelPos;				//宠物的模型位置
		private Vector3 _modelScale;				//缩放宠物的模型位置
		private Vector3 _rotateXYZ;				//旋转宠物的模型位置
		private bool _isLimit;				//是否有限制
		
		private int  _petSkill;			//宠物技能
		 
			
		
		
		public PetVo ()
		{
			attrTypes = new List<int> ();
			attrValues = new List<int> ();
		}
	 
		public uint CostDiamond {
			get {
				return this._costDiamond;
			}
			set {
				_costDiamond = value;
			}
		}

		public uint CostGold {
			get {
				return this._costGold;
			}
			set {
				_costGold = value;
			}
		}

		public uint EvoCostItem {
			get {
				return this._evoCostItem;
			}
			set {
				_evoCostItem = value;
			}
		}

		public uint EvoNum {
			get {
				return this._evoNum;
			}
			set {
				_evoNum = value;
			}
		}

		public uint HighLimit {
			get {
				return this._highLimit;
			}
			set {
				_highLimit = value;
			}
		}

		public uint Id {
			get {
				return this._id;
			}
			set {
				_id = value;
			}
		}

		public bool IsLimit {
			get {
				return this._isLimit;
			}
			set {
				_isLimit = value;
			}
		}

		public uint LowLimit {
			get {
				return this._lowLimit;
			}
			set {
				_lowLimit = value;
			}
		}

		public Vector3 ModelPos {
			get {
				return this._modelPos;
			}
			set {
				_modelPos = value;
			}
		}

		public string PetEffect {
			get {
				return this._petEffect;
			}
			set {
				_petEffect = value;
			}
		}

		public uint PetLv {
			get {
				return this._petLv;
			}
			set {
				_petLv = value;
			}
		}

		public string PetModle {
			get {
				return this._petModle;
			}
			set {
				_petModle = value;
			}
		}

		public string PetName {
			get {
				return this._petName;
			}
			set {
				_petName = value;
			}
		}

		public uint PetParam {
			get {
				return this._petParam;
			}
			set {
				_petParam = value;
			}
		}

		public uint SucRateAdd {
			get {
				return this._sucRateAdd;
			}
			set {
				_sucRateAdd = value;
			}
		}

		public List<int> AttrTypes {
			get {
				return this.attrTypes;
			}
			set {
				attrTypes = value;
			}
		}

		public List<int> AttrValues {
			get {
				return this.attrValues;
			}
			set {
				attrValues = value;
			}
		}

		public Vector3 ModelScale {
			get {
				return this._modelScale;
			}
			set {
				_modelScale = value;
			}
		}

		public Vector3 RotateXYZ {
			get {
				return this._rotateXYZ;
			}
			set {
				_rotateXYZ = value;
			}
		}

		public int PetSkill {
			get {
				return _petSkill;
			}
			set {
				_petSkill = value;
			}
		}

		public SkillVo PetSkillVO
		{
			get{
				return SkillTalentManager.Instance.GetSkillVo(this.PetSkill);
			}
		}

		public string EffectDesc {
			set;
			get;
		}

		public string UsedDesc {
			set;
			get;
		}

	}

	


}
