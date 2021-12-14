using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public GameObject wb;
    public Camera cam;
    public Transform launchStartPos;
    public float launchStrength;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShootWebBall()
    {
        GameObject webBall = GameObject.Instantiate(wb, launchStartPos);
        webBall.transform.parent = this.gameObject.transform;
        WebBall wbScript = webBall.GetComponent<WebBall>();
        wbScript.cam = cam;
        wbScript.launchStrength = launchStrength;
    }
}
