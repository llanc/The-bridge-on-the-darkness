using System.Xml;
using System.Collections.Generic;

public class XmlData{

    public List<ShopItem> shopList = new List<ShopItem>();//存放Shop物品Item实体
    public List<int> shopState = new List<int>();//商品的购买状态
    //region两个分数
    public int GemCount=0;
    public int HeightScore=0;

    /// <summary>
    /// 读取XML
    /// </summary>
    public void ReadXmlPath( string path)
    {
        XmlDocument doc = new XmlDocument();
        //doc.Load(path);
        doc.LoadXml(path);
        XmlNode root = doc.SelectSingleNode("Shop");
        XmlNodeList nodeList = root.ChildNodes;
        foreach (XmlNode node in nodeList)
        {
            string model = node.ChildNodes[0].InnerText;
            string price = node.ChildNodes[1].InnerText;
            string id = node.ChildNodes[2].InnerText;

            ShopItem item = new ShopItem(id,model, price);//实例化xml中的数据
            shopList.Add(item);//把实例出来的对象存储进shopList集合中
        }
    }
    /// <summary>
    /// 读取金币分数和状态
    /// </summary>
    /// <param name="path"></param>
    public void ReadGemAndScoreAndStateByPath(string path)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(path);
        XmlNode root = doc.SelectSingleNode("SaveData");
        XmlNodeList nodeList =root.ChildNodes;
        GemCount = int.Parse(nodeList[0].InnerText);
        HeightScore = int.Parse(nodeList[1].InnerText);
        for (int i = 2 ; i < 19; i++)
        {
            shopState.Add(int.Parse(nodeList[i].InnerText));
        }
    }
    /// <summary>
    /// 更新xml
    /// </summary>
    /// <param name="path"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void UpdateXMLData(string path,string key,string value)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(path);
        XmlNode root = doc.SelectSingleNode("SaveData");
        XmlNodeList nodeList = root.ChildNodes;
        foreach (XmlNode node in nodeList)
        {
            if (node.Name==key)
            {
                node.InnerText = value;
                doc.Save(path);
            }
        }
    }
}
