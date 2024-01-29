using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
public class ShowVelocity : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro velocityValue = null;
    public void OnSliderUpdated(SliderEventData eventData)
    {
        if (velocityValue == null)
        {
            velocityValue = GetComponent<TextMeshPro>();
        }

        if (velocityValue != null)
        {
            velocityValue.text = $"{eventData.NewValue:F2}";
        }
    }
}
