using UnityEngine;
using System.Collections;

public class MirrorSprite : MonoBehaviour 
{
	public int mirrorMethod = 0; //镜像的方法, 0是一半镜像，1是四分之一镜像
	public int offset = 0; //偏移量
	public bool useImageSize = true; //是否使用图片尺寸 
	
	void Awake () 
	{
		UISprite sp = gameObject.GetComponent<UISprite>();
		bool makePerfect = false;
		if(useImageSize)
			makePerfect = true;
		if(mirrorMethod == 0)
			DealTexture.Instance.mirrorSprite(sp, DealTexture.MIRROR_POS.right, makePerfect, offset);
		else
		{
			DealTexture.Instance.mirrorSprite(sp, DealTexture.MIRROR_POS.topRight,  makePerfect, offset);
        	DealTexture.Instance.mirrorSprite(sp, DealTexture.MIRROR_POS.bottomLeft,  makePerfect, offset);
        	DealTexture.Instance.mirrorSprite(sp, DealTexture.MIRROR_POS.bottomRight,  makePerfect, offset); 
		}
	}
}
