using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Microsoft.MixedReality.Toolkit.UI;
public class LineCreator : MonoBehaviour
{
    private LineRenderer line;
    private ListPoints listPoints;
    private GameObject[] points;
    private PointCreator pointCreator;
    bool isLined = false;
    bool isChanging = false;
    private PointManage pointManage;
    void Awake()
    {
        listPoints = GameObject.FindObjectOfType<ListPoints>();
        pointCreator = GameObject.FindObjectOfType<PointCreator>();
        pointManage = GameObject.FindObjectOfType<PointManage>();
    }
    void Start()
    {
        line = this.gameObject.GetComponent<LineRenderer>();
    }
    void Update()
    {
        this.isChanging = pointManage.isEditing;

        if (pointCreator.isCreated && !isLined)
        {
            SetUpLine();
            isLined = true;
        }
        if (!pointCreator.isCreated)
        {
            isLined = false;
        }
        if (isChanging)
        {
            if(points != null)
            {
                LineUp();
                Debug.Log("Position is editing...");
            }
            else
            {
                Debug.Log("No point can be adjusted");
            }
        }
    }
    void SetUpLine()
    {
        if (listPoints.points != null)
        {
            LineUp();
        }
    }
    void LineUp()
    {
        line.startWidth = 0.012f;
        line.endWidth = 0.012f;
        points = new GameObject[listPoints.points.Length];
        line.positionCount = points.Length;
        for (int i = 0; i <= points.Length; i++)
        {
            points[i] = listPoints.points[i];
            line.SetPosition(i, points[i].transform.position);
        }
    }
}
