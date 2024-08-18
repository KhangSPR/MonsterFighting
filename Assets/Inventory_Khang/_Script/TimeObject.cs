using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeObject : MonoBehaviour
{
    [SerializeField] ImageRefresh imageRefresh;
    public ImageRefresh ImageRefresh { get { return imageRefresh; } set { imageRefresh = value; } }

    [SerializeField] TMP_Text m_Time; // Reference to the time Text component
    float time;
    public float _Time { get { return time; } set { time = value; } }

    private void Update()
    {
        if (imageRefresh.isCoolingDown && time > 0)
        {
            UpdateTimeText();
        }
    }

    public void UpdateTimeText()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            time = 0;
            m_Time.text = "";
        }
        else
        {
            m_Time.text = "" + (int)time;
        }
    }
}
