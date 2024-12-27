using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialog/Actor/Database")]
public class DialogActorDatabase : ScriptableObject {
    [SerializeField] List<DialogActor> actors = new();
    Dictionary<string,DialogActor> actorsDict = new(); 
    
    private void Awake(){
        actors.ForEach((actor) => AddActor(actor.name,actor));
    }

    public DialogActor GetActor(string name){
        return actorsDict[name];
    }

    public void AddActor(string name, DialogActor actor){
        if (actorsDict.ContainsKey(name)) return;
        actorsDict.Add(name, actor);
    }
}
