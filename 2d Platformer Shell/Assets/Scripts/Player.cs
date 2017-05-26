using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    //from Sebastian Lague youtube. 

    Controller2D controller;

    public float jumpHeight = 4;
    public float timeToJumpApex = .4f;
    Vector3 velocity;
    float gravity;
    float jumpVelocity;
    float velocityXSmoothing;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    public float velocityXSmoothTime = .1f;
    public float moveSpeed = 10f;

    public bool inAir = false;


    // Use this for initialization
    void Start()
    {
        controller = GetComponent<Controller2D>();
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        print("Gravity: " + gravity + " Jump Velocity: " + jumpVelocity);
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetButtonDown("Jump") && controller.collisions.below)
        {
            velocity.y = jumpVelocity;
        }

        //        float hMove = Input.GetAxisRaw("Horizontal");
        //      bool jumping = Input.GetButton("Jump");

        //        if (jumping && !inAir)
        //      {
        //      inAir = true;
        //    velocity.y += jumpHeight;
        //    }
        //        if (hMove != 0f)
        //      {
        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }
}
