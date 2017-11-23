using UnityEngine;
using System.Collections;
using System.Collections.Generic;//引入集合的命名空间

public class MapManager : MonoBehaviour {
    /// <summary>
    /// 地图管理器
    /// </summary>
    private GameObject m_prefab_tile;
    private GameObject m_prefab_wall;
    private GameObject m_prefab_spikes;
    private GameObject m_prefab_sky_spikes;
    private GameObject m_prefab_gem;

    //声音
    private AudioSource m_SpikesAudio;
    public AudioSource SpikesAudio
    {
        get
        {
            return m_SpikesAudio;
        }

        set
        {
            m_SpikesAudio = value;
        }
    }
    private AudioSource m_SkySpikesAudio;
    public AudioSource SkySpikesAudio
    {
        get
        {
            return m_SkySpikesAudio;
        }

        set
        {
            m_SkySpikesAudio = value;
        }
    }
    private AudioSource m_GemAudio;
    public AudioSource GemAudio
    {
        get
        {
            return m_GemAudio;
        }

        set
        {
            m_GemAudio = value;
        }
    }

    //概率
    private int pr_hole=0;
    private float  pr_spikes=0;
    private float  pr_sky_spikes = 0;
    private float pr_gem = 1.5f;

    private int index = 0;//记录器，记录当前元素所在行
    public int Index
    {
        get { return index; }
    }

    private Transform m_Transform;
    private PlayerController m_PlayerController;//存储PlayerController

    public List<GameObject[]> mapList = new List<GameObject[]>();//定义一个集合（列表）存储GameObject类型的数组

    public float bottomLength = Mathf.Sqrt(2) * 0.254f;/*Sqrt(2)是平方根，整个式子为求对角线长度*/
    //存放预设的颜色值
    private static Color [ , ] ColorList ={
        {
            new Color(0, 109 / 255f, 121 / 255f),
            new Color(2 / 255f, 145 / 255f, 161 / 255f),
            new Color(2 / 255f, 76 / 255f, 115 / 255f)   
        },
        {
            new Color(148 / 255f, 214 / 255f, 107 / 255f),
            new Color(3 / 255f, 136 / 255f, 78 / 255f),
            new Color(40 / 255f, 176 / 255f, 181 / 255f)
        },
        {
            new Color(210 / 255f, 30 / 255f, 47 / 255f),
            new Color(245 / 255f, 150 / 255f, 14 / 255f),
            new Color(147 / 255f, 12 / 255f, 60 / 255f)
        },
        {
            new Color( 204/ 255f,  204/255f,  0/ 255f),
            new Color( 255/ 255f, 153 / 255f, 51/ 255f),
            new Color(102/ 255f,  51/ 255f, 135 / 255f)
        },
        {
            new Color( 77/ 255f, 180/ 255f, 180 / 255f),
            new Color( 177/ 255f,  181/ 255f,  167/ 255f),
            new Color(90 / 255f,  175/ 255f, 111/ 255f)
        },
        {
            new Color(105 / 255f,90  / 255f,  145/ 255f),
            new Color( 145/ 255f,  96/ 255f, 126 / 255f),
            new Color( 98/ 255f, 120 / 255f,  128/ 255f)
        },
        {
            new Color(255 / 255f, 221 / 255f, 0 / 255f),//大黄
            new Color(255 / 255f, 81 / 255f, 33 / 255f),//大红
            new Color( 93/ 255f,  204/ 255f,245  / 255f)//天蓝色   保存
        },
        {
            new Color( 240/ 255f,  131/ 255f,  68/ 255f),
            new Color( 100/ 255f,  200/ 255f, 60/ 255f),
            new Color( 111/ 255f,  123/ 255f,  241/ 255f)
        }   
    };
    private static int RandomI()//取随机数
    {
        return Random.Range(0, 8);
    }
    void Start () {
        //加载资源
        m_prefab_tile = Resources.Load("tile_white") as GameObject;
        m_prefab_wall = Resources.Load("wall2") as GameObject;
        m_prefab_spikes = Resources.Load("moving_spikes") as GameObject;
        m_prefab_sky_spikes = Resources.Load("smashing_spikes") as GameObject;
        m_prefab_gem = Resources.Load("gem 2") as GameObject;
        //获取组件
        m_Transform = gameObject.GetComponent<Transform>();

        m_PlayerController = GameObject.Find("CubeManager").GetComponent<PlayerController>();//获取PlayerController脚本

        //调用方法
        CreateMapItem(0);//第一次不用偏移

    }
	
