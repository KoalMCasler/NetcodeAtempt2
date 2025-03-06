using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SharedHUD : MonoBehaviour
{
    public TextMeshProUGUI timeText;

    public void UpdateScore(float score)
    {
        timeText.text = string.Format("Time: {0:0.00}",score);
    }

}
