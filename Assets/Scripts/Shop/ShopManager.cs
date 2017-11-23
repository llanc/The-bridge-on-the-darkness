using UnityEngine;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// 商城管理器
/// </summary>
public class ShopManager : MonoBehaviour {

    private ShopItemUI m_ShopItemUI;
    private UIManager m_UIManager;
    private CubeManager m_CubeManager;
    public XmlData Xmldata;

    //private string xmlPath = "Assets/Datas/ShopData.xml";//商城物品路径
    private string xmlPath;

    //private string savePath = "Assets/Datas/SaveData.xml";//玩家数据路径

    private string savePath;//= Application.persistentDataPath + @"/SaveData.xml";
    private string content = "<SaveData><GemCount>0</GemCount><HeightScore>0</HeightScore><ID0>0</ID0><ID1>0</ID1><ID2>0</ID2><ID3>0</ID3><ID4>0</ID4><ID5>0</ID5><ID6>0</ID6><ID7>0</ID7><ID8>0</ID8><ID9>0</ID9><ID10>0</ID10><ID11>0</ID11><ID12>0</ID12><ID13>0</ID13><ID14>0</ID14><ID15>0</ID15><ID16>0</ID16></SaveData>";


    //商品UI集合
    private List<GameObject> shopUI = new List<GameObject>();
    //展现的商品的索引
    private int index = 0;

    //
    private GameObject m_ShopUI;
    //商城商品模板
    private GameObject ui_ShopItem;
    //两个按钮
    private GameObject m_LeftNext;
    private GameObject m_RightNext;

    //宝石数
    private UILabel m_GemNumber;
    public UILabel GemNumber
    {
        get
        {
            return m_GemNumber;
        }

        set
        {
            m_GemNumber = value;
        }
    }

    private AudioSource m_NextAudio;//商城左右退出声音


    void Update()
    {
        Xmldata.ReadGemAndScoreAndStateByPath(savePath);//读取xml
        UpdateGemNumber();
    }
    void Start () {
        xmlPath = Resources.Load("ShopData").ToString();
        savePath = Application.persistentDataPath + @"/SaveData.xml";
        if (!File.Exists(savePath))
        {
            File.WriteAllText(savePath, content);
            Debug.Log("shopmanager创建" + xmlPath + savePath);
        }
        Debug.Log("shopmanager"+ xmlPath + savePath);
        m_ShopUI = GameObject.Find("Shop_UI");
        m_UIManager = GameObject.Find("UI Root").GetComponent<UIManager>();
        m_CubeManager = GameObject.Find("CubeManager").GetComponent<CubeManager>();
        GemNumber = GameObject.Find("GemNumber").GetComponent<UILabel>();


        m_LeftNext = GameObject.Find("LeftNext");
        m_RightNext = GameObject.Find("RightNext");
        m_NextAudio = GameObject.Find("LeftNext").GetComponent<AudioSource>();//获取声音

        UIEventListener.Get(m_LeftNext).onClick = LeftNextButtonClick;
        UIEventListener.Get(m_RightNext).onClick = RightNextButtonClick;

        ui_ShopItem = Resources.Load<GameObject>("UI/ShopItem");//加载资源
        m_ShopItemUI = ui_ShopItem.GetComponent<ShopItemUI>();//获取脚本
        Xmldata = new XmlData();//实例化商品对象
        Xmldata.ReadXmlPath(xmlPath);
        Xmldata.ReadGemAndScoreAndStateByPath(savePath);//读取xml

        UpdateGemNumber();

        CreateShopUI();//创建商品模板
        //创建默认物体
        
        //SetPlayerInfo(Xmldata.shopList[0].Model);

    }

    public void UpdateGemNumber()
    {
        GemNumber.text = Xmldata.GemCount.ToString();
    }
    /// <summary>
    /// 创建商城中所有的商品
    /// </summary>
    private void CreateShopUI()
    {
        for (int i = 0; i < Xmldata.shopList.Count; i++)
        {
            //Debug.Log("创建商品创建商品创建商品创建商品");
            GameObject item= NGUITools.AddChild(gameObject, ui_ShopItem);//创建商品UI
            GameObject cube = Resources.Load<GameObject>(Xmldata.shopList[i].Model);//创建cube
            item.GetComponent<ShopItemUI>().SetUIValue(Xmldata.shopList[i].Id,Xmldata.shopList[i].Price,cube,Xmldata.shopState[i]);//给商品赋值 
            shopUI.Add(item);//把生成的元素添加到集合中
        }
        ShopUIHeidOrShow(index);
    }
    private void ShopUIHeidOrShow(int index)
    {
        //全部设置为隐藏
        for (int i = 0; i < shopUI.Count; i++)
        {
            shopUI[i].SetActive(false);
        }
        shopUI[index].SetActive(true);
    }
    //左右按钮
    private void LeftNextButtonClick(GameObject go)
    {
        if (index>0)
        {
            index--;
            ShopUIHeidOrShow(index);
            m_NextAudio.Play();
        }
    }
    private void RightNextButtonClick(GameObject go)
    {
        if (index<shopUI.Count-1)
        {
            index++;
            ShopUIHeidOrShow(index);
            m_NextAudio.Play();
        }
    }
    /// <summary>
    /// 判断可购买性
    /// </summary>
    /// <param name="Item"></param>
    private void CalaPrice(ShopItemUI ShopItemUI)
    {
            
        if (Xmldata.GemCount >=int.Parse(Xmldata.shopList[index].Price))
        {
            BroadcastMessage("BuyUIActiveAndAudio");//向该游戏物体及其子物体发送名字为的BuyUIActiveAndAudio消息
            Xmldata.GemCount -= int.Parse(Xmldata.shopList[index].Price);
            //更新UI
            GemNumber.text = Xmldata.GemCount.ToString();
            m_UIManager.GemLabel.text= Xmldata.GemCount.ToString();
            //保存金币数据
            Xmldata.UpdateXMLData(savePath, "GemCount", GemNumber.text);
            //保存商品状态                      save中的
            Xmldata.UpdateXMLData(savePath, "ID" + ShopItemUI.ItemId, "1");
        }
        else
        {           
            m_UIManager.HintUI.SetActive(true);//显示提示
            m_ShopUI.SetActive(false); 
        }
    }
    /// <summary>
    /// 商品开始按钮
    /// </summary>
    private void PlayNow()
    {
        m_CubeManager.DistroyPlayer();
        SetPlayerInfo(Xmldata.shopList[index].Model);
        m_UIManager.PlayByThis();
    }
    /// <summary>
    /// 存储当前角色到PlayerPrefs中
    /// </summary>
    public void SetPlayerInfo(string name)
    {
        PlayerPrefs.SetString("PlayerName", name);
        m_CubeManager.CreatePlayer();
    }
}
