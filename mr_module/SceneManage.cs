using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManage : MonoBehaviour
{
    public void CompleteRegister()
    {
        SceneManager.LoadScene("ProcessReview", LoadSceneMode.Single);
    }
    public void BackRegister()
    {
        SceneManager.LoadScene("UserRegistration", LoadSceneMode.Single);
    }
    public void ProcessConfirm()
    {
        SceneManager.LoadScene("Vuforia10 - TrackingTarget", LoadSceneMode.Single);
    }
}
