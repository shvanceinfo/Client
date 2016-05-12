using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using helper;
using UnityEngine;
using ProtoBuf;
using NetPackage;
using System.IO;
using System.Text;

namespace NetGame
{
   
    class NetBase
    {
    	public bool kickOutByServer; 	//被客户端踢下线
        public bool clientDisconnect; 	//客户端重置
        public bool serverDisconnect; 	//服务端断开重练 

        public const bool DEBUGER = false;

        protected Socket _socket = null;
        protected string _ip;
        protected int _port;
        protected Thread _thread = null;
        ManualResetEvent _connectDone = new ManualResetEvent(false);
        protected IMessage _message;
        protected byte[] _buff = new byte[NetBuff.DEFAULT_BUFF_LENGTH];
        protected NetBuff _readBuff = new NetBuff();

        protected int _connectNum = 0;

        protected static NetBase _instance;

        private bool _createRecvThread = true; //是否需要创建线程
        private bool _reconnectOnce = false;  //保证只重连一次
        private DateTime _lastConnectTime;	  //最后一次ping的时间
        
        public Socket Socket
        {
        	set{ _socket = value;}
        }
        
        protected NetBase()
        {         
			kickOutByServer = false;        	
        	serverDisconnect = false;
        	clientDisconnect = false;
        	_reconnectOnce = false;  
        	_createRecvThread = true;
			_lastConnectTime = DateTime.Now;
        }
        
        ~NetBase()
        {
            Close();
        }
        
        static public NetBase GetInstance()
        {
            if (_instance == null)
            {
                _instance = new NetBase();
            }
            return _instance;
        }

        public void SetMessage(IMessage message)
        {
            _message = message;
        }
      
        //判断网络是否连接状态
        public bool IsConnected
        {
        	get
        	{
				if (serverDisconnect || _socket == null || !_socket.Connected)
    		    {
    		    	/*clientDisconnect = false;
    		    	_lastConnectTime = DateTime.Now;
    		    	if(_reconnectOnce)  
    		    	{
    		    		serverDisconnect = true;
    		    		MainLogic.sMainLogic.needReconnect = true;
						_reconnectOnce = false;//开始连接了就不能再次重连  
    		    	}
    		    	return false;
    		    }
        		else if( _socket == null || !_socket.Connected)
	            {      			
        			if(_reconnectOnce)  
    		    	{
						clientDisconnect = true;
    		    		//MainLogic.sMainLogic.needReconnect = true;
    		    		_reconnectOnce = false;
    		    	}
	                return false;
	            }
        		else
        		{
        			_lastConnectTime = DateTime.Now;
        			return true;
        		}
//	            else if(ToolFunc.getDeltaSecond(ref lastPingTime) > Constant.DISCONNECT_CHECK) //ping 超时就代表断线
//				{
//					//Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//					try
//					{
//		            	if(_socket.Poll(-1, SelectMode.SelectRead))
//		            	{
//		            		byte[] buf = new byte[32];
//		            		int recvNum = _socket.Receive(buf);
//		            		if(recvNum == 0)  //真正意义上socket已经由服务器那边断开连接
//		            			isConnected = false;
//		            	}
//					}
//					catch(ObjectDisposedException ex)
//					{
//						isConnected = false;
//					}
//					isConnected = false; //
//	            }*/


					// show disconnect UI
					if (MainLogic.sMainLogic.isInGame) {
						UIManager.Instance.ShowDialog (eDialogSureType.eDisconnect, LanguageManager.GetText ("client_disconnected"));
                        EasyTouchJoyStickProperty.ShowJoyTouch(false);
						MainLogic.sMainLogic.isInGame = false;
					}
					return false;
				}
				return true;
        	}
        }

