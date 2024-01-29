using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ShowPointPosition : MonoBehaviour
{
    private Vector3 pointPosition;
    private GetPointState getPointState;
    private TextMeshPro position = null;
    void Awake()
    {
        getPointState = GameObject.FindObjectOfType<GetPointState>();
    }
    void Start()
    {
        position = GetComponent<TextMeshPro>();
    }
    void Update()
    {
        this.pointPosition = getPointState.pointPosition;
        position.text = "x:" + pointPosition.x.ToString("F3") + " y:" + pointPosition.y.ToString("F3") + " z:" + pointPosition.z.ToString("F3");
    }
}
