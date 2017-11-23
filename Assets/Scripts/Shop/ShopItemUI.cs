using UnityEngine;
using System.Collections;
using System;
/// <summary>
/// 商品UI控制器
/// </summary>
public class ShopItemUI : MonoBehaviour {
    private Transform m_Transform;

    private UILabel ui_price;
    //商品ID
    private string itemId;
    public string ItemId
    {
        get
        {
            return itemId;
        }

        set
        {
            itemId = value;
        }
    }

    private GameObject m_ShopUI;//商城UI

    private GameObject cubemodel;//模型的父物体

    private GameObject m_Buybutton;//购买按钮
    public GameObject m_PlayByThis;//开始按钮
    private AudioSource m_BuyAudio;

    public bool a;

    void Awake () {//先查找后再ShopManager中使用
        m_Transform = gameObject.GetComponent<Transform>();
        m_ShopUI = GameObject.Find("Shop_UI");
        ui_price = m_Transform.FindChild("Buy").GetComponent<Transform>().FindChild("Price").GetComponent<UILabel>();//获取子物体的UILable组件
        cubemodel = m_Transform.FindChild("Model").gameObject;//查找模型
        m_Buybutton = m_Transform.FindChild("Buy").gameObject;//查找购买按钮
        m_PlayByThis = m_Transform.FindChild("PlayByThis").gameObject;//查找开始按钮
        m_BuyAudio = m_Transform.FindChild("PlayByThis").GetComponent<AudioSource>();//查找声音组件
        UIEventListener.Get(m_Buybutton).onClick = m_BuybuttonClick;
        UIEventListener.Get(m_PlayByThis).onClick = m_PlayNowClick;
        m_PlayByThis.SetActive(false);//隐藏play按钮
    }
    void Update () {
	
	}
    /// <summary>
    /// 给对象赋值
    /// </summary>
    /// <param name="price">价格</param>
    /// <param name="model">模型</param>
    /// <param name="state">状态</param>
    public void SetUIValue(string id,string price,GameObject model,int state)
   {
        ui_price.text = price;//UI元素赋值
        ItemId = id;//
        GameObject cube= NGUITools.AddChild(cubemodel, model);//给父物体添加子模型
        cube.layer = 8;//设置层级为8
        Transform cube_Transform = cube.GetComponent<Transform>();//获取当前模型
        cube_Transform.localScale = new Vector3(600, 600, 600);//模型的缩放
        cube_Transform.localPosition = new Vector3(-3.000031f, -172,-123);//模型位置
        cube_Transform.localRotation= Quaternion.Euler(new Vector3(0, 60, 0));//旋转
        #region 判断是否购买
        if (state==1)
        {
            m_Buybutton.SetActive(false);
            m_PlayByThis.SetActive(true);
        }
        #endregion
    }
    /// <summary>
    /// 购买点击
    /// </summary>
    /// <param name="go"></param>
    private void m_BuybuttonClick(GameObject go)
    {
        SendMessageUpwards("CalaPrice", this);//向子物体发送消息，参数为自身这个脚本
    }
    private void m_PlayNowClick(GameObject go)
    {

        SendMessageUpwards("PlayNow");
    }
    /// <summary>
    /// 购买成功后按钮的显示与隐藏&音效&Change状态
    /// </summary>
    public void BuyUIActiveAndAudio()
    {
        m_Buybutton.SetActive(false);
        m_PlayByThis.SetActive(true);
        m_BuyAudio.Play();
    }

}
