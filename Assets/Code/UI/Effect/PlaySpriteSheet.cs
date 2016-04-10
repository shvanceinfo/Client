/**该文件实现的基本功能等
function:播放材质球的相关动作
author:ljx
date:2013-11-21
**/
using UnityEngine;
using System.Collections;

public class PlaySpriteSheet : MonoBehaviour 
{
//	public int columns = 2; 			//列数
//  public int rows = 2;  				//行数
    public int framesPerSecond = 10;	//每秒多少帧
    public Vector3 scale;               //缩放比例

    private BetterList<string> _allSpriteNames;			//所有的特效Sprite
    private UISprite _uiSprite;
    private float _waitTimes;	//每次的等待时间	
    private int _currentFrame; //当前帧数
    private int _totalNum; //全部总数
    private bool _startFlag;	//是否开启标志，在UIFight跟UiMain切换的时候使用
    
    void Awake()
    {
    	_uiSprite = gameObject.GetComponent<UISprite>();
    	_allSpriteNames = _uiSprite.atlas.GetListOfSprites();
    	_startFlag = true;
    	if(_allSpriteNames != null && _allSpriteNames.size > 0)
    	{
    		_allSpriteNames.Sort(delegate(string a, string b) { return a.CompareTo(b); });
    		if(_currentFrame <=0)
    			_currentFrame = 0;
    		_totalNum = _allSpriteNames.size;
    		transform.localScale = new Vector3(_uiSprite.GetAtlasSprite().width, _uiSprite.GetAtlasSprite().height, 0f);
    		if(framesPerSecond <=0)
    			framesPerSecond = 10;
    		_waitTimes = 1f/ framesPerSecond;
    	}
    }
    
    void Start()
    {
    	_currentFrame = Random.Range(0, _totalNum-1); //随机帧开始播放
    	startPlay();
    }
	
	void OnEnable()
	{
		_currentFrame = Random.Range(0, _totalNum-1);
		startPlay();
	}
	
	void OnDisable()
	{
		StopAllCoroutines();
		_startFlag = true;
	}
    
    IEnumerator playAnimate()
    {
        while (true)
        {
        	if(_currentFrame >= _totalNum)
        		_currentFrame = 0;
        	_uiSprite.spriteName = _allSpriteNames[_currentFrame];
			_uiSprite.MakePixelPerfect();
            transform.localScale = scale;
            _currentFrame++;
            yield return new WaitForSeconds(_waitTimes);
        }
    }
    
    void startPlay()
    {
    	if(_startFlag && _allSpriteNames != null && _allSpriteNames.size > 0)
    	{
    		StartCoroutine(playAnimate());
    		_startFlag = false;
    	}
    }
    
//    void Awake()
//    {	
//    	if(renderer != null)
//    	{
//    		_size = new Vector2(1f/columns, 1f/rows);
//    		_frames = 0;
//    		renderer.sharedMaterial.SetTextureScale("_MainTex", _size);
//    		_waitTimes = 1f/ framesPerSecond;
//    	}
//    }
// 
//    void Start()
//    {
//    	if(renderer != null)
//    	{
//	        StartCoroutine(playAnimate());
//    	}
//    }
// 
//    private IEnumerator playAnimate()
//    {
//        while (true)
//        {
//        	_frames++;
//            if (_frames >= rows*columns)
//           		_frames = 0;
// 			Vector2 offset = new Vector2((float)_frames / columns - (_frames / columns), //x index
//                                      1- (_frames / columns) / (float)rows);          //y index
//            renderer.sharedMaterial.SetTextureOffset("_MainTex", offset);       
//            yield return new WaitForSeconds(_waitTimes);
//        }
//    }
    
//	public int columns = 2; 			//列数
//    public int rows = 2;  				//行数
//    public int framesPerSecond = 10;	//每秒多少帧
//    
//    private UISpriteData _atlasSprite;			//UI的材质
//    private UISprite _uiSprite;
//    private float _imageW;  		//小图片的宽
//    private float _imageH;  		//小图片的高
//    private float _bigImageH;
////    private float _scaleW; 		//图片大小的宽度比例
////    private float _scaleH;		//图片大小的高度比例
//    private float _waitTimes;	//每次的等待时间
//    private int _currentRow; 	//当前行数
//    private int _currentCol; 	//当前列数
//    
//    
//    void Awake()
//    {
//    	_uiSprite = gameObject.GetComponent<UISprite>();
//    	//_uiSprite.innerUV
//    	_atlasSprite = _uiSprite.GetAtlasSprite();
//    	if(_atlasSprite != null)
//    	{
//    		_currentCol = 0;
//    		_currentRow = 0;
//    		_imageW = _atlasSprite.outer.width/columns;
//    		_bigImageH = _atlasSprite.outer.height;
//    		_imageH = _bigImageH/rows;
//    		transform.localScale = new Vector3(_imageW, _imageH, 0f);
//    		if(framesPerSecond <=0)
//    			framesPerSecond = 10;
//    		_waitTimes = 1f/ framesPerSecond;
//    	}
//    }
// 
//    void Start()
//    {
//    	if(_atlasSprite != null)
//    	{
//	        //StartCoroutine(playAnimate());
//    	}
//    }
// 
//    private IEnumerator playAnimate()
//    {
//        while (true)
//        {
//            if (_currentCol >= columns)
//            {
//                _currentCol = 0;
//                _currentRow++;
//                if(_currentRow >= rows)
//                	_currentRow = 0;
//            }
//            _atlasSprite.outer = new Rect(_currentCol*_imageW, _bigImageH - _currentRow*_imageH, _imageW, _imageH);
//			_atlasSprite.inner = new Rect(_currentCol*_imageW, _bigImageH - _currentRow*_imageH, _imageW, _imageH);
//			_uiSprite.MakePixelPerfect();
////	        renderer.material.SetTextureOffset("_MainTex", offset);
////	        _texture.material.SetTextureOffset(_texture.name, offset);
//// 			_texture.uvRect = new Rect(_currentCol*_imageW, _currentRow*_imageH, _scaleW, _scaleH);
//            _currentCol++;
//            yield return new WaitForSeconds(_waitTimes);
//        }
//    }
}
