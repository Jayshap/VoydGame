using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
    public float groundSpeed;
    public float airSpeed;
    public float jumpForce;
    public float gravity;
    public float groundedMargin;
    public List<GameObject> feet;
    public float doubleJumpResetTime;
    public float dashSpeed;


    public KeyCode jumpKey;

    public Animator animator;

    public Joystick joystick;

    [HideInInspector]
    public enum Direction
    {
        L,
        LU,
        U,
        RU,
        R
    }

    [HideInInspector]
    public Direction facing;

    private float speed;
    private bool facingRight = true;
    private Rigidbody2D rb;
    private bool groundedThisFrame;
    private bool disableInput;
    private bool doubleJump;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = groundSpeed;
        GetComponent<Rigidbody2D>().gravityScale = gravity;
        disableInput = false;
        doubleJump = true;
    }

    // fixed update for physics
    private void FixedUpdate()
    {
        float moveInputH = Input.GetAxisRaw("Horizontal");
        float moveInputJoystickH = joystick.Horizontal;

        groundedThisFrame = Grounded();

        // when grounded go back to ground speed
        if (groundedThisFrame)
        {
            speed = groundSpeed;
        }

        else
        {
            animator.SetFloat("SpeedVertical", rb.velocity.y);
        }

        // facing left then turn right
        if (!facingRight && (moveInputH > 0 || moveInputJoystickH > 0))
        {
            FlipPlayerYAxis();

            // in the air and turn, lower speed until hit ground
            if (!groundedThisFrame)
            {
                speed = airSpeed;
            }

        }

        // facing right then turn left
        else if (facingRight && (moveInputH < 0 || moveInputJoystickH < 0))
        {
            FlipPlayerYAxis();

            // in the air and turn, lower speed until hit ground
            if (!groundedThisFrame)
            {
                speed = airSpeed;
            }
        }

        if (!disableInput)
        {

            // move depending on horizontal input, getaxis KEYBOARD
           // rb.velocity = new Vector2(moveInputH * speed, rb.velocity.y);

            // move depending on horizontal input. joystick
            rb.velocity = new Vector2(joystick.Horizontal * speed, rb.velocity.y);

            animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawLine(transform.position, feet.transform.position - new Vector3(0, groundedMargin,0));

        float moveInputH = Input.GetAxisRaw("Horizontal");
        float moveInputJoystickH = joystick.Horizontal;
        float moveInputV = Input.GetAxisRaw("Vertical");
        float moveInputJoystickV = joystick.Vertical;

        groundedThisFrame = Grounded();

        animator.SetBool("Grounded", groundedThisFrame);

        if (groundedThisFrame)
        {
            disableInput = false;
        }

        if (!disableInput)
        {
            // JUMPING
            if (Input.GetKeyDown(jumpKey) || moveInputJoystickV > 0.5)
            {
                // jump
                if (groundedThisFrame)
                {
                    rb.velocity = Vector2.up * jumpForce;
                }

                // double jump
                else if (doubleJump)
                {
                    rb.velocity = Vector2.up * jumpForce;
                    doubleJump = false;

                    // reset double jump after x seconds
                    StartCoroutine(ResetDoubleJumpAfterSeconds());
                }
            }


            //    // left
            //    if (moveInputH < 0 || moveInputJoystickH < 0)
            //    {
            //        // left up
            //        if (moveInputV > 0)
            //            facing = Direction.LU;
            //        // left
            //        else
            //            facing = Direction.L;
            //    }

            //    // right
            //    else if (moveInputH > 0 || moveInputJoystickH > 0)
            //    {
            //        // right up
            //        if (moveInputV > 0  || moveInputJoystickV > 0)
            //            facing = Direction.RU;
            //        // right
            //        else
            //            facing = Direction.R;
            //    }

            //    // up
            //    else if (moveInputV > 0 || moveInputJoystickV > 0)
            //    {
            //        facing = Direction.U;
            //    }

            //    // no input, go back to Left or Right facing depending on previous
            //    else
            //    {
            //        if (facingRight)
            //            facing = Direction.R;
            //        else
            //            facing = Direction.L;

            //    }
            //}

        }
    }

    public  void OnCollisionEnter2D(Collision2D collision)
    {
        // stuck on a wall, disable input
        if (!Grounded() && collision.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            disableInput = true;
        }
    }

    bool Grounded()
    {
        // check if any of the feet points are grounded
        foreach(GameObject foot in feet)
        {
            if (Physics2D.Raycast(foot.transform.position, -Vector2.up, groundedMargin, 1 << LayerMask.NameToLayer("Ground")))
            {
                return true;
            }
        }

        return false;
    }

    void FlipPlayerYAxis()
    {
        // flip the x axis of the player
        facingRight = !facingRight;

        Vector3 flipScale = transform.localScale;
        flipScale.x *= -1;
        transform.localScale = flipScale;
    }
    IEnumerator ResetDoubleJumpAfterSeconds()
    {
        // reset the double jumpe after given seconds
        yield return new WaitForSeconds(doubleJumpResetTime);

        doubleJump = true;
    }
}
