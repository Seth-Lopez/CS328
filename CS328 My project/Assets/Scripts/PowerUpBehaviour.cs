using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBehaviour : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Object is colliding with: " + other.gameObject.name);
        if(other.gameObject.name == "Player") 
        {
            GameObject target = other.gameObject;
            if(target != null)
                target.GetComponent<PlayerScript>().SetSpellSelected(1);
            Destroy(gameObject);
        }
    }
}
