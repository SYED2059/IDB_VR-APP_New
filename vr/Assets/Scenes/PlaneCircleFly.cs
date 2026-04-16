using UnityEngine;

public class PlaneCircleFly : MonoBehaviour
{
    public Transform[] waypoints;
    public float moveSpeed = 5f;
    public float rotateSpeed = 5f;

    [Header("Model Rotation Fix")]
    public Vector3 modelRotationOffset = new Vector3(0, 180, 0);

    private int currentIndex = 0;
    private bool startMoving = false;

    public void Start()
    {
        startMoving = true;
    }

    void Update()
    {
        if (!startMoving || currentIndex >= waypoints.Length)
            return;

        Transform target = waypoints[currentIndex];

        // Move toward current waypoint
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime
        );

        // Direction toward target
        Vector3 direction = (target.position - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            // Fix wrong model forward direction
            lookRotation *= Quaternion.Euler(modelRotationOffset);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                lookRotation,
                rotateSpeed * Time.deltaTime
            );
        }

        // If reached current waypoint
        if (Vector3.Distance(transform.position, target.position) < 0.2f)
        {
            currentIndex++;

            // Optional: hide plane after last point
            if (currentIndex >= waypoints.Length)
            {
                gameObject.SetActive(false);
            }
        }
    }
}