    /// <summary>
    /// 创建地图元素段
    /// </summary>
    public void CreateMapItem(float offsetZ)//传入偏移量offsetZ
    {
        int RandomColor =RandomI();
        //单数排地板
        for(int i=0;i<14;i++)
        {
            GameObject[] item = new GameObject[6]; //定一个GameObgect类型的数组item用于存储生成的地板或墙壁
            for (int j = 0; j < 6; j++)
            {
                Vector3 pos = new Vector3(j*bottomLength, 0, offsetZ+i*bottomLength); //位置坐标,除第一次生成外，z需要偏移量
                Vector3 rot = new Vector3(-90, 45, 0);//旋转-90度
                //判断要生成 每排的1，5个模型时 生成wall2，2-4时生成tile
                GameObject tile = null;
                if (j == 0 || j == 5)
                {
                    tile = GameObject.Instantiate(m_prefab_wall, pos, Quaternion.Euler(rot)/*转换三维向量为4元数*/) as GameObject;//创建物体
                    tile.GetComponent<MeshRenderer>().material.color = ColorList[RandomColor, 2];// ColorThree;//父物体对象下的网格渲染组件下的材质球下的颜色属性
                }
                else
                {
                    int pr = CalcPR();//获取概率值
                    if(pr==0)//为0时创建物体
                    {
                        tile = GameObject.Instantiate(m_prefab_tile, pos, Quaternion.Euler(rot)/*转换三维向量为4元数*/) as GameObject;//创建物体
                        tile.GetComponent<MeshRenderer>().material.color = ColorList[RandomColor, 0]; // ColorOne;//父物体对象下的网格渲染组件下的材质球下的颜色属性
                        tile.GetComponent<Transform>().FindChild("normal_a2").GetComponent<MeshRenderer>().material.color = ColorList[RandomColor, 0];//ColorOne;//子物体的颜色
                        //创建宝石
                        int prgem = CalcGemPR();
                        if (prgem == 1)
                        {
                            GameObject gem= GameObject.Instantiate(m_prefab_gem, tile.GetComponent<Transform>().position + new Vector3(0, 0.06f, 0), Quaternion.identity)as GameObject;//在原来物体上方0.06f处创建宝石
                            gem.GetComponent<Transform>().SetParent(tile.GetComponent<Transform>());//设置当前物体为tile的子物体
                        }
                    }
                    else if(pr==1)//创建空洞
                    {
                        tile = new GameObject();
                        tile.GetComponent<Transform>().position=pos;//空物体的生成位置
                        tile.GetComponent<Transform>().rotation = Quaternion.Euler(rot);//旋转
                    }
                    else if(pr==2)//创建地面障碍
                    {
                        tile = GameObject.Instantiate(m_prefab_spikes, pos, Quaternion.Euler(rot)/*转换三维向量为4元数*/) as GameObject;//创建物体
                    }
                    else if (pr == 3)//创建天空障碍
                    {
                        tile = GameObject.Instantiate(m_prefab_sky_spikes, pos, Quaternion.Euler(rot)/*转换三维向量为4元数*/) as GameObject;//创建物体
                    }
                }
                tile.GetComponent<Transform>().SetParent(m_Transform);//给生成的物体设置父物体（当前组件）
                item[j] = tile;//把对象存入数组
            }
            mapList.Add(item);//把数组存入列表
            //双数排地板
            GameObject[] item2 = new GameObject[5]; //定义另外一个GameObgect类型的数组item用于存储生成的地板或墙壁
            for (int j = 0; j < 5; j++)//!!!
            {
                Vector3 pos = new Vector3(j * bottomLength + bottomLength / 2, 0,offsetZ+ i * bottomLength + bottomLength / 2); //位置向右向上移动半个bottomLength
                Vector3 rot = new Vector3(-90, 45, 0);//旋转-90度
                GameObject tile = null;
                int pr = CalcPR();//获取概率值
                if (pr == 0)//为0时创建物体
                {
                    tile = GameObject.Instantiate(m_prefab_tile, pos, Quaternion.Euler(rot)/*转换三维向量为4元数*/) as GameObject;//创建物体
                    tile.GetComponent<MeshRenderer>().material.color = ColorList[RandomColor, 1];// ColorTwo;//父物体对象下的网格渲染组件下的材质球下的颜色属性
                    tile.GetComponent<Transform>().FindChild("normal_a2").GetComponent<MeshRenderer>().material.color = ColorList[RandomColor, 1];// ColorTwo;//子物体的颜色
                    //创建宝石
                    int prgem = CalcGemPR();
                    if (prgem == 1)
                    {
                        GameObject gem = GameObject.Instantiate(m_prefab_gem, tile.GetComponent<Transform>().position + new Vector3(0, 0.06f, 0), Quaternion.identity) as GameObject;//在原来物体上方0.06f处创建宝石
                        gem.GetComponent<Transform>().SetParent(tile.GetComponent<Transform>());//设置当前物体为tile的子物体
                    }
                }
                else if (pr == 1)//创建空洞
                {
                    tile = new GameObject();
                    tile.GetComponent<Transform>().position = pos;//空物体的生成位置
                    tile.GetComponent<Transform>().rotation = Quaternion.Euler(rot);//旋转
                }
                else if (pr == 2)//创建地面障碍
                {
                    tile = GameObject.Instantiate(m_prefab_spikes, pos, Quaternion.Euler(rot)/*转换三维向量为4元数*/) as GameObject;//创建物体
                }
                else if (pr == 3)//创建天空障碍
                {
                    tile = GameObject.Instantiate(m_prefab_sky_spikes, pos, Quaternion.Euler(rot)/*转换三维向量为4元数*/) as GameObject;//创建物体
                }
                tile.GetComponent<Transform>().SetParent(m_Transform);//给生成的物体设置父物体（当前组件）
                item2[j]= tile;//把对象存入数组
            }
            mapList.Add(item2);//把数组存入列表
        }
    }

