using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpEffect: ScriptableObject
{
    public abstract void Apply(GameObject target);
}
