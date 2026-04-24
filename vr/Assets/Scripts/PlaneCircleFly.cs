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

    [Header("Disable Object At Waypoint")]
    public GameObject objectToDisable;
    public int disableAtIndex = 5;
    private bool objectDisabled = false;

    public GameObject TargetCanvas;
    public GameObject TargetLoopCanvas;


    [Header("UI Card Animation")]
    public GameObject[] uiCards;
    public bool startUICardLoop = false;

    public float cardStartDistance = 30f;
    public float cardCenterDistance = 4f;
    public float cardEndDistance = -10f;

    public float enterDuration = 5f;
    public float stayDuration = 1.5f;
    public float exitDuration = 4f;
    public float delayBetweenCards = 0.5f;


    /* void Start()
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
     }*/

    void Update()
    {
        if (!startMoving || currentIndex >= waypoints.Length)
            return;

        if (!objectDisabled && currentIndex == disableAtIndex)
        {
            objectDisabled = true;

            if (objectToDisable != null)
            {
                objectToDisable.SetActive(false);
            }
        }

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
        objectDisabled = false;
        if (objectToDisable != null)
        {
            objectToDisable.SetActive(true);
        }

        TargetCanvas.SetActive(true);

        StartCoroutine(EnableMovementAfterDelay());
        StartCoroutine(ShowUICardsLoop());
    }
    

    IEnumerator EnableMovementAfterDelay()
    {
        yield return new WaitForSeconds(1f);

        if (locomotor != null)
        {
            locomotor.SetActive(false);
        }
    }


    IEnumerator ShowUICardsLoop()
    {
        while (startUICardLoop)
        {
            for (int i = 0; i < uiCards.Length; i++)
            {
                GameObject currentCard = uiCards[i];

                if (currentCard == null)
                    continue;

                currentCard.SetActive(true);

                CanvasGroup cg = currentCard.GetComponent<CanvasGroup>();

                if (cg == null)
                {
                    cg = currentCard.AddComponent<CanvasGroup>();
                }

                cg.alpha = 0f;

                Vector3 startLocalPos = new Vector3(0f, 0f, cardStartDistance);
                Vector3 centerLocalPos = new Vector3(0f, 0f, cardCenterDistance);
                Vector3 endLocalPos = new Vector3(0f, 0f, cardEndDistance);

                currentCard.transform.localPosition = startLocalPos;
                currentCard.transform.localRotation = Quaternion.identity;
                currentCard.transform.localScale = Vector3.one;

                float enterTimer = 0f;

                while (enterTimer < enterDuration)
                {
                    enterTimer += Time.deltaTime;

                    float t = enterTimer / enterDuration;

                    currentCard.transform.localPosition = Vector3.Lerp(
                        startLocalPos,
                        centerLocalPos,
                        t
                    );

                    cg.alpha = Mathf.Lerp(0f, 1f, t);

                    yield return null;
                }

                currentCard.transform.localPosition = centerLocalPos;
                cg.alpha = 1f;

                yield return new WaitForSeconds(stayDuration);

                float exitTimer = 0f;

                while (exitTimer < exitDuration)
                {
                    exitTimer += Time.deltaTime;

                    float t = exitTimer / exitDuration;

                    currentCard.transform.localPosition = Vector3.Lerp(
                        centerLocalPos,
                        endLocalPos,
                        t
                    );

                    cg.alpha = Mathf.Lerp(1f, 0f, t);

                    yield return null;
                }

                currentCard.transform.localPosition = endLocalPos;
                cg.alpha = 0f;

                currentCard.SetActive(false);

                yield return new WaitForSeconds(delayBetweenCards);
            }
        }
    }

    public void ActiveLoop()
    {
        TargetCanvas.SetActive(false);
        TargetLoopCanvas.SetActive(true);
        startUICardLoop = true;
        StartCoroutine(ShowUICardsLoop());
        Debug.Log("startUICardLoop" +: startUICardLoop);
    }


}