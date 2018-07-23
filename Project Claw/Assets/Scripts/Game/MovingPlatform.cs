using UnityEngine;

public class MovingPlatform : MonoBehaviour {
    [Header("References and Variables")]
    public Transform platform;
    [Tooltip("Empty game object used as a point of reference for movement")]
    public Transform markerPointA;
    [Tooltip("Empty game object used as a point of reference for movement")]
    public Transform markerPointB;
    [Tooltip("Controls how fast platform moves between reference points")]
    [SerializeField]private float speed = 2f;

    [Header("Positions")]
    private Vector3 lastPosition;
    private Vector3 curPosition;
    private Vector3 pointA;
    private Vector3 pointB;
    private Vector3 moveTarget;
    public Vector3 moveResultant; // Actual movement vector between last frame and current frame

    public void Start()
    {
        curPosition = platform.position;
        pointA = markerPointA.position;
        pointB = markerPointB.position;
        moveTarget = pointA;
        moveResultant = Vector3.zero;
    }
    public void Update()
    {
        // Get last position
        lastPosition = curPosition;

        // Locate target position
        if (curPosition == pointA)
            moveTarget = pointB;
        else if (curPosition == pointB)
            moveTarget = pointA;

        // Calculate movement and find next position
        curPosition = Vector3.MoveTowards(curPosition, moveTarget, Time.deltaTime * speed);

        // Set new position
        platform.position = new Vector3(curPosition.x, platform.position.y, curPosition.z);

        // Calculate total movement
        moveResultant = curPosition - lastPosition;
    }
}