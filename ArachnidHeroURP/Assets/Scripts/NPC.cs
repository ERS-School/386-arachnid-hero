using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    float health;
    public float Health { get { return health; } }
    // Start is called before the first frame update
    void Start()
    {
        health = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(float dmgToTake)
    {
        //if it loses all its health, die
        health -= dmgToTake;
        if (health <= 0)
        {
            Die();
        }
        else
        {
            print("Ouch!");
        }
    }

    public void Die()
    {
        Destroy(this.gameObject);
        print("ARGHHH");
    }
}
