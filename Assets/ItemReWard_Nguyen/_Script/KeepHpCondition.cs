using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Stars Condition/Keep HP")]
public class KeepHpCondition : StarsCondition
{
    
    public int currentHpValue;
    public override float CheckThreshold()
    {
        float result = -1;
        if (currentHpValue >= threshold3) result =  3;
        if (currentHpValue < threshold3) result = 2;
        if (currentHpValue < threshold2) result = 1;
        if (currentHpValue < threshold1) result = 0;
        if (result == -1) Debug.LogError("Kết quả lỗi : không trả về giá trị số sao nằm trong phạm vi [0,3]");
        else Debug.Log($"HP khi hoàn thành game : {currentHpValue} , số lượng sao đạt được : {result}");
        return result;
    }
}
