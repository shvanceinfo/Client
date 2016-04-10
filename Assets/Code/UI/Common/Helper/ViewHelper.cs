///懒人代码 v:0.0.1
///没有最懒，只有更懒...
using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using System;
using System.Collections.Generic;
using UnityEngineInternal;
using manager;

namespace helper
{
	/// <summary>
	/// 扩展方法
	/// </summary>
	public static class Extensions
	{
		public static int toInt32 (this String value)
		{
			return int.Parse (value);
		}

		public static uint toUInt32 (this String value)
		{
			return uint.Parse (value);
		}

		public static int toInt32 (this System.Object value)
		{
			return value.ToString ().toInt32 ();
		}

		public static T F<T> (this GameObject obj, string path) where T : Component
		{
			return obj.transform.FindChild (path).GetComponent<T> ();
		}

		public static T F<T> (this Transform obj, string path) where T : Component
		{
			return obj.FindChild (path).GetComponent<T> ();
		}

		public static GameObject F (this GameObject obj, string path)
		{
			return obj.transform.FindChild (path).gameObject;
		}

		public static GameObject F (this Transform obj, string path)
		{
			return obj.FindChild (path).gameObject;
		}
	}
 
	public class ViewHelper
	{
		public const string Warrior = "arena_zhanshi";
		public const string Magic = "arena_mofashi";
		public const string Archer = "arena_gongjianshou";

		public static void AddItemTemplatePrefab (GameObject prefab, Transform grid, int id)
		{
			prefab.SetActive (true);
			GameObject obj = BundleMemManager.Instance.instantiateObj (prefab);
			obj.transform.parent = grid;
			obj.name = id.ToString ();
			obj.transform.localPosition = new Vector3 (0, 0, 0);
			obj.transform.localScale = new Vector3 (1, 1, 1);
			prefab.SetActive (false);
		}

		public static void DeleteItemTemplate (Transform grid, int id)
		{
			Transform t = grid.FindChild (id.ToString ());
			if (t == null) {
				Debug.LogError ("Index error");
			} else {
				UnityEngine.Object.Destroy (t.gameObject);
			}
		}


		/// <summary>
		/// 将UI坐标转成模型坐标
		/// </summary>
		/// <param name="uiCamera">UI摄像机</param>
		/// <param name="tagCamera">模型摄像机</param>
		/// <param name="pos">UI坐标</param>
		/// <returns></returns>
		public static Vector3 UIPositionToCameraPosition (Camera uiCamera, Camera tagCamera, Vector3 pos)
		{
			pos = uiCamera.WorldToScreenPoint (pos);
			pos.z = Mathf.Abs (tagCamera.transform.position.z);
			return tagCamera.ScreenToWorldPoint (pos);
		}

		/// <summary>
		/// 刷新列表，自定义
		/// </summary>
		/// <typeparam name="T">数据源集合类型</typeparam>
		/// <typeparam name="Vo">数据类型</typeparam>
		/// <param name="prefab">复制体</param>
		/// <param name="grid">列表</param>
		/// <param name="data">数据</param>
		/// <param name="func">(Vo,int=复制体下标)</param>
		public static void FormatTemplate<T, Vo> (GameObject prefab, Transform grid, T data, Action<Vo,int> func)
		{
			int tChild = grid.childCount;
			int size = 0;
			if (data is BetterList<Vo>) {
				size = (data as BetterList<Vo>).size;
			} else if (data is Hashtable) {
				size = (data as Hashtable).Count;
			} else if (data is IList<Vo>) {
				size = (data as IList<Vo>).Count;
			}
			if (tChild < size) {
				for (int i = tChild; i < size; i++) {
					AddItemTemplatePrefab (prefab, grid, i);
				}
			}
			if (tChild > size) {
				for (int i = size; i < tChild; i++) {
					DeleteItemTemplate (grid, i);
				}
			}

			if (data is BetterList<Vo>) {
				BetterList<Vo> ds = data as BetterList<Vo>;
				for (int i = 0; i < ds.size; i++) {
					func (ds [i], i);
				}
			} else if (data is Hashtable) {
				Hashtable ds = data as Hashtable;
				for (int i = 0; i < ds.Count; i++) {
					func (((Vo)ds [i]), i);
				}
			} else if (data is IList<Vo>) {
				IList<Vo> ds = data as IList<Vo>;
				for (int i = 0; i < ds.Count; i++) {
					func (ds [i], i);
				}
			}
		}

