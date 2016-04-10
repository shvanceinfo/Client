using UnityEngine;
using System.Collections;

public enum SERVICE_TYPE
{
	eInit = 0,  //初始未选中状态
	eNotice,	//公告
	eService,	//客服
	eAnnoucement, //运维公告
	eHelp,		//帮助
	eSetting	//设置	
};

public enum HELP_TYPE
{
	eInit = 0,  //初始未选中状态
	eAdvanced,	//进阶
	eAffinate,	//洗练
	eAnalysis,	//分解
	eInlay,		//镶嵌
	eStrengthen	//强化	
};

public enum REPORT_TYPE
{
	eNone =0,
	eBug,		//BUG
	eComplaint, //投诉
	eSuggest,	//建议
	eOther		//其它
}

public class ServiceMsg
{    
    public uint msgID; 		//公告ID
    public SERVICE_TYPE  msgType;	//1:系统公告;2:客服信息;3:运维公告
    public string msgTime;	//发布的时间
    public uint titleLen;	//标题长度
    public uint contentLen;	//内容长度
    public uint authorLen;	//作者长度
    public string title;	//标题
    public string content;	//内容
    public string author;	//作者
}

public class ServiceManager 
{
	private static ServiceManager _sInstance = null;
	
	public GameObject helpNotice;		//帮助公告
	public GameObject customerService;	//客服界面
	public GameObject gameSetting;		//游戏设置
	public bool submitOpen;
	
	private ServiceMsg _gameNotice;  	//游戏的公告，在游戏开始就获取
	private ServiceMsg _serviceMsg;  	//客服的信息，在打开客服面板的时候获取
	
	private int _maxHelpWidth = 750; 		//最大的帮助的宽度
	
    private ServiceManager() 
    {
    	helpNotice = null;
    	customerService = null;
    	gameSetting = null;
    	submitOpen = false;
    }
    
    public static ServiceManager Instance
    {
    	get
    	{
    		if(_sInstance == null)
    			_sInstance = new ServiceManager();
    		return _sInstance;
    	}
    }
    
    //定义客服中底部框的代理，并在点击后触发
    public delegate void SelectServiceTab(SERVICE_TYPE type);
    public event SelectServiceTab eventSelectService;
    public void OnSelectServiceTab(SERVICE_TYPE type)
    {
        if (eventSelectService != null)
        {
            eventSelectService(type);
        }
    }
    
    //定义点击bug的代理，并在点击后触发
    public delegate void SelectReportBox(REPORT_TYPE type);
    public event SelectReportBox eventSelectReport;
    public void OnSelectReportBox(REPORT_TYPE type)
    {
        if (eventSelectReport != null)
        {
            eventSelectReport(type);
        }
    }
    
    //定义返回按钮，关闭相应发送bug的弹窗
    public delegate void CloseSendMsg();
    public event CloseSendMsg eventCloseSend;
    public void OnCloseSendMsg()
    {
        if (eventCloseSend != null)
        {
            eventCloseSend();
        }
    }

	public void setData(ServiceMsg msg)
	{
		if(msg.msgType == SERVICE_TYPE.eNotice) //公告
		{
			_gameNotice = msg;
		}
		else if(msg.msgType == SERVICE_TYPE.eService)
		{
			_serviceMsg = msg;
			setServiceInfo(); //收到网络消息设置面板
		}
	}
	
	//根据不同的类型显示不同的数据
    //public void showData(SERVICE_TYPE type, bool firstShow=true)
    //{
    //    switch (type) 
    //    {
    //        case SERVICE_TYPE.eInit:				
    //            break;
    //        case SERVICE_TYPE.eNotice:
    //        case SERVICE_TYPE.eHelp:
    //            if(firstShow) //第一次直接显示，以后就是缓动替换显示
    //            {
    //                helpNotice.SetActive(true);
    //                customerService.SetActive(false);
    //                gameSetting.SetActive(false);
    //                setNotice(type == SERVICE_TYPE.eNotice);
    //            }
    //            else
    //            {
    //                tweenMenu();
    //            }
    //            break;
    //        case SERVICE_TYPE.eService:
    //            if(firstShow)
    //            {
    //                helpNotice.SetActive(false);
    //                customerService.SetActive(true);
    //                gameSetting.SetActive(false);
    //                if(_serviceMsg == null)
    //                {
    //                    GCAskNotice notice = new GCAskNotice(SERVICE_TYPE.eService); //请求服务器发送公告
    //                    MainLogic.SendMesg(notice.ToBytes());
    //                }
    //                else
    //                {
    //                    setServiceInfo();
    //                }
    //            }
    //            else
    //            {
    //                tweenMenu();
    //            }
    //            break;
    //        case SERVICE_TYPE.eSetting:
    //            if(firstShow)
    //            {
    //                helpNotice.SetActive(false);
    //                customerService.SetActive(false);
    //                gameSetting.SetActive(true);
    //            }
    //            else
    //            {
    //                tweenMenu();
    //            }
    //            break;
    //        default:
    //            break;
    //    }
    //}
	
