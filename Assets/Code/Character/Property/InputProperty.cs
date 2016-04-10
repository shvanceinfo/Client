using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputProperty : MonoBehaviour
{
    public Vector2 m_kJoyStickPt = Vector2.zero;

    public bool m_bInJoyStickStatus = false;

    public Character m_kOwner = null;

    // 摇杆用来脱离战斗
    public bool m_bNeedLeaveFight = false;


    public Vector3 m_kXuanFengZhanDir = Vector3.zero;

    // 摇杆移动中算出来人物应该的朝向
    public Quaternion m_kPlayerNeedRot = Quaternion.identity;

    void Awake()
    {
        m_kOwner = GetComponent<Character>();
    }

    // Use this for initialization
    void Start()
    {

    }

    void OnEnable()
    {
        EasyJoystick.On_JoystickMoveStart += On_JoystickMoveStart;
        EasyJoystick.On_JoystickMove += On_JoystickMove;
        EasyJoystick.On_JoystickMoveEnd += On_JoystickMoveEnd;
    }

    void OnDisable()
    {
        EasyJoystick.On_JoystickMoveStart -= On_JoystickMoveStart;
        EasyJoystick.On_JoystickMove -= On_JoystickMove;
        EasyJoystick.On_JoystickMoveEnd -= On_JoystickMoveEnd;
    }

    void OnDestroy()
    {
        EasyJoystick.On_JoystickMoveStart -= On_JoystickMoveStart;
        EasyJoystick.On_JoystickMove -= On_JoystickMove;
        EasyJoystick.On_JoystickMoveEnd -= On_JoystickMoveEnd;
    }

    public void On_JoystickMoveStart(MovingJoystick joystick)
    {
        m_kJoyStickPt = Vector2.zero;

        m_bInJoyStickStatus = true;

        if (m_kOwner.GetAI().GetCharacterState() == CharacterAI.CHARACTER_STATE.CS_ATTACK ||
            m_kOwner.GetAI().GetCharacterState() == CharacterAI.CHARACTER_STATE.CS_SKILL)
        {
            m_bNeedLeaveFight = true;
        }

        m_kOwner.GetComponent<FightProperty>().m_bEnterFight = false;

        m_kPlayerNeedRot = Quaternion.identity;
    }

    public void On_JoystickMove(MovingJoystick joystick)
    {
        m_kJoyStickPt = joystick.joystickAxis;

        m_bInJoyStickStatus = true;
    }

    public void On_JoystickMoveEnd(MovingJoystick joystick)
    {
        ResetData();
    }

    public void ResetData()
    {
        m_kJoyStickPt = Vector2.zero;

        m_bInJoyStickStatus = false;

        m_kPlayerNeedRot = Quaternion.identity;

        m_kXuanFengZhanDir = Vector3.zero;
    }

    void Update()
    {
        if (!EasyTouchJoyStickProperty.IsJoyStickEnable() || 
            CharacterAI.IsInState(m_kOwner, CharacterAI.CHARACTER_STATE.CS_DIE) || 
            Global.m_bCameraCruise ||
            MainLogic.sMainLogic.isGameSuspended())
        {
            // joystick is not enable 一般在切场景的时候
            return;
        }


        if (m_kJoyStickPt != Vector2.zero)
        {
            if (m_kOwner.GetAI().GetCharacterState() == CharacterAI.CHARACTER_STATE.CS_PURSUE && Global.inCityMap())
            {
                // 主城寻路需要被摇杆打断
                //m_kOwner.GetAI().m_kPursueState.StopPursur(m_kOwner.GetAI());
                CharacterPlayer.sPlayerMe.GetAI().SendStateMessage(CharacterAI.CHARACTER_STATE.CS_IDLE);
                return;
            }


            Vector3 kMovePos = CharacterPlayer.sPlayerMe.getPosition() +
                CharacterPlayer.sPlayerMe.transform.forward * 4.0f * Time.deltaTime;

            Vector3 kToDir = new Vector3(m_kJoyStickPt.x, 0.0f, m_kJoyStickPt.y);
            kToDir.Normalize();

            // 摄像机偏移
            Vector3 kOffset = Quaternion.Euler(0, 225, 0) * kToDir;
            kOffset.Normalize();

            m_kXuanFengZhanDir = kOffset;

            Vector3 to = Quaternion.LookRotation(kOffset).eulerAngles;


            m_kPlayerNeedRot = Quaternion.Slerp(CharacterPlayer.sPlayerMe.transform.rotation,
                Quaternion.Euler(to), Time.deltaTime * 10);

            if (m_kOwner.GetAI().GetCharacterState() == CharacterAI.CHARACTER_STATE.CS_IDLE ||
                m_kOwner.GetAI().GetCharacterState() == CharacterAI.CHARACTER_STATE.CS_MOVE ||
                m_kOwner.GetAI().GetCharacterState() == CharacterAI.CHARACTER_STATE.CS_PURSUE)
            {
                CharacterPlayer.sPlayerMe.GetAI().SendStateMessage(CharacterAI.CHARACTER_STATE.CS_MOVE, kMovePos);
                CharacterPlayer.sPlayerMe.transform.rotation = m_kPlayerNeedRot;
                return;    
            }
        }
    }
}
