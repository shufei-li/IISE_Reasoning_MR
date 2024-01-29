using UnityEngine;
using Vuforia;
public class FrameRate : MonoBehaviour
{
    void Start()
    {
        VuforiaApplication.Instance.OnVuforiaStarted += OnVuforiaStarted;
    }
    void OnVuforiaStarted()
    {
        Var target Fps=uforiaBehaviour.Instance.CameraDevice.GetRecommendedFPS();
        if (Application.targetFrameRate != targetFps)
        {
            Debug.Log("Setting frame rate to " + targetFps + "fps");
            Application.targetFrameRate = targetFps;
        }
    }
}
