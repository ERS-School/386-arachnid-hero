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
        //if the left mouse button is clicked, shoot a web ball
        if (Input.GetMouseButtonDown(0))
        {
            bMan.ShootWebBall();
        }
    }
}
