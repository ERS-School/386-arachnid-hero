using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebBall : MonoBehaviour
{
    private float strength;
    public Rigidbody rb;
    public Camera cam;
    private Vector3 fwd;
    public float launchStrength;

    // Start is called before the first frame update
    void Awake()
    {
        strength = 25;
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        gameObject.transform.position = cam.transform.position;
        fwd = cam.transform.forward;
        Launch();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            NPC enemyScript = other.GetComponent<NPC>();
            enemyScript.Damage(strength);
            Destroy(this.gameObject);
        }
        else if (other.gameObject.layer == 6) //"Grappleable"
        {
            Destroy(this.gameObject);
        }
    }

    public void Launch()
    {
        rb.velocity = fwd * launchStrength;
    }
}
