using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CharacterController( typeof(CharacterController))]
public class Player : MonoBehaviour {
	[SerializeField] private CharacterController controller;
	[SerializeField] float moveSpeed = 2.0f;
    [SerializeField] float mudSpeed = 1.0f;
    [SerializeField] float boostSpeed = 0.5f;
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

        float currentSpeed = moveSpeed;
		if ( controller.isGrounded )
		{
			move = Vector3.zero;
            Vector3 boostDir = Vector3.zero;

            Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
            Ray ray = new Ray(pos,Vector3.down);
            RaycastHit hit;
            if ( Physics.Raycast( ray, out hit ) )
            {
                if ( hit.collider.gameObject.tag == "Mud" )
                {
                    currentSpeed = mudSpeed;
                }
                else if ( hit.collider.gameObject.tag == "Boost")
                {
                    GameObject boost = hit.collider.gameObject;
                    boostDir = boost.transform.forward;
                    boostDir *= boostSpeed;
                    Debug.Log("Boosting");

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
            move = move + boostDir;
			move *= currentSpeed;

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