using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        if (CameraModeManager.cameraMode == CameraMode.FPS)
        {
            this.gameObject.SetActive(true);
        }
        else if (CameraModeManager.cameraMode == CameraMode.TPS)
        {
            this.gameObject.SetActive(false);
        }
    }
}
