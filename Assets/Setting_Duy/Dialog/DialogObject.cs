using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Dialog/Object")]
public class DialogObject : ScriptableObject {

    public new string name;
    public int id;
    public int index;
    public int indexChooseAvatar;

    //public UnityEvent onCompleted;
    public DialogLine[] lines;


    [System.Serializable]
    public class DialogLine {
        [TextArea] public string content;
        public DialogActor leftActor;
        public DialogActor rightActor;
        public Speaker speaker;
        public UnityEvent onBeforeDialog;
        public UnityEvent onAfterDialog;

        [System.Flags]
        public enum Speaker {
            None = 0,
            Left = 1 << 1,
            Right = 1 << 2,
            Both = ~None
        }
    }

    [System.Serializable]
    public class DialogActor {
        public string name;
        public Sprite avatar;
    }
}
