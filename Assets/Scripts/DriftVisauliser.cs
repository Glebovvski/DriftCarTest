using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DriftVisauliser : MonoBehaviour
{
    [SerializeField] private TMP_Text driftCountText;
    
    public void UpdateDriftCounter(int value)
    {
        driftCountText.text = value.ToString();
    }
}
