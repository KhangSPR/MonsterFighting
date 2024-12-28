using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTest : MonoBehaviour {
    private static AudioTest instance;
    public static AudioTest Instance { get => instance; }

    AudioSource source;

    private void Awake(){
        if (instance == null)
            instance = this;
        source = GetComponent<AudioSource>();
    }

    public static void Play(AudioClip clip){
        Instance?.source.PlayOneShot(clip);
    }
}
