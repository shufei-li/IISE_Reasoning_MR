using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SafetyDetection : MonoBehaviour
{
    public GameObject robotArm;
    private GameObject safetyReminder;
    private GameObject innerSafetyArea;
    private GameObject midSafetyArea;
    private GameObject outterSafetyArea;
    public Material oldInnerLayer;
    public Material oldMidLayer;
    public Material oldOutterLayer;
    public Material newInnerLayer;
    public Material newMidLayer;
    public Material newOutterLayer;
    void Start()
    {
        safetyReminder = this.gameObject;
        innerSafetyArea = this.gameObject.transform.Find("SafetyArea - inner").gameObject;
        midSafetyArea = this.gameObject.transform.Find("SafetyArea - mid").gameObject;
        outterSafetyArea = this.gameObject.transform.Find("SafetyArea - outer").gameObject;
    }
    void Update()
    {
        Vector3 robotArmPosition = robotArm.transform.position;
        safetyReminder.transform.position = robotArmPosition;
        var headPosition = Camera.main.transform.position;
        float safetyDistance = Mathf.Abs(Vector3.Distance(robotArmPosition, headPosition));
        MeshRenderer inMeshRenderer = innerSafetyArea.GetComponent<MeshRenderer>();
        MeshRenderer midMeshRenderer = midSafetyArea.GetComponent<MeshRenderer>();
        MeshRenderer outMeshRenderer = outterSafetyArea.GetComponent<MeshRenderer>();
        if (safetyDistance <= 2.5f)
        {
            inMeshRenderer.enabled = true;
            midMeshRenderer.enabled = true;
            outMeshRenderer.enabled = true;
            inMeshRenderer.material = oldInnerLayer;
            midMeshRenderer.material = oldMidLayer;
            outMeshRenderer.material = oldOutterLayer;
            if (safetyDistance <= 2)
            {
                midMeshRenderer.material = newMidLayer;
                outMeshRenderer.material = newOutterLayer;
                if (safetyDistance <= 1.5f)
                {
                    inMeshRenderer.material = newInnerLayer;
                    midMeshRenderer.material = oldMidLayer;
                    outMeshRenderer.material = newOutterLayer;
                }
            }
        }
        else
        {
            inMeshRenderer.enabled = false;
            midMeshRenderer.enabled = false;
            outMeshRenderer.enabled = false;
        }
        TextMessage.Instance.ChangeTextMessage(string.Format("SafetyDistance :\n {0}", safetyDistance >= 1.0f ? safetyDistance.ToString("####0.0") + "m" : safetyDistance.ToString("0.00") + "cm"));
    }
}
