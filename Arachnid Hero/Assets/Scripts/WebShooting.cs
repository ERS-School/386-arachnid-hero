using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebShooting : MonoBehaviour
{
    // Start is called before the first frame update
    public BulletManager bMan;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            bMan.ShootWebBall();
        }
    }
}
