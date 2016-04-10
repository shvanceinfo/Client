/**该文件实现的基本功能等
function:相关的图片处理函数
author:ljx
date:2013-11-28
**/
using UnityEngine;

public class DealTexture 
{
	//镜像源都是左边，左上
	public enum MIRROR_POS
	{
		right=0,  //镜像右边
		topRight, //右上
		bottomLeft,
		bottomRight
	}
	public const float COLOR_NONE = 0;
	public const float COLOR_ORANGE = 42;
	public const float COLOR_BLUE = 193;
	public const float COLOR_PURPLE = 314;
	public const float COLOR_GREEN = 114;
	
	private static DealTexture _instance;
	
	public static DealTexture Instance
	{
		get
		{
			if(null == _instance)
			{
				_instance = new DealTexture();
			}
			return _instance;
		}
	}
	
	//镜像生成新的UISprite，并且生成新的Sprite
	public UISprite mirrorSprite(UISprite origSp, MIRROR_POS pos, bool makePerfect = true, int offset = 0)
	{
		UISprite newSp = NGUITools.AddWidget<UISprite>(origSp.transform.parent.gameObject);
		Component[] objs = origSp.GetComponents<Component>();
		foreach (Component comp in objs) 
		{
			if(!(comp is MirrorSprite || comp is Transform || comp is UISprite))
			{
				if(comp.gameObject.activeSelf) //只有激活状态的才复制
					ToolFunc.CopyComponent(comp, newSp.gameObject);
			}
		}
		float width = 0;
		float height = 0;
		Vector3 vecPos = origSp.transform.localPosition;
		Vector3 vecScale = origSp.transform.localScale;
		UISpriteData innerSp = origSp.GetAtlasSprite();
		newSp.pivot = origSp.pivot;		
		if(makePerfect)
		{
			origSp.MakePixelPerfect(); //先将素材对好
			 
			width = innerSp.width;
			height = innerSp.height;
		}
		else
		{
			width = vecScale.x;
			height = vecScale.y;
		}
		newSp.atlas = origSp.atlas;
		newSp.spriteName = origSp.spriteName;	
		switch (pos) 
		{
			case MIRROR_POS.right:
			case MIRROR_POS.topRight:
				if(newSp.pivot == UIWidget.Pivot.Center)
				{
						newSp.transform.localPosition = new Vector3(calNewPos(vecPos.x, width, origSp.pivotOffset.x+offset, true), 
						                                            vecPos.y, vecPos.z);
				}
				else if(newSp.pivot == UIWidget.Pivot.TopLeft)
				{
					newSp.transform.localPosition = new Vector3(calNewPos(vecPos.x, width, origSp.pivotOffset.x, true) + width, 
					                                            vecPos.y, vecPos.z);
				}
				newSp.transform.localScale = new Vector3(-vecScale.x, vecScale.y, vecScale.z);
				break;
			case MIRROR_POS.bottomLeft:
				newSp.transform.localPosition = new Vector3(vecPos.x, calNewPos(vecPos.y, height, origSp.pivotOffset.y, false), vecPos.z);
				newSp.transform.localScale = new Vector3(vecScale.x, -vecScale.y, vecScale.z);
				break;
			case MIRROR_POS.bottomRight:
				newSp.transform.localPosition = new Vector3(calNewPos(vecPos.x, width, origSp.pivotOffset.x+offset, true), 
				calNewPos(vecPos.y, height, origSp.pivotOffset.y, false), vecPos.z);
				newSp.transform.localScale = new Vector3(-vecScale.x, -vecScale.y, vecScale.z);
				break;
			default:				
				break;
		}
		newSp.depth = origSp.depth;
		newSp.width = origSp.width;
		newSp.height = origSp.height;
		return newSp;
	}
		
	//为不同的ICON设置不同的贴图
	public void setTextureToIcon(UITexture equip,  ItemTemplate itemTemp, bool overLay = true)
	{
		Shader shader = null;
		if(overLay)
			shader = Shader.Find("Unlit/Transparent Colored Overlay");	
		else
			shader = Shader.Find("Unlit/Transparent Colored");
		
		#region 换成ngui3.X后加的判断
		if (equip.material==null) {
			equip.material = new Material(shader);
		}
		#endregion
		else if(!equip.material.shader.name.Equals(shader.name)) //如果material的shader不一样要替换
			equip.material = new Material(shader);	
		equip.mainTexture = SourceManager.Instance.getTextByIconName(itemTemp.icon);
		equip.shader = shader;		
        //if(itemTemp.itemType == eItemType.eEquip) //生成不同颜色
        //{
        //    //float color = ItemManager.GetInstance().getItemColor(itemTemp.quality);
        //    //createEqipColor(equip, itemTemp.icon, color);
        //}
	}
	
