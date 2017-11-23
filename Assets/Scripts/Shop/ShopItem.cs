/// <summary>
/// 商城物品Item实体类
/// </summary>
public class ShopItem{
    private string model;
    private string price;
    private string id;

    public string Model
    {
        get
        {
            return model;
        }

        set
        {
            model = value;
        }
    }

    public string Price
    {
        get
        {
            return price;
        }

        set
        {
            price = value;
        }
    }

    public string Id
    {
        get
        {
            return id;
        }

        set
        {
            id = value;
        }
    }

    public ShopItem(string id,string model,string price)
    {
        this.id = id;
        this.model = model;
        this.price = price;
    }
}
