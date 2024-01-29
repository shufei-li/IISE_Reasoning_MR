using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using TMPro;
public class PanelManage : MonoBehaviour
{
    private ListPoints listPoints;
    private GameObject[] points;
    public GameObject MyPanel;
    private GameObject[] PointsPanel;
    internal bool isOpen = false;
    void Awake()
    {
        listPoints = GameObject.FindObjectOfType<ListPoints>();
    }
    void Update()
    {
        if(listPoints.points != null)
        {
            GetAllPoints();
        }
        if (points != null)
        {
            AddToopTipinEachPoint();
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
    void AddToopTipinEachPoint()
    {
        for (int i = 0; i < points.Length; i++)
        {
            int hasTooltip = points[i].transform.childCount;
            PointsPanel = new GameObject[points.Length];
            if (hasTooltip == 0)
            {
                GameObject PointTooltip = Instantiate(MyPanel, points[i].transform.position, Quaternion.identity);
                PointsPanel[i] = PointTooltip;
                PointTooltip.transform.parent = points[i].transform;
                PointTooltip.SetActive(false);
                PointTooltip.GetComponent<ToolTipConnector>().Target = points[i];
                PointTooltip.GetComponent<ToolTipConnector>().PivotDistance = 0.1f;
            }
        }
    }
}