        public void ReConnect()
        {
            Loger.Notice("reconnect to server~~");
            Connect(_ip, _port);
        }

        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <param name="port">端口</param>
        public void Connect(string ip, int port, bool bCloseThread = false)
        {
        	if(kickOutByServer) //服务器踢下线，永不重练
        		return;
        	Close(bCloseThread);
        	_connectDone.Reset();
			UIManager.Instance.showWaitting(true); //开始联网的时候显示连接状态
            _ip = ip;
            _port = port;
			_buff = new byte[1024];
        	_readBuff = new NetBuff(); //清除原来缓存
            IPAddress ipa = IPAddress.Parse(_ip);
            IPEndPoint ipe = new IPEndPoint(ipa, port);
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.BeginConnect(ipe, new AsyncCallback(ConnectCallback), _socket);   
//			_reconnectOnce = false;           
            _connectDone.WaitOne();
            BeginRece();                
        }
        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close(bool forceClose = false)
        {
            if (serverDisconnect || forceClose || clientDisconnect) //如果是服务器断开连接，需要重新创建线程
            {
                try
                {
					if(_thread != null)
	                	_thread.Abort();			//关闭线程
	                _createRecvThread = true; //需要再次创建一个线程
                    if(_socket != null)
                    {
	                    _socket.Shutdown(SocketShutdown.Both);
	                    _socket.Close();
	                    Loger.Notice("close socket~~~");
                    }
                }
                catch(Exception ex)
                {
                    Loger.Notice(ex.ToString());
                }
            }
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="bytesData">内容</param>
        public void Send(byte[] bytesData, bool showWait = true)
        {
            if (IsConnected)
            {
	            if (_createRecvThread)
	            {
	                BeginRece();
	            }
	            ///异步发送
	            //_socket.BeginSend(bytesData, 0, bytesData.Length, SocketFlags.None, new AsyncCallback(SendCallback), _socket);
	            ShowSendHeadMsg(bytesData);
	            if(showWait) //是否需要显示加载窗口
	            	UIManager.Instance.showWaitting(); //如果Send的时候没有Recv回调那么就会waiting
            }
            //同步发送
            //_socket.Send(bytesData, 0, bytesData.Length, SocketFlags.None);
        }

        public void Send<T>(T obj, ushort cmd, bool showWait = true)
        {
            if (IsConnected)
            {
                if (_createRecvThread)
                {
                    BeginRece();
                }
                ///异步发送
                ///

                NetHead head = new NetHead();
                //head.ToObject(data);


                MemoryStream ms = new MemoryStream();
                Serializer.Serialize<T>(ms, obj);

                //压入包头
                head._assistantCmd = cmd;
                head._body = System.Text.Encoding.UTF8.GetString(ms.ToArray(), 0, ms.ToArray().Length);
                head._length = (UInt16)head._body.Length;

                //MemoryStream real = new MemoryStream();
                //Serializer.Serialize<CNetHead>(real, head);

                //byte[] bytesData = real.ToArray();
                byte[] bytesData = head.ToBytes();

                _socket.BeginSend(bytesData, 0, bytesData.Length, SocketFlags.None, new AsyncCallback(SendCallback), _socket);
                ShowSendHeadMsg(bytesData);
                if (showWait) //是否需要显示加载窗口
                    UIManager.Instance.showWaitting(); //如果Send的时候没有Recv回调那么就会waiting
            }
            //同步发送
            //_socket.Send(bytesData, 0, bytesData.Length, SocketFlags.None);
        }

        /// <summary>
        /// 异步连接回掉
        /// </summary>
        /// <param name="ar"></param>
        protected void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                _socket.EndConnect(ar);
                Loger.Log("Socket connected to {0}",  _socket.RemoteEndPoint.ToString());               
                serverDisconnect = false;
                _reconnectOnce = true;
            }
            catch (System.Exception ex)
            {
                Loger.Notice(ex.ToString());
            }
            finally
            {
                _connectDone.Set();
            }
        }

        /// <summary>
        /// 异步发送数据回掉
        /// </summary>
        /// <param name="ar"></param>
        protected void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket sock = (Socket)ar.AsyncState;
                int bytesSend = sock.EndSend(ar);
            }
            catch (Exception ex)
            {
                Loger.Notice(ex.ToString());
            }
        }

        protected void ShowSendHeadMsg(byte[] data)
        {
            if (DEBUGER)
            {
                NetHead head = new NetHead();
                head.ToObject(data);
                Loger.Log("send msg type={0},wlen={1},msgLen={2}", head._assistantCmd, data.Length);
            }
        }

        protected void BeginRece()
        {
            _connectDone.WaitOne();
            if (IsConnected)
            {
                Loger.Log("new thread rece~~~");
                if(_createRecvThread) //只有没创建线程才必须创建线程
                {
	                _createRecvThread = false;
	                _thread = new Thread(new ThreadStart(StartRece)); //接收使用独立线程
	                _thread.IsBackground = true;
	                _thread.Start();
                }
                EventDispatcher.GetInstance().OnConnectedServer();
            }
            _connectDone.Set();
        }
        /// <summary>
        /// 接受数据回掉
        /// </summary>
        /// <param name="ar"></param>
        protected void ReceCallback(IAsyncResult ar)
        {            
            if (IsConnected)
            {
            	try
	            {
	                int readLength = _socket.EndReceive(ar);
	                if (readLength > 0)
	                {
	                    _readBuff.AddBuff(_buff, readLength);
	                    Package.Parse(ref _readBuff, ref _message);
	                }                   
	                StartRece();
	            }
	            catch(Exception ex)
	            {
	                Loger.Notice(ex.ToString());
	            }
            }
        }
        /// <summary>
        /// 监听接受数据
        /// </summary>
        protected void StartRece()
        {
            try
            {
                _socket.BeginReceive(_buff, 0, _buff.Length, SocketFlags.None, ReceCallback, _socket);
            }
            catch (ThreadAbortException)
            {
                Debug.Log(" 线程直接出现异常退出！！！！！！");
            }
            catch (System.Exception ex)
            {
                Debug.Log(" 其他的异常状况出现！" + ex.ToString());
            }
        }

        protected void Ping()
        {

        }
    }
}
