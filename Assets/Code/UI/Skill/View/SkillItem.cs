using UnityEngine;
using System.Collections;
using manager;

public class SkillItem : MonoBehaviour {

    public bool Islock { get; set; }
    private UITexture sp_icon;
    private UILabel lbl_name;
    private UILabel lbl_level;
    private UILabel lbl_description;
    private UILabel lbl_Desc;
    private UIToggle chk;
    private void Awake()
    {
        Islock = false;
        sp_icon = transform.FindChild("icon").GetComponent<UITexture>();
        lbl_name = transform.FindChild("name").GetComponent<UILabel>();
        lbl_level = transform.FindChild("level").GetComponent<UILabel>();
        lbl_description = transform.FindChild("description").GetComponent<UILabel>();
        lbl_Desc = transform.FindChild("goldicon").GetComponent<UILabel>();
        chk = transform.GetComponent<UIToggle>();
    }
    public void Display(string icon, string name, string lvl, string des,bool _islock)
    {
        this.Islock = _islock;
        
        if (_islock)
        {
            
            icon = "skill_lock";
        }
        lbl_Desc.enabled = Islock;
        sp_icon.mainTexture = SourceManager.Instance.getTextByIconName(icon, PathConst.SKILL_PATH);
        lbl_name.text = name;
        lbl_level.text = lvl;
        lbl_description.text = des;
    }
}
