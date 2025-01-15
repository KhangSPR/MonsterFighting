using UIGameDataMap;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemySO", menuName = "Object/Enemy Type")]
public class EnemyTypeSO : ScriptableObject
{
    public Rarity rarity;
    public TypeEnemy typeEnemy;
    public string enemyName; // Đổi 'name' để tránh xung đột với tên của Unity Object
    public string attackType;
    public string skillDes;
    public Sprite sprite;
}
