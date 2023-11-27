using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// this is just for organization purposes for the types of powerups //
[CreateAssetMenu(menuName = "Powerups/HealthBuff" )]
public class HealthBuff : PowerUpEffect
{
   public float amount;

   public override void Apply(GameObject target)
   {
// Input the health script here
    target.GetComponent<Health>().health.value += amount;
   }
}

