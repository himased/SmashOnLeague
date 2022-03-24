using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyHumanController : MonoBehaviour
{
	public float animSpeed = 1.5f;              // กำหนด speed ของ animator
	


	// ความเร็วไปด้านหน้า
	public float forwardSpeed = 7.0f;
	// ความเร็วไปด้านหลัง
	public float backwardSpeed = 2.0f;
	// ความเร็วในการหมุนตัวเพื่อเปลี่ยนทิศทางการเคลื่อนที่ 
	public float rotateSpeed = 2.0f;
	// พลังการกระโดด
	public float jumpPower = 3.0f;


	private Rigidbody rb;
	private Vector3 velocity;
	

	private Animator anim;                          // Animator ที่เป็น component ของ player
	private AnimatorStateInfo currentBaseState;         // state ปัจจุบันใน Animator

	
	// ดึงค่าสถานะของ state ต่างๆ มาใส่เก็บเอวไว้ในตัวแปร
	// เพื่อเอาไปใช้อ้างอิงในโปรแกรม

	static int idleState = Animator.StringToHash("Base Layer.Idle");
	static int locoState = Animator.StringToHash("Base Layer.Locomotion");
	static int jumpState = Animator.StringToHash("Base Layer.Jump");
	static int restState = Animator.StringToHash("Base Layer.Rest");

	
	void Start()
	{
		// anim จะอ้างอิงไปยัง Animator
		anim = GetComponent<Animator>();
		// rb จะอ้างอิงไปยัง Rigidbody 
		rb = GetComponent<Rigidbody>();
		
	
	}


	
	void FixedUpdate()
	{
		float h = Input.GetAxis("Horizontal");              // เก็บค่าในแนวแกนนอน ( -1 <= h <=1 ) 
		float v = Input.GetAxis("Vertical");                // เก็บค่าในแนวแกนตั้ง (-1 <= v <= 1)  
		anim.SetFloat("Speed", v);                          // Speed <--- v 
		anim.SetFloat("Direction", h);                      // Direction <--- h 
		anim.speed = animSpeed;                             // กำหนด speed ของ Animator
		currentBaseState = anim.GetCurrentAnimatorStateInfo(0); // Base Layer(0)
		rb.useGravity = true;



		// กดลูกศรขึ้นลง เพื่อกำหนดทิศทางไปด้านหน้า ซึ่งก็คือในแนวแกน z 
		velocity = new Vector3(0, 0, v);       
		// แล้วเปลี่ยนเวกเตอร์ veclocity ให้หันไปในทิศทางของ World Coordinate
		velocity = transform.TransformDirection(velocity);
		
		if (v > 0.1) // ถ้า v > 0.1 จะค่อยๆ เร็วไปทางด้านหน้า (หน้าคือ + )
		{
			velocity *= forwardSpeed;     
		}
		else if (v < -0.1) // ถ้า v < -0.1 จะค่อยๆ เร็วไปทางด้านหลัง (หลังคือ - )
		{
			velocity *= backwardSpeed;  // 
		}

		if (Input.GetButtonDown("Jump")) // เหมือนกับการกด  space 
		{   
			// ถ้าสถานะปัจจุบันคืออยู่ใน state ที่เป็น locomotion (walk or run) 
			if (currentBaseState.fullPathHash == locoState)
			{
				// และไม่ได้อยู่ในช่วง transition คืออยู่ใน state เรียบร้อยแล้ว
				if (!anim.IsInTransition(0))
				{
					// กระโดดดดดดดดดดไปในทิศทาง up (0,1,0) 
					rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
					anim.SetBool("Jump", true);     // พร้อมเปลี่ยน animationไปอยู่ในทางกระโดด
				}
			}
		}


		//  เปลี่ยนตำแหน่งไปทางด้านหน้าด้วยสูตร ระยะทาง = ความเร็ว x เวลาท
		transform.localPosition += velocity * Time.fixedDeltaTime;

		// หมุนรอบตัวเอง (หมุนรอบแกน y ) ด้วยมุม h*rotateSpeed 
		transform.Rotate(0, h * rotateSpeed, 0);


		// check เงื่อนไขหรือสถานะต่างๆ ตามชอบ หรือตามความจำเป็น 
		// ระหว่างที่อยู่ในสถานะ jumpState คือลอยตัวอยู่กลางอากาศ 
		if (currentBaseState.fullPathHash == jumpState)
		{

			if (!anim.IsInTransition(0))
			{
		      //  reset ค่า Jump --> false จะได้กลับไปเป็นทางเดินหรือวิ่งตอนกระโดดเสร็จ 
				anim.SetBool("Jump", false);
			}
		}
		// ระหว่างที่เป็น idleState คือยืนอยู่เฉยๆ 
		else if (currentBaseState.fullPathHash == idleState)
		{
		
			if (Input.GetButtonDown("Jump")) // ถ้ากดปุ่ม space จะเปลี่ยนเป็นท่าบิดขี้เกียจ
			{
				anim.SetBool("Rest", true);
			}
		}
	
	
		else if (currentBaseState.fullPathHash == restState)
		{
			if (!anim.IsInTransition(0))
			{
				// ปิดขี้เกียจเสร็จแล้ว ต้องให้กลับไปอยู่ท่า idle เหมือนเดิม ดังนั้นต้องเปลี่ยน Rest --> false
				anim.SetBool("Rest", false);
			}
		}
	}


}

