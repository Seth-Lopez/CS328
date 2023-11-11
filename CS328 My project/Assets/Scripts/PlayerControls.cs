using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
   // variables

   public CharacterController2D controller;

   public float runspeed = 40f;

   float horizontalMove = 0f;



    // Start is called before the first frame update
    void Start()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runspeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, false)
    }
}
