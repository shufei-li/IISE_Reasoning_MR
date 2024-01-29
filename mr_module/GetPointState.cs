using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
public class GetPointState : MonoBehaviour
{
    internal TextMeshPro pointName = null;
    internal Vector3 pointPosition;
    void Start()
    {
        if (pointName == null)
        {
            pointName = GetComponent<TextMeshPro>();
            pointName.text = this.gameObject.transform.parent.transform.parent.transform.parent.transform.parent.name;
        }
    }
    void Update()
    {
        pointPosition = this.gameObject.transform.parent.transform.parent.transform.parent.transform.parent.transform.position;
    }
}
