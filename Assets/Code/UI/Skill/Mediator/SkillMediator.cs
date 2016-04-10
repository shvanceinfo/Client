using System.Collections;
using MVC.entrance.gate;
using MVC.interfaces;
using System.Collections.Generic;
using manager;


namespace mediator
{
    public class SkillMediator : ViewMediator
    {

        public SkillMediator(SkillView view ,uint id=MediatorName.SKILL_MEDIATOR):base(id,view)
        { 
            
        }
        
        public override IList<uint> listReferNotification()
        {
            return new List<uint> 
            { 

            MsgConstant.MSG_SKILL_ASK_TALENTDATA,
            MsgConstant.MSG_SKILL_INITIAL_TALENTTABLE,
            MsgConstant.MSG_SKILL_LEVEL_TALENT,
            MsgConstant.MSG_SKILL_ASK_SKILL_LIST,
            MsgConstant.MSG_SKILL_INITIAL_SKILL_LIST,
            MsgConstant.MSG_SKILL_LEVEL_SKILL,
            MsgConstant.MSG_SKILL_TABLE_SWITCHING,
            MsgConstant.MSG_SKILL_GET_DATA,
            MsgConstant.MSG_SKILL_UI_LOAD,
            MsgConstant.MSG_SKILL_LEVEL_SKILL_COMPLATE,
            MsgConstant.MSG_SKILL_LEVEL_TALENT_COMPLATE,
            MsgConstant.MSG_SKILL_UNLOCK_SKILL,
            MsgConstant.MSG_SKILL_ERROE_MSG,
            MsgConstant.MSG_SKILL_CALLBACK_DISPLAY_INFO,
            MsgConstant.MSG_SKILL_EFFECT_INFO,
            MsgConstant.MSG_SKILL_EFFECT_TALENTINFO
            };
        }

        public override void handleNotification(INotification notification)
        {
            if (view!=null)
            {
                switch (notification.notifyId)
                {
                        //切换界面
                    case MsgConstant.MSG_SKILL_TABLE_SWITCHING:
                        int id = (int)notification.body;
                        view.TableID = id;
                        if (id == 1)
                        {
                            view.OnTableToSkill();
                        }
                        else if(id==2){
                            view.OnTableToTalent();
                        }
                        
                        break;
                        //刷新技能UI界面
                    case MsgConstant.MSG_SKILL_INITIAL_SKILL_LIST:
                        if (view.TableID==1)
                        {
                            view.RerushSkillUI();
                        }
                        break;
                        //刷新天赋界面
                    case MsgConstant.MSG_SKILL_INITIAL_TALENTTABLE:
                        if (view.TableID==2)
                        {
                            view.RerushTalentUI();
                        }
                        break;
                        //技能升级
                    case MsgConstant.MSG_SKILL_LEVEL_SKILL:
                        SkillTalentManager.Instance.SendLevelSkill((int)notification.body);
                        break;
                        //解锁技能
                    case MsgConstant.MSG_SKILL_UNLOCK_SKILL:
                        SkillTalentManager.Instance.UnLockSkill((int)notification.body);
                        break;

                        //升级天赋
                    case MsgConstant.MSG_SKILL_LEVEL_TALENT:

                        SkillTalentManager.Instance.SendLevelTalent((int)notification.body);
                        break;

                    case MsgConstant.MSG_SKILL_EFFECT_TALENTINFO:
                        view.DisplayEffectTalent((int)notification.body);
                        break;
                        //发送错误信息
                    case MsgConstant.MSG_SKILL_ERROE_MSG:
                        view.ShowErrMsg((string)notification.body);
                        break;
                    case MsgConstant.MSG_SKILL_CALLBACK_DISPLAY_INFO:
                        view.DisplayInfo();
                        break;
                    case MsgConstant.MSG_SKILL_EFFECT_INFO:
                        view.DisplayEffect();
                        break;
                }
            }
        }

        



        public SkillView view
        {
            get {
                if (base._viewComponent!=null&&base._viewComponent is SkillView)
                {
                    return base._viewComponent as SkillView;
                }
                return null;
            }
        }
    }
}

