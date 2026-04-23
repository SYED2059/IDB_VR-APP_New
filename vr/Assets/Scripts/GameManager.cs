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

    public GameObject locomotor;
    public GameObject EnterButton;
    public GameObject BeginButton;

    public GameObject VfxObj;
    public VisualEffect Vfx;

    void Start()
    {
        Vfx.Reinit();
        Vfx.SendEvent("OnPlay");
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
        /*VfxObj.SetActive(true);
        Vfx.Play();*/
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
    }
}