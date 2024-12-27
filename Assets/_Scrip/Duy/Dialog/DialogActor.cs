using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialog/Actor/Object")]
public class DialogActor : ScriptableObject {
    public new string name;
    public string desc;
    public List<Avatar> avatars = new();

    [System.Serializable]
    public class Avatar {
        public string name;
        public Sprite sprite;
    }
}
