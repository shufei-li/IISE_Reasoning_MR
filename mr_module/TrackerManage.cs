using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
public class TrackerManage : MonoBehaviour
{
    private ModelTargetBehaviour modelTargetBehaviour;
    void Awake()
    {
        modelTargetBehaviour = GameObject.FindObjectOfType<ModelTargetBehaviour>();
    }
    public void resetTracker()
    {
        modelTargetBehaviour.Reset();
        VuforiaBehaviour.Instance.DevicePoseBehaviour.Reset();
    }
    public void initializeTracker()
    {
        VuforiaApplication.Instance.Deinit();
        VuforiaApplication.Instance.Initialize();
    }
}
