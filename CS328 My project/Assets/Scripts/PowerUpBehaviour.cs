using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBehaviour : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        GameObject target = GameObject.FindWithTag("Player");
        string nameOfObj = this.gameObject.name;
        if (target.name == other.gameObject.name)
        {
            if(nameOfObj != "PowerUpV2")
            {
                target.GetComponent<PlayerScript>().SetSpellSelected(1);
                Destroy(gameObject);
            }
            else
            {
                target.GetComponent<PlayerScript>().setHealth(-20);
                Destroy(gameObject);
            }
        }
    }
}
