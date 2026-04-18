using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform CameraObj;
    public Transform TargetObj;
    public GameObject TargetCanvas;
    public PlaneCircleFly planeCircleFly;
    public void CameraPosChange()
    {
        planeCircleFly.currentIndex = 0;
        planeCircleFly.startMoving = true;

        if (planeCircleFly.locomotor != null)
        {
            planeCircleFly.locomotor.SetActive(false);
        }

        if (planeCircleFly.leftInteractions != null)
        {
            planeCircleFly.leftInteractions.SetActive(false);
        }

        if (planeCircleFly.rightInteractions != null)
        {
            planeCircleFly.rightInteractions.SetActive(false);
        }

        planeCircleFly.originalParent = planeCircleFly.playerRig.transform.parent;

        planeCircleFly.playerRig.transform.SetParent(planeCircleFly.cameraPoint);

        planeCircleFly.playerRig.transform.localPosition = Vector3.zero;
        planeCircleFly.playerRig.transform.localRotation = Quaternion.identity;

    }

    public void ColorChange(Button button)
    {
        if (button.image.color == Color.red)
        {
            button.image.color = Color.white;
        }
        else
        {
            button.image.color = Color.red;
        }

    }
}