		/// <summary>
		/// 刷新列表，自定义
		/// </summary>
		/// <typeparam name="T">数据源集合类型</typeparam>
		/// <typeparam name="Vo">数据类型</typeparam>
		/// <param name="prefab">复制体</param>
		/// <param name="grid">列表</param>
		/// <param name="data">数据</param>
		/// <param name="func">(Vo,int=复制体下标)</param>
		public static void FormatTemplate<T, Vo> (GameObject prefab, Transform grid, T data, Action<Vo, Transform> func)
		{
			int tChild = grid.childCount;
			int size = 0;
			if (data is BetterList<Vo>) {
				size = (data as BetterList<Vo>).size;
			} else if (data is Hashtable) {
				size = (data as Hashtable).Count;
			} else if (data is IList<Vo>) {
				size = (data as IList<Vo>).Count;
			}
			if (tChild < size) {
				for (int i = tChild; i < size; i++) {
					AddItemTemplatePrefab (prefab, grid, i);
				}
			}
			if (tChild > size) {
				for (int i = size; i < tChild; i++) {
					DeleteItemTemplate (grid, i);
				}
			}

			if (data is BetterList<Vo>) {
				BetterList<Vo> ds = data as BetterList<Vo>;
				for (int i = 0; i < ds.size; i++) {
					func (ds [i], grid.FindChild (i.ToString ()));
				}
			} else if (data is Hashtable) {
				Hashtable ds = data as Hashtable;
				for (int i = 0; i < ds.Count; i++) {
					func (((Vo)ds [i]), grid.FindChild (i.ToString ()));
				}
			} else if (data is IList<Vo>) {
				IList<Vo> ds = data as IList<Vo>;
				for (int i = 0; i < ds.Count; i++) {
					func (ds [i], grid.FindChild (i.ToString ()));
				}
			}
		}


		/// <summary>
		/// 刷新列表，双值,每次返回一对值
		/// </summary>
		/// <typeparam name="T">数据源集合类型</typeparam>
		/// <typeparam name="Vo">数据类型</typeparam>
		/// <param name="prefab">复制体</param>
		/// <param name="grid">列表</param>
		/// <param name="data">数据</param>
		/// <param name="func">(Vo,int=复制体下标)</param>
		public static void FormatTemplate<T, Vo> (GameObject prefab, Transform grid, T data, Action<Vo,Vo,Transform> func)
		{
			int tChild = grid.childCount * 2;
			int size = 0;
			if (data is BetterList<Vo>) {
				size = (data as BetterList<Vo>).size;
			} else if (data is Hashtable) {
				size = (data as Hashtable).Count;
			} else if (data is IList<Vo>) {
				size = (data as IList<Vo>).Count;
			}

			size = size % 2 == 0 ? size : size + 1;

			if (tChild < size) {
				for (int i = tChild; i < size; i+=2) {
					AddItemTemplatePrefab (prefab, grid, i);
				}
			}
			if (tChild > size) {
				for (int i = size; i < tChild; i+=2) {
					DeleteItemTemplate (grid, i);
				}
			}

			if (data is BetterList<Vo>) {
				BetterList<Vo> ds = data as BetterList<Vo>;
				for (int i = 0; i < ds.size; i+=2) {
					if (i + 1 >= ds.size) {
						func (ds [i], default(Vo), grid.FindChild (i.ToString ()));
					} else {
						func (ds [i], ds [i + 1], grid.FindChild (i.ToString ()));
					}
                    
				}
			} else if (data is Hashtable) {
				Hashtable ds = data as Hashtable;
				for (int i = 0; i < ds.Count; i += 2) {
					if (i + 1 >= ds.Count) {
						func ((Vo)ds [i], default(Vo), grid.FindChild (i.ToString ()));
					} else {
						func ((Vo)ds [i], (Vo)ds [i + 1], grid.FindChild (i.ToString ()));
					}

				}
			} else if (data is IList<Vo>) {
				IList<Vo> ds = data as IList<Vo>;
				for (int i = 0; i < ds.Count; i+=2) {
					if (i + 1 >= ds.Count) {
						func (ds [i], default(Vo), grid.FindChild (i.ToString ()));
					} else {
						func (ds [i], ds [i + 1], grid.FindChild (i.ToString ()));
					}
				}
			}
		}

