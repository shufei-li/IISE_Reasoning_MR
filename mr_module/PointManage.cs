using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
public class PointManage : MonoBehaviour
{
    private ListPoints listPoints;
    private GameObject[] points;
    internal bool isEditing;
    void Awake()
    {
        listPoints = GameObject.FindObjectOfType<ListPoints>();
    }
    void Update()
    {
        if (listPoints.points != null)
        {
            GetAllPoints();
        }
    }
    void GetAllPoints()
    {
        points = new GameObject[listPoints.points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = listPoints.points[i];
        }
    }
    public void OpenEditMode()
    {
        if (points != null)
        {
            isEditing = true;
            for (int i = 0; i < points.Length; i++)
            {
                var pointerHandler = points[i].GetComponent<PointerHandler>();
                var objectManipulator = points[i].GetComponent<ObjectManipulator>();
                pointerHandler.enabled = true;
                objectManipulator.enabled = true;
            }
        }
    }
    public void CloseEditMode()
    {
        if (points != null)
        {
            isEditing = false;
            for (int i = 0; i < points.Length; i++)
            {
                var pointerHandler = points[i].GetComponent<PointerHandler>();
                var objectManipulator = points[i].GetComponent<ObjectManipulator>();
                pointerHandler.enabled = false;
                objectManipulator.enabled = false;
            }
        }
    }
}