	//清除Texture为Unlit
	public void clearTexture(UITexture texture)
	{
		Shader shader = Shader.Find("Unlit/Texture");	
		if(!texture.shader.name.Equals(shader.name))
		{
//			texture.mainTexture = SourceMenager.GetInstance().getTextByIconName(Constant.IMAGE_EXP); //需要一个模板填充
			texture.mainTexture = null;
			texture.material = null;	
			texture.shader = null;			
//			texture.material.shader = shader;
//			texture.material = new Material(shader);
//			texture.material.mainTextureScale = new Vector2(0, 0);
//			texture.shader = shader;
		}
	}
	
	//为不同的ICON设置不同的贴图
	public void setTextureToIcon(UITexture equip,  string textureName, bool overLay = true)
	{
		Shader shader = null;
		if(overLay)
			shader = Shader.Find("Unlit/Transparent Colored Overlay");	
		else
			shader = Shader.Find("Unlit/Transparent Colored");
		#region 换成ngui3.X后加的判断
		if (equip.material==null) {
			equip.material = new Material(shader);
		}
		#endregion
		else if(!equip.material.shader.name.Equals(shader.name))
			equip.material = new Material(shader);
		equip.shader = shader;
		equip.mainTexture = SourceManager.Instance.getTextByIconName(textureName);	
	}
	
	
	//移除所有装备控件
	public void freeResource()
	{
	}
	
	//创建新的装备，生成不同的颜色，入参数是装备本身
	private void createEqipColor(UITexture equip,  string equipName, float hue)
	{
		if(equip.transform.parent.Find(Constant.ITEM_BORDER_SPRITE_NAME) != null) //已经存在颜色
		{
			GameObject prevColor = equip.transform.parent.Find(Constant.ITEM_BORDER_SPRITE_NAME).gameObject;
			GameObject.Destroy(prevColor);
		}
		if(hue != COLOR_NONE)	//有颜色才需要装备
		{
			string bgName = equipName + Constant.ITEM_BORDER_PNG;	
			UITexture bgTexture = createTexture(bgName, equip.transform.parent.gameObject);
			bgTexture.shader =  equip.shader;//Shader.Find("unlit/Transparent Colored (AlphaClip)");
			bgTexture.transform.localPosition = equip.transform.localPosition;
			bgTexture.transform.localScale = equip.transform.localScale;
			bgTexture.name = Constant.ITEM_BORDER_SPRITE_NAME;
			bgTexture.depth = equip.depth-1;
			float[] rgbs = hsb2rgb(hue);
			bgTexture.color = new Color(rgbs[0], rgbs[1], rgbs[2]);
		}
	}
	
	//计算实际的,x变小y变大
	private float calNewPos(float oldPos, float size, float offset, bool isX)
	{
		if(offset > 0)
		{
			if(isX)
				return oldPos + size - offset;
			else
				return oldPos - size + offset;
		}
		else
		{
			if(isX)
				return oldPos + size + offset;
			else
				return oldPos - size - offset;
		}
	}
	
	//生成新的UISprite
	private UISprite createSp(string spName, GameObject go)
	{
        //UISprite equipSp = NGUITools.AddWidget<UISprite>(go);
        //equipSp.atlas = _equipAtlas;
        //equipSp.spriteName = spName;
        //equipSp.MakePixelPerfect();
        //equipSp.transform.localPosition = Vector3.zero;
        //return equipSp;
	    return null;
	}
	
	//生成新的UItexture
	private UITexture createTexture(string textureName, GameObject go)
	{
		UITexture equipTexture = NGUITools.AddWidget<UITexture>(go);
		equipTexture.mainTexture = SourceManager.Instance.getTextByIconName(textureName);
		equipTexture.MakePixelPerfect();
		equipTexture.transform.localPosition = Vector3.zero;
		return equipTexture;
	}
	
	//色相装换
	 /*	* h  颜色      用角度表示，范围：0到360度 
	 	* s  色度      0.0到1.0   0为白色，越高颜色越“纯” 
	 	* v  亮度      0.0到1.0   0为黑色，越高越亮 
	 */  
	private float[] hsb2rgb(float h, float s=1f, float v=1f) 
	{  
		float r = 0, g = 0, b = 0;  
    	int i = (int) ((h / 60) % 6);  
    	float f = (h / 60) - i;  
    	float p = v * (1 - s);  
    	float q = v * (1 - f * s);  
    	float t = v * (1 - (1 - f) * s);  
    	switch (i) 
		{  
		    case 0:  
		        r = v;  
		        g = t;  
		        b = p;  
		        break;  
		    case 1:  
		        r = q;  
		        g = v;  
		        b = p;  
		        break;  
		    case 2:  
		        r = p;  
		        g = v;  
		        b = t;  
		        break;  
		    case 3:  
		        r = p;  
		        g = q;  
		        b = v;  
		        break;  
		    case 4:  
		        r = t;  
		        g = p;  
		        b = v;  
		        break;  
		    case 5:  
		        r = v;  
		        g = p;  
		        b = q;  
		        break;  
		    default:  
		        break;  
    	}  
    	return new float[] { r, g, b };  
	}  
}
