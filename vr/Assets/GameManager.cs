using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform CameraObj;
    public Transform TargetObj;
    public GameObject TargetCanvas;

    public void CameraPosChange()
    {
        CameraObj.position = TargetObj.position;
        CameraObj.rotation = TargetObj.rotation;
        TargetCanvas.SetActive(true);

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
