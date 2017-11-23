using UnityEngine;
using System.Collections;

public class Gem : MonoBehaviour {
    private Transform m_Transform;
    private Transform m_gem;
    void Start () {
        m_Transform = gameObject.GetComponent<Transform>();
        m_gem= m_Transform.FindChild("gem 3");
	}
	
	void Update () {
        m_gem.Rotate(new Vector3(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)));//旋转
	}
}
