using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CameraMode
{
    TPS,
    FPS
}

public class CameraModeManager : MonoBehaviour
{
    public static CameraMode cameraMode = CameraMode.TPS;

    public void SetCameraModeFromToggle(bool check)
    {
        if (check)
        {
            cameraMode = CameraMode.FPS;
        }
        else
        {
            cameraMode = CameraMode.TPS;
        }
    }

}
