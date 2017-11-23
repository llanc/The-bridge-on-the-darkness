using UnityEngine;
using System.Collections;
using System;
using System.IO;
/// <summary>
/// UI管理器
/// </summary>
public class UIManager : MonoBehaviour {

    private Transform m_Transform;

    private PlayerController m_PlayerController;

    private GameObject m_StartUI;
    private GameObject m_GameUI;
    private GameObject m_BeginUI;
    private GameObject m_Quit;
    public GameObject m_ShopUI;
    private GameObject m_HintUI;
    public GameObject HintUI
    {
        get
        {
            return m_HintUI;
        }

        set
        {
            m_HintUI = value;
        }
    }
    public GameObject Quit
    {
        get
        {
            return m_Quit;
        }

        set
        {
            m_Quit = value;
        }
    }

    private GameObject m_OpenAudio;
    private GameObject m_stop;
    private GameObject m_begin;
    private GameObject m_Left;
    private GameObject m_Right;
    private GameObject m_Forward;//跳跃
    public GameObject m_F_orward
    {
        set { m_Forward = value; }
        get { return m_Forward; }
    }
    private GameObject m_LongLife;//无敌
    public GameObject m_L_ongLife
    {
        set { m_LongLife = value; }
        get {return m_LongLife; }
    }
    private GameObject m_time;//倒计时
    public GameObject m_Time
    {
        set { m_time = value; }
        get { return m_time; }
    }

    private AudioSource m_FordWardAudio;//跳跃声音
    private AudioSource m_LongLifeAudio;//无敌声音
    
    public AudioSource FordWardAudio
    {
        get
        {
            return m_FordWardAudio;
        }

        set
        {
            m_FordWardAudio = value;
        }
    }
    public AudioSource LongLifeAudio
    {
        get
        {
            return m_LongLifeAudio;
        }

        set
        {
            m_LongLifeAudio = value;
        }
    }


    private GameObject m_end;
    private GameObject m_help;//帮助
    private GameObject m_GoOn;//退出帮助
    private GameObject m_Ok;//退出
    private GameObject m_Cancel;//继续游戏
    private GameObject m_Tuichu;//退出
    private GameObject m_Continue;//继续浏览
    private GameObject m_PlayForGem;//开始游戏

    private UILabel m_ScoreLabel;
    private UILabel m_GemLabel;
    public UILabel GemLabel
    {
        get
        {
            return m_GemLabel;
        }

        set
        {
            m_GemLabel = value;
        }
    }
    private UILabel m_GameScoreLable;
    private UILabel m_GameGemLable;//临时

    private GameObject m_PlayButton;
    private GameObject m_ShopButton;//商城
    private GameObject m_FanHui;//退出商城
    

    private bool m_openaudio=true;

    private string Savepath;// = Application.persistentDataPath + @"/SaveData.xml";
    private string content = "<SaveData><GemCount>0</GemCount><HeightScore>0</HeightScore><ID0>0</ID0><ID1>0</ID1><ID2>0</ID2><ID3>0</ID3><ID4>0</ID4><ID5>0</ID5><ID6>0</ID6><ID7>0</ID7><ID8>0</ID8><ID9>0</ID9><ID10>0</ID10><ID11>0</ID11><ID12>0</ID12><ID13>0</ID13><ID14>0</ID14><ID15>0</ID15><ID16>0</ID16></SaveData>";

