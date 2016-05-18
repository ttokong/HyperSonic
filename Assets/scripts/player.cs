using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class player : MonoBehaviour {

	private Rigidbody2D myRigidbody;
	private Animator animator;
	private CircleCollider2D collider;


	public float speed = 5f;
	bool isCrouching = false;
	bool isRunning = false;
	bool isJumping = false;


	public Vector2 jumpHeight = new Vector2(0, 15);

	public float health;
	public float maxHealth;

	public float mana;
	public float maxMana;

	public float jumpTimer = 1;
	public float attackTimer = 0;

 	public List<float> coolDown = new List<float>();
 	public List<float> currentCoolDown = new List<float>();
 	public List<float> spellCost = new List<float>();


	public List<GameObject> SpellsCooldownOverlays;
	public List<GameObject> SpellsCooldownNumber;

	public GameObject healthUI;
	public GameObject manaUI;

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

		for (int i = 0; i < 4; i++){
			if(currentCoolDown[i] <= 0){
				currentCoolDown[i] = 0;
			} else {
				currentCoolDown[i] -= 0.02f;
			}
		}

		if(mana < maxMana){
			mana += 0.08f;
			if(mana > maxMana){
				mana = maxMana;
			}
		}

	}

	// Update is called once per frame
	void Update () {
		float horizontal = Input.GetAxis("Horizontal");

		//Crounching And Running Stuff
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
		HandleUI();
	}

	private void HandleUI(){
		//Updates the Health and Mana

		healthUI.GetComponent<Text>().text = "<b>HP</b> "+ Mathf.Round(health).ToString() + "/" + Mathf.Round(maxHealth).ToString();
		manaUI.GetComponent<Text>().text = "<b>MP</b> "+ Mathf.Round(mana).ToString() + "/" + Mathf.Round(maxMana).ToString();

		//Updates the Cooldown UI Area
		for (int i = 0; i < 4; i++){

			if(currentCoolDown[i] > 0){

				SpellsCooldownOverlays[i].SetActive(true);
				SpellsCooldownNumber[i].SetActive(true);
				SpellsCooldownNumber[i].GetComponent<Text>().text = Mathf.Round(currentCoolDown[i]).ToString();

			} else if(spellCost[i] > mana){
				
				SpellsCooldownNumber[i].SetActive(false);
				SpellsCooldownOverlays[i].SetActive(true);

			} else {

				SpellsCooldownOverlays[i].SetActive(false);
				SpellsCooldownNumber[i].SetActive(false);

			}
		}
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

    	if(Input.GetButtonDown("attack") && !attacking && !isJumping && currentCoolDown[0] <= 0){
    		animator.SetTrigger("attack");
    		attacking = true;
			attackTimer = 1f;
			currentCoolDown[0] = coolDown[0];
    	}

    	if(Input.GetButtonDown("airattack") && !attacking && !isJumping && currentCoolDown[1] <= 0 && mana >= 50){
    		animator.SetTrigger("airattack");
    		attacking = true;
			attackTimer = 1f;
			currentCoolDown[1] = coolDown[1];
			mana -= 50;
    	}
    	if(!attacking){
			myRigidbody.velocity = new Vector2(h * speed, myRigidbody.velocity.y);
		} else {
			myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);
		}
	}
}
