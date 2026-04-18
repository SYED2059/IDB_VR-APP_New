using UnityEngine;
using System.Collections;

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
    public Transform cameraReset;

    [Header("Reset Points")]
    public Transform resetPlanePoint;

    public int currentIndex = 0;
    public bool startMoving = false;
    public Transform originalParent;


    [Header("Disable During Ride")]
    public GameObject locomotor;
    public GameObject leftInteractions;
    public GameObject rightInteractions;

    void Start()
    {
        currentIndex = 0;
        startMoving = true;

        if (locomotor != null)
        {
            locomotor.SetActive(false);
        }

        if (leftInteractions != null)
        {
            leftInteractions.SetActive(false);
        }

        if (rightInteractions != null)
        {
            rightInteractions.SetActive(false);
        }
        originalParent = playerRig.transform.parent;

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

        playerRig.transform.position = cameraReset.position;
        playerRig.transform.rotation = cameraReset.rotation;

        transform.position = resetPlanePoint.position;
        transform.rotation = resetPlanePoint.rotation;
        playerRig.transform.localScale = Vector3.one;


        if (locomotor != null)
        {
            locomotor.SetActive(true);
        }

        if (leftInteractions != null)
        {
            leftInteractions.SetActive(true);
        }

        if (rightInteractions != null)
        {
            rightInteractions.SetActive(true);
        }

    }

}