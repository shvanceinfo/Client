using UnityEngine;
using System.Collections;

public class LuckStoneDisplayItem : MonoBehaviour {

    public int Id { get; set; }
    private UILabel _name;
    private UILabel _goldNum;
    private UILabel _itemNum;
    private void Awake()
    {
        _name = transform.FindChild("name").GetComponent<UILabel>();
        _goldNum = transform.FindChild("diamonNum").GetComponent<UILabel>();
        _itemNum = transform.FindChild("fushiNum").GetComponent<UILabel>();
    }

    public void Display(string name, string dimonCount, string itemCount)
    {
        _name.text = name;
        _goldNum.text = dimonCount;
        _itemNum.text = itemCount;
    }
}
