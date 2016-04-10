using UnityEngine;
using System.Collections;
using manager;
using helper;

public class ModelView : MonoBehaviour {
	public GameObject model;	//游戏主角的模型对象

	/// <summary>
	/// 显示模型对象
	/// </summary>
	/// <param name='modelName'>
	/// 模型名称
	/// </param>
	/// <param name='modelPos'>
	/// 模型位置
	/// </param>
	public void ShowCareerModel (string modelName, ModelPos modelPos)
	{
		if (this.model == null) {  //如果已经存在模型则激活
			this.ShowModel (modelName, modelPos, transform.FindChild ("bg/fazhen").position);
		}
//		else {
//			this.model.SetActive (true);
//		}
	}
	
	/// <summary>
	/// Hides the career model.
	/// </summary>
	public void HideCareerModel ()
	{
		this.model.SetActive (false);
	}


	/// <summary>
	/// 显示模型
	/// </summary>
	/// <param name='modelName'>
	/// 模型名称
	/// </param>
	/// <param name='modelPos'>
	/// 模型位置信息
	/// </param>
	public	 void ShowModel (string modelName, ModelPos modelPos, Vector3 rootPos)
	{
		if (BundleMemManager.Instance.isTypeInCache(EBundleType.eBundleWeaponEffect))
		{
			BundleMemManager.Instance.loadPrefabViaWWW<GameObject>(EBundleType.eBundleWeaponEffect, modelName,
			                                                       (asset) =>
			                                                       {
				setModel((GameObject)asset, modelPos, rootPos);
			}, CharacterPlayer.character_property.career);
		}
		else
		{
			GameObject asset = BundleMemManager.Instance.getPrefabByName(modelName, EBundleType.eBundleWeaponEffect);
			setModel(asset, modelPos, rootPos);
		}				
	}
	
	private  void setModel(GameObject asset, ModelPos modelPos, Vector3 rootPos)
	{
		GameObject model = BundleMemManager.Instance.instantiateObj(asset, Vector3.zero, Quaternion.identity); //复制模型对象
		model.AddComponent<rolRotation>();
		model.AddComponent("CharacterPlayerUI");
		CharacterPlayerUI playerOther = model.GetComponent<CharacterPlayerUI>(); //加载模型装备信息
		if (CharacterPlayer.character_property.weapon > 0)
			playerOther.equipItem(CharacterPlayer.character_property.weapon,true);
		if (CharacterPlayer.character_property.armor > 0)
			playerOther.equipItem(CharacterPlayer.character_property.armor,true);
		if (WingManager.Instance.CurrentWing != null && WingManager.Instance.CurrentWing.id > 0)
			playerOther.character_avatar.installWing(WingManager.Instance.CurrentWing.id, true);
		playerOther.enabled = false; //取消控件才能旋转
		
		NPCManager.Instance.createCamera(false);
		model.transform.parent = NPCManager.Instance.ModelCamera.transform;  //设置父级对象
		NPCManager.Instance.ModelCamera.orthographic = false;
		NPCManager.Instance.ModelCamera.fieldOfView = modelPos.cameraView;
		model.transform.localScale = modelPos.modelScale;
		model.transform.localEulerAngles = modelPos.modelRolate;
		model.transform.position = ViewHelper.UIPositionToCameraPosition(UIManager.Instance.getRootTrans().FindChild("Camera").GetComponent<Camera>(),
		                                                                 NPCManager.Instance.ModelCamera, rootPos);
		model.name = "ui_player";
		ToolFunc.SetLayerRecursively(model, LayerMask.NameToLayer("TopUI")); //设置位置，大小，旋转角度，摄像机和层
		this.model = model;
	}
}
