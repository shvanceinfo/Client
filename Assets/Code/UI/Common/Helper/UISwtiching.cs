using UnityEngine;
using System.Collections;

public class UISwtiching : MonoBehaviour {

    public GameObject Tag1;
    public GameObject Tag2;
    public bool IsActive;
    private void OnClick()
    {
        IsActive = !IsActive;
        Swtich(IsActive);
    }
    private void Swtich(bool act)
    {
        Tag1.SetActive(act);
        Tag2.SetActive(!act);
    }

    public void Initial(bool b)
    {
        IsActive = b;
        Swtich(b);
    }
}
