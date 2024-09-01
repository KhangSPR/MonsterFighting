using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepHpCondition : StarsCondition
{
    
    public uint currentHpValue;
    public override uint CheckThreshold()
    {
        uint result = 0;
        if (currentHpValue >= threshold3) result =  3;
        if (currentHpValue < threshold3) result = 2;
        if (currentHpValue < threshold2) result = 1;
        if (currentHpValue < threshold1) result = 0;
        if (result < 0) Debug.LogError("Kết quả lỗi : không trả về giá trị số sao nằm trong phạm vi [0,3]");
        else Debug.Log($"HP khi hoàn thành game : {currentHpValue} , số lượng sao đạt được : {result}");
        return result;
    }
}