		/// <summary>
		/// 刷新list强化版
		/// </summary>
		/// <typeparam name="T">数据源集合类型</typeparam>
		/// <typeparam name="Vo">数据源类型</typeparam>
		/// <typeparam name="D">刷新显示的组件类型</typeparam>
		/// <param name="prefab">复制的object</param>
		/// <param name="grid">放在Grid下面</param>
		/// <param name="data">数据源</param>
		/// <param name="dp">刷新的类型</param>
		/// <param name="func">调用赋值逻辑 Vo(数据类型),D(刷新组件)</param>
		public static void FormatTemplate<T, Vo,D> (GameObject prefab, Transform grid, T data, Action<Vo, D> func) where D : Component
		{
			int tChild = grid.childCount;
			int size = 0;
			if (data is BetterList<Vo>) {
				size = (data as BetterList<Vo>).size;
			} else if (data is Hashtable) {
				size = (data as Hashtable).Count;
			} else if (data is IList<Vo>) {
				size = (data as IList<Vo>).Count;
			}
			if (tChild < size) {
				for (int i = tChild; i < size; i++) {
					AddItemTemplatePrefab (prefab, grid, i);
				}
			}
			if (tChild > size) {
				for (int i = size; i < tChild; i++) {
					DeleteItemTemplate (grid, i);
				}
			}

			if (data is BetterList<Vo>) {
				BetterList<Vo> ds = data as BetterList<Vo>;
				for (int i = 0; i < ds.size; i++) {

					func (ds [i], grid.FindChild (i.ToString ()).GetComponent<D> ());
				}
			} else if (data is Hashtable) {
				Hashtable ds = data as Hashtable;
				for (int i = 0; i < ds.Count; i++) {
					func (((Vo)ds [i]), grid.FindChild (i.ToString ()).GetComponent<D> ());
				}
			} else if (data is IList<Vo>) {
				IList<Vo> ds = data as IList<Vo>;
				foreach (Vo v in ds) {

				}
				for (int i = 0; i < ds.Count; i++) {
					func (ds [i], grid.FindChild (i.ToString ()).GetComponent<D> ());
				}
			}
		}

		/// <summary>
		/// 根据职业获取，Comment图集里的图片名称
		/// </summary>
		/// <param name="career"></param>
		/// <returns></returns>
		public static string GetHandIcon (CHARACTER_CAREER career)
		{
			switch (career) {
			case CHARACTER_CAREER.CC_BEGIN:
				break;
			case CHARACTER_CAREER.CC_SWORD:
				return Constant.Fight_WarriorHandIcon;
			case CHARACTER_CAREER.CC_ARCHER:
				return Constant.Fight_ArcherHandIcon;
			case CHARACTER_CAREER.CC_MAGICIAN:
				return Constant.Fight_MagicHandIcon;

			case CHARACTER_CAREER.CC_END:
				break;
			default:
				break;
			}
			return null;
		}

		public static void DisplayMessage (string message)
		{
			FloatMessage.GetInstance ().PlayFloatMessage (message, UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);	
		}

		public static void DisplayMessageLanguage (string message)
		{
			FloatMessage.GetInstance ().PlayFloatMessage (LanguageManager.GetText (message), UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);	
		}

		public static void DisplayMessageLanguage (string message, params object[] args)
		{
			FloatMessage.GetInstance ().PlayFloatMessage (string.Format (LanguageManager.GetText (message), args), UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);
		}

		public static string FormatLanguage (string message, params object[] args)
		{
			return string.Format (LanguageManager.GetText (message), args);
		}

