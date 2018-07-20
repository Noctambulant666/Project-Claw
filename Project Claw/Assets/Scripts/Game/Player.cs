using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CharacterController( typeof(CharacterController))]
public class Player : MonoBehaviour {
	[SerializeField] private CharacterController controller;
	[SerializeField] float baseMoveSpeed = 2f;
    [SerializeField] float mudMoveSpeed = 1f;
    //[SerializeField] float boostMoveSpeed = 2f;
	[SerializeField] float jump = 3f;
	[SerializeField] float gravity = 10f;
	private Vector3 move;
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
		ControlScheme();
	}
	void ControlScheme()
	{
		controller.Move( move * Time.deltaTime);
		if ( !hasControl ) return; // check if controls are enabled

        float speed = baseMoveSpeed;
		if ( controller.isGrounded )
		{
			move = Vector3.zero;

            Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
            Ray ray = new Ray(pos,Vector3.down);
            RaycastHit hit;
            if ( Physics.Raycast( ray, out hit ) )
            {
                if ( hit.collider.gameObject.tag == "Mud" )
                {
                    speed = mudMoveSpeed;
                }
            }

			KeyCode keyUp = KeyCode.W;
			KeyCode keyDown = KeyCode.S;
			KeyCode keyLeft = KeyCode.A;
			KeyCode keyRight = KeyCode.D;

			if ( Input.GetKey(keyUp) )
			{
				transform.rotation = Quaternion.Euler( 0, 180, 0 );
				move = Vector3.forward;
			}
			if ( Input.GetKey(keyDown) )
			{
				transform.rotation = Quaternion.Euler( 0, 0, 0 );
				move = Vector3.forward;
			}
			if ( Input.GetKey(keyLeft) )
			{
				transform.rotation = Quaternion.Euler( 0, 90, 0 );
				move = Vector3.forward;
			}
			if ( Input.GetKey(keyRight) )
			{
				transform.rotation = Quaternion.Euler( 0, 270, 0 );
				move = Vector3.forward;
			}

			move = transform.TransformDirection( move );
			move *= speed;

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