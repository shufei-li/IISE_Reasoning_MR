using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ListPoints : MonoBehaviour
{
    private GameObject pointPath;
    internal GameObject[] points;
    internal Vector3[] pointsT;
    private PointCreator pointCreator;
    bool isStored = false;

    void Awake()
    {
        pointCreator = GameObject.FindObjectOfType<PointCreator>();
    }
    void Start()
    {
        pointPath = GameObject.Find("PointPath");
    }
    void Update()
    {
        Debug.Log(pointCreator.isCreated);
        if (pointCreator.isCreated && !isStored)
        {
            pointsList();
            isStored = true;
        }
        if (!pointCreator.isCreated)
        {
            isStored = false;
        }
    }
    void pointsList()
    {
        points = new GameObject[pointPath.transform.childCount];
        pointsT = new Vector3[pointPath.transform.childCount];
        for (int i = 0; i < pointPath.transform.childCount; i++)
        {
            points[i] = pointPath.transform.GetChild(i).gameObject;
            pointsT[i] = pointPath.transform.GetChild(i).position;
            points[i].name = "Point" + i;
        }
    }
    public void destoryAllPoints()
    {
        for (int i = 0; i < points.Length; i++)
        {
            Destroy(points[i]);
        }
    }
}
