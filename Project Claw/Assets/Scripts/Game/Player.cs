using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CharacterController( typeof(CharacterController))]
public class Player : MonoBehaviour {
	private CharacterController controller;
    [Header("Speed Base Variables")]
	[SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private float jump = 3f;
    [SerializeField] private float gravity = 10f;
    [Header("Speed Modifiers")]
    [SerializeField] private float mudSpeedModifier = 0.5f;
    [SerializeField] private float boostSpeedModifier = 0.5f;
    private Vector3 moveDirection; // Player directional movement vector
    private Vector3 additiveDirection; // Add to directional move vector to find adjusted move vector
    private bool hasControl = false;

    KeyCode keyUp = KeyCode.W;
    KeyCode keyDown = KeyCode.S;
    KeyCode keyLeft = KeyCode.A;
    KeyCode keyRight = KeyCode.D;

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
        // check if controls are enabled
        if (!hasControl)
        {
            return;
        }

        // Apply move here to ensure physics is always firing on character controller component
		controller.Move( moveDirection * Time.deltaTime);

        if ( controller.isGrounded )
		{
			moveDirection = Vector3.zero;
            additiveDirection = Vector3.zero; // Vector added to move vector to find adjusted speed

            // Find player movement direction
			if ( Input.GetKey(keyUp) )
			{
				transform.rotation = Quaternion.Euler( 0, 180, 0 );
				moveDirection = Vector3.forward;
			}
			if ( Input.GetKey(keyDown) )
			{
				transform.rotation = Quaternion.Euler( 0, 0, 0 );
				moveDirection = Vector3.forward;
			}
			if ( Input.GetKey(keyLeft) )
			{
				transform.rotation = Quaternion.Euler( 0, 90, 0 );
				moveDirection = Vector3.forward;
			}
			if ( Input.GetKey(keyRight) )
			{
				transform.rotation = Quaternion.Euler( 0, 270, 0 );
				moveDirection = Vector3.forward;
            }

            // Adjust movement for speed modifiers. Calculate direction vector to adjust movement
            // Maintains a linear relationship with player speed through the use of direction vectors
            Ray ray = new Ray(transform.position, Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "Mud")
                {
                    // Slow down player by a given factor
                    additiveDirection = transform.InverseTransformDirection(moveDirection);
                    additiveDirection *= mudSpeedModifier;
                }
                else if (hit.collider.gameObject.tag == "Boost")
                {
                    // Apply boost direction to player direction vector
                    additiveDirection = hit.collider.transform.forward;
                    additiveDirection *= boostSpeedModifier;
                }
            }

            // Calculate player move speed as direction vector multiplied by the moveSpeed variable
            moveDirection = transform.TransformDirection( moveDirection );
            moveDirection = moveDirection + additiveDirection;
            moveDirection *= moveSpeed;

            // Jump
            if (Input.GetButton("Jump"))
			{
				moveDirection.y = jump;
			}
		}
		else
		{
            // Adjust for gravity
			moveDirection.y -= gravity * Time.deltaTime;
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
        // Toggle control
		hasControl = !hasControl;
		moveDirection = Vector3.zero;
	}
}