using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CharacterController( typeof(CharacterController))]
public class Player2 : MonoBehaviour {
//	Rigidbody rb;
	CharacterController controller;
	[SerializeField] float moveSpeed = 2f;
	[SerializeField] float rotSpeed = 375f;
	[SerializeField] float jump = 3f;
	[SerializeField] float gravity = 10f;
	Vector3 move;
//	bool canJump = true;

	void Awake()
	{
//		rb = gameObject.GetComponent<Rigidbody>();
		controller = gameObject.GetComponent<CharacterController>();
	}
	void Update(){
		controller.Move( move * Time.deltaTime);

		if ( controller.isGrounded )
		{
			move.y = -gravity * Time.deltaTime;
//			move.y = -controller.stepOffset / Time.deltaTime;

			transform.Rotate( 0, Input.GetAxis("Horizontal")*rotSpeed *Time.deltaTime,0 );

			move = Vector3.forward * Input.GetAxis( "Vertical");
			move = transform.TransformDirection( move );
			move *= moveSpeed;

			if (Input.GetButton("Jump"))
//			if ( canJump )
			{
				Debug.Log("Jump");
				move.y = jump;
			}
		}
		else
		{
			move.y -= gravity * Time.deltaTime;
		}
//		move.y -= gravity * Time.deltaTime;

//		controller.Move( move * Time.deltaTime);
	}
	void OnTriggerEnter( Collider other )
	{
		if ( other.tag == "Pit" )
		{
			EventManager.TriggerEvent("Pit");
		}
		if ( other.tag == "Check Point" )
		{
			EventManager.TriggerEvent("Check Point");
		}
	}
}