    /// <summary>
    /// 开始协程塌陷
    /// </summary>
    public void StartTileDown()
    {
        StartCoroutine("TileDown");
    }
    /// <summary>
    /// 关闭协程塌陷
    /// </summary>
    public void StopTileDown()
    {
        StopCoroutine("TileDown");
    }

    /// <summary>
    /// 地面塌陷
    /// </summary>
    private IEnumerator TileDown()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);//等待几秒
            for (int i = 0; i <mapList[index].Length;i++ )
            {
                Rigidbody rd= mapList[index][i].AddComponent<Rigidbody>();//添加刚体组件,会返回一个Rigidbody的引用
                rd.angularVelocity = new Vector3(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)) * Random.Range(1, 10);//添加一个角力，随机出力的方向并且乘以力的大小
                GameObject.Destroy(mapList[index][i], 1.0f);
            }
            if(index==m_PlayerController.Z)
            {
                StopTileDown();//停止下落
                Rigidbody rd = m_PlayerController.gameObject.AddComponent<Rigidbody>();
                m_PlayerController.StartCoroutine("GameOver", false);
            }
            index++;     
        }
    }
    /// <summary>
    /// 计算宝石生成的概率
    /// </summary>
    /// <returns>1：生成，0：不生成</returns>
    private int CalcGemPR()
    {
        if (Random.Range(1, 100) < pr_gem)
        {
            return 1;
        }
        else
            return 0;
    }
    /// <summary>
    /// 计算概率值，并返回
    /// 0：瓷砖
    /// 1：坑洞
    /// 2：地面
    /// 3：天空
    /// </summary>
    private int CalcPR()
    {
        int pr = Random.Range(1, 100);
        if (pr <= pr_hole)
        {
            return 1;
        }
        else if (31 < pr && pr < pr_spikes + 30)  //范围为31到30
        {
            return 2;
        }
        else if (61 < pr && pr < pr_sky_spikes + 60)//范围61到90
        {
            return 3;
        }
        return 0;
    }

    /// <summary>
    /// 增加概率
    /// </summary>
    /// 
    public void AddPR()
    {
        pr_hole += 1;
        pr_spikes += 1.8f;
        pr_sky_spikes += 1.4f;
        pr_gem += 2;//要减小
    }
    void Update () {
    }
    /// <summary>
    /// 重置地图
    /// </summary>
    public void ResetGameMap()
    {
        Transform[] sonTransform = m_Transform.GetComponentsInChildren<Transform>();//获取当前组件下的所有子物体，返回一个数组
        for (int i=1;i<sonTransform.Length;i++)//i=0是包含父物体，遍历该数组 全部销毁
        {
            GameObject.Destroy(sonTransform[i].gameObject); 
        }
        pr_hole = 0;
        pr_spikes = 0;
        pr_sky_spikes = 0;
        pr_gem = 2;

        index = 0;
        mapList.Clear();//清空列表集合

        CreateMapItem(0);//生成地图
    }
}
