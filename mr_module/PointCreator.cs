using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.UI;

public class PointCreator : MonoBehaviour
{
    private IMixedRealityHandJointService handJointService;
    private IMixedRealityHandJointService HandJointService =>
        handJointService ??
        (handJointService = CoreServices.GetInputSystemDataProvider<IMixedRealityHandJointService>());
    private MixedRealityPose? previousLeftHandPose;
    private MixedRealityPose? previousRightHandPose;
    private const float GrabThreshold = 0.4f;
    private GameObject trackingObject;
    private GameObject movementPoint;
    private Vector3 trackingTransform;
    private Vector3 setScale;
    internal bool isCreated = false;
    void Start()
    {
        if (trackingObject == null && movementPoint == null)
        {
            trackingObject = GameObject.Find("HandTrack");
            movementPoint = GameObject.Find("PointPath");
        }
        else
        {
            Debug.Log("Object is missed!!");
        }
    }
    void Update()
    {
        trackingTransform = trackingObject.transform.position;
        if (IsGrabbing(Handedness.Both) && !isCreated)
        {
            CreatePoint();
            isCreated = true;
        }
        if (!IsGrabbing(Handedness.Both))
        {
            isCreated = false;
        }
    }
    public static bool IsGrabbing(Handedness trackedHand)
    {
        return HandPoseUtils.IndexFingerCurl(trackedHand) > GrabThreshold &&
               HandPoseUtils.MiddleFingerCurl(trackedHand) > GrabThreshold &&
               HandPoseUtils.RingFingerCurl(trackedHand) > GrabThreshold &&
               HandPoseUtils.PinkyFingerCurl(trackedHand) > GrabThreshold &&
               HandPoseUtils.ThumbFingerCurl(trackedHand) > GrabThreshold;
    }
    private void CreatePoint()
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.parent = movementPoint.transform;
        sphere.transform.position = trackingTransform;
        Vector3 setScale = sphere.transform.localScale;
        setScale = new Vector3(0.05f * setScale.x, 0.05f * setScale.y, 0.05f * setScale.z);
        sphere.transform.localScale = setScale;
        sphere.AddComponent<ObjectManipulator>();
        sphere.AddComponent<NearInteractionGrabbable>();
        sphere.AddComponent<TouchDetection>();
        sphere.GetComponent<ObjectManipulator>().enabled = false;
    }
}
