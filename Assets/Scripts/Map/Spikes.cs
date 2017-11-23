using UnityEngine;
using System.Collections;

public class Spikes : MonoBehaviour {
    /// <summary>
    /// 地面陷阱
    /// </summary>
    private Transform m_Transform;
    private Transform son_Transform;

    //两个位置
    private Vector3 normalPos;
    private Vector3 targetPos;

	void Start () {
        m_Transform = gameObject.GetComponent<Transform>();
        son_Transform= m_Transform.FindChild("moving_spikes_b").GetComponent<Transform>();//子物体

        normalPos = son_Transform.position;//危险位置
        targetPos = son_Transform.position + new Vector3(0, 0.15f, 0);//安全位置

        StartCoroutine("UpAndDown");
	}

    //控制上升下降
    public IEnumerator UpAndDown()
    {
        while(true)
        {
            StopCoroutine("Down");
            StartCoroutine("Up");//开始
            yield return new WaitForSeconds(1.0f);//等待
            StopCoroutine("Up");//暂停
            StartCoroutine("Down");
            yield return new WaitForSeconds(2.0f);
        }
    }
    //上升
    private IEnumerator Up()
    {
        while(true)
        {
            son_Transform.position = Vector3.Lerp(son_Transform.position, targetPos, Time.deltaTime*40);
            yield return null;
        }
    }
    //下降
    private IEnumerator Down()
    {
        while (true)
        {
            son_Transform.position = Vector3.Lerp(son_Transform.position, normalPos, Time.deltaTime*10);
            yield return null;//暂停一帧
        }
    }
}
