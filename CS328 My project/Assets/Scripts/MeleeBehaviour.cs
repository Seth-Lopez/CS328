using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meleeehaviour : MonoBehaviour
{
    private float damage = 20f;
    private float damageDelay = 1f;

    void Update()
    {
        damageDelay -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Object is Triggering with: " + other.gameObject.name);
        if(other.gameObject.name == "Enemy")
        {
            GameObject target = other.gameObject;
            target.GetComponent<EnemyBehavior>().setHealth(damage);
        }
        Destroy(gameObject);
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if(damageDelay <= 0f)
        {
            damageDelay = 1f;
        }
        if(damageDelay == 1f)
        {
            Debug.Log("Object is colliding with: " + other.gameObject.name);
            Debug.Log(gameObject.name.Substring(0, Mathf.Min(5, other.gameObject.name.Length)));
            if(other.gameObject.name == "Player" && gameObject.name.Substring(0, Mathf.Min(5, other.gameObject.name.Length)) == "Weapo") 
            {
                GameObject target = other.gameObject;
                if(target != null)
                    target.GetComponent<PlayerScript>().setHealth(damage);
                //Destroy(gameObject);
            }
        }
    }
}
