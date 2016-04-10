using UnityEngine;
using System.Collections;
using helper;
using model;
using manager;
using mediator;
using MVC.entrance.gate;
public class TalkView : MonoBehaviour {

    private UITextList _textList;           //内容list
    private GameObject _contentObj;         //内容模块
    private GameObject _settingObj;         //设置模块

    private UILabel _curSendChannel;        //当前频道标签
    private GameObject _channelList;        //频道列表
    private UIInput _input;
    private UILabel _inputLabel;
    private GameObject _playerTipObj;       //玩家TIP功能菜单

    private GameObject _whisperObj;         //输入私聊玩家面板
    private GameObject _friendsObj;         //好友列表

    private UIInput _friendInput;           //好友输入

	private GameObject _friend;			//好友的功能 

    private void Awake()
    {
        _contentObj = F("Content");
        _settingObj = F("Setting");
        _textList = F<UITextList>("Content/TextList");
        _inputLabel = F<UILabel>("Content/TextList/Label");

        _curSendChannel = F<UILabel>("Send/Type/Label");
        _channelList = F("Send/TypeList");
        _channelList.SetActive(false);

        _input = F<UIInput>("Send/Input");

        _playerTipObj = F("FunctionList");
        _playerTipObj.SetActive(false);

        _friendInput = F<UIInput>("Friend/Add/Input");

        _whisperObj = F("Friend/Add");
        _friendsObj = F("Friend/List");
        _whisperObj.SetActive(false);
        _friendsObj.SetActive(false);

		if (!FastOpenManager.Instance.CheckFunctionIsOpen (FunctionName.Friend,false)) {
			_friend = F ("FunctionList/TipPanel/Grid/F0");
			_friend.SetActive(false);
		}
    }
    private void OnEnable()
    {
        Gate.instance.registerMediator(new TalkMediator(this));
    }
    private void OnDisable()
    {
        Gate.instance.removeMediator(MediatorName.TALK_MEDIATOR);
    }

    //清空消息
    public void ClearInput()
    {
        _input.value = "";
    }

    //获取输入消息
    public string GetInputValue()
    {
        return _input.value;
    }

    //显示标签
    public void Display(Table table)
    {
        switch (table)
        {
            case Table.None:
                break;
            case Table.Table1: DisplayAll();
                break;
            case Table.Table2: DisplayWorld();
                break;
            case Table.Table3: DisplayGrid();
                break;
            case Table.Table4: DisplayWhisper();
                break;
            case Table.Table5: DisplaySystem();
                break;
            default:
                break;
        }
    }

    //更新当前频道最新获得的消息
    public void UpdateCurTable(TalkVo vo)
    {
        switch (TalkManager.Instance.SelectTable)
        {
            case Table.None:
                break;
            case Table.Table1:
                switch (vo.Type)
                {
                    case TalkType.Error:
                        _textList.Add(TalkManager.FormatSystem(vo));
                        break;
                    case TalkType.World:
                        _textList.Add(TalkManager.FormatWorldText(vo));
                        break;
                    case TalkType.Guild:
                        _textList.Add(TalkManager.FormatGrildText(vo));
                        break;
                    case TalkType.Whisper:
                        _textList.Add(TalkManager.FormatWhisper(vo));
                        break;
                    case TalkType.System:
                        _textList.Add(TalkManager.FormatSystem(vo));
                        break;
                    case TalkType.Post:
                        break;
                    case TalkType.SystemAndPost:
                        _textList.Add(TalkManager.FormatSystem(vo));
                        break;
                    default:
                        break;
                }
                break;
            case Table.Table2:
                switch (vo.Type)
                {
                    case TalkType.Error:
                        _textList.Add(TalkManager.FormatSystem(vo));
                        break;
                    case TalkType.World:
                        _textList.Add(TalkManager.FormatWorldText(vo));
                        break;
                    default:
                        break;
                }
                break;
            case Table.Table3:
                switch (vo.Type)
                {
                    case TalkType.Error:
                        _textList.Add(TalkManager.FormatSystem(vo));
                        break;
                    case TalkType.Guild:
                        _textList.Add(TalkManager.FormatGrildText(vo));
                        break;
                    default:
                        break;
                }
                break;
            case Table.Table4:
                switch (vo.Type)
                {
                    case TalkType.Error:
                        _textList.Add(TalkManager.FormatSystem(vo));
                        break;
                    case TalkType.Whisper:
                        _textList.Add(TalkManager.FormatWhisper(vo));
                        break;
                    default:
                        break;
                }
                break;
            case Table.Table5:
                break;
            default:
                break;
        }
    }

    //显示频道列表
    public void DisplayChannelList(bool isActive)
    {
        _channelList.SetActive(isActive);
    }

