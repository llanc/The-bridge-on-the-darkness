using UnityEngine;
using System.Collections;
using System.IO;

/// <summary>
/// 角色控制
/// </summary>
public class PlayerController : MonoBehaviour
{
    
    private int x = 3;
    private int z = 4;//两个角标
    public int Z
    {
        set { z = value; }
        get { return z; }
    }

    private bool life=true;
    public bool Life
    {
        set { life = value; }
        get { return life; }
    }

    private bool start = false;
    public bool S_tart
    {
        set { start = value; }
        get { return start; }
    }

    static bool pd = true;

    private int gemCount = 0;//临时宝石数
    public int G_emCount { set { gemCount = value; } get { return gemCount; } }
    private int GemCount = 0;//永久宝石数
    public int GemCount1
    {
        get
        {
            return GemCount;
        }

        set
        {
            GemCount = value;
        }
    }
    private int scroeCount = 0;
    public int ScoreCount
    {
        set { scroeCount = value; }
        get { return scroeCount; }
    }


    private Color ColorOne = new Color(74 / 255f, 173 / 255f, 118 / 255f);
    private Color ColorTwo = new Color(203 / 255f, 77 / 255f, 207 / 255f);

    //private  Transform[] ziwuti;
    private Transform m_Transform;//用于存储当前对象
    private MapManager m_MapManager;//用于Mapmanager的获取
    private CameraFollow m_CameraFollow;//获取CameraFollow
    private UIManager m_UIManager;
    private ShopManager m_ShopManager;
    //声音
    private AudioSource m_PlayerAudio;
    //加载资源
    private AudioClip AudioBoom;
    private AudioClip AudioEat;

    public XmlData xmldata;
    //public string Savepath = "Assets/Datas/SaveData.xml";

    private string Savepath;//=Application.persistentDataPath+ @"/SaveData.xml";
    private string content ="<SaveData><GemCount>0</GemCount><HeightScore>0</HeightScore><ID0>0</ID0><ID1>0</ID1><ID2>0</ID2><ID3>0</ID3><ID4>0</ID4><ID5>0</ID5><ID6>0</ID6><ID7>0</ID7><ID8>0</ID8><ID9>0</ID9><ID10>0</ID10><ID11>0</ID11><ID12>0</ID12><ID13>0</ID13><ID14>0</ID14><ID15>0</ID15><ID16>0</ID16></SaveData>";
    void Start()
    {
        Savepath = Application.persistentDataPath + @"/SaveData.xml";
        if (!File.Exists(Savepath))
        {
            File.WriteAllText(Savepath,content);
            Debug.Log("Playerconller创建" + Savepath);
        }
        Debug.Log("Playerconller"+Savepath);
        m_Transform = gameObject.GetComponent<Transform>();
        //Transform[] ziwuti = m_Transform.GetComponentsInChildren<Transform>();//查找当前物体的自由体
        m_MapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
        m_CameraFollow = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
        m_UIManager = GameObject.Find("UI Root").GetComponent<UIManager>();
        m_ShopManager = GameObject.Find("Shop_UI").GetComponent<ShopManager>();
        m_PlayerAudio = m_Transform.GetComponent<AudioSource>();//查找声音组件
        AudioBoom = Resources.Load("Boom") as AudioClip;
        AudioEat = Resources.Load("Eat") as AudioClip;

        xmldata = new XmlData();
        //xmldata.ReadGemAndScoreAndStateByPath(Savepath);//从Xml读取数据
        
        //GemCount = PlayerPrefs.GetInt("gem", 0);//读取数据
    }

    void Update()
    {
        xmldata.ReadGemAndScoreAndStateByPath(Savepath);
        GemCount1 = xmldata.GemCount;
        if (life&&start)
        {
            PlayerControl();
        }
    }

