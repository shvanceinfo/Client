/**该文件实现的基本功能等
function: 实现角色的数据管理
author:zyl
date:2014-4-12
**/
using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using System.Collections.Generic;
using NetGame;

namespace  manager
{
	public class RoleManager
	{
 		private static RoleManager _instance;
		 
		
		private RoleManager(){
			
		}
		
		#region 属性信息
		public string ModelName {
			get {
				return BagManager.Instance.ModelName;
			}
		 
		}

		public ModelPos ModelPos {
			get {
				return BagManager.Instance.ModelPos;
			}
			 
		}

		public Dictionary<eEquipPart, EquipmentStruct> EquipData {
			get {
				return BagManager.Instance.EquipData;
			}
		}
		
		#endregion
		
		
		/// <summary>
		/// 打开角色界面
		/// </summary>
		public void OpenRole ()
		{
			UIManager.Instance.openWindow (UiNameConst.ui_role);
			#region 左边数据
			BagManager.Instance.InitCareerModel ();  //初始化模型位置信息
			//BagManager.Instance.InitEquipData ();    //初始化装备信息
			this.ShowCareerModel();
			this.ShowEquipData();
			#endregion
			
			#region 右边数据
			this.ShowPeopleInfo();
			#endregion
		}
		
		/// <summary>
		/// Shows the career model.
		/// </summary>
		public void ShowCareerModel(){
			Gate.instance.sendNotification (MsgConstant.MSG_ROLE_SHOWCAREERMODEL);	//显示模型
		}
		/// <summary>
		/// Shows the equip data.
		/// </summary>
		public void ShowEquipData(){
			Gate.instance.sendNotification (MsgConstant.MSG_ROLE_SHOWEQUIPDATA);	//显示装备数据
		}
		/// <summary>
		/// Hides the career model.
		/// </summary>
		public void HideCareerModel(){
			Gate.instance.sendNotification (MsgConstant.MSG_ROLE_HIDECAREERMODEL);	//隐藏模型
		}
		/// <summary>
		/// Shows the people info.
		/// </summary>
		public void ShowPeopleInfo(){
			Gate.instance.sendNotification (MsgConstant.MSG_ROLE_SHOWPEOPLEINFO);	//显示用户信息
		}
		
		
		public static RoleManager Instance{
			get	{
				if (_instance == null) {
					_instance = new RoleManager();
				}
				return _instance;
			}
		}
		
	}

}
