using manager;
using model;
public class DataReadShop : DataReadBase
{


    public override string getRootNodeName()
    {
        return "RECORDS";

    }


    public override void appendAttribute(int key, string name, string value)
    {
        ShopVo vo;
        if (ShopManager.Instance.ShopHash.ContainsKey(key))
        {
            vo = ShopManager.Instance.ShopHash[key] as ShopVo;
        }
        else {
            vo = new ShopVo();
            ShopManager.Instance.ShopHash.Add(key,vo);
        }

        switch (name)
        {
            case "ID":
                vo.Id = int.Parse(value);
                break;
            case "name":
                vo.Name = value;
                break;
            case "sort":
                vo.Table = (SellShopType)int.Parse(value);
                break;
            case "itemNum":
                vo.DisplayId = int.Parse(value);
                break;
            case "itemID":
                vo.ItemId = int.Parse(value);
                break;
            case "getItemNum":
                vo.GetItemCount = int.Parse(value);
                break;
            case "getDiamondNum":
                vo.GetDiamonCount = int.Parse(value);
                break;
            case "ResourcePrice":
                break;
            case "RMBPrice":
                vo.RmbPrice = int.Parse(value);
                break;
            case "region":
                vo.Region = int.Parse(value);
                break;
            case "huodongType":
                vo.SellState = (ShopStateType)int.Parse(value);
                break;
            case "discountIcon":
                vo.StateIcon = value;
                break;
            case "discount":
                vo.StateDescription = value;
                break;
            default:
                break;
        }
    }
}
