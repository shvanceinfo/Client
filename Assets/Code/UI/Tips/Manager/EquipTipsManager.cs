/**该文件实现的基本功能等
function: 实现装备tips的管理
author:zyl
date:2014-04-17
**/
using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using System.Collections.Generic;
using NetGame;

namespace manager
{
	public class EquipTipsManager
	{
		private static EquipTipsManager _instance;

		private EquipTipsManager ()
		{
			
		}
		
		public static EquipTipsManager Instance {
			get {
				if (_instance == null) {
					_instance = new EquipTipsManager ();
				}
				return _instance;
			}
		}	 
		
	}
}