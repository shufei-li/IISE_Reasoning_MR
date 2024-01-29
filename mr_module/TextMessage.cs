using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextMessage : MonoBehaviour
{
    public static TextMessage Instance { get; private set; }
    public TextMeshProUGUI distance;
    void Awake()
    {
        Instance = this;
    }
    public void ChangeTextMessage(string textMessage)
    {
        distance.text = textMessage;
    }
}
