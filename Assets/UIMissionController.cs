using System.Collections;
using System.Collections.Generic;
using TMPro;
using UIGameDataManager;
using UIGameDataMap;
using UnityEditor.AdaptivePerformance.Editor;
using UnityEngine;
using UnityEngine.UI;

public class UIMissionController : MonoBehaviour
{
    // Logic 
    [ReadOnlyInspector, SerializeField] private MapSO currentMapSO; // Start is called before the first frame update
    public float minSliderValue;
    public float maxSliderValue;
    public bool allowSetDefaultValue;
    // UI
    public Slider slider;
    public TextMeshProUGUI MissionDescrition;
    public Transform Threshold_Min;
    public Transform Threshold_1_Star;
    public Transform Threshold_2_Star;
    public Transform Threshold_3_Star;
    private void Awake()
    {
        currentMapSO = GameDataManager.Instance.currentMapSO;
        if(allowSetDefaultValue) currentMapSO.GetStarsCondition(currentMapSO.difficult).SetDefaultValue();
        MissionDescrition.text = currentMapSO.GetStarsCondition(currentMapSO.difficult).conditionDescription;
        minSliderValue = 0;
        maxSliderValue = currentMapSO.GetStarsCondition(currentMapSO.difficult).threshold3;

        var VectorThreshold03 = Threshold_3_Star.position - Threshold_Min.position;

        var ratioDistance1 = currentMapSO.GetStarsCondition(currentMapSO.difficult).threshold1 / maxSliderValue;
        var ratioDistance2 = currentMapSO.GetStarsCondition(currentMapSO.difficult).threshold2 / maxSliderValue;
        Threshold_1_Star.position = new Vector3(Threshold_Min.position.x+(Threshold_3_Star.position.x - Threshold_Min.position.x)* ratioDistance1, Threshold_1_Star.position.y, Threshold_1_Star.position.z);
        Threshold_2_Star.position = new Vector3(Threshold_Min.position.x+(Threshold_3_Star.position.x - Threshold_Min.position.x) * ratioDistance2, Threshold_2_Star.position.y, Threshold_2_Star.position.z);

        Threshold_1_Star.Find("ValueTxt").GetComponent<TextMeshProUGUI>().text = currentMapSO.GetStarsCondition(currentMapSO.difficult).threshold1.ToString();
        Threshold_2_Star.Find("ValueTxt").GetComponent<TextMeshProUGUI>().text = currentMapSO.GetStarsCondition(currentMapSO.difficult).threshold2.ToString();
        Threshold_3_Star.Find("ValueTxt").GetComponent<TextMeshProUGUI>().text = currentMapSO.GetStarsCondition(currentMapSO.difficult).threshold3.ToString();
        slider.minValue = minSliderValue;
        slider.maxValue = maxSliderValue;
    }
    public void Update()
    {
        //currentMapSO.GetStarsCondition(currentMapSO.difficult).SetDefaultValue();
        if(slider.value != currentMapSO.GetStarsCondition(currentMapSO.difficult).currentThreshold)
        {
            slider.value += -(slider.value - currentMapSO.GetStarsCondition(currentMapSO.difficult).currentThreshold) * 0.01f;
        }
        //slider.value = currentMapSO.GetStarsCondition(currentMapSO.difficult).currentThreshold;
    }
    public void SetThresholdUIPosition(Transform source , Transform target , Transform current)
    {

    }
}
