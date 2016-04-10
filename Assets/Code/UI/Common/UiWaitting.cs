//网络等待的信号
using UnityEngine;
using System.Collections;

public class UiWaitting : MonoBehaviour 
{
	public delegate void NetWorkFinish();   
    public NetWorkFinish netWorkFinish;
	
	private bool _isRotate; //是否开始旋转
    private float _rotateValue;
    private Transform _rotateTrans;
	private UILabel _waitLabel; //在连接过程中的提示
	private UILabel _connectLabel; //连接的原因	
	private float _consumeTime; //断线重连消耗的时间
	private int _reconnectNum; //重练的次数
	private bool _hasShow;
    
    void Awake()
    {
        _rotateTrans = transform.FindChild("rotateSp");
        //_waitLabel = transform.Find("waitContent").GetComponent<UILabel>();
    	_connectLabel = transform.Find("connectContent").GetComponent<UILabel>();
    	//_waitLabel.text = LanguageManager.GetText("connect_first");
    	_connectLabel.text = " ";
    	_isRotate = false;
    	_reconnectNum = 1;
    	_consumeTime = 0f;
    	_hasShow = false;
    }

	//使用
	void Update() 
	{
		if(_isRotate)
		{
			_consumeTime += Time.deltaTime;
	        _rotateValue = Time.deltaTime * 360;
	        _rotateTrans.localEulerAngles = new Vector3(0, 0, _rotateTrans.localEulerAngles.z - _rotateValue);
	        if(_consumeTime > Constant.WAIT_TIME)
	        {
	        	_consumeTime = 0f;
	        	_reconnectNum++;
	        	stillWaiting();
	        }
	        if(_reconnectNum > Constant.RECONNECT_NUM) //大于重连次数，那么就重新登录
	        {
	        	_reconnectNum = 0;
	        	//loginServer();
	        }
		}
	}

	//显示等待的信息
	public void showWaiting( bool isShow)
	{
		if(isShow)
		{
			if(!_hasShow) //并且没有显示过
			{
				//MainLogic.sMainLogic.suspendGame(); //重连暂停游戏
				if(Application.internetReachability == NetworkReachability.NotReachable) //没有网络
				{
					gameObject.active = true;
					_isRotate = true;
					//_waitLabel.text = LanguageManager.GetText("connect_no_net");
		    		_connectLabel.text = LanguageManager.GetText("connect_fail0");
		    		InvokeRepeating("loginServer", 2f, 2f); //每两秒登录一次
				}
				else
				{
					_hasShow = true;
					gameObject.active = true;
					_isRotate = true;
					_reconnectNum = 1;
		    		_consumeTime = 0f;
		    		//_waitLabel.text = LanguageManager.GetText("connect_first");
		    		_connectLabel.text = " ";
				}
			}
		}
		else //第一次进来可要释放资源？？
		{
			MainLogic.sMainLogic.resumeGame(); //关闭窗口恢复游戏
			if(_hasShow)
			{
				gameObject.active = false;
				_isRotate = false;
				_hasShow = false;
			}
		}
	}
	
	//继续等待网络响应
	private void stillWaiting()
	{
		if(_reconnectNum == 2)
		{
			//_waitLabel.text = LanguageManager.GetText("connect_second");
    		_connectLabel.text = LanguageManager.GetText("connect_fail2");
		}
		else if(_reconnectNum == 3)
		{
			//_waitLabel.text = LanguageManager.GetText("connect_second");
    		_connectLabel.text = LanguageManager.GetText("connect_fail3");
		}
	}
	
	//继续请求登录服务器
	private void loginServer()
	{	
		if(Application.internetReachability != NetworkReachability.NotReachable) //有网络就开始连接服务器
		{
			UIManager.Instance.closeWaitting(); //清除等待
			MainLogic.sMainLogic.ConnectGameServer(); //链接服务器
			CancelInvoke(); //取消监听
		}
	}
}
