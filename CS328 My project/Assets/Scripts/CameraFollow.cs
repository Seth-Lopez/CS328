using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        public float Follow Speed = 2f;
        public Transform tartget;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(target,position.x,target.position.y,-10f);
        transform.position = Vector3.Slerp(transform.position,newPos,FollowSpeed*Time.deltaTime);
        
    }
}
