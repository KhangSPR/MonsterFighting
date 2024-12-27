using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Dialog/Object")]
public class DialogObject : ScriptableObject {

    public new string name;
    public int id;
    public int index;

    public UnityEvent<int> onDialog;
    public UnityEvent onCompleted;
    public DialogActorDatabase actorDB;
    public Line[] lines;


    [System.Serializable]
    public class Line {
        [TextArea] public string content;
        public Actor leftActor;
        public Actor rightActor;
    }

    [System.Serializable]
    public class Actor {
        public string name;
        public string avatar;
        public bool speaker;
    }
}