    void Start()
    {
        Savepath= Application.persistentDataPath + "/SaveData.xml";
        if (!File.Exists(Savepath))
        {
            File.WriteAllText(Savepath, content);
            Debug.Log("UImanagersavpath创建" + Savepath);
        }
        Debug.Log("UImanagersavpath" + Savepath);
        m_Transform = gameObject.GetComponent<Transform>();
               
        //获取引用
        m_PlayerController = GameObject.Find("CubeManager").GetComponent<PlayerController>();
        m_StartUI = GameObject.Find("Start_UI");
        m_GameUI = GameObject.Find("Game_UI");
        m_BeginUI = GameObject.Find("Begin_UI");
        Quit = GameObject.Find("Quit_UI");
        m_ShopUI = GameObject.Find("Shop_UI");
        m_HintUI = GameObject.Find("Hint_UI");
        m_end = GameObject.Find("End");

        //label组件
        m_ScoreLabel = GameObject.Find("Score_Label").GetComponent<UILabel>();
        GemLabel = GameObject.Find("Gem_Label").GetComponent<UILabel>();
        m_GameScoreLable = GameObject.Find("GameScoreLabel").GetComponent<UILabel>();
        m_GameGemLable = GameObject.Find("GameGemLabel").GetComponent<UILabel>();
        m_time = GameObject.Find("Time");
        //获取按钮组件
        m_PlayButton = GameObject.Find("Play_Btn");
        m_Left = GameObject.Find("Left");
        m_Right = GameObject.Find("Right");
        m_Forward = GameObject.Find("Forward");
        m_LongLife = GameObject.Find("LongLife");
        m_stop = GameObject.Find("Stop");
        m_begin = GameObject.Find("Begin");
        m_help = GameObject.Find("Help_Btn");
        m_ShopButton = GameObject.Find("Shop_Btn");
        m_FanHui = GameObject.Find("FanHui");      
        m_OpenAudio = GameObject.Find("OpenAudio");
        m_GoOn = GameObject.Find("GoOn");
        m_Ok = GameObject.Find("Ok");
        m_Cancel = GameObject.Find("Cancel");
        m_Tuichu = GameObject.Find("Tuichu");
        m_Continue = GameObject.Find("ContinueSee");
        m_PlayForGem = GameObject.Find("PlayForGem");
        //获取声音组件
        FordWardAudio = GameObject.Find("Forward").GetComponent<AudioSource>();
        LongLifeAudio = GameObject.Find("Time").GetComponent<AudioSource>();
        //开始
        UIEventListener.Get(m_PlayButton).onClick = PlayButtonClick;//委托实现点击效果
        //左右
        UIEventListener.Get(m_Left).onClick = LeftButtonClick;
        UIEventListener.Get(m_Right).onClick = RightButtonClick;
        //前 无敌
        UIEventListener.Get(m_Forward).onClick = ForwardButtonClick;
        UIEventListener.Get(m_LongLife).onClick = LongLifeButtonClick;
        //帮助
        UIEventListener.Get(m_help).onClick = HelpButtonClick;
        UIEventListener.Get(m_GoOn).onClick = GoOnButtonClick;
        //商城
        UIEventListener.Get(m_ShopButton).onClick = ShopButtonClick;
        UIEventListener.Get(m_FanHui).onClick = FanHuiButtonCluick;

        //声音
        UIEventListener.Get(m_OpenAudio).onClick = OpenAudioButtonClick;
        //暂停
        UIEventListener.Get(m_stop).onClick = stopButtonClick;
        UIEventListener.Get(m_begin).onClick = beginButtonClick;
        //退出，返回
        UIEventListener.Get(m_Ok).onClick = OkButtonClick;
        UIEventListener.Get(m_Cancel).onClick = CancelButtonClick;
        UIEventListener.Get(m_Tuichu).onClick = TuichuButtonClick;
        //提示
        UIEventListener.Get(m_Continue).onClick = ContinueOnClick;
        UIEventListener.Get(m_PlayForGem).onClick = PlayForGemClick;
        //初始化
        Init();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void Init()
    {
        m_PlayerController.xmldata.ReadGemAndScoreAndStateByPath(Savepath);
        m_ScoreLabel.text = m_PlayerController.xmldata.HeightScore.ToString();//PlayerPrefs.GetInt("score", 0).ToString();
        GemLabel.text = m_PlayerController.xmldata.GemCount.ToString();//PlayerPrefs.GetInt("gem", 0).ToString();
        m_GameScoreLable.text = "0";
        m_GameGemLable.text = "0/3";
        m_GameUI.SetActive(false);
        m_BeginUI.SetActive(false);
        m_Quit.SetActive(false);
        m_GoOn.SetActive(false);
        m_HintUI.SetActive(false);//隐藏提示
        m_ShopUI.SetActive(false);
    }
    /// <summary>
    /// Game新更新数据
    /// </summary>
    public void UpdateData(int score,int gem,int Gem)
    {
        GemLabel.text = Gem.ToString();
        m_GameScoreLable.text = score.ToString();
        m_GameGemLable.text = gem+"/3";

    }
    public void EndScore()
    {
        m_end.SetActive(true);
        m_end.gameObject.GetComponent<UILabel>().text = "得分\n" + m_PlayerController.ScoreCount;
    }

    /// <summary>
    /// 委托对象
    /// </summary>
    //开始
    private void PlayButtonClick(GameObject go)//点击时
    {
        TF();
        m_StartUI.SetActive(false);
        m_GameUI.SetActive(true);
        m_PlayerController.StartGame();
        m_PlayerController.S_tart = true;
        m_time.SetActive(false);
        m_end.SetActive(false);
        m_begin.SetActive(false);
    }
    /// <summary>
    /// 商城开始
    /// </summary>
    public void PlayByThis()
    {
        TF();
        m_StartUI.SetActive(false);
        m_GameUI.SetActive(true);
        m_PlayerController.StartGame();
        m_PlayerController.S_tart = true;
        m_time.SetActive(false);
        m_end.SetActive(false);
        m_begin.SetActive(false);
        m_ShopUI.SetActive(false);
    }
    ///左右
    private void LeftButtonClick(GameObject go)
    {
        if(m_PlayerController.S_tart&&m_PlayerController.Life)
        {
            m_PlayerController.Left();
               
        }
    }
    private void RightButtonClick(GameObject go)
    {
        if (m_PlayerController.S_tart && m_PlayerController.Life)
        {
            m_PlayerController.Right();
            
        }
    }
    //前
    private void ForwardButtonClick(GameObject go)
    {
        FordWardAudio.Play();//播放音乐
        m_PlayerController.Forward();
        m_GameGemLable.text = m_PlayerController.G_emCount + "/3";
    }
    //无敌
    private void LongLifeButtonClick(GameObject go)
    {
        m_time.SetActive(true);
        m_PlayerController.LongLife();
        m_GameGemLable.text = m_PlayerController.G_emCount + "/3";
        m_LongLife.SetActive(false);
        StartCoroutine(Timer(5f, m_Time.GetComponent<UILabel>(),Time.deltaTime));
    }
    //倒计时UI协成,3个参数：倒计时，要更新的UI,更新频率
    private IEnumerator Timer(float time,UILabel timeUpdate,float waitTime)
    {
        while(time >0)
        {
            yield return new WaitForSeconds(waitTime);
            time-= waitTime;
            timeUpdate.text= time.ToString();
        }
    }
    //help
    private void HelpButtonClick(GameObject go)
    {
        m_StartUI.SetActive(false);
        m_BeginUI.SetActive(true);
        m_GoOn.SetActive(true);
    }
    private void GoOnButtonClick(GameObject go)
    {
        m_StartUI.SetActive(true);
        m_BeginUI.SetActive(false);
        m_GoOn.SetActive(false);
    }
    //Shop
    private void ShopButtonClick(GameObject go)
    {
        m_StartUI.SetActive(false);
        m_ShopUI.SetActive(true);
    }
    private void FanHuiButtonCluick(GameObject go)
    {
        m_StartUI.SetActive(true);
        m_ShopUI.SetActive(false);
    }
    //继续浏览
    private void ContinueOnClick(GameObject go)
    {
        m_HintUI.SetActive(false);
        m_ShopUI.SetActive(true);
    }
    //开始游戏ForGem
    private void PlayForGemClick(GameObject go)//点击时
    {
        TF();
        m_StartUI.SetActive(false);
        m_GameUI.SetActive(true);
        m_PlayerController.StartGame();
        m_PlayerController.S_tart = true;
        m_time.SetActive(false);
        m_end.SetActive(false);
        m_begin.SetActive(false);
        m_HintUI.SetActive(false);
    }

    //OpenAudio
    private void OpenAudioButtonClick(GameObject go)
    {
        if (m_openaudio)
        {
            AudioListener.volume = 0;//类变量  volume设置音量
            m_openaudio = false;
        }
        else if (!m_openaudio)
        {
            AudioListener.volume = 1;
            m_openaudio = true;
        }
    }

    /// <summary>
    /// 暂停开始
    /// </summary>

    //暂停
    private void stopButtonClick(GameObject go)
    {
        m_stop.SetActive(false);
        Time.timeScale = 0;
        m_begin.SetActive(true);
    }
    //开始
    private void beginButtonClick(GameObject go)
    {
        m_begin.SetActive(false);
        Time.timeScale = 1;
        m_stop.SetActive(true);
    }
    //退出，继续游戏---继续游戏
    private void CancelButtonClick(GameObject go)
    {
        Time.timeScale = 1;//点击时继续
        m_Quit.SetActive(false);
        if (m_StartUI.activeSelf == false && m_GameUI.activeSelf == false&& Time.timeScale == 1)
        {
            m_StartUI.SetActive(true);
        }
        if(m_GameUI.activeSelf==true)
        {
            m_Right.SetActive(true);
            m_Left.SetActive(true);
        }
    }
    #region 退出，继续游戏---退出
    private void OkButtonClick(GameObject go)
    {
        Application.Quit();
    }
    private void TuichuButtonClick (GameObject go)
    {
        m_Quit.SetActive(true);
        m_StartUI.SetActive(false);
        Time.timeScale = 0;//点击退出时暂停
    }
    #endregion
    /// <summary>
    /// 技能开启判断
    /// </summary>
    public void TF()
    {
        if(m_PlayerController.G_emCount>=3&&m_PlayerController.Life)
        {
            m_Forward.SetActive(true); 
        }
        else
            m_Forward.SetActive(false);
        if (m_PlayerController.G_emCount >= 6 && m_PlayerController.Life)
        {
            m_LongLife.SetActive(true);
        }
        else
            m_LongLife.SetActive(false);
    }

    /// <summary>
    /// 重置UI
    /// </summary>
    public void ResetUI()
    {
        m_PlayerController.xmldata.ReadGemAndScoreAndStateByPath(Savepath);
        m_ScoreLabel.text = m_PlayerController.xmldata.HeightScore.ToString();// PlayerPrefs.GetInt("score", 0).ToString();
       // m_GemLabel.text = m_PlayerController.xmldata.GemCount.ToString();
        m_StartUI.SetActive(true);
        m_GameUI.SetActive(false);
        m_GameScoreLable.text = "0";
        m_GameGemLable.text = "0/3";
        TF();
    }
     
    /// <summary>
    /// 判断静音
    /// </summary>
    private void OpenAudio()
    {
        if (m_openaudio)
        {
            m_OpenAudio.GetComponent<UISprite>().spriteName = "btn_mute_off";
        }
        else if (!m_openaudio)
        {
            m_OpenAudio.GetComponent<UISprite>().spriteName = "btn_mute_on";
        }
    }
    
    void Update () {
        if (Application.platform == RuntimePlatform.Android && (Input.GetKeyDown(KeyCode.Escape)))
        {
            m_Quit.SetActive(true);
            m_StartUI.SetActive(false);
            m_Left.SetActive(false);
            m_Right.SetActive(false);
            Time.timeScale = 0;//点击退出时暂停
        }
        OpenAudio();//静音图标
    }
}

