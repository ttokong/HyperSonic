using UnityEngine;
using System.Collections;

public class player : MonoBehaviour {

	private Rigidbody2D myRigidbody;
	private Animator animator;
	private CircleCollider2D collider;
	public float speed = 5f;
	bool isCrouching = false;
	bool isRunning = false;
	bool isJumping = false;
	public Vector2 jumpHeight = new Vector2(0, 15);
	public float jumpTimer = 1;
	public float attackTimer = 0;
	bool attacking = false;

	// Use this for initialization
	void Start () {
		myRigidbody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		collider = GetComponent<CircleCollider2D>();
	}

	void FixedUpdate () {
		if(jumpTimer < 1){
			jumpTimer += 0.02f;
		} else if(isJumping){
			isJumping = false;
		}

		if(attackTimer > 0){
			attackTimer -= 0.02f;
		} else if(attacking){
			attacking = false;
		}
	}

	// Update is called once per frame
	void Update () {
		float horizontal = Input.GetAxis("Horizontal");
		if(!isJumping && !attacking){
			if(Input.GetButton("Crouch") && horizontal != 0){
				Debug.Log("Crouching");
				collider.offset = new Vector2(0f, 0.23f);
				animator.SetBool("isCrouching", true);
				isCrouching = true;
				speed = 2.5f;
			} else if(Input.GetButton("Run") && horizontal != 0){
				collider.offset = new Vector2(0f, 0f);
				isCrouching = false;
				animator.SetBool("isCrouching", false);
				animator.SetBool("isRunning", true);
				isRunning = true;
				speed = 7.5f;
			} else {
				collider.offset = new Vector2(0f, 0f);
				animator.SetBool("isCrouching", false);
				animator.SetBool("isRunning", false);
				isCrouching = false;
				isRunning = false;
				speed = 5f;
			}
		}

		HandleMovement(horizontal);
	}

	private void HandleMovement(float h){

		if(h != 0f && !isCrouching && !isRunning && !isJumping && !attacking)
		{
			animator.SetBool("isWalking", true);
		} 
		else 
		{
			animator.SetBool("isWalking", false);
		}

		if (GetComponent<Rigidbody2D>().velocity.x > 0) 
        {
            transform.localScale = new Vector2(1, transform.localScale.y);            
        }
        else if (GetComponent<Rigidbody2D>().velocity.x < 0) 
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
        }

        if (Input.GetButtonDown("Jump") && jumpTimer >= 1 && !attacking)
    	{
        	myRigidbody.AddForce(jumpHeight, ForceMode2D.Impulse);
        	jumpTimer = 0f;
        	isJumping = true;
        	animator.SetTrigger("jump");
    	}

    	if(Input.GetButtonDown("attack") && !attacking && !isJumping){
    		animator.SetTrigger("attack");
    		attacking = true;
			attackTimer = 1f;

    	}

    	if(Input.GetButtonDown("airattack") && !attacking && !isJumping){
    		animator.SetTrigger("airattack");
    		attacking = true;
			attackTimer = 1f;
    	}
    	if(!attacking){
			myRigidbody.velocity = new Vector2(h * speed, myRigidbody.velocity.y);
		} else {
			myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);
		}
	}
}
