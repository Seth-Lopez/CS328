using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    void Awake()
    {
        // To make sure that the background music does not get cut off when the scene loads
        DontDistroyAudioOnLoad(transform.gameObject);
    }
}