    //显示当前的频道名称
    public void DisplayCurChannel()
    {
        switch (TalkManager.Instance.SendType)
        {
            case TalkType.World: _curSendChannel.text = TalkManager.WORLD_CHANNEL;
                break;
            case TalkType.Guild: _curSendChannel.text = TalkManager.GRILD_CHANNEL;
                break;
            case TalkType.Whisper:
                if (string.IsNullOrEmpty(TalkManager.Instance.WhisperName))
                {
                    _curSendChannel.text = TalkManager.WHISPER_CHANNEL;
                }
                else {
                    _curSendChannel.text = TalkManager.FormatWhipserName(TalkManager.Instance.WhisperName);
                }
                break;
            case TalkType.System:
                break;
            case TalkType.Post:
                break;
            case TalkType.SystemAndPost:
                break;
            default:
                break;
        }
    }

    //显示URL玩家对应功能列表
    public void DisplayPlayerTip(bool isActive)
    {
        _playerTipObj.SetActive(isActive);
    }

    //获取URL链接
    public string GetURLLinkString()
    {
        return _inputLabel.GetUrlAtPosition(UICamera.lastHit.point);
    }

    //获取私聊对象值
    public string GetFriendInputValue()
    {
        return _friendInput.value;
    }
    //设置私聊对象input值
    public void SetFriendInputValue(string value)
    {
        _friendInput.value = value;
    }
    //显示输入私聊对象面板
    public void DisplayWhisperFunc(bool isActive)
    {
        _whisperObj.SetActive(isActive);
    }
    //显示好友列表
    public void DisplayFriendList(bool isActive)
    {
        if (_friendsObj.activeSelf && isActive)
        {
            _friendsObj.SetActive(false);
        }
        else {
            _friendsObj.SetActive(isActive);
        }
    }

    private void AddText(TalkVo vo)
    {
        switch (vo.Type)
        {
            case TalkType.World:
                _textList.Add(TalkManager.FormatWorldText(vo));
                break;
            case TalkType.Guild:
                _textList.Add(TalkManager.FormatGrildText(vo));
                break;
            case TalkType.Whisper:
                _textList.Add(TalkManager.FormatWhisper(vo));
                break;
            case TalkType.System:
                _textList.Add(TalkManager.FormatSystem(vo));
                break;
            case TalkType.Post:
                break;
            case TalkType.SystemAndPost:
                _textList.Add(TalkManager.FormatSystem(vo));
                break;
            default:
                break;
        }
    }

    private void DisplaySystem()
    {
        _contentObj.SetActive(false);
        _settingObj.SetActive(true);
    }

    private void DisplayAll()
    {
        _contentObj.SetActive(true);
        _settingObj.SetActive(false);
        _textList.Clear();

        foreach (TalkVo vo in TalkManager.Instance.Contents)
        {
            switch (vo.Type)
            {
                case TalkType.Error:
                    _textList.Add(TalkManager.FormatSystem(vo));
                    break;
                case TalkType.World:
                    _textList.Add(TalkManager.FormatWorldText(vo));
                    break;
                case TalkType.Guild:
                    _textList.Add(TalkManager.FormatGrildText(vo));
                    break;
                case TalkType.Whisper:
                    _textList.Add(TalkManager.FormatWhisper(vo));
                    break;
                case TalkType.System:
                    _textList.Add(TalkManager.FormatSystem(vo));
                    break;
                case TalkType.Post:
                    break;
                case TalkType.SystemAndPost:
                    _textList.Add(TalkManager.FormatSystem(vo));
                    break;
                default:
                    break;
            }
        }
    }

    private void DisplayWorld()
    { 
        _contentObj.SetActive(true);
        _settingObj.SetActive(false);
        _textList.Clear();

        foreach (TalkVo vo in TalkManager.Instance.Contents)
        {
            switch (vo.Type)
            {
                case TalkType.Error:
                    _textList.Add(TalkManager.FormatSystem(vo));
                    break;
                case TalkType.World:
                    _textList.Add(TalkManager.FormatWorldText(vo));
                    break;
                default:
                    break;
            }
        }
    }

    private void DisplayGrid()
    { 
        _contentObj.SetActive(true);
        _settingObj.SetActive(false);
        _textList.Clear();

        foreach (TalkVo vo in TalkManager.Instance.Contents)
        {
            switch (vo.Type)
            {
                case TalkType.Error:
                    _textList.Add(TalkManager.FormatSystem(vo));
                    break;
                case TalkType.Guild:
                    _textList.Add(TalkManager.FormatGrildText(vo));
                    break;
                default:
                    break;
            }
        }
    }

    private void DisplayWhisper()
    { 
        _contentObj.SetActive(true);
        _settingObj.SetActive(false);
        _textList.Clear();

        foreach (TalkVo vo in TalkManager.Instance.Contents)
        {
            switch (vo.Type)
            {
                case TalkType.Error:
                    _textList.Add(TalkManager.FormatSystem(vo));
                    break;
                case TalkType.Whisper:
                    _textList.Add(TalkManager.FormatWhisper(vo));
                    break;
                default:
                    break;
            }
        }
    }




    #region F
    private T F<T>(string path) where T:Component
    {
        return transform.FindChild(path).GetComponent<T>();
    }

    private GameObject F(string path)
    {
        return transform.FindChild(path).gameObject;
    }
    #endregion
}
