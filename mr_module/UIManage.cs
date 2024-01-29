using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UIManage : MonoBehaviour
{
    public GameObject videoInstrucion;
    public GameObject buttonList;
    public GameObject statusBoard;
    public GameObject robotArm;
    void Update()
    {
        Vector3 robotArmPosition = robotArm.transform.position;
        videoInstrucion.transform.position = new Vector3(robotArmPosition.x - 0.713f, robotArmPosition.y + 0.47f, robotArmPosition.z - 0.236f);
        buttonList.transform.position = new Vector3(robotArmPosition.x + 0.411f, robotArmPosition.y + 0.663f, robotArmPosition.z - 0.25f);
        statusBoard.transform.position = new Vector3(robotArmPosition.x - 0.523f, robotArmPosition.y, robotArmPosition.z);
    }
}