		public static bool CheckIsHava (eGoldType t, int gold, bool showMsg=true)
		{
			switch (t) {
			case eGoldType.none:
                    //if (showMsg)
                    //DisplayMessage( LanguageManager.GetText("暂未开放"));
				return false;
				break;
			case eGoldType.gold:
				if (CharacterPlayer.character_asset.gold < gold) {
					if (showMsg)
						DisplayMessage (LanguageManager.GetText ("msg_money_not_enough"));
					return false;
				}
				break;
			case eGoldType.zuanshi:
				if (CharacterPlayer.character_asset.diamond < gold) {
					if (showMsg)
						DisplayMessage (LanguageManager.GetText ("-99999945"));
					return false;
				}
				break;
			case eGoldType.rongyu:
				if (CharacterPlayer.character_asset.Honor < gold) {
					if (showMsg)
						DisplayMessage (LanguageManager.GetText ("medal_nothave"));
					return false;
				}
				break;
			case eGoldType.fushi:
				if (manager.SkillTalentManager.Instance.SkillPoint < gold) {
					if (showMsg)
						DisplayMessage (LanguageManager.GetText ("-99999956"));
					return false;
				}
				break;
			case eGoldType.shuijing:
				if (CharacterPlayer.character_asset.Crystal < gold) {
					if (showMsg)
						DisplayMessage (LanguageManager.GetText ("水晶不足"));
					return false;
				}
				break;
			default:
				break;
			}
			return true;
		}

		public static bool CheckIsHave (int id, int value, bool isshowdebug=true)
		{
			int have = (int)ItemManager.GetInstance ().GetItemNumById ((uint)id);
			int need = value;
			if (have >= value) {
				return true;
			}
			if (isshowdebug) {
				DisplayMessageLanguage ("strengthen_notength");  //提示材料不足
			}
			return false;
		}
		/// <summary>
		/// 根据职业获取图片
		/// </summary>
		/// <param name="car"></param>
		/// <returns></returns>
		public static string GetCarrerSpriteByEnum (CHARACTER_CAREER car)
		{
			switch (car) {
			case CHARACTER_CAREER.CC_BEGIN:
				break;
			case CHARACTER_CAREER.CC_SWORD:
				return Warrior;
			case CHARACTER_CAREER.CC_ARCHER:
				return Archer;
			case CHARACTER_CAREER.CC_MAGICIAN:
				return Magic;
			case CHARACTER_CAREER.CC_END:
				break;
			default:
				break;
			}
			return "null";
		}
		/// <summary>
		/// 根据职业获取，职业名
		/// </summary>
		/// <param name="career"></param>
		/// <returns></returns>
		public static string GetStringByCareer (CHARACTER_CAREER career)
		{
			switch (career) {
			case CHARACTER_CAREER.CC_BEGIN:
				return "无限制";
			case CHARACTER_CAREER.CC_SWORD:
				return "剑士";
			case CHARACTER_CAREER.CC_ARCHER:
				return "弓箭手";
			case CHARACTER_CAREER.CC_MAGICIAN:
				return "魔法师";
			case CHARACTER_CAREER.CC_END:
				break;
			default:
				break;
			}
			return "";
		}

		/// <summary>
		/// 获取tip
		/// </summary>
		/// <returns></returns>
		public static string GetTips ()
		{
			return ConfigDataManager.GetInstance ().getLoadingTipsConfig ().getTipData (UnityEngine.Random.Range (Constant.LOAD_TIP_MIN, Constant.LOAD_TIP_MAX)).tip;
		}

		/// <summary>
		/// 获取物品是否足够
		/// have/value (红/绿)
		/// </summary>
		/// <param name="id"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetItemHave (int id, int value)
		{ 
			int have = (int)ItemManager.GetInstance ().GetItemNumById ((uint)id);
			int need = value;
			if (have >= need) {
				return ColorConst.Format (ColorConst.Color_Green, have, "/", value);
			} else {
				return ColorConst.Format (ColorConst.Color_Red, have, "/", value);
			}
		}

		/// <summary>
		/// 检测是否足够，返回一个带颜色的字符串
		/// </summary>
		/// <param name="have"></param>
		/// <param name="need"></param>
		/// <returns></returns>
		public static string GetPriceColor (int have, int need)
		{ 
			return string.Format ("[{0}]{1}[-]", have >= need ? ColorConst.Color_Green : ColorConst.Color_Red, need);
		}

		public static string GetGoldHave (eGoldType gold, int value)
		{
			switch (gold) {
			case eGoldType.none:
				return "-1";
			case eGoldType.gold:
				if (CharacterPlayer.character_asset.gold < value) {
					return ColorConst.Format (ColorConst.Color_Red, value);
				}
				break;
			case eGoldType.zuanshi:
				if (CharacterPlayer.character_asset.diamond < value) {
					return ColorConst.Format (ColorConst.Color_Red, value);
				}
				break;
			case eGoldType.rongyu:
                    
				break;
			case eGoldType.fushi:
				if (manager.SkillTalentManager.Instance.SkillPoint < value) {
					return ColorConst.Format (ColorConst.Color_Red, value);
				}
				break;
			case eGoldType.shuijing:
				if (CharacterPlayer.character_asset.Crystal < value) {
					return ColorConst.Format (ColorConst.Color_Red, value);
				}
				break;
			default:
				break;
			}
			return ColorConst.Format (ColorConst.Color_Green, value);
			;
		}

