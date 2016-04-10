using UnityEngine;
using System.Collections;
using helper;

public class DemonRankItem : MonoBehaviour {


    const string rank1 = "emodongku_paihang1";
    const string rank2 = "emodongku_paihang2";
    const string rank3 = "emodongku_paihang3";

    const string rank1Color = "fefd00";
    const string rank2Color = "fe9300";
    const string rank3Color = "ef5700";
    const string rankOther = "d8d0a7";
    const string coutChar = "x";
    const string gate = "层";
    public int Id { get; set; }
    private UISprite _spIcon;
    private UILabel _lblRank;
    private UILabel _lblName;
    private UILabel _lblLevel;
    private UISprite _spResultIcon;
    private UILabel _lblNum;
    private UISprite _spCarrer;
    private void Awake()
    {
        _spIcon = transform.FindChild("icon").GetComponent<UISprite>();
        _spResultIcon = transform.FindChild("item").GetComponent<UISprite>();
        _lblRank = transform.FindChild("rank").GetComponent<UILabel>();
        _lblName = transform.FindChild("name").GetComponent<UILabel>();
        _lblLevel = transform.FindChild("level").GetComponent<UILabel>();
        _lblNum = transform.FindChild("num").GetComponent<UILabel>();
        _spCarrer = transform.FindChild("career").GetComponent<UISprite>();
    }


    public void Display(int id,int rank,string carrer,string name,string level,string itemIcon,int num)
    {
        if (id==1)
        {
            _spIcon.spriteName = rank1;
            _spIcon.alpha = 1f;
            _lblRank.text = ColorConst.Format(rank1Color, rank);
            _lblName.text = ColorConst.Format(rank1Color, name);
            _lblLevel.text = ColorConst.Format(rank1Color, level + gate); 
        }
        else if (id == 2)
        {
            _spIcon.spriteName = rank2;
            _spIcon.alpha = 1f;
            _lblRank.text = ColorConst.Format(rank2Color, rank);
            _lblName.text = ColorConst.Format(rank2Color, name);
            _lblLevel.text = ColorConst.Format(rank2Color, level + gate); 
        }
        else if (id == 3)
        {
            _spIcon.spriteName = rank3;
            _spIcon.alpha = 1f;
            _lblRank.text = ColorConst.Format(rank3Color, rank);
            _lblName.text = ColorConst.Format(rank3Color, name);
            _lblLevel.text = ColorConst.Format(rank3Color, level + gate); 
        }
        else {
            _spIcon.alpha = 0f;
            _lblRank.text = ColorConst.Format(rankOther, rank);
            _lblName.text = ColorConst.Format(rankOther, name);
            _lblLevel.text = ColorConst.Format(rankOther, level + gate); 
        }
        _lblNum.text = coutChar+num.ToString();

        _spCarrer.spriteName = carrer;
    }
}