    public void StartGame()
    {
        SetPlayerPos();
        m_CameraFollow.StartFollow = true;    //对startFollow写操作
        m_MapManager.StartTileDown();//开始塌陷
    }
    /// <summary>
    /// 左移
    /// </summary>
    public void Left()
    {  
        if(x!=0)
        {
            z++;
            AddscoreCount();//增加分数
        }  
        if (z % 2 == 1&&x!=0)//z++之后z是奇数
            {
                x--;
            }
            SetPlayerPos();//创建物体并且渲染
            CalcPosition();//计算位置是否生成新地图    
    }
    /// <summary>
    /// 右移
    /// </summary>
    public void Right()
    {
        
            if (x != 4 || z % 2 == 0)
            {
                z++;
                AddscoreCount();//增加分数
            }
            if (z % 2 == 0 && x != 4)//z++之后为偶数
            {
                x++;
                AddscoreCount();
            }
            SetPlayerPos();//创建物体并且渲染
            CalcPosition();//计算位置是否生成新地图
        
    }
    /// <summary>
    /// 前跳
    /// </summary>
    public void Forward()
    {
        if(pd)
        {
            StartCoroutine(Timer(3));//倒计时3s 
            pd = false;
        }
        z += 4;
        AddscoreCount();
        AddscoreCount();
        SetPlayerPos();
        CalcPosition();//计算位置
    }
    /// <summary>
    /// 前跳倒计时
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public IEnumerator Timer(float time)
    {
        while (time > 0)
        {
            yield return new WaitForSeconds(1);
            time--;
        }
        if (gemCount >0)
        {
            gemCount -= 3;
            m_UIManager.TF();
            pd = true;
        }
    }
    /// <summary>
    /// 无敌
    /// </summary>
    public void LongLife()
    {
        if(life)
        {
            GameObject.Destroy(m_Transform.GetComponent<BoxCollider>());//移除BoxCollider
            gemCount -= 6;
            m_UIManager.TF();
            gemCount += 6;
            StartCoroutine(AddBoxCollider(5));
        }
    }
    //5秒后无敌取消
    private IEnumerator AddBoxCollider(float time)
    {
        while(time>0)
        {
            yield return new WaitForSeconds(1);
            time--;
        }
        if(gemCount>=0)
        {
            m_Transform.gameObject.AddComponent<BoxCollider>().isTrigger=true;
            m_Transform.GetComponent<BoxCollider>().size = new Vector3(0.254f, 0.254f, 0.254f);
            if(gemCount>0)
            {
                gemCount -= 6;
            }
            m_UIManager.TF();
            m_UIManager.m_Time.SetActive(false);
        }            
    }
    /// <summary>
    /// 随机颜色的RGB值。即刻得到一个随机的颜色  
    /// </summary>
    /// <returns></returns>
    private Color RandomColor()
    {
        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 1f);
        Color color = new Color(r, g, b);
        return color;
    }
    /// <summary>
    /// 左右前创建物体和蜗牛痕迹
    /// </summary>
    private void SetPlayerPos()
    {
        Transform playerPos = m_MapManager.mapList[z][x].GetComponent<Transform>();//获取位置的Transform组件
        MeshRenderer GetMeshRenderer = null;

        m_Transform.position = playerPos.position + new Vector3(0, 0.254f / 2, 0);//将要生成的对象的位置设置为3 4 并且偏移0.254f
        m_Transform.rotation = Quaternion.Euler(new Vector3(0, 135, 0));// playerPos.rotation;//将要生成的对象旋转
        //判断查找不同tag标签的组件
        if (playerPos.tag == "Tile")
        {
            GetMeshRenderer= playerPos.FindChild("normal_a2").GetComponent<MeshRenderer>();//获取当前对象下子物体的渲染组建
        }
        if (playerPos.tag == "Spikes")
        {
            GetMeshRenderer = playerPos.FindChild("moving_spikes_a2").GetComponent<MeshRenderer>();//获取但前对象下子物体的渲染组建
        }
        if (playerPos.tag == "Sky_Spikes")
        {
            GetMeshRenderer = playerPos.FindChild("smashing_spikes_a2").GetComponent<MeshRenderer>();//获取但前对象下子物体的渲染组建
        }
        //判断，当不为空时进行渲染
        if(GetMeshRenderer!=null)
        {
            if (z % 2 == 0)
            {
                GetMeshRenderer.material.color = RandomColor();//ColorOne ;
            }
            else
            {
                GetMeshRenderer.material.color = RandomColor(); // ColorTwo;
            }
        }
        else
        {
            
            m_PlayerAudio.Play();//播放声音
            gameObject.AddComponent<Rigidbody>();
            StartCoroutine("GameOver", false);
            m_MapManager.StopTileDown();//停止地面塌陷
        }
    }

    /// <summary>
    ///计算位置（角色在地图上的位置）并且生成新地图
    /// </summary>
    private void CalcPosition()
    {
        if(m_MapManager.mapList.Count-z<=22)
        {
            m_MapManager.AddPR();//增加概率
            //  局部变量                    【数组长度-1】【0】这个位置的物体位置的z轴数值                                    增加半个对角线
            float offsetZ = m_MapManager.mapList[m_MapManager.mapList.Count - 1][0].GetComponent<Transform>().position.z + m_MapManager.bottomLength / 2;
            m_MapManager.CreateMapItem(offsetZ);
            
        }
    }
    /// <summary>
    /// 激活触发事件
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        Vector3 Pos = m_MapManager.mapList[m_MapManager.mapList.Count - 1][0].GetComponent<Transform>().position;//物体位置
        if (other.tag=="Spikes_ci")
        {
            AudioSource.PlayClipAtPoint(AudioBoom, Pos);//在固定位置播放音频
            StartCoroutine("GameOver", false);
            m_MapManager.StopTileDown();
        }
        if (other.tag == "Sky_Spikes_ci")
        {
            AudioSource.PlayClipAtPoint(AudioBoom, Pos);//在固定位置播放音频
            StartCoroutine("GameOver",false);
            m_MapManager.StopTileDown();
        }
        if(other.tag=="Gem")
        {
            AudioSource.PlayClipAtPoint(AudioEat, Pos);//在固定位置播放
            GameObject.Destroy(other.gameObject.GetComponent<Transform>().parent.gameObject);
            AddGemCount();
        }
    }
    /// <summary>
    /// 增加加奖励物品的分数
    /// </summary>
    private void AddGemCount()
    {
        gemCount++;//临时
        GemCount1++;
        xmldata.UpdateXMLData(Savepath, "GemCount", GemCount1.ToString());
        m_UIManager.UpdateData(scroeCount, gemCount,GemCount1);
        m_UIManager.TF();
    }
    /// <summary>
    /// 分数增加
    /// </summary>
    private void AddscoreCount()
    {
        scroeCount++;
        //xmldata.UpdateXMLData(Savepath, "HeightScore", scroeCount.ToString());
        m_UIManager.UpdateData(scroeCount,gemCount,GemCount1);
    }
    /// <summary>
    /// 保存数据
    /// </summary>
    private void SaveData()
    {
        xmldata.UpdateXMLData(Savepath, "GemCount", GemCount1.ToString());
        //PlayerPrefs.SetInt("gem",GemCount);
        if (scroeCount> xmldata.HeightScore /*PlayerPrefs.GetInt("score",0)*/)
        {
            xmldata.UpdateXMLData(Savepath, "HeightScore", scroeCount.ToString());
            //PlayerPrefs.SetInt("score", scroeCount);
        }
    }

    /// <summary>
    /// 游戏结束携程（用协成的原因是为了让物体下后停止）
    /// </summary>
    private IEnumerator GameOver(bool b)
    {
        if(b)
        {
            yield return new WaitForSeconds(0.5f);
        }
        if(life)
        {
            life = false;
            m_UIManager.TF();
            m_CameraFollow.StartFollow = false;//摄像机停止跟随
            SaveData();
            //UI
            StartCoroutine("ResetGame");
        }
    }
    /// <summary>
    /// 重置游戏对象
    /// </summary>
    private void ResetPlayer()
    {
        GameObject.Destroy(gameObject.GetComponent<Rigidbody>());
        x = 3;
        z = 4;
        life = true;
        scroeCount = 0;
        gemCount = 0;
        start = false;
        m_UIManager.TF();
        pd = true;
        
    }
    /// <summary>
    /// 重置游戏
    /// </summary>
    private IEnumerator ResetGame()
    {
        m_UIManager.EndScore();//显示分数
        yield return new WaitForSeconds(2.0f);
        m_UIManager.ResetUI();//重置UI
        ResetPlayer();//重置物体
        m_MapManager.ResetGameMap();//重置地图
        m_CameraFollow.ResetCamera();//重置摄像机
    }










    /// <summary>
    /// 角色移动
    /// </summary>
    private void PlayerControl()
    {
        //左移
        if (Input.GetKeyDown(KeyCode.A))
        {
            Left();
        }
        //右移
        if (Input.GetKeyDown(KeyCode.L))
        {
            Right();
        }
        //向前跳一格
        if (Input.GetKeyDown(KeyCode.K) && gemCount >= 3)
        {
            Forward();
            m_UIManager.FordWardAudio.Play();
        }
        if (Input.GetKeyDown(KeyCode.S) && gemCount >= 6)
        {
            LongLife();
            m_UIManager.LongLifeAudio.Play();
        }
    }


}
