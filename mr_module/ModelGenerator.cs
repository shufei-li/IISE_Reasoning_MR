using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ModelGenerator : MonoBehaviour
{
    [System.Serializable]
    public struct ModelTarget
    {
        public GameObject oModelTarget;
        public GameObject gModelTarget;
    }
    public ModelTarget[] modelTargets;
    private Vector3[] oModelTransform;
    private Quaternion[] oModelRotation;
    private Vector3[] gModelTransform;
    private Quaternion[] gModelRotation;
    void Start()
    {
        oModelTransform = new Vector3[modelTargets.Length];
        gModelTransform = new Vector3[modelTargets.Length];
        oModelRotation = new Quaternion[modelTargets.Length];
        gModelRotation = new Quaternion[modelTargets.Length];
    }
    public void GenerateBattery()
    {
        oModelTransform[0] = modelTargets[0].oModelTarget.transform.position;
        oModelRotation[0] = modelTargets[0].oModelTarget.transform.rotation;
        Vector3 GenModelTransform = oModelTransform[0];
        Quaternion GenModelRotation = oModelRotation[0];
        modelTargets[0].gModelTarget.transform.position = GenModelTransform;
        modelTargets[0].gModelTarget.transform.rotation = GenModelRotation;
        modelTargets[0].gModelTarget.SetActive(true);
    }
    public void GenerateRobotArm()
    {
        oModelTransform[1] = modelTargets[1].oModelTarget.transform.position;
        oModelRotation[1] = modelTargets[1].oModelTarget.transform.rotation;
        Vector3 GenModelTransform = oModelTransform[1];
        Quaternion GenModelRotation = oModelRotation[1];
        modelTargets[1].gModelTarget.transform.position = GenModelTransform;
        modelTargets[1].gModelTarget.transform.rotation = GenModelRotation;
        modelTargets[1].gModelTarget.SetActive(true);
    }
}