		/// <summary>
		/// 根据物品ID获取边框
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static string GetBoderById (int id)
		{
			return BagManager.Instance.getItemBgByType (ItemManager.GetInstance ().GetTemplateByTempId ((uint)id).quality, true);
		}

		/// <summary>
		/// 获取每个资源名称 
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static string GetResoucesString (eGoldType type)
		{
			return FormatLanguage ("resource_" + (int)type);
		}

		/// <summary>
		/// public data
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static PublicDataItem FindPublicById (int id)
		{
			return ConfigDataManager.GetInstance ().GetPublicDataConfig ().getPublicData (id);
		}

		//总页数
		public static int MaxPage (int pageSize, int count)
		{
			int page = count / pageSize; //算出总页数
			if (count % pageSize != 0) {
				page++;
			}
			return page;
		}

		public static void TouchUpAddingData (ref int currentPage, int pageSize, float Height, int allCount, UIPanel itemListPanel, Action<int> deleRowCallBack, Action<int> addRowCallBack)
		{
			if (UICamera.currentTouch == null) {
				return;
			}
			if (UICamera.currentTouch.delta.y > 0) {
				if ((currentPage * pageSize * Height - 4 * Height) > itemListPanel.clipOffset.y) {
					if ((currentPage + 1) > MaxPage (pageSize, allCount)) {
						return;
					}
					 
					#region 删除前一页
					int prevPage = currentPage - 1; //需要删除的页 
					if (prevPage > 0) {
						for (int i = (prevPage -1)* pageSize,max = prevPage*pageSize; i < max; i++) {
							deleRowCallBack (i);
						}
					}
					#endregion
					//			print(currentPage);
					currentPage ++;
					//			print(currentPage);
	
					for (int i = (currentPage -1)* pageSize,max = allCount>(currentPage* pageSize)? (currentPage* pageSize): allCount; i < max; i++) {
						addRowCallBack (i);
					}
				}//加载数据
			}
		}

		public static void TouchDownAddingData (ref int currentPage, int pageSize, float Height, int allCount, UIPanel itemListPanel, Action<int> deleRowCallBack, Action<int> addRowCallBack)
		{
			if (UICamera.currentTouch == null) {
				return;
			}
			if (UICamera.currentTouch.delta.y < 0) { //往上拉的状态
				if (((currentPage - 1) * pageSize * Height - 4 * Height) < itemListPanel.clipOffset.y) {
					if ((currentPage - 1) < 1) {
						return;
					}

					#region 删除当前页
					for (int i = (currentPage -1)* pageSize,max = currentPage*pageSize; i < max; i++) {
						deleRowCallBack (i);
					}
					
					#endregion
					//			print(currentPage);
					currentPage --;
					//			print(currentPage);
					if (currentPage - 2 >= 0) {
						for (int i = (currentPage -2)* pageSize,max = 0>=((currentPage-1)* pageSize)? 0: (currentPage-1)* pageSize; i < max; i++) {
							addRowCallBack (i);
						} 
					}
					
				}//加载数据
			}
		}




	}
	/// <summary>
	/// 用法
	/// XmlHelper.CallTry(()=>{ //TODE Code...});
	/// </summary>
	public class XmlHelper
	{
		/// <summary>
		/// 尝试
		/// </summary>
		/// <param name="fc"></param>
		/// <returns></returns>
		public static int CallTry (Func<int> fc)
		{
			try {
				return fc ();
			} catch (Exception ex) {
				Debug.LogError (ex.ToString ());
				throw;
			}

		}

		public static void CallTry (Func<string> fc)
		{
			try {
				fc ();
			} catch (Exception ex) {
				Debug.LogError (ex.ToString ());
				throw;
			}
		}

		public static void CallTry (Action fc)
		{
			try {
				fc ();
			} catch (Exception ex) {
				Debug.LogError (ex.ToString ());
				throw;
			}
		}
	}
}

