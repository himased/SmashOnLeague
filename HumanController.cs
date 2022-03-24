using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : MonoBehaviour
{
	// 
	public float forwardSpeed = 5.0f;
	// 
	public float backwardSpeed = 2.0f;
	// 
	public float rotateSpeed = 2.0f;
	// 
	public float jumpSpeed = 3.0f;
	// 
	public float gravity = 20.0f;

	// 
	CharacterController character;
	// 
	Vector3 velocity;
	Animator anim;

	float animSpeed = 1.5f;

	static int idleState = Animator.StringToHash("Base Layer.Idle");
	static int locoState = Animator.StringToHash("Base Layer.Locomotion");
	static int jumpState = Animator.StringToHash("Base Layer.Jump");
	static int restState = Animator.StringToHash("Base Layer.Rest");

	private AnimatorStateInfo currentBaseState;

	void Start()
	{
		//
		character = GetComponent<CharacterController>();
		anim = GetComponent<Animator>();
	}

	void Update()
	{
		float v = Input.GetAxis("Vertical");        // 
		float h = Input.GetAxis("Horizontal");  // 

		anim.SetFloat("Speed", v);
		anim.SetFloat("Direction", h);

		currentBaseState = anim.GetCurrentAnimatorStateInfo(0);
		
		if (character.isGrounded)
		{   // 
			velocity = new Vector3(0, 0, v);        // 
													// 
			velocity = transform.TransformDirection(velocity);

			if (v > 0.1f)
			{
				velocity *= forwardSpeed;       // 
			}
			else if (v < -0.1)
			{
				velocity *= backwardSpeed;  // 
			}

			if (Input.GetKey(KeyCode.Space))
			{   // 

				if(currentBaseState.fullPathHash == locoState)
                {
					
					if (!anim.IsInTransition(0))
					{
						velocity.y = jumpSpeed;
						anim.SetBool("Jump", true);
					}
				}
				  
			}
		}

		velocity.y -= gravity * Time.deltaTime;

		character.Move(velocity * Time.deltaTime);

		transform.Rotate(0, h * rotateSpeed, 0);

		if (currentBaseState.fullPathHash == jumpState)
		{
	
			if (!anim.IsInTransition(0))
			{
			
				anim.SetBool("Jump", false);
			}
		}
		// 
		else if (currentBaseState.fullPathHash == idleState)
		{

			if (Input.GetButtonDown("Jump"))
			{
				anim.SetBool("Rest", true);
			}
		}
		// 
		else if (currentBaseState.fullPathHash == restState)
		{

			{
				anim.SetBool("Rest", false);
			}
		}

	}
}
