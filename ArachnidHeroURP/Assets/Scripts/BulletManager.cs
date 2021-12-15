using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public GameObject wb;   //web ball prefab
    public Camera cam;      //player camera
    public Transform launchStartPos;    //starting position of the web ball
    public float launchStrength;        //launch strength of the web ball

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Method instantiates a web ball to shoot at enemies
    public void ShootWebBall()
    {
        print("ballshot");
        GameObject webBall = GameObject.Instantiate(wb, launchStartPos);
        webBall.transform.parent = this.gameObject.transform;
        WebBall wbScript = webBall.GetComponent<WebBall>();
        wbScript.cam = cam;
        wbScript.launchStrength = launchStrength;
    }
}
