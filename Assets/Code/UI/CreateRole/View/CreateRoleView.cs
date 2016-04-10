using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using mediator;
using System.Text;
using helper;

public class CreateRoleView : MonoBehaviour {

    const int MAX_CHAT = 12;
    private UIInput _name;
    private TweenRotation rotation;     //筛子旋转特效
    private UILabel _error;

    private Animation _warrior;
    private Animation _anchor;
    private Animation _magic;

    private GameObject checkBox;
    private GameObject _descWarrior;
    private GameObject _descAnchor;
    private GameObject _descMagic;

    CHARACTER_CAREER previousState;
    private void Awake()
    {
        previousState = CHARACTER_CAREER.CC_SWORD;
        _warrior = GameObject.Find("jianshi_pose").GetComponent<Animation>();
        _anchor = GameObject.Find("longnv_pose").GetComponent<Animation>();
        _magic = GameObject.Find("jingling_pose").GetComponent<Animation>();
        _warrior.gameObject.SetActive(true);
        _anchor.gameObject.SetActive(false);
        _magic.gameObject.SetActive(false);

        _error = transform.FindChild("name_function/Error").GetComponent<UILabel>();
        _name = transform.FindChild("name_function/Name_Input").GetComponent<UIInput>();
        rotation = transform.FindChild("name_function/Button_Roll/background").GetComponent<TweenRotation>();

        checkBox = transform.FindChild("name_function/CheckBoxs").gameObject;
        _descWarrior = transform.FindChild("name_function/Desc/Warrior").gameObject;
        _descAnchor = transform.FindChild("name_function/Desc/Ancher").gameObject;
        _descMagic = transform.FindChild("name_function/Desc/Magic").gameObject;

        transform.FindChild("name_function/background/Label").GetComponent<UILabel>().text =
            LanguageManager.GetText("create_role_length_control");
    }
    private void Start()
    {
        //默认选择战士
        Gate.instance.sendNotification(MsgConstant.MSG_CREATEROLE_SELECT_CAREER, CHARACTER_CAREER.CC_SWORD);
        Gate.instance.sendNotification(MsgConstant.MSG_CREATEROLE_ROLL_NAME);
    }
    private void OnEnable()
    {
        Gate.instance.registerMediator(new CreateRoleMediator(this));
    }

    private void OnDisable()
    {
        Gate.instance.removeMediator(MediatorName.CREATEROLE_MEDIATOR);
    }


    public void DisplayName(string name)
    {
        _name.GetComponentInChildren<UILabel>().text = name;
    }

    //提交名字
    public void Submit()
    {
        string name = _name.value;
        int tempCount = 0;
        char[] cs = name.ToCharArray();
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < cs.Length; i++)
        {
            if ((cs[i] >> 7) == 0x00)
            {
                tempCount++;
            }
            else {
                tempCount += 2;
            }
            if (tempCount<=MAX_CHAT)
            {
                sb.Append(cs[i]);
            }
        }
        _name.value = sb.ToString();
		_name.GetComponentInChildren<UILabel>().text = sb.ToString();
        Gate.instance.sendNotification(MsgConstant.MSG_CREATEROLE_SETNAME, sb.ToString());
    }

    public string GetInput()
    {
        return _name.GetComponentInChildren<UILabel>().text;
    }


    public void RotationRoll()
    {
        if (!rotation.enabled)
        {
            rotation.enabled = true;
            rotation.ResetToBeginning();
        }
        
    }

    private void CreateRolePlay(CHARACTER_CAREER cc)
    {
        switch (cc)
        {
            case CHARACTER_CAREER.CC_ARCHER:
                transform.parent.FindChild("createRoleAnim").position = checkBox.transform.FindChild("Ancher").position;
                break;
            case CHARACTER_CAREER.CC_MAGICIAN:
                transform.parent.FindChild("createRoleAnim").position = checkBox.transform.FindChild("Magic").position;
                break;
            case CHARACTER_CAREER.CC_SWORD:
                transform.parent.FindChild("createRoleAnim").position = checkBox.transform.FindChild("Warrior").position;
                break;
            default:
                break;
        }
    }

    public void DisplayAnimation(CHARACTER_CAREER cc)
    {
        _descWarrior.SetActive(false);
        _descAnchor.SetActive(false);
        _descMagic.SetActive(false);
        switch (cc)
        {
            case CHARACTER_CAREER.CC_BEGIN:
                break;
            case CHARACTER_CAREER.CC_SWORD:
                if (previousState != CHARACTER_CAREER.CC_SWORD)
                {
                    _warrior.gameObject.SetActive(true);
                    _anchor.gameObject.SetActive(false);
                    _magic.gameObject.SetActive(false);
                }
                if (!_warrior.animation.IsPlaying("pose"))
                {
                    _warrior.Play("pose");
                }
                _warrior.PlayQueued("wait");
                _descWarrior.SetActive(true);
                CreateRolePlay(CHARACTER_CAREER.CC_SWORD);
                previousState = CHARACTER_CAREER.CC_SWORD;
                break;
            case CHARACTER_CAREER.CC_ARCHER:
                if (previousState != CHARACTER_CAREER.CC_ARCHER)
                {
                    _warrior.gameObject.SetActive(false);
                    _anchor.gameObject.SetActive(true);
                    _magic.gameObject.SetActive(false);
                }
                if (!_anchor.animation.IsPlaying("pose"))
                    _anchor.Play("pose");
                _anchor.PlayQueued("wait");
                _descAnchor.SetActive(true);
                CreateRolePlay(CHARACTER_CAREER.CC_ARCHER);
                previousState = CHARACTER_CAREER.CC_ARCHER;
                break;
            case CHARACTER_CAREER.CC_MAGICIAN: 
                if (previousState != CHARACTER_CAREER.CC_MAGICIAN)
                {
                    _warrior.gameObject.SetActive(false);
                    _anchor.gameObject.SetActive(false);
                    _magic.gameObject.SetActive(true);
                }
                if (!_magic.animation.IsPlaying("pose"))
                    _magic.Play("pose");
                _magic.PlayQueued("wait");
                _descMagic.SetActive(true);
                CreateRolePlay(CHARACTER_CAREER.CC_MAGICIAN);
                previousState = CHARACTER_CAREER.CC_MAGICIAN;
                break;
            case CHARACTER_CAREER.CC_END:
                break;
            default:
                break;
        }
    }

    public void DisplayError(string error)
    {
        _error.text = error;
    }

    

}
