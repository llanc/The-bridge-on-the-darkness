using UnityEngine;
using System.Collections;
/// <summary>
/// 摄像机移动
/// </summary>
public class CameraFollow : MonoBehaviour {

    private Transform m_Transform;
    private Transform m_Player;
    
    private Vector3 normalPos;

    private bool startFollow=false;
    public bool StartFollow//属性封装字段
    {
        set { startFollow = value; }
        get { return startFollow; }
    }
    
    void Start () {
        m_Transform = gameObject.GetComponent<Transform>();
        normalPos = m_Transform.position;//保存当前摄像机初始的坐标
        m_Player = GameObject.Find("CubeManager").GetComponent<Transform>();//获取游戏物体
	}
	

	void Update () {
        CameraMove();
	}
    /// <summary>
    /// 摄像移动
    /// </summary>
    void CameraMove()
    {
        if(startFollow)
        {
            Vector3 nextPos=new Vector3(m_Transform.position.x, m_Player.position.y + 1.7f, m_Player.position.z);
            //m_Transform.position = nextPos;
            m_Transform.position = Vector3.Lerp(m_Transform.position, nextPos, Time.deltaTime);//差值运算（原来的值，后来的值，时间）
        }
    }

    public void ResetCamera()
    {
        m_Transform.position = normalPos;
    }

}
