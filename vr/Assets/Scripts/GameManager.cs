using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.VFX;

public class GameManager : MonoBehaviour
{
    public Transform SpaceshipObj;
    public Transform PlayerCameraObj;

    public Transform SpaceshipEntryPoint;
    public Transform PlayerCameraEntryPoint;

    public Transform SpaceshipBeginPoint;
    public Transform PlayerCameraBeginPoint;

    public Transform SpaceshipRestartPoint;
    public Transform PlayerCameraRestartPoint;

    public GameObject locomotor;
    public GameObject EnterButton;
    public GameObject BeginButton;
    public GameObject TargetCanvasObj;
    public GameObject LoopCanvasObj;


    public GameObject VfxObj;
    public VisualEffect Vfx;


    [Header("UI Card Animation")]
    public GameObject[] uiCards;
    public bool startUICardLoop = false;

    public float cardStartDistance = 50f;
    public float cardCenterDistance = 4f;
    public float cardEndDistance = -50f;

    public float enterDuration = 3f;
    public float stayDuration = 0.5f;
    public float exitDuration = 1f;
    public float delayBetweenCards = 0.1f;

    void Start()
    {
       
    }
    public void OnEnterClicked()
    {
        Debug.Log("Enter Button Clicked");

        if (SpaceshipObj && SpaceshipEntryPoint)
        {
            SpaceshipObj.SetPositionAndRotation(
                SpaceshipEntryPoint.position,
                SpaceshipEntryPoint.rotation
            );
        }

        if (PlayerCameraObj && PlayerCameraEntryPoint)
        {
            PlayerCameraObj.SetPositionAndRotation(
                PlayerCameraEntryPoint.position,
                PlayerCameraEntryPoint.rotation
            );
        }
        StartCoroutine(EnableMovementAfterDelay());
        EnterButton.SetActive(false);

    }

    IEnumerator EnableMovementAfterDelay()
    {
        yield return new WaitForSeconds(1f);

        if (locomotor != null)
        {
            locomotor.SetActive(false);
        }
    }

    public void OnBeginClicked()
    {
        Debug.Log("Begin Button Clicked");
        locomotor.SetActive(true);
        VfxObj.SetActive(true);
        Vfx.Reinit();
        Vfx.SendEvent("OnPlay");
        if (SpaceshipObj && SpaceshipEntryPoint)
        {
            SpaceshipObj.SetPositionAndRotation(
                SpaceshipBeginPoint.position,
                SpaceshipBeginPoint.rotation
            );
        }

        if (PlayerCameraObj && PlayerCameraEntryPoint)
        {
            PlayerCameraObj.SetPositionAndRotation(
                PlayerCameraBeginPoint.position,
                PlayerCameraBeginPoint.rotation
            );
        }
        StartCoroutine(EnableMovementAfterDelay());
        TargetCanvasObj.SetActive(true);
    }
   
    public void ActiveLoop()
    {
        TargetCanvasObj.SetActive(false);
        LoopCanvasObj.SetActive(true);
        startUICardLoop = true;
        StartCoroutine(ShowUICardsLoop());
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

    public void OnRestartClicked()
    {
        Debug.Log("Restart Button Clicked");
        startUICardLoop = false;
        EnterButton.SetActive(true);
        BeginButton.SetActive(true);
        TargetCanvasObj.SetActive(false);
        LoopCanvasObj.SetActive(false);
        Vfx.Stop();
        VfxObj.SetActive(false);

        if (SpaceshipObj && SpaceshipEntryPoint)
        {
            SpaceshipObj.SetPositionAndRotation(
                SpaceshipRestartPoint.position,
                SpaceshipRestartPoint.rotation
            );
        }

        if (PlayerCameraObj && PlayerCameraEntryPoint)
        {
            PlayerCameraObj.SetPositionAndRotation(
                PlayerCameraRestartPoint.position,
                PlayerCameraRestartPoint.rotation
            );
        }
        
    }
}