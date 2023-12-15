using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healerCircle : MonoBehaviour
{
    float damage = 30f;
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Object is Triggering with: " + other.gameObject.name);
        if(other.gameObject.name.Substring(0, Mathf.Min(7, other.gameObject.name.Length)) == "Enemy -")
        {
            other.gameObject.GetComponent<EnemyBehavior>().setHealth(-damage);
        }
        if(other.gameObject.name.Substring(0, Mathf.Min(7, other.gameObject.name.Length)) == "EnemyV2")
        {
            other.gameObject.GetComponent<EnemyModes>().setHealth(-damage);
        }
    }
}
