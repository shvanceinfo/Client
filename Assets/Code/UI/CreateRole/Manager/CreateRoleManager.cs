using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using System;
using NetGame;
using helper;
using ProtoBuf;
using NetPackage;

namespace manager
{
    public class CreateRoleManager
    {
        static CreateRoleManager _instance;

        private string[] familyNames; //姓
        private string[] names; //名
        private CHARACTER_CAREER _career;
        private string _name;
        private bool _isMale;       //性别

        public CreateRoleManager()
        {
            TextAsset txt = BundleMemManager.Instance.loadResource(PathConst.RAND_NAME_PATH, typeof(TextAsset)) as TextAsset;
            string[] names = txt.text.Split('=');
            if (names.Length == 2)
            {
                char[] split = { '\r', '\n' };
                this.names = names[0].Split(split, StringSplitOptions.RemoveEmptyEntries);
                this.familyNames = names[1].Split(split, StringSplitOptions.RemoveEmptyEntries);
            }
        }

        /// <summary>
        /// 创建名字
        /// </summary>
        /// <param name="name"></param>
        public void SetName(string name)
        {
            _name = name;
            Gate.instance.sendNotification(MsgConstant.MSG_CREATEROLE_ERROR, "");
        }

        //选择职业
        public void SelectCharaterCareer(CHARACTER_CAREER c)
        {
            _career = c;
            if (_career != CHARACTER_CAREER.CC_MAGICIAN)
            {
                _isMale = false;
            }
            else {
                _isMale = true;
            }
        }

        //创建角色
        public void CreateRole(string name)
        {
            _name = _name.Trim();
            if (string.IsNullOrEmpty(_name) || _name.Equals(LanguageManager.GetText("nick_default")))
            {
                Gate.instance.sendNotification(MsgConstant.MSG_CREATEROLE_ERROR, LanguageManager.GetText("nick_default"));
            }
            else if (_name.Length > Constant.MAX_NICE_NAME_LEN)
            {
                //_name = LanguageManager.GetText("nick_outof");
                Gate.instance.sendNotification(MsgConstant.MSG_CREATEROLE_ERROR, LanguageManager.GetText("nick_outof"));
            }
            else if (LeachDirtyManager.Instance.CheckFilter(_name))
            {
                Gate.instance.sendNotification(MsgConstant.MSG_CREATEROLE_ERROR, LanguageManager.GetText("nick_filter_name"));
                ViewHelper.DisplayMessageLanguage("nick_filter_name");
            }
            else if (NetBase.GetInstance().IsConnected)
            {
                CGCSendCreateRole sendMsg = new CGCSendCreateRole();
                sendMsg._u32VocationID = (uint)_career;
                sendMsg._nickName = _name;
                sendMsg._byteVerify = false;
                if (_isMale)
                    sendMsg._byteGender = true;
                else
                    sendMsg._byteGender = false;

                NetBase.GetInstance().Send<CGCSendCreateRole>(sendMsg, (UInt16)CeC2GType.C2G_CreateRole);

                UIManager.Instance.showWaitting(true);
            }
            else 
            {
                //_name = "服务器还没有成功链接";
                //tipLable.text = LanguageManager.GetText("nick_not_connect");	
            }

            Gate.instance.sendNotification(MsgConstant.MSG_CREATEROLE_DISPLAY_NAME, _name);
        }

        //随机名字
        public void RandomName()
        {
            int familyIndex = UnityEngine.Random.Range(0, familyNames.Length);
            int nameIndex = UnityEngine.Random.Range(0, names.Length);
            _name = familyNames[familyIndex] + names[nameIndex];
            Gate.instance.sendNotification(MsgConstant.MSG_CREATEROLE_DISPLAY_NAME, _name);
            Gate.instance.sendNotification(MsgConstant.MSG_CREATEROLE_ERROR, "");
        }

        public void CreateRoleCallback(int result)
        {
            UIManager.Instance.closeWaitting();
            //TODO仅用来调试BEGIN
            if (result == 0) //创角成功
            {
                //MessageManager.Instance.sendMessageSelectRole();
                //UIManager.Instance.showWaitting(true);
                //MainLogic.hasLoadCreateScene = false; //标记为创角结束，能够加载游戏场景
            }
            //TODO仅用来调试END
            if (result == 1) //角色昵称重复
            {
                DeathManager.Instance.ShowError(LanguageManager.GetText("nick_success"));
            }
            else if (result == -99999918) //其他的服务器未知错误
            {
                //Loger.Log("创建角色服务器返回错误码" + result);
                //DeathManager.Instance.ShowError("创建角色服务器返回错误码" + result);
                ViewHelper.DisplayMessageLanguage("-99999918");
            }
            else if (result == -99999917) //其他的服务器未知错误
            {
                //Loger.Log("创建角色服务器返回错误码" + result);
                //DeathManager.Instance.ShowError("创建角色服务器返回错误码" + result);
                ViewHelper.DisplayMessageLanguage("-99999917");
            }
            else {
                DeathManager.Instance.ShowError("创建角色服务器返回错误码" + result);
            }
        }

        public static CreateRoleManager Instance
        {
            get { if (_instance == null) _instance = new CreateRoleManager(); return CreateRoleManager._instance; }
        }

    }
}