	//根据不同的帮助类型改变数据
	public void changeHelpData(HELP_TYPE type)
	{
		Transform helpTrans = helpNotice.transform.Find("help_panel");
		UILabel title = helpNotice.transform.Find("title").GetComponent<UILabel>();
		UILabel scrollLabel = helpTrans.Find("ScrollView/TextList/Label").GetComponent<UILabel>();
		UIScrollBar scrollBar =  helpTrans.Find("Scroll Bar").GetComponent<UIScrollBar>();
		scrollBar.scrollValue = 0f; //置位scroll bar
		scrollLabel.lineWidth = _maxHelpWidth;
		scrollLabel.text = "";
		HelpInfo helpInfo = ConfigDataManager.GetInstance().getHelpConfig().getHelpInfo((int)type);
		title.text = helpInfo.helpTitle;
		foreach(string perLine in helpInfo.helpContent)
		{
			scrollLabel.text += Constant.PARAGRAPH_BEGIN_SIMBOL + perLine + Constant.PARAGRAPH_END_SIMBOL;
		}
	}
	
	//滑动的淡入
	private void tweenMenu()
	{
		
	}
	
	//根据公告还是帮助的类型显示相应的数据
	private void setNotice(bool isNotice)
	{
		Transform helpTrans = helpNotice.transform.Find("help_panel");
		Transform noticeTrans = helpNotice.transform.Find("notice_panel");	
		if(isNotice)
		{
			UILabel title = helpNotice.transform.Find("title").GetComponent<UILabel>();
			helpTrans.gameObject.SetActive(false);
			noticeTrans.gameObject.SetActive(true);		
			title.text = GameNotice.title;			
			UILabel content = noticeTrans.transform.Find("contentView/TextList/content").GetComponent<UILabel>();
			content.lineWidth = 800;
			content.text = GameNotice.content;
			UILabel author = noticeTrans.transform.Find("author").GetComponent<UILabel>();
		    author.text = "《降临》运营团队";//GameNotice.author;
			UILabel time = noticeTrans.transform.Find("time").GetComponent<UILabel>();
			time.text = GameNotice.msgTime;
		}
		else
		{
			helpTrans.gameObject.SetActive(true);
			noticeTrans.gameObject.SetActive(false);
		}
	}
	
	//设置客服的消息
	private void setServiceInfo()
	{
		UILabel leftMsg = customerService.transform.Find("left_msg").GetComponent<UILabel>();
		UILabel titleMsg = customerService.transform.Find("right_input/title").GetComponent<UILabel>();
		titleMsg.text = ServiceMsg.title;
		leftMsg.text = ServiceMsg.content;
	}
	
	public ServiceMsg GameNotice
	{
		get 
		{ 
			if(_gameNotice == null)
			{
				_gameNotice = new ServiceMsg();
				if(_gameNotice.titleLen == 0)
				{
					_gameNotice.title = LanguageManager.GetText("title_notice");
				}
				if(_gameNotice.authorLen == 0)
				{
					_gameNotice.author = "降临运营团队";
				}
				if(_gameNotice.contentLen == 0)
				{
				    _gameNotice.content = Constant.PARAGRAPH_BEGIN_SIMBOL +
				                          "尊敬的各位：" +
				                          Constant.PARAGRAPH_END_SIMBOL + Constant.PARAGRAPH_BEGIN_SIMBOL +
				                          "欢迎进入游戏体验《降临》第一次内部技术测试，本次测试旨在系统稳定性和游戏更加完美，在测试过程如果您有任何的游戏建议和意见或者对游戏想做更多了解的话，欢迎直接联系我们，我们将认真对待每一个问题，对待每一个合作伙伴！" +
				                          Constant.SECTION_BEGIN_SIMBOL +
				                          "联系QQ：9803825（蒋先生）、29833983（王先生）";//+ Constant.SECTION_BEGIN_SIMBOL +
						//"3.调整装备、技能系统，玩法更有可玩性" + Constant.SECTION_BEGIN_SIMBOL +
						//"4.优化创建角色场景" + Constant.SECTION_BEGIN_SIMBOL +
						//"5.整体UI风格更新"+Constant.SECTION_BEGIN_SIMBOL +
                        //"6.修复若干BUG" + Constant.SECTION_BEGIN_SIMBOL +
						//"7.优化若干用户体验问题";//+
//						Constant.PARAGRAPH_END_SIMBOL+Constant.PARAGRAPH_BEGIN_SIMBOL+
//						"健康游戏忠告"+ Constant.SECTION_BEGIN_SIMBOL +
//						"抵制不良游戏，拒绝盗版游戏。" + Constant.SECTION_BEGIN_SIMBOL +
//						"注意自我保护，谨防上当受骗。" + Constant.SECTION_BEGIN_SIMBOL +
//						"适度游戏益脑，沉迷游戏伤身。" + Constant.SECTION_BEGIN_SIMBOL +
//						"合理安排时间，享受健康生活。";
				}
			}
			return _gameNotice; 
		}
	}
	
	public ServiceMsg ServiceMsg
	{
		get 
		{ 
			if(_serviceMsg == null)
			{
				_serviceMsg = new ServiceMsg();
				if(_serviceMsg.titleLen == 0)
				{
					_serviceMsg.title = LanguageManager.GetText("title_service");
				}
				if(_serviceMsg.contentLen == 0)
				{
					_gameNotice.content = Constant.PARAGRAPH_BEGIN_SIMBOL+
						"QQ:11223311"+
						Constant.PARAGRAPH_END_SIMBOL+Constant.PARAGRAPH_BEGIN_SIMBOL+
						"客服邮箱：jianglin@3top7.com"+
						Constant.PARAGRAPH_END_SIMBOL+Constant.PARAGRAPH_BEGIN_SIMBOL+
						"客服邮箱：jianglin@3top7.com"+
						Constant.PARAGRAPH_END_SIMBOL+Constant.PARAGRAPH_BEGIN_SIMBOL+
						"客服邮箱：jianglin@3top7.com";
				}
			}
			return _serviceMsg; 
		}
	}
}
