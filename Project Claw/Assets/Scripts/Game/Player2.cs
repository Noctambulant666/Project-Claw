using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CharacterController( typeof(CharacterController))]
public class Player2 : MonoBehaviour {
	CharacterController controller;
	[SerializeField] float moveSpeed = 2f;
	[SerializeField] float rotSpeed = 375f;
	[SerializeField] float jump = 3f;
	[SerializeField] float gravity = 10f;
	Vector3 move;
	bool hasControl = false;

	void Awake()
	{
		controller = gameObject.GetComponent<CharacterController>();
	}
	void OnEnable()
	{
		if ( EventManager.Exists )
		{
			EventManager.StartListening( "Has Control", HasControl );
		}
	}
	void OnDisable()
	{
		if ( EventManager.Exists )
		{
			EventManager.StopListening( "Has Control", HasControl );
		}
	}
	void Update(){
		ControlSchemeA();
	}
	void ControlSchemeA()
	{

		controller.Move( move * Time.deltaTime);
		if ( !hasControl ) return; // check if controls are enabled

		if ( controller.isGrounded )
		{
			move.y = -gravity * Time.deltaTime;
			//			move.y = -controller.stepOffset / Time.deltaTime;

			transform.Rotate( 0, Input.GetAxis("Horizontal")*rotSpeed *Time.deltaTime,0 );

			move = Vector3.forward * Input.GetAxis( "Vertical");
			move = transform.TransformDirection( move );
			move *= moveSpeed;

			if (Input.GetButton("Jump"))
			{
				move.y = jump;
			}
		}
		else
		{
			move.y -= gravity * Time.deltaTime;
		}
	}
	void ControlSchemeB()
	{
		controller.Move( move * Time.deltaTime);
		if ( !hasControl ) return; // check if controls are enabled

		if ( controller.isGrounded )
		{
			move.y = -gravity * Time.deltaTime;

			KeyCode keyUp = KeyCode.W;
			KeyCode keyDown = KeyCode.S;
			KeyCode keyLeft = KeyCode.A;
			KeyCode keyRight = KeyCode.D;

//			if ( keyUp )
//			{
//				
//			}

			move = transform.TransformDirection( move );
			move *= moveSpeed;

			if (Input.GetButton("Jump"))
			{
				move.y = jump;
			}
		}
		else
		{
			move.y -= gravity * Time.deltaTime;
		}
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
	void HasControl()
	{
		hasControl = !hasControl;
		move = Vector3.zero;
	}
}