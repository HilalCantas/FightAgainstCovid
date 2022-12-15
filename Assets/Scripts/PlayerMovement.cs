using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{ 
    public float runSpeed = 6f;
    public float jumpSpeed = 5f;
    public float climbSpeed = 3f;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;
    public Vector2 deathKick = new Vector2 (20f,20f); 
    float gravityScaleAtStart;
    Vector2 moveInput;
    Rigidbody2D rb;
    Animator myAnimator;
    CapsuleCollider2D myCapsuleCollider;
    BoxCollider2D myFeetCollider;

     

    bool isAlive = true;
 
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = rb.gravityScale;
    }

  
    void Update()
    {

        if(!isAlive)
        {
            return;
        }

        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    void OnFire(InputValue value)
    {
        if(!isAlive)
        {
            return;
        }

        Instantiate(bullet, gun.position, transform.rotation);
  

    }

    void OnMove(InputValue value)
    {

        
        if(!isAlive)
        {
            return;
        }

        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }

    void OnJump(InputValue value)
    {

        
        if(!isAlive)
        {
            return;
        }

        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) 
        {
            return;
        }

        if(value.isPressed)
        {
            rb.velocity += new Vector2 (0f, jumpSpeed);
        }

    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, rb.velocity.y);
        rb.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
              transform.localScale =  new Vector2 (Mathf.Sign(rb.velocity.x), 1f);

        }
    }

    void ClimbLadder()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))) 
        {
            rb.gravityScale = gravityScaleAtStart;
             myAnimator.SetBool("isClimbing", false);
            return;
        }

        Vector2 climbVelocity = new Vector2 (rb.velocity.x , moveInput.y * climbSpeed);
        rb.velocity = climbVelocity;
        rb.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(rb.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);



    }

    void Die() 
    {
        if(myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards", "Water")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            rb.velocity = deathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
           
           
        }     
        
    }

}
