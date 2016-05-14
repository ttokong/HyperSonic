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
	}

	// Update is called once per frame
	void Update () {
		float horizontal = Input.GetAxis("Horizontal");
		if(!isJumping){
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

		if(h != 0f && !isCrouching && !isRunning && !isJumping)
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

        if (Input.GetKeyDown(KeyCode.Space) && jumpTimer >= 1)  //makes player jump
    	{
        	myRigidbody.AddForce(jumpHeight, ForceMode2D.Impulse);
        	jumpTimer = 0f;
        	isJumping = true;
        	animator.SetTrigger("jump");
    	}

		myRigidbody.velocity = new Vector2(h * speed, myRigidbody.velocity.y);
	}
}
