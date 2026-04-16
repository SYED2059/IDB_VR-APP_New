using UnityEngine;

public class PlaneCircleFly : MonoBehaviour
{
    [Header("Plane Movement")]
    public Transform[] waypoints;
    public float moveSpeed = 5f;
    public float rotateSpeed = 5f;

    [Header("Plane Model Rotation Fix")]
    public Vector3 modelRotationOffset = new Vector3(0, 180, 0);

    [Header("Player Setup")]
    public GameObject playerRig;
    public Transform seatPoint;
    public Transform cameraPoint;

    [Header("Disable During Ride")]
    public GameObject movementScript;
    public GameObject rotationScript;

    private int currentIndex = 0;
    private bool startMoving = false;
    private Transform originalParent;

    void Start()
    {
        startMoving = true;

        originalParent = playerRig.transform.parent;

        if (movementScript != null)
            movementScript.SetActive(false);

        if (rotationScript != null)
            rotationScript.SetActive(false);

        playerRig.transform.SetParent(cameraPoint);

        playerRig.transform.localPosition = Vector3.zero;
        playerRig.transform.localRotation = Quaternion.identity;
    }

    void Update()
    {
        if (!startMoving || currentIndex >= waypoints.Length)
            return;

        

        Transform target = waypoints[currentIndex];

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime
        );

        Vector3 direction = (target.position - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            lookRotation *= Quaternion.Euler(modelRotationOffset);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                lookRotation,
                rotateSpeed * Time.deltaTime
            );
        }

        playerRig.transform.position = cameraPoint.position;
        playerRig.transform.rotation = cameraPoint.rotation;

        if (Vector3.Distance(transform.position, target.position) < 0.2f)
        {
            currentIndex++;

            if (currentIndex >= waypoints.Length)
            {
                EndPlaneRide();
            }
        }
    }

    void EndPlaneRide()
    {
        startMoving = false;

        playerRig.transform.SetParent(originalParent);

        if (movementScript != null)
            movementScript.SetActive(true);

        if (rotationScript != null)
            rotationScript.SetActive(true);

        gameObject.SetActive(false);
    }
}