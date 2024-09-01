
using System;

[Serializable]
public abstract class ILevelCondition
{
    protected abstract bool IsConditionMet(); // Triển khai cụ thể sẽ định nghĩa cách kiểm tra
}