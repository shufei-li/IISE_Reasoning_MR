using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
public class TouchDetection : MonoBehaviour
{
    private PanelManage panelManage;
    private bool isOpen;
    public void OnTouchStarted(HandTrackingInputEventData eventData) { }
    public void OnTouchCompleted(HandTrackingInputEventData eventData) { }
    public void OnTouchUpdated(HandTrackingInputEventData eventData) { }
    public static void MakeTouchable(GameObject target)
    {
        var touchable = target.AddComponent<NearInteractionTouchableVolume>();
        touchable.EventsToReceive = TouchableEventType.Pointer;
        var material = target.GetComponent<Renderer>().material;
        var pointerHandler = target.AddComponent<PointerHandler>();
        pointerHandler.enabled = false;
        pointerHandler.OnPointerClicked.AddListener((e) => OpenPanel(target));
    }
    void Awake()
    {
        panelManage = GameObject.FindObjectOfType<PanelManage>();
    }
    void Start()
    {
        MakeTouchable(this.gameObject);
    }
    public static void OpenPanel(GameObject point)
    {
        if (point.transform.childCount != 0)
        {
            GameObject Panel = point.transform.GetChild(0).gameObject;
            Panel.SetActive(true);
        }
    }
}
