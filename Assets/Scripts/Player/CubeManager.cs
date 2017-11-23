using UnityEngine;
using System.Collections;

public class CubeManager : MonoBehaviour {
    private  Transform m_Transform;
    private ShopManager m_ShopManager;
    private UIManager m_UIManager;

	void Start () {
        m_Transform = gameObject.GetComponent<Transform>();
        m_UIManager = GameObject.Find("UI Root").GetComponent<UIManager>();
        m_ShopManager = GameObject.Find("Shop_UI").GetComponent<ShopManager>();
        //创建默认物体
        // m_ShopManager.SetPlayerInfo(m_ShopManager.Xmldata.shopList[0].Model);
        PlayerPrefs.SetString("PlayerName", "CubeModelUI/cube_box");
        CreatePlayer();
    }
    /// <summary>
    /// 删除角色
    /// </summary>
    public void DistroyPlayer()
    {
        string PalyerName= PlayerPrefs.GetString("PlayerName");
        GameObject Child =m_Transform.GetChild(0).gameObject;
        Destroy(Child);
    }
    /// <summary>
    /// 添加角色
    /// </summary>
    public void CreatePlayer()
    {

        string PalyerName = PlayerPrefs.GetString("PlayerName");
        GameObject Player = Resources.Load(PalyerName) as GameObject ;
        GameObject PlayerCube = Instantiate(Player, new Vector3(5, 0, 0), Quaternion.Euler(new Vector3(0, 0, 0)))as GameObject;
        PlayerCube.layer = 1;
        GameObject.Find("CubeManager").AddChild(PlayerCube);
    }
}
